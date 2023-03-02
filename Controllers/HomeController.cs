using GPSsite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using GPSsite.Data;

namespace GPSsite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; 
        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var addresses_list = _context.Addresses.Where(x => x.UserId == GetUserId()).OrderBy(x => x.Type).ToList();
            return View(addresses_list);
        }

        public IActionResult SharedAddresses()
        {
            var public_addresses_list = _context.Addresses.Where(x => x.IsPublic).OrderByDescending(x => x.CreationDate).ToList();
            return View(public_addresses_list);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public int? InsertAddress([FromBody] Address myAddress)
        {
            myAddress.UserId = GetUserId();
            if(myAddress.UserId != "0")
            {
                myAddress.CreationDate = DateTime.Now;
                _context.Addresses.Add(myAddress);
                _context.SaveChanges();
                return myAddress.Id;
            }
            return null;
        }

        [HttpPost]
        public bool DeleteAddress([FromBody] int id)
        {
            var toDel = _context.Addresses.FirstOrDefault(x => x.Id == id);
            if (toDel == null) return false;
            _context.Addresses.Remove(toDel);
            _context.SaveChanges();
            return true;
        }

        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var userIdClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                return userIdClaim.Value;
            }
            return null;
        }

    }
}
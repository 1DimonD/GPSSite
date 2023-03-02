using System.ComponentModel.DataAnnotations;

namespace GPSsite.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? CreationDate { get; set; } = null;
        public string UserId { get; set; }
        public bool IsPublic { get; set; }
    }
}

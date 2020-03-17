using System.ComponentModel.DataAnnotations;

namespace Taxi.Common.Models
{
    public class TripDetailRequest
    {
        [Required]
        public int TripId { get; set; }

        [MaxLength(500, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres.")]
        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
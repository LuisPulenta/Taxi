using System.ComponentModel.DataAnnotations;

namespace Taxi.Common.Models
{
    public class CompleteTripRequest
    {
        [Required]
        public int TripId { get; set; }

        [MaxLength(500, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres.")]
        public string Target { get; set; }

        public double TargetLatitude { get; set; }

        public double TargetLongitude { get; set; }

        public float Qualification { get; set; }

        public string Remarks { get; set; }
    }
}
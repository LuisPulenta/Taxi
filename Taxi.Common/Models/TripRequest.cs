using System;
using System.ComponentModel.DataAnnotations;

namespace Taxi.Common.Models
{
    public class TripRequest
    {
        [RegularExpression(@"^([A-Za-z]{3}\d{3})$", ErrorMessage = "El campo {0} debe tener 3 caracteres y 3 números.")]
        [Required(ErrorMessage = "El campo{0} es obligatorio.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres.")]
        public string Plaque { get; set; }

        [Required(ErrorMessage = "El campo{0} es obligatorio.")]
        public Guid UserId { get; set; }

        public string Address { get; set; } 

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
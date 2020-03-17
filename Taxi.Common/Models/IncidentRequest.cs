using System.ComponentModel.DataAnnotations;

namespace Taxi.Common.Models
{
    public class IncidentRequest : TripRequest
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Remarks { get; set; }
    }
}
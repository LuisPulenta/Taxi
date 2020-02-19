using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Taxi.Web.Data.Entities
{
    public class TaxiEntity
    {
        public int Id { get; set; }

        [Display (Name ="Patente")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener {1} caracteres.")]
        [RegularExpression(@"^([A-Za-z]{3}\d{3})$", ErrorMessage = "El campo {0} debe iniciar con 3 caracteres y terminar con 3 números.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Plaque { get; set; }

        public ICollection<TripEntity> Trips { get; set; }

        public UserEntity User { get; set; }
    }
}
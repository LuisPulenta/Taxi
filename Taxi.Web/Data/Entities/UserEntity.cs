using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Taxi.Common.Enums;

namespace Taxi.Web.Data.Entities
{
    public class UserEntity : IdentityUser
    {
        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellido")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [Display(Name = "Domicilio")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public string Address { get; set; }

        [Display(Name = "Foto")]
        public string PicturePath { get; set; }

        [Display(Name = "Tipo Usuario")]
        public UserType UserType { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Nombre Completo y Documento")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";

        public ICollection<TaxiEntity> Taxis { get; set; }

        public ICollection<TripEntity> Trips { get; set; }

    }
}
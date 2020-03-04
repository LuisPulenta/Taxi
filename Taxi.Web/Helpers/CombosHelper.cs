using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Taxi.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        public IEnumerable<SelectListItem> GetComboRoles()
        {
            List<SelectListItem> list = new List<SelectListItem>
                {
                new SelectListItem { Value = "0", Text = "[Seleccione un Rol...]" },
                new SelectListItem { Value = "1", Text = "Conductor" },
                new SelectListItem { Value = "2", Text = "Usuario" }
                };
            return list;
        }
    }
}
using System.Collections.Generic;
using Taxi.Common.Models;

namespace Taxi.Prism.Helpers
{
    public static class CombosHelper
    {
        public static List<Role> GetRoles()
        {
            return new List<Role>
            {
                new Role { Id = 1, Name = "Usuario" },
                new Role { Id = 2, Name = "Conductor" }
            };
        }


        public static List<Comment> GetComments()
        {
            return new List<Comment>
        {
            new Comment { Id = 1, Name = "Muy buen servicio"},
            new Comment { Id = 2, Name = "Conductor muy amigable"},
            new Comment { Id = 2, Name = "Taxi limpio"},
            new Comment { Id = 2, Name = "Mal conductor"},
            new Comment { Id = 2, Name = "Cobro mayor a lo esperado"},
            new Comment { Id = 2, Name = "Taxi sucio o en mal estado"}
        };
        }
    }
}
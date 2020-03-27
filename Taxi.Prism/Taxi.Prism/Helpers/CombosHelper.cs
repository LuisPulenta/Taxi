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
    }
}
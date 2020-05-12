using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenPlannerApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Garden> Gardens { get; } = new List<Garden>();

        public List<TileType> TileTypes { get; } = new List<TileType>();
    }
}

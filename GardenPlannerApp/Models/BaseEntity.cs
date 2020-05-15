using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GardenPlannerApp.Models
{
    public class BaseEntity
    {

        [Required]
        public bool Public { get; set; } = false;

        [Required]
        public ApplicationUser Owner { get; set; }
    }
}

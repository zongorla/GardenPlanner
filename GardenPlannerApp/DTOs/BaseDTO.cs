using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenPlannerApp.DTOs
{
    public class BaseDTO
    {

        public bool Public { get; set; } = false;

        public bool ReadOnly { get; set; }

        public UserDTO Owner { get; set; }
    }
}

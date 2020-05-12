using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GardenPlannerApp.Controllers.DTOs
{
    public class NewGardenDTO
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public int Width { get; set; }

        [Required]
        public int  Height { get; set; }
    }
}

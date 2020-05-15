using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GardenPlannerApp.DTOs
{
    public class TileTypeDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public bool Public { get; set; }

    }
}

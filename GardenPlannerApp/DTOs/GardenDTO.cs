using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GardenPlannerApp.DTOs
{
    public class GardenDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<GardenTileDTO> Tiles { get; set; } = new List<GardenTileDTO>();

        public int Width { get; set; }

        public int Height { get; set; }
    }
}

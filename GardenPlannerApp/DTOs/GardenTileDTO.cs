using GardenPlannerApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GardenPlannerApp.DTOs
{
    public class GardenTileDTO: BaseDTO
    {
        public string Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public TileTypeDTO TileType { get; set; }

    }
}

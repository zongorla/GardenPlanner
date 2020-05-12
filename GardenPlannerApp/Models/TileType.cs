﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GardenPlannerApp.Models
{
    public class TileType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool Public { get; set; }

        public ApplicationUser Creator { get; set; }

        public List<GardenTile> Tiles { get; } = new List<GardenTile>();

    }
}

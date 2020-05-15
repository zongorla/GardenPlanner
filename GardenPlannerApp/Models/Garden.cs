using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GardenPlannerApp.Models
{
    public class Garden: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<GardenTile> Tiles { get; } = new List<GardenTile>();

        public int Width { get; set; }

        public int  Height { get; set; }
    }
}

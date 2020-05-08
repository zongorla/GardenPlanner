using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GardenPlanner.Entities
{
    public class Garden
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public string Name { get; set; }

        public List<GardenTile> Tiles { get; } = new List<GardenTile>();
    }
}

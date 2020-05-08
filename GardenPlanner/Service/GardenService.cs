using GardenPlanner.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenPlanner.Service
{
    public class GardenService
    {
        private readonly GardenPlannerContext context;

        public GardenService(GardenPlannerContext context)
        {
            this.context = context;
        }


        public void Create(string userId, string Name)
        {
            var newGarden = new Garden
            {
                Name = Name,
                User = this.context.Users.Find(userId)
            };
            this.context.Gardens.Add(newGarden);
        }

        public Garden GetGarden(string gardenId, string userId)
        {
            return this.context.Gardens.Find(gardenId);
        }

        public void AddTile(string gardenId, string tileTypeId, int posX, int posY)
        {
            var garden = this.context.Gardens.Find(gardenId);
            garden.Tiles.Add(new GardenTile()
            {
                TileType = this.context.TileTypes.Find(tileTypeId),
                Garden = garden,
                X = posX,
                Y = posY
            });
        }

    }
}

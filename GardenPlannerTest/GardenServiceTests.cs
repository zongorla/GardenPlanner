using GardenPlanner;
using GardenPlanner.Entities;
using GardenPlanner.Service;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace GardenPlannerTest
{
    public class GardenServiceTest
    {


        [Fact]
        public void createGardenTest()
        {
            var options = getInMemoryDbOptions("createGardenTest");
            var userId = "id";

            using (var context = new GardenPlannerContext(options))
            {
                context.Users.Add(new User
                {
                    Name = "First",
                    Id = userId
                });
                context.SaveChanges();
            }


            using (var context = new GardenPlannerContext(options))
            {

                var service = new GardenService(context);
                service.Create(userId, "First garden");
                context.SaveChanges();
            }

            using (var context = new GardenPlannerContext(options))
            {
                Assert.Equal(1, context.Gardens.Count());
                Assert.Equal("First garden", context.Gardens.Single().Name);

                Assert.Equal(1, context.Users.Find(userId)?.Gardens?.Count());
                Assert.Equal("First garden", context.Gardens.Single().Name);
            }
        }



        [Fact]
        public void addTileTest()
        {
            var options = getInMemoryDbOptions("addTileTest");
            var gardenId = "gardenId";
            var tileTypeId = "tileTypeId";
            var userId = "id";
            using (var context = new GardenPlannerContext(options))
            {

                context.Users.Add(new User
                {
                    Name = "First",
                    Id = userId
                });
                context.SaveChanges();

                context.Gardens.Add(new Garden
                {
                    Name = "Garden",
                    Id = gardenId,
                    User = context.Users.Single()
                });

                context.TileTypes.Add(new TileType
                {
                    Id = tileTypeId,
                    Name = "Tomato",
                    Creator = context.Users.Single()
                });

                context.SaveChanges();
            }

            using (var context = new GardenPlannerContext(options))
            {
                var service = new GardenService(context);
                service.AddTile(gardenId, tileTypeId, 10, 12);
                context.SaveChanges();
            }

            using (var context = new GardenPlannerContext(options))
            {
                var tile = context.Gardens.Find(gardenId)?.Tiles.Single();
                Assert.Equal(tileTypeId, tile.TileType.Id );
                Assert.Equal(10, tile.X);
                Assert.Equal(12, tile.Y);
            }

        }



        private DbContextOptions<GardenPlannerContext> getInMemoryDbOptions(string name)
        {
            return new DbContextOptionsBuilder<GardenPlannerContext>()
                .UseInMemoryDatabase(databaseName: name)
                .Options;
        }

    }
}
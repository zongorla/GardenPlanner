using GardenPlanner.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenPlanner
{
    public class GardenPlannerContext : DbContext
    {

        public GardenPlannerContext() : base()
        { }

        public GardenPlannerContext(DbContextOptions<GardenPlannerContext> options) : base(options)
        { }

        private ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.AddConsole());
            return serviceCollection.BuildServiceProvider()
                    .GetService<ILoggerFactory>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=xgef0q;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
                .UseLoggerFactory(GetLoggerFactory());
            }
        }

        public DbSet<Garden> Gardens { get; set; }

        public DbSet<GardenTile> GardenTiles { get; set; }

        public DbSet<TileType> TileTypes { get; set; }
                
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            createUserModel(modelBuilder);
            createGardenModel(modelBuilder);

            modelBuilder.Entity<TileType>()
                .HasMany(c => c.Tiles)
                .WithOne(t => t.TileType)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private void createUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Gardens)
                .WithOne(g => g.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.TileTypes)
                .WithOne(u => u.Creator)
                .OnDelete(DeleteBehavior.SetNull);

        }

        private void createGardenModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Garden>()
                .Property(g => g.Name)
                .HasMaxLength(15)
                .IsRequired();

            modelBuilder.Entity<Garden>()
                .HasMany(g => g.Tiles)
                .WithOne(t => t.Garden)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
using GardenPlannerApp.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenPlannerApp.Data
{
    public class GardenPlannerAppDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public GardenPlannerAppDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Garden> Gardens { get; set; }

        public DbSet<GardenTile> GardenTiles { get; set; }

        public DbSet<TileType> TileTypes { get; set; }


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
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Gardens)
                .WithOne(g => g.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
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

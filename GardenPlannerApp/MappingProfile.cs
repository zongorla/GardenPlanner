using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GardenPlannerApp.Models;
using GardenPlannerApp.DTOs;

namespace GardenPlannerApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Garden, GardenDTO>();
            CreateMap<GardenDTO, Garden>();
            CreateMap<GardenTile, GardenTileDTO>();
            CreateMap<GardenTileDTO, GardenTile>();
            CreateMap<TileType, TileTypeDTO>();
            CreateMap<TileTypeDTO, TileType>();
            CreateMap<UserDTO, ApplicationUser>();
            CreateMap<ApplicationUser, UserDTO>();
        }
    }
}

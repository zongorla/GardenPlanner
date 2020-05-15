using AutoMapper;
using GardenPlannerApp.Data;
using GardenPlannerApp.DTOs;
using GardenPlannerApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GardenPlannerApp.Controllers
{
    public class GardenPlannerAppControllerBase : ControllerBase
    {

        protected readonly GardenPlannerAppDbContext _context;
        protected readonly IMapper _mapper;

        public GardenPlannerAppControllerBase(GardenPlannerAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private string userId = null;
        protected string UserId {
            get
            {
                if(userId == null)
                {
                    userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                return userId;
            }
        }

        protected bool Owned(BaseEntity entity)
        {
            return entity.Owner.Id == UserId;
        }

        protected void OwnIt(BaseEntity entity)
        {
            entity.Owner = _context.Users.Find(UserId);
        }

        protected T SetReadonly<T>(T dto) where T: BaseDTO
        {
            dto.ReadOnly = dto.Owner.Id != UserId;
            return dto;
        }

        protected List<T> SetReadonly<T>(List<T> dtos) where T: BaseDTO
        {
            return  dtos.Select(x => this.SetReadonly(x)).ToList();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GardenPlannerApp.Data;
using GardenPlannerApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using GardenPlannerApp.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GardenPlannerApp.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GardensController : ControllerBase
    {
        private readonly GardenPlannerAppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public GardensController(GardenPlannerAppDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: api/Gardens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GardenDTO>>> GetGardens()
        {
            return _mapper.Map<List<GardenDTO>>(await _context.Gardens.ToListAsync());
        }

        // GET: api/Gardens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GardenDTO>> GetGarden(string id)
        {
            var garden = await _context.Gardens.Where(x => x.Id == id)
                .Include(x => x.Tiles)
                .Include("Tiles.TileType")
                .FirstOrDefaultAsync();
            if (garden == null)
            {
                return NotFound();
            }
            var gardenDto = _mapper.Map<GardenDTO>(garden);
            return gardenDto;
        }

        // PUT: api/Gardens/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGarden(string id, GardenDTO gardenDTO)
        {
            Garden garden = _mapper.Map<Garden>(gardenDTO);
            if (id != garden.Id)
            {
                return BadRequest();
            }

            _context.Entry(garden).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GardenExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Gardens
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GardenDTO>> PostGarden(GardenDTO newGarden)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            ApplicationUser applicationUser = await _context.Users.FindAsync(userId);
            var garden = _mapper.Map<Garden>(newGarden);
            garden.User = applicationUser;

            _context.Gardens.Add(garden);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetGarden", new { id = garden.Id }, _mapper.Map<GardenDTO>(garden));
        }

        // DELETE: api/Gardens/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GardenDTO>> DeleteGarden(string id)
        {
            var garden = await _context.Gardens.FindAsync(id);
            if (garden == null)
            {
                return NotFound();
            }

            _context.Gardens.Remove(garden);
            await _context.SaveChangesAsync();

            return _mapper.Map<GardenDTO>(garden);
        }

        private bool GardenExists(string id)
        {
            return _context.Gardens.Any(e => e.Id == id);
        }
    }
}

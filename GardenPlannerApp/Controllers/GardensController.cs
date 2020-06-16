using System;
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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace GardenPlannerApp.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GardensController : GardenPlannerAppControllerBase
    {

        public GardensController(GardenPlannerAppDbContext context, IMapper mapper, ILogger<GardensController> logger)
            : base(context, mapper, logger) { }

        // GET: api/Gardens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GardenDTO>>> GetGardens()
        {
            return SetReadonly(_mapper.Map<List<GardenDTO>>(await _context.Gardens.OwnedOrPublic(UserId).ToListAsync()));
        }

        // GET: api/Gardens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GardenDTO>> GetGarden(string id)
        {
            var garden = await _context.Gardens.OwnedOrPublic(UserId).Where(x => x.Id == id)
                .Include(x => x.Tiles)
                .Include("Tiles.TileType")
                .FirstOrDefaultAsync();
            if (garden == null)
            {
                return NotFound();
            }
            var gardenDto = _mapper.Map<GardenDTO>(garden);

            return SetReadonly(gardenDto);
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
            if (!Owned(garden))
            {
                return Unauthorized();
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
            var garden = _mapper.Map<Garden>(newGarden);

            OwnIt(garden);

            _context.Gardens.Add(garden);

            await _context.SaveChangesAsync();
            return CreatedAtAction("GetGarden", new { id = garden.Id }, SetReadonly(_mapper.Map<GardenDTO>(garden)));
        }


        [HttpPost("share")]
        public async Task<ActionResult<GardenDTO>> PostShareGarden(GardenDTO gardenDTO)
        {
            var garden = await _context.Gardens.OwnedOrPublic(UserId).Where(x => x.Id == gardenDTO.Id)
                 .Include(x => x.Tiles)
                 .Include("Tiles.TileType")
                 .FirstOrDefaultAsync();
            if (garden == null)
            {
                return BadRequest();
            }
            if (!Owned(garden))
            {
                return Unauthorized();
            }

            garden.Public = gardenDTO.Public;
            garden.Tiles.ForEach(x => x.Public = gardenDTO.Public);
            garden.Tiles.ForEach(x => x.TileType.Public = gardenDTO.Public);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGarden", new { id = garden.Id }, SetReadonly(_mapper.Map<GardenDTO>(garden)));
        }


        // DELETE: api/Gardens/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GardenDTO>> DeleteGarden(string id)
        {
            var garden = await _context.Gardens.Include(x => x.Owner).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (!Owned(garden))
            {
                return Unauthorized();
            }

            if (garden == null)
            {
                return NotFound();
            }

            _context.Gardens.Remove(garden);
            await _context.SaveChangesAsync();

            return SetReadonly(_mapper.Map<GardenDTO>(garden));
        }




        private bool GardenExists(string id)
        {
            return _context.Gardens.OwnedOrPublic(UserId).Any(e => e.Id == id);
        }
    }
}

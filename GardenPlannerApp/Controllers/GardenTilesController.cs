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
using AutoMapper;
using GardenPlannerApp.DTOs;

namespace GardenPlannerApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GardenTilesController : ControllerBase
    {
        private readonly GardenPlannerAppDbContext _context;
        private readonly IMapper _mapper;

        public GardenTilesController(GardenPlannerAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/GardenTiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GardenTileDTO>>> GetGardenTiles()
        {
            return _mapper.Map<List<GardenTileDTO>>(await _context.GardenTiles.ToListAsync());
        }

        // GET: api/GardenTiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GardenTileDTO>> GetGardenTile(string id)
        {
            var gardenTile = await _context.GardenTiles.FindAsync(id);

            if (gardenTile == null)
            {
                return NotFound();
            }

            return _mapper.Map<GardenTileDTO>(gardenTile);
        }

        // PUT: api/GardenTiles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGardenTile(string id, GardenTileDTO gardenTileDTO)
        {
            GardenTile gardenTile = _mapper.Map<GardenTile>(gardenTileDTO);
            if (id != gardenTile.Id)
            {
                return BadRequest();
            }

            _context.Entry(gardenTile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GardenTileExists(id))
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

        // POST: api/GardenTiles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GardenTileDTO>> PostGardenTile(NewGardenTileDTO newGardenTile)
        {
            var gardenTile = new GardenTile
            {
                X = newGardenTile.X,
                Y = newGardenTile.Y,
                Garden = await _context.Gardens.FindAsync(newGardenTile.GardenId),
                TileType = await _context.TileTypes.FindAsync(newGardenTile.TileTypeId)
            };
            _context.GardenTiles.Add(gardenTile);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetGardenTile", new { id = gardenTile.Id }, _mapper.Map<GardenTileDTO>(gardenTile));
        }

        // DELETE: api/GardenTiles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GardenTileDTO>> DeleteGardenTile(string id)
        {
            var gardenTile = await _context.GardenTiles.FindAsync(id);
            if (gardenTile == null)
            {
                return NotFound();
            }

            _context.GardenTiles.Remove(gardenTile);
            await _context.SaveChangesAsync();

            return _mapper.Map<GardenTileDTO>(gardenTile);
        }

        private bool GardenTileExists(string id)
        {
            return _context.GardenTiles.Any(e => e.Id == id);
        }
    }
}

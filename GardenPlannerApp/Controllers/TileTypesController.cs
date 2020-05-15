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
using GardenPlannerApp.DTOs;
using AutoMapper;

namespace GardenPlannerApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TileTypesController : GardenPlannerAppControllerBase
    {
        public TileTypesController(GardenPlannerAppDbContext context, IMapper mapper)
            : base(context, mapper) { }

        // GET: api/TileTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TileTypeDTO>>> GetTileTypes()
        {
            return SetReadonly(_mapper.Map<List<TileTypeDTO>>(await _context.TileTypes.OwnedOrPublic(UserId).ToListAsync()));
        }

        // GET: api/TileTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TileTypeDTO>> GetTileType(string id)
        {
            var tileType = await _context.TileTypes.OwnedOrPublic(UserId).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (tileType == null)
            {
                return NotFound();
            }

            return SetReadonly(_mapper.Map<TileTypeDTO>(tileType));
        }

        // PUT: api/TileTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTileType(string id, TileTypeDTO tileTypeDto)
        {
            TileType tileType = _mapper.Map<TileType>(tileTypeDto);
            if (id != tileType.Id)
            {
                return BadRequest();
            }

            _context.Entry(tileType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TileTypeExists(id))
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

        // POST: api/TileTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TileTypeDTO>> PostTileType(TileTypeDTO tileTypeDto)
        {
            TileType tileType = _mapper.Map<TileType>(tileTypeDto);

            OwnIt(tileType);

            _context.TileTypes.Add(tileType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTileType", new { id = tileType.Id }, SetReadonly(_mapper.Map<TileTypeDTO>(tileType)));
        }

        // DELETE: api/TileTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TileTypeDTO>> DeleteTileType(string id)
        {
            var tileType = await _context.TileTypes.Include(x => x.Owner).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (tileType == null)
            {
                return NotFound();
            }
            if (!Owned(tileType))
            {
                return Unauthorized();
            }
            _context.TileTypes.Remove(tileType);
            await _context.SaveChangesAsync();

            return SetReadonly(_mapper.Map<TileTypeDTO>(tileType));
        }

        private bool TileTypeExists(string id)
        {
            return _context.TileTypes.OwnedOrPublic(UserId).Any(e => e.Id == id);
        }
    }
}

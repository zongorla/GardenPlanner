using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GardenPlannerApp.Data;
using GardenPlannerApp.Models;
using GardenPlannerApp.Controllers.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;

namespace GardenPlannerApp.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GardensController : ControllerBase
    {
        private readonly GardenPlannerAppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GardensController(GardenPlannerAppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Gardens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Garden>>> GetGardens()
        {
            return await _context.Gardens.ToListAsync();
        }

        // GET: api/Gardens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Garden>> GetGarden(string id)
        {
            var garden = await _context.Gardens.FindAsync(id);

            if (garden == null)
            {
                return NotFound();
            }

            return garden;
        }

        // PUT: api/Gardens/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGarden(string id, Garden garden)
        {
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
        public async Task<ActionResult<Garden>> PostGarden(NewGardenDTO newGarden)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            var userName = User.FindFirstValue(ClaimTypes.Name);

            ApplicationUser applicationUser = await _context.Users.FindAsync(userId);
            var garden = new Garden
            {
                Name = newGarden.Name,
                Width = newGarden.Width,
                Height = newGarden.Height,
                User = applicationUser
            };
            _context.Gardens.Add(garden);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetGarden", new { id = garden.Id });
        }

        // DELETE: api/Gardens/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Garden>> DeleteGarden(string id)
        {
            var garden = await _context.Gardens.FindAsync(id);
            if (garden == null)
            {
                return NotFound();
            }

            _context.Gardens.Remove(garden);
            await _context.SaveChangesAsync();

            return garden;
        }

        private bool GardenExists(string id)
        {
            return _context.Gardens.Any(e => e.Id == id);
        }
    }
}

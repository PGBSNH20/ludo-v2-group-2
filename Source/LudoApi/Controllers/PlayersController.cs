using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LudoEngine.Database;

namespace LudoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly LudoContext _context;

        public PlayersController(LudoContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DbPlayer>>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DbPlayer>> GetDbPlayer(int id)
        {
            var dbPlayer = await _context.Players.FindAsync(id);

            if (dbPlayer == null)
            {
                return NotFound();
            }

            return dbPlayer;
        }

        // PUT: api/Players/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDbPlayer(int id, DbPlayer dbPlayer)
        {
            if (id != dbPlayer.Id)
            {
                return BadRequest();
            }

            _context.Entry(dbPlayer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DbPlayerExists(id))
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

        // POST: api/Players
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DbPlayer>> PostDbPlayer(DbPlayer dbPlayer)
        {
            _context.Players.Add(dbPlayer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDbPlayer", new { id = dbPlayer.Id }, dbPlayer);
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDbPlayer(int id)
        {
            var dbPlayer = await _context.Players.FindAsync(id);
            if (dbPlayer == null)
            {
                return NotFound();
            }

            _context.Players.Remove(dbPlayer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DbPlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}

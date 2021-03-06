using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LudoApi.Database;
using Microsoft.AspNetCore.Authorization;
using LudoApi.DTOs;

namespace LudoApi.Controllers
{
    //[Authorize]
    [Route("api/winners")]
    [ApiController]
    public class WinnersController : ControllerBase
    {
        private readonly LudoContext _context;

        public WinnersController(LudoContext context)
        {
            _context = context;
        }

        // GET: api/Winners
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WinnerDTO>>> GetWinners()
        {
           
            return await _context.Winners.Select(winners => DbWinnersToDTO(winners)).ToListAsync();
        }

        // GET: api/Winners/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WinnerDTO>> GetDbWinner(int id)
        {
            var dbWinner = await _context.Winners.FindAsync(id);

            if (dbWinner == null)
            {
                return NotFound();
            }

            return DbWinnersToDTO(dbWinner);
        }

        [HttpGet("/boards/{id}")]
        public async Task<ActionResult<IEnumerable<WinnerDTO>>> GetWinnerBoard(int id)
        {
            
            return  await _context.Winners
                .Where(x => x.BoardId == id)
                .Select(player => DbWinnersToDTO(player))
                .ToListAsync(); 
        }

        // PUT: api/Winners/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDbWinner(int id, DbWinner dbWinner)
        {
            if (id != dbWinner.Id)
            {
                return BadRequest();
            }

            _context.Entry(dbWinner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DbWinnerExists(id))
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

        // POST: api/Winners
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DbWinner>> PostDbWinner(DbWinner dbWinner)
        {
            _context.Winners.Add(dbWinner);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDbWinner", new { id = dbWinner.Id }, dbWinner);
        }

        // DELETE: api/Winners/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDbWinner(int id)
        {
            var dbWinner = await _context.Winners.FindAsync(id);
            if (dbWinner == null)
            {
                return NotFound();
            }

            _context.Winners.Remove(dbWinner);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DbWinnerExists(int id)
        {
            return _context.Winners.Any(e => e.Id == id);
        }

        private static WinnerDTO DbWinnersToDTO(DbWinner winner) =>
        new()
        {
            PlayerId = winner.PlayerId,
            Placement = winner.Placement
        };
    }
}

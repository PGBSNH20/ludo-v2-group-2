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
    public class BoardStatesController : ControllerBase
    {
        private readonly LudoContext _context;

        public BoardStatesController(LudoContext context)
        {
            _context = context;
        }

        // GET: api/BoardStates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DbBoardState>>> GetBoardStates()
        {
            return await _context.BoardStates.ToListAsync();
        }

        // GET: api/BoardStates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DbBoardState>> GetDbBoardState(int id)
        {
            var dbBoardState = await _context.BoardStates.FindAsync(id);

            if (dbBoardState == null)
            {
                return NotFound();
            }

            return dbBoardState;
        }

        // PUT: api/BoardStates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDbBoardState(int id, DbBoardState dbBoardState)
        {
            if (id != dbBoardState.Id)
            {
                return BadRequest();
            }

            _context.Entry(dbBoardState).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DbBoardStateExists(id))
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

        // POST: api/BoardStates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DbBoardState>> PostDbBoardState(DbBoardState dbBoardState)
        {
            _context.BoardStates.Add(dbBoardState);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDbBoardState", new { id = dbBoardState.Id }, dbBoardState);
        }

        // DELETE: api/BoardStates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDbBoardState(int id)
        {
            var dbBoardState = await _context.BoardStates.FindAsync(id);
            if (dbBoardState == null)
            {
                return NotFound();
            }

            _context.BoardStates.Remove(dbBoardState);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DbBoardStateExists(int id)
        {
            return _context.BoardStates.Any(e => e.Id == id);
        }
    }
}

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
    public class BoardsController : ControllerBase
    {
        private readonly LudoContext _context;

        public BoardsController(LudoContext context)
        {
            _context = context;
        }

        // GET: api/Boards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DbBoard>>> GetBoards()
        {
            return await _context.Boards.ToListAsync();
        }

        // GET: api/Boards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DbBoard>> GetDbBoard(int id)
        {
            var dbBoard = await _context.Boards.FindAsync(id);

            if (dbBoard == null)
            {
                return NotFound();
            }

            return dbBoard;
        }

        // PUT: api/Boards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDbBoard(int id, DbBoard dbBoard)
        {
            if (id != dbBoard.Id)
            {
                return BadRequest();
            }

            _context.Entry(dbBoard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DbBoardExists(id))
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

        // POST: api/Boards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DbBoard>> PostDbBoard(DbBoard dbBoard)
        {
            _context.Boards.Add(dbBoard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDbBoard", new { id = dbBoard.Id }, dbBoard);
        }

        // DELETE: api/Boards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDbBoard(int id)
        {
            var dbBoard = await _context.Boards.FindAsync(id);
            if (dbBoard == null)
            {
                return NotFound();
            }

            _context.Boards.Remove(dbBoard);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DbBoardExists(int id)
        {
            return _context.Boards.Any(e => e.Id == id);
        }
    }
}

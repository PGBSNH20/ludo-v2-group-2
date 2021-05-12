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
    public class ColorsController : ControllerBase
    {
        private readonly LudoContext _context;

        public ColorsController(LudoContext context)
        {
            _context = context;
        }

        // GET: api/Colors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DbColor>>> GetColors()
        {
            return await _context.Colors.ToListAsync();
        }

        // GET: api/Colors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DbColor>> GetDbColor(int id)
        {
            var dbColor = await _context.Colors.FindAsync(id);

            if (dbColor == null)
            {
                return NotFound();
            }

            return dbColor;
        }

        // PUT: api/Colors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDbColor(int id, DbColor dbColor)
        {
            if (id != dbColor.Id)
            {
                return BadRequest();
            }

            _context.Entry(dbColor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DbColorExists(id))
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

        // POST: api/Colors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DbColor>> PostDbColor(DbColor dbColor)
        {
            _context.Colors.Add(dbColor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDbColor", new { id = dbColor.Id }, dbColor);
        }

        // DELETE: api/Colors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDbColor(int id)
        {
            var dbColor = await _context.Colors.FindAsync(id);
            if (dbColor == null)
            {
                return NotFound();
            }

            _context.Colors.Remove(dbColor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DbColorExists(int id)
        {
            return _context.Colors.Any(e => e.Id == id);
        }
    }
}

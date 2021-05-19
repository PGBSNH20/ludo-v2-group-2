using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LudoEngine.Database;
using LudoApi.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace LudoApi.Controllers
{
    [Authorize]
    [Route("api/colors")]
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
        public async Task<ActionResult<IEnumerable<ColorDTO>>> GetColors()
        {
            return await _context.Colors
                .Select(color => DbColorToDTO(color))
                .ToListAsync();
        }

        // GET: api/Colors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ColorDTO>> GetDbColor(int id)
        {
            var dbColor = await _context.Colors.FindAsync(id);

            if (dbColor == null)
            {
                return NotFound();
            }

            return DbColorToDTO(dbColor);
        }

        // Patch api/colors/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchColor(int id,
            [FromBody] JsonPatchDocument<DbColor> patchDoc)
        {
            if (patchDoc != null)
            {
                //var color = await DbQuery.GetColor(id);
                var color = await _context.Colors.FindAsync(id);

                if (color == null)
                {
                    return NotFound("Couldn't find any color with that Id!");
                }

                patchDoc.ApplyTo(color, ModelState);

                _context.SaveChanges();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return new ObjectResult(color);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }        

        // Todo: DbColor in the parameter, should that be done this way? check best practice
        // POST: api/Colors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DbColor>> PostDbColor(DbColor dbColor)
        {
            _context.Colors.Add(dbColor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDbColor", new { id = dbColor.Id }, dbColor);
        }

        // Todo: Admin control only
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

        private static ColorDTO DbColorToDTO(DbColor color) =>
        new ColorDTO
        {
            Id = color.Id,
            ColorCode = color.ColorCode
        };
    }
}

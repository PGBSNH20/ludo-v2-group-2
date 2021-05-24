using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LudoEngine.Database;
using LudoEngine.Engine;
using LudoApi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace LudoApi.Controllers
{
    //[Authorize]
    [Route("api/boards")]
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
        public async Task<ActionResult<IEnumerable<BoardDTO>>> GetBoards()
        {
            return await _context.Boards
                .Select(board => DbBoardToDTO(board))
                .ToListAsync();
        }

        // GET: api/Boards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDTO>> GetDbBoard(int id)
        {
            var dbBoard = await _context.Boards.FindAsync(id);

            if (dbBoard == null)
            {
                return NotFound("No board with that id exists!");
            }

            return DbBoardToDTO(dbBoard);
        }

        // GET: api/Boards/history
        [HttpGet("history")]
        public async Task<ActionResult<List<History>>> GetDbBoardsHistory()
        {
            var finishedBoards = await DbQuery.GetHistory(_context);

            if (finishedBoards == null)
            {
                return NotFound("There are no finished games!");
            }

            return finishedBoards;
        }

        // GET: api/Boards/unfinished
        [HttpGet("unfinished")]
        public async Task<ActionResult<List<BoardData>>> GetDbUnfinishedBoards()
        {
            var unfinishedBoards = await DbQuery.GetUnfinishedBoards(_context);

            if (unfinishedBoards == null)
            {
                return NotFound("There are no unfinished games!");
            }

            return unfinishedBoards;
        }

        // To create new boards (in new game menu)
        // POST: api/Boards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BoardDTO>> PostDbBoard(DbBoard dbBoard)
        {
            _context.Boards.Add(dbBoard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDbBoard", new { id = dbBoard.Id }, DbBoardToDTO(dbBoard));
        }

        // For admin
        // DELETE: api/Boards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDbBoard(int id)
        {
            var dbBoard = await _context.Boards.FindAsync(id);
            if (dbBoard == null)
            {
                return NotFound("Couldn't find the board with that id.");
            }

            _context.Boards.Remove(dbBoard);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DbBoardExists(int id)
        {
            return _context.Boards.Any(e => e.Id == id);
        }

        private static BoardDTO DbBoardToDTO(DbBoard board) => 
        new()
        {
            Id = board.Id,
            LastTimePlayed = board.LastTimePlayed,
            IsFinished = board.IsFinished       
        };
    }
}

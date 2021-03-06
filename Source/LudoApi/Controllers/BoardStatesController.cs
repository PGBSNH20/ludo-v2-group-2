using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LudoApi.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using LudoApi.Database;

namespace LudoApi.Controllers
{
    //[Authorize]
    [Route("api/boardStates")]
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
        public async Task<ActionResult<IEnumerable<BoardStateDTO>>> GetBoardStates()
        {
            return await _context.BoardStates
                .Select(boardstate => DbBoardStateToDTO(boardstate))
                .ToListAsync();
        }

        // GET: api/BoardStates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BoardStateDTO>> GetBoardState(int id)
        {
            var dbBoardState = await _context.BoardStates.FindAsync(id);

            if (dbBoardState == null)
            {
                return NotFound();
            }

            return DbBoardStateToDTO(dbBoardState);
        }

        // GET: api/BoardStates/Board/5
        [HttpGet("board/{boardId}")]
        public async Task<ActionResult<List<BoardStateDTO>>> GetAllBoardStatesByBoardId(int boardId)
        {
            List<BoardStateDTO> boardStates = await _context.BoardStates
                .Where(state => state.BoardId == boardId)
                .Select(state => DbBoardStateToDTO(state))
                .ToListAsync();

            if (boardStates == null)
            {
                return NotFound();
            }

            return boardStates;
        }

        // GET: api/BoardStates/5/10
        [HttpGet("board/{boardId}/{playerId}")]
        public async Task<ActionResult<List<BoardStateDTO>>> GetAllBoardStatesForPlayerId(int boardId, int playerId)
        {
            List<BoardStateDTO> boardStates = await _context.BoardStates
                .Where(b => b.BoardId == boardId && b.PlayerId == playerId)
                .Select(board => DbBoardStateToDTO(board))
                .ToListAsync();

            if (boardStates == null)
            {
                return NotFound();
            }

            return boardStates;
        }

        //// POST: api/BoardStates
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<DbBoardState>> PostDbBoardState(DbBoardState dbBoardState)
        //{
        //    _context.BoardStates.Add(dbBoardState);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetDbBoardState", new { id = dbBoardState.Id }, dbBoardState);
        //}

        // Patch api/boardStates/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchBoardState(int id,
            [FromBody] JsonPatchDocument<DbBoardState> patchDoc)
        {
            if (patchDoc != null)
            {
                //var color = await DbQuery.GetColor(id);
                var boardState = await _context.BoardStates.FindAsync(id);

                if (boardState == null)
                {
                    return NotFound("Couldn't find any board move with that Id!");
                }

                patchDoc.ApplyTo(boardState, ModelState);

                _context.SaveChanges();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return new ObjectResult(boardState);
            }
            else
            {
                return BadRequest(ModelState);
            }
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

        private static BoardStateDTO DbBoardStateToDTO(DbBoardState boardState) =>
        new BoardStateDTO
        {
            Id = boardState.Id,
            PlayerId  = boardState.PlayerId,
            BoardId = boardState.BoardId,
            PieceNumber = boardState.PieceNumber,
            PiecePosition = boardState.PiecePosition,
            IsInSafeZone = boardState.IsInSafeZone,
            IsInBase = boardState.IsInBase
    };
}
}

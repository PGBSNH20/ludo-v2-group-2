using LudoApi.DTOs;
using LudoEngine.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoApi.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly LudoContext _context;
        private readonly int _boardSize = 52;
        private readonly int _maxPlayers = 4;
        private readonly int _safeZoneSize = 6;

        public GameController(LudoContext context)
        {
            _context = context;
        }

        // ToDo: remove piece when at goal

        [HttpGet("boards/{boardId}/players/")]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayersByBoard(int boardId)
        {
            int[] playerIds = await _context.BoardStates
                .Where(state => state.BoardId == boardId)
                .GroupBy(state => state.PlayerId)
                .Select(group => group.Key)
                .ToArrayAsync();

            if (!playerIds.Any())
            {
                return NotFound();
            }

            return await _context.Players
                .Where(player => playerIds.Contains(player.Id))
                .Select(player => DbPlayerToDTO(player))
                .ToListAsync();
        }

        [HttpGet("players/{playerId}")]
        public async Task<ActionResult<PlayerDTO>> GetPlayer(int playerId)
        {
            DbPlayer dbPlayer = await _context.Players.FindAsync(playerId);

            if (dbPlayer is null)
            {
                return NotFound("The player with that Id was not found! Try a different id...");
            }

            return DbPlayerToDTO(dbPlayer);
        }

        [HttpPost("players")]
        public async Task<ActionResult<PlayerDTO>> PostPlayer(PlayerDTO playerDTO)
        {
            DbPlayer dbPlayer = new DbPlayer
            {
                Name = playerDTO.Name,
                ColorId = playerDTO.ColorId
            };

            await _context.AddAsync(dbPlayer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayer", new { playerId = dbPlayer.Id }, DbPlayerToDTO(dbPlayer));
        }

        [HttpGet("boards/{boardId}/boardStates")]
        public async Task<ActionResult<IEnumerable<BoardStateDTO>>> GetBoardStatesByBoard(int boardId)
        {
            List<BoardStateDTO> boardStates = await _context.BoardStates
                .Where(state => state.BoardId == boardId)
                .Select(state => DbBoardStateToDTO(state))
                .ToListAsync();

            if (boardStates is null)
            {
                return NotFound();
            }

            return boardStates;
        }

        [HttpGet("boards/{boardId}/players/{playerId}/boardStates")]
        public async Task<ActionResult<IEnumerable<BoardStateDTO>>> GetBoardStatesByBoardAndPlayer(int boardId, int playerId)
        {
            List<BoardStateDTO> boardStates = await _context.BoardStates
                .Where(state => state.BoardId == boardId && state.PlayerId == playerId)
                .Select(state => DbBoardStateToDTO(state))
                .ToListAsync();

            if (boardStates is null)
            {
                return NotFound();
            }

            return boardStates;
        }

        [HttpGet("boards/{boardId}/players/{playerId}/pieces/{pieceNumber}/boardState")]
        public async Task<ActionResult<BoardStateDTO>> GetBoardStatesByBoardPlayerAndPiece(int boardId, int playerId, int pieceNumber)
        {
            var boardState = DbBoardStateToDTO(await _context.BoardStates.FirstOrDefaultAsync(state => state.BoardId == boardId
                                                                                      && state.PlayerId == playerId
                                                                                      && state.PieceNumber == pieceNumber));

            if (boardState is null)
            {
                return NotFound();
            }

            return boardState;
        }

            [HttpGet("players/{playerId}/color")]
        public async Task<ActionResult<ColorDTO>> GetPlayerColor(int playerId)
        {
            DbPlayer dbPlayer = await _context.Players.FindAsync(playerId);
            return DbColorToDTO(await _context.Colors.FindAsync(dbPlayer.ColorId));
        }

        [HttpGet("colors")]
        public async Task<ActionResult<IEnumerable<ColorDTO>>> GetColors()
        {
            return await _context.Colors
                .Select(color => DbColorToDTO(color))
                .ToListAsync();
        }

        [HttpGet("boards")]
        public async Task<ActionResult<IEnumerable<BoardDTO>>> GetBoards()
        {
            return await _context.Boards
                .Select(board => DbBoardToDTO(board))
                .ToListAsync();
        }

        [HttpGet("boards/{boardId}")]
        public async Task<ActionResult<BoardDTO>> GetBoard(int boardId)
        {
            return DbBoardToDTO(await _context.Boards.FindAsync(boardId));
        }

        [HttpPost("new")]
        public async Task<ActionResult<int>> NewGame(int[] playerIds, int firstPlayer)
        {
            int boardId = await _context.CreateBoard(firstPlayer);
            await _context.CreateBoardStates(boardId, playerIds);
            return boardId;
        }

        [HttpPost("boards/{boardId}/skipround")]
        public async Task<ActionResult> SkipRound(int boardId)
        {
            await SetNextPlayer(boardId);
            return Ok();
        }

        [HttpGet("boards/{boardId}/players/{playerId}/next")]
        public async Task<ActionResult<int>> GetNextPlayer(int boardId, int playerId)
        {
            bool isGameOver = (await IsGameOver(boardId)).Value;
            if (isGameOver)
            {
                throw new Exception("Game is over, there are no more players.");
            }

            var actionResult = await GetPlayersByBoard(boardId);
            var players = actionResult.Value as List<PlayerDTO>;
            int[] orderedPlayerIds = players
                .OrderBy(player => player.ColorId)
                .Select(player => player.Id)
                .ToArray();

            int nextPlayerIndex = (Array.IndexOf(orderedPlayerIds, playerId) + 1) % orderedPlayerIds.Length;
            int nextPlayerId = orderedPlayerIds[nextPlayerIndex];

            bool hasPlayerWon = (await HasPlayerWon(boardId, nextPlayerId)).Value;
            return hasPlayerWon ? await GetNextPlayer(boardId, nextPlayerId) : nextPlayerId;
        }

        [HttpGet("boards/{boardId}/players/{playerId}/haswon")]
        public async Task<ActionResult<bool>> HasPlayerWon(int boardId, int playerId)
        {
            return !(await GetBoardStatesByBoardAndPlayer(boardId, playerId)).Value.Any();
        }

        [HttpGet("boards/{boardId}/GameOver")]
        public async Task<ActionResult<bool>> IsGameOver(int boardId)
        {

            var actionResult = await GetBoardStatesByBoard(boardId);
            var boardStates = actionResult.Value as List<BoardStateDTO>;

            var activePlayersAmount = boardStates
                .GroupBy(state => state.PlayerId)
                .Count();

            return activePlayersAmount <= 1;
        }

        [HttpPost("boards/{boardId}/players/{playerId}/leaveBase")]
        public async Task<ActionResult<bool>> LeaveBase(int boardId, int playerId)
        {
            int startPosition = (await GetStartingPosition(boardId, playerId)).Value;

            bool isOccupied = (await IsOccupied(boardId, startPosition)).Value;
            if (isOccupied)
            {
                return false;
            }

            DbBoardState baseBoardState = await _context.BoardStates.FirstOrDefaultAsync(state =>
                state.BoardId == boardId &&
                state.PlayerId == playerId &&
                state.IsInBase);
            if (baseBoardState is null)
            {
                return false;
            }
            baseBoardState.PiecePosition = startPosition;
            baseBoardState.IsInBase = false;
            await _context.SaveChangesAsync();

            return true;
        }

        [HttpGet("boards/{boardId}/squares/{position}/isOccupied")]
        public async Task<ActionResult<bool>> IsOccupied(int boardId, int position)
        {
            return await _context.BoardStates
                .Where(state => state.BoardId == boardId && state.PiecePosition == position)
                .AnyAsync();
        }

        [HttpGet("boards/{boardId}/players/{playerId}/startingPosition")]
        public async Task<ActionResult<int>> GetStartingPosition(int boardId, int playerId)
        {
            var players = (await GetPlayersByBoard(boardId)).Value as List<PlayerDTO>;
            int playerOrderPosition = players
                .OrderBy(player => player.ColorId)
                .ToList()
                .FindIndex(player => player.Id == playerId);

            int positionOffset = _boardSize / _maxPlayers;

            return playerOrderPosition * positionOffset;
        }

        [HttpPost("boards/{boardId}/players/{playerId}/pieces/{pieceNumber}/{steps}")]
        public async Task<ActionResult<bool>> PostMove(int boardId, int playerId, int pieceNumber, int steps)
        {
            var dbBoardState = await _context.BoardStates.FirstOrDefaultAsync(state => state.BoardId == boardId
                                                                                       && state.PlayerId == playerId
                                                                                       && state.PieceNumber == pieceNumber);

            if (dbBoardState is null)
            {
                return false;
            }

            int targetPosition = await GetTargetPosition(dbBoardState, steps);

            if (dbBoardState.IsInSafeZone && await IsValidSafeZoneMove(dbBoardState, targetPosition))
            {
                if (targetPosition >= _safeZoneSize)
                {
                    throw new Exception("Can't move piece beyond safe zone.");
                }

                dbBoardState.PiecePosition = targetPosition;
                await _context.SaveChangesAsync();
                await SetNextPlayer(boardId);

                return true;
            }

            if (await IsMovingToSafeZone(dbBoardState, steps))
            {
                int remainingBoardSteps = await GetRemainingBoardSteps(dbBoardState);
                int safeZonePosition = steps - remainingBoardSteps - 1;

                if (!await IsValidSafeZoneMove(dbBoardState, safeZonePosition)) { return false; }

                dbBoardState.IsInSafeZone = true;
                dbBoardState.PiecePosition = safeZonePosition;
                await _context.SaveChangesAsync();
                await SetNextPlayer(boardId);

                return true;
            }

            if (await IsValidBoardMove(dbBoardState, steps))
            {
                bool isOccupied = (await IsOccupied(dbBoardState.BoardId, targetPosition)).Value;
                if (isOccupied)
                {
                    var targetBoardState = await _context.BoardStates.FirstOrDefaultAsync(state => state.BoardId == dbBoardState.BoardId && state.PiecePosition == targetPosition);
                    targetBoardState.IsInBase = true;
                    targetBoardState.PiecePosition = 0;
                }

                dbBoardState.PiecePosition = targetPosition;
                await _context.SaveChangesAsync();
                await SetNextPlayer(boardId);

                return true;
            }

            return false;
        }

        [HttpGet("boards/{boardId}/players/{playerId}/pieces/base/")]
        public async Task<ActionResult<IEnumerable<BoardStateDTO>>> GetBasePieces(int boardId, int playerId)
        {
            return await _context.BoardStates
                .Where(state => state.BoardId == boardId && state.PlayerId == playerId && state.IsInBase)
                .Select(state => DbBoardStateToDTO(state))
                .ToListAsync();
        }

        [HttpGet("boards/{boardId}/players/{playerId}/pieces/goal/")]
        public async Task<int> GetGoalPieceAmount(int boardId, int playerId)
        {
            int maxPieceAmount = 4;
            int activePlayerPieceAmount = await _context.BoardStates
                .Where(state => state.BoardId == boardId && state.PlayerId == playerId)
                .CountAsync();

            return maxPieceAmount - activePlayerPieceAmount;
        }

        [HttpGet("boards/{boardId}/players/{playerId}/pieces/movable/{steps}")]
        public async Task<ActionResult<IEnumerable<BoardStateDTO>>>GetMovablePieces(int boardId, int playerId, int steps)
        {
            var tempStates = await _context.BoardStates.Where(state => state.BoardId == boardId && state.PlayerId == playerId && !state.IsInBase).ToListAsync();
            List<BoardStateDTO> boardStateDTOs = new();
            foreach (var state in tempStates)
            {
                if(await IsValidBoardMove(state, steps))
                {
                    boardStateDTOs.Add(DbBoardStateToDTO(state));
                }
            }
            return boardStateDTOs;
        }

        private async Task<bool> IsValidBoardMove(DbBoardState dbBoardState, int steps)
        {
            int targetPosition = await GetTargetPosition(dbBoardState, steps);

            if (dbBoardState.IsInSafeZone || dbBoardState.IsInBase) { return false; }
            if (await IsLoopingAroundBoard(dbBoardState, steps)) { return false; }
            if (await IsOwnedBySamePlayer(dbBoardState, targetPosition)) { return false; }

            return true;
        }

        private async Task<bool> IsOwnedBySamePlayer(DbBoardState dbBoardState, int position)
        {
            return await _context.BoardStates.AnyAsync(state => state.BoardId == dbBoardState.BoardId
                                                                && state.PlayerId == dbBoardState.PlayerId
                                                                && state.PiecePosition == position);
        }

        private async Task<bool> IsValidSafeZoneMove(DbBoardState dbBoardState, int targetPosition)
        {
            return targetPosition < _safeZoneSize && !(await IsOccupiedInSafeZone(dbBoardState, targetPosition));
        }

        private async Task<bool> IsOccupiedInSafeZone(DbBoardState dbBoardState, int targetPosition)
        {
            return await _context.BoardStates.AnyAsync(state => state.PlayerId == dbBoardState.PlayerId
                                                           && state.IsInSafeZone
                                                           && state.PiecePosition == targetPosition);
        }

        private async Task SetNextPlayer(int boardId)
        {
            var board = await _context.Boards.FindAsync(boardId);
            var nextPlayer = await GetNextPlayer(boardId, board.activePlayerId);
            board.activePlayerId = nextPlayer.Value;
            await _context.SaveChangesAsync();
        }

        private async Task<int> GetTargetPosition(DbBoardState boardState, int steps)
        {
            int startPosition = boardState.PiecePosition;

            if (boardState.IsInSafeZone)
            {
                return startPosition + steps;
            }

            if (await IsMovingToSafeZone(boardState, steps))
            {
                int remainingBoardSteps = await GetRemainingBoardSteps(boardState);
                return steps - remainingBoardSteps - 1;
            }

            return await GetNewPosition(boardState, steps);
        }

        private async Task<int> GetNewPosition(DbBoardState boardState, int steps)
        {
            int[] playerPath = await GetPlayerPath(boardState);
            int startingPosition = boardState.PiecePosition;
            int startPositionIndex = Array.IndexOf(playerPath, startingPosition);
            return playerPath[startPositionIndex + steps];
        }

        private async Task<int> GetRemainingBoardSteps(DbBoardState boardState)
        {
            int[] playerPath = await GetPlayerPath(boardState);

            int lastIndex = playerPath.Length - 1;
            int piecePosition = boardState.PiecePosition;
            int pieceIndex = Array.IndexOf(playerPath, piecePosition);

            return lastIndex - pieceIndex;
        }

        private async Task<bool> IsMovingToSafeZone(DbBoardState boardState, int steps)
        {
            return !boardState.IsInSafeZone && await IsLoopingAroundBoard(boardState, steps);
        }

        private async Task<bool> IsLoopingAroundBoard(DbBoardState boardState, int steps)
        {
            int[] playerPath = await GetPlayerPath(boardState);
            int startPositionIndex = Array.IndexOf(playerPath, boardState.PiecePosition);

            return startPositionIndex + steps >= playerPath.Length;
        }

        private async Task<int[]> GetPlayerPath(DbBoardState boardState)
        {
            int[] playerPath = new int[_boardSize];
            int startingPosition = (await GetStartingPosition(boardState.BoardId, boardState.PlayerId)).Value;
            for (int i = 0; i < _boardSize; i++)
            {
                playerPath[i] = (i + startingPosition) % _boardSize;
            }

            return playerPath;
        }

        private static BoardDTO DbBoardToDTO(DbBoard board) => new()
        {
            Id = board.Id,
            IsFinished = board.IsFinished,
            LastTimePlayed = board.LastTimePlayed,
            activePlayerId = board.activePlayerId
        };

        private static PlayerDTO DbPlayerToDTO(DbPlayer dbPlayer) => new()
        {
            Id = dbPlayer.Id,
            Name = dbPlayer.Name,
            ColorId = dbPlayer.ColorId
        };

        private static BoardStateDTO DbBoardStateToDTO(DbBoardState boardState) => new()
        {
            Id = boardState.Id,
            PlayerId = boardState.PlayerId,
            BoardId = boardState.BoardId,
            PieceNumber = boardState.PieceNumber,
            PiecePosition = boardState.PiecePosition,
            IsInSafeZone = boardState.IsInSafeZone,
            IsInBase = boardState.IsInBase
        };

        private static ColorDTO DbColorToDTO(DbColor color) => new()
        {
            Id = color.Id,
            ColorCode = color.ColorCode
        };

        // HasActivePieces(int boardId, int playerId)
        // GetActivePieces(int boardId, int playerId)
        // GetMovablePieces(int boardId, int playerId, int steps)
        // IsMovable(int boardId, playerId, pieceNo)
    }
}

using LudoEngine.Engine;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine.Database
{
    public static class DbQuery
    {
        public static async Task<List<Player>> GetAllPlayers()
        {
            await using var context = new LudoContext();
            List<Player> players = await context.Players
                .Select(dbPlayer => new Player
                {
                    Id = dbPlayer.Id,
                    Name = dbPlayer.Name,
                    ColorId = dbPlayer.Color.Id
                }).ToListAsync();

            return players;
        }

        public static async Task<List<Player>> GetPlayers(int boardId)
        {
            await using var context = new LudoContext();
            List<Player> players = await context.BoardStates
                .Where(boardState => boardState.BoardId == boardId)
                .Select(boardState => boardState.Player)
                .Distinct()
                .Select(player => new Player
                {
                    Id = player.Id,
                    ColorId = player.ColorId,
                    Name = player.Name
                }
                ).ToListAsync();

            return players;
        }

        // todo: use these notes for Documentation, delete afterwards.
        // If someone calls GetHistory without a parameter, it will be giving it a new LudoContext, but if they put a different Context in the parameter then 
        // the method has to use that one we did this because for testing we use a different context (overloading a method) solution for testing that is.
        public static async Task<List<History>> GetHistory()
        {
            return await GetHistory(new LudoContext());
        }

        public static async Task<List<History>> GetHistory(LudoContext context)
        {
            await using var _context = context;
            //we need Winner, Board and Player
            List<DbWinner> winners = await _context.Winners
                .Include(winner => winner.Board)
                .Include(winner => winner.Player)
                .Where(winner => winner.Board.IsFinished)
                .ToListAsync();

            List<History> history = winners
                .GroupBy(winner => winner.Board)
                .Select(group => new History()
                {
                    GameId = group.Key.Id,
                    LastTimePlayed = group.Key.LastTimePlayed,
                    Placements = group
                        .OrderBy(placement => placement.Placement)
                        .ToDictionary(group => group.Placement, group => group.Player.Name)
                })
                .Where(history => history.Placements.Count > 0)
                .ToList();

            return history;
        }

        public static async Task<List<BoardData>> GetUnfinishedBoards()
        {
            return await GetUnfinishedBoards(new LudoContext());
        }

        public static async Task<List<BoardData>> GetUnfinishedBoards(LudoContext context)
        {
            // of not finished boards, so we go into Board look at the ids that aren't finished and get a list of boardstates
            await using var _context = context;
            List<DbBoardState> boardStates = await _context.BoardStates
                .Include(boardState => boardState.Player)
                .Include(boardState => boardState.Board)
                .Where(boardState => !boardState.Board.IsFinished)
                .ToListAsync();

            List<BoardData> boardData = boardStates
                .GroupBy(boardState => boardState.Board)
                .Select(group => new BoardData()
                {
                    BoardId = group.Key.Id,
                    LastTimePlayed = group.Key.LastTimePlayed,
                    PlayerNames = group
                        .GroupBy(group => group.PlayerId)
                        .Select(group => group.First())
                        .Select(group => group.Player.Name)
                        .ToList()
                })
                .ToList();

            return boardData;
        }

        public static async Task AddWinner(int boardId, int playerId, int placement)
        {
            await using var context = new LudoContext();

            var winner = new DbWinner
            {
                BoardId = boardId,
                PlayerId = playerId,
                Placement = placement
            };

            context.Add(winner);
            await context.SaveChangesAsync();
        }

        public static async Task<Dictionary<int, Player>> GetWinners(int selectedBoardId)
        {
            await using var context = new LudoContext();
            var winners = await context.Winners
                .Where(winner => winner.BoardId == selectedBoardId)
                .ToDictionaryAsync(winner => winner.Placement, winner => new Player
                {
                    ColorId = winner.Player.ColorId,
                    Id = winner.Player.Id,
                    Name = winner.Player.Name
                });

            return winners;
        }

        public static async Task<List<DbBoardState>> GetBoardStates(int selectedBoardId)
        {
            await using var context = new LudoContext();
            List<DbBoardState> boardStates = await context.BoardStates
                .Include(boardState => boardState.Player)
                .Include(boardState => boardState.Board)
                .Where(boardState => boardState.BoardId == selectedBoardId)
                .ToListAsync();

            return boardStates;
        }

        public static async Task<int> AddPlayer(Player player)
        {
            await using LudoContext context = new();
            DbPlayer dbPlayer = new()
            {
                Name = player.Name,
                ColorId = player.ColorId
            };
            context.Players.Add(dbPlayer);
            await context.SaveChangesAsync();

            return dbPlayer.Id;
        }

        public static async Task<DbColor> GetColor(int id)
        {
            await using var context = new LudoContext();
            return context.Colors.Find(id);
        }

        public static async Task<List<Color>> GetAllColors()
        {
            await using var context = new LudoContext();
            List<Color> colors = await context.Colors
                .Select(dbColor => new Color
                {
                    Id = dbColor.Id,
                    ColorCode = dbColor.ColorCode
                })
                .ToListAsync();
            return colors;
        }

        public static async Task InitializeBoardStates(List<DbBoardState> boardStates)
        {
            await using var context = new LudoContext();
            boardStates.ForEach(boardState => context.BoardStates.Add(boardState));
            await context.SaveChangesAsync();
        }

        public static async Task UpdateDbState(int boardId, int playerId, int pieceNumber, int piecePosition, bool isInBase, bool isInSafeZone)
        {
            await using var context = new LudoContext();
            DbBoardState boardState = await context.BoardStates.Where(state =>
                state.BoardId == boardId &&
                state.PlayerId == playerId &&
                state.PieceNumber == pieceNumber
            ).FirstOrDefaultAsync();

            try
            {
                boardState.PiecePosition = piecePosition;
                boardState.IsInBase = isInBase;
                boardState.IsInSafeZone = isInSafeZone;
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't find piece in database.");
            }
        }

        public static async Task DeleteDbState(int boardId, int playerId, int pieceNumber)
        {
            await using var context = new LudoContext();

            var boardState = await context.BoardStates.FirstOrDefaultAsync(state =>
                state.BoardId == boardId &&
                state.PlayerId == playerId &&
                state.PieceNumber == pieceNumber);

            context.Remove(boardState);
            await context.SaveChangesAsync();
        }

        public static async Task SaveDbState(Player player, Piece piece, Board board, int boardId, bool isInBase, bool isInSafeZone)
        {
            var pieceNumber = GetSpecificPieceByPlayerIdAndPiecenumber(player, piece, board);
            var piecePosition = GetPiecePositionsByPlayer(player, board).Where(x => x.Value.PieceNumber == piece.PieceNumber).Select(x => x.Key).FirstOrDefault();
            using var context = new LudoContext();
            DbBoardState dbBoardState = new DbBoardState()
            {
                BoardId = boardId,
                PlayerId = player.Id,
                PieceNumber = pieceNumber,
                PiecePosition = piecePosition,
                IsInBase = isInBase,
                IsInSafeZone = isInSafeZone
            };
            context.BoardStates.Add(dbBoardState);
            await context.SaveChangesAsync();
        }

        //Save Db Board to database.
        public static async Task<int> AddDbBoard()
        {
            using var context = new LudoContext();
            var lastTimePlayed = DateTime.Now;
            DbBoard dbBoard = new DbBoard()
            {
                IsFinished = false,
                LastTimePlayed = lastTimePlayed
            };

            context.Add(dbBoard);
            await context.SaveChangesAsync();

            return dbBoard.Id;
        }

        public static async Task FinishBoard(int boardId)
        {
            await using var context = new LudoContext();

            var board = await context.Boards.FirstOrDefaultAsync(board => board.Id == boardId);
            board.IsFinished = true;

            List<DbBoardState> boardStates = await context.BoardStates.Where(state => state.BoardId == boardId).ToListAsync();
            context.RemoveRange(boardStates);

            await context.SaveChangesAsync();
        }

        //Save the winners to databse.
        public static async Task SaveWinner(int playerId, int boardId, int placement)
        {
            await using var context = new LudoContext();
            DbWinner dbWinner = new DbWinner()
            {
                BoardId = boardId,
                PlayerId = playerId,
                Placement = placement
            };

            context.Add(dbWinner);
            await context.SaveChangesAsync();
        }

        //Get specific piece position by player Id and piece number.
        public static int GetSpecificPieceByPlayerIdAndPiecenumber(Player player, Piece piece, Board board)
        {
            int x = GetPiecePositionsByPlayer(player, board).Where(x => x.Value.PieceNumber == piece.PieceNumber).Select(x => x.Value.PieceNumber).FirstOrDefault();
            return x;
        }
        public static Dictionary<int, Piece> GetPiecePositionsByPlayer(Player player, Board board)
        {
            Dictionary<int, Piece> playerPiecePositions = new Dictionary<int, Piece>();

            var result = board.GetBoardPieces(player);
            foreach (var item in result)
            {
                playerPiecePositions.Add(item.Key, item.Value);
            }
            return playerPiecePositions;
        }
    }
}

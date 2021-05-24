using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoFrontEnd.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LudoFrontEnd.Pages
{
    public class LudoGameModel : PageModel
    {
        private LudoApi _ludoApi = new();
        public List<LudoBoardState> BoardStates = new();
        public Dictionary<int, int> GoalCount { get; set; } = new();
        // key = player, value = count
        public Dictionary<int, int> BasePiecesCount { get; set; } = new();
        //public List<(int, int)> BoardPieces { get; set; }

        private readonly (int x, int y)[] _squareCoordinates =
        {
            (7, 15), (7, 14), (7, 13), (7, 12), (7, 11), (7, 10),
            (6, 9), (5, 9), (4, 9), (3, 9), (2, 9), (1, 9),
            (1, 8),
            (1, 7), (2, 7), (3, 7), (4, 7), (5, 7), (6, 7),
            (7, 6), (7, 5), (7, 4), (7, 3), (7, 2), (7, 1),
            (8, 1),
            (9, 1), (9, 2), (9, 3), (9, 4), (9, 5), (9, 6),
            (10, 7), (11, 7), (12, 7), (13, 7), (14, 7), (15, 7),
            (15, 8),
            (15, 9), (14, 9), (13, 9), (12, 9), (11, 9), (10, 9),
            (9, 10), (9, 11), (9, 12), (9, 13), (9, 14), (9, 15),
            (8, 15)
        };

        private readonly Dictionary<int, (int x, int y)[]> _safeZones = new Dictionary<int, (int x, int y)[]>()
        {
            { 1, new (int x, int y)[] { (8, 14), (8, 13), (8, 12), (8, 11), (8, 10) } },
            { 2, new (int x, int y)[] { (2, 8), (3, 8), (4, 8), (5, 8), (6, 8) } },
            { 3, new (int x, int y)[] { (8, 2), (8, 3), (8, 4), (8, 5), (8, 6) } },
            { 4, new (int x, int y)[] { (14, 8), (13, 8), (12, 8), (11, 8), (10, 8) } }
        };

        public (int x, int y)[] SquareCoordinates => _squareCoordinates;

        public Dictionary<int, (int x, int y)[]> SafeZones => _safeZones;

        public async Task OnGet(int boardId)
        {
            //BoardStates = await _ludoApi.GetBoardStatesByBoard(boardId);

            //CountBasePieces();
            //CountGoalPieces();
            //AssignPlayerIds();
        }

        private void AssignPlayerIds()
        {
            // get all players, assign them to player id 1 - 4

        }

        private void CountBasePieces()
        {
            foreach (var state in BoardStates)
            {
                if (state.IsInBase)
                {
                    if(!BasePiecesCount.ContainsKey(state.PlayerId))
                    {
                        BasePiecesCount.Add(state.PlayerId, 0);
                    }
                    BasePiecesCount[state.PlayerId]++;
                }
            }
        }

        private void CountGoalPieces()
        {
            int maxPieces = 4;

            GoalCount = BoardStates
                .Where(state => !state.IsInSafeZone && !state.IsInBase)
                .GroupBy(state => state.PlayerId)
                .Select(group => new
                {
                    playerId = group.Key,
                    count = maxPieces - group.Count()
                })
                .ToDictionary(group => group.playerId, group => group.count);
        }


        // count pieces in base (per player)
        // count pieces in goal (per player) (max pieces (4) - board pieces - base pieces)
        // put board pieces on board (per player)

        //new DbBoardState { Id = 1, PlayerId = 1, BoardId = 5, PieceNumber = 1, PiecePosition = 18, IsInSafeZone = false, IsInBase = false },
        //new DbBoardState { Id = 2, PlayerId = 1, BoardId = 5, PieceNumber = 2, PiecePosition = 13, IsInSafeZone = false, IsInBase = false },
        //new DbBoardState { Id = 3, PlayerId = 1, BoardId = 5, PieceNumber = 3, PiecePosition = 0, IsInSafeZone = false, IsInBase = true },
        //new DbBoardState { Id = 4, PlayerId = 1, BoardId = 5, PieceNumber = 4, PiecePosition = 0, IsInSafeZone = false, IsInBase = true },
        //new DbBoardState { Id = 5, PlayerId = 2, BoardId = 5, PieceNumber = 1, PiecePosition = 0, IsInSafeZone = true, IsInBase = false },
        //new DbBoardState { Id = 6, PlayerId = 2, BoardId = 5, PieceNumber = 2, PiecePosition = 22, IsInSafeZone = false, IsInBase = false },
        //new DbBoardState { Id = 7, PlayerId = 2, BoardId = 5, PieceNumber = 3, PiecePosition = 0, IsInSafeZone = false, IsInBase = true },
        //new DbBoardState { Id = 8, PlayerId = 2, BoardId = 5, PieceNumber = 4, PiecePosition = 0, IsInSafeZone = false, IsInBase = true }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoFrontEnd.Api;
using LudoFrontEnd.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LudoFrontEnd.Pages
{
    public class LudoGameModel : PageModel
    {
        private readonly LudoApi _ludoApi = new();
        public int BoardId { get; set; }
        public List<LudoBoardState> BoardStates = new();
        public List<Color> Colors { get; set; } = new();
        public Dictionary<int, int> GoalCount { get; set; } = new();
        public Dictionary<int, int> BaseCount { get; set; } = new();

        public List<int> Players = new();
        public int ActivePlayerId { get; set; }

        public (int x, int y)[] SquareCoordinates { get; } =
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

        public List<(int x, int y)[]> SafeZones { get; } = new List<(int x, int y)[]>
        {
            new (int x, int y)[] { (8, 14), (8, 13), (8, 12), (8, 11), (8, 10) },
            new (int x, int y)[] { (2, 8), (3, 8), (4, 8), (5, 8), (6, 8) },
            new(int x, int y)[] { (8, 2), (8, 3), (8, 4), (8, 5), (8, 6) },
            new(int x, int y)[] { (14, 8), (13, 8), (12, 8), (11, 8), (10, 8) }
        };

        public async Task OnGet(int boardId)
        {
            BoardId = boardId;
            if (BoardId == 0)
            {
                return;
            }
            BoardStates = await _ludoApi.GetBoardStatesByBoard(boardId);
            GetPlayers();
            await GetActivePlayer();
            await CountBasePieces();
            await CountGoalPieces();
            await GetColors();
        }

        private async Task GetActivePlayer()
        {
            ActivePlayerId = await _ludoApi.GetActivePlayerId(BoardId);
        }

        private async Task GetColors()
        {
            Colors = await _ludoApi.GetColors();
        }

        private void GetPlayers()
        {
            Players = BoardStates
                .GroupBy(state => state.PlayerId)
                .Select(group => group.Key)
                .ToList();
        }

        private async Task CountBasePieces()
        {
            foreach (var playerId in Players)
            {
                var basePieces = await _ludoApi.GetBasePieces(BoardId, playerId);
                if(!BaseCount.TryAdd(playerId, basePieces.Count))
                {
                    BaseCount[playerId] = basePieces.Count;
                }
            }
        }

        private async Task CountGoalPieces()
        {
            foreach (var playerId in Players)
            {
                var goalCount = await _ludoApi.GetGoalCount(BoardId, playerId);
                if(!GoalCount.TryAdd(playerId, goalCount))
                {
                    GoalCount[playerId] = goalCount;
                }
            }
        }

        public bool IsOccupied(int position, bool isInSaveZone = false, int playerIndex = 0)
        {
            if(isInSaveZone && playerIndex < Players.Count)
            {
                return BoardStates.Any(state => !state.IsInBase
                    && state.IsInSafeZone == isInSaveZone
                    && state.PlayerId == Players[playerIndex]
                    && state.PiecePosition == position);

            }
            return BoardStates.Any(state => !state.IsInBase && !state.IsInSafeZone && state.PiecePosition == position);
        }

        public int GetPlayerIdAtIndex(int index, bool isInBase = false, bool isInSafeZone = false)
        {
            var boardState = BoardStates.First(state => state.PiecePosition == index && state.IsInBase == isInBase && state.IsInSafeZone == isInSafeZone);
            if (boardState is null)
            {
                throw new Exception($"Unable to find a LudoBoardState at index: {index}!");
            }
            return boardState.PlayerId;
        }

        public int GetPlayerIndex(int playerId)
        {
            return Players.IndexOf(playerId);
        }

        public int GetBaseCount(int index)
        {
            return index >= Players.Count ? 0 : BaseCount[Players[index]];
        }

        public int GetGoalCount(int index)
        {
            return index >= Players.Count ? 0 : GoalCount[Players[index]];
        }

        public int RollDie()
        {
            Random random = new();
            return random.Next(1, 7);
        }
    }
}

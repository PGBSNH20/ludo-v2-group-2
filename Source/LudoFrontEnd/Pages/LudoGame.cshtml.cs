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
        public List<BoardState> BoardStates = new();
        public List<Color> Colors { get; set; } = new();
        // color/board-Index, count
        public int[] GoalCount { get; set; } = new int[4];
        // color/board-Index, count
        public int[] BaseCount { get; set; } = new int[4];
        // color/board-Index, player
        public Dictionary<int, Player> Players = new();
        // placement, playerid
        public Dictionary<int, int> Winners { get; set; } = new();
        public int ActivePlayerId { get; set; }
        public int ActivePlayerIndex { get; set; }
        public int PlayerNumber { get; set; }
        public bool IsActivePlayer {
            get {
                if (PlayerNumber == 0)
                {
                    return true;
                }
                return ActivePlayerIndex == PlayerNumber - 1;
            }
        }

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

        public async Task<IActionResult> OnGetAsync(int boardId, int playerNumber)
        {
            if (boardId == 0)
            {
                return new RedirectToPageResult("/Index");

            }
            if (await _ludoApi.IsGameOver(boardId))
            {
                Response.Redirect($"/LudoGame/{boardId}/gameover");
            }
            BoardId = boardId;

            await LoadColors();
            PlayerNumber = playerNumber;
            BoardStates = await _ludoApi.GetBoardStatesByBoard(boardId);
            await LoadPlayers();
            await LoadActivePlayer();
            await LoadWinners();
            await CountBasePieces();
            await CountGoalPieces();
            return new PageResult();
        }

        private async Task LoadWinners()
        {
            List<Winner> winners = await _ludoApi.GetWinners(BoardId);
            if (winners is null)
            {
                return;
            }

            Winners = winners.ToDictionary(winner => winner.Placement, Winner => Winner.PlayerId);
        }

        private async Task LoadActivePlayer()
        {
            ActivePlayerId = await _ludoApi.GetActivePlayerId(BoardId);
            ActivePlayerIndex = Players.FirstOrDefault(player => player.Value.Id == ActivePlayerId).Key;
        }

        public async Task LoadColors()
        {
            Colors = await _ludoApi.GetColors();
        }

        private async Task LoadPlayers()
        {
            if (Colors.Count < 4)
            {
                throw new Exception("Looks like there aren't enough colors!");
            }
            List<int> playerIds = BoardStates
                .GroupBy(state => state.PlayerId)
                .Select(group => group.Key)
                .ToList();
            Players = new();
            foreach (var id in playerIds)
            {
                var player = await _ludoApi.GetPlayer(id);
                Players.Add(Colors.FindIndex(color => color.Id == player.ColorId), player);
            }
        }

        private async Task CountBasePieces()
        {
            foreach (var player in Players)
            {
                var basePieces = await _ludoApi.GetBasePieces(BoardId, player.Value.Id);
                BaseCount[player.Key] = basePieces.Count;
            }
        }

        private async Task CountGoalPieces()
        {
            foreach (var player in Players)
            {
                var goalCount = await _ludoApi.GetGoalCount(BoardId, player.Value.Id);
                GoalCount[player.Key] = goalCount;
            }
        }

        public bool IsOccupied(int position, bool isInSaveZone = false, int playerIndex = -1)
        {
            if (isInSaveZone && Players.ContainsKey(playerIndex))
            {
                return BoardStates.Any(state => !state.IsInBase
                    && state.IsInSafeZone == isInSaveZone
                    && state.PlayerId == Players[playerIndex].Id
                    && state.PiecePosition == position);
            }
            if (isInSaveZone) return false;
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
            return Players.FirstOrDefault(player => player.Value.Id == playerId).Key;
        }

        public int GetBaseCount(int index)
        {
            return BaseCount[index];
        }

        public int GetGoalCount(int index)
        {
            return GoalCount[index];
        }

        public int RollDie()
        {
            Random random = new();
            return random.Next(1, 7);
        }

        public async Task<int> GetWinnerIndex(int playerId)
        {
            var playerColor = await _ludoApi.GetPlayerColor(playerId);
            return Colors.FindIndex(color => color.Id == playerColor.Id);
        }

        public async Task<int> GetPlacement(int playerIndex)
        {
            foreach (var winner in Winners)
            {
                var winnerIndex = await GetWinnerIndex(winner.Value);
                if (winnerIndex == playerIndex)
                {
                    return winner.Key;
                }
            }
            return -1;
        }

        public async Task<bool> IsWinner(int playerIndex)
        {
            foreach (var winner in Winners)
            {
                if (await GetWinnerIndex(winner.Value) == playerIndex)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<string> GetPlayerName(int playerIndex)
        {
            if (Players.ContainsKey(playerIndex))
            {
                return Players[playerIndex].Name;
            }

            foreach (var winner in Winners)
            {
                if (await GetWinnerIndex(winner.Value) == playerIndex)
                {
                    var player = await _ludoApi.GetPlayer(winner.Value);
                    return player.Name;
                }
            }

            return "";
        }

        public string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }
        }
    }
}

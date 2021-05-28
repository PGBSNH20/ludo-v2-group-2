using LudoFrontEnd.Model;
using LudoFrontEnd.Pages;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoFrontEnd.Api
{
    public class LudoApi
    {
        private readonly RestClient client;

        public LudoApi()
        {
            client = new RestClient("https://localhost:5001/api/game");
            client.AddDefaultHeader("ApiKey", "Mountain24");
        }

        public async Task<List<BoardState>> GetBoardStatesByBoard(int boardId)
        {
            var request = new RestRequest($"/boards/{boardId}/boardStates", DataFormat.Json);
            return await client.GetAsync<List<BoardState>>(request);
        }

        public async Task<List<Board>> GetUnfinishedBoards()
        {
            var request = new RestRequest($"/boards/unfinished", DataFormat.Json);
            return await client.GetAsync<List<Board>>(request);
        }

        public async Task<List<BoardState>> GetBasePieces(int boardId, int playerId)
        {
            var request = new RestRequest($"boards/{boardId}/players/{playerId}/pieces/base/", DataFormat.Json);
            return await client.GetAsync<List<BoardState>>(request);
        }

        public async Task<int> GetGoalCount(int boardId, int playerId)
        {
            var request = new RestRequest($"boards/{boardId}/players/{playerId}/pieces/goal/", DataFormat.Json);
            return await client.GetAsync<int>(request);
        }

        public async Task<List<Color>> GetColors()
        {
            var request = new RestRequest($"colors", DataFormat.Json);
            return await client.GetAsync<List<Color>>(request);
        }

        public async Task<Color> GetPlayerColor(int playerId)
        {
            var request = new RestRequest($"players/{playerId}/color", DataFormat.Json);
            return await client.GetAsync<Color>(request);
        }

        public async Task<int> GetActivePlayerId(int boardId)
        {
            var request = new RestRequest($"boards/{boardId}", DataFormat.Json);
            var ludoBoard = await client.GetAsync<Board>(request);
            return ludoBoard.activePlayerId;
        }

        public async Task<List<BoardState>> GetMovablePieces(int boardId, int playerId, int steps)
        {
            var request = new RestRequest($"boards/{boardId}/players/{playerId}/pieces/movable/{steps}", DataFormat.Json);
            return await client.GetAsync<List<BoardState>>(request);
        }

        public async Task<Player> GetPlayer(int playerId)
        {
            var request = new RestRequest($"players/{playerId}", DataFormat.Json);
            return await client.GetAsync<Player>(request);
        }

        public async Task<List<Winner>> GetWinners(int boardId)
        {
            var request = new RestRequest($"boards/{boardId}/winners", DataFormat.Json);
            return await client.GetAsync<List<Winner>>(request);
        }

        public async Task<bool> IsGameOver(int boardId)
        {
            var request = new RestRequest($"boards/{boardId}/GameOver", DataFormat.Json);
            return await client.GetAsync<bool>(request);
        }

        public async Task<int> AddPlayer(Player player)
        {
            var request = new RestRequest($"players", Method.POST, DataFormat.Json);
            request.AddJsonBody(player);
            return (await client.PostAsync<Player>(request)).Id;
        }

        public async Task<int> NewGame(int[] playerIds, int firstPlayer)
        {
            var request = new RestRequest($"new?firstPlayer={firstPlayer}", Method.POST, DataFormat.Json);
            request.AddJsonBody(playerIds);
            var boardId = await client.PostAsync<int>(request);
            return boardId;
        }

        //public async Task<object> GetTargetPositionForPlayer(int boardId, int playerId, int pieceNumber, int steps)
        //{
        //    var request = new RestRequest($"boards/{boardId}/players/{playerId}/pieces/{pieceNumber}/targetposition/{steps}", DataFormat.Json);
        //    return await client.GetAsync<object>(request);
        //}
    }
}

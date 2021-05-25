using LudoFrontEnd.Model;
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

        public async Task<List<LudoBoardState>> GetBoardStatesByBoard(int boardId)
        {
            var request = new RestRequest($"/boards/{boardId}/boardStates", DataFormat.Json);
            return await client.GetAsync<List<LudoBoardState>>(request);
        }

        public async Task<List<LudoBoardState>> GetBasePieces(int boardId, int playerId)
        {
            var request = new RestRequest($"boards/{boardId}/players/{playerId}/pieces/base/", DataFormat.Json);
            return await client.GetAsync<List<LudoBoardState>>(request);
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

        public async Task<int> GetActivePlayerId(int boardId)
        {
            var request = new RestRequest($"boards/{boardId}", DataFormat.Json);
            var ludoBoard = await client.GetAsync<LudoBoard>(request);
            return ludoBoard.activePlayerId;
        }

        public async Task<List<LudoBoardState>> GetMovablePieces(int boardId, int playerId, int steps)
        {
            var request = new RestRequest($"boards/{boardId}/players/{playerId}/pieces/movable/{steps}", DataFormat.Json);
            return await client.GetAsync<List<LudoBoardState>>(request);
        }
    }
}

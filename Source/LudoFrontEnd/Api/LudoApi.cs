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
            client = new RestClient("https://localhost:5001/api/");
            client.AddDefaultHeader("ApiKey", "Mountain24");
        }

        public async Task<List<LudoBoardState>> GetBoardStatesByBoard(int boardId)
        {
            /// BoardStates / Board / 5
            var request = new RestRequest($"BoardStates/Board/{boardId}", DataFormat.Json);
            return await client.GetAsync<List<LudoBoardState>>(request);
        }
    }
}

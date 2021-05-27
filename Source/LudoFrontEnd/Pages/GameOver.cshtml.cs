using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoFrontEnd.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LudoFrontEnd.Pages
{
    public class GameOverModel : PageModel
    {
        private readonly LudoApi _ludoApi = new();
        private int _boardId;
        public Dictionary<int, Player> Winners { get; set; } = new();

        public async Task OnGetAsync(int boardId)
        {
            _boardId = boardId;
            await LoadWinners();
        }

        private async Task LoadWinners()
        {
            List<Winner> winners = await _ludoApi.GetWinners(_boardId);
            if (winners is null)
            {
                return;
            }
            foreach (var winner in winners)
            {
                Winners.TryAdd(winner.Placement, await _ludoApi.GetPlayer(winner.PlayerId));
            }
        }

        public async Task<string> GetPlayerColor(int playerId)
        {
            var color = await _ludoApi.GetPlayerColor(playerId);
            return color.ColorCode;
        }
    }
}

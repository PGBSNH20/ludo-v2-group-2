using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LudoFrontEnd.Api;
using LudoFrontEnd.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LudoFrontEnd.Pages
{
    public class NewGameAnasModel : PageModel
    {
        private readonly LudoApi _ludoApi = new();
        [TempData]
        public int PlayerCount { get; set; } = 0;
        public List<Player> Players { get; set; } = new();
        public List<Color> Colors { get; set; }
        public string[] ColorNames = { "Yellow", "Blue", "Green", "Red" };
        public int BoardId { get; set; } = 0;
        public int CurrentPlayer { get
            {
                return Players.Count + 1;
            }
        }
        //public string PlayerName = "";
        public async Task<IActionResult> OnGet()
        {
            TempData.Clear();
            Colors = await _ludoApi.GetColors();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int playerCount, string playerName, int colorId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PlayerCount = playerCount > 0 ? playerCount : PlayerCount;
            Colors = await _ludoApi.GetColors();
            TempData.Keep("PlayerCount");

            if (TempData.Peek("Players") is not null)
            {
                Players = JsonSerializer.Deserialize<List<Player>>(TempData.Peek("Players") as string);
            }

            if (playerName is not null)
            {
                Players.Add(new Player
                {
                    Name = playerName,
                    ColorId = colorId
                });
                TempData["Players"] = JsonSerializer.Serialize(Players);
            }

            if (Players.Count == PlayerCount)
            {
                List<int> playerIds = new();
                foreach (var player in Players)
                {
                    playerIds.Add(await _ludoApi.AddPlayer(player));
                }
                Random random = new();
                int firstPlayerIndex = random.Next(0, playerIds.Count);
                int boardId = await _ludoApi.NewGame(playerIds.ToArray(), playerIds[firstPlayerIndex]);
                TempData.Clear();
                Response.Redirect($"/LudoGame/{boardId}/");
            }

            if (BoardId > 0)
            {
                Response.Redirect($"/LudoGame/{BoardId}/");
            }
            return Page();
        }

        public int[] GetAvailableColorIndixes()
        {
            var colors = Colors.Where(color => !Players.Any(player => player.ColorId == color.Id));
                
            return colors.Select(color => Colors.IndexOf(color)).ToArray();
        }
    }
}

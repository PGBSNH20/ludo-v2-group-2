using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoFrontEnd.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LudoFrontEnd.Pages
{
    public class LoadGameModel : PageModel
    {
        private readonly LudoApi _ludoApi = new();
        public Dictionary<Board, List<Player>> Boards { get; set; } = new();
        public async Task OnGetAsync()
        {
            await GetUnfinishedBoards();
        }

        public IActionResult OnPost(int boardId)
        {
            if (boardId == 0)
            {
                return new RedirectToPageResult("/LoadGame");

            }
            if (boardId > 0)
            {
                return new RedirectToPageResult("/LudoGame", new { boardId = boardId });
            }

            return new PageResult();
        }

        private async Task GetUnfinishedBoards()
        {
            List<Board> boards = await _ludoApi.GetUnfinishedBoards();
            foreach (var board in boards)
            {
                if(!Boards.TryAdd(board, await GetActivePlayers(board.Id)))
                {
                    throw new Exception("The board already exists in Boards.");
                }
            }
        }

        private async Task<List<Player>> GetActivePlayers(int boardId)
        {
            var boardStates = await _ludoApi.GetBoardStatesByBoard(boardId);
            var playerIds = boardStates
                .GroupBy(state => state.PlayerId)
                .Select(group => group.Key)
                .ToList();
            List<Player> players = new();
            foreach (var id in playerIds)
            {
                players.Add(await _ludoApi.GetPlayer(id));
            }
            return players;
        }

        public string GetPlayersString(int boardId)
        {
            var players = Boards.First(board => board.Key.Id == boardId).Value;
            string result = "";
            for (int i = 0; i < players.Count; i++)
            {
                result += players[i].Name;
                if (i < players.Count - 1)
                {
                    result += ", ";
                }
            }
            return result;
        }
    }
}

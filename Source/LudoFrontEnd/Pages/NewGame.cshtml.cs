using LudoApi.Controllers;
using LudoEngine.Database;
using LudoEngine.Engine;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LudoFrontEnd.Pages
{
   
    public class NewGameModel : PageModel
    {

        private DbContext _context;
        private PlayersController _controller;


        //public DbPlayer player { get; set; }
        [BindProperty]
        public List<Player> Players { get; set; }       
        // public int NumberOfPlayers { get; set; }
        public int FirstPlayerId { get; private set; }
        [BindProperty(SupportsGet = true)]
        public Player Winner { get; set; }
        
       

        //public async void OnPost()
        //{
        //    _context = new LudoContext();
        //    _controller= new PlayersController((LudoContext)_context);

        //   await _controller.PostDbPlayer(player);            
        //}


        public async void OnPost()
        {
            _context = new LudoContext();
            _controller = new PlayersController((LudoContext)_context);
            List<DbPlayer> dbPlayers = new List<DbPlayer>();

            foreach (var item in Players)
            {
                dbPlayers.Add(Convert(item));
            }            
            var x = RollForFirstPlayerPrompt(Players);
            Winner = x;

            foreach (var item in dbPlayers)
            {
                await _controller.PostDbPlayer(item);
            }
        }
        private DbPlayer Convert(Player player)
        {

            DbPlayer db = new DbPlayer() 
            {
                Name=player.Name,
                ColorId=player.ColorId
            };
            return db;
        }

        private Player RollForFirstPlayerPrompt(List<Player> players)
        {
            
                Dictionary<Player, int> playerRolls = new();

                foreach (Player player in players)
                {
                   
                    int roll = Die.Roll();                 
                    playerRolls.Add(player, roll);
                   
                }                

                List<Player> maxRollPlayers = GetMaxRollPlayers(playerRolls);

                if (maxRollPlayers.Count == 1)
                {
                    Player winner = maxRollPlayers[0];                    
                    FirstPlayerId = winner.Id;
                    Winner = winner;
                    return winner;
                }
                return RollForFirstPlayerPrompt(maxRollPlayers);            
        }
        
        private List<Player> GetMaxRollPlayers(Dictionary<Player, int> playerRolls)
        {
            int maxRoll = GetMaxRoll(playerRolls);
            return playerRolls
                .Where(playerRoll => playerRoll.Value == maxRoll)
                .Select(playerRoll => playerRoll.Key)
                .ToList();
        }
        private static int GetMaxRoll(Dictionary<Player, int> playerRolls)
        {
            return playerRolls.Max(playerRoll => playerRoll.Value);
        }

    }
}


//public async Task NewGame()
//{
//    board = new Board();
//    BoardId = await DbQuery.AddDbBoard();
//    int numberOfPlayers = GetNumberOfPlayers();

//    for (int i = 0; i < numberOfPlayers; i++)
//    {
//        Player player = await GetPlayerDetails(i);

//        // We do player.Id in order to Add the player and Add that player with that Id to the AddPlayerToBoard()
//        player.Id = await DbQuery.AddPlayer(player);
//        AddPlayerToBoard(player);
//    }

//    Player activePlayer = RollForFirstPlayer();

//    await board.StartGame(activePlayer.Id, BoardId);
//    await PlayBoard(activePlayer);

//    GameOverScreen();
//}
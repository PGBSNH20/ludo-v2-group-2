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
        private PlayersController _playerController;
        private BoardsController _boardController;



        [BindProperty]
        public List<Player> Players { get; set; }             
        public int FirstPlayerId { get; private set; }
        public int BoardId { get; private set; }
        public int NumberOfPlayers { get; set; }

        [BindProperty(SupportsGet = true)]
        public Player Winner { get; set; }

        Board board = new Board();


        public async void OnPost()
        {
            
            _context = new LudoContext();
            _playerController = new PlayersController((LudoContext)_context);
           
            List<DbPlayer> dbPlayers = new List<DbPlayer>();
            board = new Board();
            BoardId =  await AddDbBoard();

            foreach (var item in Players)
            {
                dbPlayers.Add(Convert(item));
            }            
            var x = RollForFirstPlayerPrompt(Players);
            Winner = x;

            foreach (var item in dbPlayers)
            {
                await _playerController.PostDbPlayer(item);
               
            }
            foreach (var item in Players)
            {
                AddPlayerToBoard(item);
            }

            await board.StartGame(Winner.Id, BoardId);

           // await PlayBoard(Winner);
        }
        private void AddPlayerToBoard(Player player)
        {
            board.AddPlayer(player);
        }
        private  async Task<int> AddDbBoard()
        {           
            var lastTimePlayed = DateTime.Now;
            DbBoard dbBoard = new DbBoard()
            {
                IsFinished = false,
                LastTimePlayed = lastTimePlayed
            };
            _boardController = new BoardsController((LudoContext)_context);
            await _boardController.PostDbBoard(dbBoard);
            return dbBoard.Id;
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
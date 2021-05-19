using LudoApi.Controllers;
using LudoEngine.Database;
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

        

        [BindProperty]
        public DbPlayer player { get; set; }


        public async void OnPost()
        {
            _context = new LudoContext();
            _controller= new PlayersController((LudoContext)_context);
            DbPlayer player = new DbPlayer();
            player = this.player;
           await _controller.PostDbPlayer(player);            
        }
    }
}

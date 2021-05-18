using LudoApi.Controllers;
using LudoEngine.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
       
        PlayersController _context = new PlayersController(PlayersController._context);
        public DbPlayer postPlayer = new DbPlayer();
        

        public IActionResult OnPost()
        {
            if (ModelState.IsValid==false)
            {
                return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}

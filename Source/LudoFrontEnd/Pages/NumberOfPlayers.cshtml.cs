using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LudoFrontEnd.Pages
{
    public class NumberOfPlayersModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int NumberPlayers { get; set; }

        [BindProperty(SupportsGet = true)]
        public int NumberPlayers1 { get; set; }
        public int GetNumberOfPlayers()
        {
            int number = NumberPlayers;
            return number;
        }

        public int GetNumberOfPlayers1()
        {
            int number = NumberPlayers1;
            return number;
        }
    }
}

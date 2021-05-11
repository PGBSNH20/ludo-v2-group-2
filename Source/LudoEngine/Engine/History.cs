using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine.Engine
{
    public class History
    {
        // Game: 235 - LastDayPlayed - 1. name, 2. name, ...
        public int GameId { get; set; }
        public DateTime LastTimePlayed { get; set; }
        public Dictionary<int, string> Placements { get; set; }
    }
}

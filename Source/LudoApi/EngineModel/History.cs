using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine.EngineModel
{
    public class History
    {
        public int GameId { get; set; }
        public DateTime LastTimePlayed { get; set; }
        public Dictionary<int, string> Placements { get; set; }
    }
}

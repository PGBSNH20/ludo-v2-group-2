using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine.Engine
{
    public class BoardData
    {
        public int BoardId { get; set; }
        public DateTime LastTimePlayed { get; set; }
        public List<string> PlayerNames { get; set; }
    }
}

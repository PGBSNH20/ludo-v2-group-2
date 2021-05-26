using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoFrontEnd.Api
{
    public class Board
    {
        public int Id { get; set; }
        public DateTime LastTimePlayed { get; set; }
        public bool IsFinished { get; set; }
        public int activePlayerId { get; set; }
    }
}

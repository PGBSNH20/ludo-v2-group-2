using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoFrontEnd.Api
{
    public class LudoBoardState
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int BoardId { get; set; }
        public int PieceNumber { get; set; }
        public int PiecePosition { get; set; }
        public bool IsInSafeZone { get; set; }
        public bool IsInBase { get; set; }
    }
}

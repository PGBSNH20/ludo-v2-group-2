using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoFrontEnd.Api
{
    public class Winner
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int BoardId { get; set; }
        public int Placement { get; set; }
    }
}

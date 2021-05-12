using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoApi.DTOs
{
    public class BoardDTO
    {
        public int Id { get; set; }
        public DateTime LastTimePlayed { get; set; }
        public bool IsFinished { get; set; }
    }
}

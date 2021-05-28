using LudoApi.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoApi.DTOs
{
    public class WinnerDTO
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int BoardId { get; set; }
        public int Placement { get; set; }
    }
}

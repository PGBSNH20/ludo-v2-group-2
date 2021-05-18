using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LudoApi.DTOs
{
    public class BoardStateDTO
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

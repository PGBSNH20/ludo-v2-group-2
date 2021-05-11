using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine.Database
{
    public class DbBoardState
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Players")]
        public int PlayerId { get; set; }
        public virtual DbPlayer Player { get; set; }

        [ForeignKey("Boards")]
        public int BoardId { get; set; }
        public virtual DbBoard Board { get; set; }

        // Min 1, Max 4 check EF code first constraint
        [Range(1, 4)]
        public int PieceNumber { get; set; }
        public int PiecePosition { get; set; }
        public bool IsInSafeZone { get; set; }
        public bool IsInBase { get; set; }
    }
}

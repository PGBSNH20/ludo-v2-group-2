using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine.Database
{
    public class DbWinner
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Players")]
        public int PlayerId { get; set; }
        public virtual DbPlayer Player { get; set; }

        [ForeignKey("Boards")]
        public int BoardId { get; set; }
        public virtual DbBoard Board { get; set; }

        public int Placement { get; set; }
    }
}

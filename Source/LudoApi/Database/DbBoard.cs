using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoApi.Database
{
    public class DbBoard
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastTimePlayed { get; set; }
        [Required]
        public bool IsFinished { get; set; }

        [ForeignKey("Players")]
        public int activePlayerId { get; set; }
    }
}

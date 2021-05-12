using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine.Database
{
    public class DbBoard
    {
        [Key]
        public int Id { get; set; }
        public DateTime LastTimePlayed { get; set; }
        public bool IsFinished { get; set; }
    }
}

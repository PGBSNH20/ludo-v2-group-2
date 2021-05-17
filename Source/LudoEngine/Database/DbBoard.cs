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
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastTimePlayed { get; set; }
        [Required]
        public bool IsFinished { get; set; }
    }
}

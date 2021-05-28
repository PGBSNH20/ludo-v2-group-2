using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoApi.Database
{
    public class DbPlayer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Colors")]
        public int ColorId { get; set; }
        public virtual DbColor Color { get; set; }
    }
}

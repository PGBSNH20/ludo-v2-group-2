using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoApi.Database
{
    public class DbColor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(6)]
        public string ColorCode { get; set; }
    }
}

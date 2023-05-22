using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAPI.Models
{
    public class TPERSON : TBASE
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int USERID { get; set; }
        [Required]
        public string DISPLAYNAME { get; set; }
    }
}

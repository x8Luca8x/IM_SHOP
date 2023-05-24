using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAPI.Models
{
    public class TIMAGE
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int ENTITYID { get; set; }
        [Required]
        public string ENTITYTYPE { get; set; }
        [Required]
        public string NAME { get; set; }
        [Required]
        public string TYPE { get; set; }
        [Required]
        public byte[] DATA { get; set; }
    }
}

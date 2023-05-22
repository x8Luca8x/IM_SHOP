using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAPI.Models
{
    public class TBASE
    {
        [Required]
        public DateTime CREATED { get; set; }

        [Required]
        public DateTime CHANGED { get; set; }
    }
}

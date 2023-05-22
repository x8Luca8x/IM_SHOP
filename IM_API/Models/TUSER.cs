using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAPI.Models
{
    public class TUSER : TUSER_V
    {
        [Required]
        public string PASSWORD { get; set; } = string.Empty;
    }
}

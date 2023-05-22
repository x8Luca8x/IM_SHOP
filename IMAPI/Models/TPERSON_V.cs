using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAPI.Models
{
    public class TPERSON_V : TPERSON
    {
        public string? EMAIL { get; set; }
        public string? FIRSTNAME { get; set; }
        public string? LASTNAME { get; set; }
        public DateTime? BIRTHDATE { get; set; }
        public string? ROLE { get; set; }
    }
}

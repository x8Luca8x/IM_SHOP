using IMAPI;
using System.ComponentModel.DataAnnotations;

namespace IM_API.Models
{
    public class TLOGIN
    {
        [Required]
        public string USERNAME { get; set; }
        [Required]
        public string PASSWORD { get; set; }
        [Required]
        public bool CAN_EXPIRE { get; set; } = true;
        [Required]
        public DateTime EXPIRES { get; set; }
    }
}

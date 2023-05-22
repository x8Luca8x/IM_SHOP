using System.ComponentModel.DataAnnotations;

namespace IM_API.Models
{
    public class TREGISTER
    {
        [Required]
        public string USERNAME { get; set; }
        [Required]
        public string EMAIL { get; set; }
        [Required]
        public string PASSWORD { get; set; }
        [Required]
        public string PASSWORD_CONFIRM { get; set; }
        [Required]
        public string FIRSTNAME { get; set; }
        [Required]
        public string LASTNAME { get; set; }
        [Required]
        public DateTime BIRTHDATE { get; set; }
    }
}

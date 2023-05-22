using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IM_API.Models
{
    public class TUSEROPTIONS : TBASE
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int USERID { get; set; }
        [Required]
        [DefaultValue(true)]
        public bool SHOW_EMAIL { get; set; } = true;
        [Required]
        [DefaultValue(true)]
        public bool SHOW_BIRTHDATE { get; set; } = true;
        [Required]
        [DefaultValue(true)]
        public bool SHOW_FIRSTNAME { get; set; } = true;
        [Required]
        [DefaultValue(true)]
        public bool SHOW_LASTNAME { get; set; } = true;
        [Required]
        [DefaultValue(false)]
        public bool SHOW_ROLE { get; set; } = false;
        [Required]
        [DefaultValue(true)]
        public bool SHOW_CREATED { get; set; } = true;
        [Required]
        [DefaultValue(false)]
        public bool SHOW_CHANGED { get; set; } = false;
    }
}

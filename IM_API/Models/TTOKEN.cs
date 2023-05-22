using System.ComponentModel.DataAnnotations;

namespace IM_API.Models
{
    public class TTOKEN : TBASE
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string TOKEN { get; set; }
        [Required]
        public int USERID { get; set; }
        [Required]
        public DateTime CREATED { get; set; }
        [Required]
        public bool CAN_EXPIRE { get; set; } = true;
        [Required]
        public DateTime EXPIRES { get; set; }
        [Required]
        public string DEVICE_NAME { get; set; }
        [Required]
        public string DEVICE_OS { get; set; }
        [Required]
        public string DEVICE_APP { get; set; }
    }
}

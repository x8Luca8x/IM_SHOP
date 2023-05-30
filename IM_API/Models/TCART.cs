using System.ComponentModel.DataAnnotations;

namespace IM_API.Models
{
    public class TCART : TBASE
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int USERID { get; set; }
    }
}

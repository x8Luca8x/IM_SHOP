using System.ComponentModel.DataAnnotations;

namespace IM_API.Models
{
    public class TCARTARTICLE : TBASE
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int CARTID { get; set; }
        [Required]
        public int ARTICLEID { get; set; }
        [Required]
        public int QUANTITY { get; set; } = 1;
    }
}

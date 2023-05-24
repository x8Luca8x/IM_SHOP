using System.ComponentModel.DataAnnotations;

namespace IM_API.Models
{
    public class TCURRENCY
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string NAME { get; set; }
        [Required]
        public string SYMBOL { get; set; }
    }
}

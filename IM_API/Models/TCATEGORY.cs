using System.ComponentModel.DataAnnotations;

namespace IM_API.Models
{
    public class TCATEGORY
    {
        [Key]
        public int ID { get; set; }
        [Required]
        // 0 = Main Category, otherwise = Sub Category
        public int PARENTID { get; set; }
        [Required]
        public string NAME { get; set; }
        [Required]
        public string DESCRIPTION { get; set; }
    }
}

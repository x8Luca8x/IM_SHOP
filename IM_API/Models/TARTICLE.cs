using IMAPI.Attributes;
using System.ComponentModel.DataAnnotations;

namespace IM_API.Models
{
    public class TARTICLE : TBASE
    {
        [Key]
        [InitialOnly]
        public int ID { get; set; }
        [Required]
        [InitialOnly]
        // Person who created the article
        public int PERSONID { get; set; }
        [Required]
        public int CATEGORYID { get; set; }
        [Required]
        public int SUBCATEGORYID { get; set; }
        [Required]
        public int CURRENCYID { get; set; }
        [Required]
        public string TITLE { get; set; }
        [Required]
        public string DESCRIPTION { get; set; }
        [Required]
        public string PRICE { get; set; }
        [Required]
        public int QUANTITY { get; set; }
        [Required]
        public int VIEWS { get; set; }
    }
}

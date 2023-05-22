using System.ComponentModel.DataAnnotations;

namespace IM_API.Models
{
    public class TUSERUPDATE
    {
        [StringLength(50)]
        public string? USERNAME { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string? EMAIL { get; set; }

        [StringLength(50)]
        public string? FIRSTNAME { get; set; }

        [StringLength(50)]
        public string? LASTNAME { get; set; }

        public DateTime? BIRTHDATE { get; set; }
    }
}

using IMAPI.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAPI.Models
{
    public static class UserRoles
    {
        public const string User = "USER";
        public const string Admin = "ADMIN";
    }

    public class TUSER_V : TBASE
    {
        [Key]
        [InitialOnly]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string USERNAME { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string EMAIL { get; set; }

        [Required]
        [StringLength(50)]
        public string FIRSTNAME { get; set; }

        [Required]
        [StringLength(50)]
        public string LASTNAME { get; set; }

        [Required]
        [StringLength(50)]
        [DefaultValue(UserRoles.User)]
        public string ROLE { get; set; } = UserRoles.User;

        [Required]
        public bool VERIFIED { get; set; }

        [Required]
        public bool ACTIVE { get; set; }

        [Required]
        public DateTime BIRTHDATE { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace EP.IdentityIsolation.Infra.CrossCutting.Identity.Model
{
    public class UserInfoViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }
}

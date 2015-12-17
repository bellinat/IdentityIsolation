using System.ComponentModel.DataAnnotations;

namespace EP.IdentityIsolation.Infra.CrossCutting.Identity.Model
{
    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}

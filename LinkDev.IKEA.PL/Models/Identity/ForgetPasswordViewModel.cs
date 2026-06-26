using System.ComponentModel.DataAnnotations;

namespace LinkDev.IKEA.PL.Models.Identity
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}

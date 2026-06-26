using System.ComponentModel.DataAnnotations;

namespace LinkDev.IKEA.PL.Models.Identity
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage ="New password is required.")]
        [Display(Name ="New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;
        [Required(ErrorMessage ="Confirm new password is required.")]
        [Display(Name ="Confirm New Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace LinkDev.IKEA.PL.Models.Identity
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage ="First name is required")]
        [Display(Name ="First Name")]
        
        public required string FirstName { get; set; }
        [Required(ErrorMessage ="Last name is required")]
        [Display(Name ="Last Name")]
        public required string LastName { get; set; }

        [Required(ErrorMessage ="User name is required")]
        [Display(Name ="User name")]
        public required string UserName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm Password do not match")]
        public required string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}

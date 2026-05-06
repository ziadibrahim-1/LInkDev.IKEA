using LinkDev.IKEA.DAL.Common.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LinkDev.IKEA.PL.Models.Employee
{
    public class EmployeeCreateViewModel
    {
        [Required]
        [Display(Name ="First Name")]
        [MinLength(3,ErrorMessage ="Min length of First Name is 3 Chars")]
        [MaxLength(30 , ErrorMessage = "Max length of First Name is 30 Chars")]
        public  string FirstName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Last Name")]
        [MinLength(3, ErrorMessage = "Min length of Last Name is 3 Chars")]
        [MaxLength(30, ErrorMessage = "Max length of Last Name is 30 Chars")]
        public  string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public  string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [DataType(DataType.Currency)]
        [Range(0.01,1_000_000,ErrorMessage ="Salary Must be large than 0 and less than 1,000,000")]
        public decimal Salary { get; set; }
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,30}-[a-zA-Z]{4,30}-[a-zA-Z]{5,30}"
                            ,ErrorMessage ="Address must be like 123-Street-City-Country")]
        public string? Address { get; set; }
        [Display(Name ="Phone Number")]
        [Phone]
        public string? PhoneNumber { get; set; }
        [Display(Name ="hiring Date")]
        public DateOnly HireDate { get; set; }
        public Gender Gender { get; set; }
        [Display(Name ="Employee Type")]
        public EmployeeType EmployeeType { get; set; }
        [Display(Name ="Department")]
        public int? DepartmentId { get; set; }
        public  IEnumerable<SelectListItem>? Departments { get; set; }
    }
}

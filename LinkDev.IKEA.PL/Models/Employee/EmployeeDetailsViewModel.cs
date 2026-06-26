using LinkDev.IKEA.DAL.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace LinkDev.IKEA.PL.Models.Employee
{
    public class EmployeeDetailsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Display(Name = "Phone Number")]
        [Phone]
        public string? PhoneNumber { get; set; }
        public int? Age { get; set; }
        [Display(Name ="Gender")]
        public Gender Gender { get; set; }
        [Display(Name ="Gender")]
        public string? GenderName => Gender.ToString();
        [Display(Name ="Address")]
        public string? Address { get; set; }
        [Display(Name ="Hiring Date")]
        public string? FormattedHireDate { get; set; }

        [Display(Name= "Years Of Service")]
        public int YearsOfService { get; set; }
        [Display(Name ="Employee Type")]
        public EmployeeType EmployeeType { get; set; }
        [Display(Name ="Salary")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name ="Status")]
        public bool IsActive { get; set; }

        public string Status => IsActive ? "Active" : "Inactive";
        // Department Details
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }
        [Display(Name ="Department")]
        public string? DepartmentName { get; set; }
        [Display(Name ="Departmnet Code")]
        public string? DepartmentCode { get; set; }
        [Display(Name ="Department Description")]
        public string? DepartmentDescription { get; set; }
        [Display(Name ="Department Of")]
        public string? ManagerDepartmentName { get; set; }

        // Audit Fields
        [Display(Name ="Created By")]
        public string? CreatedBy { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name ="Last Modified By")]
        public string? LastModifiedBy { get; set; }
        [Display(Name ="Last Modefied Date")]
        public DateTime? LastModifiedDate { get; set; }







    }
}

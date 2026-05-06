using LinkDev.IKEA.DAL.Common.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace LinkDev.IKEA.PL.Models.Employee
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }

        public int? Age { get; set; }
        [Display(Name ="Hiring Date")]
        public string? FormattedHireDate { get; set; }

        public string? Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name ="Is Active")]
        public bool IsActive { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Display(Name ="Phone Number")]
        [Phone]
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public EmployeeType EmployeeType { get; set; }

        #region Addministration
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        #endregion

        public string? Department { get; set; }

    }
}

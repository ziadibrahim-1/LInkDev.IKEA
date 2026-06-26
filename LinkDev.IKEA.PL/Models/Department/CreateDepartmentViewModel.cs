using System.ComponentModel.DataAnnotations;

namespace LinkDev.IKEA.PL.Models.Department
{
    public class CreateDepartmentViewModel 
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        [Display(Name = "Date of Creation")]
        public DateOnly CreationDate { get; set; }
    }
}

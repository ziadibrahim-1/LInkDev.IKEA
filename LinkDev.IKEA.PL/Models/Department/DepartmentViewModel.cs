using System.ComponentModel.DataAnnotations;

namespace LinkDev.IKEA.PL.Models.Department
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        [Display(Name = "Date of Creation")]
        public DateOnly CreationDate { get; set; }

    }
}

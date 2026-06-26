using System.ComponentModel.DataAnnotations;

namespace LinkDev.IKEA.PL.Models.Department
{
    public class DepartmentDetailsViewMOdel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Code { get; set; }
        [Display(Name = "Date of Creation")]
        public DateOnly CreationDate { get; set; }

        #region Audit Fields
        [Display(Name ="Created By")]
        public required string CreatedBy { get; set; }
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }
        [Display(Name= "Last Modified By")]
        public required string LastModifiedBy { get; set; }
        [Display(Name = "Last Modified On")]
        public DateTime LastModifiedOn { get; set; }
        #endregion

    }
}

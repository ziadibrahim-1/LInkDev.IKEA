using LinkDev.IKEA.DAL.Common.Entites;
using LinkDev.IKEA.DAL.Entities.Employees;

namespace LinkDev.IKEA.DAL.Entities.Departments
{
    public class Department : BaseAuditableEntity<int>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Code { get; set; }

        public DateOnly CreationDate { get; set; }
        //Manger of the department
        public int? ManagerId { get; set; }
        public virtual Employee? Manager { get; set; }

        //Navigation Properties work relationship
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}

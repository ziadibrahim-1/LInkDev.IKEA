using LinkDev.IKEA.DAL.Common.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Entities.Department
{
    public class Department : BaseAuditableEntity<int>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Code { get; set; }

        public DateOnly CreationDate { get; set; }
    }
}

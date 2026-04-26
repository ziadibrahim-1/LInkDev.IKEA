using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Common.Entites
{
    internal class BaseAuditableEntity<Tkey> : BaseEntity<Tkey>
        where Tkey : IEquatable <Tkey>
    {
        public required string CreatedBy { get; set; }
        public DateTime CreatedOn  { get; set; }
        public required string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}

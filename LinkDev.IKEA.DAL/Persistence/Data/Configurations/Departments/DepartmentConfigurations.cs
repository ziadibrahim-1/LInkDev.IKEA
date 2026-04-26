using LinkDev.IKEA.DAL.Common.Entites;
using LinkDev.IKEA.DAL.Entities.Department;
using LinkDev.IKEA.DAL.Persistence.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkDev.IKEA.DAL.Persistence.Data.Configurations.Departments
{
    internal class DepartmentConfigurations : BaseAuditableEntityConfigurations<int , Department>
    {
        public override void Configure(EntityTypeBuilder<Department> builder)
        {
            base.Configure(builder);
            builder.Property(D => D.Id).UseIdentityColumn(10, 10);
            builder.Property(D => D.Name).HasColumnType("varchar(100)");
            builder.Property(D => D.Description).HasColumnType("varchar(200)");
            builder.Property(D => D.Code).HasColumnType("varchar(10)");
        }
    }
}

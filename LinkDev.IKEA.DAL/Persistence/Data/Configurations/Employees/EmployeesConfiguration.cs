using LinkDev.IKEA.DAL.Common.Enums;
using LinkDev.IKEA.DAL.Entities.Employees;
using LinkDev.IKEA.DAL.Persistence.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace LinkDev.IKEA.DAL.Persistence.Data.Configurations.Employees
{   
    public class EmployeesConfiguration : BaseAuditableEntityConfigurations<int, Employee>
    {
        public override void Configure(EntityTypeBuilder<Employee> builder)
        {
            base.Configure(builder);
            builder.Property(E => E.Id).UseIdentityColumn(1, 1);
            builder.Property(E => E.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(E => E.LastName).IsRequired().HasMaxLength(50);
            builder.Property(E => E.Email).HasMaxLength(100);
            builder.Property(E => E.Address).HasMaxLength(200);
            builder.Property(E => E.gender).HasConversion(
                (gender) => gender.ToString(),
                (gender) => Enum.Parse<Gender>(gender)
            );

            builder.HasOne(E=>E.Department)
                .WithMany(D=>D.Employees)
                .HasForeignKey(E=>E.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(E => E.EmployeeType).HasConversion(
                (employeeType) => employeeType.ToString(),
                (employeeType) => Enum.Parse<EmployeeType>(employeeType)
            );
        }
    }
}
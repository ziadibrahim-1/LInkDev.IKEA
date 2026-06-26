using LinkDev.IKEA.DAL.Common.Enums;

namespace LinkDev.IKEA.BLL.Models.Employees
{
    public record EmployeeDto(int Id, string? DepartmentName,string FirstName, string LastName,int? Age,
        string Email, string? PhoneNumber, string? Address,
        Decimal Salary, bool IsActive, DateOnly HireDate,
        Gender Gender, EmployeeType EmployeeType, int? DepartmentId,
        string? CreatedBy, DateTime CreatedOn, string? LastModifiedBy,
        DateTime? LastModifiedOn)
    {
        public string FullName => $"{FirstName} {LastName}";
        public string FormattedHIreDate => HireDate.ToString("MMMM d,yyyy");
    };
}

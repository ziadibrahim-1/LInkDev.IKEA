using LinkDev.IKEA.DAL.Common.Enums;

namespace LinkDev.IKEA.BLL.Models.Employees
{
    public record CreateEmployeeDto
    (
        string FirstName,
        string LastName,
        string Email,
        string? PhoneNumber,
        string? Address,
        decimal Salary,
        DateOnly HireDate,
        Gender Gender,
        EmployeeType EmployeeType,
        int? DepartmentId
    );
}

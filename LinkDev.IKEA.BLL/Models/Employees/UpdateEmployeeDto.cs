using LinkDev.IKEA.DAL.Common.Enums;

namespace LinkDev.IKEA.BLL.Models.Employees
{
    public record UpdateEmployeeDto(
        int Id,
        string FirstName,
        string LastName,
        string? Address,
        decimal Salary,
        string Email,
        string? PhoneNumber,
        bool IsActive,
        Gender Gender,
        EmployeeType EmployeeType,
        int? DepartmentId
        );
    
}

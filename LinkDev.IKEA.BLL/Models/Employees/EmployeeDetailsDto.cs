using LinkDev.IKEA.BLL.Models.Departments;

namespace LinkDev.IKEA.BLL.Models.Employees
{
    public record EmployeeDetailsDto
    (
        EmployeeDto Employee,
        DepartmentDto Department,
        int YearsOfExperience
    );
}

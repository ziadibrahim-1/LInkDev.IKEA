using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.DAL.Persistence.Common;

namespace LinkDev.IKEA.BLL.Services.Employees
{
    public interface IEmployeeService
    {
        EmployeeDto? GetEmployeeById(int id);

        EmployeeDetailsDto? EmployeeDetailsDto(int id);

        PaginatedResult<EmployeeDto> GetEmployees(QueryParamters paramters);

        int CreateEmployee(CreateEmployeeDto employeeDto);
        int UpdateEmployee(UpdateEmployeeDto employeeDto);
        bool ChangeEmployeeStatus(int id, bool IsActive);
        int DeleteEmployee(int id);
    }
}

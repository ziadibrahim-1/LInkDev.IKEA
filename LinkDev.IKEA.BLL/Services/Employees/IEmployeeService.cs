using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.DAL.Persistence.Common;

namespace LinkDev.IKEA.BLL.Services.Employees
{
    public interface IEmployeeService
    {
        EmployeeDto? GetEmployeeById(int id);

        EmployeeDetailsDto? EmployeeDetailsDto(int id);

        PaginatedResult<EmployeeDto> GetEmployees(QueryParamters paramters);

        void CreateEmployee(CreateEmployeeDto employeeDto);
        void UpdateEmployee(UpdateEmployeeDto employeeDto);
        void DeleteEmployee(int id);
    }
}

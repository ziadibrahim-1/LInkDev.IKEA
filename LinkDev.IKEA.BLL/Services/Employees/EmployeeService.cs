using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.DAL.Contracts;
using LinkDev.IKEA.DAL.Entities.Employees;
using LinkDev.IKEA.DAL.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.IKEA.BLL.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public EmployeeDto? GetEmployeeById(int id)
        {
            var employee = _unitOfWork.Employees.Get(id);
            if (employee is null)
                return null;

            var employeeDto = new EmployeeDto(employee.Id, employee.FirstName, employee.LastName, employee.Age, employee.Email, employee.PhoneNumber, employee.Address, employee.Salary, employee.IsActive, employee.HireDate, employee.gender, employee.EmployeeType, employee.DepartmentId, employee.CreatedBy, employee.CreatedOn, employee.LastModifiedBy, employee.LastModifiedOn);
            return employeeDto;
        }

        public EmployeeDetailsDto? EmployeeDetailsDto(int id)
        {
            var employee = _unitOfWork.Employees.Get(
                filter: E=> E.Id == id,
                include: E => E.Include(E => E.Department)
                );

            if (employee is null)
                return null;

            var employeeDto = new EmployeeDto(employee.Id, employee.FirstName, employee.LastName, employee.Age, employee.Email, employee.PhoneNumber, employee.Address, employee.Salary, employee.IsActive, employee.HireDate, employee.gender, employee.EmployeeType, employee.DepartmentId, employee.CreatedBy, employee.CreatedOn, employee.LastModifiedBy, employee.LastModifiedOn);

            DepartmentDto DepartmentDto = default!;
            if (employee.Department != null)
                DepartmentDto = new DepartmentDto(employee.Department.Id ,employee.Department.Name, employee.Department.Code, employee.Department.CreationDate);

            var yearsOfExperience = CalculateYearsOfExperience(employee.HireDate, DateOnly.FromDateTime(DateTime.Now));

            var employeeDetailsDto = new EmployeeDetailsDto(employeeDto , DepartmentDto , yearsOfExperience);
            return employeeDetailsDto;
        }
        public PaginatedResult<EmployeeDto> GetEmployees(QueryParamters paramters)
        {
            if(paramters.PageIndex < 1)
                paramters.PageIndex = 1;
            if(paramters.PageSize < 1)
                paramters.PageSize = 10;
            if(paramters.PageSize > 20)
                paramters.PageSize = 20;

            var result = _unitOfWork.Employees.GetAll(
                paramters: paramters,
                include: E => E.Include(E => E.Department)
                //filter: E => E.FirstName.Contains(paramters.SearchTerm)
                //orderBy: E => E.OrderBy(E => E.Age),
                );
            var resultDto = new PaginatedResult<EmployeeDto>()
            {
                date = result.date.Select(employee => new EmployeeDto(employee.Id, employee.FirstName, employee.LastName, employee.Age, employee.Email, employee.PhoneNumber, employee.Address, employee.Salary, employee.IsActive, employee.HireDate, employee.gender, employee.EmployeeType, employee.DepartmentId, employee.CreatedBy, employee.CreatedOn, employee.LastModifiedBy, employee.LastModifiedOn)),
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
            };
            return resultDto;
        }

        public void CreateEmployee(CreateEmployeeDto employeeDto)
        {
            validateCreateEmployeeBusinissRules(employeeDto);

            var employee = new Employee()
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Address = employeeDto.Address,
                Age = DateTime.Now.Year - employeeDto.HireDate.Year,
                DepartmentId = employeeDto.DepartmentId,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                Salary = employeeDto.Salary,
                CreatedBy = "",
                LastModifiedBy = "",
            };
            employee.HireDate = DateOnly.FromDateTime(DateTime.Now);
            employee.IsActive = true;

            _unitOfWork.Employees.Add(employee);
            _unitOfWork.Complete();
        }


        public void UpdateEmployee(UpdateEmployeeDto employeeDto)
        {
            var ExistingEmployee = _unitOfWork.Employees.Get(employeeDto.Id);
            if (ExistingEmployee is null)
                return;
            validateUpdateEmployeeBusinissRules(employeeDto, ExistingEmployee);

            ExistingEmployee.FirstName = employeeDto.FirstName;
            ExistingEmployee.LastName = employeeDto.LastName;
            ExistingEmployee.Address = employeeDto.Address;
            ExistingEmployee.DepartmentId = employeeDto.DepartmentId;
            ExistingEmployee.Email = employeeDto.Email;
            ExistingEmployee.PhoneNumber = employeeDto.PhoneNumber;
            ExistingEmployee.gender = employeeDto.Gender;
            ExistingEmployee.EmployeeType = employeeDto.EmployeeType;
            ExistingEmployee.IsActive = employeeDto.IsActive;

            _unitOfWork.Employees.Update(ExistingEmployee);
            _unitOfWork.Complete();
        }

       
        public void DeleteEmployee(int id)
        {
            _unitOfWork.Employees.Delete(id);
            _unitOfWork.Complete();  
        }

        #region Helper Methods
        private void validateCreateEmployeeBusinissRules(CreateEmployeeDto employeeDto)
        {
            if(employeeDto.DepartmentId.HasValue)
            {

                var department  = _unitOfWork.Departments.Get(employeeDto.DepartmentId.Value);

                if(department is null)
                    throw new Exception($"Department with ID {employeeDto.DepartmentId} does not exist.");

                int MinAge = 18;
                var age =  DateTime.Now.Year - employeeDto.HireDate.Year;
                if(age < MinAge)
                    throw new Exception($"Employee must be at least {MinAge} years old.");

                if(employeeDto.Salary < 5_000)
                    throw new Exception("Salary must be at least 5000.");
            }
        }

        private void validateUpdateEmployeeBusinissRules(UpdateEmployeeDto employeeDto , Employee ExistingEmployee)
        {
            if (employeeDto.DepartmentId.HasValue)
            {
                var department = _unitOfWork.Departments.Get(employeeDto.DepartmentId.Value);
                if (department is null)
                    throw new Exception($"Department with ID {employeeDto.DepartmentId} does not exist.");

                var expectedSalary = ExistingEmployee.Salary * 1.1m;

                if (employeeDto.Salary < expectedSalary)
                    throw new Exception("Salary must be at least 10% higher than the current salary.");
            }
        }
        private static int CalculateYearsOfExperience(DateOnly hireDate, DateOnly referenceDate)
        {
            var years = referenceDate.Year - hireDate.Year;
            // If reference date is before the anniversary in the reference year, subtract one year.
            if (referenceDate < hireDate.AddYears(years))
            {
                years--;
            }

            return Math.Max(0, years);
        }
        #endregion
    }
}

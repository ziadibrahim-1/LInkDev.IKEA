using AutoMapper;
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
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public EmployeeDto? GetEmployeeById(int id)
        {
            var employee = _unitOfWork.Employees.Get(id);
            if (employee is null)
                return null;

            //var employeeDto = new EmployeeDto(employee.Id,employee.Department?.Name ,employee.FirstName, employee.LastName, employee.Age, employee.Email??string.Empty, employee.PhoneNumber, employee.Address, employee.Salary, employee.IsActive, employee.HireDate, employee.Gender, employee.EmployeeType, employee.DepartmentId, employee.CreatedBy, employee.CreatedOn, employee.LastModifiedBy, employee.LastModifiedOn);
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return employeeDto;
        }

        public EmployeeDetailsDto? EmployeeDetailsDto(int id)
        {
            // i Want to modify quering data in REpository not Serverice
            var employee = _unitOfWork.Employees.Get(
                filter: E=> E.Id == id,
                include: E => E.Include(E => E.Department)
                );

            if (employee is null)
                return null;


            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            
            var DepartmentDto = _mapper.Map<DepartmentDto>(employee.Department);
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

            var result = _unitOfWork.Employees.GetAll(paramters: paramters);
            var employeeDto = _mapper.Map<IEnumerable<EmployeeDto>>(result.date);
            var resultDto = new PaginatedResult<EmployeeDto>()
            {
                date = employeeDto,
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
            };
            return resultDto;
        }

        public int CreateEmployee(CreateEmployeeDto employeeDto)
        {
            validateCreateEmployeeBusinissRules(employeeDto);

            var employee = _mapper.Map<Employee>(employeeDto);
            //employee.HireDate = DateOnly.FromDateTime(DateTime.Now);
            //employee.IsActive = true;

            _unitOfWork.Employees.Add(employee);
            return _unitOfWork.Complete();
        }


        public int UpdateEmployee(UpdateEmployeeDto employeeDto)
        {
            var ExistingEmployee = _unitOfWork.Employees.Get(employeeDto.Id);
            if (ExistingEmployee is null)
                return -1;
            validateUpdateEmployeeBusinissRules(employeeDto, ExistingEmployee);

            ExistingEmployee.FirstName = employeeDto.FirstName;
            ExistingEmployee.LastName = employeeDto.LastName;
            ExistingEmployee.Address = employeeDto.Address;
            ExistingEmployee.DepartmentId = employeeDto.DepartmentId;
            ExistingEmployee.Email = employeeDto.Email;
            ExistingEmployee.PhoneNumber = employeeDto.PhoneNumber;
            ExistingEmployee.Gender = employeeDto.Gender;
            ExistingEmployee.EmployeeType = employeeDto.EmployeeType;
            ExistingEmployee.IsActive = employeeDto.IsActive;

            _unitOfWork.Employees.Update(ExistingEmployee);
            return _unitOfWork.Complete();
        }

       
        public int DeleteEmployee(int id)
        {
            _unitOfWork.Employees.Delete(id);
            return _unitOfWork.Complete();  
        }

        #region Helper Methods
        private void validateCreateEmployeeBusinissRules(CreateEmployeeDto employeeDto)
        {
            if(employeeDto.DepartmentId.HasValue)
            {

                var department  = _unitOfWork.Departments.Get(employeeDto.DepartmentId.Value);

                if(department is null)
                    throw new Exception($"Department with ID {employeeDto.DepartmentId} does not exist.");

                //int MinAge = 18;
                //var age =  DateTime.Now.Year - employeeDto.HireDate.Year;
                //if(age < MinAge)
                //    throw new Exception($"Employee must be at least {MinAge} years old.");

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
                if(employeeDto.Salary != ExistingEmployee.Salary && employeeDto.Salary < expectedSalary)
                {
                    if (employeeDto.Salary < expectedSalary)
                        throw new Exception($"Salary must be at least 10% higher than the current salary. Expected: {expectedSalary}");
                }
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

        public bool ChangeEmployeeStatus(int id, bool IsActive)
        {
            var employee = _unitOfWork.Employees.Get(id);
            if(employee is null)
                return false;
            if(employee.IsActive == IsActive)
                throw new Exception($"Employee with ID {id} is already {(IsActive ? "active" : "inactive")}.");

            employee.IsActive = IsActive;
            _unitOfWork.Employees.Update(employee);
            _unitOfWork.Complete();
            return true;
        }
        #endregion
    }
}

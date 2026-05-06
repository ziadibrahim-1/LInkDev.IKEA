using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.BLL.Services.Departments;
using LinkDev.IKEA.BLL.Services.Employees;
using LinkDev.IKEA.DAL.Persistence.Common;
using LinkDev.IKEA.PL.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LinkDev.IKEA.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _EmployeeService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IWebHostEnvironment _environment;

        public EmployeeController(IEmployeeService EmployeeService, ILogger<EmployeeController> logger, IWebHostEnvironment environment)
        {
            _EmployeeService = EmployeeService;
            _logger = logger;
            _environment = environment;
        }
        [HttpGet] // GET: Employee/Index
        public IActionResult Index(int pageIndex = 1, int pageSize = 10)
        {
            var queryParamters = new QueryParamters
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var employees = _EmployeeService.GetEmployees(queryParamters);

            var model = new EmployeeListViewModel()
            {
                Employees = employees.date.Select(E => new EmployeeViewModel()
                {
                    Id = E.Id,
                    FullName = $"{E.FirstName} {E.LastName}",
                    Address = E.Address,
                    Age = E.Age,
                    CreatedBy = E.CreatedBy,
                    CreatedOn = E.CreatedOn,
                    Email = E.Email,
                    EmployeeType = E.EmployeeType,
                    Gender = E.Gender,
                    IsActive = E.IsActive,
                    FormattedHireDate = E.FormattedHIreDate,
                    ModifiedBy = E.LastModifiedBy,
                    ModifiedOn = E.LastModifiedOn,
                    PhoneNumber = E.PhoneNumber,
                    Salary = E.Salary,
                    Department = E.DepartmentName
                }),
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = employees.TotalCount
            };


            return View(model);
        }

        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = _EmployeeService.EmployeeDetailsDto(id.Value);
            if (employee is null)
                return NotFound();

            var model = new EmployeeDetailsViewModel()
            {
                Id = employee.Employee.Id,
                FirstName = employee.Employee.FirstName,
                LastName = employee.Employee.LastName,
                Address = employee.Employee.Address,
                Age = employee.Employee.Age,
                CreatedBy = employee.Employee.CreatedBy,
                CreatedDate = employee.Employee.CreatedOn,
                DepartmentCode = employee.Department.Code,
                DepartmentId = employee.Department.Id,
                DepartmentName = employee.Department.Name,
                Email = employee.Employee.Email,
                EmployeeType = employee.Employee.EmployeeType,
                FormattedHireDate = employee.Employee.FormattedHIreDate,
                Gender = employee.Employee.Gender,
                IsActive = employee.Employee.IsActive,
                LastModifiedBy = employee.Employee.LastModifiedBy,
                LastModifiedDate = employee.Employee.LastModifiedOn,
                PhoneNumber = employee.Employee.PhoneNumber,
                Salary = employee.Employee.Salary,
                YearsOfService = employee.YearsOfExperience,
                ManagerDepartmentName = employee.Department.Manager,
                DepartmentDescription = employee.Department.Description

            };
            return View(model);


        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new EmployeeCreateViewModel()
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel createViewModel)
        {
            if (!ModelState.IsValid)
                return View(createViewModel);

            var message = "Employee created successfully";
            try
            {
                var EmployeeToCreate = new CreateEmployeeDto(createViewModel.FirstName, createViewModel.LastName, createViewModel.Email, createViewModel.PhoneNumber, createViewModel.Address, createViewModel.Salary, createViewModel.HireDate, createViewModel.Gender, createViewModel.EmployeeType, createViewModel.DepartmentId);
                var created = _EmployeeService.CreateEmployee(EmployeeToCreate) > 0;
                if (!created)
                    message = $"Failed To Create Employee {EmployeeToCreate.FirstName} {EmployeeToCreate.LastName}";
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace!.ToString());
                if (_environment.IsDevelopment())
                    message = ex.Message;
                else
                    message = $"An error occurred while Creating the Employee";
            }
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id <= 0)
                return BadRequest();
            var employee = _EmployeeService.GetEmployeeById(id);
            if (employee is null)
                return NotFound();
            var model = new EmployeeEditViewModel()
            {
                Address = employee.Address,
                DepartmentId = employee.DepartmentId,
                EmployeeType = employee.EmployeeType,
                Email = employee.Email,
                FirstName = employee.FirstName,
                Gender = employee.Gender,
                Id = employee.Id,
                IsActive = employee.IsActive,
                LastName = employee.LastName,
                PhoneNumber = employee.PhoneNumber,
                Salary = employee.Salary,

            };
            TempData["Id"] = id;
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, EmployeeEditViewModel employeeEditViewModel)
        {

            if (((int?)TempData["Id"]) != id)
            {
                ModelState.AddModelError("Id", "Invalid Department Id");
                return View(employeeEditViewModel);
            }

            if (!ModelState.IsValid)
                return View(employeeEditViewModel);

            var message = "Employee Edit Successfuly";
            try
            {
                var employeeToCreate = new UpdateEmployeeDto(employeeEditViewModel.Id,
                      employeeEditViewModel.FirstName,
                      employeeEditViewModel.LastName, employeeEditViewModel.Address,
                      employeeEditViewModel.Salary, employeeEditViewModel.Email, employeeEditViewModel.PhoneNumber,
                      employeeEditViewModel.IsActive, employeeEditViewModel.Gender, employeeEditViewModel.EmployeeType,
                      employeeEditViewModel.DepartmentId
                      );

                var updated = _EmployeeService.UpdateEmployee(employeeToCreate) > 0;
                if (!updated)
                    message = $"Failed To Edit Employee {employeeToCreate.FirstName} {employeeToCreate.LastName}";

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex.StackTrace!.ToString());
                if (_environment.IsDevelopment())
                    message = ex.Message;
                else
                    message = $"An error occurred while Editing the Employee";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            var message = "Employee Deleted Successfully";
            try
            {
                var deleted = _EmployeeService.DeleteEmployee(id) > 0;
                if (!deleted)
                    message = $"Failed To Delete Employee with id {id}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace!.ToString());
                if (_environment.IsDevelopment())
                    message = ex.Message;
                else
                    message = $"An error occurred while deleting the Employee";
            }
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}
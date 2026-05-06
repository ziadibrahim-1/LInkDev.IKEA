using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.BLL.Services.Departments;
using LinkDev.IKEA.PL.Models.Department;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.IKEA.PL.Controllers
{
    // 
    // Inheritance Department Controller is a Controller
    // Compstion : DepartmentController has a IDepartmentService
    public class DepartmentController : Controller
    {
        private readonly ILogger<DepartmentController> _logger;
        #region Services
        private readonly IDepartmentService _departmentService;
        private readonly IWebHostEnvironment _environment;

        public DepartmentController(ILogger<DepartmentController> logger, IDepartmentService departmentService,IWebHostEnvironment environment)
        {
            _logger = logger;
            _departmentService = departmentService;
            _environment = environment;
        }
        #endregion

        public IActionResult Index()
        {

            var departments = _departmentService.GetDepartments();
            var departmentViewModels = departments.Select(d => new DepartmentViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Code = d.Code,
                CreationDate = d.CreationDate
            });
            return View(departmentViewModels);
        }

        [HttpGet]
        public IActionResult Details(int? id)  // Department/Details/id?
        {
            if (!id.HasValue)
                return BadRequest();
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department == null)
                return NotFound();

            var departmentDetailsDto = new DepartmentDetailsViewMOdel()
            {
                Code = department.Code,
                CreationDate = department.CreationDate,
                CreatedBy = department.CreatedBy,
                CreatedOn = department.CreatedOn,
                Description = department.Description,
                LastModifiedBy = department.LastModifiedBy,
                LastModifiedOn = department.LastModifiedOn,
                Name = department.Name
            };
            return View(departmentDetailsDto);

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentViewModel model)
        {
            string message = "Department Created Successfully";
            if(model != null)

            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                

                var DepartmentToCreate = new CreateDepartmentDto(model.Name?? string.Empty, model.Description, model.Code ?? string.Empty, model.CreationDate);

                var created = _departmentService.CreateDepartment(DepartmentToCreate) > 0;

                if (!created)
                    message = "Faild to create Department";



            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace!.ToString());
                if (_environment.IsDevelopment())
                    message = ex.Message;
                else
                    message = $"An error occurred while creating the Department";

            }
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));


        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var department = _departmentService.GetDepartmentById(id.Value);

            if (department == null)
                return NotFound();

            var departmentViewModel = new UpdateDepartmentViewModel()
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description ?? "",
                Code = department.Code,
                CreationDate = department.CreationDate
            };
            TempData["Id"] = id;
            return View(departmentViewModel);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int? id, UpdateDepartmentViewModel model)
        {
            string message = "Department Updated Successfully";
            try
            {
                if (!id.HasValue)
                    return BadRequest();

                if ((((int?)TempData["Id"]) != id))
                {
                    ModelState.AddModelError("Id", "Invalid Department Id");
                    return View(model);
                }

                if (!ModelState.IsValid)
                    return View(model);

                var DepartmentToUpdate = new UpdateDepartmentDto(id.Value, model.Name, model.Description, model.Code, model.CreationDate);
                var updated = _departmentService.UpdateDepartment(DepartmentToUpdate) > 0;
                if (!updated)
                    message = "Faild to update Department";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace!.ToString());
                if (_environment.IsDevelopment())
                    message = ex.Message;
                else
                    message = $"An error occurred while updating the Department";
            }
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            string message = "Department Deleted Successfully";
            try
            {

                var deleted = _departmentService.DeleteDepartment(id);
                if (!deleted)
                    message = "Faild to delete Department";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace!.ToString());
                if (_environment.IsDevelopment())
                    message = ex.Message;
                else
                    message = $"An error occurred while Deleting the Department";
            }
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}

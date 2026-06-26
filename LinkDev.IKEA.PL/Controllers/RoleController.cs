using LinkDev.IKEA.DAL.Entities.Identity;
using LinkDev.IKEA.PL.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinkDev.IKEA.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<RoleController> _logger;

        public RoleController(
            RoleManager<IdentityRole> roleManager ,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            ILogger<RoleController> logger)

        {
            _roleManager = roleManager;
            _userManager = userManager;
            _environment = environment;
            _logger = logger;
        }
        #region Index
        [HttpGet]
        public IActionResult Index(string SearchValue)
        {
            var RoleQuery = _roleManager.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(SearchValue))
                RoleQuery = RoleQuery.Where(r => r.Name != null && r.Name.ToLower().Contains(SearchValue.ToLower()));

            var roles = RoleQuery.Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name ?? string.Empty
            }).ToList();
            return View(roles);
        }
        #endregion
        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (id is null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();
            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty
            };
            return View(model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id is null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();
            var users = _userManager.Users.ToList();
            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty,
                Users = users.Select( u => new UserRoleViewModel
                {
                    UserId = u.Id,
                    UserName = u.UserName ?? string.Empty,
                    IsSelected = _userManager.IsInRoleAsync(u, role.Name ?? string.Empty).Result
                }).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id,RoleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (id != model.Id)
                return BadRequest();

            string message = "Role updated successfully.";

            try
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role is null)
                    return NotFound();
                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);
                foreach(var userRole in model.Users)
                {
                    var user = await _userManager.FindByIdAsync(userRole.UserId);
                    if (user is not null)
                    {
                        if (userRole.IsSelected && !await _userManager.IsInRoleAsync(user, role.Name ?? string.Empty))
                        {
                            await _userManager.AddToRoleAsync(user, role.Name ?? string.Empty);
                        }
                        else if (!userRole.IsSelected && await _userManager.IsInRoleAsync(user, role.Name ?? string.Empty))
                        {
                            await _userManager.RemoveFromRoleAsync(user, role.Name ?? string.Empty);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace!.ToString());
                if (_environment.IsDevelopment())
                    message = ex.Message;
                else
                    message = "An error occurred while updating the role.";
            }
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (id is null)
                return BadRequest();
            string message = "Role deleted successfully.";
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null)
                    return NotFound();
                var result = await _roleManager.DeleteAsync(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace!.ToString());
                if (_environment.IsDevelopment())
                    message = ex.Message;
                else
                    message = "An error occurred while deleting the role.";
            }
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            string message = "Role created successfully.";
            try
            {
                var role = new IdentityRole
                {
                    Name = model.Name
                };
                var result = await _roleManager.CreateAsync(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace!.ToString());
                if (_environment.IsDevelopment())
                    message = ex.Message;
                else
                    message = "An error occurred while creating the role.";
            }
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}

using LinkDev.IKEA.DAL.Entities.Identity;
using LinkDev.IKEA.DAL.Persistence.Common;
using LinkDev.IKEA.PL.Models.Identity;
using LinkDev.IKEA.PL.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LinkDev.IKEA.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManager, IWebHostEnvironment environment, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _environment = environment;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string searchTerm)
        {
            var userQuery = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                userQuery = userQuery.Where(u => u.Email!.ToLower().Contains(searchTerm.ToLower()));
            }
            
            var users = userQuery.Select(u => new UserViewModel()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email ?? string.Empty,
                Roles = new List<string>()
            }).ToList();
            
            // Populate roles for each user
            foreach (var user in users)
            {
                var foundUser = await _userManager.FindByIdAsync(user.Id);
                if (foundUser is not null)
                {
                    var roles = await _userManager.GetRolesAsync(foundUser);
                    user.Roles = roles.ToList();
                }
            }
            
            return View(users);
        }

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            if (id is null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();
            var roles = await _userManager.GetRolesAsync(user);
            var model = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Roles = roles.ToList()
            };
            return View(model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id is null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();
            var roles = await _userManager.GetRolesAsync(user);
            var model = new UserEditViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Roles = roles.ToList()
            };
            TempData["UserId"] = id;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserEditViewModel model)
        {
            if (id is null)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            // Avoid InvalidCastException: TempData may contain non-string types (e.g. Guid).
            var tempUserIdObj = TempData["UserId"];
            var tempUserId = tempUserIdObj?.ToString();
            if (tempUserId != id)
                return BadRequest();
            string message = "User updated successfully.";


            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                    return NotFound();

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace!.ToString());
                message = "An error occurred while updating the user.";
            }
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (id is null)
                return BadRequest();
            string message = "User deleted successfully.";
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                    return NotFound();
                var result = await _userManager.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace!.ToString());
                message = "An error occurred while deleting the user.";
            }
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}

using LinkDev.IKEA.DAL.Common.JsonConvertors;
using LinkDev.IKEA.DAL.Contracts;
using LinkDev.IKEA.DAL.Entities.Departments;
using LinkDev.IKEA.DAL.Entities.Employees;
using LinkDev.IKEA.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LinkDev.IKEA.DAL.Persistence.Data.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext dbContext,
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            if (_dbContext.Database.GetPendingMigrations().Any())
                _dbContext.Database.Migrate();
            
        }

        public async Task Seed()
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
                Converters =
                {
                    new DateOnlyJsonConvertor(),
                    new GenderJsonConvertor(),
                    new EmployeeTypeJsonConvertor()
                }
            };
            if (!_dbContext.Departments.Any())
            {
                
                var DeparmentOption = File.ReadAllText("../LinkDev.IKEA.DAL/Persistence/Data/Seeds/departments.json");
                var Departments = JsonSerializer.Deserialize<List<Department>>(DeparmentOption);
                if (Departments != null && Departments.Count > 0)
                {
                    await _dbContext.Departments.AddRangeAsync(Departments);
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (!_dbContext.Employees.Any())
            {

                var EmployeeOptions = File.ReadAllText("../LinkDev.IKEA.DAL/Persistence/Data/Seeds/employees.json");
                var Employees = JsonSerializer.Deserialize<List<Employee>>(EmployeeOptions, serializeOptions);
                if (Employees != null && Employees.Count > 0)
                {
                    await _dbContext.Employees.AddRangeAsync(Employees);
                    await _dbContext.SaveChangesAsync();
                }
            }
           await SeedIdentityAsync();
        }

        private async Task SeedIdentityAsync()
        {
            // Create Role
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Create Admin User
            var email = "admin@gmail.com";

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "Admin"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}

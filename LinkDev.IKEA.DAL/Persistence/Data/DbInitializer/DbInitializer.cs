using LinkDev.IKEA.DAL.Common.JsonConvertors;
using LinkDev.IKEA.DAL.Contracts;
using LinkDev.IKEA.DAL.Entities.Departments;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LinkDev.IKEA.DAL.Persistence.Data.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;

        public DbInitializer(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Initialize()
        {
            if (_dbContext.Database.GetPendingMigrations().Any())
                _dbContext.Database.Migrate();
            
        }

        public void Seed()
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
                    _dbContext.Departments.AddRange(Departments);
                    _dbContext.SaveChanges();
                }
            }

            if (!_dbContext.Employees.Any())
            {

                var EmployeeOptions = File.ReadAllText("../LinkDev.IKEA.DAL/Persistence/Data/Seeds/employees.json");
                var Employees = JsonSerializer.Deserialize<List<Department>>(EmployeeOptions, serializeOptions);
                if (Employees != null && Employees.Count > 0)
                {
                    _dbContext.Departments.AddRange(Employees);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}

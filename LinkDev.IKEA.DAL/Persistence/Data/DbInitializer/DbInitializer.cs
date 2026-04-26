using LinkDev.IKEA.DAL.Contracts;
using LinkDev.IKEA.DAL.Entities.Department;
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
        }
    }
}

using LinkDev.IKEA.DAL.Contracts.Repositories;
using LinkDev.IKEA.DAL.Entities.Department;
using LinkDev.IKEA.DAL.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Persistence.Repositories
{
    internal class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Department? Get(int id)
        {
            var department = _dbContext.Departments.Find(id);
            //// var department = _dbContext.Departments.Local.FirstOrDefault(d => d.Id == id);
            //// 
            //// if (department == null)
            ////     return _dbContext.Departments.FirstOrDefault(d => d.Id == id);

            return department;

        }

        public IEnumerable<Department> GetAll(bool WithTracking = false)
        {
            if (!WithTracking)
                return _dbContext.Departments.AsNoTracking();

            return _dbContext.Departments;

        }
        public int Add(Department department)
        {
            var entry = _dbContext.Departments.Add(department);
            return _dbContext.SaveChanges();
        }

        public int Update(Department department)
        {
            var entry = _dbContext.Departments.Update(department);
            return _dbContext.SaveChanges();
        }
        public int Delete(int id)
        {
            var department = _dbContext.Departments.Find(id);
            if (department is { })
            {
                var entry = _dbContext.Departments.Remove(department);
                return _dbContext.SaveChanges();
            }
            return 0;
        }


    }
}

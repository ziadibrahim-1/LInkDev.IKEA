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
        public void Add(Department department)
            => _dbContext.Departments.Add(department);
          
        

        public void Update(Department department)
            =>  _dbContext.Departments.Update(department);
          
        
        public void Delete(int id)
        {
            var department = _dbContext.Departments.Find(id);
            if (department is { })
                 _dbContext.Departments.Remove(department);
                
            

        }


    }
}

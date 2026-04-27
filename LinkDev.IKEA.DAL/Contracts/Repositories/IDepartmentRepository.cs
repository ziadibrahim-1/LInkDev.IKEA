using LinkDev.IKEA.DAL.Entities.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Contracts.Repositories
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll(bool WithTracking = false);
        Department? Get(int id);
        void Add(Department department);
        void Update(Department department);
        void Delete(int id);
    }
}

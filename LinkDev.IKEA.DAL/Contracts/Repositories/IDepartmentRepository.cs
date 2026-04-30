using LinkDev.IKEA.DAL.Entities.Departments;

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

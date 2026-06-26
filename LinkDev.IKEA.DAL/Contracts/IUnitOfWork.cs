using LinkDev.IKEA.DAL.Contracts.Repositories;

namespace LinkDev.IKEA.DAL.Contracts
{
    public interface IUnitOfWork
    {
        public IDepartmentRepository Departments { get; }
        public IEmployeeRepository Employees { get; }
        int Complete();
        void Dispose();
    }
}

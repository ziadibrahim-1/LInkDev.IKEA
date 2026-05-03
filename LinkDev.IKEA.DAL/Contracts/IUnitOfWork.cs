using LinkDev.IKEA.DAL.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

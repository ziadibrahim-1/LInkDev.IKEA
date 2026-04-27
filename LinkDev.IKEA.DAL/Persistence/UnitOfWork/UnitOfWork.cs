using LinkDev.IKEA.DAL.Contracts;
using LinkDev.IKEA.DAL.Contracts.Repositories;
using LinkDev.IKEA.DAL.Persistence.Data;
using LinkDev.IKEA.DAL.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Persistence.UnitOfWork
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext) // Ask RunTime to create an instance of ApplicationDbContext and pass it to this constructor Implecitly
        {
            DepartmentRepository = new DepartmentRepository(dbContext);
            _dbContext = dbContext;
        }
        public IDepartmentRepository? DepartmentRepository { get; set; }

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}

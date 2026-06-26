using LinkDev.IKEA.DAL.Contracts.Repositories;
using LinkDev.IKEA.DAL.Entities.Employees;
using LinkDev.IKEA.DAL.Persistence.Common;
using LinkDev.IKEA.DAL.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LinkDev.IKEA.DAL.Persistence.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee, int>, IEmployeeRepository
    {
        public EmployeeRepository( ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public PaginatedResult<Employee> GetAll(QueryParamters paramters)
        {
            Expression<Func<Employee , bool>>? filter = null;
            if(!string.IsNullOrEmpty(paramters.SearchTerm))
            {
                filter = e => e.FirstName.Contains(paramters.SearchTerm) || e.LastName.Contains(paramters.SearchTerm);
            }
            // Apply include
            Func<IQueryable<Employee>, IQueryable<Employee>>? include ;

            include = Query => Query.Include(e => e.Department);

            // Apply sorting
            Func<IQueryable<Employee>, IOrderedQueryable<Employee>>? orderBy = paramters.SortedBy?.ToLower() switch
            {
                "name" or "fullname" => q => paramters.SortAscynding
                    ? q.OrderBy(e => e.FirstName).ThenBy(e => e.LastName)
                    : q.OrderByDescending(e => e.FirstName).ThenByDescending(e => e.LastName),

                "email" => q => paramters.SortAscynding
                    ? q.OrderBy(e => e.Email)
                    : q.OrderByDescending(e => e.Email),

                "department" => q => paramters.SortAscynding
                    ? q.OrderBy(e => e.Department!.Name)
                    : q.OrderByDescending(e => e.Department!.Name),

                "status" => q => paramters.SortAscynding
                    ? q.OrderBy(e => e.IsActive)
                    : q.OrderByDescending(e => e.IsActive),

                _ => q => paramters.SortAscynding
                    ? q.OrderBy(e => e.FirstName).ThenBy(e => e.LastName)
                    : q.OrderByDescending(e => e.FirstName).ThenByDescending(e => e.LastName)

            };

            return base.GetAll(paramters , filter , orderBy , include);
        }

    }
}

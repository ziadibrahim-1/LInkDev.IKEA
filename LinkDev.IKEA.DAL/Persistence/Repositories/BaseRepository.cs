using LinkDev.IKEA.DAL.Common.Entites;
using LinkDev.IKEA.DAL.Contracts.Repositories;
using LinkDev.IKEA.DAL.Persistence.Common;
using LinkDev.IKEA.DAL.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LinkDev.IKEA.DAL.Persistence.Repositories
{
    public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository( ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }


        public TEntity? Get(TKey id) => _dbSet.Find(id);

        
        public TEntity? Get(Expression<Func<TEntity, bool>>? filter =null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet; // // _dbcontext.Set<TEntity>().Include().Where();

            if (include != null)
                query = include(query);

            if (filter != null)
                query = query.Where(filter);

            return query.FirstOrDefault(); // Immediate execution to get the entity
        }

        public IEnumerable<TEntity> GetAll(bool WithTracking = false)
        {
            if(!WithTracking)
                return _dbSet.AsNoTracking().ToList();
            return _dbSet.ToList();
        }

        public virtual PaginatedResult<TEntity> GetAll(QueryParamters paramters,Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if(include != null)
                query = include(query);
           
            if (filter != null)
                query = query.Where(filter);

            // get the total count before applying Ordring and pagination
            var TotalCount = query.Count();

            if (orderBy != null)
                query = orderBy(query);
            
            var entities = query
                            .Skip((paramters.PageIndex - 1) * paramters.PageSize)
                            .Take(paramters.PageSize)
                            .ToList();

            return new PaginatedResult<TEntity>() 
            {
                date = entities,
                PageIndex = paramters.PageIndex,
                PageSize = paramters.PageSize,
                TotalCount = TotalCount

            } ;
        }
        public void Add(TEntity entity )=> _dbSet.Add(entity);




        public void Delete(TKey id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public bool Exist(Expression<Func<TEntity, bool>> filter) => _dbSet.Any(filter);




        public void Update(TEntity entity) => _dbSet.Update(entity);
        
    }
}

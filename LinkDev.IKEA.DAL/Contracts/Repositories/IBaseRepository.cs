using LinkDev.IKEA.DAL.Common.Entites;
using LinkDev.IKEA.DAL.Persistence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Contracts.Repositories
{
    public interface IBaseRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        IEnumerable<TEntity> GetAll(bool WithTracking = false);

        /// <summary>
        /// Retrieves entities that match the specified filter, with optional ordering and related data inclusion.
        /// </summary>
        /// <param name="paramters">Pagination parameters including page index and page size.</param>
        /// <param name="filter">An expression used to filter the returned entities.</param>
        /// <param name="orderBy">An optional function that orders the query results.</param>
        /// <param name="include">An optional function that includes related entities in the query.</param>
        /// <returns>A collection of entities that satisfy the filter and query options.</returns>
        PaginatedResult<TEntity> GetAll(
            QueryParamters paramters,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

       
        TEntity? Get(TKey id);

        /// <summary>
        /// Retrieves the first entity that matches the specified filter, with optional related data inclusion.
        /// </summary>
        /// <param name="filter">An expression used to find the entity.</param>
        /// <param name="include">An optional function that includes related entities in the query.</param>
        /// <returns>The matching entity, or <see langword="null" /> if no entity is found.</returns>
        TEntity? Get(
             Expression<Func<TEntity, bool>>? filter = null,
             Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TKey id);

        /// <summary>
        /// Determines whether any entity matches the specified filter.
        /// </summary>
        /// <param name="filter">An expression used to test for matching entities.</param>
        /// <returns><see langword="true" /> if at least one entity matches; otherwise, <see langword="false" />.</returns>
        bool Exist(Expression<Func<TEntity, bool>> filter);
    }
}
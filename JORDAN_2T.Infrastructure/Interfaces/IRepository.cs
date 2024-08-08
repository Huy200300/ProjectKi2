using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JORDAN_2T.Infrastructure.Interfaces
{
    /// <summary>
    /// The idea is to subclass the DataContext class to pull data from different sources. 
    /// The UnitOfWork class takes a DataContext object in its constructor. A UnitOfWork can
    /// contain several repositories, all must use the same DataContext so we can implement transactions.  
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int? id);

        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter, string? includeProperties = null, bool tracked = true);
        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter=null, string? includeProperties = null);

        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}

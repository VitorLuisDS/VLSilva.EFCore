using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DS.EFCore.Helper.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(object id);
        TEntity GetById(object id);
        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> filter);
        TEntity GetBy(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetManyByAsync(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetManyBy(Expression<Func<TEntity, bool>> filter);
        Task RemoveByAsync(Expression<Func<TEntity, bool>> filter);
        void RemoveBy(Expression<Func<TEntity, bool>> filter);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        IQueryable<TEntity> GetQueryableBy(Expression<Func<TEntity, bool>> filter);
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DS.EFCore.Helper
{
    public static class DbContextExtensions
    {
        public static void RemoveUntrackedEntities<TEntity>(this DbContext dbContext, IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (TEntity entity in entities)
            {
                if (dbContext.Entry(entity).State == EntityState.Detached)
                {
                    dbContext.Attach(entity);
                }
            }

            dbContext.RemoveRange(entities);
        }

        public static void UpdateUntrackedEntities<TEntity>(this DbContext dbContext, IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (TEntity entity in entities)
            {
                if (dbContext.Entry(entity).State == EntityState.Detached)
                {
                    dbContext.Attach(entity);
                }

                dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public static void RemoveUntrackedEntity<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbContext.Attach(entity);
            }

            dbContext.Remove(entity);
        }

        public static void UpdateUntrackedEntity<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbContext.Attach(entity);
            }

            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public static void RemoveBy<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            TEntity entity = dbContext
                .Set<TEntity>()
                .AsTracking()
                .Single(filter);

            dbContext.Remove(entity);
        }

        public static async Task RemoveAsyncBy<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            TEntity entity = await dbContext
                .Set<TEntity>()
                .AsTracking()
                .SingleAsync(filter);

            dbContext.Remove(entity);
        }

        public static void RemoveAll<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            TEntity[] entities = dbContext
                .Set<TEntity>()
                .AsTracking()
                .Where(filter)
                .ToArray();

            dbContext.RemoveRange(entities);
        }

        public static async Task RemoveAllAsync<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            TEntity[] entities = await dbContext
                .Set<TEntity>()
                .AsTracking()
                .Where(filter)
                .ToArrayAsync();

            dbContext.RemoveRange(entities);
        }
    }
}

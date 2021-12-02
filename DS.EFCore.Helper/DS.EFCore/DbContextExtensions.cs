using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DS.EFCore.Helper
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// <para>
        /// Begins tracking the given (multiple) entities setting State = EntityState.Deleted.
        /// </para>
        /// <para>
        /// Entities with State = EntityState.Added will be detached from the DbContext.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="entities">Entities to remove.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void RemoveUntrackedEntities<TEntity>(this DbContext dbContext, IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities is null)
                throw new ArgumentNullException(nameof(entities));

            foreach (TEntity entity in entities)
            {
                if (dbContext.Entry(entity).State == EntityState.Detached)
                {
                    dbContext.Attach(entity);
                }
            }

            dbContext.RemoveRange(entities);
        }

        /// <summary>
        /// <para>
        /// Begins tracking the given (multiple) entities setting State = EntityState.Modified.
        /// </para>
        /// <para>
        /// Entities with State = EntityState.Detached will be attached to the DbContext.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="entities">Entities to update.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void UpdateUntrackedEntities<TEntity>(this DbContext dbContext, IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities is null)
                throw new ArgumentNullException(nameof(entities));

            foreach (TEntity entity in entities)
            {
                if (dbContext.Entry(entity).State == EntityState.Detached)
                {
                    dbContext.Attach(entity);
                }

                dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// <para>
        /// Begins tracking the given entity setting State = EntityState.Deleted.
        /// </para>
        /// <para>
        /// If the entity has State = EntityState.Added, it will be detached from the DbContext.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="entity">Entity to remove.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void RemoveUntrackedEntity<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbContext.Attach(entity);
            }

            dbContext.Remove(entity);
        }

        /// <summary>
        /// <para>
        /// Begins tracking the given entity setting State = EntityState.Modified.
        /// <para>
        /// </para>
        /// If the entity has State = EntityState.Detached, it will be attached to the DbContext.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes..</param>
        /// <param name="entity">Entity to update..</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void UpdateUntrackedEntity<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbContext.Attach(entity);
            }

            dbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// <para>
        /// Begins tracking the entity (that exists in the database and) that matches the given expression setting State = EntityState.Deleted.
        /// </para>
        /// <para>
        /// If no entity is found, an exception will be thrown.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="filter">Expression that filters the entity to remove.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void RemoveBy<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));

            TEntity entity = dbContext
                .Set<TEntity>()
                .AsTracking()
                .Single(filter);

            dbContext.Remove(entity);
        }

        /// <summary>
        /// <para>
        /// Begins tracking asynchronously the entity (that exists in the database and) that matches the given expression setting State = EntityState.Deleted.
        /// </para>
        /// <para>
        /// If no entity is found, an exception will be thrown.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="filter">Expression that filters the entity to remove.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Task to execute.</returns>
        public static async Task RemoveAsyncBy<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));

            TEntity entity = await dbContext
                .Set<TEntity>()
                .AsTracking()
                .SingleAsync(filter);

            dbContext.Remove(entity);
        }

        /// <summary>
        /// <para>
        /// Begins tracking the (multiple) entities (that exists in the database and) that matches the given expression setting State = EntityState.Deleted.
        /// </para>
        /// <para>
        /// Entities with State = EntityState.Added will be detached from the DbContext.
        /// </para>
        /// <para>
        /// If no entity is found, nothing will be done.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="filter">Expression that filters the entities to remove.</param>
        public static void RemoveAll<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            TEntity[] entities = dbContext
                .Set<TEntity>()
                .AsTracking()
                .Where(filter)
                .ToArray();

            if (entities.Any())
                dbContext.RemoveRange(entities);
        }

        /// <summary>
        /// <para>
        /// Begins tracking asynchronously the (multiple) entities (that exists in the database and) that matches the given expression setting State = EntityState.Deleted.
        /// </para>
        /// <para>
        /// Entities with State = EntityState.Added will be detached from the DbContext.
        /// </para>
        /// <para>
        /// If no entity is found, nothing will be done.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="filter">Expression that filters the entities to remove.</param>
        /// <returns>Task to execute.</returns>
        public static async Task RemoveAllAsync<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            TEntity[] entities = await dbContext
                .Set<TEntity>()
                .AsTracking()
                .Where(filter)
                .ToArrayAsync();

            if (entities.Any())
                dbContext.RemoveRange(entities);
        }

        /// <summary>
        /// <para>
        /// Changes the tracking of the entity that matches the given expression setting State = EntityState.Detached.
        /// </para>
        /// <para>
        /// If no entity is found, an exception will be thrown.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="filter">Expression that filters the entity to remove.</param>
        /// <param name="entityStateToRemove">State to filter the entity entry.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void DetachUnsavedEntityBy<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> filter, EntityState entityStateToRemove = EntityState.Added) where TEntity : class
        {
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));

            var entriesFromChangeTracker = dbContext
                .ChangeTracker
                .Entries<TEntity>()
                .Where(entry => entry.State == entityStateToRemove)
                .AsQueryable();

            TEntity entityFromChangeTracker = entriesFromChangeTracker
                .Select(entry => entry.Entity)
                .Single(filter);

            EntityEntry entityEntry = entriesFromChangeTracker.Single(entry => entry.Entity == entityFromChangeTracker);
            entityEntry.State = EntityState.Detached;
        }

        /// <summary>
        /// <para>
        /// Changes the tracking of the entities (multiple) that matches the given expression setting State = EntityState.Detached.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="filter">Expression that filters the entity to remove.</param>
        /// <param name="entityStateToRemove">State to filter the entities entries.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Task to execute.</returns>
        public static void DetachUnsavedEntitiesBy<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> filter, EntityState entityStateToRemove = EntityState.Added) where TEntity : class
        {
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));

            var entriesFromChangeTracker = dbContext
                .ChangeTracker
                .Entries<TEntity>()
                .Where(entry => entry.State == entityStateToRemove)
                .AsQueryable();

            IEnumerable<TEntity> entitiesFromChangeTracker = entriesFromChangeTracker
                .Select(entry => entry.Entity)
                .Where(filter)
                .ToArray();

            foreach (TEntity entityFromChangeTracker in entitiesFromChangeTracker)
            {
                EntityEntry entityEntry = entriesFromChangeTracker.Single(entry => entry.Entity == entityFromChangeTracker);
                entityEntry.State = EntityState.Detached;
            }
        }
    }
}

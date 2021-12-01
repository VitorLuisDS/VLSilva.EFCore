﻿using Microsoft.EntityFrameworkCore;
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
        /// Begins tracking the given (multiple) entities with State = EntityState.Deleted.
        /// </para>
        /// <para>
        /// Entities with State = EntityState.Added will be detached from the DbContext.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="entities">Entities to remove.</param>
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

        /// <summary>
        /// <para>
        /// Begins tracking the given (multiple) entities with State = EntityState.Modified.
        /// </para>
        /// <para>
        /// Entities with State = EntityState.Detached will be attached to the DbContext.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="entities">Entities to update.</param>
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

        /// <summary>
        /// <para>
        /// Begins tracking the given entity with State = EntityState.Deleted.
        /// </para>
        /// <para>
        /// If the entity has State = EntityState.Added, it will be detached from the DbContext.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="entity">Entity to remove.</param>
        public static void RemoveUntrackedEntity<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbContext.Attach(entity);
            }

            dbContext.Remove(entity);
        }

        /// <summary>
        /// <para>
        /// Begins tracking the given entity with State = EntityState.Modified.
        /// <para>
        /// </para>
        /// If the entity has State = EntityState.Detached, it will be attached to the DbContext.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes..</param>
        /// <param name="entity">Entity to update..</param>
        public static void UpdateUntrackedEntity<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbContext.Attach(entity);
            }

            dbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// <para>
        /// Begins tracking the entity that matches the given expression with State = EntityState.Deleted.
        /// </para>
        /// <para>
        /// If the entity has State = EntityState.Added, it will be detached from the DbContext.
        /// </para>
        /// <para>
        /// If no entity is found, an exception will be thrown.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="filter">Expression that filters the entity to remove.</param>
        public static void RemoveBy<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            TEntity entity = dbContext
                .Set<TEntity>()
                .AsTracking()
                .Single(filter);

            dbContext.Remove(entity);
        }

        /// <summary>
        /// <para>
        /// Begins tracking asynchronously the entity that matches the given expression with State = EntityState.Deleted.
        /// </para>
        /// <para>
        /// If the entity has State = EntityState.Added, it will be detached from the DbContext.
        /// </para>
        /// <para>
        /// If no entity is found, an exception will be thrown.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">Entity class.</typeparam>
        /// <param name="dbContext">DbContext that will track changes.</param>
        /// <param name="filter">Expression that filters the entity to remove.</param>
        /// <returns></returns>
        public static async Task RemoveAsyncBy<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            TEntity entity = await dbContext
                .Set<TEntity>()
                .AsTracking()
                .SingleAsync(filter);

            dbContext.Remove(entity);
        }

        /// <summary>
        /// <para>
        /// Begins tracking the (multiple) entities that matches the given expression with State = EntityState.Deleted.
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
        /// Begins tracking asynchronously the (multiple) entities that matches the given expression with State = EntityState.Deleted.
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
        /// <returns></returns>
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

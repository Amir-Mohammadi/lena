// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Linq.Expressions;
// using System.Threading;
// using System.Threading.Tasks;
// using core.Models;
// using Microsoft.EntityFrameworkCore;
// using core.Data;
// namespace core.Data
// {
//   public class Repository : IRepository
//   {
//     private readonly ApplicationDbContext dbContext;
//     private DbSet<TEntity> Entities { get; }
//     public Repository(ApplicationDbContext dbContext)
//     {
//       this.dbContext = dbContext;
//       Entities = this.dbContext.Set<TEntity>();
//     }
//     #region Async Method
//     public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, IInclude<TEntity> include = null)
//     => GetAsync(predicate: predicate, selector: x => x, cancellationToken: cancellationToken, include: include);
//     public virtual Task<TResult> GetAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken, IInclude<TEntity> include = null)
//     {
//       var query = GetQuery(include);
//       query = query.Where(predicate);
//       return query.Select(selector)
//                   .FirstOrDefaultAsync(cancellationToken);
//     }
//     public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
//     {
//       await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
//       if (saveNow)
//         await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
//     }
//     public virtual async Task AddRangeAsync(IQueryable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
//     {
//       await this.Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
//       if (saveNow)
//         await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
//     }
//     public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
//     {
//       dbContext.Entry<TEntity>(entity).OriginalValues["RowVersion"] = entity.RowVersion;
//       Entities.Update(entity);
//       if (saveNow)
//         await dbContext.SaveChangesAsync(cancellationToken);
//     }
//     public virtual async Task UpdateRangeAsync(IQueryable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
//     {
//       this.Entities.UpdateRange(entities);
//       if (saveNow)
//         await dbContext.SaveChangesAsync(cancellationToken);
//     }
//     public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
//     {
//       dbContext.Entry<TEntity>(entity).OriginalValues["RowVersion"] = entity.RowVersion;
//       Entities.Remove(entity);
//       if (saveNow)
//         await dbContext.SaveChangesAsync(cancellationToken);
//     }
//     public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, bool saveNow = true)
//     {
//       var query = GetQuery().Where(predicate: predicate);
//       await query.DeleteFromQueryAsync(cancellationToken);
//       if (saveNow)
//         await dbContext.SaveChangesAsync(cancellationToken);
//     }
//     public virtual async Task DeleteAsync(IQueryable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
//     {
//       this.Entities.RemoveRange(entities);
//       if (saveNow)
//         await dbContext.SaveChangesAsync(cancellationToken);
//     }
//     #endregion
//     #region Sync Methods
//     public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate, IInclude<TEntity> include = null)
//     => Get<TEntity>(predicate: predicate, selector: x => x, include: include);
//     public virtual TResult Get<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, IInclude<TEntity> include = null)
//     {
//       var query = GetQuery(include).Where(predicate);
//       var result = query.Select(selector);
//       return result.FirstOrDefault();
//     }
//     public virtual IQueryable<TEntity> GetQuery(IInclude<TEntity> include = null)
//     => GetQuery(selector: x => x, include: include);
//     public virtual IQueryable<TResult> GetQuery<TResult>(Expression<Func<TEntity, TResult>> selector, IInclude<TEntity> include = null)
//     {
//       var query = Entities.AsQueryable();
//       if (include != null)
//         query = include.Execute(query);
//       return query.Select(selector);
//     }
//     public virtual void Add(TEntity entity, bool saveNow = true)
//     {
//       Entities.Add(entity);
//       if (saveNow)
//         dbContext.SaveChanges();
//     }
//     public virtual void AddRange(IQueryable<TEntity> entities, bool saveNow = true)
//     {
//       this.Entities.AddRange(entities);
//       if (saveNow)
//         dbContext.SaveChanges();
//     }
//     public virtual void Update(TEntity entity, bool saveNow = true)
//     {
//       dbContext.Entry<TEntity>(entity).OriginalValues["RowVersion"] = entity.RowVersion;
//       Entities.Update(entity);
//       dbContext.SaveChanges();
//     }
//     public virtual void UpdateRange(IQueryable<TEntity> entities, bool saveNow = true)
//     {
//       this.Entities.UpdateRange(entities);
//       if (saveNow)
//         dbContext.SaveChanges();
//     }
//     public void Delete(Expression<Func<TEntity, bool>> predicate, bool saveNow = true)
//     {
//       var query = GetQuery().Where(predicate: predicate);
//       query.DeleteFromQuery();
//       if (saveNow)
//         dbContext.SaveChanges();
//     }
//     public virtual void Delete(TEntity entity, bool saveNow = true)
//     {
//       dbContext.Entry<TEntity>(entity).OriginalValues["RowVersion"] = entity.RowVersion;
//       Entities.Remove(entity);
//       if (saveNow)
//         dbContext.SaveChanges();
//     }
//     public virtual void Delete(IQueryable<TEntity> entities, bool saveNow = true)
//     {
//       this.Entities.RemoveRange(entities);
//       if (saveNow)
//         dbContext.SaveChanges();
//     }
//     #endregion
//     #region Attach & Detach
//     public virtual void Detach(TEntity entity)
//     {
//       var entry = dbContext.Entry(entity);
//       if (entry != null)
//         entry.State = EntityState.Detached;
//     }
//     public virtual void Attach(TEntity entity)
//     {
//       if (dbContext.Entry(entity).State == EntityState.Detached)
//         Entities.Attach(entity);
//     }
//     #endregion
//     #region Explicit Loading
//     public virtual async Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken)
//         where TProperty : class
//     {
//       Attach(entity);
//       var collection = dbContext.Entry<TEntity>(entity).Collection(collectionProperty);
//       if (!collection.IsLoaded)
//         await collection.LoadAsync(cancellationToken).ConfigureAwait(false);
//     }
//     public virtual void LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty)
//         where TProperty : class
//     {
//       dbContext.Entry<TEntity>(entity).Collection(collectionProperty).Load();
//       Attach(entity);
//       var collection = dbContext.Entry(entity).Collection(collectionProperty);
//       if (!collection.IsLoaded)
//         collection.Load();
//     }
//     public virtual async Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty, CancellationToken cancellationToken)
//         where TProperty : class
//     {
//       Attach(entity);
//       var reference = dbContext.Entry(entity).Reference(referenceProperty);
//       if (!reference.IsLoaded)
//         await reference.LoadAsync(cancellationToken).ConfigureAwait(false);
//     }
//     public virtual void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty)
//         where TProperty : class
//     {
//       Attach(entity);
//       var reference = dbContext.Entry(entity).Reference(referenceProperty);
//       if (!reference.IsLoaded)
//         reference.Load();
//     }
//     public bool IsModified<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property)
//     {
//       return dbContext.Entry<TEntity>(entity).Property(property).IsModified;
//     }
//     public TProperty OriginalValue<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property)
//     {
//       return dbContext.Entry<TEntity>(entity).Property(property).OriginalValue;
//     }
//     #endregion
//   }
// }
//where TEntity : class, IEntity
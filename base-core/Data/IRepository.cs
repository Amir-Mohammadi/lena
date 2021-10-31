using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using core.Models;
namespace core.Data
{
  public interface IRepository
  {
    TEntity Create<TEntity>() where TEntity : class, IEntity;
    void Add<TEntity>(TEntity entity, bool saveNow = true) where TEntity : class, IEntity;
    Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken, bool saveNow = true) where TEntity : class, IEntity;
    void AddRange<TEntity>(IQueryable<TEntity> entities, bool saveNow = true) where TEntity : class, IEntity;
    Task AddRangeAsync<TEntity>(IQueryable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true) where TEntity : class, IEntity;
    void Attach<TEntity>(TEntity entity) where TEntity : class, IEntity;
    void Delete<TEntity>(TEntity entity, bool saveNow = true) where TEntity : class, IEntity;
    Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken, bool saveNow = true) where TEntity : class, IEntity;
    void Delete<TEntity>(Expression<Func<TEntity, bool>> predicate, bool saveNow = true) where TEntity : class, IEntity;
    Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, bool saveNow = true) where TEntity : class, IEntity;
    void Delete<TEntity>(IQueryable<TEntity> entities, bool saveNow = true) where TEntity : class, IEntity;
    Task DeleteAsync<TEntity>(IQueryable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true) where TEntity : class, IEntity;
    void Detach<TEntity>(TEntity entity) where TEntity : class, IEntity;
    TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate, IInclude<TEntity> include = null) where TEntity : class, IEntity;
    TResult Get<TEntity, TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, IInclude<TEntity> include = null) where TEntity : class, IEntity;
    Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, IInclude<TEntity> include = null) where TEntity : class, IEntity;
    Task<TResult> GetAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken, IInclude<TEntity> include = null) where TEntity : class, IEntity;
    IQueryable<TEntity> GetQuery<TEntity>(IInclude<TEntity> include = null) where TEntity : class, IEntity;
    IQueryable<TResult> GetQuery<TEntity, TResult>(Expression<Func<TEntity, TResult>> selector, IInclude<TEntity> include = null) where TEntity : class, IEntity;
    void LoadCollection<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty) where TProperty : class where TEntity : class, IEntity;
    Task LoadCollectionAsync<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken) where TProperty : class where TEntity : class, IEntity;
    void LoadReference<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty) where TProperty : class where TEntity : class, IEntity;
    Task LoadReferenceAsync<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty, CancellationToken cancellationToken) where TProperty : class where TEntity : class, IEntity;
    void Update<TEntity>(TEntity entity, byte[] rowVersion, bool saveNow = true) where TEntity : class, IEntity;
    Task UpdateAsync<TEntity>(TEntity entity, byte[] rowVersion, CancellationToken cancellationToken, bool saveNow = true) where TEntity : class, IEntity;
    void UpdateRange<TEntity>(IQueryable<TEntity> entities, bool saveNow = true) where TEntity : class, IEntity;
    Task UpdateRangeAsync<TEntity>(IQueryable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true) where TEntity : class, IEntity;
    bool IsModified<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property) where TEntity : class, IEntity;
    TProperty OriginalValue<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property) where TEntity : class, IEntity;
  }
}
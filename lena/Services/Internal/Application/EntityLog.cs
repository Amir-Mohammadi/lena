//using LinqLib.Sort;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.EntityLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Application
{
  public partial class ApplicationManagement
  {
    #region Add
    public EntityLog AddEntityLog(
        string entityType,
        string apiParams,
        string api,
        string description)
    {

      var entityLog = repository.Create<EntityLog>();
      entityLog.UserId = App.Providers.Security.CurrentLoginData?.UserId;
      entityLog.DateTime = DateTime.UtcNow;
      entityLog.Ip = App.Providers.Request.ClientAddress;
      entityLog.Api = api;
      entityLog.ApiParams = apiParams;
      entityLog.Description = description;
      repository.Add(entityLog);
      return entityLog;
    }
    #endregion
    #region Edit
    public EntityLog EditEntityLog(
        byte[] rowVersion,
        int id,
        TValue<string> ip = null,
        TValue<string> api = null,
        TValue<string> apiParams = null,
        TValue<string> description = null)
    {

      var entityLog = GetEntityLog(id: id);
      if (ip != null)
        entityLog.Ip = ip;
      if (api != null)
        entityLog.Api = api;
      if (apiParams != null)
        entityLog.ApiParams = apiParams;
      if (description != null)
        entityLog.Description = description;
      repository.Update(entity: entityLog, rowVersion: entityLog.RowVersion);
      return entityLog;
    }
    #endregion
    #region Delete
    public void DeleteEntityLog(int id)
    {

      var entityLog = GetEntityLog(id: id);
      repository.Delete(entityLog);
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetEntityLogs<TResult>(
        Expression<Func<EntityLog, TResult>> selector,
        TValue<int> id = null,
        TValue<int> userId = null,
        TValue<DateTime> dateTime = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<string> api = null,
        TValue<string> ip = null)
    {

      var query = repository.GetQuery<EntityLog>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (dateTime != null)
        query = query.Where(i => i.DateTime == dateTime);
      if (fromDate != null)
        query = query.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        query = query.Where(i => i.DateTime <= toDate);
      if (api != null)
        query = query.Where(i => i.Api == api);
      if (ip != null)
        query = query.Where(i => i.Ip == ip);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public EntityLog GetEntityLog(int id) => GetEntityLog(e => e, id: id);
    public TResult GetEntityLog<TResult>(
        Expression<Func<EntityLog, TResult>> selector,
        int id)
    {

      var entityLog = GetEntityLogs(
                    selector: selector,
                    userId: id)


                .FirstOrDefault();
      if (entityLog == null)
        throw new RecordNotFoundException(id, typeof(EntityLog));
      return entityLog;
    }
    #endregion
    #region SortResult
    public IOrderedQueryable<EntityLogResult> SortEntityLogResult(
        IQueryable<EntityLogResult> query,
        SortInput<EntityLogSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case EntityLogSortType.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case EntityLogSortType.EmployeeFullName:
          return query.OrderBy(r => r.EmployeeFullName, sortInput.SortOrder);
        case EntityLogSortType.DateTime:
          return query.OrderBy(r => r.DateTime, sortInput.SortOrder);
        case EntityLogSortType.Ip:
          return query.OrderBy(r => r.Ip, sortInput.SortOrder);
        case EntityLogSortType.Api:
          return query.OrderBy(r => r.Api, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region SearchResult
    public IQueryable<EntityLogResult> SearchEntityLogResult(IQueryable<EntityLogResult> query, string searchText, AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(i => i.Id.ToString().Contains(searchText) ||
                                 i.Api.Contains(searchText) ||
                                 i.ApiParams.Contains(searchText) ||
                                 i.Description.ToString().Contains(searchText));
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<EntityLog, EntityLogResult>> ToEntityLogResult =
        entityLog => new EntityLogResult()
        {
          Id = entityLog.Id,
          UserId = entityLog.UserId,
          EmployeeFullName = entityLog.User.Employee.FirstName + " " + entityLog.User.Employee.LastName,
          DateTime = entityLog.DateTime,
          Ip = entityLog.Ip,
          Api = entityLog.Api,
          ApiParams = entityLog.ApiParams,
          Description = entityLog.Description,
          RowVersion = entityLog.RowVersion
        };
    #endregion
  }
}
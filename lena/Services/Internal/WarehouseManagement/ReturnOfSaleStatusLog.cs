using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Common;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Services.Core;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.ReturnOfSaleStatusLog;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Get
    public ReturnOfSaleStatusLog GetReturnOfSaleStatusLog(int id) => GetReturnOfSaleStatusLog(selector: e => e, id: id);
    public TResult GetReturnOfSaleStatusLog<TResult>(
        Expression<Func<ReturnOfSaleStatusLog, TResult>> selector,
        int id)
    {

      var returnOfSaleStatusLog = GetReturnOfSaleStatusLogs(
                   selector: selector,
                   id: id)


               .FirstOrDefault();
      if (returnOfSaleStatusLog == null)
        throw new ReturnOfSaleStatusLogNotFoundException(id);
      return returnOfSaleStatusLog;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetReturnOfSaleStatusLogs<TResult>(
            Expression<Func<ReturnOfSaleStatusLog, TResult>> selector,
            TValue<int> id = null,
            TValue<int> baseEntityId = null,
            TValue<int> userId = null,
            TValue<DateTime> dateTime = null,
            TValue<ReturnOfSaleStatus> status = null
            )
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntityLogs(
                selector: e => e,
                id: id,
                baseEntityId: baseEntityId,
                userId: userId,
                dateTime: dateTime
                );
      var query = baseQuery.OfType<ReturnOfSaleStatusLog>();
      if (status != null) query = query.Where(x => x.Status == status);

      return query.Select(selector);
    }
    #endregion
    #region Add
    public ReturnOfSaleStatusLog AddReturnOfSaleStatusLog(
        int baseEntityId,
        int userId,
        ReturnOfSaleStatus status
                )
    {

      var returnOfSaleStatusLog = repository.Create<ReturnOfSaleStatusLog>();
      returnOfSaleStatusLog.Status = status;
      App.Internals.ApplicationBase.AddBaseEntityLog(concreteClass: returnOfSaleStatusLog, baseEntityId: baseEntityId, userId: userId);
      return returnOfSaleStatusLog;
    }
    #endregion
    #region Edit
    public ReturnOfSaleStatusLog EditReturnOfSaleStatusLog(
        int id,
        byte[] rowVersion,
            TValue<int> baseEntityId = null,
            TValue<int> userId = null,
            TValue<ReturnOfSaleStatus> status = null
        )
    {

      var returnOfSaleStatusLog = GetReturnOfSaleStatusLog(id: id);
      return EditReturnOfSaleStatusLog(
                 returnOfSaleStatusLog: returnOfSaleStatusLog,
                rowVersion: rowVersion,
                baseEntityId: baseEntityId,
                userId: userId,
                status: status

                );

    }

    public ReturnOfSaleStatusLog EditReturnOfSaleStatusLog(
                ReturnOfSaleStatusLog
     returnOfSaleStatusLog,
                byte[] rowVersion,
                TValue<int> baseEntityId = null,
                TValue<int> userId = null,
                TValue<ReturnOfSaleStatus> status = null


                )
    {


      if (baseEntityId != null) returnOfSaleStatusLog.BaseEntityId = baseEntityId;
      if (userId != null) returnOfSaleStatusLog.UserId = userId;
      if (status != null) returnOfSaleStatusLog.Status = status;

      repository.Update(rowVersion: rowVersion, entity: returnOfSaleStatusLog);
      return returnOfSaleStatusLog;
    }

    #endregion
    #region Delete
    public void DeleteReturnOfSaleStatusLog(int id)
    {

      var returnOfSaleStatusLog = GetReturnOfSaleStatusLog(id: id);
      repository.Delete(returnOfSaleStatusLog);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ReturnOfSaleStatusLogResult> SortReturnOfSaleStatusLogResult(
        IQueryable<ReturnOfSaleStatusLogResult> query,
        SortInput<ReturnOfSaleStatusLogSortType> sort)
    {
      switch (sort.SortType)
      {
        case ReturnOfSaleStatusLogSortType.DateTime: return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case ReturnOfSaleStatusLogSortType.EmployeeFullName: return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case ReturnOfSaleStatusLogSortType.EmployeeCode: return query.OrderBy(a => a.EmployeeCode, sort.SortOrder);
        case ReturnOfSaleStatusLogSortType.Status: return query.OrderBy(a => a.Status, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<ReturnOfSaleStatusLogResult> SearchReturnOfSaleStatusLogResult(
        IQueryable<ReturnOfSaleStatusLogResult> query,
        string searchText)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.EmployeeFullName.Contains(searchText) ||
                item.EmployeeCode.Contains(searchText)
                select item;
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<ReturnOfSaleStatusLog, ReturnOfSaleStatusLogResult>> ToReturnOfSaleStatusLogResult =
                 returnOfSaleStatusLog => new ReturnOfSaleStatusLogResult
                 {
                   Id = returnOfSaleStatusLog.Id,
                   DateTime = returnOfSaleStatusLog.DateTime,
                   EmployeeCode = returnOfSaleStatusLog.User.Employee.Code,
                   EmployeeFullName = returnOfSaleStatusLog.User.Employee.FirstName + " " + returnOfSaleStatusLog.User.Employee.LastName,
                   Status = returnOfSaleStatusLog.Status,

                   RowVersion = returnOfSaleStatusLog.RowVersion
                 };
    #endregion

  }
}

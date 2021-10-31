using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.PurchaseRequestEditLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {

    #region Add
    public PurchaseRequestEditLog AddPurchaseRequestEditLog(
        int purchaseRequestId,
        string description,
        DateTime? beforeDeadLineDateTime,
        DateTime? afterDeadLineDateTime,
        TValue<double> beforeRequestQty = null,
        TValue<double> afterRequestQty = null)
    {

      var purchaseRequestEditLog = repository.Create<PurchaseRequestEditLog>();
      purchaseRequestEditLog.DateTime = DateTime.UtcNow;
      purchaseRequestEditLog.UserId = App.Providers.Security.CurrentLoginData.UserId;
      purchaseRequestEditLog.BeforeDeadLineDateTime = beforeDeadLineDateTime;
      purchaseRequestEditLog.AfterDeadLineDateTime = afterDeadLineDateTime;
      purchaseRequestEditLog.BeforeRequestQty = beforeRequestQty;
      purchaseRequestEditLog.AfterRequestQty = afterRequestQty;
      purchaseRequestEditLog.Description = description;
      purchaseRequestEditLog.PurchaseRequestId = purchaseRequestId;
      repository.Add(purchaseRequestEditLog);
      return purchaseRequestEditLog;
    }
    #endregion

    #region Get
    public PurchaseRequestEditLog GetPurchaseRequestEditLog(int id) => GetPurchaseRequestEditLog(e => e, id: id);
    public TResult GetPurchaseRequestEditLog<TResult>(
        Expression<Func<PurchaseRequestEditLog, TResult>> selector,
            int id)
    {

      var result = GetPurchaseRequestEditLogs(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (result == null)
        throw new RecordNotFoundException(id, typeof(PurchaseRequestEditLog));
      return result;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetPurchaseRequestEditLogs<TResult>(
        Expression<Func<PurchaseRequestEditLog, TResult>> selector,
        TValue<int> id = null,
        TValue<int> purchaseRequestId = null,
        TValue<int> userId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null)
    {

      var query = repository.GetQuery<PurchaseRequestEditLog>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (purchaseRequestId != null)
        query = query.Where(i => i.PurchaseRequestId == purchaseRequestId);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);

      return query.Select(selector);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<PurchaseRequestEditLogResult> SortPurchaseRequestEditLogResult(
        IQueryable<PurchaseRequestEditLogResult> query,
        SortInput<PurchaseRequestEditLogSortType> options)
    {
      switch (options.SortType)
      {
        case PurchaseRequestEditLogSortType.Id:
          return query.OrderBy(a => a.Id, options.SortOrder);
        case PurchaseRequestEditLogSortType.AfterDeadLineDateTime:
          return query.OrderBy(a => a.AfterDeadLineDateTime, options.SortOrder);
        case PurchaseRequestEditLogSortType.BeforeDeadLineDateTime:
          return query.OrderBy(a => a.BeforeDeadLineDateTime, options.SortOrder);
        case PurchaseRequestEditLogSortType.DateTime:
          return query.OrderBy(a => a.DateTime, options.SortOrder);
        case PurchaseRequestEditLogSortType.Description:
          return query.OrderBy(a => a.Description, options.SortOrder);
        case PurchaseRequestEditLogSortType.EmployeeName:
          return query.OrderBy(a => a.Description, options.SortOrder);
        case PurchaseRequestEditLogSortType.BeforeRequestQty:
          return query.OrderBy(a => a.BeforeRequestQty, options.SortOrder);
        case PurchaseRequestEditLogSortType.AfterRequestQty:
          return query.OrderBy(a => a.AfterRequestQty, options.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToResult
    public Expression<Func<PurchaseRequestEditLog, PurchaseRequestEditLogResult>> ToPurchaseRequestEditLogResult =
        purchaseRequestEditLog => new PurchaseRequestEditLogResult()
        {
          Id = purchaseRequestEditLog.Id,
          AfterDeadLineDateTime = purchaseRequestEditLog.AfterDeadLineDateTime,
          BeforeDeadLineDateTime = purchaseRequestEditLog.BeforeDeadLineDateTime,
          BeforeRequestQty = purchaseRequestEditLog.BeforeRequestQty,
          AfterRequestQty = purchaseRequestEditLog.AfterRequestQty,
          DateTime = purchaseRequestEditLog.DateTime,
          Description = purchaseRequestEditLog.Description,
          EmployeeName = purchaseRequestEditLog.User.Employee.FirstName + " " + purchaseRequestEditLog.User.Employee.LastName,
          RowVersion = purchaseRequestEditLog.RowVersion
        };
    #endregion

    #region Search
    public IQueryable<PurchaseRequestEditLogResult> SearchPurchaseRequestEditLogResultQuery(
        IQueryable<PurchaseRequestEditLogResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.EmployeeName.Contains(searchText) ||
                    item.Description.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);

      return query;
    }
    #endregion
  }
}

using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.StoreReceiptDeleteRequestStuffSerialResult;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public StoreReceiptDeleteRequestConfirmationLog AddStoreReceiptDeleteRequestConfirmationLog(
        int storeReceiptDeleteRequestId,
        string description,
        StoreReceiptDeleteRequestStatus status)
    {

      var storeReceiptDeleteRequestConfirmationLog = repository.Create<StoreReceiptDeleteRequestConfirmationLog>();
      storeReceiptDeleteRequestConfirmationLog.StoreReceiptDeleteRequestId = storeReceiptDeleteRequestId;
      storeReceiptDeleteRequestConfirmationLog.ConfirmerUserId = App.Providers.Security.CurrentLoginData.UserId;
      storeReceiptDeleteRequestConfirmationLog.DateTime = DateTime.Now.ToUniversalTime();
      storeReceiptDeleteRequestConfirmationLog.Description = description;
      storeReceiptDeleteRequestConfirmationLog.Status = status;
      repository.Add(storeReceiptDeleteRequestConfirmationLog);
      return storeReceiptDeleteRequestConfirmationLog;
    }

    #endregion

    #region Get

    public StoreReceiptDeleteRequestConfirmationLog GetStoreReceiptDeleteRequestConfirmationLog(int id) => GetStoreReceiptDeleteRequestConfirmationLog(selector: e => e, id: id);
    public TResult GetStoreReceiptDeleteRequestConfirmationLog<TResult>(Expression<Func<StoreReceiptDeleteRequestConfirmationLog, TResult>> selector,
        int id)
    {

      var orderItemBlock = GetStoreReceiptDeleteRequestConfirmationLogs(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (orderItemBlock == null)
        throw new ReturnOfSaleNotFoundException(id);
      return orderItemBlock;
    }


    #endregion

    #region Gets

    public IQueryable<TResult> GetStoreReceiptDeleteRequestConfirmationLogs<TResult>(
        Expression<Func<StoreReceiptDeleteRequestConfirmationLog, TResult>> selector,
            TValue<int> id = null,
            TValue<int> storeReceiptDeleteRequestId = null,
            TValue<int> confirmerUserId = null,
            TValue<DateTime> dateTime = null,
            TValue<string> description = null,
            TValue<StoreReceiptDeleteRequestStatus> status = null)
    {


      var query = repository.GetQuery<StoreReceiptDeleteRequestConfirmationLog>();
      if (storeReceiptDeleteRequestId != null)
        query = query.Where(r => r.StoreReceiptDeleteRequestId == storeReceiptDeleteRequestId);
      if (confirmerUserId != null)
        query = query.Where(r => r.ConfirmerUserId == confirmerUserId);
      if (dateTime != null)
        query = query.Where(r => r.DateTime == dateTime);
      if (description != null)
        query = query.Where(r => r.Description == description);
      if (status != null)
        query = query.Where(r => r.Status == status);

      return query.Select(selector);
    }

    #endregion

    #region Edit
    public StoreReceiptDeleteRequestConfirmationLog EditStoreReceiptDeleteRequestConfirmationLog(
        int id,
        byte[] rowVersion,
        TValue<int> storeReceiptDeleteRequestId = null,
        TValue<int> confirmationUserId = null,
        TValue<DateTime> dateTime = null,
        TValue<string> description = null,
        TValue<StoreReceiptDeleteRequestStatus> status = null)
    {

      var storeReceiptDeleteRequestConfirmationLog = GetStoreReceiptDeleteRequestConfirmationLog(id: id);
      if (storeReceiptDeleteRequestId != null)
        storeReceiptDeleteRequestConfirmationLog.StoreReceiptDeleteRequestId = storeReceiptDeleteRequestId;
      if (confirmationUserId != null)
        storeReceiptDeleteRequestConfirmationLog.ConfirmerUserId = confirmationUserId;
      if (dateTime != null)
        storeReceiptDeleteRequestConfirmationLog.DateTime = dateTime;
      if (description != null)
        storeReceiptDeleteRequestConfirmationLog.Description = description;
      if (status != null)
        storeReceiptDeleteRequestConfirmationLog.Status = status;

      repository.Update(rowVersion: rowVersion, entity: storeReceiptDeleteRequestConfirmationLog);
      return storeReceiptDeleteRequestConfirmationLog;
    }
    #endregion


    #region ToResult

    public Expression<Func<StoreReceiptDeleteRequestConfirmationLog, StoreReceiptDeleteRequestConfirmationLogResult>> ToStoreReceiptDeleteRequestConfirmationLogResult =
        storeReceiptDeleteRequestConfirmationLogResult => new StoreReceiptDeleteRequestConfirmationLogResult
        {
          Id = storeReceiptDeleteRequestConfirmationLogResult.Id,
          ConfirmerUserId = storeReceiptDeleteRequestConfirmationLogResult.ConfirmerUser.Id,
          ConfirmerUserFullName = storeReceiptDeleteRequestConfirmationLogResult.ConfirmerUser.Employee.FirstName + " " + storeReceiptDeleteRequestConfirmationLogResult.ConfirmerUser.Employee.LastName,
          StoreReceiptDeleteRequestId = storeReceiptDeleteRequestConfirmationLogResult.StoreReceiptDeleteRequestId,
          DateTime = storeReceiptDeleteRequestConfirmationLogResult.DateTime,
          Status = storeReceiptDeleteRequestConfirmationLogResult.Status,
          Description = storeReceiptDeleteRequestConfirmationLogResult.Description,
          RowVersion = storeReceiptDeleteRequestConfirmationLogResult.RowVersion
        };

    public IQueryable<StoreReceiptDeleteRequestConfirmationLogResult> ToStoreReceiptDeleteRequestConfirmationLogResultQuery(
        IQueryable<StoreReceiptDeleteRequestConfirmationLog> query)
    {

      if (query.All(i => i.StoreReceiptDeleteRequest.Status == StoreReceiptDeleteRequestStatus.NoAction))
      {
        var membershipUserIds = App.Internals.UserManagement.GetMemberships(
                  selector: e => e.UserId,
                  userGroupId: Models.StaticData.UserGroups.StoreReceiptDeleteRequestConfirmers.Id)

              .Distinct();

        var employees = App.Internals.UserManagement.GetEmployees(
                  selector: e => e);

        return from membershipUserId in membershipUserIds
               join employee in employees on membershipUserId equals employee.User.Id
               join log in query on membershipUserId equals log.ConfirmerUserId into tempLog
               from logResult in tempLog.DefaultIfEmpty()
               select new StoreReceiptDeleteRequestConfirmationLogResult
               {
                 Id = logResult.Id,
                 ConfirmerUserId = membershipUserId,
                 ConfirmerUserFullName = employee.FirstName + " " + employee.LastName,
                 StoreReceiptDeleteRequestId = logResult.StoreReceiptDeleteRequestId,
                 DateTime = logResult.DateTime,
                 Status = logResult.Status,
                 Description = logResult.Description,
                 RowVersion = logResult.RowVersion
               };
      }
      else
      {
        return query.Select(q => new StoreReceiptDeleteRequestConfirmationLogResult
        {
          Id = q.Id,
          ConfirmerUserId = q.ConfirmerUserId,
          ConfirmerUserFullName = q.ConfirmerUser.Employee.FirstName + " " + q.ConfirmerUser.Employee.LastName,
          StoreReceiptDeleteRequestId = q.StoreReceiptDeleteRequestId,
          DateTime = q.DateTime,
          Status = q.Status,
          Description = q.Description,
          RowVersion = q.RowVersion
        });
      }
    }
    #endregion

    #region sort

    public IOrderedQueryable<StoreReceiptDeleteRequestConfirmationLogResult> SortStoreReceiptDeleteRequestConfirmationLogResult(
      IQueryable<StoreReceiptDeleteRequestConfirmationLogResult> query,
      SortInput<StoreReceiptDeleteRequestConfirmationLogSortType> sort)
    {
      switch (sort.SortType)
      {
        case StoreReceiptDeleteRequestConfirmationLogSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case StoreReceiptDeleteRequestConfirmationLogSortType.StoreReceiptDeleteRequestId:
          return query.OrderBy(a => a.StoreReceiptDeleteRequestId, sort.SortOrder);
        case StoreReceiptDeleteRequestConfirmationLogSortType.ConfirmerUserId:
          return query.OrderBy(a => a.ConfirmerUserId, sort.SortOrder);
        case StoreReceiptDeleteRequestConfirmationLogSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<StoreReceiptDeleteRequestConfirmationLogResult> SearchStoreReceiptDeleteRequestConfirmationLogResult(
        IQueryable<StoreReceiptDeleteRequestConfirmationLogResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.ConfirmerUserFullName.Contains(searchText)

                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion

    #region Delete
    public void DeleteStoreReceiptDeleteRequestConfirmationLog(int id)
    {

      var storeReceiptDeleteRequestConfirmationLog = GetStoreReceiptDeleteRequestConfirmationLog(id: id);
      repository.Delete(storeReceiptDeleteRequestConfirmationLog);
    }
    #endregion






  }

}
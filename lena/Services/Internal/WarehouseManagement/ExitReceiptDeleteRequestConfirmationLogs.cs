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
using lena.Models.WarehouseManagement.ExitReceiptDeleteRequestConfirmationLog;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public ExitReceiptDeleteRequestConfirmationLog AddExitReceiptDeleteRequestConfirmationLog(
        int exitReceiptDeleteRequestId,
        string description,
        ExitReceiptDeleteRequestStatus status)
    {

      var exitReceiptDeleteRequestConfirmationLog = repository.Create<ExitReceiptDeleteRequestConfirmationLog>();
      exitReceiptDeleteRequestConfirmationLog.ExitReceiptDeleteRequestId = exitReceiptDeleteRequestId;
      exitReceiptDeleteRequestConfirmationLog.ConfirmerUserId = App.Providers.Security.CurrentLoginData.UserId;
      exitReceiptDeleteRequestConfirmationLog.DateTime = DateTime.Now.ToUniversalTime();
      exitReceiptDeleteRequestConfirmationLog.Description = description;
      exitReceiptDeleteRequestConfirmationLog.Status = status;
      repository.Add(exitReceiptDeleteRequestConfirmationLog);
      return exitReceiptDeleteRequestConfirmationLog;
    }

    #endregion

    #region Get

    public ExitReceiptDeleteRequestConfirmationLog GetExitReceiptDeleteRequestConfirmationLog(int id) => GetExitReceiptDeleteRequestConfirmationLog(selector: e => e, id: id);
    public TResult GetExitReceiptDeleteRequestConfirmationLog<TResult>(Expression<Func<ExitReceiptDeleteRequestConfirmationLog, TResult>> selector,
        int id)
    {

      var getExitReceiptDeleteRequestConfirmationLogs = GetExitReceiptDeleteRequestConfirmationLogs(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (getExitReceiptDeleteRequestConfirmationLogs == null)
        throw new ExitReceiptDeleteRequestConfirmationLogNotFoundException(id);
      return getExitReceiptDeleteRequestConfirmationLogs;
    }


    #endregion

    #region Gets

    public IQueryable<TResult> GetExitReceiptDeleteRequestConfirmationLogs<TResult>(
        Expression<Func<ExitReceiptDeleteRequestConfirmationLog, TResult>> selector,
            TValue<int> id = null,
            TValue<int> exitReceiptDeleteRequestId = null,
            TValue<int> confirmerUserId = null,
            TValue<DateTime> dateTime = null,
            TValue<string> description = null,
            TValue<ExitReceiptDeleteRequestStatus> status = null)
    {


      var query = repository.GetQuery<ExitReceiptDeleteRequestConfirmationLog>();

      if (exitReceiptDeleteRequestId != null)
        query = query.Where(r => r.ExitReceiptDeleteRequestId == exitReceiptDeleteRequestId);
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
    public ExitReceiptDeleteRequestConfirmationLog EditExitReceiptDeleteRequestConfirmationLog(
        int id,
        byte[] rowVersion,
        TValue<int> exitReceiptDeleteRequestId = null,
        TValue<int> confirmationUserId = null,
        TValue<DateTime> dateTime = null,
        TValue<string> description = null,
        TValue<ExitReceiptDeleteRequestStatus> status = null)
    {

      var exitReceiptDeleteRequestConfirmationLog = GetExitReceiptDeleteRequestConfirmationLog(id: id);
      if (exitReceiptDeleteRequestId != null)
        exitReceiptDeleteRequestConfirmationLog.ExitReceiptDeleteRequestId = exitReceiptDeleteRequestId;
      if (confirmationUserId != null)
        exitReceiptDeleteRequestConfirmationLog.ConfirmerUserId = confirmationUserId;
      if (dateTime != null)
        exitReceiptDeleteRequestConfirmationLog.DateTime = dateTime;
      if (description != null)
        exitReceiptDeleteRequestConfirmationLog.Description = description;
      if (status != null)
        exitReceiptDeleteRequestConfirmationLog.Status = status;

      repository.Update(rowVersion: rowVersion, entity: exitReceiptDeleteRequestConfirmationLog);
      return exitReceiptDeleteRequestConfirmationLog;
    }
    #endregion



    #region ToResult

    public Expression<Func<ExitReceiptDeleteRequestConfirmationLog, ExitReceiptDeleteRequestConfirmationLogResult>> ToExitReceiptDeleteRequestConfirmationLogResult =
        exitReceiptDeleteRequestConfirmationLogResult => new ExitReceiptDeleteRequestConfirmationLogResult
        {
          Id = exitReceiptDeleteRequestConfirmationLogResult.Id,
          ConfirmerUserId = exitReceiptDeleteRequestConfirmationLogResult.ConfirmerUser.Id,
          ConfirmerUserFullName = exitReceiptDeleteRequestConfirmationLogResult.ConfirmerUser.Employee.FirstName + " " + exitReceiptDeleteRequestConfirmationLogResult.ConfirmerUser.Employee.LastName,
          ExitReceiptDeleteRequestId = exitReceiptDeleteRequestConfirmationLogResult.ExitReceiptDeleteRequestId,
          DateTime = exitReceiptDeleteRequestConfirmationLogResult.DateTime,
          Status = exitReceiptDeleteRequestConfirmationLogResult.Status,
          Description = exitReceiptDeleteRequestConfirmationLogResult.Description,
          RowVersion = exitReceiptDeleteRequestConfirmationLogResult.RowVersion
        };

    public IQueryable<ExitReceiptDeleteRequestConfirmationLogResult> ToExitReceiptDeleteRequestConfirmationLogResultQuery(
        IQueryable<ExitReceiptDeleteRequestConfirmationLog> query)
    {

      if (query.All(i => i.ExitReceiptDeleteRequest.Status == ExitReceiptDeleteRequestStatus.NotAction))
      {
        var membershipUserIds = App.Internals.UserManagement.GetMemberships(
                  selector: e => e.UserId,
                  userGroupId: Models.StaticData.UserGroups.ExitReceiptDeleteRequestConfirmers.Id)

              .Distinct();

        var employees = App.Internals.UserManagement.GetEmployees(
                  selector: e => e);

        return from membershipUserId in membershipUserIds
               join employee in employees on membershipUserId equals employee.User.Id
               join log in query on membershipUserId equals log.ConfirmerUserId into tempLog
               from logResult in tempLog.DefaultIfEmpty()
               select new ExitReceiptDeleteRequestConfirmationLogResult
               {
                 Id = logResult.Id,
                 ConfirmerUserId = membershipUserId,
                 ConfirmerUserFullName = employee.FirstName + " " + employee.LastName,
                 ExitReceiptDeleteRequestId = logResult.ExitReceiptDeleteRequestId,
                 DateTime = logResult.DateTime,
                 Status = logResult.Status,
                 Description = logResult.Description,
                 RowVersion = logResult.RowVersion
               };
      }
      else
      {
        return query.Select(q => new ExitReceiptDeleteRequestConfirmationLogResult
        {
          Id = q.Id,
          ConfirmerUserId = q.ConfirmerUserId,
          ConfirmerUserFullName = q.ConfirmerUser.Employee.FirstName + " " + q.ConfirmerUser.Employee.LastName,
          ExitReceiptDeleteRequestId = q.ExitReceiptDeleteRequestId,
          DateTime = q.DateTime,
          Status = q.Status,
          Description = q.Description,
          RowVersion = q.RowVersion
        });
      }
    }
    #endregion

    #region sort

    public IOrderedQueryable<ExitReceiptDeleteRequestConfirmationLogResult> SortExitReceiptDeleteRequestConfirmationLogResult(
      IQueryable<ExitReceiptDeleteRequestConfirmationLogResult> query,
      SortInput<ExitReceiptDeleteRequestConfirmationLogSortType> sort)
    {
      switch (sort.SortType)
      {
        case ExitReceiptDeleteRequestConfirmationLogSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ExitReceiptDeleteRequestConfirmationLogSortType.ExitReceiptDeleteRequestId:
          return query.OrderBy(a => a.ExitReceiptDeleteRequestId, sort.SortOrder);
        case ExitReceiptDeleteRequestConfirmationLogSortType.ConfirmerUserId:
          return query.OrderBy(a => a.ConfirmerUserId, sort.SortOrder);
        case ExitReceiptDeleteRequestConfirmationLogSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ExitReceiptDeleteRequestConfirmationLogResult> SearchExitReceiptDeleteRequestConfirmationLogResult(
        IQueryable<ExitReceiptDeleteRequestConfirmationLogResult> query,
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
    public void DeleteExitReceiptDeleteRequestConfirmationLog(int id)
    {

      var exitReceiptDeleteRequestConfirmationLog = GetExitReceiptDeleteRequestConfirmationLog(id: id);
      repository.Delete(exitReceiptDeleteRequestConfirmationLog);
    }
    #endregion







  }

}
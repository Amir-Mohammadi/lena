using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Application.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Application;
using lena.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Application
{
  public partial class ApplicationManagement
  {
    public ApplicationLog AddApplicationLog()
    {

      var appLog = repository.Create<ApplicationLog>();
      var requestProvider = App.Providers.Request;
      appLog.UserId = App.Providers.Security.CurrentLoginData?.UserId;
      appLog.ClientIP = requestProvider.ClientAddress;
      appLog.UserAgent = "";
      appLog.Action = requestProvider.Method;
      appLog.RequestEndTime = null;
      appLog.LogTime = DateTime.UtcNow;
      repository.Add(appLog);
      return appLog;
    }
    public ApplicationLog EditApplicationLog(
        byte[] rowVersion,
        int id,
        TValue<string> clientIp,
        TValue<string> userAgent,
        TValue<string> action,
        TValue<DateTime> requestEndTime)
    {

      var appLog = GetApplicationLog(id: id);
      if (clientIp != null)
        appLog.ClientIP = clientIp;
      if (userAgent != null)
        appLog.UserAgent = userAgent;
      if (action != null)
        appLog.Action = action;
      if (requestEndTime != null)
        appLog.RequestEndTime = requestEndTime;
      repository.Update(entity: appLog, rowVersion: appLog.RowVersion);
      return appLog;
    }
    public void DeleteApplicationLog(int id)
    {

      var ApplicationLog = GetApplicationLog(id: id);
      repository.Delete(ApplicationLog);
    }
    public void DeleteAllApplicationLogs()
    {

      //Delete All Records
      repository.Delete<ApplicationLog>(x => x.Id != 0);
    }
    public IQueryable<TResult> GetApplicationLogs<TResult>(
        Expression<Func<ApplicationLog, TResult>> selector,
        TValue<int> id = null,
        TValue<int?> userId = null,
        TValue<string> clientIp = null,
        TValue<string> userAgent = null,
        TValue<string> action = null,
        string searchText = null
        )
    {

      var query = repository.GetQuery<ApplicationLog>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (userId != null)
        query = query.Where(x => x.UserId == userId);
      if (clientIp != null)
        query = query.Where(x => x.ClientIP == clientIp);
      if (userAgent != null)
        query = query.Where(x => x.UserAgent == userAgent);
      if (action != null)
        query = query.Where(x => x.Action == action);
      if (!string.IsNullOrEmpty(searchText))
        query = query.Where(x =>
              x.Action.Contains(searchText) ||
              x.ClientIP.Contains(searchText) ||
              x.UserAgent.Contains(searchText) ||
              x.User.UserName.Contains(searchText)
              );
      return query.Select(selector);
    }
    public ApplicationLog GetApplicationLog(int id) => GetApplicationLog(selector: e => e, id: id);
    public TResult GetApplicationLog<TResult>(Expression<Func<ApplicationLog, TResult>> selector, int id)
    {

      var ApplicationLog = GetApplicationLogs(selector: selector, id: id).FirstOrDefault();
      if (ApplicationLog == null)
        throw new ApplicationLogNotFoundException(id);
      return ApplicationLog;
    }
    public IOrderedQueryable<ApplicationLogResult> SortApplicationLogResult(IQueryable<ApplicationLogResult> query, SortInput<ApplicationLogSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case ApplicationLogSortType.Id:
          return query.OrderBy(x => x.Id, sortInput.SortOrder);
        case ApplicationLogSortType.ClientIp:
          return query.OrderBy(x => x.ClientIp, sortInput.SortOrder);
        case ApplicationLogSortType.UserAgent:
          return query.OrderBy(x => x.UserAgent, sortInput.SortOrder);
        case ApplicationLogSortType.Action:
          return query.OrderBy(x => x.Action, sortInput.SortOrder);
        case ApplicationLogSortType.EmployeeCode:
          return query.OrderBy(x => x.EmployeeCode, sortInput.SortOrder);
        case ApplicationLogSortType.EmployeeFullName:
          return query.OrderBy(x => x.EmployeeFullName, sortInput.SortOrder);
        case ApplicationLogSortType.LogTime:
          return query.OrderBy(x => x.LogTime, sortInput.SortOrder);
        case ApplicationLogSortType.RequestEndTime:
          return query.OrderBy(x => x.RequestEndTime, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<ApplicationLogGroupedMinMaxResult> SortApplicationLogResult(IQueryable<ApplicationLogGroupedMinMaxResult> query, SortInput<ApplicationLogSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case ApplicationLogSortType.Date:
          return query.OrderBy(x => x.Date, sortInput.SortOrder);
        case ApplicationLogSortType.UserName:
          return query.OrderBy(x => x.UserName, sortInput.SortOrder);
        case ApplicationLogSortType.EmployeeCode:
          return query.OrderBy(x => x.EmployeeCode, sortInput.SortOrder);
        case ApplicationLogSortType.EmployeeFullName:
          return query.OrderBy(x => x.EmployeeFullName, sortInput.SortOrder);
        case ApplicationLogSortType.FirstRequestTime:
          return query.OrderBy(x => x.FirstRequestTime, sortInput.SortOrder);
        case ApplicationLogSortType.LastRequestTime:
          return query.OrderBy(x => x.LastRequestTime, sortInput.SortOrder);
        case ApplicationLogSortType.TotalTime:
          return query.OrderBy(x => x.TotalTime, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public Expression<Func<ApplicationLog, ApplicationLogResult>> ToApplicationLogResult => i =>
                 new ApplicationLogResult
                 {
                   Id = i.Id,
                   UserId = i.UserId,
                   Action = i.Action,
                   ClientIp = i.ClientIP,
                   EmployeeCode = i.User.Employee.Code,
                   EmployeeFullName = i.User.Employee.FirstName + " " + i.User.Employee.LastName,
                   EmployeeId = i.User.Employee.Id,
                   LogTime = i.LogTime,
                   UserAgent = i.UserAgent,
                   UserName = i.User.UserName,
                   RequestEndTime = i.RequestEndTime,
                   RowVersion = i.RowVersion
                 };
    public IQueryable<ApplicationLogResult> SearchApplicationLogResult(
       IQueryable<ApplicationLogResult> query,
       string searchText,
       DateTime? fromDateTime,
       DateTime? toDateTime,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (fromDateTime != null)
        query = query.Where(i => i.LogTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.LogTime <= toDateTime);
      if (!string.IsNullOrWhiteSpace(searchText))
        query = query.Where(x =>
           x.Action.Contains(searchText) ||
           x.ClientIp.Contains(searchText) ||
           x.UserAgent.Contains(searchText) ||
           x.UserName.Contains(searchText) ||
           x.EmployeeFullName.Contains(searchText)
           );
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    public IQueryable<ApplicationLogGroupedMinMaxResult> SearchApplicationLogResult(
      IQueryable<ApplicationLogGroupedMinMaxResult> query,
      string searchText,
      DateTime? fromDateTime,
      DateTime? toDateTime,
      AdvanceSearchItem[] advanceSearchItems)
    {
      if (fromDateTime != null)
        query = query.Where(i => i.FirstRequestTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.FirstRequestTime <= toDateTime);
      if (!string.IsNullOrWhiteSpace(searchText))
        query = query.Where(x =>
           x.UserName.Contains(searchText) ||
           x.EmployeeFullName.Contains(searchText)
           );
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
  }
}
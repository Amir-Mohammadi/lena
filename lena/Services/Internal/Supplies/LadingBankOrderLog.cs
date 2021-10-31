using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
//using System.Data.Entity.SqlServer;
//using System.Data.Entity;
using lena.Models.Supplies.Ladings;
using lena.Models.UserManagement.User;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public LadingBankOrderLog AddLadingBankOrderLog(
        int ladingId,
         int ladingBankOrderStausId,
        string description)
    {
      var currentUser = App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString());
      var log = repository.Create<LadingBankOrderLog>();
      log.DateTime = DateTime.Now.ToUniversalTime();
      log.Description = description;
      log.LadingBankOrderStatusId = ladingBankOrderStausId;
      log.UserId = currentUser.UserId;
      log.LadingId = ladingId;
      var lading = GetLading(ladingId);
      lading.CurrentLadingBankOrderLog = log;
      repository.Add(log);
      return log;

    }
    #endregion
    public LadingBankOrderLog EditLadingBankOrderLog(
               int id,
       byte[] rowVersion,
       TValue<int> ladingBankOrderStausId = null,
       TValue<string> description = null)
    {
      var ladingBankOrderLog = GetLadingBankOrderLog(id);
      if (ladingBankOrderStausId != null)
        ladingBankOrderLog.LadingBankOrderStatusId = ladingBankOrderStausId;
      if (description != null)
        ladingBankOrderLog.Description = description;
      var log = repository.Update(ladingBankOrderLog, rowVersion);
      return log;
    }
    #region Gets
    public IQueryable<TResult> GetLadingBankOrderLogs<TResult>(
       Expression<Func<LadingBankOrderLog, TResult>> selector,
        TValue<int> id = null,
        TValue<int> ladingId = null,
        TValue<int> ladingBankOrderStausId = null,
        TValue<string> description = null,
        TValue<int> userId = null
     )
    {
      var query = repository.GetQuery<LadingBankOrderLog>();
      if (id != null)
        query = query.Where(a => a.Id == id);
      if (ladingBankOrderStausId != null)
        query = query.Where(a => a.LadingBankOrderStatusId == ladingBankOrderStausId);
      if (userId != null)
        query = query.Where(a => a.UserId == userId);
      if (description != null)
        query = query.Where(a => a.Description == description);
      if (ladingId != null)
        query = query.Where(a => a.LadingId == ladingId);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public LadingBankOrderLog GetLadingBankOrderLog(int id) => GetLadingBankOrderLog(selector: e => e, id: id);
    public TResult GetLadingBankOrderLog<TResult>(
           Expression<Func<LadingBankOrderLog, TResult>> selector,
        int id)
    {
      var ladingBankOrderLog = GetLadingBankOrderLogs(selector: selector, id: id)
            .FirstOrDefault();
      if (ladingBankOrderLog == null)
        throw new LadingBankOrderLogNotFoundException(id);
      return ladingBankOrderLog;
    }
    #endregion
    #region ToResult
    public Expression<Func<LadingBankOrderLog, LadingBankOrderLogResult>> ToLadingBankOrderLogResult =
        (ladingBankOrderLog) => new LadingBankOrderLogResult
        {
          Id = ladingBankOrderLog.Id,
          UserId = ladingBankOrderLog.UserId,
          UserFullName = ladingBankOrderLog.User.Employee.FirstName + " " + ladingBankOrderLog.User.Employee.LastName,
          DateTime = ladingBankOrderLog.DateTime,
          LadingBankOrderStatusId = ladingBankOrderLog.LadingBankOrderStatus.Id,
          LadingBankOrderStatusCode = ladingBankOrderLog.LadingBankOrderStatus.Code,
          LadingBankOrderStatusName = ladingBankOrderLog.LadingBankOrderStatus.Name,
          Description = ladingBankOrderLog.Description,
          RowVersion = ladingBankOrderLog.RowVersion
        };
    #endregion
    #region Delete
    public void DeleteLadingBankOrderLog(int id)
    {
      var ladingBankOrderLog = GetLadingBankOrderLog(id); ; var lading = GetLading(ladingBankOrderLog.LadingId)
     ;
      var log = GetLadingBankOrderLogs(selector: e => e, ladingId: lading.Id)
                .Where(x => x.Id != id)
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();
      lading.CurrentLadingBankOrderLog = log;
      repository.Delete(ladingBankOrderLog);
    }
    #endregion
  }
}
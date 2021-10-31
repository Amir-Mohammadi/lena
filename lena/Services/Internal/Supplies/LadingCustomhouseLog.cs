using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Supplies.Ladings;
using lena.Models.UserManagement.User;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public LadingCustomhouseLog AddLadingCustomhouseLog(
        int ladingId,
         int ladingCustomhouseStausId,
        string description)
    {
      var currentUser = App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString());
      var log = repository.Create<LadingCustomhouseLog>();
      log.DateTime = DateTime.Now.ToUniversalTime();
      log.Description = description;
      log.LadingCustomhouseStatusId = ladingCustomhouseStausId;
      log.UserId = currentUser.UserId;
      log.LadingId = ladingId;
      var lading = GetLading(ladingId);
      lading.CurrentLadingCustomhouseLog = log;
      repository.Add(log);
      return log;

    }
    #endregion
    #region Edit
    public LadingCustomhouseLog EditLadingCustomhouseLog(
               int id,
       byte[] rowVersion,
       TValue<int> ladingBankOrderStausId = null,
       TValue<string> description = null)
    {
      var ladingCustomhouseLog = GetLadingCustomhouseLog(id);
      if (ladingBankOrderStausId != null)
        ladingCustomhouseLog.LadingCustomhouseStatusId = ladingBankOrderStausId;
      if (description != null)
        ladingCustomhouseLog.Description = description;
      repository.Update(ladingCustomhouseLog, rowVersion);
      return ladingCustomhouseLog;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetLadingCustomhouseLogs<TResult>(
       Expression<Func<LadingCustomhouseLog, TResult>> selector,
        TValue<int> id = null,
        TValue<int> ladingId = null,
        TValue<int> ladingCustomhouseStausId = null,
        TValue<string> description = null,
        TValue<int> userId = null
     )
    {
      var query = repository.GetQuery<LadingCustomhouseLog>();
      if (id != null)
        query = query.Where(a => a.Id == id);
      if (ladingCustomhouseStausId != null)
        query = query.Where(a => a.LadingCustomhouseStatusId == ladingCustomhouseStausId);
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
    public LadingCustomhouseLog GetLadingCustomhouseLog(int id) => GetLadingCustomhouseLog(selector: e => e, id: id);
    public TResult GetLadingCustomhouseLog<TResult>(
           Expression<Func<LadingCustomhouseLog, TResult>> selector,
        int id)
    {
      var ladingCustomhouseLog = GetLadingCustomhouseLogs(selector: selector, id: id)
            .FirstOrDefault();
      if (ladingCustomhouseLog == null)
        throw new LadingCustomhouseLogNotFoundException(id);
      return ladingCustomhouseLog;
    }
    #endregion
    #region ToResult
    public Expression<Func<LadingCustomhouseLog, LadingCustomhouseLogResult>> ToLadingCustomhouseLogResult =
        (ladingCustomhouseLog) => new LadingCustomhouseLogResult
        {
          Id = ladingCustomhouseLog.Id,
          UserId = ladingCustomhouseLog.UserId,
          UserFullName = ladingCustomhouseLog.User.Employee.FirstName + " " + ladingCustomhouseLog.User.Employee.LastName,
          DateTime = ladingCustomhouseLog.DateTime,
          LadingCustomhouseStatusId = ladingCustomhouseLog.LadingCustomhouseStatus.Id,
          LadingCustomhouseStatusCode = ladingCustomhouseLog.LadingCustomhouseStatus.Code,
          LadingCustomhouseStatusName = ladingCustomhouseLog.LadingCustomhouseStatus.Name,
          Description = ladingCustomhouseLog.Description,
          RowVersion = ladingCustomhouseLog.RowVersion
        };
    #endregion
    #region Delete
    public void DeleteLadingCustomhouseLog(int id)
    {
      var ladingCustomhouseLog = GetLadingCustomhouseLog(id); ; var lading = GetLading(ladingCustomhouseLog.LadingId)
     ;
      var log = GetLadingCustomhouseLogs(selector: e => e, ladingId: lading.Id)
                .Where(x => x.Id != id)
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();
      lading.CurrentLadingCustomhouseLog = log;
      repository.Delete(ladingCustomhouseLog);
    }
    #endregion
  }
}
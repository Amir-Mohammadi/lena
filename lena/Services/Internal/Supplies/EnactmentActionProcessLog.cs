using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.UserManagement.User;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Supplies.EnactmentActionProcessLog;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public EnactmentActionProcessLog AddEnactmentActionProcessLog(
        int enactmentId,
         int enactmentActionProcessId,
        string description)
    {

      var currentUser = App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString());
      var log = repository.Create<EnactmentActionProcessLog>();
      log.DateTime = DateTime.Now.ToUniversalTime();
      log.Description = description;
      log.EnactmentActionProcessId = enactmentActionProcessId;
      log.UserId = currentUser.UserId;
      log.EnactmentId = enactmentId;
      repository.Add(log);
      var enactment = GetEnactment(enactmentId);
      enactment.CurrentEnactmentActionProcessLog = log;
      repository.Update(enactment, enactment.RowVersion);
      return log;
    }

    #endregion

    #region Edit
    public EnactmentActionProcessLog EditEnactmentActionProcessLog(
       int id,
       byte[] rowVersion,
       TValue<int> enactmentActionProcessId = null,
       TValue<string> description = null)
    {



      var enactmentActionProcessLog = GetEnactmentActionProcessLog(id);
      if (enactmentActionProcessId != null)
        enactmentActionProcessLog.EnactmentActionProcessId = enactmentActionProcessId;
      if (description != null)
        enactmentActionProcessLog.Description = description;

      repository.Update(enactmentActionProcessLog, rowVersion);
      return enactmentActionProcessLog;

    }
    #endregion

    #region Gets        
    public IQueryable<TResult> GetEnactmentActionProcessLogs<TResult>(
       Expression<Func<EnactmentActionProcessLog, TResult>> selector,
        TValue<int> id = null,
        TValue<int> enactmentId = null,
        TValue<int> enactmentActionProcessId = null,
        TValue<string> description = null,
        TValue<int> userId = null
     )
    {

      var query = repository.GetQuery<EnactmentActionProcessLog>();
      if (id != null)
        query = query.Where(a => a.Id == id);
      if (enactmentActionProcessId != null)
        query = query.Where(a => a.EnactmentActionProcessId == enactmentActionProcessId);
      if (userId != null)
        query = query.Where(a => a.UserId == userId);
      if (description != null)
        query = query.Where(a => a.Description == description);
      if (enactmentId != null)
        query = query.Where(a => a.Enactment.Id == enactmentId);

      return query.Select(selector);
    }
    #endregion

    #region Get
    public EnactmentActionProcessLog GetEnactmentActionProcessLog(int id) => GetEnactmentActionProcessLog(selector: e => e, id: id);
    public TResult GetEnactmentActionProcessLog<TResult>(
           Expression<Func<EnactmentActionProcessLog, TResult>> selector,
        int id)
    {

      var enactmentActionProcessLog = GetEnactmentActionProcessLogs(selector: selector, id: id)


            .FirstOrDefault();
      if (enactmentActionProcessLog == null)
        throw new EnactmentActionProcessLogNotFoundException(id);
      return enactmentActionProcessLog;
    }
    #endregion

    #region ToResult
    public Expression<Func<EnactmentActionProcessLog, EnactmentActionProcessLogResult>> ToEnactmentActionProcessLog =
        (enactmentActionProcessLog) => new EnactmentActionProcessLogResult
        {
          Id = enactmentActionProcessLog.Id,
          UserId = enactmentActionProcessLog.UserId,
          UserFullName = enactmentActionProcessLog.User.Employee.FirstName + " " + enactmentActionProcessLog.User.Employee.LastName,
          DateTime = enactmentActionProcessLog.DateTime,
          EnactmentActionProcessId = enactmentActionProcessLog.EnactmentActionProcess.Id,
          EnactmentActionProcessCode = enactmentActionProcessLog.EnactmentActionProcess.Code,
          EnactmentActionProcessName = enactmentActionProcessLog.EnactmentActionProcess.Name,
          Description = enactmentActionProcessLog.Description,
          RowVersion = enactmentActionProcessLog.RowVersion
        };
    #endregion

    #region Delete
    public void DeleteEnactmentActionProcessLog(int id)
    {

      var enactmentActionProcessLog = GetEnactmentActionProcessLog(id); ; var enactment = GetEnactment(enactmentActionProcessLog.Enactment.Id);
      var log = GetEnactmentActionProcessLogs(selector: e => e, enactmentId: enactment.Id)


                .Where(x => x.Id != id)
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();

      enactment.CurrentEnactmentActionProcessLog = log;
      repository.Delete(enactmentActionProcessLog);

    }
    #endregion

  }
}

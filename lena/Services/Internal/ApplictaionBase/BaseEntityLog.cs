using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Common;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    #region Get
    public BaseEntityLog GetBaseEntityLog(int id) => GetBaseEntityLog(selector: e => e, id: id);
    public TResult GetBaseEntityLog<TResult>(
        Expression<Func<BaseEntityLog, TResult>> selector,
        int id)
    {

      var baseEntityLog = GetBaseEntityLogs(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (baseEntityLog == null)
        throw new BaseEntityLogNotFoundException(id: id);
      return baseEntityLog;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetBaseEntityLogs<TResult>(
        Expression<Func<BaseEntityLog, TResult>> selector,
        TValue<int> id = null,
        TValue<int> baseEntityId = null,
        TValue<int> userId = null,
        TValue<DateTime> dateTime = null)
    {

      var query = repository.GetQuery<BaseEntityLog>();
      if (id != null) query =
                query.Where(x => x.Id == id);
      if (baseEntityId != null)
        query = query.Where(x => x.BaseEntityId == baseEntityId);
      if (userId != null)
        query = query.Where(x => x.UserId == userId);
      if (dateTime != null)
        query = query.Where(x => x.DateTime == dateTime);

      return query.Select(selector);
    }
    #endregion
    #region Add        
    public BaseEntityLog AddBaseEntityLog(
            BaseEntityLog concreteClass,
            int baseEntityId,
            int userId
                )
    {


      var baseEntityLog = concreteClass ?? repository.Create<BaseEntityLog>();
      baseEntityLog.BaseEntityId = baseEntityId;
      baseEntityLog.UserId = userId;
      baseEntityLog.DateTime = DateTime.Now;

      repository.Add(baseEntityLog);
      return baseEntityLog;
    }
    #endregion
    #region Edit
    public BaseEntityLog EditBaseEntityLog(
        int id,
        byte[] rowVersion,
        TValue<int> baseEntityId = null,
        TValue<int> userId = null
        )
    {

      var baseEntityLog = GetBaseEntityLog(id: id);
      return EditBaseEntityLog(
                baseEntityLog: baseEntityLog,
                rowVersion: rowVersion,
                 baseEntityId: baseEntityId,
                 userId: userId

                );

    }

    public BaseEntityLog EditBaseEntityLog(
                BaseEntityLog baseEntityLog,
                byte[] rowVersion,
                TValue<int> baseEntityId = null,
                TValue<int> userId = null


                )
    {


      if (baseEntityId != null) baseEntityLog.BaseEntityId = baseEntityId;
      if (userId != null) baseEntityLog.UserId = userId;

      repository.Update(rowVersion: rowVersion, entity: baseEntityLog);
      return baseEntityLog;
    }

    #endregion
    #region Delete
    public void DeleteBaseEntityLog(int id)
    {

      var baseEntityLog = GetBaseEntityLog(id: id);
      repository.Delete(baseEntityLog);
    }
    #endregion

  }
}

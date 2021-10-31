using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Common;
using lena.Models.WarehouseManagement.StuffFractionByProject;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Gets
    public IQueryable<TResult> GetStuffFractionTemporaryStuffs<TResult>(
        Expression<Func<StuffFractionTemporaryStuff, TResult>> selector,
        TValue<int> id = null,
        TValue<int> stuffId = null,
        TValue<string> projectCode = null,
        TValue<double> qty = null,
        TValue<int> userId = null,
        TValue<DateTime> dateTime = null
    )
    {

      var query = repository.GetQuery<StuffFractionTemporaryStuff>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (projectCode != null)
        query = query.Where(i => i.ProjectCode == projectCode);
      if (qty != null)
        query = query.Where(i => i.Qty == qty);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (dateTime != null)
        query = query.Where(i => i.DateTime == dateTime);

      return query.Select(selector);
    }
    #endregion
    #region Get
    public StuffFractionTemporaryStuff GetStuffFractionTemporaryStuff(int id) => GetStuffFractionTemporaryStuff(selector: e => e, id: id);
    public TResult GetStuffFractionTemporaryStuff<TResult>(Expression<Func<StuffFractionTemporaryStuff, TResult>> selector, int id)
    {

      var entity = GetStuffFractionTemporaryStuffs(
                selector: selector,
                id: id).FirstOrDefault();

      if (entity == null)
        throw new StuffFractionTemporaryStuffNotFoundException(id);

      return entity;
    }
    #endregion
    #region Add
    public StuffFractionTemporaryStuff AddStuffFractionTemporaryStuff(
        int stuffId,
        string projectCode,
        double qty,
        int userId,
        DateTime dateTime
        )
    {

      var entity = repository.Create<StuffFractionTemporaryStuff>();
      entity.StuffId = stuffId;
      entity.ProjectCode = projectCode;
      entity.Qty = qty;
      entity.UserId = userId;
      entity.DateTime = dateTime;
      repository.Add(entity);
      return entity;
    }
    #endregion
    #region Delete
    public void DeleteStuffFractionTemporaryStuff(StuffFractionTemporaryStuff entity, int id)
    {

      var e = entity == null ? GetStuffFractionTemporaryStuff(id) : entity;
      repository.Delete(e);
    }
    #endregion

    #region ToResult
    public Expression<Func<StuffFractionTemporaryStuff, StuffFractionTemporaryStuffResult>> ToStuffFractionTemporaryStuffResult =
       result => new StuffFractionTemporaryStuffResult()
       {
         Id = result.Id,
         StuffId = result.StuffId,
         StuffCode = result.Stuff.Code,
         StuffName = result.Stuff.Title,
         ProjectCode = result.ProjectCode,
         UserId = result.UserId,
         UserFullName = result.User.Employee.FirstName + " " + result.User.Employee.LastName,
         Qty = result.Qty,
         DateTime = result.DateTime
       };

    #endregion



  }
}

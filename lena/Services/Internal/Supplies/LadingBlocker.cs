using System.Linq;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using System;
using lena.Models.Supplies.LadingBlocker;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Get
    public LadingBlocker GetLadingBlocker(int id) => GetLadingBlocker(selector: e => e, id: id);
    public TResult GetLadingBlocker<TResult>(
        Expression<Func<LadingBlocker, TResult>> selector,
        int id)
    {

      var ladingBlocker = GetLadingBlockers(selector: selector, id: id)


                .FirstOrDefault();
      if (ladingBlocker == null)
        throw new LadingBlockerNotFoundException(id);
      return ladingBlocker;
    }
    #endregion
    #region Gets[
    public IQueryable<TResult> GetLadingBlockers<TResult>(
        Expression<Func<LadingBlocker, TResult>> selector,
        TValue<int> id = null,
        TValue<int> userGroupId = null)
    {

      var query = repository.GetQuery<LadingBlocker>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (userGroupId != null)
        query = query.Where(x => x.UserGroupId == userGroupId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public LadingBlocker AddLadingBlocker(
        string title,
        int userGroupId)
    {

      var entity = repository.Create<LadingBlocker>();
      entity.Title = title;
      entity.UserGroupId = userGroupId;
      repository.Add(entity);
      return entity;
    }
    #endregion
    #region Edit
    public LadingBlocker EditLadingBlocker(
        int id,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<int> userGroupId = null)
    {

      var ladingBlocker = GetLadingBlocker(id: id);
      if (title != null)
        ladingBlocker.Title = title;
      if (userGroupId != null)
        ladingBlocker.UserGroupId = userGroupId;
      repository.Update(rowVersion: rowVersion, entity: ladingBlocker);
      return ladingBlocker;
    }
    #endregion
    #region Delete
    public void DeleteLadingBlocker(int id)
    {

      var ladingBlocker = GetLadingBlocker(id: id);
      repository.Delete(ladingBlocker);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<LadingBlockerResult> SortLadingBlockerResult(
        IQueryable<LadingBlockerResult> query,
        SortInput<LadingBlockerSortType> sort)
    {
      switch (sort.SortType)
      {
        case LadingBlockerSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case LadingBlockerSortType.UserGroupName:
          return query.OrderBy(a => a.UserGroupName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<LadingBlockerResult> SearchLadingBlockerResult(
        IQueryable<LadingBlockerResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where item.Title.Contains(search) ||
                item.UserGroupName.Contains(search)
                select item;
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<LadingBlocker, LadingBlockerResult>> ToLadingBlockerResult =
        ladingBlocker => new LadingBlockerResult
        {
          Id = ladingBlocker.Id,
          Title = ladingBlocker.Title,
          UserGroupId = ladingBlocker.UserGroupId,
          UserGroupName = ladingBlocker.UserGroup.Name,
          RowVersion = ladingBlocker.RowVersion
        };
    #endregion
  }
}

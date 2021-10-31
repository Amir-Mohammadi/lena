using System.Linq;
using lena.Services.Core;
using lena.Services.Core.Foundation;
////using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using System;
using lena.Models.QualityControl.QualityControlAccepter;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Get
    public QualityControlAccepter GetQualityControlAccepter(int id) => GetQualityControlAccepter(selector: e => e, id: id);
    public TResult GetQualityControlAccepter<TResult>(
        Expression<Func<QualityControlAccepter, TResult>> selector,
        int id)
    {

      var qualityControlAccepter = GetQualityControlAccepters(selector: selector, id: id)


                .FirstOrDefault();
      if (qualityControlAccepter == null)
        throw new QualityControlAccepterNotFoundException(id);
      return qualityControlAccepter;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetQualityControlAccepters<TResult>(
        Expression<Func<QualityControlAccepter, TResult>> selector,
        TValue<int> id = null,
        TValue<int> userGroupId = null)
    {

      var query = repository.GetQuery<QualityControlAccepter>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (userGroupId != null)
        query = query.Where(x => x.UserGroupId == userGroupId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public QualityControlAccepter AddQualityControlAccepter(
        string title,
        int userGroupId)
    {

      var entity = repository.Create<QualityControlAccepter>();
      entity.Title = title;
      entity.UserGroupId = userGroupId;
      repository.Add(entity);
      return entity;
    }
    #endregion
    #region Edit
    public QualityControlAccepter EditQualityControlAccepter(
        int id,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<int> userGroupId = null)
    {

      var qualityControlAccepter = GetQualityControlAccepter(id: id);
      if (title != null)
        qualityControlAccepter.Title = title;
      if (userGroupId != null)
        qualityControlAccepter.UserGroupId = userGroupId;
      repository.Update(rowVersion: rowVersion, entity: qualityControlAccepter);
      return qualityControlAccepter;
    }
    #endregion
    #region Delete
    public void DeleteQualityControlAccepter(int id)
    {

      var qualityControlAccepter = GetQualityControlAccepter(id: id);
      repository.Delete(qualityControlAccepter);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<QualityControlAccepterResult> SortQualityControlAccepterResult(
        IQueryable<QualityControlAccepterResult> query,
        SortInput<QualityControlAccepterSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlAccepterSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case QualityControlAccepterSortType.UserGroupName:
          return query.OrderBy(a => a.UserGroupName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<QualityControlAccepterResult> SearchQualityControlAccepterResult(
        IQueryable<QualityControlAccepterResult> query,
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
    public Expression<Func<QualityControlAccepter, QualityControlAccepterResult>> ToQualityControlAccepterResult =
        qualityControlAccepter => new QualityControlAccepterResult
        {
          Id = qualityControlAccepter.Id,
          Title = qualityControlAccepter.Title,
          UserGroupId = qualityControlAccepter.UserGroupId,
          UserGroupName = qualityControlAccepter.UserGroup.Name,
          RowVersion = qualityControlAccepter.RowVersion
        };
    #endregion
  }
}

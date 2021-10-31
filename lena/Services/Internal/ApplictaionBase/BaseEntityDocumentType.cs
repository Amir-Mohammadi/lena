using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.ApplicationBase.BaseEntityDocumentType;
using lena.Models.Common;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    #region Get
    public BaseEntityDocumentType GetBaseEntityDocumentType(int id) =>
        GetBaseEntityDocumentType(selector: e => e, id: id);
    public TResult GetBaseEntityDocumentType<TResult>(
        Expression<Func<BaseEntityDocumentType, TResult>> selector,
        int id)
    {

      var baseEntityDocumentType = GetBaseEntityDocumentTypes(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (baseEntityDocumentType == null)
        throw new BaseEntityDocumentTypeNotFoundException(id);
      return baseEntityDocumentType;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetBaseEntityDocumentTypes<TResult>(
        Expression<Func<BaseEntityDocumentType, TResult>> selector,
        TValue<int> id = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<EntityType?> entityType = null)
    {

      var query = repository.GetQuery<BaseEntityDocumentType>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (title != null)
        query = query.Where(x => x.Title == title);
      if (description != null)
        query = query.Where(x => x.Description == description);
      if (entityType != null)
        query = query.Where(x => x.EntityType == entityType);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public BaseEntityDocumentType AddBaseEntityDocumentType(
        string title,
        string description,
        EntityType? entityType
    )
    {

      var baseEntityDocumentType = repository.Create<BaseEntityDocumentType>();
      baseEntityDocumentType.Title = title;
      baseEntityDocumentType.Description = description;
      baseEntityDocumentType.EntityType = entityType;
      repository.Add(baseEntityDocumentType);
      return baseEntityDocumentType;
    }
    #endregion
    #region Edit
    public BaseEntityDocumentType EditBaseEntityDocumentType(
        int id,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<EntityType?> entityType = null)
    {

      var baseEntityDocumentType = GetBaseEntityDocumentType(id: id);
      return EditBaseEntityDocumentType(
                    baseEntityDocumentType: baseEntityDocumentType,
                    rowVersion: rowVersion,
                    title: title,
                    description: description,
                    entityType: entityType);
    }
    public BaseEntityDocumentType EditBaseEntityDocumentType(
        BaseEntityDocumentType baseEntityDocumentType,
        byte[] rowVersion,
        TValue<int> id = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<EntityType?> entityType = null)
    {

      if (id != null)
        baseEntityDocumentType.Id = id;
      if (title != null)
        baseEntityDocumentType.Title = title;
      if (description != null)
        baseEntityDocumentType.Description = description;
      if (entityType != null)
        baseEntityDocumentType.EntityType = entityType;
      repository.Update(rowVersion: rowVersion, entity: baseEntityDocumentType);
      return baseEntityDocumentType;
    }
    #endregion
    #region Delete
    public void DeleteBaseEntityDocumentType(int id)
    {

      var baseEntityDocumentType = GetBaseEntityDocumentType(id: id);
      repository.Delete(baseEntityDocumentType);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<BaseEntityDocumentTypeResult> SortBaseEntityDocumentTypeResult(
        IQueryable<BaseEntityDocumentTypeResult> query,
        SortInput<BaseEntityDocumentTypeSortType> sort)
    {
      switch (sort.SortType)
      {
        case BaseEntityDocumentTypeSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case BaseEntityDocumentTypeSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case BaseEntityDocumentTypeSortType.EntityType:
          return query.OrderBy(a => a.EntityType, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<BaseEntityDocumentTypeResult> SearchBaseEntityDocumentTypeResult(
        IQueryable<BaseEntityDocumentTypeResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
      {
        query = from item in query
                where
                    item.Title.Contains(searchText) ||
                    item.Description.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<BaseEntityDocumentType, BaseEntityDocumentTypeResult>> ToBaseEntityDocumentTypeResult =
        baseEntityDocumentType => new BaseEntityDocumentTypeResult
        {
          Id = baseEntityDocumentType.Id,
          Title = baseEntityDocumentType.Title,
          Description = baseEntityDocumentType.Description,
          EntityType = baseEntityDocumentType.EntityType,
          RowVersion = baseEntityDocumentType.RowVersion
        };
    #endregion
  }
}

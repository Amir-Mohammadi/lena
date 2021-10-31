using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.StuffPurchaseCategory;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Gets
    public IQueryable<TResult> GetStuffPurchaseCategorys<TResult>(
        Expression<Func<StuffPurchaseCategory, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> title = null,
        TValue<int> stuffDefinitionUserGroupId = null,
        TValue<int> stuffDefinitionConfirmerUserGroupId = null,
        TValue<int> qualityControlUserGroupId = null,
        TValue<int> qualityControlDepartmentId = null)
    {

      var query = repository.GetQuery<StuffPurchaseCategory>();

      if (id != null)
        query = query.Where(i => i.Id == id);
      if (title != null)
        query = query.Where(i => i.Title == title);
      if (code != null)
        query = query.Where(i => i.Code == code);
      if (stuffDefinitionUserGroupId != null)
        query = query.Where(i => i.StuffDefinitionUserGroupId == stuffDefinitionUserGroupId);
      if (stuffDefinitionConfirmerUserGroupId != null)
        query = query.Where(i => i.StuffDefinitionConfirmerUserGroupId == stuffDefinitionConfirmerUserGroupId);
      if (qualityControlUserGroupId != null)
        query = query.Where(i => i.QualityControlUserGroupId == qualityControlUserGroupId);
      if (qualityControlDepartmentId != null)
        query = query.Where(i => i.QualityControlDepartmentId == qualityControlDepartmentId);
      return query.Select(selector);
    }
    #endregion
    #region Get

    public StuffPurchaseCategory GetStuffPurchaseCategory(int id) => GetStuffPurchaseCategory(selector: e => e, id: id);
    public TResult GetStuffPurchaseCategory<TResult>(Expression<Func<StuffPurchaseCategory, TResult>> selector, int id)
    {

      var stuffPurchaseCategory = GetStuffPurchaseCategorys(
                    selector: selector,
                    id: id)


                .SingleOrDefault();
      if (stuffPurchaseCategory == null)
        throw new StuffPurchaseCategoryNotFoundException(id);
      return stuffPurchaseCategory;
    }
    #endregion
    #region Add
    public StuffPurchaseCategory AddStuffPurchaseCategory(
        string code,
        string title,
        string description,
        int stuffDefinitionUserGroupId,
        int stuffDefinitionConfirmerUserGroupId,
        int qualityControlUserGroupId,
        short qualityControlDepartmentId)
    {

      var entity = repository.Create<StuffPurchaseCategory>();
      entity.Code = code;
      entity.Title = title;
      entity.Description = description;
      entity.StuffDefinitionUserGroupId = stuffDefinitionUserGroupId;
      entity.StuffDefinitionConfirmerUserGroupId = stuffDefinitionConfirmerUserGroupId;





      entity.QualityControlUserGroupId = qualityControlUserGroupId;
      entity.QualityControlDepartmentId = qualityControlDepartmentId;
      repository.Add(entity);
      return entity;
    }
    #endregion
    #region Edit
    public StuffPurchaseCategory EditStuffPurchaseCategory(
        byte[] rowVersion,
        int id,
        TValue<string> code,
        TValue<string> title,
        TValue<string> description,
        TValue<int> stuffDefinitionUserGroupId,
        TValue<int> stuffDefinitionConfirmerUserGroupId,
        TValue<int> qualityControlUserGroupId = null,
        TValue<short> qualityControlDepartmentId = null
        )
    {

      var entity = GetStuffPurchaseCategory(id);

      if (code != null)
        entity.Code = code;

      if (title != null)
        entity.Title = title;

      if (description != null)
        entity.Description = description;

      if (stuffDefinitionUserGroupId != null)
        entity.StuffDefinitionUserGroupId = stuffDefinitionUserGroupId;

      if (stuffDefinitionConfirmerUserGroupId != null)
        entity.StuffDefinitionConfirmerUserGroupId = stuffDefinitionConfirmerUserGroupId;

      if (qualityControlUserGroupId != null)
        entity.QualityControlUserGroupId = qualityControlUserGroupId;

      if (qualityControlDepartmentId != null)
        entity.QualityControlDepartmentId = qualityControlDepartmentId;
      repository.Update(entity, rowVersion: rowVersion);
      return entity;
    }
    #endregion
    #region Delete
    public void DeleteStuffPurchaseCategory(int id)
    {

      var stuffPurchaseCategory = GetStuffPurchaseCategory(id: id);

      int[] stuffPurchaseCategoryDetailsIds = stuffPurchaseCategory.Details.Select(s => s.Id).ToArray();

      foreach (var item in stuffPurchaseCategoryDetailsIds)
        DeleteStuffPurchaseCategoryDetail(item);

      repository.Delete(stuffPurchaseCategory);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffPurchaseCategoryResult> SortStuffPurchaseCategoryResult(IQueryable<StuffPurchaseCategoryResult> input, SortInput<StuffPurchaseCategorySortType> options)
    {
      switch (options.SortType)
      {
        case StuffPurchaseCategorySortType.Code:
          return input.OrderBy(i => i.Code, options.SortOrder);
        case StuffPurchaseCategorySortType.Title:
          return input.OrderBy(i => i.Title, options.SortOrder);
        case StuffPurchaseCategorySortType.Description:
          return input.OrderBy(i => i.Description, options.SortOrder);
        case StuffPurchaseCategorySortType.StuffDefinitionUserGroupName:
          return input.OrderBy(i => i.StuffDefinitionUserGroupName, options.SortOrder);
        case StuffPurchaseCategorySortType.StuffDefinitionConfirmerUserGroupName:
          return input.OrderBy(i => i.StuffDefinitionConfirmerUserGroupName, options.SortOrder);
        case StuffPurchaseCategorySortType.QualityControlUserGroupName:
          return input.OrderBy(i => i.QualityControlUserGroupName, options.SortOrder);
        case StuffPurchaseCategorySortType.QualityControlDepartmentName:
          return input.OrderBy(i => i.QualityControlDepartmentName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<StuffPurchaseCategoryResult> SearchStuffPurchaseCategoryResultQuery(IQueryable<StuffPurchaseCategoryResult> query, string searchText, AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
      {
        query = from q in query
                where q.Title.Contains(searchText) ||
                q.StuffDefinitionConfirmerUserGroupName.Contains(searchText) ||
                q.StuffDefinitionUserGroupName.Contains(searchText) ||
                q.Code.Contains(searchText) ||
                q.Description.Contains(searchText) ||
                q.QualityControlUserGroupName.Contains(searchText) ||
                q.QualityControlDepartmentName.Contains(searchText)
                select q;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;


    }
    #endregion
    #region ToResult
    public Expression<Func<StuffPurchaseCategory, StuffPurchaseCategoryResult>> ToStuffPurchaseCategoryResult =>
        stuffPurchaseCategory => new StuffPurchaseCategoryResult
        {
          Id = stuffPurchaseCategory.Id,
          Code = stuffPurchaseCategory.Code,
          Title = stuffPurchaseCategory.Title,
          Description = stuffPurchaseCategory.Description,
          StuffDefinitionUserGroupId = stuffPurchaseCategory.StuffDefinitionUserGroupId,
          StuffDefinitionUserGroupName = stuffPurchaseCategory.StuffDefinitionUserGroup.Name,
          StuffDefinitionConfirmerUserGroupId = stuffPurchaseCategory.StuffDefinitionConfirmerUserGroupId,
          StuffDefinitionConfirmerUserGroupName = stuffPurchaseCategory.StuffDefinitionConfirmerUserGroup.Name,
          QualityControlUserGroupId = stuffPurchaseCategory.QualityControlUserGroupId,
          QualityControlUserGroupName = stuffPurchaseCategory.QualityControlUserGroup.Name,
          QualityControlDepartmentId = stuffPurchaseCategory.QualityControlDepartmentId,
          QualityControlDepartmentName = stuffPurchaseCategory.QualityControlDepartment.Name,
          RowVersion = stuffPurchaseCategory.RowVersion
        };
    #endregion
    #region ToFullResult
    public Expression<Func<StuffPurchaseCategory, FullStuffPurchaseCategoryResult>> ToStuffPurchaseCategoryFullResult =>
        stuffPurchaseCategory => new FullStuffPurchaseCategoryResult()
        {
          Id = stuffPurchaseCategory.Id,
          Code = stuffPurchaseCategory.Code,
          Title = stuffPurchaseCategory.Title,
          Description = stuffPurchaseCategory.Description,
          StuffDefinitionUserGroupId = stuffPurchaseCategory.StuffDefinitionUserGroupId,
          StuffDefinitionUserGroupName = stuffPurchaseCategory.StuffDefinitionUserGroup.Name,
          StuffDefinitionConfirmerUserGroupId = stuffPurchaseCategory.StuffDefinitionConfirmerUserGroupId,
          StuffDefinitionConfirmerUserGroupName = stuffPurchaseCategory.StuffDefinitionConfirmerUserGroup.Name,
          Details = stuffPurchaseCategory.Details.AsQueryable().Select(ToStuffPurchaseCategoryDetailResult),
          QualityControlUserGroupId = stuffPurchaseCategory.QualityControlUserGroupId,
          QualityControlUserGroupName = stuffPurchaseCategory.QualityControlUserGroup.Name,
          QualityControlDepartmentId = stuffPurchaseCategory.QualityControlDepartmentId,
          QualityControlDepartmentName = stuffPurchaseCategory.QualityControlDepartment.Name,
          RowVersion = stuffPurchaseCategory.RowVersion
        };
    #endregion
    #region ToComboResult
    public Expression<Func<StuffPurchaseCategory, StuffPurchaseCategoryComboResult>> ToStuffPurchaseCategoryComboResult =>
        stuffPurchaseCategory => new StuffPurchaseCategoryComboResult()
        {
          Id = stuffPurchaseCategory.Id,
          Name = stuffPurchaseCategory.Title,
          RowVersion = stuffPurchaseCategory.RowVersion
        };
    #endregion
  }
}

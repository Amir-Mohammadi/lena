using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.ApplicationBase.StuffCategory;
using lena.Models.Common;
using lena.Models.StuffCategory;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Add
    public StuffCategory AddStuffCategory(
        string name,
        string description,
        bool isActive,
        short? parentStuffCategoryId,
        short defaultWarehouseId)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentNullException(name);
      var stuffCategory = repository.Create<StuffCategory>();
      if (parentStuffCategoryId != null)
      {
        var haveStuff = GetStuffs(selector: e => e, stuffCategoryId: parentStuffCategoryId);
        if (haveStuff.Any())
          throw new StuffCategoryHaveStuffException(parentStuffCategoryId.Value);
      }
      stuffCategory.Name = name;
      stuffCategory.IsActive = isActive;
      stuffCategory.Description = description;
      stuffCategory.ParentStuffCategoryId = parentStuffCategoryId;
      stuffCategory.DefaultWarehouseId = defaultWarehouseId;
      repository.Add(stuffCategory);
      return stuffCategory;
    }
    #endregion
    #region Edit
    public StuffCategory EditStuffCategory(
        int id,
        byte[] rowVersion,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<bool> isActive = null,
        TValue<short?> parentStuffCategoryId = null,
        TValue<short> defaultWarehouseId = null
    )
    {
      StuffCategory stuffCategory = GetStuffCategory(id: id);
      if (stuffCategory == null)
        throw new StuffCategoryNotFoundException(id);
      if (parentStuffCategoryId != null)
      {
        var haveStuff = GetStuffs(selector: e => e, stuffCategoryId: parentStuffCategoryId.Value);
        if (haveStuff.Any())
          throw new StuffCategoryHaveStuffException(parentStuffCategoryId);
      }
      if (name != null)
        stuffCategory.Name = name;
      if (description != null)
        stuffCategory.Description = description;
      if (isActive != null)
        stuffCategory.IsActive = isActive;
      if (parentStuffCategoryId != null)
        stuffCategory.ParentStuffCategoryId = parentStuffCategoryId;
      if (defaultWarehouseId != null)
        stuffCategory.DefaultWarehouseId = defaultWarehouseId;
      repository.Update(entity: stuffCategory, rowVersion: stuffCategory.RowVersion);
      return stuffCategory;
    }
    #endregion
    #region Delete
    public void DeleteStuffCategory(int id)
    {
      var stuffCategory = GetStuffCategory(id: id);
      if (stuffCategory == null)
        throw new StuffCategoryNotFoundException(id);
      repository.Delete(stuffCategory);
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStuffCategories<TResult>(
        Expression<Func<StuffCategory, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<bool> isActive = null,
        TValue<int?> parentStuffCategoryId = null,
        TValue<int> defaultWarehouseId = null
    )
    {
      var query = repository.GetQuery<StuffCategory>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);
      if (parentStuffCategoryId != null)
        query = query.Where(i => i.ParentStuffCategoryId == parentStuffCategoryId);
      if (defaultWarehouseId != null)
        query = query.Where(i => i.DefaultWarehouseId == defaultWarehouseId);
      return query.Select(selector);
    }
    public List<StuffCategoryTreeResult> GetStuffCategoriesTree(
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<bool> isActive = null,
        TValue<string> searchText = null)
    {
      var query = repository.GetQuery<StuffCategory>()
            .Where(i => i.ParentStuffCategoryId == null);
      if (searchText != null && !string.IsNullOrEmpty(searchText))
        query = query.Where(x => x.Name.Contains(searchText) ||
              x.Description.Contains(searchText) ||
              x.DefaultWarehouse.Name.Contains(searchText));
      var rootCategories = query.ToList();
      var result = new List<StuffCategoryTreeResult>();
      foreach (var item in rootCategories)
      {
        if (id != null && id.Value != item.Id)
          continue;
        if (name != null && name.Value != item.Name)
          continue;
        if (description != null && description.Value != item.Description)
          continue;
        if (isActive != null && isActive.Value != item.IsActive)
          continue;
        var nCat = new StuffCategoryTreeResult()
        {
          Id = item.Id,
          Name = item.Name,
          Description = item.Description,
          IsActive = item.IsActive,
          RowVersion = item.RowVersion,
          CategoryLevel = 1,
          DefaultWarehouseId = item.DefaultWarehouseId,
          DefaultWarehouseName = item.DefaultWarehouse.Name,
          ChildCategories = GetStuffCategoriesTree(item, 1, id, name, description, isActive, searchText)
        };
        result.Add(nCat);
      }
      return result;
    }
    private IList<StuffCategoryTreeResult> GetStuffCategoriesTree(
        StuffCategory rootCategory,
        int categoryLevel,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<bool> isActive = null,
        TValue<string> searchText = null)
    {
      var result = new List<StuffCategoryTreeResult>();
      foreach (var item in rootCategory.SubStuffCategories)
      {
        if (id != null && id.Value != item.Id)
          continue;
        if (name != null && name.Value != item.Name)
          continue;
        if (description != null && description.Value != item.Description)
          continue;
        if (isActive != null && isActive.Value != item.IsActive)
          continue;
        if (searchText != null && !string.IsNullOrEmpty(searchText))
          if (!item.Name.Contains(searchText) && !item.Description.Contains(searchText) && !item.DefaultWarehouse.Name.Contains(searchText))
            continue;
        var catResult = new StuffCategoryTreeResult()
        {
          Id = item.Id,
          Name = item.Name,
          Description = item.Description,
          IsActive = item.IsActive,
          RowVersion = item.RowVersion,
          CategoryLevel = categoryLevel + 1,
          DefaultWarehouseId = item.DefaultWarehouseId,
          DefaultWarehouseName = item.DefaultWarehouse.Name,
          ChildCategories = GetStuffCategoriesTree(item, categoryLevel + 1, id, name, description, isActive)
        };
        result.Add(catResult);
      }
      return result;
    }
    #endregion
    #region Get
    public StuffCategory GetStuffCategory(int id) => GetStuffCategory(selector: e => e, id: id);
    public TResult GetStuffCategory<TResult>(
        Expression<Func<StuffCategory, TResult>> selector,
        int id)
    {
      var stuffCategory = GetStuffCategories(
                selector: selector,
                id: id)
                .FirstOrDefault();
      if (stuffCategory == null)
        throw new StuffCategoryNotFoundException(id);
      return stuffCategory;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffCategoryResult> SortStuffCategoryResult(IQueryable<StuffCategoryResult> input,
        SortInput<StuffCategorySortType> options)
    {
      switch (options.SortType)
      {
        case StuffCategorySortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        case StuffCategorySortType.Description:
          return input.OrderBy(i => i.Description, options.SortOrder);
        case StuffCategorySortType.IsActive:
          return input.OrderBy(i => i.IsActive, options.SortOrder);
        case StuffCategorySortType.ParentStuffCategoryName:
          return input.OrderBy(i => i.ParentStuffCategoryName, options.SortOrder);
        case StuffCategorySortType.DefaultWarehouseName:
          return input.OrderBy(i => i.DefaultWarehouseName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<StuffCategoryTreeResult> SortStuffCategoryTreeResult(IQueryable<StuffCategoryTreeResult> input,
       SortInput<StuffCategorySortType> options)
    {
      switch (options.SortType)
      {
        case StuffCategorySortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        case StuffCategorySortType.Description:
          return input.OrderBy(i => i.Description, options.SortOrder);
        case StuffCategorySortType.IsActive:
          return input.OrderBy(i => i.IsActive, options.SortOrder);
        case StuffCategorySortType.DefaultWarehouseName:
          return input.OrderBy(i => i.DefaultWarehouseName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<StuffCategory, StuffCategoryResult>> ToStuffCategoryResult =
        stuffCategory => new StuffCategoryResult()
        {
          Id = stuffCategory.Id,
          Name = stuffCategory.Name,
          Description = stuffCategory.Description,
          IsActive = stuffCategory.IsActive,
          ParentStuffCategoryId = stuffCategory.ParentStuffCategoryId,
          ParentStuffCategoryName = stuffCategory.ParentStuffCategory.Name,
          DefaultWarehouseId = stuffCategory.DefaultWarehouseId,
          DefaultWarehouseName = stuffCategory.DefaultWarehouse.Name,
          RowVersion = stuffCategory.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<StuffCategoryResult> SearchStuffCategoryResult(
        IQueryable<StuffCategoryResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(r => r.Name.Contains(searchText) || r.Description.Contains(searchText) || r.ParentStuffCategoryName.Contains(searchText));
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region ToComboResult
    public Expression<Func<StuffCategory, StuffCategoryComboResult>> ToStuffCategoryComboResult =
        stuffCategory => new StuffCategoryComboResult()
        {
          Id = stuffCategory.Id,
          Name = stuffCategory.Name,
          ParentStuffCategoryId = stuffCategory.ParentStuffCategoryId,
          ParentStuffCategoryName = stuffCategory.ParentStuffCategory.Name
        };
    #endregion
    #region SortCombo
    public IOrderedQueryable<StuffCategoryComboResult> SortStuffCategoryComboResult(
        IQueryable<StuffCategoryComboResult> query, SortInput<StuffCategoryComboSortType> type)
    {
      switch (type.SortType)
      {
        case StuffCategoryComboSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case StuffCategoryComboSortType.Name:
          return query.OrderBy(a => a.Name, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
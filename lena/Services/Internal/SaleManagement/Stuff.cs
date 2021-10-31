using System;
//using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
using Microsoft.EntityFrameworkCore;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.QualityControl.Exception;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Services.Internals.Supplies.Exception;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Stuff;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Add
    public Stuff AddStuff(
        string name,
        string noun,
        string title,
        string code,
        bool isActive,
        bool isTraceable,
        string description,
        short stuffCategoryId,
        short? qualityControlDepartmentId,
        int? qualityControlEmployeeId,
        byte unitTypeId,
        int stockSafety,
        double faultyPercentage,
        StuffType stuffType,
        bool needToQualityControl,
        bool needToQualityControlDocumentUpload,
        short ceoficcientSet,
        double tolerance,
        double? netWeight,
        double? grossWeight,
        double? volume,
        int? stuffHSGroupId,
        int? stuffPurchaseCategoryId,
        int? qualityControlCheckDuration,
        int? stuffDefinitionRequestId = null)
    {
      var stuff = repository.Create<Stuff>();
      var existStuff = GetStuffs(
                    selector: e => e,
                    code: code);
      if (code.Length != 4)
      {
        throw new CodeExistsException(code);
      }
      if (code == null || code == "")
      {
        var stuffCode = GetStuffs(selector: e => e.Code)
                  .Where(x => DBFunctions.IsNumeric(x))
                  .Max();
        stuff.Code = (Convert.ToInt32(stuffCode) + 1).ToString();
      }
      else
        stuff.Code = code;
      var isParent = GetStuffCategories(selector: e => e, parentStuffCategoryId: stuffCategoryId);
      if (isParent.Any())
        throw new CantAssignStuffToParentStuffCategory(stuffCategoryId);
      stuff.Name = name;
      stuff.Noun = noun;
      stuff.Title = title;
      stuff.IsActive = isActive;
      stuff.IsTraceable = isTraceable;
      stuff.Description = description;
      stuff.StuffCategoryId = stuffCategoryId;
      stuff.QualityControlEmployeeId = qualityControlEmployeeId;
      stuff.QualityControlDepartmentId = qualityControlDepartmentId;
      stuff.UnitTypeId = unitTypeId;
      stuff.StuffType = stuffType;
      stuff.FaultyPercentage = faultyPercentage;
      stuff.StockSafety = stockSafety;
      stuff.NeedToQualityControl = needToQualityControl;
      stuff.Tolerance = tolerance;
      stuff.NetWeight = netWeight;
      stuff.GrossWeight = grossWeight;
      stuff.Volume = volume;
      stuff.StuffHSGroupId = stuffHSGroupId;
      stuff.NeedToQualityControlDocumentUpload = needToQualityControlDocumentUpload;
      stuff.StuffPurchaseCategoryId = stuffPurchaseCategoryId;
      stuff.QualityControlCheckDuration = qualityControlCheckDuration;
      stuff.CeofficientSet = ceoficcientSet;
      if (stuffDefinitionRequestId != null)
      {
        var stuffDefRequest = App.Internals.ApplicationBase.GetStuffDefinitionRequest(stuffDefinitionRequestId.Value);
        stuff.StuffDefinitionRequest = stuffDefRequest;
      }
      if (stuff.NeedToQualityControl == true && stuff.QualityControlDepartmentId == null)
        throw new QualityControlDepartmentNotDefinedException();
      repository.Add(stuff);
      return stuff;
    }
    #endregion
    #region Edit
    public Stuff EditStuff(
        byte[] rowVersion,
        int id,
        TValue<string> name = null,
        TValue<string> noun = null,
        TValue<string> title = null,
        TValue<string> code = null,
        TValue<bool> isActive = null,
        TValue<bool> isTraceable = null,
        TValue<string> description = null,
        TValue<short> stuffCategoryId = null,
        TValue<short> qualityControlDepartmentId = null,
        TValue<int> qualityControlEmployeeId = null,
        TValue<byte> unitTypeId = null,
        TValue<int> stockSafety = null,
        TValue<double> faultyPercentage = null,
        TValue<StuffType> stuffType = null,
        TValue<bool> needToQualityControl = null,
        TValue<bool> needToQualityControlDocumentUpload = null,
        TValue<double> tolerance = null,
        TValue<double?> netWeight = null,
        TValue<double?> grossWeight = null,
        TValue<double?> volume = null,
        TValue<int?> stuffHSGroupId = null,
        TValue<int?> qualityControlCheckDuration = null,
        TValue<int?> stuffPurchaseCategoryId = null,
        TValue<short> ceofficientSet = null
        )
    {
      var stuff = GetStuff(id: id);
      var existStuff = GetStuffs(selector: e => e,
                    code: code);
      if (existStuff.Any(i => i.Id != id))
        throw new StuffExistsException(code);
      var isParent = GetStuffCategories(selector: e => e, parentStuffCategoryId: stuffCategoryId.Value);
      if (isParent.Any())
        throw new CantAssignStuffToParentStuffCategory(stuffCategoryId);
      if (name != null)
        stuff.Name = name;
      if (noun != null)
        stuff.Noun = noun;
      if (title != null)
        stuff.Title = title;
      if (code != null)
      {
        if (code.Value.Length != 4)
          throw new CodeExistsException(code);
        var stuffSerials = App.Internals.WarehouseManagement.GetStuffSerials(e => e, stuffId: id, stuffCode: stuff.Code);
        if (stuffSerials.Any())
          throw new StuffCodeCanNotEditException(code: code);
        stuff.Code = code;
      }
      if (isActive != null)
        stuff.IsActive = isActive;
      if (isTraceable != null)
        stuff.IsTraceable = isTraceable;
      if (qualityControlDepartmentId != null)
        stuff.QualityControlDepartmentId = qualityControlDepartmentId;
      if (qualityControlEmployeeId != null)
        stuff.QualityControlEmployeeId = qualityControlEmployeeId;
      if (description != null)
        stuff.Description = description;
      if (stuffCategoryId != null)
        stuff.StuffCategoryId = stuffCategoryId;
      if (unitTypeId != null)
      {
        if (stuff.UnitTypeId != unitTypeId)
        {
          var hasBaseTransaction = App.Internals.WarehouseManagement.GetBaseTransactions(e => e,
                    stuffId: stuff.Id)
                .Any();
          if (hasBaseTransaction)
            throw new CanNotEditUnitTypeStuffException(stuff.Code);
        }
        stuff.UnitTypeId = unitTypeId;
      }
      if (stockSafety != null)
        stuff.StockSafety = stockSafety;
      if (faultyPercentage != null)
        stuff.FaultyPercentage = faultyPercentage;
      if (stuffType != null)
        stuff.StuffType = stuffType;
      if (needToQualityControl != null)
        stuff.NeedToQualityControl = needToQualityControl;
      if (tolerance != null)
        stuff.Tolerance = tolerance;
      if (volume != null)
        stuff.Volume = volume;
      if (netWeight != null)
        stuff.NetWeight = netWeight;
      if (grossWeight != null)
        stuff.GrossWeight = grossWeight;
      if (needToQualityControlDocumentUpload != null)
        stuff.NeedToQualityControlDocumentUpload = needToQualityControlDocumentUpload;
      if (stuffHSGroupId != null)
      {
        if (stuffHSGroupId == 0)
        {
          stuff.StuffHSGroupId = null;
        }
        else
        {
          stuff.StuffHSGroupId = stuffHSGroupId;
        }
      }
      if (stuff.NeedToQualityControl == true && stuff.QualityControlDepartmentId == null)
        throw new QualityControlDepartmentNotDefinedException();
      if (stuffPurchaseCategoryId != null)
        stuff.StuffPurchaseCategoryId = stuffPurchaseCategoryId == 0 ? null : stuffPurchaseCategoryId;
      stuff.QualityControlCheckDuration = qualityControlCheckDuration;
      if (ceofficientSet != null)
        stuff.CeofficientSet = ceofficientSet;
      repository.Update(entity: stuff, rowVersion: stuff.RowVersion);
      return stuff;
    }
    #endregion
    #region Delete
    public void DeleteStuff(int id)
    {
      var stuff = GetStuff(id: id);
      repository.Delete(stuff);
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStuffs<TResult>(
      Expression<Func<Stuff, TResult>> selector, TValue<int> id = null,
      TValue<string> name = null,
      TValue<string> code = null,
      TValue<bool> isActive = null,
      TValue<bool> isTraceable = null,
      TValue<bool> needToQualityControl = null,
      TValue<bool> needToQualityControlDocumentUpload = null,
      TValue<string> description = null,
      TValue<int> stuffCategoryId = null,
      TValue<int> qualityControlEmployeeId = null,
      TValue<int> qualityControlDepartmentId = null,
      TValue<int> unitTypeId = null,
      TValue<int> stockSafety = null,
      TValue<float> faultyPercentage = null,
      TValue<StuffType> stuffType = null,
      TValue<StuffType[]> stuffTypes = null,
      TValue<bool?> hasProjectHeader = null,
      TValue<int[]> ids = null,
      TValue<int> stuffPurchaseCategoryId = null,
      TValue<StuffDefinitionStatus> definitionStatus = null,
      TValue<int> stuffPurchaseCategoryQualityControlDepartmentId = null,
      TValue<short> ceofficientSet = null)
    {
      var query = repository.GetQuery<Stuff>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (code != null)
        query = query.Where(i => i.Code == code);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);
      if (needToQualityControl != null)
        query = query.Where(i => i.NeedToQualityControl == needToQualityControl);
      if (needToQualityControlDocumentUpload != null)
        query = query.Where(i => i.NeedToQualityControlDocumentUpload == needToQualityControlDocumentUpload);
      if (isTraceable != null)
        query = query.Where(i => i.IsTraceable == isTraceable);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (stuffCategoryId != null)
        query = query.Where(i => i.StuffCategoryId == stuffCategoryId);
      if (unitTypeId != null)
        query = query.Where(i => i.UnitTypeId == unitTypeId);
      if (stockSafety != null)
        query = query.Where(i => i.StockSafety == stockSafety);
      if (faultyPercentage != null)
        query = query.Where(i => i.FaultyPercentage == faultyPercentage);
      if (stuffType != null)
        query = query.Where(i => i.StuffType == stuffType);
      if (qualityControlDepartmentId != null)
        query = query.Where(i => i.QualityControlDepartmentId == qualityControlDepartmentId);
      if (qualityControlEmployeeId != null)
        query = query.Where(i => i.QualityControlEmployeeId == qualityControlEmployeeId);
      if (stuffTypes != null)
        query = query.Where(i => stuffTypes.Value.Contains(i.StuffType));
      if (hasProjectHeader != null)
        if (hasProjectHeader == true)
          query = query.Where(i => (int?)i.ProjectHeader.Id != null);
        else
          query = query.Where(i => (int?)i.ProjectHeader.Id == null);
      if (ids != null)
        query = query.Where(i => ids.Value.Contains(i.Id));
      if (stuffPurchaseCategoryId != null)
        query = query.Where(i => i.StuffPurchaseCategoryId == stuffPurchaseCategoryId);
      //if (definitionStatus != null)
      //    query = query.Where(i => i.DefinitionStatus == definitionStatus);
      if (stuffPurchaseCategoryQualityControlDepartmentId != null)
        query = query.Where(i => i.StuffPurchaseCategory.QualityControlDepartmentId == stuffPurchaseCategoryQualityControlDepartmentId);
      if (ceofficientSet != null)
        query = query.Where(i => i.CeofficientSet == ceofficientSet);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public Stuff GetStuff(int id) => GetStuff(selector: e => e, id: id);
    public TResult GetStuff<TResult>(Expression<Func<Stuff, TResult>> selector, int id)
    {
      var stuff = GetStuffs(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (stuff == null)
        throw new StuffNotFoundException(id);
      return stuff;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffResult> SortStuffResult(IQueryable<StuffResult> input,
        SortInput<StuffSortType> options)
    {
      switch (options.SortType)
      {
        case StuffSortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        case StuffSortType.Noun:
          return input.OrderBy(i => i.Noun, options.SortOrder);
        case StuffSortType.Title:
          return input.OrderBy(i => i.Title, options.SortOrder);
        case StuffSortType.Code:
          return input.OrderBy(i => i.Code, options.SortOrder);
        case StuffSortType.IsActive:
          return input.OrderBy(i => i.IsActive, options.SortOrder);
        case StuffSortType.Description:
          return input.OrderBy(i => i.Description, options.SortOrder);
        case StuffSortType.StuffCategory:
          return input.OrderBy(i => i.CategoryName, options.SortOrder);
        case StuffSortType.UnitType:
          return input.OrderBy(i => i.UnitTypeName, options.SortOrder);
        case StuffSortType.StuffType:
          return input.OrderBy(i => i.StuffType, options.SortOrder);
        case StuffSortType.FaultyPercentage:
          return input.OrderBy(i => i.FaultyPercentage, options.SortOrder);
        case StuffSortType.StockSafety:
          return input.OrderBy(i => i.StockSafety, options.SortOrder);
        case StuffSortType.NeedToQualityControl:
          return input.OrderBy(i => i.NeedToQualityControl, options.SortOrder);
        case StuffSortType.IsTraceable:
          return input.OrderBy(i => i.IsTraceable, options.SortOrder);
        case StuffSortType.QualityControlDepartmentName:
          return input.OrderBy(i => i.QualityControlDepartmentName, options.SortOrder);
        case StuffSortType.QualityControlEmployeeFullName:
          return input.OrderBy(i => i.QualityControlEmployeeFullName, options.SortOrder);
        case StuffSortType.Tolerance:
          return input.OrderBy(i => i.Tolerance, options.SortOrder);
        case StuffSortType.Volume:
          return input.OrderBy(i => i.Volume, options.SortOrder);
        case StuffSortType.NetWeight:
          return input.OrderBy(i => i.NetWeight, options.SortOrder);
        case StuffSortType.GrossWeight:
          return input.OrderBy(i => i.GrossWeight, options.SortOrder);
        case StuffSortType.StuffHSGroupCode:
          return input.OrderBy(i => i.StuffHSGroupCode, options.SortOrder);
        case StuffSortType.StuffHSGroupTitle:
          return input.OrderBy(i => i.StuffHSGroupTitle, options.SortOrder);
        case StuffSortType.ParentStuffCategoryName:
          return input.OrderBy(i => i.ParentStuffCategoryName, options.SortOrder);
        case StuffSortType.NeedToQualityControlDocumentUpload:
          return input.OrderBy(i => i.NeedToQualityControlDocumentUpload, options.SortOrder);
        case StuffSortType.StuffDefinitionRequestId:
          return input.OrderBy(i => i.StuffDefinitionRequestId, options.SortOrder);
        case StuffSortType.QualityControlCheckDuration:
          return input.OrderBy(i => i.QualityControlCheckDuration, options.SortOrder);
        case StuffSortType.StuffPurchaseCategoryName:
          return input.OrderBy(i => i.StuffPurchaseCategoryName, options.SortOrder);
        case StuffSortType.CheckDocument:
          return input.OrderBy(i => i.CheckDocument, options.SortOrder);
        case StuffSortType.StuffPurchaseCategoryQualityControlDepartmentName:
          return input.OrderBy(i => i.StuffPurchaseCategoryQualityControlDepartmentName, options.SortOrder);
        case StuffSortType.CeofficientSet:
          return input.OrderBy(i => i.CeofficientSet, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<StuffComboResult> SortStuffComboResult(IQueryable<StuffComboResult> input,
        SortInput<StuffSortType> options)
    {
      switch (options.SortType)
      {
        case StuffSortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        case StuffSortType.Code:
          return input.OrderBy(i => i.Code, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<StuffComboWithStuffTypeResult> SortStuffComboWithStuffTypeResult(IQueryable<StuffComboWithStuffTypeResult> input,
        SortInput<StuffSortType> options)
    {
      switch (options.SortType)
      {
        case StuffSortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        case StuffSortType.Code:
          return input.OrderBy(i => i.Code, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<Stuff, StuffResult>> ToStuffResult =>
    stuff => new StuffResult
    {
      Id = stuff.Id,
      Code = stuff.Code,
      Name = stuff.Name,
      Noun = stuff.Noun,
      Title = stuff.Title,
      UnitTypeName = stuff.UnitType.Name,
      IsActive = stuff.IsActive,
      Description = stuff.Description,
      CategoryName = stuff.StuffCategory.Name,
      StuffCategoryId = stuff.StuffCategoryId,
      ParentStuffCategoryName = stuff.StuffCategory.ParentStuffCategory.Name,
      StuffType = stuff.StuffType,
      UnitTypeId = stuff.UnitTypeId,
      FaultyPercentage = stuff.FaultyPercentage,
      StockSafety = stuff.StockSafety,
      NeedToQualityControl = stuff.NeedToQualityControl,
      NeedToQualityControlDocumentUpload = stuff.NeedToQualityControlDocumentUpload,
      IsTraceable = stuff.IsTraceable,
      QualityControlDepartmentId = stuff.QualityControlDepartmentId,
      QualityControlDepartmentName = stuff.QualityControlDepartment.Name,
      QualityControlEmployeeId = stuff.QualityControlEmployeeId,
      QualityControlEmployeeFullName = stuff.QualityControlEmployee.FirstName + " " + stuff.QualityControlEmployee.LastName,
      Tolerance = stuff.Tolerance,
      NetWeight = stuff.NetWeight,
      GrossWeight = stuff.GrossWeight,
      Volume = stuff.Volume,
      StuffHSGroupId = stuff.StuffHSGroupId,
      StuffHSGroupCode = stuff.StuffHSGroup.Code,
      StuffHSGroupTitle = stuff.StuffHSGroup.Title,
      StuffPurchaseCategoryId = stuff.StuffPurchaseCategoryId,
      StuffPurchaseCategoryName = stuff.StuffPurchaseCategory.Title,
      StuffDefinitionRequestId = stuff.StuffDefinitionRequest.Id,
      QualityControlCheckDuration = stuff.QualityControlCheckDuration,
      RowVersion = stuff.RowVersion,
      CheckDocument = stuff.StuffDocuments.Any(),
      StuffPurchaseCategoryQualityControlDepartmentId = stuff.StuffPurchaseCategory.QualityControlDepartmentId,
      StuffPurchaseCategoryQualityControlDepartmentName = stuff.StuffPurchaseCategory.QualityControlDepartment.Name,
      CeofficientSet = stuff.CeofficientSet
    };
    #endregion
    #region ToStuffComboResult
    public Expression<Func<Stuff, StuffComboResult>> ToStuffComboResult =
        stuff => new StuffComboResult()
        {
          Id = stuff.Id,
          Code = stuff.Code,
          Name = stuff.Name,
          Noun = stuff.Noun,
          RowVersion = stuff.RowVersion
        };
    #endregion
    #region ToStuffComboWithStuffTypeResult
    public Expression<Func<Stuff, StuffComboWithStuffTypeResult>> ToStuffComboWithStuffTypeResult =
        stuff => new StuffComboWithStuffTypeResult()
        {
          Id = stuff.Id,
          Code = stuff.Code,
          Name = stuff.Name,
          Noun = stuff.Noun,
          StuffType = stuff.StuffType,
          RowVersion = stuff.RowVersion
        };
    #endregion
    #region ToStuffWithUnitsResult
    public Expression<Func<Stuff, StuffWithUnitsResult>> ToStuffWithUnitsResult =
        stuff => new StuffWithUnitsResult()
        {
          Id = stuff.Id,
          Code = stuff.Code,
          Name = stuff.Name,
          CategoryName = stuff.StuffCategory.Name,
          Description = stuff.Description,
          IsActive = stuff.IsActive,
          StuffCategoryId = stuff.StuffCategoryId,
          StuffType = stuff.StuffType,
          UnitTypeId = stuff.UnitTypeId,
          UnitTypeName = stuff.UnitType.Name,
          FaultyPercentage = stuff.FaultyPercentage,
          StockSafety = stuff.StockSafety,
          NeedToQualityControl = stuff.NeedToQualityControl,
          IsTraceable = stuff.IsTraceable,
          QualityControlDepartmentId = stuff.QualityControlDepartmentId,
          QualityControlDepartmentName = stuff.QualityControlDepartment.Name,
          QualityControlEmployeeId = stuff.QualityControlEmployeeId,
          QualityControlEmployeeFullName = stuff.QualityControlEmployee.FirstName + " " + stuff.QualityControlEmployee.LastName,
          Volume = stuff.Volume,
          NetWeight = stuff.NetWeight,
          GrossWeight = stuff.GrossWeight,
          RowVersion = stuff.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<StuffResult> SearchStuffResultQuery(
        IQueryable<StuffResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from stuff in query
                where stuff.Code.Contains(searchText) ||
                      stuff.Name.Contains(searchText) ||
                      stuff.Title.Contains(searchText) ||
                      stuff.CategoryName.Contains(searchText) ||
                      stuff.UnitTypeName.Contains(searchText) ||
                      stuff.Noun.Contains(searchText) ||
                      stuff.QualityControlDepartmentName.Contains(searchText) ||
                      stuff.QualityControlEmployeeFullName.Contains(searchText) ||
                      stuff.Description.Contains(searchText)
                select stuff;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    public IQueryable<StuffComboResult> SearchStuffComboResultQuery(
        IQueryable<StuffComboResult> query,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from stuff in query
                where stuff.Code.Contains(searchText) ||
                      stuff.Name.Contains(searchText)
                select stuff;
      return query;
    }
    public IQueryable<StuffComboWithStuffTypeResult> SearchStuffComboWithStuffTypeResultQuery(
        IQueryable<StuffComboWithStuffTypeResult> query,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from stuff in query
                where stuff.Code.Contains(searchText) ||
                      stuff.Name.Contains(searchText)
                select stuff;
      return query;
    }
    #endregion
  }
}
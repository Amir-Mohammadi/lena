using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.EquivalentStuffUsage;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Gets
    public IQueryable<TResult> GetEquivalentStuffUsages<TResult>(
        Expression<Func<EquivalentStuffUsage, TResult>> selector,
        TValue<int> id = null,
        TValue<int> equivalentStuffId = null,
        TValue<double> usageQty = null,
        TValue<EquivalentStuffUsageStatus> status = null,
        TValue<int> productionPlanDetailId = null,
        TValue<int> productionOrderId = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<string> description = null,
        TValue<EquivalentStuffUsageStatus[]> statuses = null,
        TValue<int> billOfMaterialDetailId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    description: description);
      var query = baseQuery.OfType<EquivalentStuffUsage>();
      if (equivalentStuffId != null)
        query = query.Where(i => i.EquivalentStuffId == equivalentStuffId);
      if (usageQty != null)
        query = query.Where(i => i.UsageQty == usageQty);
      if (status != null)
        query = query.Where(i => i.Status == status);
      if (statuses != null)
        query = query.Where(i => statuses.Value.Contains(i.Status));

      if (productionOrderId != null && productionPlanDetailId != null)
        query = query.Where(i => (i.ProductionOrderId != null && i.ProductionOrderId == productionOrderId) || (i.ProductionPlanDetailId == productionPlanDetailId));
      else if (productionOrderId == null && productionPlanDetailId != null)
        query = query.Where(i => i.ProductionPlanDetailId == productionPlanDetailId);
      else if (productionOrderId != null)
        query = query.Where(i => i.ProductionOrderId == productionOrderId);
      if (billOfMaterialDetailId != null)
        query = query.Where(i => i.EquivalentStuff.BillOfMaterialDetailId == billOfMaterialDetailId);

      return query.Select(selector);
    }
    #endregion

    #region Get

    public EquivalentStuffUsage GetEquivalentStuffUsage(int id) => GetEquivalentStuffUsage(selector: e => e, id: id);
    public TResult GetEquivalentStuffUsage<TResult>(
        Expression<Func<EquivalentStuffUsage, TResult>> selector,
        int id)
    {

      var equivalentStuffUsage = GetEquivalentStuffUsages(
                    selector: selector,
                    id: id)

                .FirstOrDefault();
      if (equivalentStuffUsage == null)
        throw new EquivalentStuffUsageNotFoundException(id: id);
      return equivalentStuffUsage;
    }

    #endregion

    #region Add
    public EquivalentStuffUsage AddEquivalentStuffUsage(
        EquivalentStuffUsage equivalentStuffUsage,
        TransactionBatch transactionBatch,
        int equivalentStuffId,
        double usageQty,
        EquivalentStuffUsageStatus status,
        int? productionPlanDetailId,
        int? productionOrderId,
        string description)
    {

      equivalentStuffUsage = equivalentStuffUsage ?? repository.Create<EquivalentStuffUsage>();
      equivalentStuffUsage.EquivalentStuffId = equivalentStuffId;
      equivalentStuffUsage.UsageQty = usageQty;
      equivalentStuffUsage.Status = status;
      equivalentStuffUsage.ProductionPlanDetailId = productionPlanDetailId;
      equivalentStuffUsage.ProductionOrderId = productionOrderId;
      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: equivalentStuffUsage,
                  transactionBatch: transactionBatch,
                  description: description);
      return equivalentStuffUsage;
    }

    public EquivalentStuffUsage AddEquivalentStuffUsageProcess(
        int equivalentStuffId,
        double usageQty,
        EquivalentStuffUsageStatus status,
        int? productionPlanDetailId,
        int? productionOrderId,
        string description)
    {


      var equivalentStuff = AddEquivalentStuffUsage(
                   equivalentStuffUsage: null,
                   transactionBatch: null,
                   equivalentStuffId: equivalentStuffId,
                   usageQty: usageQty,
                   status: status,
                   productionPlanDetailId: productionPlanDetailId,
                   productionOrderId: productionOrderId,
                   description: description);
      return equivalentStuff;
    }
    #endregion

    #region Edit
    public EquivalentStuffUsage EditEquivalentStuffUsage(
        byte[] rowVersion,
        int id,
        TValue<int> equivalentStuffId = null,
        TValue<double> usageQty = null,
        TValue<EquivalentStuffUsageStatus> status = null,
        TValue<int> productionPlanDetailId = null,
        TValue<int> productionOrderId = null,
        TValue<string> description = null)
    {

      var equivalentStuffUsage = GetEquivalentStuffUsage(id: id);
      if (equivalentStuffId != null)
        equivalentStuffUsage.EquivalentStuffId = equivalentStuffId;
      if (usageQty != null)
        equivalentStuffUsage.UsageQty = usageQty;
      if (status != null)
        equivalentStuffUsage.Status = status;
      if (productionPlanDetailId != null)
        equivalentStuffUsage.ProductionPlanDetailId = productionPlanDetailId;
      if (productionOrderId != null)
        equivalentStuffUsage.ProductionOrderId = productionOrderId;
      App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: equivalentStuffUsage,
                    rowVersion: rowVersion,
                    description: description);
      return equivalentStuffUsage;
    }
    #endregion

    #region Delete
    public void DeleteEquivalentStuffUsageProcess(int id, byte[] rowVersion)
    {

      var equivalentStuffUsage = GetEquivalentStuffUsage(id: id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                  transactionBatchId: null,
                  baseEntity: equivalentStuffUsage,
                  rowVersion: rowVersion);
    }
    #endregion

    #region ToResult

    public Expression<Func<EquivalentStuffUsage, EquivalentStuffUsageResult>> ToEquivalentStuffUsageResult =>
        equivalentStuffUsage => new EquivalentStuffUsageResult()
        {
          Id = equivalentStuffUsage.Id,
          Code = equivalentStuffUsage.Code,
          StuffId = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetail.BillOfMaterialStuffId,
          StuffCode = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetail.BillOfMaterial.Stuff.Code,
          StuffName = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetail.BillOfMaterial.Stuff.Name,
          BillOfMaterialVersion = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetail.BillOfMaterialVersion,
          EquivalentStuffType = equivalentStuffUsage.EquivalentStuff.EquivalentStuffType,
          EquivalentStuffId = equivalentStuffUsage.EquivalentStuffId,
          EquivalentStuffTitle = equivalentStuffUsage.EquivalentStuff.Title,
          EquivalentStuffStuffId = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetail.StuffId,
          EquivalentStuffCode = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetail.Stuff.Code,
          EquivalentStuffName = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetail.Stuff.Name,
          BillOfMaterialDetailId = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetail.Id,
          BillOfMaterialDetailValue = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetail.Value,
          BillOfMaterialDetailUnitId = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetail.UnitId,
          BillOfMaterialDetailUnitName = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetail.Unit.Name,
          UsageQty = equivalentStuffUsage.UsageQty,
          Status = equivalentStuffUsage.Status,
          ProductionPlanDetailId = equivalentStuffUsage.ProductionPlanDetailId,
          ProductionPlanDetailCode = equivalentStuffUsage.ProductionPlanDetail.Code,
          ProductionPlanId = equivalentStuffUsage.ProductionPlanDetail.ProductionPlanId,
          ProductionPlanEstimatedDate = equivalentStuffUsage.ProductionPlanDetail.ProductionPlan.EstimatedDate,
          ProductionOrderId = equivalentStuffUsage.ProductionOrderId,
          ProductionOrderCode = equivalentStuffUsage.ProductionOrder.Code,
          ProductionOrderDateTime = equivalentStuffUsage.ProductionOrder.CalendarEvent.DateTime,
          RowVersion = equivalentStuffUsage.RowVersion
        };
    #endregion

    #region Search
    public IQueryable<EquivalentStuffUsageResult> SearchEquivalentStuffUsage(
        IQueryable<EquivalentStuffUsageResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        int? stuffId = null,
        int? productionPlanId = null,
        string productionOrderCode = null,
        int? billOfMaterialVersion = null,
        DateTime? fromDateTime = null,
        DateTime? toDateTime = null,
        EquivalentStuffType? equivalentStuffType = null)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where
                item.Code.Contains(searchText) ||
                item.StuffCode.Contains(searchText) ||
                item.StuffName.Contains(searchText) ||
                item.EquivalentStuffCode.Contains(searchText) ||
                item.EquivalentStuffName.Contains(searchText)
                select item;
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (productionPlanId != null)
        query = query.Where(i => i.ProductionPlanId == productionPlanId);
      if (productionOrderCode != null)
        query = query.Where(i => i.ProductionOrderCode == productionOrderCode);
      if (billOfMaterialVersion != null)
        query = query.Where(i => i.BillOfMaterialVersion == billOfMaterialVersion);
      if (toDateTime != null)
        query = query.Where(i => i.ProductionPlanEstimatedDate <= toDateTime || i.ProductionOrderDateTime <= toDateTime);
      if (fromDateTime != null)
        query = query.Where(i => i.ProductionPlanEstimatedDate >= fromDateTime || i.ProductionOrderDateTime >= fromDateTime);
      if (equivalentStuffType != null)
        query = query.Where(i => i.EquivalentStuffType == equivalentStuffType);



      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    public IOrderedQueryable<EquivalentStuffUsageResult> SortEquivalentStuffUsageResult(IQueryable<EquivalentStuffUsageResult> query
        , SortInput<EquivalentStuffUsageSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case EquivalentStuffUsageSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sortInput.SortOrder);
        case EquivalentStuffUsageSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sortInput.SortOrder);
        case EquivalentStuffUsageSortType.BillOfMaterialVersion:
          return query.OrderBy(i => i.BillOfMaterialVersion, sortInput.SortOrder);
        case EquivalentStuffUsageSortType.EquivalentStuffTitle:
          return query.OrderBy(i => i.EquivalentStuffTitle, sortInput.SortOrder);
        case EquivalentStuffUsageSortType.EquivalentStuffType:
          return query.OrderBy(i => i.EquivalentStuffType, sortInput.SortOrder);
        case EquivalentStuffUsageSortType.EquivalentStuffCode:
          return query.OrderBy(i => i.EquivalentStuffCode, sortInput.SortOrder);
        case EquivalentStuffUsageSortType.EquivalentStuffName:
          return query.OrderBy(i => i.EquivalentStuffName, sortInput.SortOrder);
        case EquivalentStuffUsageSortType.UsageQty:
          return query.OrderBy(i => i.UsageQty, sortInput.SortOrder);
        case EquivalentStuffUsageSortType.Status:
          return query.OrderBy(i => i.Status, sortInput.SortOrder);
        case EquivalentStuffUsageSortType.ProductionPlanDetailCode:
          return query.OrderBy(i => i.ProductionPlanDetailCode, sortInput.SortOrder);
        case EquivalentStuffUsageSortType.ProductionOrderCode:
          return query.OrderBy(i => i.ProductionOrderCode, sortInput.SortOrder);
        case EquivalentStuffUsageSortType.ProductionPlanEstimatedDate:
          return query.OrderBy(i => i.ProductionPlanEstimatedDate, sortInput.SortOrder);
        case EquivalentStuffUsageSortType.ProductionOrderDateTime:
          return query.OrderBy(i => i.ProductionOrderDateTime, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}

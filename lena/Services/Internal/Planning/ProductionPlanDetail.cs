using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Planning.ProductionPlanDetail;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region AddProductionPlanDetail
    public ProductionPlanDetail AddProductionPlanDetail(
        ProductionPlanDetail productionPlanDetail,
        TransactionBatch transactionBatch,
        string description,
        double qty,
        byte unitId,
        int billOfMaterialStuffId,
        short billOfMaterialVersion,
        int productionPlanId,
        int levelId)
    {

      productionPlanDetail = productionPlanDetail ?? repository.Create<ProductionPlanDetail>();
      productionPlanDetail.Qty = qty;
      productionPlanDetail.UnitId = unitId;
      productionPlanDetail.BillOfMaterialStuffId = billOfMaterialStuffId;
      productionPlanDetail.BillOfMaterialVersion = billOfMaterialVersion;
      productionPlanDetail.ProductionPlanId = productionPlanId;
      productionPlanDetail.ProductionPlanDetailLevelId = levelId;
      productionPlanDetail.Status = ProductionPlanDetailStatus.NotAction;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: productionPlanDetail,
                    transactionBatch: transactionBatch,
                    description: description);
      return productionPlanDetail;
    }
    #endregion
    #region EditProductionPlanDetail
    public ProductionPlanDetail EditProductionPlanDetail(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<bool> isDelete = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<short> billOfMaterialVersion = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<int> productionPlanId = null,
        TValue<ProductionPlanDetailStatus> status = null)
    {

      var productionPlanDetail = GetProductionPlanDetail(id: id);
      return EditProductionPlanDetail(
                    productionPlanDetail: productionPlanDetail,
                    rowVersion: rowVersion,
                    description: description,
                    isDelete: isDelete,
                    billOfMaterialStuffId: billOfMaterialStuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    qty: qty,
                    unitId: unitId,
                    productionPlanId: productionPlanId,
                    status: status);
    }
    public ProductionPlanDetail EditProductionPlanDetail(
        ProductionPlanDetail productionPlanDetail,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<bool> isDelete = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<short> billOfMaterialVersion = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<int> productionPlanId = null,
        TValue<ProductionPlanDetailStatus> status = null)
    {

      if (productionPlanId != null)
        productionPlanDetail.ProductionPlanId = productionPlanId;
      if (qty != null)
        productionPlanDetail.Qty = qty;
      if (unitId != null)
        productionPlanDetail.UnitId = unitId;
      if (billOfMaterialStuffId != null)
        productionPlanDetail.BillOfMaterialStuffId = billOfMaterialStuffId;
      if (billOfMaterialVersion != null)
        productionPlanDetail.BillOfMaterialVersion = billOfMaterialVersion;
      if (status != null)
        productionPlanDetail.Status = status;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: productionPlanDetail,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description);
      return retValue as ProductionPlanDetail;
    }
    #endregion
    #region Get
    public ProductionPlanDetail GetProductionPlanDetail(int id) => GetProductionPlanDetail(selector: e => e, id: id);
    public TResult GetProductionPlanDetail<TResult>(
        Expression<Func<ProductionPlanDetail, TResult>> selector,
        int id)
    {

      var productionPlanDetail = GetProductionPlanDetails(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (productionPlanDetail == null)
        throw new ProductionPlanDetailNotFoundException(id);
      return productionPlanDetail;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionPlanDetails<TResult>(
        Expression<Func<ProductionPlanDetail, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<int> billOfMaterialVersion = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<int> productionPlanId = null,
        TValue<DateTime> deadlineFromDate = null,
        TValue<DateTime> deadlineToDate = null,
        TValue<DateTime> fromPlanEstimatedDate = null,
        TValue<DateTime> toPlanEstimatedDate = null,
        TValue<int> orderId = null,
        TValue<ProductionPlanDetailStatus> status = null,
        TValue<ProductionPlanDetailStatus[]> statuses = null,
        TValue<ProductionPlanDetailStatus[]> notHasStatuses = null
        //TValue<bool> isTemporary
        )
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var productionPlanDetails = baseQuery.OfType<ProductionPlanDetail>();
      //if (isTemporary != null)
      //    productionPlanDetails = productionPlanDetails.Where(i => i.ProductionPlan.IsTemporary == isTemporary);
      if (qty != null)
        productionPlanDetails = productionPlanDetails.Where(i => Math.Abs(i.Qty - qty) < 0.000001);
      if (unitId != null)
        productionPlanDetails = productionPlanDetails.Where(i => i.UnitId == unitId);
      if (billOfMaterialStuffId != null)
        productionPlanDetails = productionPlanDetails.Where(i => i.BillOfMaterialStuffId == billOfMaterialStuffId);
      if (billOfMaterialVersion != null)
        productionPlanDetails = productionPlanDetails.Where(i => i.BillOfMaterialVersion == billOfMaterialVersion);
      if (productionPlanId != null)
        productionPlanDetails = productionPlanDetails.Where(i => i.ProductionPlanId == productionPlanId);
      if (deadlineFromDate != null)
        productionPlanDetails = productionPlanDetails.Where(i => i.ProductionPlan.ProductionRequest.DeadlineDate >= deadlineFromDate);
      if (deadlineToDate != null)
        productionPlanDetails = productionPlanDetails.Where(i => i.ProductionPlan.ProductionRequest.DeadlineDate <= deadlineToDate);
      if (orderId != null)
        productionPlanDetails = productionPlanDetails.Where(i => i.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.OrderId == orderId);
      if (fromPlanEstimatedDate != null)
        productionPlanDetails = productionPlanDetails.Where(i => i.ProductionPlan.EstimatedDate >= fromPlanEstimatedDate);
      if (toPlanEstimatedDate != null)
        productionPlanDetails = productionPlanDetails.Where(i => i.ProductionPlan.EstimatedDate <= toPlanEstimatedDate);
      if (status != null)
        productionPlanDetails = productionPlanDetails.Where(i => i.Status.HasFlag(status));
      if (statuses != null)
      {
        var s = ProductionPlanDetailStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        productionPlanDetails = productionPlanDetails.Where(i => (i.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = ProductionPlanDetailStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        productionPlanDetails = productionPlanDetails.Where(i => (i.Status & s) == 0);
      }

      return productionPlanDetails.Select(selector);
    }
    #endregion
    #region AddProductionPlanDetailProcess
    public ProductionPlanDetail AddProductionPlanDetailProcess(
        int productionPlanId,
        int billOfMaterialStuffId,
        short billOfMaterialVersion,
        int levelId,
        byte unitId,
        double qty
        )
    {

      var productionPlan = GetProductionPlan(productionPlanId);
      var billOfMaterial = GetBillOfMaterial(stuffId: billOfMaterialStuffId,
                    version: billOfMaterialVersion);
      return AddProductionPlanDetailProcess(
                billOfMaterial: billOfMaterial,
                productionPlan: productionPlan,
                unitId: unitId,
                levelId: levelId,
                qty: qty
                );
    }
    public ProductionPlanDetail AddProductionPlanDetailProcess(
        BillOfMaterial billOfMaterial,
        ProductionPlan productionPlan,
        byte unitId,
        int levelId,
        double qty
        )
    {

      #region Insert TransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      var transactionBatch = warehouseManagement.AddTransactionBatch();
      #endregion
      #region Calculate Qty
      var unit = App.Internals.ApplicationBase.GetUnit(id: unitId);
      var factor = (qty * unit.ConversionRatio) / (billOfMaterial.Unit.ConversionRatio * billOfMaterial.Value);
      #endregion
      #region Insert TransactionPlans
      foreach (var bomDetail in billOfMaterial.BillOfMaterialDetails)
      {
        #region TransactionType
        TransactionType transactionType;
        switch (bomDetail.BillOfMaterialDetailType)
        {
          case BillOfMaterialDetailType.Material:
            transactionType = Models.StaticData.StaticTransactionTypes.ImportConsumPlan;
            break;
          case BillOfMaterialDetailType.StepProduct:
            transactionType = Models.StaticData.StaticTransactionTypes.ImportProductionPlan;
            break;
          case BillOfMaterialDetailType.Waste:
            transactionType = Models.StaticData.StaticTransactionTypes.ImportWastePlan;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        #endregion
        #region Insert TransactionPlan
        var transactionPlan = warehouseManagement.AddTransactionPlanProcess(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: productionPlan.EstimatedDate,
                stuffId: bomDetail.StuffId,
                billOfMaterialVersion: bomDetail.SemiProductBillOfMaterialVersion,
                stuffSerialCode: null,
                transactionTypeId: transactionType.Id,
                amount: bomDetail.Value * factor / bomDetail.ForQty,
                unitId: bomDetail.UnitId,
                description: "",
                isEstimated: true,
                referenceTransaction: null);
        #endregion
      }
      #endregion
      #region Insert ProductionPlanDetail
      var productionPlanDetail = AddProductionPlanDetail(
              productionPlanDetail: null,
              transactionBatch: transactionBatch,
              description: "",
              qty: qty,
              unitId: unitId,
              billOfMaterialStuffId: billOfMaterial.StuffId,
              billOfMaterialVersion: billOfMaterial.Version,
              productionPlanId: productionPlan.Id,
              levelId: levelId);
      #endregion
      #region AddProductionPlanDetailSummary

      AddProductionPlanDetailSummary(
              producedQty: 0,
              scheduledQty: 0,
              productionPlanDetailId: productionPlanDetail.Id);
      #endregion
      return productionPlanDetail;
    }
    #endregion
    #region AddProductionPlanDetailProcessRecursive
    public void AddProductionPlanDetailProcessRecursive(
        int billOfMaterialStuffId,
        short billOfMaterialVersion,
        int productionPlanId,
        byte unitId,
        double qty)
    {

      var productionPlan = GetProductionPlan(productionPlanId);
      var billOfMaterial = GetBillOfMaterial(
                    stuffId: billOfMaterialStuffId,
                    version: billOfMaterialVersion);
      var unit = App.Internals.ApplicationBase.GetUnit(id: unitId);
      AddProductionPlanDetailProcessRecursive(
                billOfMaterial: billOfMaterial,
                productionPlan: productionPlan,
                unit: unit,
                qty: qty,
                parentLevelId: null);
    }
    public void AddProductionPlanDetailProcessRecursive(
        BillOfMaterial billOfMaterial,
        ProductionPlan productionPlan,
        Unit unit,
        double qty,
        int? parentLevelId)
    {

      var baseUnit = billOfMaterial.Unit;
      var detailQty = qty * unit.ConversionRatio / baseUnit.ConversionRatio;
      var level = AddProductionPlanDetailLevel(parentLevelId);
      #region AddProductionPlanDetailProcess
      AddProductionPlanDetailProcess(
              billOfMaterial: billOfMaterial,
              productionPlan: productionPlan,
              unitId: baseUnit.Id,
              qty: detailQty,
              levelId: level.Id);
      #endregion
      #region AddProductionPlanDetailProcess for SemiProductDetails
      foreach (var bomDetail in billOfMaterial.BillOfMaterialDetails)
      {

        if (bomDetail.Stuff.StuffType != StuffType.General && bomDetail.Stuff.StuffType != StuffType.RawMaterial)
        {

          BillOfMaterial semiProductBom = null;
          if (bomDetail.SemiProductBillOfMaterialVersion != null)
          {
            semiProductBom = GetBillOfMaterial(
                          stuffId: bomDetail.StuffId,
                          version: bomDetail.SemiProductBillOfMaterialVersion.Value);
          }
          else
          {
            semiProductBom = GetPublishedBillOfMaterial(stuffId: bomDetail.StuffId);
          }

          var detailUnit = bomDetail.Unit;
          var value = Math.Floor((bomDetail.Value * detailQty) / (billOfMaterial.Value * bomDetail.ForQty));
          AddProductionPlanDetailProcessRecursive(
                        billOfMaterial: semiProductBom,
                        productionPlan: productionPlan,
                        unit: detailUnit,
                        qty: value,
                        parentLevelId: level.Id);
        }

      }
      #endregion
    }
    #endregion
    #region ToResult
    public Expression<Func<ProductionPlanDetail, ProductionPlanDetailResult>> ToProductionPlanDetailResult =
        productionPlanDetail => new ProductionPlanDetailResult
        {
          Id = productionPlanDetail.Id,
          ProductionPlanId = productionPlanDetail.ProductionPlanId,
          ProductionPlanCode = productionPlanDetail.ProductionPlan.Code,
          OrderItemDeliveryDate = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.DeliveryDate,
          CustomerCode = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Order.Customer.Code,
          CustomerName = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Order.Customer.Name,
          StuffId = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.StuffId,
          StuffCode = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Stuff.Code,
          StuffName = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Stuff.Name,
          LevelId = productionPlanDetail.ProductionPlanDetailLevelId,
          BillOfMaterialVersion = productionPlanDetail.BillOfMaterialVersion,
          SemiProductStuffId = productionPlanDetail.BillOfMaterialStuffId,
          SemiProductStuffCode = productionPlanDetail.BillOfMaterial.Stuff.Code,
          SemiProductStuffName = productionPlanDetail.BillOfMaterial.Stuff.Name,
          OrderId = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItemId, //کد ثبت
          OrderItemPlannedQty = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.OrderItemSummary.PlannedQty,
          OrderItemId = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItemId,
          OrderItemCode = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Code,
          OrderItemQty = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Qty,
          OrderItemUnit = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Unit.Name,
          ProductionRequestId = productionPlanDetail.ProductionPlan.ProductionRequestId,
          ProductionRequestCode = productionPlanDetail.ProductionPlan.ProductionRequest.Code,
          ProductionRequestDeadlineDate = productionPlanDetail.ProductionPlan.ProductionRequest.DeadlineDate,
          ProductionRequestQty = productionPlanDetail.ProductionPlan.ProductionRequest.Qty,
          ProductionRequestUnit = productionPlanDetail.ProductionPlan.ProductionRequest.Unit.Name,
          ParentLevelId = productionPlanDetail.ProductionPlanDetailLevel.ParentId,
          ProductionPlanQty = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Qty,
          ProductionPlanUnitId = productionPlanDetail.ProductionPlan.UnitId,
          ProductionPlanUnitName = productionPlanDetail.ProductionPlan.Unit.Name,
          ProductionStepId = productionPlanDetail.BillOfMaterial.ProductionStepId,
          ProductionStepName = productionPlanDetail.BillOfMaterial.ProductionStep.Name,
          EstimatedDate = productionPlanDetail.ProductionPlan.EstimatedDate,
          Qty = productionPlanDetail.Qty,
          UnitId = productionPlanDetail.UnitId,
          UnitName = productionPlanDetail.Unit.Name,
          ConversionRatio = productionPlanDetail.Unit.ConversionRatio,
          ScheduledQty = productionPlanDetail.ProductionPlanDetailSummary.ScheduledQty,
          RowVersion = productionPlanDetail.RowVersion
        };
    #endregion
    #region SortProductionPlanDetailResult
    public IOrderedQueryable<ProductionPlanDetailResult> SortProductionPlanDetailResult(
        IQueryable<ProductionPlanDetailResult> query,
        SortInput<ProductionPlanDetailSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProductionPlanDetailSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region RemoveProductionPlanDetailProcess
    public void RemoveProductionPlanDetailProcess(int id, byte[] rowVersion)
    {

      #region GetProductionPlan
      var productionPlanDetail = GetProductionPlanDetail(id: id);
      #endregion
      #region RemoveProductionPlan
      RemoveProductionPlanDetailProcess(
      productionPlanDetail: productionPlanDetail,
      rowVersion: rowVersion);
      #endregion
    }
    public void RemoveProductionPlanDetailProcess(
        ProductionPlanDetail productionPlanDetail,
        byte[] rowVersion)
    {

      #region Remove productionPlan BaseEntity
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
      transactionBatchId: null,
      baseEntity: productionPlanDetail,
      rowVersion: rowVersion);
      #endregion
      #region GetProudctionPlanDetails
      var productionSchedules = GetProductionSchedules(
      selector: e => e,
      productionPlanDetailId: productionPlanDetail.Id,
      isDelete: false);
      #endregion
      #region Throw Exception if Exist ProductionSchedule
      if (productionSchedules.Any())
        throw new ProductionPlanDetailHasProudctionScheduleException();
      #endregion
    }
    #endregion
    #region ResetStatus
    public ProductionPlanDetail ResetProductionPlanDetailStatus(int id)
    {

      #region GetProductionPlanDetail
      var productionPlanDetail = GetProductionPlanDetail(id: id);
      #endregion

      return ResetProductionPlanDetailStatus(productionPlanDetail: productionPlanDetail);
    }
    public ProductionPlanDetail ResetProductionPlanDetailStatus(ProductionPlanDetail productionPlanDetail)
    {

      #region ResetProductionPlanDetailSummary
      var productionPlanDetailSummary = ResetProductionPlanDetailSummaryByProductionPlanDetailId(
                  productionPlanDetailId: productionPlanDetail.Id);
      #endregion
      #region Define Status
      var status = ProductionPlanDetailStatus.None;
      if (productionPlanDetailSummary.ScheduledQty > 0)
      {
        if (productionPlanDetailSummary.ScheduledQty >= productionPlanDetailSummary.ProductionPlanDetail.Qty)
          status = status | ProductionPlanDetailStatus.Scheduled;
        else
          status = status | ProductionPlanDetailStatus.Scheduling;
      }

      if (productionPlanDetailSummary.ProducedQty > 0)
      {
        if (productionPlanDetailSummary.ProducedQty >= productionPlanDetailSummary.ProductionPlanDetail.Qty)
          status = status | ProductionPlanDetailStatus.Produced;
        else
          status = status | ProductionPlanDetailStatus.InProduction;
      }

      if (status == ProductionPlanDetailStatus.None)
        status = ProductionPlanDetailStatus.NotAction;
      #endregion
      #region Edit ProductionPlanDetail
      if (status != productionPlanDetail.Status)
        EditProductionPlanDetail(
                      productionPlanDetail: productionPlanDetail,
                      rowVersion: productionPlanDetail.RowVersion,
                      status: status);
      #endregion
      return productionPlanDetail;
    }
    #endregion
  }
}

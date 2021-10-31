using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Models.Supplies.PurchaseOrderDetail;
//using LinqLib.Operators;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region AddProcess
    public PurchaseOrderDetail AddPurchaseOrderDetailProcess(
        int purchaseRequestId,
        int purchaseOrderId,
        int stuffId,
        double qty,
        byte unitId,
        BaseTransaction purchaseRequestTransaction,
        DateTime deadline)
    {

      #region AddTransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region AddPurchaseOrderDetail
      var purchaseOrderDetail = AddPurchaseOrderDetail(
              purchaseOrderDetail: null,
              transactionBatch: transactionBatch,
              purchaseOrderId: purchaseOrderId,
              purchaseRequestId: purchaseRequestId,
              qty: qty,
              unitId: unitId,
              description: null);
      #endregion
      #region AddPurchaseOrderDetailSummary
      AddPurchaseOrderDetailSummary(
              cargoedQty: 0,
              receiptedQty: 0,
              qualityControlPassedQty: 0,
              qualityControlFailedQty: 0,
              purchaseOrderDetailId: purchaseOrderDetail.Id);
      #endregion
      #region Add ExportPurchaseRequest TransactionPlan
      BaseTransaction exportPurchaseRequestTransction = null;
      if (purchaseRequestTransaction != null)
      {
        exportPurchaseRequestTransction = App.Internals.WarehouseManagement.AddTransactionPlanProcess(
                      transactionBatchId: transactionBatch.Id,
                      effectDateTime: purchaseRequestTransaction.EffectDateTime,
                      stuffId: stuffId,
                      billOfMaterialVersion: null,
                      stuffSerialCode: null,
                      transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportPurchaseRequest.Id,
                      amount: qty,
                      unitId: unitId,
                      description: "",
                      isEstimated: false,
                      referenceTransaction: purchaseRequestTransaction);
      }

      #endregion
      #region Add ImportPurchaseOrder TransactionPlan
      App.Internals.WarehouseManagement.AddTransactionPlanProcess(
              transactionBatchId: transactionBatch.Id,
              effectDateTime: deadline,
              stuffId: stuffId,
              billOfMaterialVersion: null,
              stuffSerialCode: null,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportPurchaseOrder.Id,
              amount: qty,
              unitId: unitId,
              description: "",
              isEstimated: false,
              referenceTransaction: exportPurchaseRequestTransction);
      #endregion
      #region ResetPurchaseRequestStatus
      App.Internals.Supplies.ResetPurchaseRequestStatus(id: purchaseRequestId);
      #endregion
      return purchaseOrderDetail;
    }
    #endregion
    #region EditProcess

    public PurchaseOrderDetail EditPurchaseOrderDetailProcess(
        int id,
        byte[] rowVersion,
        TValue<int> purchaseOrderId = null,
        TValue<int?> purchaseRequestId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<string> description = null,
        TValue<TransactionBatch> transactionBatch = null)
    {

      if (transactionBatch == null)
      {
        #region AddTransactionBatch
        transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
        #endregion
      }
      #region GetPurchaseOrderDetail
      var purchaseOrderDetail = GetPurchaseOrderDetail(id: id);
      #endregion
      #region EditPurchaseOrderDetail
      purchaseOrderDetail = EditPurchaseOrderDetail(
          purchaseOrderDetail: purchaseOrderDetail,
          purchaseOrderId: purchaseOrderId,
          purchaseRequestId: purchaseRequestId,
          rowVersion: rowVersion,
          qty: qty,
          unitId: unitId,
          description: description);
      #endregion
      #region Reset PurchaseRequestStatus
      if (purchaseOrderDetail.PurchaseRequestId != null)
      {
        App.Internals.Supplies.ResetPurchaseRequestStatus(id: purchaseOrderDetail.PurchaseRequestId.Value);
      }
      #endregion

      return purchaseOrderDetail;
    }
    #endregion
    #region Add
    public PurchaseOrderDetail AddPurchaseOrderDetail(
       PurchaseOrderDetail purchaseOrderDetail,
       TransactionBatch transactionBatch,
       int purchaseOrderId,
       int? purchaseRequestId,
       double qty,
       byte unitId,
       string description)
    {

      purchaseOrderDetail = purchaseOrderDetail ?? repository.Create<PurchaseOrderDetail>();
      purchaseOrderDetail.PurchaseOrderId = purchaseOrderId;
      purchaseOrderDetail.PurchaseRequestId = purchaseRequestId;
      purchaseOrderDetail.Qty = qty;
      purchaseOrderDetail.UnitId = unitId;
      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: purchaseOrderDetail,
                  transactionBatch: transactionBatch,
                  description: description);
      return purchaseOrderDetail;
    }
    #endregion
    #region Edit
    public PurchaseOrderDetail EditPurchaseOrderDetail(
        PurchaseOrderDetail purchaseOrderDetail,
        byte[] rowVersion,
        TValue<int> purchaseOrderId = null,
        TValue<int?> purchaseRequestId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<string> description = null)
    {

      if (purchaseOrderId != null)
        purchaseOrderDetail.PurchaseOrderId = purchaseOrderId;
      if (purchaseRequestId != null)
        purchaseOrderDetail.PurchaseRequestId = purchaseRequestId;
      if (qty != null)
        purchaseOrderDetail.Qty = qty;
      if (unitId != null)
        purchaseOrderDetail.UnitId = unitId;
      if (description != null)
        purchaseOrderDetail.Description = description;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: purchaseOrderDetail,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as PurchaseOrderDetail;
    }
    #endregion
    #region Get
    public PurchaseOrderDetail GetPurchaseOrderDetail(int id) => GetPurchaseOrderDetail(e => e, id: id);
    public TResult GetPurchaseOrderDetail<TResult>(
        Expression<Func<PurchaseOrderDetail, TResult>> selector,
        int id)
    {

      var purchaseOrderDetail = GetPurchaseOrderDetails(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (purchaseOrderDetail == null)
        throw new PurchaseOrderDetailNotFoundException(id);
      return purchaseOrderDetail;
    }
    public PurchaseOrderDetail GetPurchaseOrderDetail(string code) => GetPurchaseOrderDetail(e => e, code: code);
    public TResult GetPurchaseOrderDetail<TResult>(
        Expression<Func<PurchaseOrderDetail, TResult>> selector,
        string code)
    {

      var purchaseOrderDetail = GetPurchaseOrderDetails(
                    selector: selector,
                    code: code)


                .FirstOrDefault();
      if (purchaseOrderDetail == null)
        throw new PurchaseOrderDetailNotFoundException(code: code);
      return purchaseOrderDetail;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetPurchaseOrderDetails<TResult>(
        Expression<Func<PurchaseOrderDetail, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> purchaseOrderId = null,
        TValue<string> purchaseOrderCode = null,
        TValue<int[]> purchaseOrderIds = null,
        TValue<int> purchaseRequestId = null,
        TValue<double> qty = null,
        TValue<int> unitId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<PurchaseOrderDetail>();
      if (purchaseOrderId != null)
        query = query.Where(r => r.PurchaseOrderId == purchaseOrderId);
      if (purchaseRequestId != null)
        query = query.Where(r => r.PurchaseRequestId == purchaseRequestId);
      if (qty != null)
        query = query.Where(r => Math.Abs(r.Qty - qty) < 0.000001);
      if (unitId != null)
        query = query.Where(r => r.UnitId == unitId);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (purchaseOrderIds != null)
        query = query.Where(i => purchaseOrderIds.Value.Contains(i.PurchaseOrderId));
      if (purchaseOrderCode != null)
        query = query.Where(i => i.PurchaseOrder.Code == purchaseOrderCode);
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (ids != null)
        query = query.Where(i => ids.Value.Contains(i.Id));

      return query.Select(selector);
    }
    public IQueryable<TResult> GetFullPurchaseOrderDetails<TResult>(
        Expression<Func<PurchaseOrderDetail, TResult>> selector,
        TValue<int> id = null,
          TValue<int[]> ids = null,
          TValue<int> purchaseOrderGroupId = null,
          TValue<int?[]> purchaseOrderGroupIds = null,
          TValue<string> code = null,
          TValue<bool> isDelete = null,
          TValue<int> userId = null,
          TValue<int> transactionBatchId = null,
          TValue<string> description = null,
          TValue<int> purchaseOrderId = null,
          TValue<string> purchaseOrderCode = null,
          TValue<int[]> purchaseOrderIds = null,
          TValue<int> purchaseRequestId = null,
          TValue<int?> providerId = null,
          TValue<ProviderType> providerType = null,
          TValue<int?> employeeId = null,
          TValue<int?> supplierId = null,
          TValue<PurchaseOrderType> purchaseOrderType = null,
          TValue<double?> price = null,
          TValue<int?> currencyId = null,
          TValue<DateTime> deadline = null,
          TValue<double> qty = null,
          TValue<int> unitId = null,
          TValue<int> cargoId = null,
          TValue<int> stuffId = null,
          TValue<int> financialTransactionBatchId = null,
          TValue<int> financialDocumentId = null,
          TValue<FinancialDocumentTypeResult> financialDocumentTypeResult = null,
          TValue<string> planCode = null,
          TValue<string> purchaseRequsetDescription = null,
          TValue<PurchaseOrderStatus> status = null,
          TValue<PurchaseOrderStatus[]> statuses = null,
          TValue<PurchaseOrderStatus[]> notHasStatuses = null,
          TValue<DateTime> fromDate = null,
          TValue<DateTime> toDate = null,
          TValue<DateTime> fromDeadLine = null,
          TValue<DateTime> toDeadLine = null,
          TValue<DateTime> PurchaseOrderDateTime = null,
          TValue<RiskLevelStatus> riskLevelStatus = null,
          TValue<int> stuffCategoryId = null,
          TValue<FinanceAllocationStatus> financeAllocationStatus = null,
          TValue<string> purchaseOrderGroupCode = null,
          TValue<string> orderCode = null,
          TValue<bool> isArchived = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<PurchaseOrderDetail>();
      if (purchaseOrderId != null)
        query = query.Where(r => r.PurchaseOrderId == purchaseOrderId);
      if (purchaseRequestId != null)
        query = query.Where(r => r.PurchaseRequestId == purchaseRequestId);
      if (qty != null)
        query = query.Where(r => Math.Abs(r.Qty - qty) < 0.000001);
      if (unitId != null)
        query = query.Where(r => r.UnitId == unitId);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (purchaseOrderIds != null)
        query = query.Where(i => purchaseOrderIds.Value.Contains(i.PurchaseOrderId));
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (ids != null)
        query = query.Where(i => ids.Value.Contains(i.Id));
      if (providerId != null)
        query = query.Where(r => r.PurchaseOrder.ProviderId == providerId);
      if (employeeId != null)
        query = query.Where(r => r.User.Employee.Id == employeeId);
      if (supplierId != null)
        query = query.Where(r => r.PurchaseOrder.SupplierId == supplierId);
      if (purchaseOrderType != null)
        query = query.Where(r => r.PurchaseOrder.PurchaseOrderType == purchaseOrderType);
      if (price != null)
        query = query.Where(r => r.PurchaseOrder.Price == price);
      if (currencyId != null)
        query = query.Where(r => r.PurchaseOrder.CurrencyId == currencyId);
      if (deadline != null)
        query = query.Where(r => r.PurchaseOrder.Deadline == deadline);
      if (unitId != null)
        query = query.Where(r => r.UnitId == unitId);
      if (stuffId != null)
        query = query.Where(r => r.PurchaseOrder.StuffId == stuffId);
      if (purchaseOrderGroupId != null)
        query = query.Where(i => i.PurchaseOrder.PurchaseOrderGroupId == purchaseOrderGroupId);
      if (cargoId != null)
        query = query.Where(i => i.CargoItemDetails.Any(n => n.CargoItem.CargoId == cargoId));
      if (status != null)
        query = query.Where(i => i.PurchaseOrder.Status.HasFlag(status));
      if (ids != null)
        query = query.Where(i => ids.Value.Contains(i.Id));
      if (purchaseOrderGroupIds != null)
        query = query.Where(i => purchaseOrderGroupIds.Value.Contains(i.PurchaseOrder.PurchaseOrderGroupId));
      if (financialTransactionBatchId != null)
        query = query.Where(i => i.FinancialTransactionBatch.Id == financialTransactionBatchId);

      if (financialDocumentId != null && financialDocumentTypeResult != null)
      {
        if (financialDocumentTypeResult == FinancialDocumentTypeResult.PurchaseOrderDiscount)
        {
          query = from item in query
                  from PurchaseOrderDiscount in item.PurchaseOrder.PurchaseOrderDiscounts
                  where PurchaseOrderDiscount.FinancialDocumentDiscount.FinancialDocument.Id.ToString().Contains(financialDocumentId.ToString())
                  select item;
          query = query.Distinct();
        }
        else if (financialDocumentTypeResult == FinancialDocumentTypeResult.PurchaseOrderCost)
        {
          query = from item in query
                  from purchaseOrderCost in item.PurchaseOrder.PurchaseOrderCosts
                  where purchaseOrderCost.FinancialDocumentCost.FinancialDocument.Id.ToString().Contains(financialDocumentId.ToString())
                  select item;
          query = query.Distinct();
        }
      }

      if (isArchived != null)
        query = query.Where(i => i.PurchaseOrder.IsArchived == isArchived);

      if (purchaseRequsetDescription != null && purchaseRequsetDescription != "")
      {
        query = from item in query
                from purchaseOrderDetail in item.PurchaseOrder.PurchaseOrderDetails
                where purchaseOrderDetail.PurchaseRequest.Description.Contains(purchaseRequsetDescription)
                select item;
        query = query.Distinct();
      }
      if (planCode != null && planCode != "")
      {
        query = from item in query
                from purchaseOrderDetail in item.PurchaseOrder.PurchaseOrderDetails
                where purchaseOrderDetail.PurchaseRequest.PlanCode.Code.Contains(planCode)
                select item;
        query = query.Distinct();
      }
      if (orderCode != null)
      {
        query = query.Where(i => i.PurchaseOrder.Code.Contains(orderCode));
      }

      if (statuses != null)
      {
        var s = PurchaseOrderStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        query = query.Where(i => (i.PurchaseOrder.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = PurchaseOrderStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        query = query.Where(i => (i.PurchaseOrder.Status & s) == 0);
      }
      if (fromDate != null)
        query = query.Where(r => r.PurchaseOrder.PurchaseOrderDateTime >= fromDate);
      if (toDate != null)
        query = query.Where(r => r.PurchaseOrder.PurchaseOrderDateTime <= toDate);
      if (fromDeadLine != null)
        query = query.Where(r => r.PurchaseOrder.Deadline >= fromDeadLine);
      if (toDeadLine != null)
        query = query.Where(r => r.PurchaseOrder.Deadline >= toDeadLine);
      if (providerType != null)
        query = query.Where(r => r.PurchaseOrder.Provider.ProviderType == providerType);
      if (stuffCategoryId != null)
        query = query.Where(i => i.PurchaseOrder.Stuff.StuffCategoryId == stuffCategoryId);
      if (riskLevelStatus != null)
      {
        if (riskLevelStatus.Value == RiskLevelStatus.Low)
        {
          query = query.Where(i => i.PurchaseOrder.LatestRisk.LatestRiskStatus.RiskParameter.RiskLevelStatus == RiskLevelStatus.Low
                || i.PurchaseOrder.LatestRisk == null);
        }
        else
          query = query.Where(i => i.PurchaseOrder.LatestRisk.LatestRiskStatus.RiskParameter.RiskLevelStatus == riskLevelStatus.Value);

      }
      if (purchaseOrderGroupCode != null)
        query = query.Where(i => i.PurchaseOrder.PurchaseOrderGroup.Code.Contains(purchaseOrderGroupCode));

      if (statuses != null)
      {
        var s = PurchaseOrderStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        query = query.Where(i => (i.PurchaseOrder.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = PurchaseOrderStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        query = query.Where(i => (i.PurchaseOrder.Status & s) == 0);
      }

      return query.Select(selector);
    }
    #endregion
    #region Remove
    public void RemovePurchaseOrderDetail(
        int id,
        byte[] rowVersion,
        TransactionBatch transactionBatch)
    {

      var purchaseOrderDetail = GetPurchaseOrderDetail(id: id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: transactionBatch.Id,
                    baseEntity: purchaseOrderDetail,
                    rowVersion: rowVersion);
    }
    #endregion
    #region RemoveProcess
    public void RemovePurchaseOrderDetailProcess(
        int id,
        byte[] rowVersion,
        TransactionBatch transactionBatch)
    {

      if (transactionBatch == null)
      {
        #region AddTransactionBatch
        transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
        #endregion
      }
      #region Remove PurchaseOrderDetail
      RemovePurchaseOrderDetail(
              id: id,
              rowVersion: rowVersion,
              transactionBatch: transactionBatch);
      //var cargoItem = GetCargoItem(id: id);
      #endregion
      #region Get PurchaseOrderDetail
      var purchaseOrderDetail = GetPurchaseOrderDetail(id: id);
      #endregion
      #region Reset PurchaseRequestStatus
      if (purchaseOrderDetail.PurchaseRequestId != null)
      {
        App.Internals.Supplies.ResetPurchaseRequestStatus(id: purchaseOrderDetail.PurchaseRequestId.Value);
      }
      #endregion
    }
    #endregion
    #region ResetStatus
    public PurchaseOrderDetail ResetPurchaseOrderDetailStatus(int purchaseOrderDetailId)
    {

      var purchaseOrderDetail = GetPurchaseOrderDetail(id: purchaseOrderDetailId);
      return ResetPurchaseOrderDetailStatus(purchaseOrderDetail: purchaseOrderDetail);
    }
    public PurchaseOrderDetail ResetPurchaseOrderDetailStatus(PurchaseOrderDetail purchaseOrderDetail)
    {

      #region ResetOrderItemSummary
      var purchaseOrderDetailSummary = ResetPurchaseOrderDetailSummaryByPurchaseOrderDetailId(
              purchaseOrderDetailId: purchaseOrderDetail.Id);
      #endregion
      return purchaseOrderDetail;
    }
    #endregion
    #region ToResult
    public Expression<Func<PurchaseOrderDetail, PurchaseOrderDetailResult>> ToPurchaseOrderDetailResult =
        purchaseOrderDetail => new PurchaseOrderDetailResult()
        {
          Id = purchaseOrderDetail.Id,
          Code = purchaseOrderDetail.Code,
          PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId,
          PurchaseOrderCode = purchaseOrderDetail.PurchaseOrder.Code,
          Qty = purchaseOrderDetail.Qty,
          UnitId = purchaseOrderDetail.UnitId,
          UnitName = purchaseOrderDetail.Unit.Name,
          ConversionRatio = purchaseOrderDetail.Unit.ConversionRatio,
          DecimalDigitCount = purchaseOrderDetail.Unit.DecimalDigitCount,
          CargoedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
          QualityControlPassedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.QualityControlPassedQty,
          ReceiptedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.ReceiptedQty,
          RemainedQty = purchaseOrderDetail.Qty - purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
          TotalWeight = (purchaseOrderDetail.PurchaseOrder.Stuff.GrossWeight ?? 0) * purchaseOrderDetail.Qty,
          RowVersion = purchaseOrderDetail.RowVersion,

          PurchaseRequestId = purchaseOrderDetail.PurchaseRequestId,
          PurchaseRequestCode = purchaseOrderDetail.PurchaseRequest.Code,
          PurchaseRequestDepartmentName = purchaseOrderDetail.PurchaseRequest.User.Employee.Department.Name,
          PurchaseRequestEmployeeFullName = purchaseOrderDetail.PurchaseRequest.User.Employee == null ? null : (purchaseOrderDetail.PurchaseRequest.User.Employee.FirstName + " " + purchaseOrderDetail.PurchaseRequest.User.Employee.LastName),
          PurchaseRequestResponsibleEmployeeFullName = purchaseOrderDetail.PurchaseRequest.ResponsibleEmployee == null ? null : (purchaseOrderDetail.PurchaseRequest.ResponsibleEmployee.FirstName + " " + purchaseOrderDetail.PurchaseRequest.ResponsibleEmployee.LastName),
          PurchaseRquestDeadline = purchaseOrderDetail.PurchaseRequest.Deadline,
          PurchaseRequestQty = purchaseOrderDetail.PurchaseRequest.Qty * purchaseOrderDetail.PurchaseRequest.Unit.ConversionRatio / purchaseOrderDetail.Unit.ConversionRatio,
          PurchaseRequestUnitId = purchaseOrderDetail.PurchaseRequest.UnitId,
          PurchaseRequestUnitName = purchaseOrderDetail.PurchaseRequest.Unit.Name,
          PurchaseRequestCargoedQty = purchaseOrderDetail.PurchaseRequest.PurchaseRequestSummary.CargoedQty * purchaseOrderDetail.PurchaseRequest.Unit.ConversionRatio / purchaseOrderDetail.Unit.ConversionRatio,
          PurchaseRequestReceiptedQty = purchaseOrderDetail.PurchaseRequest.PurchaseRequestSummary.ReceiptedQty * purchaseOrderDetail.PurchaseRequest.Unit.ConversionRatio / purchaseOrderDetail.Unit.ConversionRatio,
          PurchaseRequsetRemainedQty = (purchaseOrderDetail.PurchaseRequest.Qty - purchaseOrderDetail.PurchaseRequest.PurchaseRequestSummary.OrderedQty) * purchaseOrderDetail.PurchaseRequest.Unit.ConversionRatio / purchaseOrderDetail.Unit.ConversionRatio,
          PurchaseRequestDateTime = purchaseOrderDetail.PurchaseRequest.DateTime,
          PurchaseRequestStatus = purchaseOrderDetail.PurchaseRequest.Status,
          PurchaseRequsetDescription = purchaseOrderDetail.PurchaseRequest.Description,
          PurchaseOrderDetailStuffId = purchaseOrderDetail.PurchaseOrder.StuffId,
          PurchaseOrderDetailStuffCode = purchaseOrderDetail.PurchaseOrder.Stuff.Code,
          PurchaseOrderDetailStuffName = purchaseOrderDetail.PurchaseOrder.Stuff.Name,
          PurchaseOrderDetailQty = purchaseOrderDetail.Qty,
          PurchaseOrderDetailUnitId = purchaseOrderDetail.UnitId,
          PurchaseOrderDetailUnitName = purchaseOrderDetail.Unit.Name,
          PurchaseOrderDetailProviderId = purchaseOrderDetail.PurchaseOrder.ProviderId,
          PurchaseOrderDetailProviderName = purchaseOrderDetail.PurchaseOrder.Provider.Name,
          PurchaseOrderDetailCargoedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
          PurchaseOrderDetailReceiptQty = purchaseOrderDetail.PurchaseOrderDetailSummary.ReceiptedQty,
          PurchaseOrderDetailRemainedQty = purchaseOrderDetail.Qty - purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
          PurchaseOrderDetailDeadline = purchaseOrderDetail.PurchaseOrder.Deadline,
          PlanCode = purchaseOrderDetail.PurchaseRequest.PlanCode.Code,
        };



    public IQueryable<FullPurchaseOrderDetailResult> ToFullPurchaseOrderResultQuery(IQueryable<PurchaseOrderDetail> purchaseOrderDetails,
        IQueryable<BaseEntityDocument> latestBaseEntityDocuments, IQueryable<BaseEntityConfirmation> purchaseOrderConfirmations, IQueryable<FinanceItem> financeItems)
    {
      var latestBaseEntityDocument = from purchaseOrderDetail in purchaseOrderDetails
                                     join baseEntityDocument in latestBaseEntityDocuments
                                     on purchaseOrderDetail.PurchaseOrderId equals baseEntityDocument.BaseEntityId into baseEntities
                                     from items in baseEntities.DefaultIfEmpty()
                                     select items;
      var finance = from latestBase in latestBaseEntityDocument
                    join financeItem in financeItems on
                    latestBase.BaseEntityId equals financeItem.PurchaseOrderId into tItems
                    from tItem in tItems.DefaultIfEmpty()
                    select tItem;
      var query = from purchaseOrderDetail in purchaseOrderDetails
                  join baseEntityDocument in latestBaseEntityDocuments
                  on purchaseOrderDetail.PurchaseOrderId equals baseEntityDocument.BaseEntityId into baseEntities
                  from baseEntityItem in baseEntities.DefaultIfEmpty()
                  join financeItem in financeItems on baseEntityItem.BaseEntityId equals financeItem.PurchaseOrderId into finances
                  from fItem in finances.DefaultIfEmpty()
                  join baseEntityConfirmation in purchaseOrderConfirmations on fItem.PurchaseOrderId equals baseEntityConfirmation.ConfirmingEntityId into confiramtions
                  from confirmationItem in confiramtions.DefaultIfEmpty()
                  select new FullPurchaseOrderDetailResult
                  {
                    Id = purchaseOrderDetail.Id,
                    Code = purchaseOrderDetail.Code,
                    OrderCode = purchaseOrderDetail.PurchaseOrder.Code,
                    PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId,
                    PurchaseOrderCode = purchaseOrderDetail.PurchaseOrder.Code,
                    Qty = purchaseOrderDetail.Qty,
                    UnitId = purchaseOrderDetail.UnitId,
                    UnitName = purchaseOrderDetail.Unit.Name,
                    ConversionRatio = purchaseOrderDetail.Unit.ConversionRatio,
                    DecimalDigitCount = purchaseOrderDetail.Unit.DecimalDigitCount,
                    CargoedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                    QualityControlPassedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.QualityControlPassedQty,
                    ReceiptedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.ReceiptedQty,
                    RemainedQty = purchaseOrderDetail.Qty - purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                    TotalWeight = (purchaseOrderDetail.PurchaseOrder.Stuff.GrossWeight ?? 0) * purchaseOrderDetail.Qty,
                    RowVersion = purchaseOrderDetail.RowVersion,
                    PurchaseRequestId = purchaseOrderDetail.PurchaseRequestId,
                    PurchaseRequestCode = purchaseOrderDetail.PurchaseRequest.Code,
                    PurchaseRequestDepartmentName = purchaseOrderDetail.PurchaseRequest.User.Employee.Department.Name,
                    PurchaseRequestEmployeeFullName = purchaseOrderDetail.PurchaseRequest.User.Employee == null ? null : (purchaseOrderDetail.PurchaseRequest.User.Employee.FirstName + " " + purchaseOrderDetail.PurchaseRequest.User.Employee.LastName),
                    PurchaseRequestResponsibleEmployeeFullName = purchaseOrderDetail.PurchaseRequest.ResponsibleEmployee == null ? null : (purchaseOrderDetail.PurchaseRequest.ResponsibleEmployee.FirstName + " " + purchaseOrderDetail.PurchaseRequest.ResponsibleEmployee.LastName),
                    PurchaseRquestDeadline = purchaseOrderDetail.PurchaseRequest.Deadline,
                    PurchaseRequestQty = purchaseOrderDetail.PurchaseRequest.Qty * purchaseOrderDetail.PurchaseRequest.Unit.ConversionRatio / purchaseOrderDetail.Unit.ConversionRatio,
                    PurchaseRequestUnitId = purchaseOrderDetail.PurchaseRequest.UnitId,
                    PurchaseRequestUnitName = purchaseOrderDetail.PurchaseRequest.Unit.Name,
                    PurchaseRequestCargoedQty = purchaseOrderDetail.PurchaseRequest.PurchaseRequestSummary.CargoedQty * purchaseOrderDetail.PurchaseRequest.Unit.ConversionRatio / purchaseOrderDetail.Unit.ConversionRatio,
                    PurchaseRequestReceiptedQty = purchaseOrderDetail.PurchaseRequest.PurchaseRequestSummary.ReceiptedQty * purchaseOrderDetail.PurchaseRequest.Unit.ConversionRatio / purchaseOrderDetail.Unit.ConversionRatio,
                    PurchaseRequsetRemainedQty = (purchaseOrderDetail.PurchaseRequest.Qty - purchaseOrderDetail.PurchaseRequest.PurchaseRequestSummary.OrderedQty) * purchaseOrderDetail.PurchaseRequest.Unit.ConversionRatio / purchaseOrderDetail.Unit.ConversionRatio,
                    PurchaseOrderDateTime = purchaseOrderDetail.PurchaseOrder.DateTime,
                    PurchaseRequestStatus = purchaseOrderDetail.PurchaseRequest.Status,
                    PurchaseRequsetDescription = purchaseOrderDetail.PurchaseRequest.Description,
                    PurchaseOrderDetailStuffId = purchaseOrderDetail.PurchaseOrder.StuffId,
                    PurchaseOrderDetailStuffCode = purchaseOrderDetail.PurchaseOrder.Stuff.Code,
                    PurchaseOrderDetailStuffName = purchaseOrderDetail.PurchaseOrder.Stuff.Name,
                    PurchaseOrderDetailQty = purchaseOrderDetail.Qty,
                    PurchaseOrderDetailUnitId = purchaseOrderDetail.UnitId,
                    PurchaseOrderDetailUnitName = purchaseOrderDetail.Unit.Name,
                    PurchaseOrderDetailProviderId = purchaseOrderDetail.PurchaseOrder.ProviderId,
                    PurchaseOrderDetailProviderName = purchaseOrderDetail.PurchaseOrder.Provider.Name,
                    PurchaseOrderDetailCargoedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                    PurchaseOrderDetailReceiptQty = purchaseOrderDetail.PurchaseOrderDetailSummary.ReceiptedQty,
                    PurchaseOrderDetailRemainedQty = purchaseOrderDetail.Qty - purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                    PurchaseOrderDetailDeadline = purchaseOrderDetail.PurchaseOrder.Deadline,
                    PurchaseRequestDateTime = purchaseOrderDetail.PurchaseRequest.DateTime,
                    PlanCode = purchaseOrderDetail.PurchaseRequest.PlanCode.Code,
                    SupplierFullName = purchaseOrderDetail.PurchaseOrder.Supplier.Employee.FirstName + " " + purchaseOrderDetail.PurchaseOrder.Supplier.Employee.LastName,
                    PurchaseOrderType = purchaseOrderDetail.PurchaseOrder.PurchaseOrderType,
                    ProviderId = purchaseOrderDetail.PurchaseOrder.ProviderId,
                    ProviderName = purchaseOrderDetail.PurchaseOrder.StuffProvider.Provider.Name,
                    ProviderCode = purchaseOrderDetail.PurchaseOrder.StuffProvider.Provider.Code,
                    StuffId = purchaseOrderDetail.PurchaseOrder.StuffId,
                    StuffCode = purchaseOrderDetail.PurchaseOrder.Stuff.Code,
                    StuffName = purchaseOrderDetail.PurchaseOrder.Stuff.Name,
                    StuffGrossWeight = purchaseOrderDetail.PurchaseOrder.Stuff.GrossWeight,
                    Price = purchaseOrderDetail.PurchaseOrder.Price,
                    CurrencuyId = purchaseOrderDetail.PurchaseOrder.CurrencyId,
                    CurrencyTitle = purchaseOrderDetail.PurchaseOrder.Currency.Title,
                    CurrencySign = purchaseOrderDetail.PurchaseOrder.Currency.Sign,
                    CurrencyDecimalDigitCount = purchaseOrderDetail.PurchaseOrder.Currency.DecimalDigitCount,
                    Deadline = purchaseOrderDetail.PurchaseOrder.Deadline,
                    Description = purchaseOrderDetail.PurchaseOrder.Description,
                    QualityControlFailedQty = purchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.QualityControlFailedQty,
                    StuffCategoryId = purchaseOrderDetail.PurchaseOrder.Stuff.StuffCategoryId,
                    StuffCategoryName = purchaseOrderDetail.PurchaseOrder.Stuff.StuffCategory.Name,
                    PurchaseOrderStatus = purchaseOrderDetail.PurchaseOrder.Status,
                    FinancialTransacionBatchId = purchaseOrderDetail.PurchaseOrder.FinancialTransactionBatch.Id,
                    EmployeeFullName = purchaseOrderDetail.PurchaseOrder.User.Employee.FirstName + " " + purchaseOrderDetail.PurchaseOrder.User.Employee.LastName,
                    PurchaseOrderGroupCode = purchaseOrderDetail.PurchaseOrder.PurchaseOrderGroup.Code,
                    PurchaseOrderGroupId = purchaseOrderDetail.PurchaseOrder.PurchaseOrderGroup.Id,
                    TotalPrice = purchaseOrderDetail.PurchaseOrder.Price * purchaseOrderDetail.PurchaseOrder.Qty,
                    RiskLevel = purchaseOrderDetail.PurchaseOrder.LatestRisk == null ? RiskLevelStatus.Low : purchaseOrderDetail.PurchaseOrder.LatestRisk.LatestRiskStatus.RiskParameter.RiskLevelStatus,
                    PurchaseOrderPreparingDateTime = purchaseOrderDetail.PurchaseOrder.PurchaseOrderPreparingDateTime,
                    OrderInvoiceNum = purchaseOrderDetail.PurchaseOrder.OrderInvoiceNum,
                    PurchaseOrderStepName = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetail.PurchaseOrderStep.Name,
                    PurchaseOrderStepDetailDescription = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetail.Description,
                    PurchaseOrderStepChangeTime = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetail.DateTime,
                    LatestBaseEntityDocumentDescription = baseEntityItem.Description,
                    LatestBaseEntityDocumentDateTime = baseEntityItem.DateTime,
                    FinanceAllocationStatus = fItem.AllocatedAmount == null ?
                            FinanceAllocationStatus.NotAllocated :
                            (purchaseOrderDetail.PurchaseOrder.Price * purchaseOrderDetail.PurchaseOrder.Qty) ==
                            (fItem.AllocatedAmount) ?
                            FinanceAllocationStatus.CompletelyAllocated : FinanceAllocationStatus.IncompleteAllocated,
                    AllocatedAmount = (fItem.AllocatedAmount) ?? 0,
                    RemainingAmount = (purchaseOrderDetail.PurchaseOrder.Price * purchaseOrderDetail.PurchaseOrder.Qty) -
                      (fItem.AllocatedAmount ?? 0),
                    PriceConfirmerFullName = confirmationItem.User.Employee.FirstName + " " + confirmationItem.User.Employee.LastName,
                    PriceConfirmDescription = confirmationItem.ConfirmDescription,
                    PriceConfirmationStatus = confirmationItem.Status,
                    MaxEstimateDateTime = purchaseOrderDetail.PurchaseOrder.CargoItems.Where(c => c.PurchaseOrderId == purchaseOrderDetail.PurchaseOrderId).Select(i => i.EstimateDateTime).Max(),
                    MinDeadlinePurchaseRequest = purchaseOrderDetail.PurchaseOrder.PurchaseOrderDetails.Select(i => i.PurchaseRequest.Deadline).Min(),
                  };
      return query;
    }
    #endregion
    #region Search
    public IQueryable<FullPurchaseOrderDetailResult> SearchFullPurchaseOrderResult(
        IQueryable<FullPurchaseOrderDetailResult> query,
        IQueryable<PurchaseOrderDetail> purchaseOrderDetails,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText,
        int[] purchaseOrderIds,
        string purchaseOrderGroupCode,
        int? purchaseOrderGroupId,
        int? stuffCategoryId,
        string planCode,
        FinanceAllocationStatus? financeAllocationStatus)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where
                      item.Code.Contains(searchText) ||
                      item.StuffName.Contains(searchText) ||
                      item.StuffCode.Contains(searchText) ||
                      item.Description.Contains(searchText) ||
                      item.CurrencyTitle.Contains(searchText) ||
                      item.ProviderCode.Contains(searchText) ||
                      item.ProviderName.Contains(searchText) ||
                      item.PurchaseOrderGroupCode.Contains(searchText) ||
                      item.UnitName.Contains(searchText)
                select item;
      if (purchaseOrderIds != null)
        query = query.Where(i => purchaseOrderIds.Contains(i.Id));
      if (stuffCategoryId != null)
        query = query.Where(i => i.StuffCategoryId == stuffCategoryId);
      if (financeAllocationStatus != null)
        query = query.Where(i => i.FinanceAllocationStatus == financeAllocationStatus);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      if (purchaseOrderGroupCode != null)
        query = query.Where(i => i.PurchaseOrderGroupCode == purchaseOrderGroupCode);
      if (purchaseOrderGroupId != null)
        query = query.Where(i => i.PurchaseOrderGroupId == purchaseOrderGroupId);
      return query;
    }
    #region Sort
    public IOrderedQueryable<PurchaseOrderDetailResult> SortPurchaseOrderDetailResult(IQueryable<PurchaseOrderDetailResult> query, SearchInput<PurchaseOrderDetailSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case PurchaseOrderDetailSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<FullPurchaseOrderDetailResult> SortFullPurchaseOrderDetailResult(IQueryable<FullPurchaseOrderDetailResult> query, SearchInput<PurchaseOrderDetailSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case PurchaseOrderDetailSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.Code:
          return query.OrderBy(i => i.Code, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.OrderInvoiceNum:
          return query.OrderBy(i => i.OrderInvoiceNum, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.Price:
          return query.OrderBy(i => i.Price, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.TotalPrice:
          return query.OrderBy(i => i.TotalPrice, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.CurrencyTitle:
          return query.OrderBy(i => i.CurrencyTitle, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.Deadline:
          return query.OrderBy(i => i.Deadline, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.DateTime:
          return query.OrderBy(i => i.DateTime, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.PurchaseOrderDateTime:
          return query.OrderBy(i => i.PurchaseOrderDateTime, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.Qty:
          return query.OrderBy(i => i.Qty, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.RemainedQty:
          return query.OrderBy(i => i.RemainedQty, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.PurchaseOrderStatus:
          return query.OrderBy(i => i.PurchaseOrderStatus, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.UnitName:
          return query.OrderBy(i => i.UnitName, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.StuffCategoryName:
          return query.OrderBy(i => i.StuffCategoryName, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.CargoedQty:
          return query.OrderBy(i => i.CargoedQty, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.RemainingAmount:
          return query.OrderBy(i => i.RemainingAmount, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.AllocatedAmount:
          return query.OrderBy(i => i.CargoedQty, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.EmployeeFullName:
          return query.OrderBy(i => i.EmployeeFullName, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.ProviderName:
          return query.OrderBy(i => i.ProviderName, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.ProviderCode:
          return query.OrderBy(i => i.ProviderCode, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.SupplierFullName:
          return query.OrderBy(i => i.SupplierFullName, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.PurchaseOrderType:
          return query.OrderBy(i => i.PurchaseOrderType, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.ReceiptedQty:
          return query.OrderBy(i => i.ReceiptedQty, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.QualityControlPassedQty:
          return query.OrderBy(i => i.QualityControlPassedQty, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.QualityControlFailedQty:
          return query.OrderBy(i => i.QualityControlFailedQty, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.PurchaseRequsetDescription:
          return query.OrderBy(i => i.Code);//.OrderBy(i => i.PurchaseRequestDescriptionArray, sortInput.SortOrder);  Not Supported !!!
        case PurchaseOrderDetailSortType.LatestBaseEntityDocumentDescription:
          return query.OrderBy(i => i.LatestBaseEntityDocumentDescription, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.LatestBaseEntityDocumentDateTime:
          return query.OrderBy(i => i.LatestBaseEntityDocumentDateTime, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.PlanCode:
          return query.OrderBy(i => i.Code);//OrderBy(i => i.PlanCodeArray, sortInput.SortOrder);  Not Supported !!!
        case PurchaseOrderDetailSortType.PriceConfirmationStatus:
          return query.OrderBy(i => i.PriceConfirmationStatus, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.PriceConfirmerFullName:
          return query.OrderBy(i => i.PriceConfirmerFullName, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.PriceConfirmDescription:
          return query.OrderBy(i => i.PriceConfirmDescription, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.IsArchived:
          return query.OrderBy(i => i.IsArchived, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.PurchaseOrderStepName:
          return query.OrderBy(i => i.PurchaseOrderStepName, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.PurchaseOrderStepDetailDescription:
          return query.OrderBy(i => i.PurchaseOrderStepDetailDescription, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.PurchaseOrderStepChangeTime:
          return query.OrderBy(i => i.PurchaseOrderStepChangeTime, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.FinanceAllocationStatus:
          return query.OrderBy(i => i.FinanceAllocationStatus, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.RiskLevelStatus:
          return query.OrderBy(i => i.RiskLevelStatus, sortInput.SortOrder);
        case PurchaseOrderDetailSortType.PurchaseOrderGroupCode:
          return query.OrderBy(i => i.PurchaseOrderGroupCode, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion


  }
  #endregion
}

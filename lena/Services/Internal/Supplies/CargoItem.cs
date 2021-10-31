//using LinqLib.Operators;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.CargoItem;
using lena.Models.Supplies.Ladings;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public CargoItem AddCargoItem(
        CargoItem cargoItem,
        TransactionBatch transactionBatch,
        FinancialTransactionBatch financialTransactionBatch,
        string description,
        int cargoId,
        int purchaseOrderId,
        double qty,
        byte unitId,
        DateTime estimateDateTime,
        DateTime cargoItemDateTime,
        short howToBuyId,
        BuyingProcess buyingProcess,
        int? forwarderId,
        Guid? forwarderDocumentId
        )
    {

      cargoItem = cargoItem ?? repository.Create<CargoItem>();
      cargoItem.CargoId = cargoId;
      cargoItem.PurchaseOrderId = purchaseOrderId;
      cargoItem.Qty = qty;
      cargoItem.UnitId = unitId;
      cargoItem.EstimateDateTime = estimateDateTime;
      cargoItem.CargoItemDateTime = cargoItemDateTime;
      cargoItem.HowToBuyId = howToBuyId;
      cargoItem.ForwarderId = forwarderId;
      cargoItem.BuyingProcess = buyingProcess;
      cargoItem.ForwarderDocumentId = forwarderDocumentId;

      cargoItem.Status = CargoItemStatus.NotAction;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: cargoItem,
                    transactionBatch: transactionBatch,
                    financialTransactionBatch: financialTransactionBatch,
                    description: description);
      return cargoItem;
    }

    internal IQueryable<LadingResult> SearchLadingsResults(IQueryable<LadingResult> query, string searchText, AdvanceSearchItem[] advanceSearchItems, int? currentBankOrderStatusId, int? currentCustomhouseStatusId, DateTime? fromDeliveryDate, DateTime? toDeliveryDate, DateTime? fromTransportDate, DateTime? toTransportDate, string bankOrderNumber, object isLocked)
    {
      throw new NotImplementedException();
    }
    #endregion
    #region Edit
    public CargoItem EditCargoItem(
        int id,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<int> cargoId = null,
        TValue<int> purchaseOrderId = null,
        TValue<DateTime> cargoItemDateTime = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<CargoItemStatus> status = null,
        TValue<int?> ladingId = null,
        TValue<bool> isArchived = null,
        TValue<FinancialTransactionBatch> financialTransactionBatch = null,
        TValue<BuyingProcess> buyingProvessStatus = null,
        TValue<int> forwarderId = null,
        TValue<Risk> risk = null)
    {

      var cargoItem = GetCargoItem(id: id);
      return EditCargoItem(
                    cargoItem: cargoItem,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description,
                    cargoId: cargoId,
                    purchaseOrderId: purchaseOrderId,
                    cargoItemDateTime: cargoItemDateTime,
                    qty: qty,
                    unitId: unitId,
                    status: status,
                    isArchived: isArchived,
                    buyingProvessStatus: buyingProvessStatus,
                    financialTransactionBatch: financialTransactionBatch,
                    forwarderId: forwarderId,
                    risk: risk);
    }
    public CargoItem EditCargoItem(
        CargoItem cargoItem,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<int> cargoId = null,
        TValue<int> purchaseOrderId = null,
        TValue<DateTime> cargoItemDateTime = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<CargoItemStatus> status = null,
        TValue<bool> isArchived = null,
        TValue<FinancialTransactionBatch> financialTransactionBatch = null,
        TValue<BuyingProcess> buyingProvessStatus = null,
        TValue<int> forwarderId = null,
        TValue<Guid?> forwarderDocumentId = null,
        TValue<Risk> risk = null)
    {

      if (cargoId != null)
        cargoItem.CargoId = cargoId;

      if (cargoItemDateTime != null)
        cargoItem.CargoItemDateTime = cargoItemDateTime;
      if (purchaseOrderId != null)
        cargoItem.PurchaseOrderId = purchaseOrderId;
      if (qty != null)
        cargoItem.Qty = Math.Round(qty, cargoItem.Unit.DecimalDigitCount);
      if (unitId != null)
        cargoItem.UnitId = unitId;
      if (status != null)
        cargoItem.Status = status;
      if (isArchived != null)
        cargoItem.IsArchived = isArchived;
      if (buyingProvessStatus != null)
        cargoItem.BuyingProcess = buyingProvessStatus;
      if (forwarderId != null)
        cargoItem.ForwarderId = forwarderId;
      if (forwarderDocumentId != null)
        cargoItem.ForwarderDocumentId = forwarderDocumentId;
      if (risk != null)
        cargoItem.LatestRisk = risk;

      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: cargoItem,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    financialTransactionBatch: financialTransactionBatch,
                    description: description);
      return retValue as CargoItem;
    }
    #endregion
    #region Remove
    public void RemoveCargoItem(int id, byte[] rowVersion)
    {

      var cargoItem = GetCargoItem(id: id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: null,
                    baseEntity: cargoItem,
                    rowVersion: rowVersion);
    }
    #endregion
    #region Get
    public CargoItem GetCargoItem(int id) => GetCargoItem(selector: e => e, id: id);
    public TResult GetCargoItem<TResult>(
        Expression<Func<CargoItem, TResult>> selector,
        int id)
    {

      var cargoItem = GetCargoItems(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (cargoItem == null)
        throw new CargoItemNotFoundException(id);
      return cargoItem;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetCargoItems<TResult>(
        Expression<Func<CargoItem, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> ladingCode = null,
        TValue<string> cargoItemCode = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> cargoId = null,
        TValue<int[]> cargoIds = null,
        TValue<string> cargoCode = null,
        TValue<int> purchaseOrderId = null,
        TValue<string> purchaseOrderCode = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<int> providerId = null,
        TValue<DateTime> fromEstimateDateTime = null,
        TValue<DateTime> toEstimateDateTime = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<int> howToBuyId = null,
        TValue<CargoItemStatus> status = null,
        TValue<CargoItemStatus[]> statuses = null,
        TValue<CargoItemStatus[]> notHasStatuses = null,
        TValue<int> financialTransactionBatchIdForCargoCost = null,
        TValue<int> financialTransactionBatchIdForDeliveryOrder = null,
        TValue<int[]> cargoItemIds = null,
        TValue<bool> isArchived = null,
        TValue<int> forwarderId = null,
        TValue<int[]> forwarderIds = null,
        TValue<int> employeeId = null,
        TValue<int[]> employeeIds = null,
        TValue<ProviderType> providerType = null,
        TValue<int> financialDocumentId = null,
        TValue<int[]> selectedPlanCodeIds = null)

    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<CargoItem>();
      if (providerType != null)
        query = query.Where(i => i.PurchaseOrder.Provider.ProviderType == providerType);
      if (employeeId != null)
        query = query.Where(i => i.User.Employee.Id == employeeId);
      if (employeeIds != null)
        query = query.Where(i => employeeIds.Value.Contains(i.User.Employee.Id));
      if (cargoItemCode != null)
        query = query.Where(i => i.Code == cargoItemCode);
      if (cargoId != null)
        query = query.Where(i => i.CargoId == cargoId);
      if (cargoCode != null)
        query = query.Where(i => i.Cargo.Code == cargoCode);
      if (purchaseOrderId != null)
        query = query.Where(i => i.PurchaseOrderId == purchaseOrderId);
      if (purchaseOrderCode != null)
        query = query.Where(i => i.PurchaseOrder.Code == purchaseOrderCode);
      if (qty != null)
        query = query.Where(i => i.Qty == qty);
      if (unitId != null)
        query = query.Where(i => i.UnitId == unitId);
      if (ids != null)
        query = query.Where(i => ids.Value.Contains(i.Id));
      if (cargoIds != null)
        query = query.Where(i => cargoIds.Value.Contains(i.CargoId));
      if (status != null)
        query = query.Where(i => i.Status.HasFlag(status));
      if (howToBuyId != null)
        query = query.Where(i => i.HowToBuyId == howToBuyId);
      if (fromEstimateDateTime != null)
        query = query.Where(i => i.EstimateDateTime >= fromEstimateDateTime);
      if (toEstimateDateTime != null)
        query = query.Where(i => i.EstimateDateTime <= toEstimateDateTime);
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);
      if (cargoItemIds != null)
        query = query.Where(i => cargoItemIds.Value.Contains(i.Id));
      if (financialTransactionBatchIdForCargoCost != null)
        query = query.Where(i => i.CargoCosts.Any(cc =>
                  cc.FinancialDocumentCost.FinancialDocument.FinancialTransactionBatch.Id == financialTransactionBatchIdForCargoCost));
      if (financialTransactionBatchIdForDeliveryOrder != null)
        query = query.Where(i =>
                  i.FinancialTransactionBatch.Id == financialTransactionBatchIdForDeliveryOrder);
      if (isArchived != null)
        query = query.Where(i => i.IsArchived == isArchived);
      if (forwarderId != null)
        query = query.Where(i => i.ForwarderId == forwarderId);
      if (forwarderIds != null)
        query = query.Where(i => forwarderIds.Value.Contains(i.Forwarder.Id));
      if (employeeIds != null)
        query = query.Where(i => employeeIds.Value.Contains(i.User.Employee.Id));


      if (financialDocumentId != null)
        query = query.Where(i => i.CargoCosts.Select(d => d.FinancialDocumentCost.FinancialDocument.Id).Contains(financialDocumentId));
      if (providerId != null)
        query = query.Where(i => i.PurchaseOrder.ProviderId == providerId);

      if (ladingCode != null && ladingCode != "")
      {
        var cargoItemsIds = (from q in query
                             from ladingItem in q.LadingItems
                             where ((ladingItem.Lading.Code == ladingCode) && (ladingItem.IsDelete == false))
                             select q.Id).Distinct();
        query = from q in query
                join cargoItemId in cargoItemsIds
                      on q.Id equals cargoItemId
                select q;
      }

      if (statuses != null)
      {
        var s = CargoItemStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        query = query.Where(i => (i.Status & s) > 0);
      }

      if (notHasStatuses != null)
      {
        var s = CargoItemStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        query = query.Where(i => (i.Status & s) == 0);
      }
      if (selectedPlanCodeIds != null)
      {

        query = from item in query
                from cargoItemDetail in item.CargoItemDetails
                where selectedPlanCodeIds.Value.Contains(cargoItemDetail.PurchaseOrderDetail.PurchaseRequest.PlanCodeId.Value)
                select item;

      }

      return query.Select(selector);
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetCargoItemWithCurrentStepAndPurchaseDetails<TResult>(
        Expression<Func<CargoItemWithCurrenStepAndPurchaseDetails, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> ladingCode = null,
        TValue<string> cargoItemCode = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> cargoId = null,
        TValue<int> purchaseOrderId = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<int[]> ids = null,
        TValue<int[]> employeeIds = null,
        TValue<int[]> forwarderIds = null,
        TValue<DateTime> fromEstimateDateTime = null,
        TValue<DateTime> toEstimateDateTime = null,
        TValue<int> howToBuyId = null,
        TValue<int> financialTransactionBatchIdForCargoCost = null,
        TValue<int> financialTransactionBatchIdForDeliveryOrder = null,
        TValue<CargoItemStatus> status = null,
        TValue<CargoItemStatus[]> statuses = null,
        TValue<CargoItemStatus[]> notHasStatuses = null,
        TValue<bool> isArchived = null,
        TValue<int> forwarderId = null,
        TValue<int> employeeId = null,
        TValue<int[]> selectedPlanCodeIds = null,
        TValue<int> financialDocumentId = null,
        TValue<int> planCodeId = null)
    {

      var cargoItems = GetCargoItems(selector: e => e,
                    cargoId: cargoId,
                    code: code,
                    description: description,
                    id: id,
                    ids: ids,
                    ladingCode: ladingCode,
                    cargoItemCode: cargoItemCode,
                    isDelete: isDelete,
                    purchaseOrderId: purchaseOrderId,
                    qty: qty,
                    status: status,
                    statuses: statuses,
                    notHasStatuses: notHasStatuses,
                    transactionBatchId: transactionBatchId,
                    unitId: unitId,
                    userId: userId,
                    employeeId: employeeId,
                    employeeIds: employeeIds,
                    forwarderIds: forwarderIds,
                    fromEstimateDateTime: fromEstimateDateTime,
                    toEstimateDateTime: toEstimateDateTime,
                    howToBuyId: howToBuyId,
                    financialTransactionBatchIdForCargoCost: financialTransactionBatchIdForCargoCost,
                    financialTransactionBatchIdForDeliveryOrder: financialTransactionBatchIdForDeliveryOrder,
                    isArchived: isArchived,
                    forwarderId: forwarderId,
                    financialDocumentId: financialDocumentId,
                    selectedPlanCodeIds: selectedPlanCodeIds);

      var query = from cargoItem in cargoItems
                  let order = cargoItem.PurchaseOrder
                  from a in cargoItem.PurchaseSteps.Where(a => a.IsCurrentStep).DefaultIfEmpty()
                  select new CargoItemWithCurrenStepAndPurchaseDetails
                  {
                    CargoItem = cargoItem,
                    PurchaseOrder = order,
                    PurchaseStep = a
                  };

      return query.Select(selector);
    }
    #endregion
    #region ToCargoItemComboList
    public IQueryable<CargoItemComboResult> GetCargoItemsCombo(
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> cargoId = null,
        TValue<string> cargoCode = null,
        TValue<int> purchaseOrderId = null,
        TValue<string> purchaseOrderCode = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<int[]> ids = null,
        TValue<CargoItemStatus> status = null,
        TValue<CargoItemStatus[]> statuses = null)
    {


      return GetCargoItems(selector: ToCargoItemComboResult,
            id: id,
            code: code,
            isDelete: isDelete,
            userId: userId,
            transactionBatchId: transactionBatchId,
            description: description,
            cargoId: cargoId,
            cargoCode: cargoCode,
            purchaseOrderId: purchaseOrderId,
            purchaseOrderCode: purchaseOrderCode,
            qty: qty,
            unitId: unitId,
            ids: ids,
            status: status,
            statuses: statuses);
    }
    #endregion
    #region AddProcess
    public CargoItem AddCargoItemProcess(
        TransactionBatch transactionBatch,
        AddCargoItemDetailInput[] addCargoItemDetails,
        string description,
        TValue<int> cargoId = null,
        TValue<int> purchaseOrderId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<DateTime> estimateDateTime = null,
        TValue<DateTime> cargoItemDateTime = null,
        TValue<short> howToBuyId = null,
        TValue<int> currentPurchaseStepId = null,
        TValue<BuyingProcess> buyingProcess = null,
        TValue<int?> forwarderId = null,
        TValue<Guid?> forwarderDocumentId = null)
    {

      #region TransactionBatch
      transactionBatch = transactionBatch ?? App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region Get PurchaseOrder
      if (purchaseOrderId == null)
        throw new ArgumentNullException(nameof(purchaseOrderId));

      var purchaseOrder = GetPurchaseOrder(id: purchaseOrderId);
      #endregion
      #region FinancialTransactionBatch
      var financialTransactionBatch = App.Internals.Accounting.AddFinancialTransactionBatch();
      #endregion
      #region AddCargoItem
      var cargoItem = AddCargoItem(
              cargoItem: null,
              transactionBatch: transactionBatch,
              financialTransactionBatch: financialTransactionBatch,
              description: description,
              cargoId: cargoId,
              purchaseOrderId: purchaseOrderId,
              qty: Math.Round(qty, purchaseOrder.Unit.DecimalDigitCount),
              unitId: unitId,
              estimateDateTime: estimateDateTime,
              cargoItemDateTime: cargoItemDateTime,
              howToBuyId: howToBuyId,
              forwarderDocumentId: forwarderDocumentId,
              buyingProcess: buyingProcess,
              forwarderId: forwarderId);
      #endregion
      #region Add Financial Transaction
      #region FinancialTransacton

      var financialTransaction = App.Internals.Accounting.GetFinancialTransactions(
              selector: s => s,
              financialTransactionTypeId: Models.StaticData.StaticFinancialTransactionTypes.ImportToPurchaseOrder.Id,
              purchaseOrderItemId: purchaseOrderId,
              isDelete: false)


          .FirstOrDefault();

      if (financialTransaction == null)
        throw new PurchaseOrderHasNoFinancialTransactionException(purchaseOrderCode: purchaseOrder.Code);

      var cargoItemQty = ((Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount)) * cargoItem.Unit.ConversionRatio) / purchaseOrder.Unit.ConversionRatio;

      var exportFromPurchaseFinancialTransaction = App.Internals.Accounting.AddFinancialTransactionProcess(
                financialTransaction: null,
                amount: purchaseOrder.Price.Value * cargoItemQty,
                effectDateTime: cargoItemDateTime,
                description: null,
                financialAccountId: financialTransaction.FinancialAccountId,
                financialTransactionType: Models.StaticData.StaticFinancialTransactionTypes.ExportFromPurchase,
                financialTransactionBatchId: financialTransactionBatch.Id,
                financialTransactionIsPermanent: false,
                referenceFinancialTransaction: financialTransaction);

      App.Internals.Accounting.AddFinancialTransactionProcess(
                financialTransaction: null,
                amount: purchaseOrder.Price.Value * cargoItemQty,
                effectDateTime: cargoItemDateTime,
                description: null,
                financialAccountId: financialTransaction.FinancialAccountId,
                financialTransactionType: Models.StaticData.StaticFinancialTransactionTypes.ImportToCargo,
                financialTransactionBatchId: financialTransactionBatch.Id,
                financialTransactionIsPermanent: false,
                referenceFinancialTransaction: exportFromPurchaseFinancialTransaction);
      #endregion
      #endregion
      #region AddCargoItemSummary
      AddCargoItemSummary(
              receiptedQty: 0,
              ladingItemQty: 0,
              qualityControlPassedQty: 0,
              qualityControlFailedQty: 0,
              cargoItemId: cargoItem.Id);
      #endregion
      #region Get PurchaseOrderDetails
      var purchaseOrderDetails = GetPurchaseOrderDetails(
          selector: e => e,
          isDelete: false,
          purchaseOrderId: purchaseOrderId);
      #endregion
      #region CargoItemDetail
      var rQty = Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount) * cargoItem.Unit.ConversionRatio;

      foreach (var addCargoItemDetail in addCargoItemDetails)
      {
        var purchaseOrderDetail = GetPurchaseOrderDetail(id: addCargoItemDetail.PurchaseOrderDetailId);

        #region Get purchaseOrderTransaction
        var purchaseOrderDetailTransaction = App.Internals.WarehouseManagement.GetTransactionPlans(
                selector: e => e,
                transactionBatchId: purchaseOrderDetail.TransactionBatch.Id,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportPurchaseOrder.Id)


            .Single();
        #endregion

        // var cargoItemUnit = App.Internals.ApplicationBase.GetUnit(addCargoItemDetail.UnitId)
        //    
        //;

        // var detailQty = (addCargoItemDetail.Value * cargoItemUnit.ConversionRatio) / cargoItem.Unit.ConversionRatio;
        #region Add CargoItemDetail
        AddCargoItemDetailProcess(
                purchaseOrderDetailId: purchaseOrderDetail.Id,
                cargoItemId: cargoItem.Id,
                stuffId: purchaseOrder.StuffId,
                qty: Math.Round(addCargoItemDetail.Value, purchaseOrderDetail.Unit.DecimalDigitCount),
                unitId: addCargoItemDetail.UnitId,
                purchaseOrderTransaction: purchaseOrderDetailTransaction,
                estimateDateTime: cargoItem.EstimateDateTime
            );
        #endregion

      }
      //foreach (var purchaseOrderDetail in purchaseOrderDetails)
      //{

      //    #region Get purchaseOrderTransaction
      //    var purchaseOrderDetailTransaction = App.Internals.WarehouseManagement.GetTransactionPlans(
      //            selector: e => e,
      //            transactionBatchId: purchaseOrderDetail.TransactionBatch.Id,
      //            transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportPurchaseOrder.Id)
      //        
      //        
      //        .Single();
      //    #endregion

      //    #region Calculate TransactionQty
      //    var purchaseOrderDetailQty = purchaseOrderDetail.Qty * purchaseOrderDetail.Unit.ConversionRatio;
      //    var cargoItemDetails = GetCargoItemDetails(
      //                selector: e => e,
      //                purchaseOrderDetailId: purchaseOrderDetail.Id,
      //                isDelete: false)
      //            
      //;

      //    var cargoedQty = cargoItemDetails.Any() ?
      //            cargoItemDetails.Sum(i => i.Qty * i.Unit.ConversionRatio) : 0;
      //    var rPurchaseOrderDetailQty = purchaseOrderDetailQty - cargoedQty;
      //    var transactionQty = Math.Min(rQty, rPurchaseOrderDetailQty);
      //    #endregion
      //    if (transactionQty > 0)
      //    {
      //        var detailQty = transactionQty / cargoItem.Unit.ConversionRatio;
      //        #region Add CargoItemDetail
      //        AddCargoItemDetailProcess(
      //                purchaseOrderDetailId: purchaseOrderDetail.Id,
      //                cargoItemId: cargoItem.Id,
      //                stuffId: purchaseOrder.StuffId,
      //                qty: detailQty,
      //                unitId: cargoItem.UnitId,
      //                purchaseOrderTransaction: purchaseOrderDetailTransaction,
      //                estimateDateTime: cargoItem.EstimateDateTime
      //            )
      //;
      //        #endregion

      //    }
      //    rQty = rQty - transactionQty;
      //}
      //#region Add Extra Qty CargoItemDetail
      //if (rQty > 0)
      //{
      //    var detailQty = rQty / cargoItem.Unit.ConversionRatio;
      //    var id = purchaseOrderDetails.ToList().LastOrDefault()?.Id;

      //    var purchaseOrderDetailTransaction = App.Internals.WarehouseManagement.GetTransactionPlans(
      //            selector: e => e,
      //            transactionBatchId: purchaseOrderDetails.ToList().LastOrDefault()?.TransactionBatch.Id,
      //            transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportPurchaseOrder.Id)
      //        
      //        
      //        .Single();

      //    #region Add CargoItemDetail
      //    AddCargoItemDetailProcess(
      //            purchaseOrderDetailId: id,
      //            cargoItemId: cargoItem.Id,
      //            stuffId: purchaseOrder.StuffId,
      //            qty: detailQty,
      //            unitId: cargoItem.UnitId,
      //            purchaseOrderTransaction: purchaseOrderDetailTransaction,
      //            estimateDateTime: cargoItem.EstimateDateTime)
      //        
      //;
      //    #endregion
      //}
      //#endregion
      #endregion
      #region ResetPurchaseOrderStatus
      ResetPurchaseOrderStatus(purchaseOrder: purchaseOrder);
      #endregion
      return cargoItem;
    }
    #endregion
    #region EditProcess
    public CargoItem EditCargoItemProcess(
        TransactionBatch transactionBatch,
        CargoItem cargoItem,
        byte[] rowVersion,
        string description,
        double qty,
        byte unitId,
        DateTime estimateDateTime,
        DateTime cargoItemDateTime,
        short howToBuyId)
    {

      #region AddTransactionBatch
      transactionBatch = transactionBatch ?? App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region RemoveCargoItemProcess
      RemoveCargoItemProcess(
              id: cargoItem.Id,
              rowVersion: rowVersion,
              transactionBatch: transactionBatch);
      #endregion
      #region AddCargoItemProcess
      var newCargoItem = AddCargoItemProcess(
              transactionBatch: transactionBatch,
              addCargoItemDetails: null,
              description: description,
              cargoId: cargoItem.CargoId,
              purchaseOrderId: cargoItem.PurchaseOrderId,
              qty: qty,
              unitId: unitId,
              forwarderDocumentId: null,
              estimateDateTime: estimateDateTime,
              cargoItemDateTime: cargoItemDateTime,
              howToBuyId: howToBuyId);
      #endregion
      return newCargoItem;
    }

    public CargoItem EditCargoItemProcess(
        TransactionBatch transactionBatch,
        int id,
        byte[] rowVersion,
        string description,
        double qty,
        byte unitId,
        DateTime estimateDateTime,
        DateTime cargoItemDateTime,
        short howToBuyId)
    {

      var cargoItem = GetCargoItem(id: id);
      return EditCargoItemProcess(
                    transactionBatch: transactionBatch,
                    cargoItem: cargoItem,
                    rowVersion: rowVersion,
                    description: description,
                    qty: qty,
                    unitId: unitId,
                    estimateDateTime: estimateDateTime,
                    cargoItemDateTime: cargoItemDateTime,
                    howToBuyId: howToBuyId);
    }


    #endregion
    #region RemoveProcess
    public void RemoveCargoItemProcess(
        int id,
        byte[] rowVersion,
        TransactionBatch transactionBatch)
    {

      #region GetCargo
      var cargoItem = GetCargoItem(id: id);
      #endregion

      var ladingItems = GetLadingItems(e => e, cargoItemId: cargoItem.Id, isDelete: false);

      #region check if auto lading remove ladingItem
      var ladings = GetLadings(
          selector: e => e,
          cargoItemCode: cargoItem.Code,
          isDelete: false
          );

      var ladingItemDetails = GetLadingItemDetails(
                selector: e => e,
                cargoItemId: cargoItem.Id,
                isDelete: false);

      if (cargoItem.PurchaseOrder.Provider.ProviderType == ProviderType.Internal)
      {
        foreach (var ladingItem in ladingItems)
        {
          RemoveLadingItemProcess(
                    id: ladingItem.Id,
                    rowVersion: ladingItem.RowVersion);
        }

        foreach (var lading in ladings)
        {
          RemoveAutoLadingProcess(
                    id: lading.Id,
                    rowVersion: lading.RowVersion);
        }
      }
      #endregion

      if ((cargoItem.Status & CargoItemStatus.Receipted) > 0 || ladingItems.Any())
      {
        throw new CargoItemCanNotDeleteException(id: cargoItem.Id);
      }

      var accounting = App.Internals.Accounting;

      #region Remove Financial Transaction
      var financialTrnasactionBatch = accounting.GetFinancialTransactionBatches(
          selector: e => e,
          baseEntityId: cargoItem.Id)


      .FirstOrDefault();

      foreach (var financialTransaction in financialTrnasactionBatch.FinancialTransactions)
      {
        accounting.DeleteFinancialTransaction(financialTransaction);
      }


      // var financialTransaction = App.Internals.Accounting.GetFinancialTransactions(selector: s => s,
      //     financialTransactionTypeId: Models.StaticData.StaticFinancialTransactionTypes.ImportToCargo.Id,
      //     baseEntityId: cargoItem.PurchaseOrderId)
      //.FirstOrDefault();

      // if (financialTransaction != null)
      //     App.Internals.Accounting.EditFinancialTransactionProcess(financialTransaction: financialTransaction,
      //         rowVersion: financialTransaction.RowVersion,
      //         isDelete: true);
      #endregion

      #region RemoveCargoItem
      RemoveCargoItem(
              id: id,
              rowVersion: rowVersion);
      //var cargoItem = GetCargoItem(id: id);
      #endregion

      #region Remove CargoItemDetails
      var cargoItemDetails = GetCargoItemDetails(selector: e => e,
              cargoItemId: id);
      foreach (var cargoItemDetail in cargoItemDetails)
      {
        RemoveCargoItemDetail(
                      transactionBatch: cargoItem.TransactionBatch,
                      id: cargoItemDetail.Id,
                      rowVersion: cargoItemDetail.RowVersion);

        #region ResetPurchaseOrderDetailStatus

        App.Internals.Supplies.ResetPurchaseOrderDetailStatus(purchaseOrderDetailId: (int)cargoItemDetail.PurchaseOrderDetailId);

        #endregion
      }
      #endregion
      #region Get PurchaseOrder
      var purchaseOrder = GetPurchaseOrder(id: cargoItem.PurchaseOrderId);
      #endregion

      #region ResetPurchaseOrderStatus

      ResetPurchaseOrderStatus(purchaseOrder: purchaseOrder);
      #endregion



      #region cargoItemLog

      var suppliesModule = App.Internals.Supplies;

      var getCargoItemDetails = suppliesModule.GetCargoItemDetails(
                e => new { e.CargoItemId, e.Qty }
                , cargoItemId: id)


            .FirstOrDefault();

      suppliesModule.AddCargoItemLog(
                cargoItemId: id,
                cargoItemCode: cargoItem.Code,
                isDelete: true,
                newcargoItemQty: cargoItem.Qty,
                oldCargoItemQty: getCargoItemDetails.Qty,
                cargoItemLogStatus: CargoItemLogStatus.Deleted);
      #endregion
    }
    #endregion

    #region SearchComboResult
    public IQueryable<CargoItemComboResult> SearchComboResult(
        IQueryable<CargoItemComboResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where item.Code.Contains(searchText) ||
                      item.CargoCode.Contains(searchText) ||
                      item.StuffCode.Contains(searchText) ||
                      item.StuffName.Contains(searchText) ||
                      item.ProviderName.Contains(searchText)
                select item;

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);


      return query;
    }
    #endregion


    #region
    public void RemoveAutoLadingProcess(
        int id,
        byte[] rowVersion)
    {

      var ladingItems = GetLadingItems(
                selector: e => e,
                ladingId: id,
                isDelete: false);

      if (!ladingItems.Any())
      {
        RemoveLadingProcess(
                  id: id,
                  rowVersion: rowVersion,
                  transactionBatch: null);
      }
    }
    #endregion
    //#region Add
    //public CargoItem AddCargoItemLadingCode(
    //   int cargoId,
    //   string description,
    //   int PurchaseOrderId,
    //   string value,
    //   byte UnitId)
    //{
    //    (repository) =>
    //    {
    //        #region Add AddCargoItemLadingCode

    //        var order = AddOrder(customerId: customerId,
    //                orderTypeId: orderTypeId,
    //                orderer: orderer,
    //                description: description,
    //                documentNumber: documentNumber,
    //                documentType: documentType)
    //            
    //;
    //        #endregion

    //        return order;
    //    });
    //}
    //#endregion
    #region ToFullCargoItemResult
    public Expression<Func<CargoItemWithCurrenStepAndPurchaseDetails, FullCargoItemResult>> ToFullCargoItemResult =
        (cargoItemResult) => new FullCargoItemResult
        {
          Id = cargoItemResult.CargoItem.Id,
          RowVersion = cargoItemResult.CargoItem.RowVersion,
          CargoId = cargoItemResult.CargoItem.Cargo.Id,
          CargoCode = cargoItemResult.CargoItem.Cargo.Code,
          HowToBuyId = cargoItemResult.CargoItem.HowToBuy.Id,
          HowToBuyTitle = cargoItemResult.CargoItem.HowToBuy.Title,
          ProviderId = cargoItemResult.PurchaseOrder.Provider.Id,
          ProviderName = cargoItemResult.PurchaseOrder.Provider.Name,
          HowToBuyDetailId = cargoItemResult.PurchaseStep.HowToBuyDetail.Id,
          HowToBuyDetailTitle = cargoItemResult.PurchaseStep.HowToBuyDetail.Title,
          PurchaseOrderDeadline = cargoItemResult.PurchaseOrder.Deadline,
          PurchaseOrderId = cargoItemResult.PurchaseOrder.Id,
          PurchaseOrderType = cargoItemResult.PurchaseOrder.PurchaseOrderType,
          StuffId = cargoItemResult.PurchaseOrder.StuffId,
          StuffCode = cargoItemResult.PurchaseOrder.Stuff.Code,
          StuffName = cargoItemResult.PurchaseOrder.Stuff.Name,
          StuffNetWeight = cargoItemResult.PurchaseOrder.Stuff.NetWeight,
          StuffGrossWeight = cargoItemResult.PurchaseOrder.Stuff.GrossWeight,
          PurchaseOrderPrice = cargoItemResult.PurchaseOrder.Price,
          PurchaseOrderCurrencyId = cargoItemResult.PurchaseOrder.CurrencyId,
          PurchaseOrderCurrencyTitle = cargoItemResult.PurchaseOrder.Currency.Title,
          Qty = Math.Round(cargoItemResult.CargoItem.Qty, cargoItemResult.CargoItem.Unit.DecimalDigitCount),
          UnitId = cargoItemResult.CargoItem.UnitId,
          UnitName = cargoItemResult.CargoItem.Unit.Name,
          PurchaseOrderCode = cargoItemResult.PurchaseOrder.Code,
          Status = cargoItemResult.CargoItem.Status,
          QualityControlPassedQty = cargoItemResult.CargoItem.CargoItemSummary.QualityControlPassedQty,
          QualityControlFailedQty = cargoItemResult.CargoItem.CargoItemSummary.QualityControlFailedQty,
          ReceiptedQty = cargoItemResult.CargoItem.CargoItemSummary.ReceiptedQty,
          EstimateDateTime = cargoItemResult.CargoItem.EstimateDateTime,
          //LadingCode = cargoItemresult.CargoItem.Lading.Code,
          EmployeeFullName = cargoItemResult.CargoItem.User.Employee.FirstName + " " + cargoItemResult.CargoItem.User.Employee.LastName,
          RemainedCargoItemQty = Math.Round(cargoItemResult.CargoItem.Qty, cargoItemResult.CargoItem.Unit.DecimalDigitCount) - Math.Round(cargoItemResult.CargoItem.CargoItemSummary.ReceiptedQty, cargoItemResult.CargoItem.Unit.DecimalDigitCount),

        };
    #endregion

    #region ToFullCargoItemResultQuery
    public IQueryable<FullCargoItemResult> ToFullCargoItemResultQuery(
        IQueryable<CargoItemWithCurrenStepAndPurchaseDetails> fullCargoItems,
        IQueryable<BaseEntityDocument> latestBaseEntityDocuments)
    {

      var query = from fullCargoItem in fullCargoItems
                  from ladingItem in fullCargoItem.CargoItem.LadingItems.Where(m => m.IsDelete == false)
                    //from cargoItemDetail in fullCargoItem.CargoItem.CargoItemDetails
                  let lading = ladingItem.Lading
                  where (lading.IsDelete == false)
                  select new { ladingItem.CargoItemId, lading.Code, LadingDateTime = lading.DateTime, ladingItem.Qty, ladingItem.CargoItem };

      var cargoItemDetails = App.Internals.Supplies.GetCargoItemDetails(e => e);

      var purchaseOrderPlanCodes = App.Internals.Supplies.GetPurchaseOrderPlanCodes();


      var groupResult = from q in query
                        group q by q.CargoItemId
                into temp
                        select new
                        {
                          CargoItemId = temp.Key,
                          Ladings = temp.Select(m => new LadingsWithLadinItem { Code = m.Code, Qty = Math.Round(m.Qty, m.CargoItem.Unit.DecimalDigitCount), DateTime = m.LadingDateTime }).AsQueryable()
                        };


      var result = from fullCargoItem in fullCargoItems
                   join g in groupResult
                             on fullCargoItem.CargoItem.Id equals g.CargoItemId
                             into tempLadings
                   from ladingsWithLadinItem in tempLadings.DefaultIfEmpty()


                   join latestBaseEntityDocument in latestBaseEntityDocuments
                             on fullCargoItem.CargoItem.Id equals latestBaseEntityDocument.BaseEntityId
                             into tLatestBaseEntityDocuments
                   from latestBaseEntityDocument in tLatestBaseEntityDocuments.DefaultIfEmpty()
                   join purchaseOrderPlanCode in purchaseOrderPlanCodes on
                             fullCargoItem.PurchaseOrder.Id equals purchaseOrderPlanCode.PurchaseOrderId into tPurchaseOrderWithPlanCode
                   from purchaseOrderWithPlanCode in tPurchaseOrderWithPlanCode.DefaultIfEmpty()

                   select new FullCargoItemResult
                   {
                     Id = fullCargoItem.CargoItem.Id,
                     RowVersion = fullCargoItem.CargoItem.RowVersion,
                     CargoId = fullCargoItem.CargoItem.Cargo.Id,
                     CargoCode = fullCargoItem.CargoItem.Cargo.Code,
                     HowToBuyId = fullCargoItem.CargoItem.HowToBuy.Id,
                     HowToBuyTitle = fullCargoItem.CargoItem.HowToBuy.Title,
                     ProviderId = fullCargoItem.PurchaseOrder.Provider.Id,
                     ProviderName = fullCargoItem.PurchaseOrder.Provider.Name,
                     CurrentPurchaseStepId = (int?)fullCargoItem.PurchaseStep.Id,
                     CurrentPurchaseStepUserId = (int?)fullCargoItem.PurchaseStep.UserId,
                     CurrentPurchaseStepDateTime = (DateTime?)fullCargoItem.PurchaseStep.DateTime,
                     CurrentPurchaseStepFollowUpDateTime = (DateTime?)fullCargoItem.PurchaseStep.FollowUpDateTime,
                     CurrentPurchaseStepEmployeeFullName =
                                 fullCargoItem.PurchaseStep.User.Employee.FirstName + " " +
                                 fullCargoItem.PurchaseStep.User.Employee.LastName,
                     HowToBuyDetailId = fullCargoItem.PurchaseStep.HowToBuyDetail.Id,
                     HowToBuyDetailTitle = fullCargoItem.PurchaseStep.HowToBuyDetail.Title,
                     PurchaseOrderDeadline = fullCargoItem.PurchaseOrder.Deadline,
                     PurchaseOrderId = fullCargoItem.PurchaseOrder.Id,
                     PurchaseOrderType = fullCargoItem.PurchaseOrder.PurchaseOrderType,
                     StuffId = fullCargoItem.PurchaseOrder.StuffId,
                     StuffCode = fullCargoItem.PurchaseOrder.Stuff.Code,
                     StuffName = fullCargoItem.PurchaseOrder.Stuff.Name,
                     StuffType = fullCargoItem.PurchaseOrder.Stuff.StuffType,
                     StuffNetWeight = fullCargoItem.PurchaseOrder.Stuff.NetWeight,
                     StuffGrossWeight = fullCargoItem.PurchaseOrder.Stuff.GrossWeight,
                     PurchaseOrderPrice = fullCargoItem.PurchaseOrder.Price,
                     PurchaseOrderCurrencyId = fullCargoItem.PurchaseOrder.CurrencyId,
                     PurchaseOrderCurrencyTitle = fullCargoItem.PurchaseOrder.Currency.Title,
                     Qty = Math.Round(fullCargoItem.CargoItem.Qty, fullCargoItem.CargoItem.Unit.DecimalDigitCount),
                     UnitId = fullCargoItem.CargoItem.UnitId,
                     UnitName = fullCargoItem.CargoItem.Unit.Name,
                     PurchaseOrderCode = fullCargoItem.PurchaseOrder.Code,
                     Status = fullCargoItem.CargoItem.Status,
                     QualityControlPassedQty = fullCargoItem.CargoItem.CargoItemSummary.QualityControlPassedQty,
                     QualityControlFailedQty = fullCargoItem.CargoItem.CargoItemSummary.QualityControlFailedQty,
                     ReceiptedQty = fullCargoItem.CargoItem.CargoItemSummary.ReceiptedQty,
                     EstimateDateTime = fullCargoItem.CargoItem.EstimateDateTime,
                     CargoItemDateTime = fullCargoItem.CargoItem.CargoItemDateTime,
                     CargoItemId = fullCargoItem.CargoItem.Id,
                     CargoItemCode = fullCargoItem.CargoItem.Code,
                     EmployeeFullName = fullCargoItem.CargoItem.User.Employee.FirstName + " " +
                                                fullCargoItem.CargoItem.User.Employee.LastName,
                     RemainedCargoItemQty = Math.Round(fullCargoItem.CargoItem.Qty, fullCargoItem.CargoItem.Unit.DecimalDigitCount) -
                                                    Math.Round(fullCargoItem.CargoItem.CargoItemSummary.ReceiptedQty, fullCargoItem.CargoItem.Unit.DecimalDigitCount),
                     LatestBaseEntityDocumentDescription = latestBaseEntityDocument.Description,
                     Ladings = ladingsWithLadinItem.Ladings,
                     IsArchived = fullCargoItem.CargoItem.IsArchived,
                     ForwarderId = fullCargoItem.CargoItem.ForwarderId,
                     ForwarderName = fullCargoItem.CargoItem.Forwarder.Name,
                     BuyingProcess = fullCargoItem.CargoItem.BuyingProcess,
                     PlanCode = purchaseOrderWithPlanCode.PlanCodes,
                     RiskLevelStatus = fullCargoItem.CargoItem.LatestRisk == null ? RiskLevelStatus.Low : fullCargoItem.CargoItem.LatestRisk.LatestRiskStatus.RiskParameter.RiskLevelStatus,
                     LatestRiskTitle = fullCargoItem.CargoItem.LatestRisk.Title,
                     LatestRiskCreateDateTime = fullCargoItem.CargoItem.LatestRisk.CreateDateTime,
                     LatestBaseEntityDocumentDateTime = latestBaseEntityDocument.DateTime,
                     ForwarderDocumentId = fullCargoItem.CargoItem.ForwarderDocumentId
                   };

      return result;
    }
    #endregion

    #region ToResult
    public IQueryable<CargoItemResult> ToCargoItemResultQuery(IQueryable<CargoItem> cargoItems)
    {

      var query = from cargoItem in cargoItems
                  from ladingItem in cargoItem.LadingItems
                  let lading = ladingItem.Lading
                  select new { ladingItem.CargoItemId, lading.Code, ladingItem.Qty, ladingItem.CargoItem };

      var groupResult = from q in query
                        group q by q.CargoItemId into temp
                        select new
                        {
                          CargoItemId = temp.Key,
                          Ladings = temp.Select(m => new CargoItemsWithLadinItem { Code = m.Code, Qty = Math.Round(m.Qty, m.CargoItem.Unit.DecimalDigitCount) }).AsQueryable()
                        };

      var result = from cargoItem in cargoItems
                   join g in groupResult
                   on cargoItem.Id equals g.CargoItemId
                   into tempLadings
                   from cargoItemsWithLadinItem in tempLadings.DefaultIfEmpty()
                   select new CargoItemResult
                   {
                     Id = cargoItem.Id,
                     Code = cargoItem.Code,
                     DateTime = cargoItem.DateTime,
                     Qty = Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount),
                     ReceiptedQty = (double?)cargoItem.CargoItemSummary.ReceiptedQty,
                     RemainedQty = Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount) - ((double?)cargoItem.CargoItemSummary.ReceiptedQty ?? 0),
                     UnitId = cargoItem.UnitId,
                     UnitName = cargoItem.Unit.Name,
                     PurchaseOrderId = cargoItem.PurchaseOrderId,
                     PurchaseOrderCode = cargoItem.PurchaseOrder.Code,
                     CargoId = cargoItem.CargoId,
                     CargoCode = cargoItem.Cargo.Code,
                     HowToBuyId = cargoItem.HowToBuyId,
                     HowToBuyTitle = cargoItem.HowToBuy.Title,
                     PurchaseOrderDateTime = cargoItem.PurchaseOrder.DateTime,
                     ProviderId = cargoItem.PurchaseOrder.ProviderId,
                     ProviderName = cargoItem.PurchaseOrder.StuffProvider.Provider.Name,
                     ProviderCode = cargoItem.PurchaseOrder.StuffProvider.Provider.Code,
                     StuffId = cargoItem.PurchaseOrder.StuffId,
                     StuffCode = cargoItem.PurchaseOrder.StuffProvider.Stuff.Code,
                     StuffName = cargoItem.PurchaseOrder.StuffProvider.Stuff.Name,
                     PurchaseOrderQty = cargoItem.PurchaseOrder.Qty,
                     PurchaseOrderUnitId = cargoItem.PurchaseOrder.UnitId,
                     PurchaseOrderUnitName = cargoItem.PurchaseOrder.Unit.Name,
                     PurchaseOrderDeadline = cargoItem.PurchaseOrder.Deadline,
                     PurchaseOrderDescription = cargoItem.PurchaseOrder.Description,
                     PurchaseOrderCargoedQty = (double?)cargoItem.PurchaseOrder.PurchaseOrderSummary.CargoedQty,
                     PurchaseOrderRemainedQty = cargoItem.PurchaseOrder.Qty - ((double?)cargoItem.PurchaseOrder.PurchaseOrderSummary.CargoedQty ?? 0),
                     PurchaseOrderStatus = cargoItem.PurchaseOrder.Status,
                     IsDelete = cargoItem.IsDelete,
                     EmployeeFullName = cargoItem.User.Employee.FirstName + " " + cargoItem.User.Employee.LastName,
                     RowVersion = cargoItem.RowVersion,
                     Status = cargoItem.Status,
                     QualityControlPassedQty = cargoItem.CargoItemSummary.QualityControlPassedQty,
                     QualityControlFailedQty = cargoItem.CargoItemSummary.QualityControlFailedQty,
                     Ladings = cargoItemsWithLadinItem.Ladings,
                     LadingItemQty = cargoItem.CargoItemSummary.LadingItemQty
                   };

      return result;
    }

    public Expression<Func<CargoItem, CargoItemResult>> ToCargoItemResult =
            (cargoItem) => new CargoItemResult
            {
              Id = cargoItem.Id,
              Code = cargoItem.Code,
              DateTime = cargoItem.DateTime,
              Qty = Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount),
              ReceiptedQty = (double?)cargoItem.CargoItemSummary.ReceiptedQty,
              RemainedQty = Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount) - ((double?)cargoItem.CargoItemSummary.ReceiptedQty ?? 0),
              UnitId = cargoItem.UnitId,
              UnitName = cargoItem.Unit.Name,
              PurchaseOrderId = cargoItem.PurchaseOrderId,
              PurchaseOrderCode = cargoItem.PurchaseOrder.Code,
              CargoId = cargoItem.CargoId,
              CargoCode = cargoItem.Cargo.Code,
              PurchaseOrderDateTime = cargoItem.PurchaseOrder.DateTime,
              ProviderId = cargoItem.PurchaseOrder.ProviderId,
              ProviderName = cargoItem.PurchaseOrder.StuffProvider.Provider.Name,
              ProviderCode = cargoItem.PurchaseOrder.StuffProvider.Provider.Code,
              StuffId = cargoItem.PurchaseOrder.StuffId,
              StuffCode = cargoItem.PurchaseOrder.StuffProvider.Stuff.Code,
              StuffName = cargoItem.PurchaseOrder.StuffProvider.Stuff.Name,
              PurchaseOrderQty = cargoItem.PurchaseOrder.Qty,
              PurchaseOrderUnitId = cargoItem.PurchaseOrder.UnitId,
              PurchaseOrderUnitName = cargoItem.PurchaseOrder.Unit.Name,
              PurchaseOrderDeadline = cargoItem.PurchaseOrder.Deadline,
              PurchaseOrderDescription = cargoItem.PurchaseOrder.Description,
              PurchaseOrderCargoedQty = (double?)cargoItem.PurchaseOrder.PurchaseOrderSummary.CargoedQty,
              PurchaseOrderRemainedQty = cargoItem.PurchaseOrder.Qty - ((double?)cargoItem.PurchaseOrder.PurchaseOrderSummary.CargoedQty ?? 0),
              PurchaseOrderStatus = cargoItem.PurchaseOrder.Status,
              IsDelete = cargoItem.IsDelete,
              EmployeeFullName = cargoItem.User.Employee.FirstName + " " + cargoItem.User.Employee.LastName,
              EmployeeId = cargoItem.User.Employee.Id,
              RowVersion = cargoItem.RowVersion,
              Status = cargoItem.Status,
              QualityControlFailedQty = cargoItem.CargoItemSummary.QualityControlFailedQty,
              QualityControlPassedQty = cargoItem.CargoItemSummary.QualityControlPassedQty
            };
    public Expression<Func<CargoItem, CargoItemComboResult>> ToCargoItemComboResult =
       (cargoItem) => new CargoItemComboResult
       {
         Id = cargoItem.Id,
         Code = cargoItem.Code,
         CargoCode = cargoItem.Cargo.Code,
         CargoId = cargoItem.Cargo.Id,
         ProviderCode = cargoItem.PurchaseOrder.Provider.Code,
         ProviderId = cargoItem.PurchaseOrder.ProviderId,
         ProviderName = cargoItem.PurchaseOrder.Provider.Name,
         RowVersion = cargoItem.RowVersion,
         StuffCode = cargoItem.PurchaseOrder.Stuff.Code,
         StuffId = cargoItem.PurchaseOrder.StuffId,
         StuffName = cargoItem.PurchaseOrder.Stuff.Name,
       };
    #endregion

    #region Search
    public IQueryable<CargoItemResult> SearchCargoItemResults(
        IQueryable<CargoItemResult> query,
        string searchText,
        int? stuffId,
        int? cooperatorId)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = query.Where(i =>
            i.Code.Contains(searchText) ||
            i.PurchaseOrderCode.Contains(searchText) ||
            i.StuffName.Contains(searchText));


      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (cooperatorId != null)
        query = query.Where(i => i.ProviderId == cooperatorId);
      return query;
    }
    #endregion
    #region SearchFullCargoItemResults
    public IQueryable<FullCargoItemResult> SearchFullCargoItemResults(
        IQueryable<FullCargoItemResult> query,
        IQueryable<CargoItem> cargoItems,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText,
        string cargoCode,
        string purchaseOrderCode,
        int? stuffId,
        StuffType? stuffType,
        int? howTobuyId,
        int? howTobuyDetailId,
        int? providerId,
        int? forwarderId,
        int? planCodeId,
        DateTime? fromDeadLine,
        DateTime? toDateTime,
        DateTime? fromEstimateDate,
        DateTime? toEstimateDate,
        DateTime? fromRegistrationDate,
        DateTime? toRegistrationDate,
        TValue<PurchaseOrderType[]> purchaseOrderType,
        bool? hasLading,
        RiskLevelStatus? riskLevelStatus)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = query.Where(i =>
            i.UnitName.Contains(searchText) ||
            i.LatestRiskTitle.Contains(searchText) ||
            i.CargoCode.Contains(searchText) ||
            i.CargoItemCode.Contains(searchText) ||
            i.PurchaseOrderCode.Contains(searchText) ||
            i.StuffCode.Contains(searchText) ||
            i.ProviderName.Contains(searchText) ||
            i.ForwarderName.Contains(searchText) ||
            i.HowToBuyTitle.Contains(searchText) ||
            i.HowToBuyDetailTitle.Contains(searchText) ||
            i.StuffName.Contains(searchText));

      if (cargoCode != null)
        query = query.Where(i => i.CargoCode == cargoCode);
      if (purchaseOrderCode != null)
        query = query.Where(i => i.PurchaseOrderCode == purchaseOrderCode);
      if (howTobuyId != null)
        query = query.Where(i => i.HowToBuyId == howTobuyId);
      if (howTobuyDetailId != null)
        query = query.Where(i => i.HowToBuyDetailId == howTobuyDetailId);
      if (providerId != null)
        query = query.Where(i => i.ProviderId == providerId);
      if (forwarderId != null)
        query = query.Where(i => i.ForwarderId == forwarderId);
      if (fromDeadLine != null)
        query = query.Where(i => i.PurchaseOrderDeadline >= fromDeadLine);
      if (toDateTime != null)
        query = query.Where(i => i.PurchaseOrderDeadline <= toDateTime);
      if (fromRegistrationDate != null)
        query = query.Where(i => i.CargoItemDateTime >= fromRegistrationDate);
      if (toRegistrationDate != null)
        query = query.Where(i => i.CargoItemDateTime <= toRegistrationDate);
      if (fromEstimateDate != null)
        query = query.Where(i => i.EstimateDateTime >= fromEstimateDate);
      if (toEstimateDate != null)
        query = query.Where(i => i.EstimateDateTime <= toEstimateDate);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (stuffType != null)
        query = query.Where(i => i.StuffType == stuffType);
      if (riskLevelStatus != null)
        query = query.Where(i => i.RiskLevelStatus == riskLevelStatus);
      if (purchaseOrderType != null)
      {
        query = query.Where(i => purchaseOrderType.Value.Contains(i.PurchaseOrderType));
      }

      if (hasLading != null)
      {
        query = query.Where(i => i.Ladings.Any() == hasLading);
      }
      if (planCodeId != null)
      {
        var queryResult = from cargoItem in cargoItems
                          from cargoItemDetail in cargoItem.CargoItemDetails
                          where cargoItemDetail.PurchaseOrderDetail.PurchaseRequest.PlanCodeId == planCodeId
                          select cargoItem;
        query = from q in query
                join qr in queryResult
                on q.CargoItemId equals qr.Id
                select q;
      }
      var hasPlanCode = advanceSearchItems.Where(m => m.FieldName == "PlanCode" || m.FieldName == "LadingDescription");
      if (hasPlanCode.Any())
      {
        advanceSearchItems.ForEach(r =>
        {
          if (r.FieldName == "PlanCode")
          {
            if (r.Value != null)
            {
              var queryResult = from cargoItem in cargoItems
                                from cargoItemDetail in cargoItem.CargoItemDetails
                                where cargoItemDetail.PurchaseOrderDetail.PurchaseRequest.PlanCode.Code.Contains(r.Value.ToString())
                                select cargoItem;
              query = from q in query
                      join qr in queryResult
                              on q.CargoItemId equals qr.Id
                      select q;


            }
          }

          if (r.FieldName == "LadingDescription")
          {
            if (r.Value != null)
            {
              var queryResult = from cargoItem in cargoItems
                                from cargoItemDetail in cargoItem.CargoItemDetails
                                from ladingItem in cargoItem.LadingItems
                                where ladingItem.Lading.Code.Contains(r.Value.ToString())
                                select cargoItem;

              query = from q in query
                      join qr in queryResult
                              on q.CargoItemId equals qr.Id
                      select q;


            }
          }
        });
        advanceSearchItems = advanceSearchItems.Where(m => m.FieldName != "PlanCode" && m.FieldName != "LadingDescription").ToArray();
      }

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);

      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<FullCargoItemResult> SortFullCargoItemResult(IQueryable<FullCargoItemResult> query, SortInput<FullCargoItemSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case FullCargoItemSortType.CargoCode:
          return query.OrderBy(i => i.CargoCode, sortInput.SortOrder);
        case FullCargoItemSortType.CargoItemStatus:
          return query.OrderBy(i => i.Status, sortInput.SortOrder);
        case FullCargoItemSortType.HowToBuyDetailTitle:
          return query.OrderBy(i => i.HowToBuyDetailTitle, sortInput.SortOrder);
        case FullCargoItemSortType.HowToBuyTitle:
          return query.OrderBy(i => i.HowToBuyTitle, sortInput.SortOrder);
        case FullCargoItemSortType.ProviderName:
          return query.OrderBy(i => i.ProviderName, sortInput.SortOrder);
        case FullCargoItemSortType.ForwarderName:
          return query.OrderBy(i => i.ForwarderName, sortInput.SortOrder);
        case FullCargoItemSortType.PurchaseOrderCode:
          return query.OrderBy(i => i.PurchaseOrderCode, sortInput.SortOrder);
        case FullCargoItemSortType.PurchaseOrderDeadline:
          return query.OrderBy(i => i.PurchaseOrderDeadline, sortInput.SortOrder);
        case FullCargoItemSortType.PurchaseOrderType:
          return query.OrderBy(i => i.PurchaseOrderType, sortInput.SortOrder);
        case FullCargoItemSortType.PurchaseOrderPrice:
          return query.OrderBy(i => i.PurchaseOrderPrice, sortInput.SortOrder);
        case FullCargoItemSortType.PurchaseOrderCurrencyTitle:
          return query.OrderBy(i => i.PurchaseOrderCurrencyTitle, sortInput.SortOrder);
        case FullCargoItemSortType.Qty:
          return query.OrderBy(i => i.Qty, sortInput.SortOrder);
        case FullCargoItemSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sortInput.SortOrder);
        case FullCargoItemSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sortInput.SortOrder);
        case FullCargoItemSortType.StuffNetWeight:
          return query.OrderBy(i => i.StuffNetWeight, sortInput.SortOrder);
        case FullCargoItemSortType.StuffGrossWeight:
          return query.OrderBy(i => i.StuffGrossWeight, sortInput.SortOrder);
        case FullCargoItemSortType.UnitName:
          return query.OrderBy(i => i.UnitName, sortInput.SortOrder);
        case FullCargoItemSortType.ReceiptedQty:
          return query.OrderBy(i => i.ReceiptedQty, sortInput.SortOrder);
        case FullCargoItemSortType.RemainedCargoItemQty:
          return query.OrderBy(i => i.RemainedCargoItemQty, sortInput.SortOrder);
        case FullCargoItemSortType.QualityControlPassedQty:
          return query.OrderBy(i => i.QualityControlPassedQty, sortInput.SortOrder);
        case FullCargoItemSortType.QualityControlFailedQty:
          return query.OrderBy(i => i.QualityControlFailedQty, sortInput.SortOrder);
        case FullCargoItemSortType.CargoItemDateTime:
          return query.OrderBy(i => i.CargoItemDateTime, sortInput.SortOrder);
        case FullCargoItemSortType.EstimateDateTime:
          return query.OrderBy(i => i.EstimateDateTime, sortInput.SortOrder);
        case FullCargoItemSortType.CurrentPurchaseStepDateTime:
          return query.OrderBy(i => i.CurrentPurchaseStepDateTime, sortInput.SortOrder);
        case FullCargoItemSortType.CurrentPurchaseStepFollowUpDateTime:
          return query.OrderBy(i => i.CurrentPurchaseStepFollowUpDateTime, sortInput.SortOrder);
        case FullCargoItemSortType.CurrentPurchaseStepEmployeeFullName:
          return query.OrderBy(i => i.CurrentPurchaseStepEmployeeFullName, sortInput.SortOrder);
        case FullCargoItemSortType.IsArchived:
          return query.OrderBy(i => i.IsArchived, sortInput.SortOrder);
        case FullCargoItemSortType.PlanCode:
          return query.OrderBy(i => i.PlanCode, sortInput.SortOrder);
        case FullCargoItemSortType.LadingDescription:
          return query.OrderBy(i => i.LadingDescription, sortInput.SortOrder);
        case FullCargoItemSortType.RiskLevelStatus:
          return query.OrderBy(i => i.RiskLevelStatus, sortInput.SortOrder);
        case FullCargoItemSortType.LatestBaseEntityDocumentDateTime:
          return query.OrderBy(i => i.LatestBaseEntityDocumentDateTime, sortInput.SortOrder);
        case FullCargoItemSortType.LatestRiskTitle:
          return query.OrderBy(i => i.LatestRiskTitle, sortInput.SortOrder);
        case FullCargoItemSortType.LatestRiskCreateDateTime:
          return query.OrderBy(i => i.LatestRiskCreateDateTime, sortInput.SortOrder);
        case FullCargoItemSortType.StuffType:
          return query.OrderBy(i => i.StuffType, sortInput.SortOrder);


        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Sort
    public IOrderedQueryable<CargoItemResult> SortCargoItemResult(IQueryable<CargoItemResult> query, SortInput<CargoItemSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case CargoItemSortType.Code:
          return query.OrderBy(i => i.Code, sortInput.SortOrder);
        case CargoItemSortType.DateTime:
          return query.OrderBy(i => i.DateTime, sortInput.SortOrder);
        case CargoItemSortType.PurchaseOrderId:
          return query.OrderBy(i => i.PurchaseOrderId, sortInput.SortOrder);
        case CargoItemSortType.PurchaseOrderCode:
          return query.OrderBy(i => i.PurchaseOrderCode, sortInput.SortOrder);
        case CargoItemSortType.PurchaseOrderDateTime:
          return query.OrderBy(i => i.PurchaseOrderDateTime, sortInput.SortOrder);
        case CargoItemSortType.ProviderId:
          return query.OrderBy(i => i.ProviderId, sortInput.SortOrder);
        case CargoItemSortType.ProviderName:
          return query.OrderBy(i => i.ProviderName, sortInput.SortOrder);
        case CargoItemSortType.ProviderCode:
          return query.OrderBy(i => i.ProviderCode, sortInput.SortOrder);
        case CargoItemSortType.HowToBuyId:
          return query.OrderBy(i => i.HowToBuyId, sortInput.SortOrder);
        case CargoItemSortType.HowToBuyTitle:
          return query.OrderBy(i => i.HowToBuyTitle, sortInput.SortOrder);
        case CargoItemSortType.StuffId:
          return query.OrderBy(i => i.StuffId, sortInput.SortOrder);
        case CargoItemSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sortInput.SortOrder);
        case CargoItemSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sortInput.SortOrder);
        case CargoItemSortType.PurchaseOrderQty:
          return query.OrderBy(i => i.PurchaseOrderQty, sortInput.SortOrder);
        case CargoItemSortType.PurchaseOrderUnitId:
          return query.OrderBy(i => i.PurchaseOrderUnitId, sortInput.SortOrder);
        case CargoItemSortType.PurchaseOrderUnitName:
          return query.OrderBy(i => i.PurchaseOrderUnitName, sortInput.SortOrder);
        case CargoItemSortType.PurchaseOrderDeadline:
          return query.OrderBy(i => i.PurchaseOrderDeadline, sortInput.SortOrder);
        case CargoItemSortType.PurchaseOrderDescription:
          return query.OrderBy(i => i.PurchaseOrderDescription, sortInput.SortOrder);
        case CargoItemSortType.PurchaseOrderCargoedQty:
          return query.OrderBy(i => i.PurchaseOrderCargoedQty, sortInput.SortOrder);
        case CargoItemSortType.PurchaseOrderRemainedQty:
          return query.OrderBy(i => i.PurchaseOrderRemainedQty, sortInput.SortOrder);
        case CargoItemSortType.PurchaseOrderStatus:
          return query.OrderBy(i => i.PurchaseOrderStatus, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IOrderedQueryable<CargoItemComboResult> SortCargoItemComboResult(IQueryable<CargoItemComboResult> query, SortInput<CargoItemComboSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case CargoItemComboSortType.Code:
          return query.OrderBy(i => i.Code, sortInput.SortOrder);
        case CargoItemComboSortType.CargoCode:
          return query.OrderBy(i => i.CargoCode, sortInput.SortOrder);
        case CargoItemComboSortType.ProviderName:
          return query.OrderBy(i => i.ProviderName, sortInput.SortOrder);
        case CargoItemComboSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sortInput.SortOrder);
        case CargoItemComboSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }


    #endregion
    #region ResetStatus
    public CargoItem ResetCargoItemStatus(int cargoItemId)
    {

      var cargoItem = GetCargoItem(id: cargoItemId);
      return ResetCargoItemStatus(cargoItem: cargoItem);
    }
    public CargoItem ResetCargoItemStatus(CargoItem cargoItem)
    {

      #region Reset CargoItemSummary
      var cargoItemSummary = ResetCargoItemSummaryByCargoItemId(
              cargoItemId: cargoItem.Id);
      #endregion
      #region Define Status
      var status = CargoItemStatus.None;

      if (cargoItemSummary.ReceiptedQty > 0)
      {
        if (cargoItemSummary.ReceiptedQty >= cargoItemSummary.CargoItem.Qty)
          status = status | CargoItemStatus.Receipted;
        else
          status = status | CargoItemStatus.Receipting;
      }

      if (cargoItemSummary.QualityControlPassedQty > 0)
      {
        if (cargoItemSummary.QualityControlPassedQty >= cargoItemSummary.CargoItem.Qty)
          status = status | CargoItemStatus.QualityControled;
        else
          status = status | CargoItemStatus.QualityControling;
      }

      if (status == CargoItemStatus.None)
        status = CargoItemStatus.NotAction;
      #endregion
      #region Edit CargoItem
      if (status != cargoItem.Status)
        EditCargoItem(
                      cargoItem: cargoItem,
                      rowVersion: cargoItem.RowVersion,
                      status: status);
      #endregion
      return cargoItem;
    }
    #endregion
  }
}

//using LinqLib.Sort;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.CargoItem;
using lena.Models.Supplies.CargoItemDetail;
using lena.Models.Supplies.Ladings;
using System;
using System.Linq;
using System.Linq.Expressions;



using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region AddProcess
    public CargoItemDetail AddCargoItemDetailProcess(
        int? purchaseOrderDetailId,
        int cargoItemId,
        int stuffId,
        double qty,
        byte unitId,
        BaseTransaction purchaseOrderTransaction,
        DateTime estimateDateTime)
    {

      #region AddTransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion

      #region AddCargoItemDetail
      var cargoItemDetail = AddCargoItemDetail(
              cargoItemDetail: null,
              transactionBatch: transactionBatch,
              cargoItemId: cargoItemId,
              purchaseOrderDetailId: purchaseOrderDetailId,
              qty: qty,
              unitId: unitId,
              description: "");
      #endregion
      #region AddCargoItemDetailSummary
      AddCargoItemDetailSummary(
              receiptedQty: 0,
              ladingItemDetailQty: 0,
              qualityControlPassedQty: 0,
              qualityControlFailedQty: 0,
              cargoItemDetailId: cargoItemDetail.Id);
      #endregion
      #region Add ExportFromOrder TransactionPlan
      BaseTransaction exportPurchaseOrderTransaction = null;
      if (purchaseOrderDetailId != null)
      {
        exportPurchaseOrderTransaction = App.Internals.WarehouseManagement
                  .AddTransactionPlanProcess(
                      transactionBatchId: transactionBatch.Id,
                      effectDateTime: purchaseOrderTransaction.EffectDateTime,
                      stuffId: stuffId,
                      billOfMaterialVersion: null,
                      stuffSerialCode: null,
                      transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportPurchaseOrder.Id,
                      amount: qty,
                      unitId: unitId,
                      description: null,
                      isEstimated: false,
                      referenceTransaction: purchaseOrderTransaction);
      }
      #endregion
      #region Add ImportToCargo TransactionPlan
      var importPurchaseOrderTransaction = App.Internals.WarehouseManagement
          .AddTransactionPlanProcess(
              transactionBatchId: transactionBatch.Id,
              effectDateTime: estimateDateTime,
              stuffId: stuffId,
              billOfMaterialVersion: null,
              stuffSerialCode: null,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportCargo.Id,
              amount: qty,
              unitId: unitId,
              description: null,
              isEstimated: false,
              referenceTransaction: exportPurchaseOrderTransaction);
      #endregion
      #region ResetPurchaseRequestStatus
      if (purchaseOrderDetailId != null)
      {
        App.Internals.Supplies.ResetPurchaseOrderDetailStatus(purchaseOrderDetailId: purchaseOrderDetailId.Value);
      }
      #endregion
      return cargoItemDetail;
    }

    //internal object GetLadings(Expression<Func<Lading, LadingResult>> selector, int? id, string code, int? userId, string sataCode, string kotazhCode, int? customhouseId, bool? isDelete, int? employeeId)
    //{
    //    throw new NotImplementedException();
    //}

    //internal object GetCargoItemWithCurrentStepAndPurchaseDetails(Func<object, object> selector, CargoItemStatus[] statuses, string ladingCode, CargoItemStatus[] notHasStatuses, int? cargoId, int? EmployeeId, int? id, int[] ids, int? financialTransactionBatchIdForCargoCost, int? financialTransactionBatchIdForDeliveryOrder, bool isDelete, bool? isArchived)
    //{
    //    throw new NotImplementedException();
    //}


    #endregion

    #region Add
    public CargoItemDetail AddCargoItemDetail(
       CargoItemDetail cargoItemDetail,
       TransactionBatch transactionBatch,
       int cargoItemId,
       int? purchaseOrderDetailId,
       double qty,
       byte unitId,
       string description)
    {

      cargoItemDetail = cargoItemDetail ?? repository.Create<CargoItemDetail>();
      cargoItemDetail.CargoItemId = cargoItemId;
      cargoItemDetail.PurchaseOrderDetailId = purchaseOrderDetailId;
      cargoItemDetail.Qty = qty;
      cargoItemDetail.UnitId = unitId;
      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: cargoItemDetail,
                  transactionBatch: transactionBatch,
                  description: description);
      return cargoItemDetail;
    }
    #endregion

    #region Edit
    public CargoItemDetail EditCargoItemDetail(
        int id,
        byte[] rowVersion,
        TValue<int> cargoItemId = null,
        TValue<int?> purchaseOrderDetailId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<string> description = null)
    {

      CargoItemDetail cargoItemDetail = GetCargoItemDetail(id: id);
      return EditCargoItemDetail(
                cargoItemDetail: cargoItemDetail,
                cargoItemId: cargoItemId,
                purchaseOrderDetailId: purchaseOrderDetailId,
                rowVersion: rowVersion,
                qty: qty,
                unitId: unitId,
                description: description);
    }
    public CargoItemDetail EditCargoItemDetail(
        CargoItemDetail cargoItemDetail,
        byte[] rowVersion,
        TValue<int> cargoItemId = null,
        TValue<int?> purchaseOrderDetailId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<string> description = null)
    {

      if (cargoItemId != null)
        cargoItemDetail.CargoItemId = cargoItemId;
      if (purchaseOrderDetailId != null)
        cargoItemDetail.PurchaseOrderDetailId = purchaseOrderDetailId;
      if (qty != null)
        cargoItemDetail.Qty = Math.Round(qty, cargoItemDetail.Unit.DecimalDigitCount);
      if (unitId != null)
        cargoItemDetail.UnitId = unitId;
      if (description != null)
        cargoItemDetail.Description = description;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: cargoItemDetail,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as CargoItemDetail;
    }
    #endregion

    #region Get
    public CargoItemDetail GetCargoItemDetail(int id) => GetCargoItemDetail(e => e, id: id);
    public TResult GetCargoItemDetail<TResult>(
        Expression<Func<CargoItemDetail, TResult>> selector,
        int id)
    {

      var cargoItemDetail = GetCargoItemDetails(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (cargoItemDetail == null)
        throw new CargoItemDetailNotFoundException(id);
      return cargoItemDetail;
    }
    public CargoItemDetail GetCargoItemDetail(string code) => GetCargoItemDetail(e => e, code: code);
    public TResult GetCargoItemDetail<TResult>(
        Expression<Func<CargoItemDetail, TResult>> selector,
        string code)
    {

      var cargoItemDetail = GetCargoItemDetails(
                    selector: selector,
                    code: code)


                .FirstOrDefault();
      if (cargoItemDetail == null)
        throw new CargoItemDetailNotFoundException(code: code);
      return cargoItemDetail;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetCargoItemDetails<TResult>(
        Expression<Func<CargoItemDetail, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> cargoItemId = null,
        TValue<int[]> cargoItemIds = null,
        TValue<int> purchaseOrderDetailId = null,
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
      var query = baseQuery.OfType<CargoItemDetail>();
      if (cargoItemId != null)
        query = query.Where(r => r.CargoItemId == cargoItemId);
      if (purchaseOrderDetailId != null)
        query = query.Where(r => r.PurchaseOrderDetailId == purchaseOrderDetailId);
      if (qty != null)
        query = query.Where(r => Math.Abs(r.Qty - qty) < 0.000001);
      if (unitId != null)
        query = query.Where(r => r.UnitId == unitId);
      if (cargoItemIds != null)
        query = query.Where(r => cargoItemIds.Value.Contains(r.CargoItemId));
      if (ids != null)
        query = query.Where(i => ids.Value.Contains(i.Id));
      if (isDelete != null)
        query = query.Where(r => r.IsDelete == isDelete);
      return query.Select(selector);
    }
    #endregion


    #region Gets FullCargoItemDetail
    public IQueryable<TResult> GetFullCargoItemDetails<TResult>(
        Expression<Func<CargoItemDetail, TResult>> selector,
        TValue<string> ladingCode = null,
          TValue<string> cargoItemCode = null,
          TValue<int> cargoId = null,
          TValue<int[]> cargoIds = null,
          TValue<string> cargoCode = null,
          TValue<int> purchaseOrderId = null,
          TValue<string> purchaseOrderCode = null,
          TValue<int> providerId = null,
          TValue<DateTime> fromEstimateDateTime = null,
          TValue<DateTime> toEstimateDateTime = null,
          TValue<DateTime> fromDateTime = null,
          TValue<DateTime> toDateTime = null,
          TValue<int> howToBuyId = null,
          TValue<string> stuffCode = null,
          TValue<CargoItemStatus> status = null,
          TValue<CargoItemStatus[]> statuses = null,
          TValue<CargoItemStatus[]> notHasStatuses = null,
          TValue<bool> isArchived = null,
          TValue<int> employeeId = null,
          TValue<int> stuffId = null,
          TValue<int[]> employeeIds = null,
          TValue<ProviderType> providerType = null,
          TValue<string> planCode = null,
          TValue<string> employeeFullName = null,
          TValue<DateTime> fromDeadLine = null,
          TValue<DateTime> toDeadLine = null
          )
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e
                 );
      var query = baseQuery.OfType<CargoItemDetail>();

      if (cargoCode != null)
        query = query.Where(r => r.CargoItem.Code == cargoCode);
      if (howToBuyId != null)
        query = query.Where(r => r.CargoItem.HowToBuyId == howToBuyId);
      if (cargoItemCode != null)
        query = query.Where(r => r.CargoItem.Code == cargoCode);
      if (providerId != null)
        query = query.Where(r => r.CargoItem.PurchaseOrder.ProviderId == providerId);
      if (ladingCode != null && ladingCode != "")
      {
        var cargoItemDetailIds = (from q in query
                                  from ladingItem in q.CargoItem.LadingItems
                                  where ((ladingItem.Lading.Code == ladingCode) && (ladingItem.IsDelete == false))
                                  select q.Id).Distinct();
        query = from q in query
                join cargoItemDetailId in cargoItemDetailIds
                      on q.Id equals cargoItemDetailId
                select q;
      }
      if (purchaseOrderId != null)
        query = query.Where(r => r.PurchaseOrderDetail.PurchaseOrderId == purchaseOrderId);
      if (purchaseOrderCode != null)
        query = query.Where(r => r.PurchaseOrderDetail.PurchaseOrder.Code == purchaseOrderCode);
      if (fromEstimateDateTime != null)
        query = query.Where(r => r.CargoItem.EstimateDateTime >= fromEstimateDateTime);
      if (toEstimateDateTime != null)
        query = query.Where(r => r.CargoItem.EstimateDateTime <= toEstimateDateTime);
      if (fromDateTime != null)
        query = query.Where(r => r.DateTime <= fromDateTime);
      if (toDateTime != null)
        query = query.Where(r => r.DateTime >= toDateTime);
      if (cargoId != null)
        query = query.Where(r => r.CargoItem.CargoId == cargoId);
      if (cargoIds != null)
        query = query.Where(r => cargoIds.Value.Contains(r.CargoItem.CargoId));

      if (statuses != null)
        query = query.Where(r => statuses.Value.Contains(r.CargoItem.Status));
      if (statuses != null)
      {
        var s = CargoItemStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        query = query.Where(i => (i.CargoItem.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = CargoItemStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        query = query.Where(i => (i.CargoItem.Status & s) == 0);
      }

      if (isArchived != null)
        query = query.Where(r => r.CargoItem.IsArchived == isArchived);
      if (employeeId != null)
        query = query.Where(r => r.CargoItem.User.Employee.Id == employeeId);
      if (employeeIds != null)
        query = query.Where(r => employeeIds.Value.Contains(r.CargoItem.User.Employee.Id));
      if (providerType != null)
        query = query.Where(r => r.CargoItem.PurchaseOrder.Provider.ProviderType == providerType);
      if (stuffId != null)
        query = query.Where(i => i.CargoItem.PurchaseOrder.StuffId == stuffId);
      if (planCode != null && planCode != "")
      {

        query = from item in query

                where item.PurchaseOrderDetail.PurchaseRequest.PlanCode.Code.Contains(planCode)
                select item;
      }

      if (stuffCode != null)
        query = query.Where(i => i.PurchaseOrderDetail.PurchaseOrder.Stuff.Code == stuffCode);
      if (employeeFullName != null)
        query = query.Where(i => (i.CargoItem.User.Employee.FirstName + " " + i.CargoItem.User.Employee.LastName) == employeeFullName);
      if (fromDeadLine != null)
        query = query.Where(i => i.CargoItem.PurchaseOrder.Deadline >= fromDeadLine);
      if (toDeadLine != null)
        query = query.Where(i => i.CargoItem.PurchaseOrder.Deadline <= toDeadLine);


      return query.Select(selector);
    }

    #region ToResult CargoItemDetail
    public Expression<Func<CargoItemDetail, CargoItemDetailResult>> ToCargoItemDetailResult =
                cargoItemDetail => new CargoItemDetailResult()
                {

                  #region CargoItemDetails
                  Id = cargoItemDetail.Id,
                  Code = cargoItemDetail.Code,
                  Qty = Math.Round(cargoItemDetail.Qty, cargoItemDetail.Unit.DecimalDigitCount),
                  UnitId = cargoItemDetail.UnitId,
                  DecimalDigitCount = cargoItemDetail.Unit.DecimalDigitCount,
                  UnitName = cargoItemDetail.Unit.Name,
                  ReceiptQty = cargoItemDetail.CargoItemDetailSummary.ReceiptedQty,
                  ForwarderDocumentId = cargoItemDetail.CargoItem.ForwarderDocumentId,
                  RowVersion = cargoItemDetail.RowVersion,
                  #endregion

                  #region LadingItem and LadingItemDetail
                  LadingItemQty = cargoItemDetail.CargoItem.CargoItemSummary.LadingItemQty,
                  LadingItemDetailQty = cargoItemDetail.CargoItemDetailSummary.LadingItemDetailQty,
                  LadingItems = cargoItemDetail.CargoItem.LadingItems.AsQueryable().Where(m => m.IsDelete == false).Select(App.Internals.Supplies.ToLadingItemResult),
                  SumLadingItemsQty = cargoItemDetail.CargoItem.LadingItems.Where(m => m.IsDelete == false).Sum(i => i.Qty),
                  LadingItemDetails = cargoItemDetail.LadingItemDetails.AsQueryable().Where(m => m.IsDelete == false).Select(App.Internals.Supplies.ToLadingItemDetailResult),
                  SumLadingItemDetailsQty = cargoItemDetail.LadingItemDetails.Where(m => m.IsDelete == false).Sum(i => i.Qty),
                  #endregion

                  #region Cargo and CargoItem
                  CargoId = cargoItemDetail.CargoItem.CargoId,
                  CargoCode = cargoItemDetail.CargoItem.Cargo.Code,
                  CargoItemId = cargoItemDetail.CargoItemId,
                  CargoItemCode = cargoItemDetail.CargoItem.Code,
                  CargoItemQty = Math.Round(cargoItemDetail.CargoItem.Qty, cargoItemDetail.CargoItem.Unit.DecimalDigitCount),
                  CargoItemReceiptQty = cargoItemDetail.CargoItem.CargoItemSummary.ReceiptedQty,
                  CargoItemUnitId = cargoItemDetail.CargoItem.UnitId,
                  CargoItemUnitName = cargoItemDetail.CargoItem.Unit.Name,
                  CargoItemStuffId = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.StuffId,
                  CargoItemStuffCode = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Stuff.Code,
                  CargoItemStuffName = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Stuff.Name,
                  CargoItemStatus = cargoItemDetail.CargoItem.Status,
                  CargoItemHowToBuyId = cargoItemDetail.CargoItem.HowToBuyId,
                  CargoItemEstimateDateTime = cargoItemDetail.CargoItem.EstimateDateTime,
                  CargoItemDateTime = cargoItemDetail.CargoItem.CargoItemDateTime,
                  CargoItemRowVersion = cargoItemDetail.CargoItem.RowVersion,
                  #endregion

                  #region PurchaseOrder
                  PurchaseRequestId = cargoItemDetail.PurchaseOrderDetail.PurchaseRequestId,
                  PurchaseOrderId = cargoItemDetail.PurchaseOrderDetail.PurchaseOrderId,
                  PurchaseOrderCode = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Code,
                  PurchaseOrderStuffId = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.StuffId,
                  PurchaseOrderStuffCode = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Stuff.Code,
                  PurchaseOrderStuffName = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Stuff.Name,
                  PurchaseOrderProviderId = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.ProviderId,
                  PurchaseOrderProviderName = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Provider.Name,
                  PurchaseOrderUnitId = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.UnitId,
                  PurchaseOrderUnitName = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Unit.Name,
                  PurchaseOrderQty = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Qty,
                  PurchaseOrderPrice = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Price,
                  PurchaseOrderCurrencyId = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Currency.Id,
                  PurchaseOrderCurrencyTitle = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Currency.Title,
                  PurchaseOrderReceiptedQty = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.ReceiptedQty,
                  PurchaseOrderCargoedQty = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.CargoedQty,
                  PurchaseOrderRemainedQty = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Qty - cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.CargoedQty,
                  PurchaseOrderDeadline = cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Deadline,
                  PurchaseOrderTotalWeight = (cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Stuff.GrossWeight ?? 0) * cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Qty,
                  #endregion

                  #region PurchaseOrderDetail
                  PurchaseOrderDetailId = cargoItemDetail.PurchaseOrderDetail.Id,
                  PurchaseOrderDetailUnitId = cargoItemDetail.PurchaseOrderDetail.UnitId,
                  PurchaseOrderDetailUnitName = cargoItemDetail.PurchaseOrderDetail.Unit.Name,
                  PurchaseOrderDetailQty = cargoItemDetail.PurchaseOrderDetail.Qty,
                  PurchaseOrderDetailCargoedQty = cargoItemDetail.PurchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                  PurchaseOrderDetailReceiptedQty = cargoItemDetail.PurchaseOrderDetail.PurchaseOrderDetailSummary.ReceiptedQty,
                  PurchaseOrderDetailRemainedQty = cargoItemDetail.PurchaseOrderDetail.Qty - cargoItemDetail.PurchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                  PurchaseOrderDetailQualityControlPassedQty = cargoItemDetail.PurchaseOrderDetail.PurchaseOrderDetailSummary.QualityControlPassedQty,
                  ForwarderId = cargoItemDetail.CargoItem.ForwarderId,
                  ForwarderName = cargoItemDetail.CargoItem.Forwarder.Name,
                  BuyingProcess = cargoItemDetail.CargoItem.BuyingProcess,
                  #endregion
                };
    #endregion


    #region ToResult FullCargoItemDetail

    #region CargoItemDetails
    //IQueryable<CargoItemWithCurrenStepAndPurchaseDetails> fullCargoItemDetails

    public IQueryable<FullCargoItemDetailResult> ToFullCargoItemDetailResultQuery(
        IQueryable<CargoItemDetail> cargoItemDetails,
        IQueryable<CargoItemWithCurrenStepAndPurchaseDetails> fullCargoItemDetails,
        IQueryable<BaseEntityDocument> latestBaseEntityDocuments)
    {

      var query = from cargoItemDetail in cargoItemDetails
                  from ladingItem in cargoItemDetail.CargoItem.LadingItems
                  let lading = ladingItem.Lading
                  select new { ladingItem.CargoItemId, lading.Code, ladingItem.Qty, ladingItem.CargoItem };

      var resultQuery =

             from cargoItemDetail in cargoItemDetails
             group cargoItemDetail by cargoItemDetail.CargoItemId
             into temp
             select new
             {
               CargoItemId = temp.Key,
               PlanCodes = temp.Select(m => m.PurchaseOrderDetail.PurchaseRequest.PlanCode.Code)
             };



      var groupResult = from q in query
                        group q by q.CargoItemId into temp
                        select new
                        {
                          CargoItemId = temp.Key,
                          Ladings = temp.Select(m => new LadingsWithLadinItem { Code = m.Code, Qty = Math.Round(m.Qty, m.CargoItem.Unit.DecimalDigitCount), DateTime = m.CargoItem.CargoItemDateTime }).AsQueryable()
                        };


      var Result = from cargoItemDetail in fullCargoItemDetails
                   join g in groupResult
                   on cargoItemDetail.CargoItem.Id equals g.CargoItemId
                   into tempLadings
                   from cargoItemsWithLadinItem in tempLadings.DefaultIfEmpty()

                   join latestBaseEntityDocument in latestBaseEntityDocuments
                          on cargoItemDetail.CargoItem.Id equals latestBaseEntityDocument.BaseEntityId
                          into tLatestBaseEntityDocuments
                   from latestBaseEntityDocument in tLatestBaseEntityDocuments.DefaultIfEmpty()
                   join rq in resultQuery
                       on cargoItemDetail.CargoItem.Id equals rq.CargoItemId
                       into tempResultQuery
                   from fullCargoWithCargiItemDetail in tempResultQuery.DefaultIfEmpty()



                   select new FullCargoItemDetailResult
                   {
                     Id = cargoItemDetail.CargoItem.Id,
                     Qty = cargoItemDetail.CargoItem.Qty,
                     ReceiptedQty = cargoItemDetail.CargoItem.CargoItemSummary.ReceiptedQty,
                     UnitName = cargoItemDetail.CargoItem.Unit.Name,
                     stuffId = cargoItemDetail.PurchaseOrder.Stuff.Id,
                     StuffCode = cargoItemDetail.PurchaseOrder.Stuff.Code,
                     StuffName = cargoItemDetail.PurchaseOrder.Stuff.Name,
                     EmployeeFullName = cargoItemDetail.CargoItem.User.Employee.FirstName + " " + cargoItemDetail.CargoItem.User.Employee.LastName,
                     ProviderId = cargoItemDetail.PurchaseOrder.Provider.Id,
                     ProviderName = cargoItemDetail.PurchaseOrder.Provider.Name,
                     CargoCode = cargoItemDetail.CargoItem.Cargo.Code,
                     StuffGrossWeight = cargoItemDetail.PurchaseOrder.Stuff.GrossWeight,
                     StuffNetWeight = cargoItemDetail.PurchaseOrder.Stuff.NetWeight,
                     PurchaseOrderPrice = cargoItemDetail.PurchaseOrder.Price,
                     PurchaseOrderCurrencyTitle = cargoItemDetail.PurchaseOrder.Currency.Title,
                     QualityControlPassedQty = cargoItemDetail.CargoItem.CargoItemSummary.QualityControlPassedQty,
                     QualityControlFailedQty = cargoItemDetail.CargoItem.CargoItemSummary.QualityControlFailedQty,
                     EstimateDateTime = cargoItemDetail.CargoItem.EstimateDateTime,
                     IsArchived = cargoItemDetail.CargoItem.IsArchived,
                     PurchaseOrderDeadline = cargoItemDetail.PurchaseOrder.Deadline,
                     BuyingProcess = cargoItemDetail.CargoItem.BuyingProcess,
                     CargoItemDateTime = cargoItemDetail.CargoItem.DateTime,
                     RiskLevelStatus = cargoItemDetail.CargoItem.LatestRisk == null ? RiskLevelStatus.Low : cargoItemDetail.CargoItem.LatestRisk.LatestRiskStatus.RiskParameter.RiskLevelStatus,
                     HowToBuyId = cargoItemDetail.CargoItem.HowToBuy.Id,
                     CargoItemCode = cargoItemDetail.CargoItem.Code,
                     CargoItemStatus = cargoItemDetail.CargoItem.Status,
                     PurchaseOrderType = cargoItemDetail.PurchaseOrder.PurchaseOrderType,
                     Ladings = cargoItemsWithLadinItem.Ladings,
                     HowToBuyTitle = cargoItemDetail.CargoItem.HowToBuy.Title,
                     CargoItemDetailDeadLine = cargoItemDetail.PurchaseOrder.Deadline,
                     HowToBuyDetailTitle = cargoItemDetail.PurchaseStep.HowToBuyDetail.Title,
                     CurrentPurchaseStepDateTime = (DateTime?)cargoItemDetail.PurchaseStep.DateTime,
                     CurrentPurchaseStepFollowUpDateTime = (DateTime?)cargoItemDetail.PurchaseStep.FollowUpDateTime,
                     CurrentPurchaseStepEmployeeFullName =
                               cargoItemDetail.PurchaseStep.User.Employee.FirstName + " " +
                               cargoItemDetail.PurchaseStep.User.Employee.LastName


                   };

      return Result;


    }

    #endregion

    #region ToFullCargoItemDetailResult

    #endregion

    #region ToFullCargoItemResult
    public Expression<Func<CargoItemWithCurrenStepAndPurchaseDetails, FullCargoItemDetailResult>> ToFullCargoItemDetailResult =
        (cargoItemDetailResult) => new FullCargoItemDetailResult
        {
          Id = cargoItemDetailResult.CargoItem.Id,
          RowVersion = cargoItemDetailResult.CargoItem.RowVersion,
          CargoId = cargoItemDetailResult.CargoItem.Cargo.Id,
          CargoCode = cargoItemDetailResult.CargoItem.Cargo.Code,
          HowToBuyId = cargoItemDetailResult.CargoItem.HowToBuy.Id,
          HowToBuyTitle = cargoItemDetailResult.CargoItem.HowToBuy.Title,
          ProviderId = cargoItemDetailResult.PurchaseOrder.Provider.Id,
          ProviderName = cargoItemDetailResult.PurchaseOrder.Provider.Name,

          HowToBuyDetailTitle = cargoItemDetailResult.PurchaseStep.HowToBuyDetail.Title,
          PurchaseOrderDeadline = cargoItemDetailResult.PurchaseOrder.Deadline,

          PurchaseOrderType = cargoItemDetailResult.PurchaseOrder.PurchaseOrderType,

          StuffCode = cargoItemDetailResult.PurchaseOrder.Stuff.Code,
          StuffName = cargoItemDetailResult.PurchaseOrder.Stuff.Name,
          StuffNetWeight = cargoItemDetailResult.PurchaseOrder.Stuff.NetWeight,
          StuffGrossWeight = cargoItemDetailResult.PurchaseOrder.Stuff.GrossWeight,
          PurchaseOrderPrice = cargoItemDetailResult.PurchaseOrder.Price,

          PurchaseOrderCurrencyTitle = cargoItemDetailResult.PurchaseOrder.Currency.Title,
          Qty = Math.Round(cargoItemDetailResult.CargoItem.Qty, cargoItemDetailResult.CargoItem.Unit.DecimalDigitCount),

          UnitName = cargoItemDetailResult.CargoItem.Unit.Name,


          QualityControlPassedQty = cargoItemDetailResult.CargoItem.CargoItemSummary.QualityControlPassedQty,
          QualityControlFailedQty = cargoItemDetailResult.CargoItem.CargoItemSummary.QualityControlFailedQty,
          ReceiptedQty = cargoItemDetailResult.CargoItem.CargoItemSummary.ReceiptedQty,
          EstimateDateTime = cargoItemDetailResult.CargoItem.EstimateDateTime,
          //LadingCode = cargoItemresult.CargoItem.Lading.Code,
          EmployeeFullName = cargoItemDetailResult.CargoItem.User.Employee.FirstName + " " + cargoItemDetailResult.CargoItem.User.Employee.LastName,
          RemainedCargoItemQty = Math.Round(cargoItemDetailResult.CargoItem.Qty, cargoItemDetailResult.CargoItem.Unit.DecimalDigitCount) - Math.Round(cargoItemDetailResult.CargoItem.CargoItemSummary.ReceiptedQty, cargoItemDetailResult.CargoItem.Unit.DecimalDigitCount),

        };
    #endregion

    #endregion



    #region sort

    public IOrderedQueryable<FullCargoItemDetailResult> SortFullCargoItemDetailsResult(IQueryable<FullCargoItemDetailResult> query, SortInput<CargoItemDetailSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case CargoItemDetailSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);
        case CargoItemDetailSortType.Qty:
          return query.OrderBy(i => i.Qty, sortInput.SortOrder);
        case CargoItemDetailSortType.CargoCode:
          return query.OrderBy(i => i.CargoCode, sortInput.SortOrder);
        case CargoItemDetailSortType.CargoItemDateTime:
          return query.OrderBy(i => i.CargoItemDateTime, sortInput.SortOrder);
        case CargoItemDetailSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sortInput.SortOrder);
        case CargoItemDetailSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sortInput.SortOrder);
        case CargoItemDetailSortType.StuffGrossWeight:
          return query.OrderBy(i => i.StuffGrossWeight, sortInput.SortOrder);
        case CargoItemDetailSortType.StuffNetWeight:
          return query.OrderBy(i => i.StuffNetWeight, sortInput.SortOrder);
        case CargoItemDetailSortType.BuyingProcess:
          return query.OrderBy(i => i.BuyingProcess, sortInput.SortOrder);
        case CargoItemDetailSortType.CargoItemStatus:
          return query.OrderBy(i => i.CargoItemStatus, sortInput.SortOrder);
        case CargoItemDetailSortType.EmployeeFullName:
          return query.OrderBy(i => i.EmployeeFullName, sortInput.SortOrder);
        case CargoItemDetailSortType.HowToBuyTitle:
          return query.OrderBy(i => i.HowToBuyTitle, sortInput.SortOrder);
        case CargoItemDetailSortType.IsArchived:
          return query.OrderBy(i => i.IsArchived, sortInput.SortOrder);
        case CargoItemDetailSortType.PlanCode:
          return query.OrderBy(i => i.PlanCode, sortInput.SortOrder);
        case CargoItemDetailSortType.ProviderName:
          return query.OrderBy(i => i.ProviderName, sortInput.SortOrder);
        case CargoItemDetailSortType.PurchaseOrderCurrencyTitle:
          return query.OrderBy(i => i.PurchaseOrderCurrencyTitle, sortInput.SortOrder);
        case CargoItemDetailSortType.PurchaseOrderDeadline:
          return query.OrderBy(i => i.PurchaseOrderDeadline, sortInput.SortOrder);
        case CargoItemDetailSortType.PurchaseOrderPrice:
          return query.OrderBy(i => i.PurchaseOrderPrice, sortInput.SortOrder);
        case CargoItemDetailSortType.PurchaseOrderType:
          return query.OrderBy(i => i.PurchaseOrderType, sortInput.SortOrder);
        case CargoItemDetailSortType.ReceiptedQty:
          return query.OrderBy(i => i.ReceiptedQty, sortInput.SortOrder);
        case CargoItemDetailSortType.RiskLevelStatus:
          return query.OrderBy(i => i.RiskLevelStatus, sortInput.SortOrder);
        case CargoItemDetailSortType.CargoItemCode:
          return query.OrderBy(i => i.CargoItemCode, sortInput.SortOrder);
        case CargoItemDetailSortType.CargoItemHowToBuyId:
          return query.OrderBy(i => i.HowToBuyId, sortInput.SortOrder);
        case CargoItemDetailSortType.LadingDescription:
          return query.OrderBy(i => i.LadingDescription, sortInput.SortOrder);
        case CargoItemDetailSortType.QualityControlPassedQty:
          return query.OrderBy(i => i.QualityControlPassedQty, sortInput.SortOrder);
        case CargoItemDetailSortType.QualityControlFailedQty:
          return query.OrderBy(i => i.QualityControlFailedQty, sortInput.SortOrder);
        case CargoItemDetailSortType.HowToBuyDetailTitle:
          return query.OrderBy(i => i.HowToBuyDetailTitle, sortInput.SortOrder);
        case CargoItemDetailSortType.CurrentPurchaseStepDateTime:
          return query.OrderBy(i => i.CurrentPurchaseStepDateTime, sortInput.SortOrder);
        case CargoItemDetailSortType.CurrentPurchaseStepFollowUpDateTime:
          return query.OrderBy(i => i.CurrentPurchaseStepDateTime);
        case CargoItemDetailSortType.CurrentPurchaseStepEmployeeFullName:
          return query.OrderBy(i => i.CurrentPurchaseStepEmployeeFullName, sortInput.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    #endregion

    #region Sort Cargo Item
    public IOrderedQueryable<CargoItemDetailResult> SortCargoItemDetailResult(IQueryable<CargoItemDetailResult> query, SearchInput<CargoItemDetailSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case CargoItemDetailSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion


    #region RemoveProcess
    public void RemoveCargoItemDetail(
        int id,
        byte[] rowVersion,
        TransactionBatch transactionBatch)
    {

      var cargoItemDetail = GetCargoItemDetail(id: id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: null,
                    baseEntity: cargoItemDetail,
                    rowVersion: rowVersion);
    }
    #endregion

    #region Search
    public IQueryable<FullCargoItemDetailResult> SearchCargoItemDetailResults(
        IQueryable<FullCargoItemDetailResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        int? stuffId,
        TValue<string> cargoCode,
        TValue<string> cargoItemCode,
        int? howToBuyId,
        int? providerId,
        RiskLevelStatus? riskLevelStatus,
        DateTime? fromDeadLine,
        DateTime? toDeadLine,
        DateTime? fromEstimateDate,
        DateTime? toEstimateDate,
        DateTime? fromRegistrationDate,
        DateTime? toRegistrationDate,
        TValue<PurchaseOrderType[]> purchaseOrderType,
        TValue<bool> isArchived,
        TValue<bool> hasLading)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = query.Where(i =>
            i.HowToBuyDetailTitle.Contains(searchText) ||
            i.StuffName.Contains(searchText) ||
            i.StuffCode.Contains(searchText) ||
            i.EmployeeFullName.Contains(searchText) ||
            i.CargoItemCode.Contains(searchText) ||
            i.ProviderName.Contains(searchText) ||
            i.UnitName.Contains(searchText) ||
            i.CargoCode.Contains(searchText) ||
            i.HowToBuyTitle.Contains(searchText));



      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      if (stuffId != null)
        query = query.Where(t => t.stuffId == stuffId);
      if (cargoCode != null)
        query = query.Where(t => t.CargoCode == cargoCode);
      if (howToBuyId != null)
        query = query.Where(t => t.HowToBuyId == howToBuyId);
      if (cargoItemCode != null)
        query = query.Where(t => t.CargoItemCode == cargoItemCode);
      if (providerId != null)
        query = query.Where(t => t.ProviderId == providerId);
      if (riskLevelStatus != null)
        query = query.Where(t => t.RiskLevelStatus == riskLevelStatus);
      if (fromRegistrationDate != null)
        query = query.Where(i => i.CargoItemDateTime >= fromRegistrationDate);
      if (toRegistrationDate != null)
        query = query.Where(i => i.CargoItemDateTime <= toRegistrationDate);
      if (fromEstimateDate != null)
        query = query.Where(i => i.EstimateDateTime >= fromEstimateDate);
      if (toEstimateDate != null)
        query = query.Where(i => i.EstimateDateTime <= toEstimateDate);
      if (riskLevelStatus != null)
        query = query.Where(i => i.RiskLevelStatus == riskLevelStatus);
      if (fromDeadLine != null)
        query = query.Where(i => i.CargoItemDetailDeadLine >= fromDeadLine);
      if (toDeadLine != null)
        query = query.Where(i => i.CargoItemDetailDeadLine <= toDeadLine);
      if (purchaseOrderType != null)
        query = query.Where(i => purchaseOrderType.Value.Contains(i.PurchaseOrderType));
      if (isArchived != null)
        query = query.Where(i => i.IsArchived == isArchived);
      if (hasLading != null)
      {
        query = query.Where(i => i.Ladings.Any() == hasLading);
      }



      return query;
    }
    #endregion
    //#region ResetStatus
    //public CargoItem ResetCargoItemDetailStatus(int cargoItemId)
    //{
    //    
    //        var cargoItem = GetCargoItem(id: cargoItemId);
    //        return ResetCargoItemDetailStatus(cargoItem: cargoItem)
    //            
    //;
    //    });
    //}
    //public CargoItem ResetCargoItemDetailStatus  (CargoItem cargoItem)
    //{
    //    
    //        #region Reset CargoItemSummary
    //        var cargoItemSummary = ResetCargoItemSummaryByCargoItemId(
    //                cargoItemId: cargoItem.Id)
    //            
    //;
    //        #endregion
    //        #region Define Status
    //        var status = CargoItemStatus.None;

    //        if (cargoItemSummary.ReceiptedQty > 0)
    //        {
    //            if (cargoItemSummary.ReceiptedQty >= cargoItemSummary.CargoItem.Qty)
    //                status = status | CargoItemStatus.Receipted;
    //            else
    //                status = status | CargoItemStatus.Receipting;
    //        }

    //        if (cargoItemSummary.QualityControlPassedQty > 0)
    //        {
    //            if (cargoItemSummary.QualityControlPassedQty >= cargoItemSummary.CargoItem.Qty)
    //                status = status | CargoItemStatus.QualityControled;
    //            else
    //                status = status | CargoItemStatus.QualityControling;
    //        }

    //        #endregion
    //        return cargoItem;
    //    });
    //}
    //#endregion
    public CargoItemDetail ResetCargoItemDetailStatus(int cargoItemDetailId)
    {

      var cargoItemDetail = GetCargoItemDetail(id: cargoItemDetailId);
      return ResetCargoItemDetailStatus(cargoItemDetail: cargoItemDetail);
    }
    public CargoItemDetail ResetCargoItemDetailStatus(CargoItemDetail cargoItemDetail)
    {


      #region ResetOrderItemSummary
      var cargoItemDetailSummary = ResetCargoItemDetailSummaryByCargoItemDetailId(
              cargoItemDetailId: cargoItemDetail.Id);
      #endregion
      var status = CargoItemStatus.None;

      if (cargoItemDetailSummary.ReceiptedQty > 0)
      {
        if (cargoItemDetailSummary.ReceiptedQty >= cargoItemDetailSummary.CargoItemDetail.Qty)
          status = status | CargoItemStatus.Receipted;
        else
          status = status | CargoItemStatus.Receipting;
      }

      if (cargoItemDetailSummary.QualityControlPassedQty > 0)
      {
        if (cargoItemDetailSummary.QualityControlPassedQty >= cargoItemDetailSummary.CargoItemDetail.Qty)
          status = status | CargoItemStatus.QualityControled;
        else
          status = status | CargoItemStatus.QualityControling;
      }
      #endregion


      return cargoItemDetail;
    }
  }
}


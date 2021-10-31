using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
//using System.Data.Entity;
using lena.Models.Supplies.Ladings;
using System.Collections.Generic;
using lena.Models.Supplies.LadingItem;
using lena.Models.UserManagement.User;
using lena.Models.StaticData;
using lena.Models.WarehouseManagement.StoreReceipt;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Reset LadingItemStatus
    public LadingItem ResetLadingItemStatus(int ladingItemId)
    {

      var ladingItem = GetLadingItem(id: ladingItemId);
      return ResetLadingItemStatus(ladingItem: ladingItem);
    }
    public LadingItem ResetLadingItemStatus(LadingItem ladingItem)
    {

      #region Reset LadingItemSummary
      var LadingItemSummary = ResetLadingItemSummaryByLadingItemId(
              ladingItemId: ladingItem.Id);
      #endregion
      #region Define Status
      var status = LadingItemStatus.None;
      if (LadingItemSummary.ReceiptedQty > 0)
      {
        if (LadingItemSummary.ReceiptedQty >= LadingItemSummary.LadingItem.Qty)
          status = status | LadingItemStatus.Receipted;
        else
          status = status | LadingItemStatus.Receipting;
      }
      if (LadingItemSummary.QualityControlPassedQty > 0)
      {
        if (LadingItemSummary.QualityControlPassedQty >= LadingItemSummary.LadingItem.Qty)
          status = status | LadingItemStatus.QualityControled;
        else
          status = status | LadingItemStatus.QualityControling;
      }
      if (status == LadingItemStatus.None)
        status = LadingItemStatus.NotAction;
      #endregion
      #region EditLadingItem
      if (status != ladingItem.Status)
        EditLadingItem(
                      ladingItem: ladingItem,
                      rowVersion: ladingItem.RowVersion,
                      status: status);
      #endregion
      return ladingItem;
    }
    #endregion
    #region AddLadingItem
    public LadingItem AddLadingItem(
       double qty,
       int cargoItemId,
       int ladingId)
    {

      var user = App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString());
      var ladingItem = repository.Create<LadingItem>();
      var cargoItem = GetCargoItem(id: cargoItemId);
      ladingItem.Qty = Math.Round(qty, cargoItem.Unit.DecimalDigitCount);
      ladingItem.CargoItemId = cargoItemId;
      ladingItem.LadingId = ladingId;
      ladingItem.UserId = user.UserId;
      ladingItem.DateTime = DateTime.Now.ToUniversalTime();
      ladingItem.Status = LadingItemStatus.NotAction;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: ladingItem,
                    transactionBatch: null,
                    financialTransactionBatch: null);
      return ladingItem;
    }
    #endregion
    #region CheckLadingItemAmount
    public void CheckLadingItemAmount(int cargoItemId, double qty, double offset = 0)
    {

      var lira = GetLadingItemRemainingAmount(new int[] { cargoItemId }).First();
      if (lira.TotalLadingAmount - offset + qty > lira.CargoItemQty)
      {
        var avaliableAmount = lira.CargoItemQty - lira.TotalLadingAmount;
        throw new LadingItemQtyCannotBeMoreThanCargoAvaliableAmountException(cargoItemId, qty, avaliableAmount);
      }
    }
    #endregion
    #region GetLadingItemRemainingAmount
    public IQueryable<LadingItemRemainingAmountResult> GetLadingItemRemainingAmount(int[] cargoItemIds)
    {

      var result = new List<LadingItemRemainingAmountResult>();
      foreach (var cargoItemId in cargoItemIds)
      {
        var cargoItem = App.Internals.Supplies.GetCargoItem(cargoItemId);
        var totalLadingAmount = cargoItem.LadingItems.Where(m => m.IsDelete == false).Sum(a => a.Qty * a.CargoItem.Unit.ConversionRatio) / cargoItem.Unit.ConversionRatio;
        result.Add(new LadingItemRemainingAmountResult
        {
          CargoItemId = cargoItemId,
          CargoItemQty = Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount),
          TotalLadingAmount = totalLadingAmount,
          UnitId = cargoItem.UnitId
        });
      }
      return result.AsQueryable();
    }
    #endregion
    #region AddLadingItemProcess
    public LadingItem AddLadingItemProcess(int ladingId, int cargoItemId, double qty)
    {

      CheckLadingItemAmount(cargoItemId: cargoItemId, qty: qty);
      #region AddLadingItem
      var ladingItem = AddLadingItem(
          cargoItemId: cargoItemId,
          ladingId: ladingId,
          qty: qty);
      #endregion
      #region AddLadingItemSummary
      AddLadingItemSummary(
              receiptedQty: 0,
              qualityControlPassedQty: 0,
              qualityControlFailedQty: 0,
              ladingItemId: ladingItem.Id);
      #endregion
      #region ResetCargoItemStatus                                                     
      ResetCargoItemStatus(
              cargoItemId: cargoItemId);
      #endregion
      return ladingItem;
    }
    #endregion
    #region EditLadingItemAmountProcess
    public void EditLadingItemAmountProcess(int id, byte[] rowVersion, int cargoItemId, double qty)
    {

      var ladingItem = GetLadingItem(id);
      CheckLadingItemAmount(cargoItemId: cargoItemId, qty: qty, offset: ladingItem.Qty);
      EditLadingItem(id: id, rowVersion: rowVersion, qty: qty);
    }
    #endregion
    #region EditLadingItem
    public LadingItem EditLadingItem(
        int id,
        byte[] rowVersion,
        TValue<double> qty = null,
        TValue<int> ladingId = null,
        TValue<bool> isDelete = null,
        TValue<int> cargoItemId = null,
        TValue<LadingItemStatus> status = null
       )
    {

      var ladingItem = GetLadingItem(id);
      ladingItem = EditLadingItem(
                    ladingItem: ladingItem,
                    rowVersion: rowVersion,
                    qty: qty,
                    status: status,
                    cargoItemId: cargoItemId,
                    ladingId: ladingId);
      #region ResetCargoItemStatus                                                     
      ResetCargoItemStatus(
              cargoItemId: ladingItem.CargoItemId);
      #endregion
      return ladingItem;
    }
    public LadingItem EditLadingItem(
        LadingItem ladingItem,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<LadingItemStatus> status = null,
        TValue<double> qty = null,
        TValue<int> cargoItemId = null,
        TValue<int> ladingId = null)
    {

      //var user = App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString());
      //ladingItem.UserId = user.UserId;
      //ladingItem.DateTime = DateTime.Now.ToUniversalTime();
      if (ladingId != null)
        ladingItem.LadingId = ladingId;
      if (qty != null)
        ladingItem.Qty = Math.Round(qty, ladingItem.CargoItem.Unit.DecimalDigitCount);
      if (cargoItemId != null)
        ladingItem.CargoItemId = cargoItemId;
      if (status != null)
        ladingItem.Status = status;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: ladingItem,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: null);
      return retValue as LadingItem;
    }
    #endregion
    #region Gets LadingItems
    public IQueryable<TResult> GetLadingItems<TResult>(
        Expression<Func<LadingItem, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<double> qty = null,
        TValue<int> cargoItemId = null,
        TValue<int> cargoItemDetailId = null,
        TValue<int> ladingId = null,
        TValue<int[]> ladingIds = null,
        TValue<int> ladingItemDetailId = null,
        TValue<DateTime> fromDeliveryDate = null,
        TValue<DateTime> toDeliveryDate = null,
        TValue<DateTime> fromTransportDate = null,
        TValue<DateTime> toTransportDate = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<int> stuffId = null,
        TValue<int> financialTransactionBatchId = null,
        TValue<int> financialDocumentId = null,
        TValue<string> cargoCode = null,
        TValue<string> cargoItemCode = null,
        TValue<string> purchaseOrderCode = null,
        TValue<int> employeeId = null,
        TValue<int> providerId = null,
        TValue<bool> isDelete = null,
        TValue<int> planCodeId = null,
        TValue<int[]> selectedPlanCodeIds = null,
        TValue<ProviderType> providerType = null
        )
    {

      var query = repository.GetQuery<LadingItem>();
      if (providerType != null)
        query = query.Where(i => i.CargoItem.PurchaseOrder.Provider.ProviderType == providerType);
      if (id != null)
        query = query.Where(a => a.Id == id);
      if (ids != null)
        query = query.Where(i => ids.Value.Contains(i.Id));
      if (qty != null)
        query = query.Where(a => a.Qty == qty);
      if (cargoItemId != null)
        query = query.Where(a => a.CargoItemId == cargoItemId);
      if (ladingId != null)
        query = query.Where(a => a.LadingId == ladingId);
      if (ladingIds != null)
        query = query.Where(i => ladingIds.Value.Contains(i.LadingId));
      if (fromDeliveryDate != null)
        query = query.Where(a => a.Lading.DeliveryDateTime >= fromDeliveryDate);
      if (toDeliveryDate != null)
        query = query.Where(i => i.Lading.DeliveryDateTime <= toDeliveryDate);
      if (financialTransactionBatchId != null)
        query = query.Where(i => i.LadingCosts.Any(lc =>
                  lc.FinancialDocumentCost.FinancialDocument.FinancialTransactionBatch.Id == financialTransactionBatchId));
      if (fromTransportDate != null)
        query = query.Where(i => i.Lading.TransportDateTime.Value.Date >= fromTransportDate);
      if (toTransportDate != null)
        query = query.Where(i => i.Lading.TransportDateTime.Value.Date <= toTransportDate);
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);
      if (stuffId != null)
        query = query.Where(a => a.CargoItem.PurchaseOrder.StuffId == stuffId);
      if (cargoCode != null)
        query = query.Where(a => a.CargoItem.Cargo.Code == cargoCode);
      if (cargoItemCode != null)
        query = query.Where(a => a.CargoItem.Code == cargoItemCode);
      if (purchaseOrderCode != null)
        query = query.Where(a => a.CargoItem.PurchaseOrder.Code == purchaseOrderCode);
      if (employeeId != null)
        query = query.Where(i => i.User.Employee.Id == employeeId);
      if (isDelete != null)
        query = query.Where(i => i.IsDelete == isDelete);
      if (financialDocumentId != null)
        query = query.Where(i => i.LadingCosts.Select(d => d.FinancialDocumentCost.FinancialDocument.Id).Contains<int>(financialDocumentId));
      if (providerId != null)
        query = query.Where(i => i.CargoItem.PurchaseOrder.ProviderId == providerId);
      if (ladingItemDetailId != null)
      {
        var ladingItemDetails = App.Internals.Supplies.GetLadingItemDetails(e => e);
        var ladingItemIds = (from q in ladingItemDetails
                             where (q.Id == ladingItemDetailId)
                             select q.LadingItemId).Distinct();
        query = from q in query
                join ladingItemId in ladingItemIds
                      on q.Id equals ladingItemId
                select q;
      }
      if (cargoItemDetailId != null)
      {
        var cargoItemDetails = GetCargoItemDetails(e => e);
        var cargoItemDetailIds = (from cargoItemDetail in cargoItemDetails
                                  where (cargoItemDetail.Id == cargoItemDetailId)
                                  select cargoItemDetail.Id).Distinct();
        query = from q in query
                from cargoItemdetail in q.CargoItem.CargoItemDetails
                join cargoItemdetailId in cargoItemDetailIds
                      on cargoItemdetail.Id equals cargoItemdetailId
                select q;
      }
      if (selectedPlanCodeIds != null)
      {
        query = from item in query
                from cargoItemDetail in item.CargoItem.CargoItemDetails
                where selectedPlanCodeIds.Value.Contains(cargoItemDetail.PurchaseOrderDetail.PurchaseRequest.PlanCodeId.Value)
                select item;
      }
      return query.Select(selector);
    }
    #endregion
    #region Get LadingItem
    public LadingItem GetLadingItem(int id) => GetLadingItem(selector: e => e, id: id);
    public TResult GetLadingItem<TResult>(
        Expression<Func<LadingItem, TResult>> selector,
        int id)
    {

      var ladingItem = GetLadingItems(selector: selector, id: id)


            .FirstOrDefault();
      if (ladingItem == null)
        throw new LadingItemNotFoundException(id);
      return ladingItem;
    }
    #endregion
    #region ToLadingItemResult
    public IQueryable<FullLadingItemsResult> ToLadingItemsResultQuery(IQueryable<LadingItem> query, IQueryable<CargoItem> cargoItems, IQueryable<PurchaseOrderPlanCodeView> purchaseOrderPlanCodes
    )
    {
      var resultQuery = from ladingItem in query
                        join purchaseOrderPlanCode in purchaseOrderPlanCodes on
                          ladingItem.CargoItem.PurchaseOrderId equals purchaseOrderPlanCode.PurchaseOrderId into tPurchaseOrderWithPlanCode
                        from purchaseOrderWithPlanCode in tPurchaseOrderWithPlanCode.DefaultIfEmpty()
                        let cargoItem = ladingItem.CargoItem
                        let purchaseOrder = cargoItem.PurchaseOrder
                        let stuff = purchaseOrder.Stuff
                        select new FullLadingItemsResult
                        {
                          LadingItemId = ladingItem.Id,
                          LadingCode = ladingItem.Lading.Code,
                          LadingItemDateTime = ladingItem.DateTime,
                          LadingItemStatus = ladingItem.Status,
                          UserId = ladingItem.UserId,
                          UserFullName = ladingItem.User.Employee.FirstName + " " + ladingItem.User.Employee.LastName,
                          CargoItemId = ladingItem.CargoItemId,
                          CargoItemCode = cargoItem.Code,
                          CargoItemDateTime = cargoItem.DateTime,
                          CargoItemProviderCode = purchaseOrder.Provider.Code,
                          CargoItemProviderName = purchaseOrder.Provider.Name,
                          CargoItemStatus = cargoItem.Status,
                          CargoItemEstimateDateTime = cargoItem.EstimateDateTime,
                          CargoItemEmployeeFullName = cargoItem.User.Employee.FirstName + " " + cargoItem.User.Employee.LastName,
                          PurchaseOrderDeadline = purchaseOrder.Deadline,
                          PurchaseOrderCurrencyTitle = purchaseOrder.Currency.Title,
                          StuffId = stuff.Id,
                          StuffCode = stuff.Code,
                          StuffName = stuff.Name,
                          StuffNetWeight = stuff.NetWeight,
                          StuffGrossWeight = stuff.GrossWeight,
                          StuffTotalGrossWeight = stuff.GrossWeight * ladingItem.Qty * cargoItem.Unit.ConversionRatio,
                          StuffPrice = purchaseOrder.Price,
                          CurrencyId = purchaseOrder.CurrencyId,
                          CurrencyTitle = purchaseOrder.Currency.Title,
                          ProviderId = purchaseOrder.ProviderId,
                          ProviderName = purchaseOrder.Provider.Name,
                          UnitId = cargoItem.UnitId,
                          UnitName = cargoItem.Unit.Name,
                          PurchaseOrderCode = purchaseOrder.Code,
                          //CargoItemQty = cargoItem.Qty,
                          //LadingItemQty = ladingItem.Qty,
                          //CargoItemReceiptedQty = ladingItem.LadingItemSummary.ReceiptedQty ,
                          //CargoItemWithoutLadingQty = ladingItem.Qty - (cargoItem.LadingItems.Sum(a => a.Qty * a.CargoItem.Unit.ConversionRatio) / cargoItem.Unit.ConversionRatio),
                          CargoItemQty = Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount),
                          LadingItemQty = Math.Round(ladingItem.Qty, cargoItem.Unit.DecimalDigitCount),
                          CargoItemReceiptedQty = (purchaseOrder.PurchaseOrderSummary.ReceiptedQty * purchaseOrder.Unit.ConversionRatio) / cargoItem.Unit.ConversionRatio,
                          CargoItemWithoutLadingQty = Math.Round(ladingItem.Qty, cargoItem.Unit.DecimalDigitCount) - (Math.Round(cargoItem.LadingItems.Sum(a => a.Qty * a.CargoItem.Unit.ConversionRatio), cargoItem.Unit.DecimalDigitCount) / cargoItem.Unit.ConversionRatio),
                          BankOrderId = ladingItem.Lading.BankOrderId,
                          BankName = ladingItem.Lading.BankOrder.Bank.Title,
                          BankOrderCode = ladingItem.Lading.BankOrder.Code,
                          BankOrderNumber = ladingItem.Lading.BankOrder.OrderNumber,
                          LadingId = ladingItem.LadingId,
                          KotazhCode = ladingItem.Lading.KotazhCode,
                          SataCode = ladingItem.Lading.SataCode,
                          CustomhouseId = ladingItem.Lading.CustomhouseId,
                          CustomhouseName = ladingItem.Lading.Customhouse.Title,
                          TransportDate = ladingItem.Lading.TransportDateTime,
                          DeliveryDate = ladingItem.Lading.DeliveryDateTime,
                          DateTime = ladingItem.Lading.DateTime,
                          CargoId = cargoItem.CargoId,
                          CargoCode = cargoItem.Cargo.Code,
                          CurrentBankOrderStatusName = ladingItem.Lading.CurrentLadingBankOrderLog.LadingBankOrderStatus.Name,
                          //CurrentBankOrderLogDescription = ladingItem.Lading.CurrentLadingBankOrderLog.Description,
                          CurrentCustomhouseStatusName = ladingItem.Lading.CurrentLadingCustomhouseLog.LadingCustomhouseStatus.Name,
                          CurrentCustomhouseLogDescription = ladingItem.Lading.CurrentLadingCustomhouseLog.Description,
                          PlanCode = purchaseOrderWithPlanCode.PlanCodes
                        };
      return resultQuery;
    }
    public Expression<Func<LadingItem, LadingItemResult>> ToLadingItemResult =
        (ladingItem) => new LadingItemResult
        {
          Id = ladingItem.Id,
          Code = ladingItem.Lading.Code,
          Qty = Math.Round(ladingItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount),
          ReceiptedQty = ladingItem.LadingItemSummary.ReceiptedQty,
          Status = ladingItem.Status,
          LadingId = ladingItem.LadingId,
          CargoCode = ladingItem.CargoItem.Cargo.Code,
          CargoItemId = ladingItem.CargoItemId,
          UserId = ladingItem.UserId,
          UserFullName = ladingItem.User.Employee.FirstName + " " + ladingItem.User.Employee.LastName,
          DateTime = ladingItem.DateTime,
          CargoItemCode = ladingItem.CargoItem.Code,
          CargoItemProviderCode = ladingItem.CargoItem.PurchaseOrder.Provider.Code,
          CargoItemProviderName = ladingItem.CargoItem.PurchaseOrder.Provider.Name,
          StuffId = ladingItem.CargoItem.PurchaseOrder.Stuff.Id,
          StuffCode = ladingItem.CargoItem.PurchaseOrder.Stuff.Code,
          StuffName = ladingItem.CargoItem.PurchaseOrder.Stuff.Name,
          UnitId = ladingItem.CargoItem.UnitId,
          UnitName = ladingItem.CargoItem.Unit.Name,
          StuffGrossWeight = ladingItem.CargoItem.PurchaseOrder.Stuff.GrossWeight,
          StuffTotalGrossWeight = ladingItem.CargoItem.PurchaseOrder.Stuff.GrossWeight * ladingItem.Qty * ladingItem.CargoItem.Unit.ConversionRatio,
          ConversionRatio = ladingItem.CargoItem.Unit.ConversionRatio,
          DecimalDigitCount = ladingItem.CargoItem.Unit.DecimalDigitCount,
          CargoItemQty = Math.Round(ladingItem.CargoItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount),
          CargoItemReceiptedQty = (ladingItem.CargoItem.PurchaseOrder.PurchaseOrderSummary.ReceiptedQty * ladingItem.CargoItem.PurchaseOrder.Unit.ConversionRatio) / ladingItem.CargoItem.Unit.ConversionRatio,
          CargoItemWithoutLadingQty = Math.Round(ladingItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount) - (Math.Round((ladingItem.CargoItem.LadingItems.Sum(a => a.Qty * a.CargoItem.Unit.ConversionRatio)), ladingItem.CargoItem.Unit.DecimalDigitCount) / ladingItem.CargoItem.Unit.ConversionRatio),
          StuffValue = ladingItem.Qty * ladingItem.CargoItem.PurchaseOrder.Price,
          PurchaseOrderCurrencyId = ladingItem.CargoItem.PurchaseOrder.CurrencyId,
          PurchaseOrderCurrencyTitle = ladingItem.CargoItem.PurchaseOrder.Currency.Title,
          RowVersion = ladingItem.RowVersion,
        };
    #endregion
    #region ToFullLadingItemResult
    public IQueryable<FullLadingItemsResult> ToFullLadingItemsResultQuery(IQueryable<LadingItem> query, IQueryable<CargoItem> cargoItems, IQueryable<PurchaseOrderPlanCodeView> purchaseOrderPlanCodes
    )
    {
      var resultQuery = from ladingItem in query
                        join purchaseOrderPlanCode in purchaseOrderPlanCodes on
                          ladingItem.CargoItem.PurchaseOrderId equals purchaseOrderPlanCode.PurchaseOrderId into tPurchaseOrderWithPlanCode
                        from purchaseOrderWithPlanCode in tPurchaseOrderWithPlanCode.DefaultIfEmpty()
                        let cargoItem = ladingItem.CargoItem
                        let purchaseOrder = cargoItem.PurchaseOrder
                        let stuff = purchaseOrder.Stuff
                        select new FullLadingItemsResult
                        {
                          LadingCode = ladingItem.Lading.Code,
                          CargoItemCode = cargoItem.Code,
                          CargoItemDateTime = cargoItem.DateTime,
                          CargoItemStatus = cargoItem.Status,
                          CargoItemEstimateDateTime = cargoItem.EstimateDateTime,
                          CargoItemEmployeeFullName = cargoItem.User.Employee.FirstName + " " + cargoItem.User.Employee.LastName,
                          PurchaseOrderDeadline = purchaseOrder.Deadline,
                          PurchaseOrderCurrencyTitle = purchaseOrder.Currency.Title,
                          StuffCode = stuff.Code,
                          StuffName = stuff.Name,
                          ProviderName = purchaseOrder.Provider.Name,
                          UnitName = cargoItem.Unit.Name,
                          CargoItemQty = Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount),
                          LadingItemQty = Math.Round(ladingItem.Qty, cargoItem.Unit.DecimalDigitCount),
                          CargoCode = cargoItem.Cargo.Code,
                          PlanCode = purchaseOrderWithPlanCode.PlanCodes
                        };
      return resultQuery;
    }
    #endregion
    #region ToLadingItemWithDetailsResult
    public Expression<Func<LadingItem, FullLadingItemsResult>> ToLadingItemWithDetailsResult =
        ladingItem => new FullLadingItemsResult
        {
          LadingItemId = ladingItem.Id,
          LadingCode = ladingItem.Lading.Code,
          LadingItemDateTime = ladingItem.DateTime,
          UserId = ladingItem.UserId,
          UserFullName = ladingItem.User.Employee.FirstName + " " + ladingItem.User.Employee.LastName,
          CargoItemId = ladingItem.CargoItemId,
          CargoItemCode = ladingItem.CargoItem.Code,
          CargoItemProviderCode = ladingItem.CargoItem.PurchaseOrder.Provider.Code,
          CargoItemProviderName = ladingItem.CargoItem.PurchaseOrder.Provider.Name,
          StuffId = ladingItem.CargoItem.PurchaseOrder.Stuff.Id,
          StuffCode = ladingItem.CargoItem.PurchaseOrder.Stuff.Code,
          StuffName = ladingItem.CargoItem.PurchaseOrder.Stuff.Name,
          StuffNetWeight = ladingItem.CargoItem.PurchaseOrder.Stuff.NetWeight,
          StuffGrossWeight = ladingItem.CargoItem.PurchaseOrder.Stuff.GrossWeight,
          StuffTotalGrossWeight = ladingItem.CargoItem.PurchaseOrder.Stuff.GrossWeight * ladingItem.Qty * ladingItem.CargoItem.Unit.ConversionRatio,
          StuffPrice = ladingItem.CargoItem.PurchaseOrder.Price,
          CurrencyId = ladingItem.CargoItem.PurchaseOrder.CurrencyId,
          CurrencyTitle = ladingItem.CargoItem.PurchaseOrder.Currency.Title,
          ProviderId = ladingItem.CargoItem.PurchaseOrder.ProviderId,
          ProviderName = ladingItem.CargoItem.PurchaseOrder.Provider.Name,
          UnitId = ladingItem.CargoItem.UnitId,
          UnitName = ladingItem.CargoItem.Unit.Name,
          CargoItemQty = Math.Round(ladingItem.CargoItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount),
          LadingItemQty = Math.Round(ladingItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount),
          CargoItemReceiptedQty = (ladingItem.CargoItem.PurchaseOrder.PurchaseOrderSummary.ReceiptedQty * ladingItem.CargoItem.PurchaseOrder.Unit.ConversionRatio) / ladingItem.CargoItem.Unit.ConversionRatio,
          CargoItemWithoutLadingQty = Math.Round(ladingItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount) - (Math.Round((ladingItem.CargoItem.LadingItems.Sum(a => a.Qty * a.CargoItem.Unit.ConversionRatio)), ladingItem.CargoItem.Unit.DecimalDigitCount) / ladingItem.CargoItem.Unit.ConversionRatio),
          BankOrderId = ladingItem.Lading.BankOrderId,
          BankName = ladingItem.Lading.BankOrder.Bank.Title,
          BankOrderCode = ladingItem.Lading.BankOrder.Code,
          BankOrderNumber = ladingItem.Lading.BankOrder.OrderNumber,
          LadingId = ladingItem.LadingId,
          KotazhCode = ladingItem.Lading.KotazhCode,
          SataCode = ladingItem.Lading.SataCode,
          CustomhouseId = ladingItem.Lading.CustomhouseId,
          CustomhouseName = ladingItem.Lading.Customhouse.Title,
          TransportDate = ladingItem.Lading.TransportDateTime,
          DeliveryDate = ladingItem.Lading.DeliveryDateTime,
          DateTime = ladingItem.Lading.DateTime,
          PurchaseOrderCode = ladingItem.CargoItem.PurchaseOrder.Code,
          CargoId = ladingItem.CargoItem.CargoId,
          CargoCode = ladingItem.CargoItem.Cargo.Code,
          CurrentBankOrderStatusName = ladingItem.Lading.BankOrder.CurrentBankOrderLog.BankOrderStatusType.Name,
          CurrentBankOrderLogDescription = ladingItem.Lading.BankOrder.CurrentBankOrderLog.Description,
          CurrentCustomhouseStatusName = ladingItem.Lading.CurrentLadingCustomhouseLog.LadingCustomhouseStatus.Name,
          CurrentCustomhouseLogDescription = ladingItem.Lading.CurrentLadingCustomhouseLog.Description,
        };
    #endregion
    #region Remove LadingItem
    public void RemoveLadingItem(int id, byte[] rowVersion)
    {

      var ladingItem = GetLadingItem(id: id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: null,
                    baseEntity: ladingItem,
                    rowVersion: rowVersion);
    }
    #endregion
    #region RemoveProcess
    public void RemoveLadingItemProcess(
        int id,
        byte[] rowVersion)
    {

      #region GetLading
      var ladingItem = GetLadingItem(id: id);
      #endregion
      if ((ladingItem.Status & LadingItemStatus.Receipted) > 0)
      {
        throw new LadingItemCanNotDeleteException(ladingItem.Id);
      }
      #region RemoveLadingItem
      RemoveLadingItem(
              id: id,
              rowVersion: rowVersion);
      #endregion
      #region Remove LadingItemDetails
      var ladingItemDetails = GetLadingItemDetails(selector: e => e,
              ladingItemId: id);
      foreach (var ladingItemDetail in ladingItemDetails)
      {
        RemoveLadingItemDetail(
                      id: ladingItemDetail.Id,
                      rowVersion: ladingItemDetail.RowVersion);
      }
      #endregion
      #region Get CargoItem 
      var cargoItem = GetCargoItem(id: ladingItem.CargoItemId);
      #endregion
      #region ResetPurchaseRequestStatus
      foreach (var cargoItemDetail in cargoItem.CargoItemDetails)
      {
        App.Internals.Supplies.ResetCargoItemDetailStatus(cargoItemDetailId: cargoItemDetail.Id);
      }
      #endregion
      #region ResetCargoItemQty
      ResetCargoItemStatus(cargoItem: cargoItem);
      #endregion
    }
    #endregion
    #region SetPriceCurrencyToRialRates
    public IQueryable<DutyCostLadingItemsResult> SetPriceCurrencyToRialRates(
        IQueryable<DutyCostLadingItemsResult> purchaseOrderResultQuery,
        IQueryable<CargoItem> cargoItems)
    {

      var accounting = App.Internals.Accounting;
      var result = purchaseOrderResultQuery.AsEnumerable().Select(fullLadingItemsResult =>
            {
              var cargoItemId = fullLadingItemsResult.CargoItemId;
              if (cargoItemId != null)
              {
                var cargoItem = cargoItems.FirstOrDefault(i => i.Id == cargoItemId);
                if (cargoItem == null)
                  throw new CargoItemNotFoundException(cargoItemId.Value);
                var importToCargoFinancialTransaction =
                          cargoItem.FinancialTransactionBatch.FinancialTransactions.FirstOrDefault(i =>
                              !i.IsDelete &&
                              i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.ImportToCargo.Id);
                var rialRate = accounting.GetRialRateOfFinancialTransaction(
                              financialTransaction: importToCargoFinancialTransaction,
                              updateRialRateIsUsedState: false);
                fullLadingItemsResult.StuffPriceCurrencyToRialRate = rialRate;
              }
              return fullLadingItemsResult;
            });
      return result.AsQueryable();
    }
    #endregion
    public IQueryable<DutyCostLadingItemsResult> ToDutyCostLadingItemsResultQuery(
        IQueryable<LadingItem> ladingItems,
        IQueryable<CargoItem> cargoItems,
        IQueryable<PurchaseOrderPlanCodeView> purchaseOrderPlanCodes
        )
    {
      var resultQuery = from ladingItem in ladingItems
                        join purchaseOrderPlanCode in purchaseOrderPlanCodes on
                          ladingItem.CargoItem.PurchaseOrderId equals purchaseOrderPlanCode.PurchaseOrderId into tPurchaseOrderWithPlanCode
                        from purchaseOrderWithPlanCode in tPurchaseOrderWithPlanCode.DefaultIfEmpty()
                        select new DutyCostLadingItemsResult
                        {
                          LadingItemId = ladingItem.Id,
                          LadingCode = ladingItem.Lading.Code,
                          LadingItemDateTime = ladingItem.DateTime,
                          LadingItemStatus = ladingItem.Status,
                          UserId = ladingItem.UserId,
                          UserFullName = ladingItem.User.Employee.FirstName + " " + ladingItem.User.Employee.LastName,
                          CargoItemId = ladingItem.CargoItemId,
                          CargoItemCode = ladingItem.CargoItem.Code,
                          CargoItemProviderCode = ladingItem.CargoItem.PurchaseOrder.Provider.Code,
                          CargoItemProviderName = ladingItem.CargoItem.PurchaseOrder.Provider.Name,
                          StuffId = ladingItem.CargoItem.PurchaseOrder.Stuff.Id,
                          StuffCode = ladingItem.CargoItem.PurchaseOrder.Stuff.Code,
                          StuffName = ladingItem.CargoItem.PurchaseOrder.Stuff.Name,
                          StuffNetWeight = ladingItem.CargoItem.PurchaseOrder.Stuff.NetWeight,
                          StuffGrossWeight = ladingItem.CargoItem.PurchaseOrder.Stuff.GrossWeight,
                          StuffTotalGrossWeight = ladingItem.CargoItem.PurchaseOrder.Stuff.GrossWeight * ladingItem.Qty * ladingItem.CargoItem.Unit.ConversionRatio,
                          StuffPrice = ladingItem.CargoItem.PurchaseOrder.Price,
                          CurrencyId = ladingItem.CargoItem.PurchaseOrder.CurrencyId,
                          CurrencyTitle = ladingItem.CargoItem.PurchaseOrder.Currency.Title,
                          ProviderId = ladingItem.CargoItem.PurchaseOrder.ProviderId,
                          ProviderName = ladingItem.CargoItem.PurchaseOrder.Provider.Name,
                          UnitId = ladingItem.CargoItem.UnitId,
                          UnitName = ladingItem.CargoItem.Unit.Name,
                          CargoItemQty = Math.Round(ladingItem.CargoItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount),
                          LadingItemQty = Math.Round(ladingItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount),
                          CargoItemReceiptedQty = (ladingItem.CargoItem.PurchaseOrder.PurchaseOrderSummary.ReceiptedQty * ladingItem.CargoItem.PurchaseOrder.Unit.ConversionRatio) / ladingItem.CargoItem.Unit.ConversionRatio,
                          CargoItemWithoutLadingQty = Math.Round(ladingItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount) - (Math.Round(ladingItem.CargoItem.LadingItems.Sum(a => a.Qty * a.CargoItem.Unit.ConversionRatio), ladingItem.CargoItem.Unit.DecimalDigitCount) / ladingItem.CargoItem.Unit.ConversionRatio),
                          BankOrderId = ladingItem.Lading.BankOrderId,
                          BankName = ladingItem.Lading.BankOrder.Bank.Title,
                          BankOrderCode = ladingItem.Lading.BankOrder.Code,
                          BankOrderNumber = ladingItem.Lading.BankOrder.OrderNumber,
                          LadingId = ladingItem.LadingId,
                          KotazhCode = ladingItem.Lading.KotazhCode,
                          SataCode = ladingItem.Lading.SataCode,
                          CustomhouseId = ladingItem.Lading.CustomhouseId,
                          CustomhouseName = ladingItem.Lading.Customhouse.Title,
                          TransportDate = ladingItem.Lading.TransportDateTime,
                          DeliveryDate = ladingItem.Lading.DeliveryDateTime,
                          DateTime = ladingItem.Lading.DateTime,
                          PurchaseOrderCode = ladingItem.CargoItem.PurchaseOrder.Code,
                          CargoId = ladingItem.CargoItem.CargoId,
                          CargoCode = ladingItem.CargoItem.Cargo.Code,
                          //CurrentBankOrderStatusName = ladingItem.Lading.CurrentLadingBankOrderLog.LadingBankOrderStatus.Name,
                          //CurrentBankOrderLogDescription = ladingItem.Lading.CurrentLadingBankOrderLog.Description,
                          CurrentCustomhouseStatusName = ladingItem.Lading.CurrentLadingCustomhouseLog.LadingCustomhouseStatus.Name,
                          CurrentCustomhouseLogDescription = ladingItem.Lading.CurrentLadingCustomhouseLog.Description,
                          PlanCode = purchaseOrderWithPlanCode.PlanCodes
                        };
      return resultQuery;
    }
  }
}
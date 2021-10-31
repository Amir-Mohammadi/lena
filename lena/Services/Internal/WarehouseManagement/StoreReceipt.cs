using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
//using LinqLib.Array;
using lena.Services.Common;
using lena.Services.Common.Helpers;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.StaticData;
using lena.Models.ApplicationBase.Unit;
using lena.Models.Common;
using lena.Models.WarehouseManagement.NewShopping;
using lena.Models.WarehouseManagement.Receipt;
using lena.Models.WarehouseManagement.ReturnOfSale;
using lena.Models.WarehouseManagement.StoreReceipt;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public StoreReceipt AddStoreReceipt(
       StoreReceipt storeReceipt,
       TransactionBatch transactionBatch,
       int? receiptId,
       int cooperatorId,
       int stuffId,
       short? billOfMaterialVersion,
       double amount,
       byte unitId,
       short warehouseId,
       int inboundCargoId,
       string description,
       bool stuffNeedToQualityControl)
    {

      storeReceipt = storeReceipt ?? repository.Create<StoreReceipt>();
      if (storeReceipt is NewShopping)
        storeReceipt.StoreReceiptType = StoreReceiptType.NewShopping;
      else
        storeReceipt.StoreReceiptType = StoreReceiptType.ReturnOfSale;
      storeReceipt.ReceiptId = receiptId;
      storeReceipt.CooperatorId = cooperatorId;
      storeReceipt.StuffId = stuffId;
      storeReceipt.Amount = amount;
      storeReceipt.UnitId = unitId;
      storeReceipt.BillOfMaterialVersion = billOfMaterialVersion;
      storeReceipt.InboundCargoId = inboundCargoId;
      storeReceipt.WarehouseId = warehouseId;
      storeReceipt.StuffNeedToQualityControl = stuffNeedToQualityControl;
      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: storeReceipt,
                transactionBatch: transactionBatch,
                  description: description);
      return storeReceipt;
    }
    #endregion
    #region Edit
    public StoreReceipt EditStoreReceipt(
        int id,
        byte[] rowVersion,
        TValue<int?> receiptId = null,
        TValue<int> cooperatorId = null,
        TValue<int> stuffId = null,
        TValue<double> amount = null,
        TValue<byte> unitId = null,
        TValue<int> inboundCargoId = null,
        TValue<short> warehouseId = null,
        TValue<int?> currentPurchasePriceId = null,
        TValue<bool> isDelete = null,
        TValue<string> description = null)
    {

      var storeReceipt = GetStoreReceipt(id: id);
      return EditStoreReceipt(
                    storeReceipt: storeReceipt,
                    rowVersion: rowVersion,
                    receiptId: receiptId,
                    cooperatorId: cooperatorId,
                    stuffId: stuffId,
                    amount: amount,
                    unitId: unitId,
                    inboundCargoId: inboundCargoId,
                    warehouseId: warehouseId,
                    currentPurchasePriceId: currentPurchasePriceId,
                    isDelete: isDelete,
                    description: description);
    }
    public StoreReceipt EditStoreReceipt(
        StoreReceipt storeReceipt,
        byte[] rowVersion,
        TValue<int?> receiptId = null,
        TValue<int> cooperatorId = null,
        TValue<int> stuffId = null,
        TValue<double> amount = null,
        TValue<byte> unitId = null,
        TValue<bool> isDelete = null,
        TValue<int> inboundCargoId = null,
        TValue<short> warehouseId = null,
        TValue<int?> currentPurchasePriceId = null,
        TValue<string> description = null)
    {

      if (receiptId != null)
        storeReceipt.ReceiptId = receiptId;
      if (cooperatorId != null)
        storeReceipt.CooperatorId = cooperatorId;
      if (stuffId != null)
        storeReceipt.StuffId = stuffId;
      if (amount != null)
        storeReceipt.Amount = amount;
      if (unitId != null)
        storeReceipt.UnitId = unitId;
      if (inboundCargoId != null)
        storeReceipt.InboundCargoId = inboundCargoId;
      if (warehouseId != null)
        storeReceipt.WarehouseId = warehouseId;
      if (currentPurchasePriceId != null)
      {
        if (currentPurchasePriceId.Value == null)
          storeReceipt.CurrentPurchasePrice = null;
        else
        {
          var purchasePrice = App.Internals.Accounting
                    .GetPurchasePrice(id: currentPurchasePriceId.Value.Value);
        }
      }
      if (description != null)
        storeReceipt.Description = description;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: storeReceipt,
                    description: description,
                    isDelete: isDelete,
                    rowVersion: rowVersion);
      return retValue as StoreReceipt;
    }
    #endregion
    #region Get
    public StoreReceipt GetStoreReceipt(int id) => GetStoreReceipt(selector: e => e, id: id);
    public TResult GetStoreReceipt<TResult>(
        Expression<Func<StoreReceipt, TResult>> selector,
        int id)
    {

      var orderItemBlock = GetStoreReceipts(
                    selector: selector,
                    id: id)


                    .FirstOrDefault();
      if (orderItemBlock == null)
        throw new StoreReceiptNotFoundException(id);
      return orderItemBlock;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStoreReceipts<TResult>(
        Expression<Func<StoreReceipt, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> receiptId = null,
        TValue<int> cooperatorId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<DateTime> fromInboundCargoDateTime = null,
        TValue<DateTime> toInboundCargoDateTime = null,
        TValue<int> stuffId = null,
        TValue<double> amount = null,
        TValue<int> unitId = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<int> warehouseId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var storeReceipt = baseQuery.OfType<StoreReceipt>();
      if (receiptId != null)
        storeReceipt = storeReceipt.Where(r => r.ReceiptId == receiptId);
      if (cooperatorId != null)
        storeReceipt = storeReceipt.Where(r => r.CooperatorId == cooperatorId);
      if (stuffId != null)
        storeReceipt = storeReceipt.Where(r => r.StuffId == stuffId);
      if (amount != null)
        storeReceipt = storeReceipt.Where(r => r.Amount == amount);
      if (unitId != null)
        storeReceipt = storeReceipt.Where(r => r.UnitId == unitId);
      if (description != null)
        storeReceipt = storeReceipt.Where(i => i.Description == description);
      if (warehouseId != null)
        storeReceipt = storeReceipt.Where(r => r.WarehouseId == warehouseId);
      if (fromDateTime != null)
        storeReceipt = storeReceipt.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        storeReceipt = storeReceipt.Where(i => i.DateTime <= toDateTime);
      if (fromInboundCargoDateTime != null)
        storeReceipt = storeReceipt.Where(i => i.InboundCargo.DateTime >= fromInboundCargoDateTime);
      if (toInboundCargoDateTime != null)
        storeReceipt = storeReceipt.Where(i => i.InboundCargo.DateTime <= toInboundCargoDateTime);
      if (ids != null)
        storeReceipt = storeReceipt.Where(i => ids.Value.Contains(i.Id));
      if (fromDate != null)
        storeReceipt = storeReceipt.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        storeReceipt = storeReceipt.Where(i => i.DateTime <= toDate);
      return storeReceipt.Select(selector);
    }
    #endregion
    #region Gets ReturnOfSaleStoreReceipt
    public IQueryable<ReturnOfSaleStoreReceiptResult> GetReturnOfSaleStoreReceipts(
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> receiptId = null,
        TValue<int> cooperatorId = null,
        TValue<int> stuffId = null,
        TValue<double> amount = null,
        TValue<int> unitId = null,
        TValue<int> warehouseId = null)
    {

      var storeReceipts = App.Internals.WarehouseManagement.GetStoreReceipts(e => e, isDelete: false);
      var returnOfSales = App.Internals.WarehouseManagement.GetReturnOfSales(e => e);
      var Stuffs = App.Internals.SaleManagement.GetStuffs(e => e); ; var unitTypes = App.Internals.ApplicationBase.GetUnitTypes();
      var mainUnits = App.Internals.ApplicationBase.GetUnits(e => e,
                 isMainUnit: true);
      var stuffMainUnit =
                        from stuff in Stuffs
                        join unitType in unitTypes on stuff.UnitTypeId equals unitType.Id
                        join mainUnit in mainUnits on unitType.Id equals mainUnit.UnitTypeId
                        select new StuffMainUnit()
                        {
                          StuffId = stuff.Id,
                          UnitId = mainUnit.Id,
                          UnitName = mainUnit.Name,
                          UnitConversionRatio = mainUnit.ConversionRatio
                        };
      var query = from storeReceipt in storeReceipts
                  join returnOfSale in returnOfSales on storeReceipt.Id equals returnOfSale.ReturnStoreReceiptId
                  join q in stuffMainUnit on storeReceipt.StuffId equals q.StuffId
                  join mainstuff in Stuffs on returnOfSale.MainStuffId equals mainstuff.Id into returnmainstuff
                  from mstuff in returnmainstuff.DefaultIfEmpty()
                  select new ReturnOfSaleStoreReceiptResult
                  {
                    ReceiptId = storeReceipt.ReceiptId,
                    ReceiptStatus = storeReceipt.Receipt.Status,
                    WarehouseId = storeReceipt.WarehouseId,
                    WarehouseName = storeReceipt.Warehouse.Name,
                    EmployeeFullName = storeReceipt.User.Employee.FirstName + " " + storeReceipt.User.Employee.LastName,
                    StoreReceiptId = storeReceipt.Id,
                    StoreReceiptCode = storeReceipt.Code,
                    StoreReceiptStuffId = storeReceipt.Stuff.Id,
                    StoreReceiptStuffCode = storeReceipt.Stuff.Code,
                    StoreReceiptStuffName = storeReceipt.Stuff.Name,
                    StoreReceiptAmount = (storeReceipt.Amount * storeReceipt.Unit.ConversionRatio) / q.UnitConversionRatio,
                    StoreReceiptDateTime = storeReceipt.DateTime,
                    StoreReceiptType = storeReceipt.StoreReceiptType,
                    ReceiptDateTime = storeReceipt.Receipt.DateTime,
                    ReceiptReceiptDateTime = storeReceipt.Receipt.ReceiptDateTime,
                    UnitId = q.UnitId,
                    UnitName = q.UnitName,
                    CooperatorId = storeReceipt.CooperatorId,
                    CooperatorName = storeReceipt.Cooperator.Name,
                    ReceiptQualityControlPassedQty = (storeReceipt.StoreReceiptSummary.ReceiptQualityControlPassedQty * storeReceipt.Unit.ConversionRatio) / q.UnitConversionRatio,
                    ReceiptQualityControlConsumedQty = (storeReceipt.StoreReceiptSummary.ReceiptQualityControlConsumedQty * storeReceipt.Unit.ConversionRatio) / q.UnitConversionRatio,
                    ReceiptQualityControlFailedQty = (storeReceipt.StoreReceiptSummary.ReceiptQualityControlFailedQty * storeReceipt.Unit.ConversionRatio) / q.UnitConversionRatio,
                    ReturnOfSaleMainStuffId = returnOfSale.MainStuffId,
                    ReturnOfSaleStuffId = returnOfSale.StuffId,
                    ReturnOfSaleStuffCode = returnOfSale.Stuff.Code,
                    ReturnOfSaleStuffName = returnOfSale.Stuff.Name,
                    ReturnOfSaleQty = (returnOfSale.Qty * returnOfSale.Unit.ConversionRatio) / q.UnitConversionRatio,
                    SendProductId = returnOfSale.SendProductId,
                    SendProductCode = returnOfSale.SendProduct.Code,
                    Serial = returnOfSale.StuffSerial.Serial,
                    ReturnOfSaleId = returnOfSale.Id,
                    ExitReceiptId = returnOfSale.SendProduct.ExitReceiptId,
                    ExitReceiptDateTime = returnOfSale.SendProduct.ExitReceipt.DateTime,
                    ExitReceiptStuffId = returnOfSale.MainStuffId ?? returnOfSale.SendProduct.PreparingSending.SendPermission.ExitReceiptRequest.StuffId,
                    ExitReceiptStuffCode = returnOfSale.MainStuffId != null ? mstuff.Code : returnOfSale.SendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Stuff.Code,
                    ExitReceiptStuffName = returnOfSale.MainStuffId != null ? mstuff.Name : returnOfSale.SendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Stuff.Name,
                    ExitReceiptCode = returnOfSale.ExitReceiptCode,
                    ExitReceiptQty = ((returnOfSale.SendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Qty * returnOfSale.SendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Unit.ConversionRatio) / q.UnitConversionRatio) == null
                            ? returnOfSale.StuffSerial.InitQty
                            : (returnOfSale.SendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Qty * returnOfSale.SendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Unit.ConversionRatio) / q.UnitConversionRatio,
                    ReturnOfSaleQualityControlConsumedQty = returnOfSale.ReturnOfSaleSummary.ReceiptQualityControlConsumedQty,
                    ReturnOfSaleQualityControlFailedQty = returnOfSale.ReturnOfSaleSummary.ReceiptQualityControlFailedQty,
                    ReturnOfSaleQualityControlPassedQty = returnOfSale.ReturnOfSaleSummary.ReceiptQualityControlPassedQty,
                  };
      if (receiptId != null)
        query = query.Where(r => r.ReceiptId == receiptId);
      if (cooperatorId != null)
        query = query.Where(r => r.CooperatorId == cooperatorId);
      if (unitId != null)
        query = query.Where(r => r.UnitId == unitId);
      if (warehouseId != null)
        query = query.Where(r => r.WarehouseId == warehouseId);
      return query;
    }
    #endregion
    #region Search
    public IQueryable<StoreReceiptResult> SearchStoreReceiptResult(
        IQueryable<StoreReceiptResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText,
        string inboundCargoCode,
        StoreReceiptType? storeReceiptType,
        string purchaseOrderCode,
        string cargoCode,
        string cargoItemCode,
        int? cargoItemId,
        string cooperatorName,
        string receiptCode,
        TValue<ReceiptStatus[]> receiptStatuses,
        TValue<ReceiptStatus[]> receiptNotHasStatuses,
        TValue<ReceiptStatus> receiptStatuse)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = query.Where(item =>
            item.Code.Contains(searchText) ||
            item.PurchaseOrderCode.Contains(searchText) ||
            item.CooperatorName.Contains(searchText) ||
            item.CargoCode.Contains(searchText) ||
            item.StuffCode.Contains(searchText) ||
            item.StuffNoun.Contains(searchText) ||
            item.StuffName.Contains(searchText));
      if (!string.IsNullOrWhiteSpace(inboundCargoCode))
        query = query.Where(i => i.InboundCargoCode == inboundCargoCode);
      if (storeReceiptType != null) query =
              query.Where(i => i.StoreReceiptType == storeReceiptType);
      if (!string.IsNullOrWhiteSpace(purchaseOrderCode))
        query = query.Where(i => i.PurchaseOrderCode == purchaseOrderCode);
      if (!string.IsNullOrWhiteSpace(cargoCode))
        query = query.Where(i => i.CargoCode == cargoCode);
      if (cargoItemCode != null)
        query = query.Where(i => i.CargoItemCode == cargoItemCode);
      if (cargoItemId != null)
        query = query.Where(i => i.CargoItemId == cargoItemId);
      if (!string.IsNullOrWhiteSpace(cooperatorName))
        query = query.Where(i => i.CooperatorName == cooperatorName);
      if (receiptStatuse != null)
        query = query.Where(i => i.ReceiptStatus.HasFlag(receiptStatuse));
      if (receiptStatuses != null)
      {
        var s = ReceiptStatus.None;
        foreach (var item in receiptStatuses.Value)
          s = s | item;
        query = query.Where(i => (i.ReceiptStatus & s) > 0);
      }
      if (receiptNotHasStatuses != null)
      {
        var s = ReceiptStatus.None;
        foreach (var item in receiptNotHasStatuses.Value)
          s = s | item;
        query = query.Where(i => (i.ReceiptStatus & s) == 0);
      }
      if (!string.IsNullOrWhiteSpace(receiptCode))
        query = query.Where(i => i.ReceiptCode == receiptCode);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    public IQueryable<ReturnOfSaleStoreReceiptResult> SearchReturnOfSaleStoreReceiptResult(
      IQueryable<ReturnOfSaleStoreReceiptResult> query,
      string search,
      string inboundCargoCode,
      StoreReceiptType? storeReceiptType,
      string purchaseOrderCode,
      string cargoCode,
      string cooperatorName,
      DateTime? fromDate,
      DateTime? toDate,
      string receiptCode,
      TValue<ReceiptStatus[]> receiptStatuses,
      TValue<ReceiptStatus[]> receiptNotHasStatuses,
      TValue<ReceiptStatus> receiptStatuse)
    {
      if (!string.IsNullOrEmpty(search))
        query = query.Where(item =>
            item.CooperatorName.Contains(search));
      if (!string.IsNullOrWhiteSpace(cooperatorName))
        query = query.Where(i => i.CooperatorName == cooperatorName);
      if (receiptStatuses != null)
      {
        var s = ReceiptStatus.None;
        foreach (var item in receiptStatuses.Value)
          s = s | item;
        query = query.Where(i => (i.ReceiptStatus & s) > 0);
      }
      if (receiptNotHasStatuses != null)
      {
        var s = ReceiptStatus.None;
        foreach (var item in receiptNotHasStatuses.Value)
          s = s | item;
        query = query.Where(i => (i.ReceiptStatus & s) == 0);
      }
      return query;
    }
    public IQueryable<StoreReceiptComboResult> SearchStoreReceiptComboResult(
       IQueryable<StoreReceiptComboResult> query,
       TValue<ReceiptStatus[]> receiptStatuses,
       TValue<ReceiptStatus[]> receiptNotHasStatuses,
       TValue<ReceiptStatus> receiptStatuse,
       string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = query.Where(item =>
            item.Code.Contains(search) ||
            item.ReceiptCode.Contains(search));
      if (receiptStatuse != null)
        query = query.Where(i => i.ReceiptStatus.HasFlag(receiptStatuse));
      if (receiptStatuses != null)
      {
        var s = ReceiptStatus.None;
        foreach (var item in receiptStatuses.Value)
          s = s | item;
        query = query.Where(i => (i.ReceiptStatus & s) > 0);
      }
      if (receiptNotHasStatuses != null)
      {
        var s = ReceiptStatus.None;
        foreach (var item in receiptNotHasStatuses.Value)
          s = s | item;
        query = query.Where(i => (i.ReceiptStatus & s) == 0);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StoreReceiptResult> SortStoreReceiptResult(
        IQueryable<StoreReceiptResult> query,
        SortInput<StoreReceiptSortType> sort)
    {
      switch (sort.SortType)
      {
        case StoreReceiptSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case StoreReceiptSortType.InboundCargoCode:
          return query.OrderBy(a => a.InboundCargoCode, sort.SortOrder);
        case StoreReceiptSortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case StoreReceiptSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case StoreReceiptSortType.StoreReceiptType:
          return query.OrderBy(a => a.StoreReceiptType, sort.SortOrder);
        case StoreReceiptSortType.CargoCode:
          return query.OrderBy(a => a.CargoCode, sort.SortOrder);
        case StoreReceiptSortType.PurchaseOrderCode:
          return query.OrderBy(a => a.PurchaseOrderCode, sort.SortOrder);
        case StoreReceiptSortType.Amount:
          return query.OrderBy(a => a.Amount, sort.SortOrder);
        case StoreReceiptSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case StoreReceiptSortType.BoxNo:
          return query.OrderBy(a => a.BoxNo, sort.SortOrder);
        case StoreReceiptSortType.QtyPerBox:
          return query.OrderBy(a => a.QtyPerBox, sort.SortOrder);
        case StoreReceiptSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case StoreReceiptSortType.TransportDateTime:
          return query.OrderBy(a => a.TransportDateTime, sort.SortOrder);
        case StoreReceiptSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case StoreReceiptSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case StoreReceiptSortType.ReceiptCode:
          return query.OrderBy(a => a.ReceiptCode, sort.SortOrder);
        case StoreReceiptSortType.ReceiptDateTime:
          return query.OrderBy(a => a.ReceiptDateTime, sort.SortOrder);
        case StoreReceiptSortType.Status:
          return query.OrderBy(a => a.ReceiptStatus, sort.SortOrder);
        case StoreReceiptSortType.QualityControlPassedQty:
          return query.OrderBy(a => a.QualityControlPassedQty, sort.SortOrder);
        case StoreReceiptSortType.QualityControlConsumedQty:
          return query.OrderBy(a => a.QualityControlConsumedQty, sort.SortOrder);
        case StoreReceiptSortType.QualityControlFailedQty:
          return query.OrderBy(a => a.QualityControlFailedQty, sort.SortOrder);
        case StoreReceiptSortType.ReceiptQualityControlPassedQty:
          return query.OrderBy(a => a.ReceiptQualityControlPassedQty, sort.SortOrder);
        case StoreReceiptSortType.ReceiptQualityControlFailedQty:
          return query.OrderBy(a => a.ReceiptQualityControlFailedQty, sort.SortOrder);
        case StoreReceiptSortType.ReceiptQualityControlConsumedQty:
          return query.OrderBy(a => a.ReceiptQualityControlConsumedQty, sort.SortOrder);
        case StoreReceiptSortType.SumOfPayRequestPayedAmounts:
          return query.OrderBy(a => a.SumOfPayRequestPayedAmounts, sort.SortOrder);
        case StoreReceiptSortType.SumOfPayRequestPayedAmountsCurrencyTitle:
          return query.OrderBy(a => a.SumOfPayRequestPayedAmountsCurrencyTitle, sort.SortOrder);
        case StoreReceiptSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case StoreReceiptSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<ReturnOfSaleStoreReceiptResult> SortReturnOfSaleStoreReceiptResult(
       IQueryable<ReturnOfSaleStoreReceiptResult> query,
       SortInput<ReturnOfSaleStoreReceiptSortType> sort)
    {
      switch (sort.SortType)
      {
        case ReturnOfSaleStoreReceiptSortType.ReceiptQualityControlPassedQty:
          return query.OrderBy(a => a.ReceiptQualityControlPassedQty, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ReceiptQualityControlFailedQty:
          return query.OrderBy(a => a.ReceiptQualityControlFailedQty, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ReceiptQualityControlConsumedQty:
          return query.OrderBy(a => a.ReceiptQualityControlConsumedQty, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ExitReceiptId:
          return query.OrderBy(a => a.ExitReceiptId, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ReceiptId:
          return query.OrderBy(a => a.ReceiptId, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.WarehouseId:
          return query.OrderBy(a => a.WarehouseId, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.StoreReceiptId:
          return query.OrderBy(a => a.StoreReceiptId, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.StoreReceiptCode:
          return query.OrderBy(a => a.StoreReceiptCode, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.StoreReceiptStuffId:
          return query.OrderBy(a => a.StoreReceiptStuffId, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.StoreReceiptStuffCode:
          return query.OrderBy(a => a.StoreReceiptStuffCode, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.StoreReceiptStuffName:
          return query.OrderBy(a => a.StoreReceiptStuffName, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.StoreReceiptDateTime:
          return query.OrderBy(a => a.StoreReceiptDateTime, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ReceiptDateTime:
          return query.OrderBy(a => a.ReceiptDateTime, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ReceiptReceiptDateTime:
          return query.OrderBy(a => a.ReceiptReceiptDateTime, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.StoreReceiptAmount:
          return query.OrderBy(a => a.StoreReceiptAmount, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.UnitId:
          return query.OrderBy(a => a.UnitId, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.CooperatorId:
          return query.OrderBy(a => a.CooperatorId, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ReturnOfSaleStuffId:
          return query.OrderBy(a => a.ReturnOfSaleStuffId, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ReturnOfSaleStuffCode:
          return query.OrderBy(a => a.ReturnOfSaleStuffCode, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ReturnOfSaleStuffName:
          return query.OrderBy(a => a.ReturnOfSaleStuffName, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ReturnOfSaleQty:
          return query.OrderBy(a => a.ReturnOfSaleQty, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.SendProductId:
          return query.OrderBy(a => a.SendProductId, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.SendProductCode:
          return query.OrderBy(a => a.SendProductCode, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ExitReceiptStuffId:
          return query.OrderBy(a => a.ExitReceiptStuffId, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ExitReceiptStuffCode:
          return query.OrderBy(a => a.ReturnOfSaleStuffCode, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ExitReceiptStuffName:
          return query.OrderBy(a => a.ExitReceiptStuffName, sort.SortOrder);
        case ReturnOfSaleStoreReceiptSortType.ExitReceiptQty:
          return query.OrderBy(a => a.ExitReceiptQty, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<StoreReceiptComboResult> SortStoreReceiptComboResult(
       IQueryable<StoreReceiptComboResult> query,
       SortInput<StoreReceiptSortType> sort)
    {
      switch (sort.SortType)
      {
        case StoreReceiptSortType.ReceiptCode:
          return query.OrderBy(a => a.ReceiptCode, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    public IQueryable<StoreReceiptResult> ToStoreReceiptResult(IQueryable<StoreReceipt> storeReceiptQuery)
    {
      var newShoppingQuery = storeReceiptQuery.OfType<NewShopping>();
      var returnStoreReceiptQuery = storeReceiptQuery.OfType<ReturnStoreReceipt>();
      var result = from storeReceipt in storeReceiptQuery
                   join tempNewShopping in newShoppingQuery on storeReceipt.Id equals tempNewShopping.Id into tNewShoppings
                   from newShopping in tNewShoppings.DefaultIfEmpty()
                   join tempReturnStoreReceipt in returnStoreReceiptQuery on storeReceipt.Id equals tempReturnStoreReceipt.Id into tReturnStoreReceipts
                   from returnStoreReceipt in tReturnStoreReceipts.DefaultIfEmpty()
                   select new StoreReceiptResult
                   {
                     Id = storeReceipt.Id,
                     StoreReceiptType = storeReceipt.StoreReceiptType,
                     Code = storeReceipt.Code,
                     ReceiptId = storeReceipt.ReceiptId,
                     ReceiptCode = storeReceipt.Receipt.Code,
                     ReceiptDateTime = storeReceipt.Receipt.DateTime,
                     ReceiptReceiptDateTime = storeReceipt.Receipt.ReceiptDateTime,
                     ReceiptStatus = storeReceipt.ReceiptId == null ? ReceiptStatus.NoReceipt : storeReceipt.Receipt.Status,
                     ReceiptUserId = storeReceipt.Receipt.UserId,
                     EmployeeFullName = storeReceipt.Receipt.User.Employee.FirstName + "  " + storeReceipt.Receipt.User.Employee.LastName,
                     WarehouseId = storeReceipt.WarehouseId,
                     WarehouseName = storeReceipt.Warehouse.Name,
                     InboundCargoId = storeReceipt.InboundCargoId,
                     InboundCargoCode = storeReceipt.InboundCargo.Code,
                     InboundCargoDateTime = storeReceipt.InboundCargo.DateTime,
                     TransportDateTime = storeReceipt.InboundCargo.TransportDateTime,
                     StuffId = storeReceipt.StuffId,
                     StuffName = storeReceipt.Stuff.Name,
                     StuffCode = storeReceipt.Stuff.Code,
                     StuffNoun = storeReceipt.Stuff.Noun,
                     Amount = storeReceipt.Amount,
                     UnitId = storeReceipt.UnitId,
                     UnitName = storeReceipt.Unit.Name,
                     BoxNo = newShopping.BoxNo,
                     QtyPerBox = newShopping.QtyPerBox,
                     DateTime = storeReceipt.DateTime,
                     SumOfPayRequestPayedAmounts = storeReceipt.ReceiptQualityControls.Sum(i => i.PayRequest.PayedAmount),
                     PurchaseOrderId = newShopping.LadingItem.CargoItem.PurchaseOrderId,
                     PurchaseOrderCode = newShopping.LadingItem.CargoItem.PurchaseOrder.Code,
                     CargoId = newShopping.LadingItem.CargoItem.CargoId,
                     CargoCode = newShopping.LadingItem.CargoItem.Cargo.Code,
                     CargoItemId = newShopping.LadingItem.CargoItemId,
                     CargoItemCode = newShopping.LadingItem.CargoItem.Code,
                     LadingItemId = newShopping.LadingItem.Id,
                     LadingId = newShopping.LadingItem.LadingId,
                     LadingCode = newShopping.LadingItem.Lading.Code,
                     CooperatorName = storeReceipt.Cooperator.Name,
                     CooperatorId = storeReceipt.CooperatorId,
                     CurrentPurchasePriceId = (int?)storeReceipt.CurrentPurchasePrice.Id,
                     CurrentPurchasePriceRowVersion = (byte[])storeReceipt.CurrentPurchasePrice.RowVersion,
                     Price = (double?)storeReceipt.CurrentPurchasePrice.Price,
                     EstimatedPrice = newShopping.LadingItem.CargoItem.PurchaseOrder.Price,
                     CurrencyRate = (double?)storeReceipt.CurrentPurchasePrice.CurrencyRate,
                     CurrencyId = (int?)storeReceipt.CurrentPurchasePrice.CurrencyId,
                     CurrencyTitle = storeReceipt.CurrentPurchasePrice.Currency.Title,
                     EstimatedCurrencyId = newShopping.LadingItem.CargoItem.PurchaseOrder.CurrencyId,
                     EstimatedCurrencyTitle = newShopping.LadingItem.CargoItem.PurchaseOrder.Currency.Title,
                     QualityControlPassedQty = storeReceipt.StoreReceiptSummary.QualityControlPassedQty,
                     QualityControlFailedQty = storeReceipt.StoreReceiptSummary.QualityControlFailedQty,
                     QualityControlConsumedQty = storeReceipt.StoreReceiptSummary.QualityControlConsumedQty,
                     ReceiptQualityControlPassedQty = storeReceipt.StoreReceiptSummary.ReceiptQualityControlPassedQty,
                     ReceiptQualityControlFailedQty = storeReceipt.StoreReceiptSummary.ReceiptQualityControlFailedQty,
                     ReceiptQualityControlConsumedQty = storeReceipt.StoreReceiptSummary.ReceiptQualityControlConsumedQty,
                     StuffNeedToQualityControl = storeReceipt.StuffNeedToQualityControl,
                     Description = returnStoreReceipt.Description,
                     RowVersion = storeReceipt.RowVersion
                   };
      return result;
    }
    public IQueryable<StoreReceiptResult> ToStoreReceiptResultPrint(IQueryable<StoreReceipt> storeReceiptQuery)
    {

      var newShoppingQuery = storeReceiptQuery.OfType<NewShopping>();
      var returnStoreReceiptQuery = storeReceiptQuery.OfType<ReturnStoreReceipt>();
      var documents = App.Internals.ApplicationBase.GetDocuments();
      var result = from storeReceipt in storeReceiptQuery
                   join tempNewShopping in newShoppingQuery on storeReceipt.Id equals tempNewShopping.Id into tNewShoppings
                   from newShopping in tNewShoppings.DefaultIfEmpty()
                   join tempReturnStoreReceipt in returnStoreReceiptQuery on storeReceipt.Id equals tempReturnStoreReceipt.Id into tReturnStoreReceipts
                   from returnStoreReceipt in tReturnStoreReceipts.DefaultIfEmpty()
                   join doc in documents on storeReceipt.Receipt.User.Employee.DocumentId equals doc.Id into tDocs
                   from documnet in tDocs.DefaultIfEmpty()
                   select new StoreReceiptResult
                   {
                     Id = storeReceipt.Id,
                     StoreReceiptType = storeReceipt.StoreReceiptType,
                     Code = storeReceipt.Code,
                     ReceiptId = storeReceipt.ReceiptId,
                     ReceiptCode = storeReceipt.Receipt.Code,
                     ReceiptDateTime = storeReceipt.Receipt.DateTime,
                     ReceiptReceiptDateTime = storeReceipt.Receipt.ReceiptDateTime,
                     ReceiptStatus = storeReceipt.ReceiptId == null ? ReceiptStatus.NoReceipt : storeReceipt.Receipt.Status,
                     ReceiptUserId = storeReceipt.Receipt.UserId,
                     EmployeeFullName = storeReceipt.Receipt.User.Employee.FirstName + "  " + storeReceipt.Receipt.User.Employee.LastName,
                     WarehouseId = storeReceipt.WarehouseId,
                     WarehouseName = storeReceipt.Warehouse.Name,
                     InboundCargoId = storeReceipt.InboundCargoId,
                     InboundCargoCode = storeReceipt.InboundCargo.Code,
                     InboundCargoDateTime = storeReceipt.InboundCargo.DateTime,
                     TransportDateTime = storeReceipt.InboundCargo.TransportDateTime,
                     UserSignature = documnet.FileStream,
                     StuffId = storeReceipt.StuffId,
                     StuffName = storeReceipt.Stuff.Name,
                     StuffCode = storeReceipt.Stuff.Code,
                     StuffNoun = storeReceipt.Stuff.Noun,
                     Amount = storeReceipt.Amount,
                     UnitId = storeReceipt.UnitId,
                     UnitName = storeReceipt.Unit.Name,
                     BoxNo = newShopping.BoxNo,
                     QtyPerBox = newShopping.QtyPerBox,
                     DateTime = storeReceipt.DateTime,
                     SumOfPayRequestPayedAmounts = storeReceipt.ReceiptQualityControls.Sum(i => i.PayRequest.PayedAmount),
                     PurchaseOrderId = newShopping.LadingItem.CargoItem.PurchaseOrderId,
                     PurchaseOrderCode = newShopping.LadingItem.CargoItem.PurchaseOrder.Code,
                     CargoId = newShopping.LadingItem.CargoItem.CargoId,
                     CargoCode = newShopping.LadingItem.CargoItem.Cargo.Code,
                     CargoItemId = newShopping.LadingItem.CargoItemId,
                     CargoItemCode = newShopping.LadingItem.CargoItem.Code,
                     LadingItemId = newShopping.LadingItem.Id,
                     LadingId = newShopping.LadingItem.LadingId,
                     LadingCode = newShopping.LadingItem.Lading.Code,
                     CooperatorName = storeReceipt.Cooperator.Name,
                     CooperatorId = storeReceipt.CooperatorId,
                     CurrentPurchasePriceId = (int?)storeReceipt.CurrentPurchasePrice.Id,
                     CurrentPurchasePriceRowVersion = (byte[])storeReceipt.CurrentPurchasePrice.RowVersion,
                     Price = (double?)storeReceipt.CurrentPurchasePrice.Price,
                     EstimatedPrice = newShopping.LadingItem.CargoItem.PurchaseOrder.Price,
                     CurrencyRate = (double?)storeReceipt.CurrentPurchasePrice.CurrencyRate,
                     CurrencyId = (int?)storeReceipt.CurrentPurchasePrice.CurrencyId,
                     CurrencyTitle = storeReceipt.CurrentPurchasePrice.Currency.Title,
                     EstimatedCurrencyId = newShopping.LadingItem.CargoItem.PurchaseOrder.CurrencyId,
                     EstimatedCurrencyTitle = newShopping.LadingItem.CargoItem.PurchaseOrder.Currency.Title,
                     QualityControlPassedQty = storeReceipt.StoreReceiptSummary.QualityControlPassedQty,
                     QualityControlFailedQty = storeReceipt.StoreReceiptSummary.QualityControlFailedQty,
                     QualityControlConsumedQty = storeReceipt.StoreReceiptSummary.QualityControlConsumedQty,
                     ReceiptQualityControlPassedQty = storeReceipt.StoreReceiptSummary.ReceiptQualityControlPassedQty,
                     ReceiptQualityControlFailedQty = storeReceipt.StoreReceiptSummary.ReceiptQualityControlFailedQty,
                     ReceiptQualityControlConsumedQty = storeReceipt.StoreReceiptSummary.ReceiptQualityControlConsumedQty,
                     StuffNeedToQualityControl = storeReceipt.StuffNeedToQualityControl,
                     Description = returnStoreReceipt.Description,
                     RowVersion = storeReceipt.RowVersion
                   };
      return result;
    }
    //public IQueryable<ReturnOfSaleStoreReceiptResult> ToReturnOfSaleStoreReceiptResult(IQueryable<StoreReceipt> storeReceiptQuery)
    //{
    //    var newShoppingQuery = storeReceiptQuery.OfType<NewShopping>();
    //    var returnStoreReceiptQuery = storeReceiptQuery.OfType<ReturnStoreReceipt>();
    //    var result = from storeReceipt in storeReceiptQuery
    //                 join tempNewShopping in newShoppingQuery on storeReceipt.Id equals tempNewShopping.Id into tNewShoppings
    //                 from newShopping in tNewShoppings.DefaultIfEmpty()
    //                 join tempReturnStoreReceipt in returnStoreReceiptQuery on storeReceipt.Id equals tempReturnStoreReceipt.Id into tReturnStoreReceipts
    //                 from returnStoreReceipt in tReturnStoreReceipts.DefaultIfEmpty()
    //                 select new ReturnOfSaleStoreReceiptResult
    //                 {
    //                     Id = storeReceipt.Id,
    //                     StoreReceiptType = storeReceipt.StoreReceiptType,
    //                     Code = storeReceipt.Code,
    //                     ReceiptId = storeReceipt.ReceiptId,
    //                     ReceiptCode = storeReceipt.Receipt.Code,
    //                     ReceiptDateTime = storeReceipt.Receipt.DateTime,
    //                     ReceiptReceiptDateTime = storeReceipt.Receipt.ReceiptDateTime,
    //                     ReceiptStatus = storeReceipt.ReceiptId == null ? ReceiptStatus.NoReceipt : storeReceipt.Receipt.Status,
    //                     ReceiptUserId = storeReceipt.Receipt.UserId,
    //                     EmployeeFullName = storeReceipt.Receipt.User.Employee.FirstName + "  " + storeReceipt.Receipt.User.Employee.LastName,
    //                     WarehouseId = storeReceipt.WarehouseId,
    //                     WarehouseName = storeReceipt.Warehouse.Name,
    //                     InboundCargoId = storeReceipt.InboundCargoId,
    //                     InboundCargoCode = storeReceipt.InboundCargo.Code,
    //                     TransportDateTime = storeReceipt.InboundCargo.TransportDateTime,
    //                     StuffId = storeReceipt.StuffId,
    //                     StuffName = storeReceipt.Stuff.Name,
    //                     StuffCode = storeReceipt.Stuff.Code,
    //                     StuffNoun = storeReceipt.Stuff.Noun,
    //                     Amount = storeReceipt.Amount,
    //                     UnitId = storeReceipt.UnitId,
    //                     UnitName = storeReceipt.Unit.Name,
    //                     BoxNo = newShopping.BoxNo,
    //                     QtyPerBox = newShopping.QtyPerBox,
    //                     DateTime = storeReceipt.DateTime,
    //                     PurchaseOrderId = newShopping.CargoItem.PurchaseOrderId,
    //                     PurchaseOrderCode = newShopping.CargoItem.PurchaseOrder.Code,
    //                     CargoId = newShopping.CargoItem.CargoId,
    //                     CargoCode = newShopping.CargoItem.Cargo.Code,
    //                     CargoItemId = newShopping.CargoItemId,
    //                     CargoItemCode = newShopping.CargoItem.Code,
    //                     CooperatorName = storeReceipt.Cooperator.Name,
    //                     CooperatorId = storeReceipt.CooperatorId,
    //                     CurrentPurchasePriceId = (int?)storeReceipt.CurrentPurchasePrice.Id,
    //                     CurrentPurchasePriceRowVersion = (byte[])storeReceipt.CurrentPurchasePrice.RowVersion,
    //                     Price = (double?)storeReceipt.CurrentPurchasePrice.Price,
    //                     EstimatedPrice = newShopping.CargoItem.PurchaseOrder.Price,
    //                     CurrencyRate = (double?)storeReceipt.CurrentPurchasePrice.CurrencyRate,
    //                     CurrencyId = (int?)storeReceipt.CurrentPurchasePrice.CurrencyId,
    //                     CurrencyTitle = storeReceipt.CurrentPurchasePrice.Currency.Title,
    //                     EstimatedCurrencyTitle = newShopping.CargoItem.PurchaseOrder.Currency.Title,
    //                     QualityControlPassedQty = storeReceipt.StoreReceiptSummary.QualityControlPassedQty,
    //                     QualityControlFailedQty = storeReceipt.StoreReceiptSummary.QualityControlFailedQty,
    //                     QualityControlConsumedQty = storeReceipt.StoreReceiptSummary.QualityControlConsumedQty,
    //                     ReceiptQualityControlPassedQty = storeReceipt.StoreReceiptSummary.ReceiptQualityControlPassedQty,
    //                     ReceiptQualityControlFailedQty = storeReceipt.StoreReceiptSummary.ReceiptQualityControlFailedQty,
    //                     ReceiptQualityControlConsumedQty = storeReceipt.StoreReceiptSummary.ReceiptQualityControlConsumedQty,
    //                     StuffNeedToQualityControl = storeReceipt.StuffNeedToQualityControl,
    //                     RowVersion = storeReceipt.RowVersion
    //                 };
    //    return result;
    //}
    public Expression<Func<NewShopping, StoreReceiptResult>> NewShoppingToStoreReceiptResult =
        newShopping => new StoreReceiptResult
        {
          Id = newShopping.Id,
          StoreReceiptType = newShopping.StoreReceiptType,
          Code = newShopping.Code,
          ReceiptId = newShopping.ReceiptId,
          ReceiptCode = newShopping.Receipt.Code,
          ReceiptDateTime = newShopping.Receipt.DateTime,
          ReceiptStatus = newShopping.ReceiptId == null ? ReceiptStatus.NoReceipt : newShopping.Receipt.Status,
          ReceiptUserId = newShopping.Receipt.UserId,
          EmployeeFullName = newShopping.Receipt.User.Employee.FirstName + "  " + newShopping.Receipt.User.Employee.LastName,
          WarehouseId = newShopping.WarehouseId,
          WarehouseName = newShopping.Warehouse.Name,
          InboundCargoId = newShopping.InboundCargoId,
          InboundCargoCode = newShopping.InboundCargo.Code,
          InboundCargoDateTime = newShopping.InboundCargo.DateTime,
          TransportDateTime = newShopping.InboundCargo.TransportDateTime,
          StuffId = newShopping.StuffId,
          StuffName = newShopping.Stuff.Name,
          StuffCode = newShopping.Stuff.Code,
          StuffNoun = newShopping.Stuff.Noun,
          Amount = newShopping.Amount,
          UnitId = newShopping.UnitId,
          UnitName = newShopping.Unit.Name,
          BoxNo = newShopping.BoxNo,
          QtyPerBox = newShopping.QtyPerBox,
          DateTime = newShopping.DateTime,
          PurchaseOrderId = newShopping.LadingItem.CargoItem.PurchaseOrderId,
          PurchaseOrderCode = newShopping.LadingItem.CargoItem.PurchaseOrder.Code,
          CargoId = newShopping.LadingItem.CargoItem.CargoId,
          CargoCode = newShopping.LadingItem.CargoItem.Cargo.Code,
          CargoItemId = newShopping.LadingItem.CargoItemId,
          CargoItemCode = newShopping.LadingItem.CargoItem.Code,
          CooperatorName = newShopping.LadingItem.CargoItem.PurchaseOrder.Provider.Name,
          CooperatorId = newShopping.LadingItem.CargoItem.PurchaseOrder.Provider.Id,
          CurrentPurchasePriceId = (int?)newShopping.CurrentPurchasePrice.Id,
          CurrentPurchasePriceRowVersion = (byte[])newShopping.CurrentPurchasePrice.RowVersion,
          Price = (double?)newShopping.CurrentPurchasePrice.Price,
          EstimatedPrice = newShopping.LadingItem.CargoItem.PurchaseOrder.Price,
          CurrencyRate = (double?)newShopping.CurrentPurchasePrice.CurrencyRate,
          CurrencyId = (int?)newShopping.CurrentPurchasePrice.CurrencyId,
          CurrencyTitle = newShopping.CurrentPurchasePrice.Currency.Title,
          EstimatedCurrencyTitle = newShopping.LadingItem.CargoItem.PurchaseOrder.Currency.Title,
          QualityControlPassedQty = newShopping.StoreReceiptSummary.QualityControlPassedQty,
          QualityControlFailedQty = newShopping.StoreReceiptSummary.QualityControlFailedQty,
          QualityControlConsumedQty = newShopping.StoreReceiptSummary.QualityControlConsumedQty,
          ReceiptQualityControlPassedQty = newShopping.StoreReceiptSummary.ReceiptQualityControlPassedQty,
          ReceiptQualityControlFailedQty = newShopping.StoreReceiptSummary.ReceiptQualityControlFailedQty,
          ReceiptQualityControlConsumedQty = newShopping.StoreReceiptSummary.ReceiptQualityControlConsumedQty,
          RowVersion = newShopping.RowVersion
        };
    public Expression<Func<ReturnStoreReceipt, StoreReceiptResult>> ReturnStoreReceiptToStoreReceiptResult =
        entity => new StoreReceiptResult
        {
          Id = entity.Id,
          StoreReceiptType = entity.StoreReceiptType,
          Code = entity.Code,
          ReceiptId = entity.ReceiptId,
          ReceiptCode = entity.Receipt.Code,
          ReceiptDateTime = entity.Receipt.DateTime,
          ReceiptStatus = entity.ReceiptId == null ? ReceiptStatus.NoReceipt : entity.Receipt.Status,
          ReceiptUserId = entity.Receipt.UserId,
          EmployeeFullName = entity.Receipt.User.Employee.FirstName + "  " + entity.Receipt.User.Employee.LastName,
          WarehouseId = entity.WarehouseId,
          WarehouseName = entity.Warehouse.Name,
          InboundCargoId = entity.InboundCargoId,
          InboundCargoCode = entity.InboundCargo.Code,
          InboundCargoDateTime = entity.InboundCargo.DateTime,
          TransportDateTime = entity.InboundCargo.TransportDateTime,
          StuffId = entity.StuffId,
          StuffName = entity.Stuff.Name,
          StuffCode = entity.Stuff.Code,
          StuffNoun = entity.Stuff.Noun,
          Amount = entity.Amount,
          UnitId = entity.UnitId,
          UnitName = entity.Unit.Name,
          BoxNo = null,
          QtyPerBox = null,
          DateTime = entity.DateTime,
          PurchaseOrderId = null,
          PurchaseOrderCode = "",
          CargoId = null,
          CargoCode = "",
          CargoItemId = null,
          CargoItemCode = "",
          CooperatorName = "",
          CooperatorId = null,
          CurrentPurchasePriceId = (int?)entity.CurrentPurchasePrice.Id,
          CurrentPurchasePriceRowVersion = (byte[])entity.CurrentPurchasePrice.RowVersion,
          Price = (double?)entity.CurrentPurchasePrice.Price,
          EstimatedPrice = (double?)0d,
          CurrencyRate = (double?)entity.CurrentPurchasePrice.CurrencyRate,
          CurrencyId = (int?)entity.CurrentPurchasePrice.CurrencyId,
          CurrencyTitle = entity.CurrentPurchasePrice.Currency.Title,
          EstimatedCurrencyTitle = "",
          QualityControlPassedQty = entity.StoreReceiptSummary.QualityControlPassedQty,
          QualityControlFailedQty = entity.StoreReceiptSummary.QualityControlFailedQty,
          QualityControlConsumedQty = entity.StoreReceiptSummary.QualityControlConsumedQty,
          ReceiptQualityControlPassedQty = entity.StoreReceiptSummary.ReceiptQualityControlPassedQty,
          ReceiptQualityControlFailedQty = entity.StoreReceiptSummary.ReceiptQualityControlFailedQty,
          ReceiptQualityControlConsumedQty = entity.StoreReceiptSummary.ReceiptQualityControlConsumedQty,
          RowVersion = entity.RowVersion
        };
    public Expression<Func<StoreReceipt, StoreReceiptComboResult>> ToStoreReceiptComboResult =
        entity => new StoreReceiptComboResult
        {
          Id = entity.Id,
          Code = entity.Code,
          ReceiptId = entity.ReceiptId.Value,
          ReceiptCode = entity.Receipt.Code,
          ReceiptStatus = entity.ReceiptId == null ? ReceiptStatus.NoReceipt : entity.Receipt.Status,
        };
    //public Expression<Func<NewShopping, StoreReceiptComboResult>> NewShoppingToStoreReceiptComboResult =
    //    entity => new StoreReceiptComboResult
    //    {
    //        Id = entity.Id,
    //        Code = entity.Code,
    //        ReceiptId = entity.ReceiptId.Value,
    //        ReceiptCode = entity.Receipt.Code,
    //        ReceiptStatus = entity.ReceiptId == null ? ReceiptStatus.NoReceipt : entity.Receipt.Status,
    //    };
    //public Expression<Func<ReturnOfSale, StoreReceiptComboResult>> ReturnOfSaleToStoreReceiptComboResult =
    //    entity => new StoreReceiptComboResult
    //    {
    //        Id = entity.Id,
    //        Code = entity.Code,
    //        ReceiptId = entity.ReceiptId.Value,
    //        ReceiptCode = entity.Receipt.Code,
    //        ReceiptStatus = entity.ReceiptId == null ? ReceiptStatus.NoReceipt : entity.Receipt.Status,
    //    };
    #endregion
    #region AddStoreReceiptsProcess
    public void AddStoreReceiptsProcess(
        int inboundCargoId,
        AddNewShoppingInput[] addNewShoppings,
        AddReturnOfSaleInput[] addReturnOfSales,
        string description,
        SerialPrintType? printType,
        int? printerId,
        bool? printBarcodeFooter,
        bool autoAddReceipt = false)
    {

      #region Check DateTime
      var transport = App.Internals.Guard.GetTransport(id: inboundCargoId);
      var dateTiemNow = DateTime.Now.ToLocalTime();
      var daysCount = App.Internals.ApplicationSetting.GetStoreReceiptDaysAfterInboundCargoValue();
      var addStoreReceiptWithouCheckingDateTimePermission = App.Internals.UserManagement.CheckPermission(
                   actionName: StaticActionName.AddStoreReceiptWihoutCheckingDateTime,
                   actionParameters: null);
      if (addStoreReceiptWithouCheckingDateTimePermission.AccessType == AccessType.Denied)
      {
        if (dateTiemNow > transport.DateTime.ToLocalTime().AddDays(daysCount))
          throw new StoreReceiptDateExpiredException(days: daysCount);
      }
      #endregion
      List<StoreReceipt> storeReceipts = new List<StoreReceipt>();
      #region Get InboundCargo
      var inboundCargo = App.Internals.Guard.GetInboundCargo(id: inboundCargoId);
      #endregion
      #region EditInboundCargo
      App.Internals.Guard.EditInboundCargo(
              inboundCargo: inboundCargo,
              rowVersion: inboundCargo.RowVersion,
              status: TransportStatus.Incomplated);
      #endregion
      if (addNewShoppings != null && addNewShoppings.Length != 0)
      {
        #region NewShopping
        #region CheckWarehouseType and CheckWarehouseStoreReceiptType
        var warehouseIds = addNewShoppings.Select(i => i.WarehouseId).Distinct();
        foreach (var warehouseId in warehouseIds)
        {
          App.Internals.WarehouseManagement.CheckWarehouseType(
                    warehouseId: warehouseId,
                    warehouseType: WarehouseType.Inbound); ; App.Internals.WarehouseManagement.CheckWarehouseStoreReceiptType(warehouseId: warehouseId, storeReceiptType: StoreReceiptType.NewShopping);
        }
        #endregion
        #region AddNewShoppings
        var newShoppings = new List<NewShopping>();
        foreach (var addNewShopping in addNewShoppings)
        {
          #region Compare NewShoppingQty with ReaminedLadingItemQty
          var sumTargetQtyItemsInput = new List<SumQtyItemInput>();
          var currentQtyNewShoppingsInput = new List<CurrentQtyItemInput>();
          var targetQtyNewShoppingsInput = new List<TargetQtyItemInput>();
          var currentQtyNewShoppingInput = new CurrentQtyItemInput();
          currentQtyNewShoppingInput.CurrentQty = addNewShopping.Amount;
          currentQtyNewShoppingInput.CurrentUnitId = addNewShopping.UnitId;
          currentQtyNewShoppingsInput.Add(currentQtyNewShoppingInput);
          foreach (var ladingItemDetail in addNewShopping.LadingItemDetails)
          {
            var currentQtyItemsInput = new List<CurrentQtyItemInput>();
            var targetQtyItemsInput = new List<TargetQtyItemInput>();
            var ladingItemDetailResult = App.Internals.Supplies.GetLadingItemDetail(id: ladingItemDetail.LadingItemDetailId);
            var ladingItemDetailRemainedQty = ladingItemDetailResult.Qty - ladingItemDetailResult.LadingItemDetailSummary.ReceiptedQty;
            var targetQtyItemInput = new TargetQtyItemInput();
            targetQtyItemInput.TargetQty = ladingItemDetailRemainedQty;
            targetQtyItemInput.TargetUnitId = ladingItemDetailResult.CargoItemDetail.UnitId;
            targetQtyItemsInput.Add(targetQtyItemInput);
            var sumTargetQtyItemInput = new SumQtyItemInput();
            sumTargetQtyItemInput.Qty = ladingItemDetailRemainedQty;
            sumTargetQtyItemInput.UnitId = ladingItemDetailResult.CargoItemDetail.UnitId;
            sumTargetQtyItemsInput.Add(sumTargetQtyItemInput);
            var currentQtyItemInput = new CurrentQtyItemInput();
            currentQtyItemInput.CurrentQty = ladingItemDetail.LadingItemDetailQty;
            currentQtyItemInput.CurrentUnitId = ladingItemDetailResult.CargoItemDetail.UnitId;
            currentQtyItemsInput.Add(currentQtyItemInput);
            var qtyItemCompareInput = new QtyItemCompareInput();
            qtyItemCompareInput.CurrentQtyItemInput = currentQtyItemsInput.ToArray();
            qtyItemCompareInput.TargetQtyItemInput = targetQtyItemsInput.ToArray();
            App.Internals.ApplicationBase.CompareQty(qtyItemCompareInput);
          }
          var sumQty = App.Internals.ApplicationBase.SumQty(targetUnitId: addNewShopping.UnitId, sumQtys: sumTargetQtyItemsInput.ToArray());
          var targetQtyNewShoppingInput = new TargetQtyItemInput();
          targetQtyNewShoppingInput.TargetQty = sumQty.Qty;
          targetQtyNewShoppingInput.TargetUnitId = sumQty.UnitId;
          targetQtyNewShoppingsInput.Add(targetQtyNewShoppingInput);
          var qtyNewShoppingCompareInput = new QtyItemCompareInput();
          qtyNewShoppingCompareInput.CurrentQtyItemInput = currentQtyNewShoppingsInput.ToArray();
          qtyNewShoppingCompareInput.TargetQtyItemInput = targetQtyNewShoppingsInput.ToArray();
          App.Internals.ApplicationBase.CompareQty(qtyNewShoppingCompareInput);
        }
        #endregion
        var newShoppingStuffIds = addNewShoppings.GroupBy(e => e.StuffId).Select(i => i.Key).ToArray();
        var newShoppingStuffs = App.Internals.SaleManagement.GetStuffs(
                  e => new { StuffId = e.Id, e.Tolerance, e.NeedToQualityControl },
                  ids: newShoppingStuffIds)
              .ToArray();
        foreach (var addNewShopping in addNewShoppings)
        {
          #region Get Stuff
          //var stuff = App.Internals.SaleManagement.GetStuff(e =>  new { e.Tolerance , e.NeedToQualityControl }, addNewShopping.StuffId);
          var stuff = newShoppingStuffs.FirstOrDefault(i => i.StuffId == addNewShopping.StuffId);
          #endregion
          #region Check Stuff Telorance
          var ladingItem = App.Internals.Supplies.GetLadingItem(
                    e => e,
                    id: addNewShopping.LadingItemId);
          var ladingItemQty = ladingItem.Qty;
          var remainedLadingItemQty = ladingItemQty - (ladingItem.LadingItemSummary == null ? 0 : ((double?)ladingItem.LadingItemSummary.ReceiptedQty ?? 0));
          var sumRemainedLadingQtyInputObj = new SumQtyItemInput();
          sumRemainedLadingQtyInputObj.Qty = remainedLadingItemQty;
          sumRemainedLadingQtyInputObj.UnitId = ladingItem.CargoItem.UnitId;
          var sumRemainedLadingQty = App.Internals.ApplicationBase.SumQty(targetUnitId: null, sumItem: sumRemainedLadingQtyInputObj);
          var sumQtyItemInputObj = new SumQtyItemInput();
          sumQtyItemInputObj.Qty = addNewShopping.Amount;
          sumQtyItemInputObj.UnitId = addNewShopping.UnitId;
          var sumQty = App.Internals.ApplicationBase.SumQty(targetUnitId: null, sumItem: sumQtyItemInputObj);
          var addnewShoppingAmount = sumQty.Qty;
          var tolerance = ((stuff.Tolerance * sumRemainedLadingQty.Qty) / 100);
          if ((sumRemainedLadingQty.Qty + tolerance) < addnewShoppingAmount)
          {
            throw new ExcessStoreReceiptEntryOnPurchaseOrderQtyException(remainedLadingItemQty, ladingItem.CargoItem.PurchaseOrder.Code);
          }
          #endregion
          var newShopping = AddNewShoppingProcess(
                        newShopping: null,
                        transactionBatch: null,
                        ladingItemId: addNewShopping.LadingItemId,
                        ladingItemDetails: addNewShopping.LadingItemDetails,
                        productionOrderId: null,
                        stuffId: addNewShopping.StuffId,
                        billOfMaterialVersion: addNewShopping.BillOfMaterialVersion,
                        amount: addNewShopping.Amount,
                        unitId: addNewShopping.UnitId,
                        boxNo: addNewShopping.BoxNo,
                        qtyPerBox: addNewShopping.QtyPerBox,
                        description: addNewShopping.Description,
                        inboundCargoId: inboundCargoId,
                        warehouseId: addNewShopping.WarehouseId,
                        cooperatorId: addNewShopping.CooperatorId,
                        printType: printType,
                        printerId: printerId,
                        printBarcodeFooter: printBarcodeFooter,
                        stuffNeedToQualityControl: stuff.NeedToQualityControl);
          newShoppings.Add(newShopping);
          storeReceipts.Add(newShopping);
        }
        Receipt receipt = null;
        #region Add Automatically Receipt for Store Receipt
        if (newShoppings.Any())
        {
          var newShoppingGroupResult = from newShopping in newShoppings
                                       group newShopping by new
                                       {
                                         CooperatorId = newShopping.CooperatorId,
                                         LadingId = newShopping.LadingItem.LadingId,
                                         StoreReceiptType = newShopping.StoreReceiptType,
                                       } into g
                                       select new
                                       {
                                         LadingId = g.Key.LadingId,
                                         StuffId = g.Select(m => m.StuffId),
                                         CooperatorId = g.Key.CooperatorId,
                                         StoreReceiptIds = g.Select(m => m.Id),
                                         StoreReceiptType = g.Key.StoreReceiptType
                                       };
          foreach (var newShoppingGroupRes in newShoppingGroupResult)
          {
            #region AddReceipt
            receipt = AddReceipt(
                receipt: null,
                transactionBatch: null,
                cooperatorId: newShoppingGroupRes.CooperatorId,
                receiptDateTime: inboundCargo.DateTime,
                receiptType: newShoppingGroupRes.StoreReceiptType,
                status: ReceiptStatus.Temporary,// نوع لیست رسید انبار در اولین ورود موقت هست
                description: description);
            #endregion
            #region EditStoreReceipt set ReceiptId
            foreach (var storeReceiptId in newShoppingGroupRes.StoreReceiptIds)
            {
              #region Check CooperatorNotMatch And StoreReceiptTypeNotMatch
              var storeReceipt = GetStoreReceipt(id: storeReceiptId);
              if (storeReceipt.CooperatorId == receipt.CooperatorId && storeReceipt.StoreReceiptType == receipt.ReceiptType)
              {
                var editStoreReceipt = EditStoreReceipt(
                              storeReceipt: storeReceipt,
                              receiptId: receipt.Id,
                              rowVersion: storeReceipt.RowVersion);
              }
              #endregion
            }
            #endregion
          }
          #endregion
          #endregion
          #region Done ShippingTracking Task
          var ladingItemIds = newShoppings.Where(i => i != null).Select(i => i.LadingItemId.Value).ToArray();
          foreach (var ladingItemId in ladingItemIds.ToList().Distinct())
          {
            #region Get ProjectWorkItem
            var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                    baseEntityId: ladingItemId,
                    scrumTaskType: ScrumTaskTypes.ShippingTracking);
            #endregion
            #region DoneTask
            if (projectWorkItem != null)
              App.Internals.ScrumManagement.DoneScrumTask(
                            scrumTask: projectWorkItem,
                            rowVersion: projectWorkItem.RowVersion);
            #endregion
          }
          #endregion
          #region Done Receipt Task
          #region Get ProjectWorkItem
          var receiptProjectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                        baseEntityId: inboundCargoId,
                        scrumTaskType: ScrumTaskTypes.StoreReceipt);
          #endregion
          #region DoneTask
          if (receiptProjectWorkItem != null)
            App.Internals.ScrumManagement.DoneScrumTask(
                          scrumTask: receiptProjectWorkItem,
                          rowVersion: receiptProjectWorkItem.RowVersion);
          #endregion
          #endregion
          #endregion
        }
      }
      else
      {
        #region ReturnStoreReceipt
        #region CheckReturn
        var retrunOfSaleWarehouseIds = addReturnOfSales.Select(i => i.WarehouseId).Distinct();
        foreach (var warehouseId in retrunOfSaleWarehouseIds)
        {
          App.Internals.WarehouseManagement.CheckWarehouseStoreReceiptType(warehouseId: warehouseId, storeReceiptType: StoreReceiptType.ReturnOfSale);
        }
        #endregion
        var returnStoreReceiptsInput = from item in addReturnOfSales
                                       group item by new
                                       {
                                         item.CooperatorId,
                                         item.StuffId,
                                         item.WarehouseId
                                       }
            into gItems
                                       select new
                                       {
                                         gItems.Key.CooperatorId,
                                         gItems.Key.StuffId,
                                         gItems.Key.WarehouseId,
                                         Items = gItems.ToList()
                                       };
        foreach (var returnStoreReceiptInput in returnStoreReceiptsInput)
        {
          #region AddReturnStoreReceipt
          var sumQtyResult = App.Internals.ApplicationBase.SumQty(
                        targetUnitId: null,
                        sumQtys: returnStoreReceiptInput.Items
                            .Select(i => new SumQtyItemInput() { Qty = i.Amount, UnitId = i.UnitId }).ToArray());
          var returnStoreReceipt = AddReturnStoreReceiptProcess(
                        returnStoreReceipt: null,
                        transactionBatch: null,
                        stuffId: returnStoreReceiptInput.StuffId,
                        billOfMaterialVersion: null,
                        amount: sumQtyResult.Qty,
                        unitId: sumQtyResult.UnitId,
                        inboundCargoId: inboundCargoId,
                        warehouseId: returnStoreReceiptInput.WarehouseId,
                        cooperatorId: returnStoreReceiptInput.CooperatorId,
                        description: description,
                        stuffNeedToQualityControl: true,
                        addReturnOfSales: returnStoreReceiptInput.Items.ToArray());
          storeReceipts.Add(returnStoreReceipt);
          #endregion
        }
        #region Add Automatically Receipt for Store Receipt
        if (storeReceipts.Any())
        {
          var storeReceiptGroupResult = from newShopping in storeReceipts
                                        group newShopping by new
                                        {
                                          CooperatorId = newShopping.CooperatorId,
                                          StoreReceiptType = newShopping.StoreReceiptType,
                                        } into g
                                        select new
                                        {
                                          StuffId = g.Select(m => m.StuffId),
                                          CooperatorId = g.Key.CooperatorId,
                                          StoreReceiptIds = g.Select(m => m.Id),
                                          StoreReceiptType = g.Key.StoreReceiptType
                                        };
          foreach (var storeReceiptGroupRes in storeReceiptGroupResult)
          {
            #region AddReceipt
            var receipt = AddReceipt(
                 receipt: null,
                 transactionBatch: null,
                 cooperatorId: storeReceiptGroupRes.CooperatorId,
                 receiptDateTime: inboundCargo.DateTime,
                 receiptType: storeReceiptGroupRes.StoreReceiptType,
                 status: ReceiptStatus.Temporary,// نوع لیست رسید انبار در اولین ورود موقت هست
                 description: description);
            #endregion
            #region EditStoreReceipt set ReceiptId
            foreach (var storeReceiptId in storeReceiptGroupRes.StoreReceiptIds)
            {
              #region EditStoreReceipt
              var storeReceipt = GetStoreReceipt(id: storeReceiptId);
              if (storeReceipt.CooperatorId == receipt.CooperatorId && storeReceipt.StoreReceiptType == receipt.ReceiptType)
              {
                var editStoreReceipt = EditStoreReceipt(
                              storeReceipt: storeReceipt,
                              receiptId: receipt.Id,
                              rowVersion: storeReceipt.RowVersion);
              }
              #endregion
            }
            #endregion
          }
        }
        #endregion
      }
      #endregion
      //todo add task
      //#region Add or Get ProjectStep
      //var projectStep = App.Internals.ProjectManagement.GetOrAddCommonProjectStep(
      //        departmentId: (int)Departments.Accounting)
      //    
      //;
      //#endregion
      //#region Add ProjectWork
      //var projectWork = App.Internals.ProjectManagement.AddProjectWork(
      //        projectWork: null,
      //        name: $"ثبت قیمت رسید ورودی {receipt.Code}",
      //        description: "",
      //        color: "",
      //        departmentId: (int)Departments.Accounting,
      //        estimatedTime: 18000,
      //        isCommit: false,
      //        projectStepId: projectStep.Id,
      //        baseEntityId: receipt.Id
      //    )
      //    
      //;
      //#endregion
      //#region Add SaveReceiptPurchasePrice Task
      //if (projectWork != null)
      //    App.Internals.ProjectManagement.AddProjectWorkItem(
      //            projectWorkItem: null,
      //            name: $"ثبت قیمت رسید ورودی {receipt.Code}",
      //            description: "",
      //            color: "",
      //            departmentId: (int)Departments.Accounting,
      //            estimatedTime: 10800,
      //            isCommit: false,
      //            scrumTaskTypeId: (int)ScrumTaskTypes.SaveReceiptPurchasePrice,
      //            userId: null,
      //            spentTime: 0,
      //            remainedTime: 0,
      //            scrumTaskStep: ScrumTaskStep.ToDo,
      //            projectWorkId: projectWork.Id,
      //            baseEntityId: receipt.Id)
      //        
      //;
      //#endregion
    }
    #endregion
    #region Delete
    public void DeleteStoreReceipt(int storeReceiptId)
    {

      var storeReceipt = GetStoreReceipt(storeReceiptId);
      EditStoreReceipt(
                id: storeReceipt.Id,
                rowVersion: storeReceipt.RowVersion,
                isDelete: true);
    }
    public void DeleteStoreReceiptProcess(int storeReceiptDeleteRequestId)
    {

      #region change inBoundCargo Status To NotAction
      var storeReceiptDeleteRequest = GetStoreReceiptDeleteRequest(
          id: storeReceiptDeleteRequestId);
      var storeReceipt = GetStoreReceipt(
                id: storeReceiptDeleteRequest.StoreReceiptId);
      var inBoundCargos = App.Internals.Guard.GetInboundCargo(
                id: storeReceipt.InboundCargoId);
      App.Internals.Guard.EditInboundCargo(
                inboundCargoId: storeReceipt.InboundCargoId,
                status: TransportStatus.NotAction,
                rowVersion: inBoundCargos.RowVersion);
      #endregion
      #region Get stuff serials
      var storeReceiptDeleteRequestStuffSerials = GetStoreReceiptDeleteRequestStuffSerials(
          selector: e =>
          new
          {
            e.StuffSerialId,
            e.StuffSerialCode,
            e.StuffSerial.Serial,
            e.StuffSerial.BillOfMaterialVersion
          },
          storeReceiptDeleteRequestId: storeReceiptDeleteRequestId);
      #region AddTransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      foreach (var stuffSerial in storeReceiptDeleteRequestStuffSerials)
      {
        #region check consumption
        var warehouseTransactions = GetWarehouseTransactions(
          selector: e => e,
          serial: stuffSerial.Serial);
        if (storeReceipt.StoreReceiptType == StoreReceiptType.NewShopping && warehouseTransactions.Any(i => i.TransactionTypeId == Models.StaticData.StaticTransactionTypes.Consum.Id))
          throw new SerialHasConsumTransactionException(stuffSerial.Serial);
        #endregion
        #region check warehouseIssue
        var pendingWarehouseIssues = GetWarehouseIssues(
            selector: e => e,
            serial: stuffSerial.Serial,
            status: WarehouseIssueStatusType.Waiting);
        if (pendingWarehouseIssues.Any())
          throw new SerialHasPendingWarehouseIssueException(serial: stuffSerial.Serial);
        #endregion
        #region add warehouseTransactions
        var stuffSerialInvetories = GetStuffSerialInventories(
            stuffId: stuffSerial.StuffSerialId,
            stuffSerialCode: stuffSerial.StuffSerialCode)


        .FirstOrDefault();
        if (stuffSerialInvetories != null)
        {
          short transactionTypeId = 0;
          if (stuffSerialInvetories.AvailableAmount > 0)
            transactionTypeId = Models.StaticData.StaticTransactionTypes.ExportAvailable.Id;
          else if (stuffSerialInvetories.BlockedAmount > 0)
            transactionTypeId = Models.StaticData.StaticTransactionTypes.ExportBlockedFromWarehouse.Id;
          else if (stuffSerialInvetories.QualityControlAmount > 0)
            transactionTypeId = Models.StaticData.StaticTransactionTypes.ExportQualityControl.Id;
          else if (stuffSerialInvetories.WasteAmount > 0)
            transactionTypeId = Models.StaticData.StaticTransactionTypes.ExportWaste.Id;
          if (transactionTypeId > 0)
          {
            AddWarehouseTransaction(
                      transactionBatchId: transactionBatch.Id,
                      effectDateTime: DateTime.Now.ToUniversalTime(),
                      stuffId: stuffSerial.StuffSerialId,
                      billOfMaterialVersion: stuffSerial.BillOfMaterialVersion,
                      stuffSerialCode: stuffSerial.StuffSerialCode,
                      warehouseId: stuffSerialInvetories.WarehouseId,
                      transactionTypeId: transactionTypeId,
                      amount: stuffSerialInvetories.TotalAmount,
                      unitId: stuffSerialInvetories.UnitId,
                      description: "حذف رسید انبار",
                      referenceTransaction: null,
                      checkFIFO: false);
          }
        }
        #endregion
      }
      var newShoppingId = storeReceipt.Id;
      var storeReceiptOriginalAmount = storeReceipt.Amount;
      EditStoreReceipt(
                id: storeReceipt.Id,
                rowVersion: storeReceipt.RowVersion,
                amount: 0);
      var newShoppingDetails = GetNewShoppingDetails(
                selector: e => e,
                newShoppingId: newShoppingId);
      Dictionary<int, double> newShoppingDetailsOriginalAmounts = new Dictionary<int, double>();
      foreach (var newShoppingDetail in newShoppingDetails)
      {
        newShoppingDetailsOriginalAmounts.Add(newShoppingDetail.Id, newShoppingDetail.Qty);
        EditNewShoppingDetail(
                  id: newShoppingDetail.Id,
                  rowVersion: newShoppingDetail.RowVersion,
                  qty: 0);
      }
      #region delete QualityControl
      var qcModule = App.Internals.QualityControl;
      var qualityControl = qcModule.GetQualityControls(
                selector: e => e,
                storeReceiptId: storeReceipt.Id)


            .FirstOrDefault();
      foreach (var qcItem in qualityControl.QualityControlItems)
      {
        qcModule.RemoveQualityControlItemProcess(
                  transactionBatchId: null,
                  id: qcItem.Id,
                  rowVersion: qcItem.RowVersion);
      }
      var qualityControlItems = qcModule.GetQualityControlItems(
                selector: e => e,
                qualityControlId: qualityControl.Id,
                isDelete: false);
      if (!qualityControlItems.Any())
      {
        qcModule.DeleteQualityControl(
                  id: qualityControl.Id,
                  rowVersion: qualityControl.RowVersion);
      }
      #region ResetQualityControlSummaryByQualityControlId
      qcModule.ResetQualityControlSummaryByQualityControlId(qualityControlId: qualityControl.Id);
      #endregion
      #region Change back Amounts and delete entities
      EditStoreReceipt(
          storeReceipt: storeReceipt,
          rowVersion: storeReceipt.RowVersion,
          amount: storeReceiptOriginalAmount,
          isDelete: true);
      foreach (var amountKeyValue in newShoppingDetailsOriginalAmounts)
      {
        var newShoppingDetail = GetNewShoppingDetail(amountKeyValue.Key);
        EditNewShoppingDetail(
                  id: amountKeyValue.Key,
                  rowVersion: newShoppingDetail.RowVersion,
                  qty: amountKeyValue.Value,
                  isDelete: true);
      }
      #endregion
      #endregion
      #endregion
    }
    #endregion
    #region ResetStatus
    public StoreReceipt ResetStoreReceiptStatus(int storeReceiptId)
    {

      var storeReceipt = GetStoreReceipt(id: storeReceiptId);
      return ResetStoreReceiptStatus(storeReceipt: storeReceipt);
    }
    public StoreReceipt ResetStoreReceiptStatus(StoreReceipt storeReceipt)
    {

      #region storeReceiptSummary
      var storeReceiptSummary = ResetStoreReceiptSummaryByStoreReceiptId(
      storeReceiptId: storeReceipt.Id);
      #endregion
      return storeReceipt;
    }
    #endregion
  }
}
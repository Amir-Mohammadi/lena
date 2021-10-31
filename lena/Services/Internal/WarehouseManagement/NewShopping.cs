using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.ApplicationBase.Unit;
using lena.Models.Common;
using lena.Models.QualityControl.QualityControlItem;
using lena.Models.WarehouseManagement.NewShopping;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public NewShopping AddNewShopping(
        NewShopping newShopping,
        TransactionBatch transactionBatch,
        int? receiptId,
        int ladingItemId,
        int cooperatorId,
        int stuffId,
        short? billOfMaterialVersion,
        double amount,
        byte unitId,
        int boxNo,
        double qtyPerBox,
        int inboundCargoId,
        short warehouseId,
        string description,
        bool stuffNeedToQualityControl)
    {

      newShopping = newShopping ?? repository.Create<NewShopping>();
      newShopping.LadingItemId = ladingItemId;
      newShopping.BoxNo = boxNo;
      newShopping.QtyPerBox = qtyPerBox;
      AddStoreReceipt(
                    storeReceipt: newShopping,
                    transactionBatch: transactionBatch,
                    receiptId: receiptId,
                    cooperatorId: cooperatorId,
                    stuffId: stuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    amount: amount,
                    unitId: unitId,
                    warehouseId: warehouseId,
                    inboundCargoId: inboundCargoId,
                    description: description,
                    stuffNeedToQualityControl: stuffNeedToQualityControl);
      return newShopping;
    }
    #endregion
    #region Edit
    public NewShopping EditNewShopping(
        int id,
        byte[] rowVersion,
        TValue<int?> receiptId = null,
        TValue<int> ladingItemId = null,
        TValue<int> providerId = null,
        TValue<int> stuffId = null,
        TValue<double> amount = null,
        TValue<byte> unitId = null,
        TValue<int> boxNo = null,
        TValue<int> qtyPerBox = null,
        TValue<string> description = null)
    {

      var entity = GetNewShopping(id: id);
      if (ladingItemId != null)
        entity.LadingItemId = ladingItemId;
      return EditNewShopping(
                    newShopping: entity,
                    rowVersion: rowVersion,
                    receiptId: receiptId,
                    ladingItemId: ladingItemId,
                    providerId: providerId,
                    stuffId: stuffId,
                    amount: amount,
                    unitId: unitId,
                    boxNo: boxNo,
                    qtyPerBox: qtyPerBox,
                    description: description);
    }
    public NewShopping EditNewShopping(
        NewShopping newShopping,
        byte[] rowVersion,
        TValue<int?> receiptId = null,
        TValue<int> ladingItemId = null,
        TValue<int> providerId = null,
        TValue<int> stuffId = null,
        TValue<double> amount = null,
        TValue<byte> unitId = null,
        TValue<int> boxNo = null,
        TValue<int> qtyPerBox = null,
        TValue<string> description = null)
    {

      if (ladingItemId != null)
        newShopping.LadingItemId = ladingItemId;
      if (boxNo != null)
        newShopping.BoxNo = boxNo;

      if (qtyPerBox != null)
        newShopping.QtyPerBox = qtyPerBox;
      var retValue = EditStoreReceipt(
                    storeReceipt: newShopping,
                    rowVersion: rowVersion,
                    receiptId: receiptId,
                    cooperatorId: providerId,
                    stuffId: stuffId,
                    amount: amount,
                    unitId: unitId,
                    description: description);
      return retValue as NewShopping;
    }
    #endregion
    #region Get
    public NewShopping GetNewShopping(int id) => GetNewShopping(selector: e => e, id: id);
    public TResult GetNewShopping<TResult>(
        Expression<Func<NewShopping, TResult>> selector,
        int id)
    {

      var orderItemBlock = GetNewShoppings(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (orderItemBlock == null)
        throw new NewShoppingNotFoundException(id);
      return orderItemBlock;
    }
    public NewShopping GetNewShopping(string code) => GetNewShopping(selector: e => e, code: code);
    public TResult GetNewShopping<TResult>(
        Expression<Func<NewShopping, TResult>> selector,
        string code)
    {

      var orderItemBlock = GetNewShoppings(
                    selector: selector,
                    code: code)


                .FirstOrDefault();
      if (orderItemBlock == null)
        throw new NewShoppingNotFoundException(code);
      return orderItemBlock;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetNewShoppings<TResult>(
        Expression<Func<NewShopping, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<int> ladingItemId = null,
        TValue<int> receiptId = null,
        TValue<int> providerId = null,
        TValue<int> stuffId = null,
        TValue<double> amount = null,
        TValue<int> unitId = null,
        TValue<int> boxNo = null,
        TValue<int> qtyPerBox = null,
        TValue<string> description = null,
        TValue<int> transactionBatchId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<ProviderType> providerType = null,
        TValue<bool> isDelete = null)
    {

      IQueryable<BaseEntity> baseQuery = GetStoreReceipts(id: id,
                    selector: e => e,
                    transactionBatchId: transactionBatchId,
                    receiptId: receiptId,
                    cooperatorId: providerId,
                    stuffId: stuffId,
                    amount: amount,
                    unitId: unitId,
                    description: description,
                    isDelete: isDelete);
      var newShopping = baseQuery.OfType<NewShopping>();
      if (ladingItemId != null)
        newShopping = newShopping.Where(r => r.LadingItemId == ladingItemId);
      if (boxNo != null)
        newShopping = newShopping.Where(r => r.BoxNo == boxNo);
      if (qtyPerBox != null)
        newShopping = newShopping.Where(r => r.QtyPerBox == qtyPerBox);
      if (fromDateTime != null)
        newShopping = newShopping.Where(r => r.DateTime >= fromDateTime);
      if (toDateTime != null)
        newShopping = newShopping.Where(i => i.DateTime <= toDateTime);
      if (providerType != null)
        newShopping = newShopping.Where(i => i.LadingItem.CargoItem.PurchaseOrder.Provider.ProviderType == providerType);
      if (ids != null)
        newShopping = newShopping.Where(i => ids.Value.Contains(i.Id));
      if (isDelete != null)
        newShopping = newShopping.Where(i => i.IsDelete == isDelete);
      return newShopping.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<NewShopping, NewShoppingResult>> ToNewShoppingResult =
        newShopping => new NewShoppingResult
        {
          Id = newShopping.Id,
          Code = newShopping.Code,
          ReceiptId = newShopping.ReceiptId,
          ReceiptCode = newShopping.Receipt.Code,
          WarehouseId = newShopping.WarehouseId,
          WarehouseName = newShopping.Warehouse.Name,
          InboundCargoId = newShopping.InboundCargoId,
          InboundCargoCode = newShopping.InboundCargo.Code,
          CooperatorId = newShopping.CooperatorId,
          CooperatorName = newShopping.Cooperator.Name,
          StuffId = newShopping.StuffId,
          StuffName = newShopping.Stuff.Name,
          StuffNoun = newShopping.Stuff.Noun,
          Amount = newShopping.Amount,
          BillOfMaterialVersion = newShopping.BillOfMaterialVersion,
          UnitId = newShopping.UnitId,
          UnitName = newShopping.Unit.Name,
          BoxNo = newShopping.BoxNo,
          QtyPerBox = newShopping.QtyPerBox,
          CargoItemId = newShopping.LadingItem.CargoItemId,
          CargoItemCode = newShopping.LadingItem.CargoItem.Code,
          CargoId = newShopping.LadingItem.CargoItem.CargoId,
          CargoCode = newShopping.LadingItem.CargoItem.Cargo.Code,
          PurchaseOrderId = newShopping.LadingItem.CargoItem.PurchaseOrderId,
          PurchaseOrderCode = newShopping.LadingItem.CargoItem.PurchaseOrder.Code,

          LadingId = newShopping.LadingItem.LadingId,
          LadingCode = newShopping.LadingItem.Lading.Code,
          LadingItemId = newShopping.LadingItemId,

          RowVersion = newShopping.RowVersion
        };
    #endregion
    #region AddProcess
    public NewShopping AddNewShoppingProcess(
        NewShopping newShopping,
        TransactionBatch transactionBatch,
        int cooperatorId,
        int ladingItemId,
        LadingItemDetails[] ladingItemDetails,
        int stuffId,
        int? productionOrderId,
        short? billOfMaterialVersion,
        double amount,
        byte unitId,
        int boxNo,
        double qtyPerBox,
        int inboundCargoId,
        short warehouseId,
        string description,
        SerialPrintType? printType,
        int? printerId,
        bool? printBarcodeFooter,
        bool stuffNeedToQualityControl)
    {

      #region Get LadingItemInfo
      var ladingItemInfo = App.Internals.Supplies.GetLadingItem(
          selector: e => new
          {
            Id = e.Id,
            LadingItemCooperatorId = e.CargoItem.PurchaseOrder.StuffProvider.ProviderId,
            ConversionRatio = e.CargoItem.Unit.ConversionRatio,
            Qty = e.Qty
          },
          id: ladingItemId);
      #endregion
      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      transactionBatch = transactionBatch ?? warehouseManagement.AddTransactionBatch();
      #endregion


      var sumQtyItemInputObj = new SumQtyItemInput();
      sumQtyItemInputObj.Qty = amount;
      sumQtyItemInputObj.UnitId = unitId;

      var sumQty = App.Internals.ApplicationBase.SumQty(targetUnitId: unitId, sumItem: sumQtyItemInputObj);

      #region AddNewShopping
      newShopping = AddNewShopping(
              newShopping: newShopping,
              transactionBatch: transactionBatch,
              receiptId: null,
              ladingItemId: ladingItemId,
              cooperatorId: cooperatorId,
              stuffId: stuffId,
              billOfMaterialVersion: billOfMaterialVersion,
              amount: sumQty.Qty,
              unitId: sumQty.UnitId,
              boxNo: boxNo,
              qtyPerBox: qtyPerBox,
              inboundCargoId: inboundCargoId,
              warehouseId: warehouseId,
              description: description,
              stuffNeedToQualityControl: stuffNeedToQualityControl);
      #endregion
      #region AddStoreReceiptSummary
      AddStoreReceiptSummary(
              qualityControlPassedQty: 0,
              qualityControlFailedQty: 0,
              qualityControlConsumedQty: 0,
              payedAmount: 0,
              storeReceiptId: newShopping.Id);
      #endregion
      #region Get stuff details
      var stuff = App.Internals
                      .SaleManagement
                      .GetStuff(e => new { e.Id, e.NeedToQualityControl, e.IsTraceable }, id: stuffId);
      #endregion
      #region AddStoreReceiptStuffSerials

      IQueryable<StuffSerial> stuffSerials = null;
      StuffSerial[] stuffSerialsArray = null;
      if (stuff.IsTraceable)
      {
        stuffSerials = AddStoreReceiptStuffSerials(
                     selector: e => e,
                     storeReceiptId: newShopping.Id,
                     stuffId: stuffId,
                     productionOrderId: productionOrderId,
                     billOfMaterialVersion: billOfMaterialVersion,
                     qty: amount,
                     unitId: unitId,
                     qtyPerBox: qtyPerBox);
        stuffSerialsArray = stuffSerials.ToArray();
        #region print barcodes
        //if (printerId == null || printType == null)
        //{
        //    throw new PrinterSettingsIsRequiredForTraceableStuffsException(stuffId: stuff.Id, stuffName: stuff.Name);
        //}
        if (printerId.HasValue && printType.HasValue && printBarcodeFooter.HasValue)
        {
          App.Internals.PrinterManagment.PrintBarcodes(
                        stuffSerials: stuffSerials,
                        printerId: printerId.Value,
                        printType: printType.Value,
                        printFooterText: printBarcodeFooter.Value);
        }


        #endregion
      }
      #endregion
      #region Get ladingItemDetails
      var ladingItemDetailResult = App.Internals.Supplies.GetLadingItemDetails(
                  selector: e => new
                  {
                    Id = e.Id,
                    TransactionBatchId = e.TransactionBatch.Id,
                    CargoItemDetailId = e.CargoItemDetailId,
                    UnitId = e.CargoItemDetail.UnitId
                  },
                  ladingItemId: ladingItemId)


              .ToList();
      var query = from ladingItemDetail in ladingItemDetailResult
                  join inputItem in ladingItemDetails on
                        ladingItemDetail.Id equals inputItem.LadingItemDetailId
                  select new
                  {
                    Id = ladingItemDetail.Id,
                    Qty = inputItem.LadingItemDetailQty,
                    UnitId = ladingItemDetail.UnitId,
                    CargoItemDetailId = ladingItemDetail.CargoItemDetailId,
                    TransactionBatchId = ladingItemDetail.TransactionBatchId,
                  };
      //.Where(m=> ladingItemDetails.Select(r=>r.LadingItemDetailId).Contains(m.Id));
      #endregion
      #region AddTransactions
      var remainQty = newShopping.Amount * newShopping.Unit.ConversionRatio;
      var index = 0;
      var remainSerialQty = 0d;
      var addQualityControlItemTransactionInputs = new List<AddQualityControlItemTransactionInput>();
      foreach (var ladingItemDetail in query)
      {
        #region Get CargoItemTransactions
        var ladingTransaction = App.Internals.WarehouseManagement.GetTransactionPlans(
                selector: e => e,
                transactionBatchId: ladingItemDetail.TransactionBatchId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportLading.Id)


            .Single();
        #endregion
        #region Calculate TransactionQty
        var ladingItemTransactionQty = ladingItemInfo.Qty * ladingItemInfo.ConversionRatio;
        var shoppedTransactionQuery = App.Internals.WarehouseManagement.GetTransactionPlans(
                      selector: e => e,
                      transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportLading.Id,
                      referenceTransactionId: ladingItemInfo.Id,
                      isDelete: false);

        var shoppingTransactionQty = shoppedTransactionQuery.Any() ?
                  shoppedTransactionQuery.Sum(i => i.Amount * i.Unit.ConversionRatio) : 0;

        var rLadingItemTransactionQty = ladingItemTransactionQty - shoppingTransactionQty;
        var transactionQty = Math.Min(remainQty, rLadingItemTransactionQty);
        #endregion

        #region AddNewShoppingDetail

        sumQtyItemInputObj = new SumQtyItemInput();
        sumQtyItemInputObj.Qty = ladingItemDetail.Qty;
        sumQtyItemInputObj.UnitId = ladingItemDetail.UnitId;

        sumQty = new SumQtyResult();
        sumQty = App.Internals.ApplicationBase.SumQty(targetUnitId: ladingItemDetail.UnitId, sumItem: sumQtyItemInputObj);


        var newShoppingDetail = AddNewShoppingDetail(
                      newShoppingDetail: null,
                      transactionBatch: transactionBatch,
                      newShoppingId: newShopping.Id,
                      ladingItemDetailId: ladingItemDetail.Id,
                      qty: sumQty.Qty,
                      unitId: sumQty.UnitId,
                      description: null);

        #region AddNewShoppingDetailSummary
        AddNewShoppingDetailSummary(
                qualityControlPassedQty: 0,
                qualityControlFailedQty: 0,
                qualityControlConsumedQty: 0,
                newShoppingDetailId: newShoppingDetail.Id);
        #endregion

        #region ResetLadingItemDetailSummary
        App.Internals.Supplies.ResetLadingItemDetailSummaryByLadingItemDetailId(ladingItemDetailId: ladingItemDetail.Id);
        #endregion

        #endregion
        #region AddTransactions
        while (transactionQty > 0)
        {
          var boxValue = remainSerialQty > 0 ? remainSerialQty : qtyPerBox * newShopping.Unit.ConversionRatio;
          var qty = Math.Min(transactionQty, boxValue);
          //var stuffSerialCode = ResolveStuffSerialCode(stuff: stuff, index: index, stuffSerials: stuffSerialsArray); // ==> removed only for increase speed
          var stuffSerialCode = stuff.IsTraceable ? stuffSerialsArray[index].Code : (long?)null;
          var version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
                        stuffSerialCode: stuffSerialCode,
                        stuffId: stuffId);
          #region Add ExportCargo TransactionPlan
          var exportLadingTransaction = App.Internals.WarehouseManagement
                    .AddTransactionPlanProcess(
                        transactionBatchId: transactionBatch.Id,
                        effectDateTime: ladingTransaction.EffectDateTime,
                        stuffId: stuffId,
                        billOfMaterialVersion: null,
                        stuffSerialCode: null,
                        transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportLading.Id,
                        amount: qty / newShopping.Unit.ConversionRatio,
                        unitId: newShopping.UnitId,
                        description: null,
                        isEstimated: false,
                        referenceTransaction: ladingTransaction);

          #endregion
          #region Add ImportAvailableTransaction
          var importAvailableTransaction = App.Internals.WarehouseManagement
                    .AddWarehouseTransaction(
                        transactionBatchId: transactionBatch.Id,
                        effectDateTime: transactionBatch.DateTime,
                        stuffId: stuffId,
                        billOfMaterialVersion: version,
                        stuffSerialCode: stuffSerialCode,
                        warehouseId: warehouseId,
                        transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAvailable.Id,
                        amount: qty / newShopping.Unit.ConversionRatio,
                        unitId: newShopping.UnitId,
                        description: null,
                        referenceTransaction: exportLadingTransaction);
          #endregion
          #region ResetCargoItemDetailStatus
          App.Internals.Supplies.ResetCargoItemDetailStatus(
                        cargoItemDetailId: ladingItemDetail.CargoItemDetailId);
          #endregion
          #region Add AddQualityControlItemTransactionInput
          var addQualityControlItemTransactionInput = new AddQualityControlItemTransactionInput()
          {
            StuffSerialCode = stuffSerialCode,
            Amount = qty / newShopping.Unit.ConversionRatio,
            UnitId = newShopping.UnitId,
            Description = null
          };
          addQualityControlItemTransactionInputs.Add(addQualityControlItemTransactionInput);
          #endregion
          if (qty == boxValue)
            index++;
          remainSerialQty = boxValue - qty;
          remainQty = remainQty - qty;
          transactionQty -= qty;
        }
        #endregion

      }
      while (remainQty > 0)
      {
        var boxValue = remainSerialQty > 0 ? remainSerialQty : qtyPerBox * newShopping.Unit.ConversionRatio;
        var qty = Math.Min(remainQty, boxValue);

        //var stuffSerialCode = ResolveStuffSerialCode(stuff: stuff, index: index, stuffSerials: stuffSerialsArray); // ==> removed only for increase speed
        var stuffSerialCode = stuff.IsTraceable ? stuffSerialsArray[index].Code : (long?)null;

        var version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
                      stuffSerialCode: stuffSerialCode,
                      stuffId: stuffId);
        #region Add ImportAvailableTransaction
        var importAvailableTransaction = App.Internals.WarehouseManagement
            .AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: stuffId,
                billOfMaterialVersion: version,
                stuffSerialCode: stuffSerialCode,
                warehouseId: warehouseId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAvailable.Id,
                amount: qty / newShopping.Unit.ConversionRatio,
                unitId: newShopping.UnitId,
                description: null,
                referenceTransaction: null);
        #endregion
        #region Add AddQualityControlItemTransactionInput

        var addQualityControlItemTransactionInput = new AddQualityControlItemTransactionInput()
        {
          StuffSerialCode = stuffSerialCode,
          Amount = qty / newShopping.Unit.ConversionRatio,
          UnitId = newShopping.UnitId,
          Description = null

        };
        addQualityControlItemTransactionInputs.Add(addQualityControlItemTransactionInput);
        #endregion
        if (qty == boxValue)
          index++;
        remainSerialQty = boxValue - qty;
        remainQty = remainQty - qty;
      }
      #endregion
      #region AddReceiptQualityControlProcess
      if (stuff.NeedToQualityControl)
      {
        App.Internals.QualityControl.AddReceiptQualityControlProcess(
                      receiptQualityControl: null,
                      transactionBatch: null,
                      storeReceiptId: newShopping.Id,
                      stuffId: stuffId,
                      warehouseId: warehouseId,
                      description: null,
                      addQualityControlItemTransactionInputs: addQualityControlItemTransactionInputs.ToArray());
      }
      #endregion
      #region Rest LadingItemStatus
      App.Internals.Supplies.ResetLadingItemStatus(ladingItemId: ladingItemId);
      #endregion
      return newShopping;
    }
    #endregion
    #region resolve stuff serial
    private long? ResolveStuffSerialCode(Stuff stuff, int index, StuffSerial[] stuffSerials)
    {
      if (stuff.IsTraceable)
      {
        return stuffSerials[index].Code;
      }
      return null;
    }
    #endregion
    #region Search
    public IQueryable<NewShoppingResult> SearchNewShoppingResult(IQueryable<NewShoppingResult> query,
    string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                select item;
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<NewShoppingResult> SortNewShoppingResult(IQueryable<NewShoppingResult> query,
        SortInput<NewShoppingSortType> sort)
    {
      switch (sort.SortType)
      {
        case NewShoppingSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case NewShoppingSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.QualityControl.QualityControlItem;
using lena.Models.WarehouseManagement.ReturnOfSale;
using lena.Models.WarehouseManagement.ReturnStoreReceipt;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public ReturnStoreReceipt AddReturnStoreReceipt(
        ReturnStoreReceipt returnStoreReceipt,
        TransactionBatch transactionBatch,
        int? receiptId,
        int cooperatorId,
        int stuffId,
        short? billOfMaterialVersion,
        double amount,
        byte unitId,
        int inboundCargoId,
        short warehouseId,
        string description,
        bool stuffNeedToQualityControl)
    {

      returnStoreReceipt = returnStoreReceipt ?? repository.Create<ReturnStoreReceipt>();
      AddStoreReceipt(
                    storeReceipt: returnStoreReceipt,
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
      return returnStoreReceipt;
    }
    #endregion
    #region Edit
    public ReturnStoreReceipt EditReturnStoreReceipt(
        int id,
        byte[] rowVersion,
        TValue<int?> receiptId = null,
        TValue<int> cooperatorId = null,
        TValue<int> stuffId = null,
        TValue<double> amount = null,
        TValue<byte> unitId = null,
        TValue<int> boxNo = null,
        TValue<int> qtyPerBox = null,
        TValue<string> description = null)
    {

      var entity = GetReturnStoreReceipt(id: id);

      return EditReturnStoreReceipt(
                    returnStoreReceipt: entity,
                    rowVersion: rowVersion,
                    receiptId: receiptId,
                    cooperatorId: cooperatorId,
                    stuffId: stuffId,
                    amount: amount,
                    unitId: unitId,
                    boxNo: boxNo,
                    qtyPerBox: qtyPerBox,
                    description: description);
    }
    public ReturnStoreReceipt EditReturnStoreReceipt(
        ReturnStoreReceipt returnStoreReceipt,
        byte[] rowVersion,
        TValue<int?> receiptId = null,
        TValue<int> cargoItemId = null,
        TValue<int> cooperatorId = null,
        TValue<int> stuffId = null,
        TValue<double> amount = null,
        TValue<byte> unitId = null,
        TValue<int> boxNo = null,
        TValue<int> qtyPerBox = null,
        TValue<string> description = null)
    {

      var retValue = EditStoreReceipt(
                    storeReceipt: returnStoreReceipt,
                    rowVersion: rowVersion,
                    receiptId: receiptId,
                    cooperatorId: cooperatorId,
                    stuffId: stuffId,
                    amount: amount,
                    unitId: unitId,
                    description: description);
      return retValue as ReturnStoreReceipt;
    }
    #endregion
    #region Get
    public ReturnStoreReceipt GetReturnStoreReceipt(int id) => GetReturnStoreReceipt(selector: e => e, id: id);
    public TResult GetReturnStoreReceipt<TResult>(
        Expression<Func<ReturnStoreReceipt, TResult>> selector,
        int id)
    {

      var orderItemBlock = GetReturnStoreReceipts(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (orderItemBlock == null)
        throw new ReturnStoreReceiptNotFoundException(id);
      return orderItemBlock;
    }
    public ReturnStoreReceipt GetReturnStoreReceipt(string code) => GetReturnStoreReceipt(selector: e => e, code: code);
    public TResult GetReturnStoreReceipt<TResult>(
        Expression<Func<ReturnStoreReceipt, TResult>> selector,
        string code)
    {

      var orderItemBlock = GetReturnStoreReceipts(
                    selector: selector,
                    code: code)


                .FirstOrDefault();
      if (orderItemBlock == null)
        throw new ReturnStoreReceiptNotFoundException(code);
      return orderItemBlock;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetReturnStoreReceipts<TResult>(
        Expression<Func<ReturnStoreReceipt, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<int> cargoItemId = null,
        TValue<int> receiptId = null,
        TValue<int> providerId = null,
        TValue<int> stuffId = null,
        TValue<double> amount = null,
        TValue<int> unitId = null,
        TValue<string> description = null,
        TValue<int> transactionBatchId = null,
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
      var returnStoreReceipt = baseQuery.OfType<ReturnStoreReceipt>();
      return returnStoreReceipt.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<ReturnStoreReceipt, ReturnStoreReceiptResult>> ToReturnStoreReceiptResult =
        returnStoreReceipt => new ReturnStoreReceiptResult
        {
          //Id = returnStoreReceipt.Id,
          //Code = returnStoreReceipt.Code,
          ////todo fix
          ////ReceiptId = returnStoreReceipt.ReceiptId.Value,
          //ReceiptCode = returnStoreReceipt.Receipt.Code,
          ////WarehouseId = returnStoreReceipt.Receipt.WarehouseId,
          ////WarehouseName = returnStoreReceipt.Receipt.Warehouse.Name,
          ////InboundCargoId = returnStoreReceipt.Receipt.InboundCargoId,
          ////InboundCargoCode = returnStoreReceipt.Receipt.InboundCargo.Code,
          //CooperatorId = returnStoreReceipt.CooperatorId,
          //CooperatorName = returnStoreReceipt.Cooperator.Name,
          //StuffId = returnStoreReceipt.StuffId,
          //StuffName = returnStoreReceipt.Stuff.Name,
          //StuffNoun = returnStoreReceipt.Stuff.Noun,
          //Amount = returnStoreReceipt.Amount,
          //BillOfMaterialVersion = returnStoreReceipt.BillOfMaterialVersion,
          //UnitId = returnStoreReceipt.UnitId,
          //UnitName = returnStoreReceipt.Unit.Name,
          //BoxNo = returnStoreReceipt.BoxNo,
          //QtyPerBox = returnStoreReceipt.QtyPerBox,
          //CargoItemId = returnStoreReceipt.CargoItemId,
          //CargoItemCode = returnStoreReceipt.CargoItem.Code,
          //CargoId = returnStoreReceipt.CargoItem.CargoId,
          //CargoCode = returnStoreReceipt.CargoItem.Cargo.Code,
          //PurchaseOrderId = returnStoreReceipt.CargoItem.PurchaseOrderId,
          //PurchaseOrderCode = returnStoreReceipt.CargoItem.PurchaseOrder.Code,
          //RowVersion = returnStoreReceipt.RowVersion
        };
    #endregion
    #region AddProcess
    public ReturnStoreReceipt AddReturnStoreReceiptProcess(
        ReturnStoreReceipt returnStoreReceipt,
        TransactionBatch transactionBatch,
        int cooperatorId,
        int stuffId,
        short? billOfMaterialVersion,
        double amount,
        byte unitId,
        int inboundCargoId,
        short warehouseId,
        string description,
        AddReturnOfSaleInput[] addReturnOfSales,
        bool stuffNeedToQualityControl)
    {


      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      transactionBatch = transactionBatch ?? warehouseManagement.AddTransactionBatch();
      #endregion
      #region AddReturnStoreReceipt
      returnStoreReceipt = AddReturnStoreReceipt(
              returnStoreReceipt: returnStoreReceipt,
              transactionBatch: transactionBatch,
              receiptId: null,
              cooperatorId: cooperatorId,
              stuffId: stuffId,
              billOfMaterialVersion: billOfMaterialVersion,
              amount: amount,
              unitId: unitId,
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
              storeReceiptId: returnStoreReceipt.Id);
      #endregion
      #region AddReturnOfSales
      var addQualityControlItemTransactionInputs = new List<AddQualityControlItemTransactionInput>();
      foreach (var addReturnOfSale in addReturnOfSales)
      {
        #region Get serial and AddReturnOfSale
        //var serial = CheckCrcAndGetSerial(serial: addReturnOfSale.Serial)
        //    
        //; ==> no Need because serial from ui is valid if not found or ... GetStuffSerial throw exception
        var stuffSerial = GetStuffSerial(selector: e => new { e.StuffId, e.Code, e.RowVersion, e.Serial, e.BillOfMaterialVersion },
                serial: addReturnOfSale.Serial);

        //Update stuff serial for warehouse inventory report (Who and when serial enter to warehouse)
        EditStuffSerial(
            stuffId: stuffSerial.StuffId,
            code: stuffSerial.Code,
            rowVersion: stuffSerial.RowVersion,
            warehouseEnterTime: DateTime.UtcNow,
            issueUserId: App.Providers.Security.CurrentLoginData.UserId
            );

        var warehouseInventory = App.Internals.WarehouseManagement.GetStuffSerialInventories(stuffId: stuffId,
                      stuffSerialCode: stuffSerial.Code);

        if (warehouseInventory.Any())
        {
          throw new ExitReceiptForSerialNotFoundException(serial: stuffSerial.Serial);
        }


        var returnOfSale = AddReturnOfSale(
                      returnOfSale: null,
                      transactionBatch: null,
                      sendProductId: addReturnOfSale.SendProductId,
                      stuffId: addReturnOfSale.StuffId,
                      mainStuffId: addReturnOfSale.PreparingSendingStuffId,
                      stuffSerialCode: stuffSerial.Code,
                      qty: addReturnOfSale.Amount,
                      unitId: addReturnOfSale.UnitId,
                      description: addReturnOfSale.Description,
                      type: ReturnOfSaleType.Type1,
                      returnStoreReceiptId: returnStoreReceipt.Id,
                      exitReceiptCode: addReturnOfSale.ExitReceiptCode);



        #region AddReturnOfSaleSummary

        AddReturnOfSaleSummary(
                qualityControlPassedQty: 0,
                qualityControlFailedQty: 0,
                qualityControlConsumedQty: 0,
                returnOfSaleId: returnOfSale.Id);

        #endregion




        #endregion
        #region AddTransactions
        var importAvailableTransaction = App.Internals.WarehouseManagement
            .AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: stuffId,
                billOfMaterialVersion: stuffSerial.BillOfMaterialVersion,
                stuffSerialCode: stuffSerial.Code,
                warehouseId: warehouseId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAvailable.Id,
                amount: addReturnOfSale.Amount,
                unitId: unitId,
                description: null,
                referenceTransaction: null);

        #endregion
        #region Add AddQualityControlItemTransactionInput
        var addQualityControlItemTransactionInput = new AddQualityControlItemTransactionInput()
        {
          StuffSerialCode = stuffSerial.Code,
          Amount = addReturnOfSale.Amount,
          UnitId = addReturnOfSale.UnitId,
          ReturnOfSaleId = returnOfSale.Id,
          Description = null
        };
        addQualityControlItemTransactionInputs.Add(addQualityControlItemTransactionInput);
        #endregion
        #region Check Cooperator
        if (addReturnOfSale.SendProductId != null)
        {
          var sendProductCooperatorId = warehouseManagement
                    .GetSendProduct(e => e.PreparingSending.SendPermission.ExitReceiptRequest.CooperatorId,
                        id: addReturnOfSale.SendProductId.Value);
          if (cooperatorId != sendProductCooperatorId)
          {
            throw new SendProductCooperatorIsNotEqualToStoreReceiptCooperatorException(stuffSerial.Serial);
          }

        }
        #endregion
      }
      #endregion
      #region Get stuff details
      var stuff = App.Internals
                      .SaleManagement
                      .GetStuff(e => new { e.Id, e.NeedToQualityControl }, id: stuffId);
      #endregion
      #region AddReceiptQualityControlProcess

      if (stuff.NeedToQualityControl)
      {
        App.Internals.QualityControl.AddReceiptQualityControlProcess(
                      receiptQualityControl: null,
                      transactionBatch: null,
                      storeReceiptId: returnStoreReceipt.Id,
                      stuffId: stuffId,
                      warehouseId: warehouseId,
                      description: null,
                      addQualityControlItemTransactionInputs: addQualityControlItemTransactionInputs.ToArray());
      }
      #endregion
      return returnStoreReceipt;
    }
    #endregion

    #region Search
    public IQueryable<ReturnStoreReceiptResult> SearchReturnStoreReceiptResult(IQueryable<ReturnStoreReceiptResult> query,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                select item;
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ReturnStoreReceiptResult> SortReturnStoreReceiptResult(IQueryable<ReturnStoreReceiptResult> query,
        SortInput<ReturnStoreReceiptSortType> sort)
    {
      switch (sort.SortType)
      {
        case ReturnStoreReceiptSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ReturnStoreReceiptSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.StockAdjustment;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public StockAdjustment AddStockAdjustment(
        StockAdjustment stockAdjustment,
        TransactionBatch transactionBatch,
        int stockCheckingTagId,
        double amount,
        byte unitId,
        string description)
    {

      stockAdjustment = stockAdjustment ?? repository.Create<StockAdjustment>();
      stockAdjustment.StockCheckingTagId = stockCheckingTagId;
      stockAdjustment.Amount = amount;
      stockAdjustment.UnitId = unitId;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: stockAdjustment,
                    transactionBatch: transactionBatch,
                    description: description);
      return stockAdjustment;
    }
    #endregion
    #region Edit
    public StockAdjustment EditStockAdjustment(
        int id,
        byte[] rowVersion,
        TValue<int> stockCheckingTagId = null,
        TValue<double> amount = null,
        TValue<byte> unitId = null,
        TValue<string> description = null)
    {

      var stockAdjustment = GetStockAdjustment(id: id);
      return EditStockAdjustment(
                    stockAdjustment: stockAdjustment,
                    rowVersion: rowVersion,
                    stockCheckingTagId: stockCheckingTagId,
                    amount: amount,
                    unitId: unitId,
                    description: description);
    }
    public StockAdjustment EditStockAdjustment(
        StockAdjustment stockAdjustment,
        byte[] rowVersion,
        TValue<int> stockCheckingTagId = null,
        TValue<double> amount = null,
        TValue<byte> unitId = null,
        TValue<string> description = null)
    {

      if (stockCheckingTagId != null)
        stockAdjustment.StockCheckingTagId = stockCheckingTagId;
      if (amount != null)
        stockAdjustment.Amount = amount;
      if (unitId != null)
        stockAdjustment.UnitId = unitId;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: stockAdjustment,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as StockAdjustment;
    }
    #endregion
    #region Get
    public StockAdjustment GetStockAdjustment(int id) => GetStockAdjustment(selector: e => e, id: id);
    public TResult GetStockAdjustment<TResult>(
        Expression<Func<StockAdjustment, TResult>> selector,
        int id)
    {

      var result = GetStockAdjustments(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new StockAdjustmentNotFoundException(id);
      return result;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStockAdjustments<TResult>(
        Expression<Func<StockAdjustment, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> stockCheckingTagId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var stockAdjustment = baseQuery.OfType<StockAdjustment>();
      if (stockCheckingTagId != null)
        stockAdjustment = stockAdjustment.Where(r => r.StockCheckingTagId == stockCheckingTagId);
      return stockAdjustment.Select(selector);
    }
    #endregion
    #region AddProcess
    public StockAdjustment AddStockAdjustmentProcess(
        int stockCheckingId,
        short warehouseId,
        int tagTypeId,
        int stuffId,
        double amount,
        byte unitId,
        long stuffSerialCode,
        string serial,
        string description)
    {

      #region AddTransactionBatch
      var transactionBatch = AddTransactionBatch();
      #endregion
      #region GetStockCheckingTag
      var stockCheckingTags = GetStockCheckingTags(
              selector: e => e,
              stockCheckingId: stockCheckingId,
              warehouseId: warehouseId,
              tagTypeId: tagTypeId,
              stuffId: stuffId,
              stuffSerialCode: stuffSerialCode);
      var stockCheckingTag = stockCheckingTags.FirstOrDefault();

      long? serialCode = stuffSerialCode;
      if (stockCheckingTag != null)
      {
        serialCode = stockCheckingTag.StuffSerialCode;
      }

      #endregion
      #region Add StockAdjustment
      var stockAdjustment = AddStockAdjustment(
              stockAdjustment: null,
              transactionBatch: transactionBatch,
              stockCheckingTagId: stockCheckingTag.Id,
              amount: amount,
              unitId: unitId,
              description: description);
      #endregion
      #region AddTransactions
      #region GetStock
      var warehouseInventory = GetStuffSerialInventories(
              warehouseId: warehouseId,
              stuffId: stuffId,
              stuffSerialCode: stuffSerialCode)

          .FirstOrDefault();
      #endregion
      #region GetTransactionAmount

      double transactionAmount = 0;

      if (warehouseInventory != null)
      {
        transactionAmount = (stockAdjustment.Amount * stockAdjustment.Unit.ConversionRatio) - warehouseInventory.TotalAmount;
        //if (warehouseInventory.TotalAmount != 0)

        //else
        //{
        //    transactionAmount = (stockAdjustment.Amount * stockAdjustment.Unit.ConversionRatio);
        //}
      }
      else
      {
        transactionAmount = (stockAdjustment.Amount * stockAdjustment.Unit.ConversionRatio);
      }


      #endregion
      var version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
                     stuffId: stuffId,
                         stuffSerialCode: serialCode);
      if (transactionAmount > 0)
      {
        #region Import Available Stock Adjustment Transaction
        var importAvailableStockAdjustmentTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: stuffId,
                billOfMaterialVersion: version,
                stuffSerialCode: serialCode,
                warehouseId: warehouseId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAvailableStockAdjustment.Id,
                amount: transactionAmount / stockAdjustment.Unit.ConversionRatio,
                unitId: stockAdjustment.UnitId,
                description: null,
                referenceTransaction: null);
        #endregion
      }
      else if (transactionAmount < 0)
      {
        #region Export Available Stock Adjustment
        var exportAvailableTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: stuffId,
                billOfMaterialVersion: version,
                stuffSerialCode: serialCode,
                warehouseId: warehouseId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportAvailableStockAdjustment.Id,
                amount: (transactionAmount / stockAdjustment.Unit.ConversionRatio) * -1,
                unitId: stockAdjustment.UnitId,
                description: null,
                referenceTransaction: null);
        #endregion
      }
      #endregion
      return stockAdjustment;
    }
    #endregion
    #region Search
    public IQueryable<StockAdjustmentResult> SearchStockAdjustmentResult(
        IQueryable<StockAdjustmentResult> query,
        string search,
        int? customerId,
        int? orderItemId,
        int? stuffId,
        int? orderItemBlockId)
    {
      //if (!string.IsNullOrEmpty(search))
      //    query = query.Where(item =>
      //        item.StuffCode.Contains(search) ||
      //        item.StuffName.Contains(search));
      //if (customerId != null)
      //    query = query.Where(i => i.CustomerId == customerId);
      //if (orderItemId != null)
      //    query = query.Where(i => i.OrderItemId == orderItemId);
      //if (stuffId != null)
      //    query = query.Where(i => i.StuffId == stuffId);
      //if (orderItemBlockId != null)
      //    query = query.Where(i => i.OrderItemBlockId == orderItemBlockId);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StockAdjustmentResult> SortSendProductResult(IQueryable<StockAdjustmentResult> query,
        SortInput<StockAdjustmentSortType> sort)
    {
      switch (sort.SortType)
      {
        //case StockAdjustmentSortType.Code:
        //    return query.OrderBy(a => a.Code, sort.SortOrder);
        //case StockAdjustmentSortType.StuffCode:
        //    return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        //case StockAdjustmentSortType.StuffName:
        //    return query.OrderBy(a => a.StuffName, sort.SortOrder);
        //case StockAdjustmentSortType.OrderItemCode:
        //    return query.OrderBy(a => a.OrderItemCode, sort.SortOrder);
        //case StockAdjustmentSortType.SendPermissionCode:
        //    return query.OrderBy(a => a.SendPermissionCode, sort.SortOrder);
        //case StockAdjustmentSortType.Amount:
        //    return query.OrderBy(a => a.Amount, sort.SortOrder);
        //case StockAdjustmentSortType.Status:
        //    return query.OrderBy(a => a.Status, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToStockAdjustmentResult
    public Expression<Func<StockAdjustment, StockAdjustmentResult>> ToStockAdjustmentResult =
        stockAdjustment => new StockAdjustmentResult
        {
          //Id = stockAdjustment.Id,
          //Code = stockAdjustment.Code,
          //StockCheckingTagId = stockAdjustment.StockCheckingTagId,
          //SendPermissionCode = stockAdjustment.SendPermission.Code,
          //CustomerId = stockAdjustment.SendPermission.OrderItemBlock.OrderItem.Order.CustomerId,
          //CustomerCode = stockAdjustment.SendPermission.OrderItemBlock.OrderItem.Order.Customer.Code,
          //CustomerName = stockAdjustment.SendPermission.OrderItemBlock.OrderItem.Order.Customer.Name,
          //OrderItemId = stockAdjustment.SendPermission.OrderItemBlock.OrderItemId,
          //OrderItemCode = stockAdjustment.SendPermission.OrderItemBlock.OrderItem.Code,
          //OrderItemUnitId = stockAdjustment.SendPermission.OrderItemBlock.OrderItem.UnitId,
          //OrderItemUnitName = stockAdjustment.SendPermission.OrderItemBlock.OrderItem.Unit.Name,
          //OrderItemAmount = stockAdjustment.SendPermission.OrderItemBlock.Amount,
          //OrderItemBlockId = stockAdjustment.SendPermission.OrderItemBlockId,
          //OrderItemBlockAmount = stockAdjustment.SendPermission.OrderItemBlock.Amount,
          //OrderItemBlockUnitId = stockAdjustment.SendPermission.OrderItemBlock.UnitId,
          //OrderItemBlockUnitName = stockAdjustment.SendPermission.OrderItemBlock.Unit.Name,
          //StuffId = stockAdjustment.SendPermission.OrderItemBlock.OrderItem.StuffId,
          //StuffCode = stockAdjustment.SendPermission.OrderItemBlock.OrderItem.Stuff.Code,
          //StuffName = stockAdjustment.SendPermission.OrderItemBlock.OrderItem.Stuff.Name,
          //SendPermissionAmount = stockAdjustment.SendPermission.Amount,
          //SendPermissionUnitId = stockAdjustment.SendPermission.UnitId,
          //SendPermissionUnitName = stockAdjustment.SendPermission.Unit.Name,
          //Amount = stockAdjustment.Amount,
          //UnitId = stockAdjustment.UnitId,
          //UnitName = stockAdjustment.Unit.Name,
          //UnitConversionRatio = stockAdjustment.Unit.ConversionRatio,
          //DateTime = stockAdjustment.DateTime,
          //SendProductId = stockAdjustment.SendProduct.Id,
          //ExitReceiptId = stockAdjustment.SendProduct.ExitReceiptId,
          //ExitReceiptDateTime = stockAdjustment.SendProduct.ExitReceipt.DateTime,
          //ExitReceiptConfirm = stockAdjustment.SendProduct.ExitReceipt.Confirmed,
          //Status = stockAdjustment.Status,
          //RowVersion = stockAdjustment.RowVersion
        };
    #endregion
  }
}

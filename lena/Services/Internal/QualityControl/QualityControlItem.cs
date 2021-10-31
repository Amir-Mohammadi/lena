using System;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.QualityControl.QualityControlItem;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Common;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Domains.Enums;
using lena.Models.QualityControl.QualityControlSample;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region AddProcess
    public QualityControlItem AddQualityControlItemProcess(
        TransactionBatch transactionBatch,
        string description,
        int qualityControlId,
        int stuffId,
        short warehouseId,
        long? stuffSerialCode,
        double qty,
        byte unitId,
        int? returnOfSaleId)
    {
      #region AddTransactionBatch
      transactionBatch = transactionBatch ?? App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region Add QualityControlItem
      var qualityControlItem = AddQualityControlItem(
              qualityControlItem: null,
              transactionBatch: transactionBatch,
              description: description,
              qualityControlId: qualityControlId,
              stuffId: stuffId,
              stuffSerialCode: stuffSerialCode,
              unitId: unitId,
              qty: qty,
              returnOfSaleId: returnOfSaleId);
      #endregion
      var version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
              stuffSerialCode: stuffSerialCode,
              stuffId: stuffId);
      var serialWarehouseInvenory = App.Internals.WarehouseManagement.GetStuffSerialInventories(
                    stuffId: stuffId,
                    stuffSerialCode: stuffSerialCode)
                .FirstOrDefault();
      if (serialWarehouseInvenory?.WarehouseId != null)
        warehouseId = serialWarehouseInvenory.WarehouseId;
      #region Add ExportAvailable WarehouseTransaction
      var exportAvailableTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
              warehouseId: warehouseId,
              transactionBatchId: transactionBatch.Id,
              effectDateTime: transactionBatch.DateTime,
              stuffId: stuffId,
              billOfMaterialVersion: version,
              stuffSerialCode: stuffSerialCode,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportAvailable.Id,
              amount: qty,
              unitId: unitId,
              description: null,
              referenceTransaction: null);
      #endregion
      #region Add ImportQualityControl WarehouseTransaction
      App.Internals.WarehouseManagement.AddWarehouseTransaction(
              warehouseId: warehouseId,
              transactionBatchId: transactionBatch.Id,
              effectDateTime: transactionBatch.DateTime,
              stuffId: stuffId,
              billOfMaterialVersion: version,
              stuffSerialCode: stuffSerialCode,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportQualityControl.Id,
              amount: qty,
              unitId: unitId,
              description: "",
              referenceTransaction: exportAvailableTransaction);
      #endregion
      return qualityControlItem;
    }
    #endregion
    #region Add
    public QualityControlItem AddQualityControlItem(
        QualityControlItem qualityControlItem,
        TransactionBatch transactionBatch,
        string description,
        int qualityControlId,
        int stuffId,
        long stuffSerialCode,
        double qty,
        byte unitId,
        int? returnOfSaleId)
    {
      qualityControlItem = qualityControlItem ?? repository.Create<QualityControlItem>();
      qualityControlItem.QualityControlId = qualityControlId;
      qualityControlItem.StuffId = stuffId;
      qualityControlItem.StuffSerialCode = stuffSerialCode;
      qualityControlItem.Qty = qty;
      qualityControlItem.UnitId = unitId;
      qualityControlItem.Status = false;
      qualityControlItem.ReturnOfSaleId = returnOfSaleId;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: qualityControlItem,
                    transactionBatch: transactionBatch,
                    description: description);
      return qualityControlItem;
    }
    #endregion
    #region Edit
    public QualityControlItem EditQualityControlItem(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<int> qualityControlId = null,
        TValue<int> stuffId = null,
        TValue<long> stuffSerialCode = null,
        TValue<bool> status = null,
        TValue<double> qty = null)
    {
      var qualityControlItem = GetQualityControlItem(id: id);
      return EditQualityControlItem(
                    qualityControlItem: qualityControlItem,
                    rowVersion: rowVersion,
                    description: description,
                    qualityControlId: qualityControlId,
                    stuffId: stuffId,
                    stuffSerialCode: stuffSerialCode,
                    status: status,
                    qty: qty);
    }
    public QualityControlItem EditQualityControlItem(
        QualityControlItem qualityControlItem,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<int> qualityControlId = null,
        TValue<int> stuffId = null,
        TValue<long> stuffSerialCode = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<bool> status = null)
    {
      if (qualityControlId != null)
        qualityControlItem.QualityControlId = qualityControlId;
      if (stuffId != null)
        qualityControlItem.StuffId = stuffId;
      if (stuffSerialCode != null)
        qualityControlItem.StuffSerialCode = stuffSerialCode;
      if (unitId != null)
        qualityControlItem.UnitId = unitId;
      if (qty != null)
        qualityControlItem.Qty = qty;
      if (status != null)
        qualityControlItem.Status = status;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: qualityControlItem,
                    rowVersion: rowVersion,
                    description: description);
      return retValue as QualityControlItem;
    }
    #endregion
    #region Get
    public QualityControlItem GetQualityControlItem(int id) =>
        GetQualityControlItem(selector: e => e, id: id);
    public TResult GetQualityControlItem<TResult>(
        Expression<Func<QualityControlItem, TResult>> selector,
            int id)
    {
      var qualityControlItem = GetQualityControlItems(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (qualityControlItem == null)
        throw new QualityControlItemNotFoundException(id);
      return qualityControlItem;
    }
    public QualityControlItem GetQualityControlItem(int qualityControlId, string serial) =>
        GetQualityControlItem(selector: e => e,
            qualityControlId: qualityControlId,
            serial: serial);
    public TResult GetQualityControlItem<TResult>(
        Expression<Func<QualityControlItem, TResult>> selector,
        int qualityControlId,
        string serial)
    {
      var qualityControlItem = GetQualityControlItems(
                    selector: selector,
                    qualityControlId: qualityControlId,
                    isDelete: false,
                    serial: serial)
                .FirstOrDefault();
      if (qualityControlItem == null)
        throw new QualityControlItemNotFoundException(
                  qualityControlId: qualityControlId,
                  serial: serial);
      return qualityControlItem;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetQualityControlItems<TResult>(
        Expression<Func<QualityControlItem, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> qualityControlIds = null,
        TValue<int[]> qualityControlItemIds = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<int> qualityControlDepartmentId = null,
        TValue<string> description = null,
        TValue<int> qualityControlId = null,
        TValue<int> stuffId = null,
        TValue<long> stuffSerialCode = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<string> serial = null,
        TValue<int> serialProfileCode = null,
        TValue<int> storeReceiptId = null,
        TValue<bool> status = null,
        TValue<QualityControlStatus> qualityControlStatus = null,
        TValue<QualityControlType> qualityControlType = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<lena.Domains.QualityControlItem>();
      if (qualityControlId != null)
        query = query.Where(i => i.QualityControlId == qualityControlId);
      if (qualityControlIds != null)
        query = query.Where(i => qualityControlIds.Value.Contains(i.QualityControlId));
      if (qualityControlItemIds != null)
        query = query.Where(i => qualityControlItemIds.Value.Contains(i.Id));
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (qualityControlDepartmentId != null)
        query = query.Where(i => i.Stuff.StuffPurchaseCategory.QualityControlDepartmentId == qualityControlDepartmentId);
      if (stuffSerialCode != null)
        query = query.Where(i => i.StuffSerialCode == stuffSerialCode);
      if (unitId != null)
        query = query.Where(i => i.UnitId == unitId);
      if (qty != null)
        query = query.Where(i => i.Qty == qty);
      if (qualityControlType != null)
        query = query.Where(i => i.QualityControl.QualityControlType == qualityControlType);
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial);
        query = query.Where(i => i.StuffSerial.Serial == serial);
      }
      if (serialProfileCode != null)
      {
        if (stuffId == null)
          throw new CannotGetSerialProfileWithoutStuffIdException();
        query = query.Where(i => i.StuffSerial.SerialProfileCode == serialProfileCode && i.StuffSerial.StuffId == stuffId);
      }
      if (storeReceiptId != null)
      {
        query = query.Where(i => (i.QualityControl as ReceiptQualityControl).StoreReceiptId == storeReceiptId);
      }
      if (status != null)
        query = query.Where(i => i.Status == status);
      if (qualityControlStatus != null)
        query = query.Where(i => i.QualityControl.Status == qualityControlStatus);
      return query.Select(selector);
    }
    #endregion
    #region RemoveProcess
    public void RemoveQualityControlItemProcess(
        int? transactionBatchId,
        int id,
        byte[] rowVersion)
    {
      var qualityControlItem = GetQualityControlItem(id: id);
      App.Internals.ApplicationBase.
      #region Edit BaseEntity
                    EditBaseEntity(baseEntity: qualityControlItem,
                    rowVersion: rowVersion,
                    isDelete: true);
      #endregion
      #region GetWarehouseInventory
      var warehouseInventory = App.Internals.WarehouseManagement.GetStuffSerialInventories(
              stuffId: qualityControlItem.StuffId,
              stuffSerialCode: qualityControlItem.StuffSerialCode)
          .FirstOrDefault();
      #endregion
      #region AddTransactions
      if (warehouseInventory != null)
      {
        if (warehouseInventory.BlockedAmount > 0)
          throw new StuffSerialIsBlockedException(serial: qualityControlItem.StuffSerial.Serial);
        var warehouseId = warehouseInventory.WarehouseId;
        #region TransactionBatch
        var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
        #endregion
        var version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
                stuffSerialCode: qualityControlItem.StuffSerialCode,
                stuffId: qualityControlItem.StuffId);
        #region Add ExportQualityControl WarehouseTransaction
        var exportAvailableTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
                warehouseId: warehouseId,
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: qualityControlItem.StuffId,
                billOfMaterialVersion: version,
                stuffSerialCode: qualityControlItem.StuffSerialCode,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportQualityControl.Id,
                amount: warehouseInventory.QualityControlAmount,
                unitId: warehouseInventory.UnitId,
                description: "",
                referenceTransaction: null);
        #endregion
        #region Add ImportAvailable WarehouseTransaction
        App.Internals.WarehouseManagement.AddWarehouseTransaction(
                warehouseId: warehouseId,
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: qualityControlItem.StuffId,
                billOfMaterialVersion: version,
                stuffSerialCode: qualityControlItem.StuffSerialCode,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAvailable.Id,
                amount: warehouseInventory.QualityControlAmount,
                unitId: warehouseInventory.UnitId,
                description: "",
                referenceTransaction: exportAvailableTransaction);
        #endregion
      }
      #endregion
    }
    #endregion
    #region Sort
    public IOrderedQueryable<QualityControlItemResult> SortQualityControlItemResult(
        IQueryable<QualityControlItemResult> query,
        SortInput<QualityControlItemSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlItemSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case QualityControlItemSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case QualityControlItemSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case QualityControlItemSortType.StuffSerialCode:
          return query.OrderBy(a => a.StuffSerialCode, sort.SortOrder);
        case QualityControlItemSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case QualityControlItemSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case QualityControlItemSortType.ConsumeQty:
          return query.OrderBy(a => a.ConsumeQty, sort.SortOrder);
        case QualityControlItemSortType.TestQty:
          return query.OrderBy(a => a.TestQty, sort.SortOrder);
        case QualityControlItemSortType.RemainedQty:
          return query.OrderBy(a => a.RemainedQty, sort.SortOrder);
        case QualityControlItemSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case QualityControlItemSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case QualityControlItemSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<QualityControlItemResult> SearchQualityControlItemResult(
        IQueryable<QualityControlItemResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where item.Code.Contains(search)
                select item;
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<QualityControlItem, QualityControlItemResult>> ToQualityControlItemResult =
        qualityControlItem => new QualityControlItemResult
        {
          Id = qualityControlItem.Id,
          QualityControlId = qualityControlItem.QualityControlId,
          Code = qualityControlItem.Code,
          StuffId = qualityControlItem.StuffId,
          StuffCode = qualityControlItem.Stuff.Code,
          StuffName = qualityControlItem.Stuff.Name,
          StuffSerialCode = qualityControlItem.StuffSerialCode,
          Serial = qualityControlItem.StuffSerial.Serial,
          Status = qualityControlItem.Status,
          Qty = qualityControlItem.Qty,
          ConsumeQty = qualityControlItem.QualityControlConfirmationItem.ConsumeQty,
          RemainedQty = qualityControlItem.QualityControlConfirmationItem.RemainedQty,
          TestQty = qualityControlItem.QualityControlConfirmationItem.TestQty,
          UnitId = qualityControlItem.UnitId,
          UnitName = qualityControlItem.Unit.Name,
          Description = qualityControlItem.Description,
          QualityControlSamples = qualityControlItem.QualityControlSamples.AsQueryable().Select(App.Internals.QualityControl.ToQualityControlSampleResult),
          RowVersion = qualityControlItem.RowVersion
        };
    #endregion
  }
}
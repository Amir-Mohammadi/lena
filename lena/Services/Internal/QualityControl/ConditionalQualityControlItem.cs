using System.Linq;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using System;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.QualityControl.ConditionalQualityControlItem;


using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region AddProcess

    public ConditionalQualityControlItem AddConditionalQualityControlItemProcess(
        int conditionalQualityControlId,
        int qualityControlConfirmationItemId,
        string description)
    {

      #region Get ConditionalQualityControl
      var conditionalQualityControl = GetConditionalQualityControl(id: conditionalQualityControlId);
      #endregion
      #region Get QualityControlConfirmationItem
      var qualityControlConfirmationItem = GetQualityControlConfirmationItem(id: qualityControlConfirmationItemId);
      #endregion
      return AddConditionalQualityControlItemProcess(
              conditionalQualityControl: conditionalQualityControl,
              qualityControlConfirmationItem: qualityControlConfirmationItem,
              description: description);
    }

    public ConditionalQualityControlItem AddConditionalQualityControlItemProcess(
       ConditionalQualityControl conditionalQualityControl,
       QualityControlConfirmationItem qualityControlConfirmationItem,
       string description
        )
    {

      #region AddTransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region AddConditionalQualityControlItem
      var serial = qualityControlConfirmationItem.QualityControlItem.StuffSerial;
      var inventories = App.Internals.WarehouseManagement.GetStuffSerialInventories(
                    stuffId: qualityControlConfirmationItem.QualityControlItem.StuffId,
                    stuffSerialCode: qualityControlConfirmationItem.QualityControlItem.StuffSerialCode)


                .FirstOrDefault();
      if (inventories == null || inventories.WasteAmount == 0)
      {
        throw new SerialAmountIsNotEnougheException();
      }

      var qty = inventories.WasteAmount;
      var unitId = inventories.UnitId;

      var entity = AddConditionalQualityControlItem(
                    conditionalQualityControlItem: null,
                    transactionBatch: transactionBatch,
                    description: description,
                    conditionalQualityControlId: conditionalQualityControl.Id,
                    qualityControlConfirmationItemId: qualityControlConfirmationItem.Id,
                    qty: qty,
                    unitId: unitId);
      #endregion
      #region Get QualityControlItem
      var qualityControlItem = GetQualityControlItem(id: qualityControlConfirmationItem.QualityControlItem.Id);
      #endregion
      #region Get ImportWaste Transaction
      var importWasteTransaction = App.Internals.WarehouseManagement.GetWarehouseTransactions(
              selector: e => e,
              transactionBatchId: qualityControlConfirmationItem.TransactionBatch?.Id,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportWaste.Id)


          .First();
      #endregion
      var version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
              stuffSerialCode: qualityControlConfirmationItem.QualityControlItem.StuffSerialCode,
              stuffId: qualityControlConfirmationItem.QualityControlItem.StuffId);
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
        #region Add ExportWaste Transaction 
        var exportWasteTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: qualityControlItem.StuffId,
                billOfMaterialVersion: version,
                stuffSerialCode: qualityControlItem.StuffSerialCode,
                warehouseId: warehouseId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportWaste.Id,
                amount: qty,
                unitId: unitId,
                description: "",
                referenceTransaction: importWasteTransaction);

        #endregion
        #region Add ImportQualityControl Transaction
        App.Internals.WarehouseManagement.AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: qualityControlItem.StuffId,
                billOfMaterialVersion: version,
                stuffSerialCode: qualityControlItem.StuffSerialCode,
                warehouseId: warehouseId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportQualityControl.Id,
                amount: qty,
                unitId: unitId,
                description: "",
                referenceTransaction: exportWasteTransaction);

        #endregion
      }
      #endregion
      return entity;
    }

    #endregion
    #region Get
    public ConditionalQualityControlItem GetConditionalQualityControlItem(int id) => GetConditionalQualityControlItem(selector: e => e, id: id);
    public TResult GetConditionalQualityControlItem<TResult>(
        Expression<Func<ConditionalQualityControlItem, TResult>> selector,
        int id)
    {

      var conditionalQualityControlItem = GetConditionalQualityControlItems(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (conditionalQualityControlItem == null)
        throw new ConditionalQualityControlItemNotFoundException(id);
      return conditionalQualityControlItem;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetConditionalQualityControlItems<TResult>(
        Expression<Func<ConditionalQualityControlItem, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> conditionalQualityControlId = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<int> qualityControlConfirmationItemId = null,
        TValue<int> qualityControlItemId = null,
        TValue<int> qualityControlId = null,
        TValue<int[]> qualityControlIds = null,
        TValue<int> stuffId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<string> serial = null,
        TValue<long?> serialProfileCode = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<ConditionalQualityControlItem>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (conditionalQualityControlId != null)
        query = query.Where(x => x.ConditionalQualityControlId == conditionalQualityControlId);
      if (qualityControlId != null)
        query = query.Where(x => x.QualityControlConfirmationItem.QualityControlItem.QualityControlId == qualityControlId);
      if (qualityControlIds != null)
        query = query.Where(x => qualityControlIds.Value.Contains(x.QualityControlConfirmationItem.QualityControlItem.QualityControlId));
      if (qty != null)
        query = query.Where(x => x.Qty == qty);
      if (unitId != null)
        query = query.Where(x => x.UnitId == unitId);
      if (qualityControlConfirmationItemId != null)
        query = query.Where(x => x.QualityControlConfirmationItemId == qualityControlConfirmationItemId);
      if (qualityControlItemId != null)
        query = query.Where(x => x.QualityControlConfirmationItem.QualityControlItem.Id == qualityControlItemId);
      if (stuffId != null)
        query = query.Where(x => x.QualityControlConfirmationItem.QualityControlItem.StuffId == stuffId);
      if (stuffSerialCode != null)
        query = query.Where(x => x.QualityControlConfirmationItem.QualityControlItem.StuffSerialCode == stuffSerialCode);
      if (serial != null)
        query = query.Where(x => x.QualityControlConfirmationItem.QualityControlItem.StuffSerial.Serial == serial);
      if (serialProfileCode != null)
      {
        if (stuffId == null)
          throw new CannotGetSerialProfileWithoutStuffIdException();
        query = query.Where(i => i.QualityControlConfirmationItem.QualityControlItem.StuffSerial.SerialProfileCode == serialProfileCode && i.QualityControlConfirmationItem.QualityControlItem.StuffSerial.StuffId == stuffId);
      }

      return query.Select(selector);
    }
    #endregion
    #region Add

    public ConditionalQualityControlItem AddConditionalQualityControlItem(
        ConditionalQualityControlItem conditionalQualityControlItem,
        TransactionBatch transactionBatch,
        string description,
        int conditionalQualityControlId,
        int qualityControlConfirmationItemId,
        double qty,
        byte unitId)
    {

      conditionalQualityControlItem = conditionalQualityControlItem ?? repository.Create<ConditionalQualityControlItem>();
      conditionalQualityControlItem.ConditionalQualityControlId = conditionalQualityControlId;
      conditionalQualityControlItem.Qty = qty;
      conditionalQualityControlItem.UnitId = unitId;
      conditionalQualityControlItem.QualityControlConfirmationItemId = qualityControlConfirmationItemId;
      conditionalQualityControlItem.Description = description;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: conditionalQualityControlItem,
                    transactionBatch: transactionBatch,
                    description: description);
      return conditionalQualityControlItem;
    }

    #endregion
    #region Edit
    public ConditionalQualityControlItem EditConditionalQualityControlItem(
        int id,
        byte[] rowVersion,
        TValue<int> conditionalQualityControlId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<int> qualityControlConfirmationItemId = null,
        TValue<string> description = null)
    {

      var conditionalQualityControlItem = GetConditionalQualityControlItem(id: id);
      return EditConditionalQualityControlItem(
                    conditionalQualityControlItem: conditionalQualityControlItem,
                    rowVersion: rowVersion,
                    conditionalQualityControlId: conditionalQualityControlId,
                    qty: qty,
                    unitId: unitId,
                    qualityControlConfirmationItemId: qualityControlConfirmationItemId,
                    description: description);
    }
    public ConditionalQualityControlItem EditConditionalQualityControlItem(
        ConditionalQualityControlItem conditionalQualityControlItem,
        byte[] rowVersion,
        TValue<int> conditionalQualityControlId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<int> qualityControlConfirmationItemId = null,
        TValue<string> description = null)
    {


      if (conditionalQualityControlId != null)
        conditionalQualityControlItem.ConditionalQualityControlId = conditionalQualityControlId;
      if (qty != null)
        conditionalQualityControlItem.Qty = qty;
      if (unitId != null)
        conditionalQualityControlItem.UnitId = unitId;
      if (qualityControlConfirmationItemId != null)
        conditionalQualityControlItem.QualityControlConfirmationItemId = qualityControlConfirmationItemId;
      if (description != null)
        conditionalQualityControlItem.Description = description;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: conditionalQualityControlItem,
                    description: description,
                    rowVersion: rowVersion);
      return conditionalQualityControlItem;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ConditionalQualityControlItemResult> SortConditionalQualityControlItemResult(
        IQueryable<ConditionalQualityControlItemResult> query,
        SortInput<ConditionalQualityControlItemSortType> sort)
    {
      switch (sort.SortType)
      {
        case ConditionalQualityControlItemSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ConditionalQualityControlItemSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ConditionalQualityControlItemSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case ConditionalQualityControlItemSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case ConditionalQualityControlItemSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case ConditionalQualityControlItemSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case ConditionalQualityControlItemSortType.QualityControlConfirmationItemRemainedQty:
          return query.OrderBy(a => a.QualityControlConfirmationItemRemainedQty, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<ConditionalQualityControlItemResult> SearchConditionalQualityControlItemResult(
        IQueryable<ConditionalQualityControlItemResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                  //where
                  //    item.Description.Contains(search) ||
                  //    item.QualityControlTestCode.Contains(search) ||
                  //    item.QualityControlTestDescription.Contains(search) ||
                  //    item.QualityControlTestName.Contains(search)
                select item;
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<ConditionalQualityControlItem, ConditionalQualityControlItemResult>> ToConditionalQualityControlItemResult =
        conditionalQualityControlItem => new ConditionalQualityControlItemResult
        {
          Id = conditionalQualityControlItem.Id,
          Code = conditionalQualityControlItem.Code,
          Qty = conditionalQualityControlItem.Qty,
          UnitId = conditionalQualityControlItem.UnitId,
          UnitName = conditionalQualityControlItem.Unit.Name,
          Description = conditionalQualityControlItem.Description,
          StuffId = conditionalQualityControlItem.QualityControlConfirmationItem.QualityControlItem.StuffId,
          StuffSerialCode = conditionalQualityControlItem.QualityControlConfirmationItem.QualityControlItem.StuffSerialCode,
          Serial = conditionalQualityControlItem.QualityControlConfirmationItem.QualityControlItem.StuffSerial.Serial,
          QualityControlConfirmationItemId = conditionalQualityControlItem.QualityControlConfirmationItemId,
          QualityControlConfirmationItemRemainedQty = (conditionalQualityControlItem.QualityControlConfirmationItem.RemainedQty *
             conditionalQualityControlItem.QualityControlConfirmationItem.Unit.ConversionRatio) /
            conditionalQualityControlItem.Unit.ConversionRatio,
          RowVersion = conditionalQualityControlItem.RowVersion
        };
    #endregion
  }
}

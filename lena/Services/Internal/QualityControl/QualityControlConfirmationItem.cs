using System.Linq;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using System;
using lena.Models.QualityControl.QualityControlConfirmationItem;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Domains.Enums;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region AddProcess

    public QualityControlConfirmationItem AddQualityControlConfirmationItemProcess(
        TransactionBatch transactionBatch,
        string description,
        double testQty,
        double consumeQty,
        byte unitId,
        int qualityControlItemId,
        int qualityControlConfirmationId)
    {

      #region Get QualityControlItem

      var qualityControlItem = GetQualityControlItem(id: qualityControlItemId);

      #endregion
      #region EditQualityControlItem
      EditQualityControlItem(qualityControlItem: qualityControlItem,
              rowVersion: qualityControlItem.RowVersion,
              status: true);
      #endregion
      #region Get QualityControlConfirmation

      var qualityControlConfirmation = GetQualityControlConfirmation(id: qualityControlConfirmationId);

      #endregion
      return AddQualityControlConfirmationItemProcess(
              transactionBatch: transactionBatch,
              description: description,
              testQty: testQty,
              consumeQty: consumeQty,
              unitId: unitId,
              qualityControlItem: qualityControlItem,
              qualityControlConfirmation: qualityControlConfirmation);
    }

    public QualityControlConfirmationItem AddQualityControlConfirmationItemProcess(
       TransactionBatch transactionBatch,
       string description,
       double testQty,
       double consumeQty,
       byte unitId,
       QualityControlItem qualityControlItem,
       QualityControlConfirmation qualityControlConfirmation)
    {

      #region AddTransactionBatch
      transactionBatch = transactionBatch ?? App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region GetUnit
      var unit = App.Internals.ApplicationBase.GetUnit(id: unitId);
      #endregion
      #region Calculate RemainedQty
      var remainedQty =
          (qualityControlItem.Qty * qualityControlItem.Unit.ConversionRatio / unit.ConversionRatio) -
          consumeQty;
      #endregion
      #region AddQualityControlConfirmationItem
      var entity = AddQualityControlConfirmationItem(
              qualityControlConfirmationItem: null,
              transactionBatch: transactionBatch,
              description: description,
              remainedQty: remainedQty,
              testQty: testQty,
              consumeQty: consumeQty,
              unitId: unitId,
              qualityControlItemId: qualityControlItem.Id,
              qualityControlConfirmationId: qualityControlConfirmation.Id);
      #endregion
      #region GetVersion
      var version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
              stuffSerialCode: qualityControlItem.StuffSerialCode,
              stuffId: qualityControlItem.StuffId);
      #endregion
      #region Get ImportQualityControl Transaction

      var importQualityControlTransaction = App.Internals.WarehouseManagement.GetWarehouseTransactions(
              selector: e => e,
              transactionBatchId: qualityControlItem.TransactionBatch.Id,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportQualityControl.Id)


          .FirstOrDefault();
      #endregion
      #region GetWarehouseInventory
      var warehouseInventory = App.Internals.WarehouseManagement.GetStuffSerialInventories(
              warehouseId: null,
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
        #region Add ExportQualityControl Transaction
        var exportQualityControlTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: qualityControlItem.StuffId,
                billOfMaterialVersion: version,
                stuffSerialCode: qualityControlItem.StuffSerialCode,
                warehouseId: warehouseId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportQualityControl.Id,
                amount: remainedQty,
                unitId: unitId,
                description: null,
                referenceTransaction: importQualityControlTransaction);

        #endregion
        #region Add Consume WarehouseTransaction
        if (consumeQty > 0)
        {
          var importConsumeTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
                            transactionBatchId: transactionBatch.Id,
                            effectDateTime: transactionBatch.DateTime,
                            stuffId: qualityControlItem.StuffId,
                            billOfMaterialVersion: version,
                            stuffSerialCode: qualityControlItem.StuffSerialCode,
                            warehouseId: warehouseId,
                            transactionTypeId: Models.StaticData.StaticTransactionTypes.ConsumedQualityControl.Id,
                            amount: consumeQty,
                            unitId: unitId,
                            description: null,
                            referenceTransaction: exportQualityControlTransaction);
        }
        #endregion
        #region Add Final WarehouseTransaction
        var finalTransactionType = qualityControlConfirmation.Status == QualityControlStatus.Accepted
            ? Models.StaticData.StaticTransactionTypes.ImportAvailable
            : Models.StaticData.StaticTransactionTypes.ImportWaste;
        App.Internals.WarehouseManagement.AddWarehouseTransaction(
                      transactionBatchId: transactionBatch.Id,
                      effectDateTime: transactionBatch.DateTime,
                      stuffId: qualityControlItem.StuffId,
                      billOfMaterialVersion: version,
                      stuffSerialCode: qualityControlItem.StuffSerialCode,
                      warehouseId: warehouseId,
                      transactionTypeId: finalTransactionType.Id,
                      amount: qualityControlItem.Qty - consumeQty,
                      unitId: unitId,
                      description: "",
                      referenceTransaction: importQualityControlTransaction);

        #endregion
      }
      #endregion




      return entity;
    }

    #endregion
    #region Get
    public QualityControlConfirmationItem GetQualityControlConfirmationItem(int qualityControlConfirmationId,
        string serial) => GetQualityControlConfirmationItem(selector: e => e,
            qualityControlConfirmationId: qualityControlConfirmationId,
            serial: serial);
    public TResult GetQualityControlConfirmationItem<TResult>(
        Expression<Func<QualityControlConfirmationItem, TResult>> selector,
        int qualityControlConfirmationId,
        string serial)
    {

      var qualityControlConfirmationItem = GetQualityControlConfirmationItems(
                    selector: selector,
                    qualityControlConfirmationId: qualityControlConfirmationId,
                    serial: serial)


                .FirstOrDefault();
      if (qualityControlConfirmationItem == null)
        throw new QualityControlConfirmationItemNotFoundException(
                  qualityControlConfirmationId: qualityControlConfirmationId,
                  serial: serial);
      return qualityControlConfirmationItem;
    }


    public QualityControlConfirmationItem GetQualityControlConfirmationItem(int id) => GetQualityControlConfirmationItem(selector: e => e, id: id);
    public TResult GetQualityControlConfirmationItem<TResult>(
        Expression<Func<QualityControlConfirmationItem, TResult>> selector,
        int id)
    {

      var qualityControlConfirmationItem = GetQualityControlConfirmationItems(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (qualityControlConfirmationItem == null)
        throw new QualityControlConfirmationItemNotFoundException(id);
      return qualityControlConfirmationItem;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetQualityControlConfirmationItems<TResult>(
        Expression<Func<QualityControlConfirmationItem, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<double> remainedQty = null,
        TValue<double> testQty = null,
        TValue<double> consumeQty = null,
        TValue<int> unitId = null,
        TValue<int> qualityControlItemId = null,
        TValue<int> qualityControlConfirmationId = null,
        TValue<int> qualityControlId = null,
        TValue<int[]> qualityControlIds = null,
        TValue<string> serial = null,
        TValue<int> stuffId = null,
        TValue<long> stuffSerialCode = null,
        TValue<int> serialProfileCode = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<QualityControlConfirmationItem>();
      if (remainedQty != null)
        query = query.Where(x => x.RemainedQty == remainedQty);
      if (testQty != null)
        query = query.Where(x => x.TestQty == testQty);
      if (consumeQty != null)
        query = query.Where(x => x.ConsumeQty == consumeQty);
      if (unitId != null)
        query = query.Where(x => x.UnitId == unitId);
      if (stuffId != null)
        query = query.Where(x => x.QualityControlItem.StuffId == stuffId);
      if (stuffSerialCode != null)
        query = query.Where(x => x.QualityControlItem.StuffSerialCode == stuffSerialCode);
      if (qualityControlItemId != null)
        query = query.Where(x => x.QualityControlItem.Id == qualityControlItemId);
      if (qualityControlConfirmationId != null)
        query = query.Where(x => x.QualityControlConfirmationId == qualityControlConfirmationId);
      if (qualityControlId != null)
        query = query.Where(x => x.QualityControlConfirmation.QualityControl.Id == qualityControlId);
      if (qualityControlIds != null)
        query = query.Where(x => qualityControlIds.Value.Contains(x.QualityControlConfirmation.QualityControl.Id));
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial);
        query = query.Where(x => x.QualityControlItem.StuffSerial.Serial == serial);
      }
      if (serialProfileCode != null)
      {
        if (stuffId == null)
          throw new CannotGetSerialProfileWithoutStuffIdException();
        query = query.Where(i => i.QualityControlItem.StuffSerial.SerialProfileCode == serialProfileCode && i.QualityControlItem.StuffId == stuffId);
      }
      return query.Select(selector);
    }
    #endregion
    #region Add
    public QualityControlConfirmationItem AddQualityControlConfirmationItem(
        QualityControlConfirmationItem qualityControlConfirmationItem,
        TransactionBatch transactionBatch,
        string description,
        double remainedQty,
        double testQty,
        double consumeQty,
        byte unitId,
        int qualityControlItemId,
        int qualityControlConfirmationId)
    {

      qualityControlConfirmationItem = qualityControlConfirmationItem ?? repository.Create<QualityControlConfirmationItem>();
      qualityControlConfirmationItem.RemainedQty = remainedQty;
      qualityControlConfirmationItem.TestQty = testQty;
      qualityControlConfirmationItem.ConsumeQty = consumeQty;
      qualityControlConfirmationItem.UnitId = unitId;
      var qualityControlItem = GetQualityControlItem(id: qualityControlItemId);
      qualityControlConfirmationItem.QualityControlItem = qualityControlItem;
      qualityControlConfirmationItem.QualityControlConfirmationId = qualityControlConfirmationId;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: qualityControlConfirmationItem,
                    transactionBatch: transactionBatch,
                    description: description);
      return qualityControlConfirmationItem;
    }
    #endregion
    #region Edit
    public QualityControlConfirmationItem EditQualityControlConfirmationItem(
        int id,
        byte[] rowVersion,
        TValue<double> remainedQty = null,
        TValue<double> testQty = null,
        TValue<double> consumeQty = null,
        TValue<byte> unitId = null,
        TValue<int> qualityControlItemId = null,
        TValue<int> qualityControlConfirmationId = null)
    {

      var qualityControlConfirmationItem = GetQualityControlConfirmationItem(id: id);
      return EditQualityControlConfirmationItem(
                    qualityControlConfirmationItem: qualityControlConfirmationItem,
                    rowVersion: rowVersion,
                    remainedQty: remainedQty,
                    testQty: testQty,
                    consumeQty: consumeQty,
                    unitId: unitId,
                    qualityControlItemId: qualityControlItemId,
                    qualityControlConfirmationId: qualityControlConfirmationId);
    }
    public QualityControlConfirmationItem EditQualityControlConfirmationItem(
        QualityControlConfirmationItem qualityControlConfirmationItem,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<double> remainedQty = null,
        TValue<double> testQty = null,
        TValue<double> consumeQty = null,
        TValue<byte> unitId = null,
        TValue<int> qualityControlItemId = null,
        TValue<int> qualityControlConfirmationId = null)
    {

      if (remainedQty != null)
        qualityControlConfirmationItem.RemainedQty = remainedQty;
      if (testQty != null)
        qualityControlConfirmationItem.TestQty = testQty;
      if (consumeQty != null)
        qualityControlConfirmationItem.ConsumeQty = consumeQty;
      if (unitId != null)
        qualityControlConfirmationItem.UnitId = unitId;
      if (qualityControlItemId != null)
        qualityControlConfirmationItem.QualityControlItem.Id = qualityControlItemId;
      if (qualityControlConfirmationId != null)
        qualityControlConfirmationItem.QualityControlConfirmationId = qualityControlConfirmationId;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: qualityControlConfirmationItem,
                    rowVersion: rowVersion,
                    description: description);
      return qualityControlConfirmationItem;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<QualityControlConfirmationItemResult> SortQualityControlConfirmationItemResult(
        IQueryable<QualityControlConfirmationItemResult> query,
        SortInput<QualityControlConfirmationItemSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlConfirmationItemSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case QualityControlConfirmationItemSortType.RemainedQty:
          return query.OrderBy(a => a.RemainedQty, sort.SortOrder);
        case QualityControlConfirmationItemSortType.TestQty:
          return query.OrderBy(a => a.TestQty, sort.SortOrder);
        case QualityControlConfirmationItemSortType.ConsumeQty:
          return query.OrderBy(a => a.ConsumeQty, sort.SortOrder);
        case QualityControlConfirmationItemSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case QualityControlConfirmationItemSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case QualityControlConfirmationItemSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case QualityControlConfirmationItemSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case QualityControlConfirmationItemSortType.StuffSerialCode:
          return query.OrderBy(a => a.StuffSerialCode, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<QualityControlConfirmationItemResult> SearchQualityControlConfirmationItemResult(
        IQueryable<QualityControlConfirmationItemResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.StuffCode.Contains(search) ||
                    item.StuffName.Contains(search) ||
                    item.Serial.Contains(search)
                select item;
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<QualityControlConfirmationItem, QualityControlConfirmationItemResult>> ToQualityControlConfirmationItemResult =
        qualityControlConfirmationItem => new QualityControlConfirmationItemResult
        {
          Id = qualityControlConfirmationItem.Id,
          RemainedQty = qualityControlConfirmationItem.RemainedQty,
          TestQty = qualityControlConfirmationItem.TestQty,
          ConsumeQty = qualityControlConfirmationItem.ConsumeQty,
          UnitId = qualityControlConfirmationItem.UnitId,
          UnitName = qualityControlConfirmationItem.Unit.Name,
          QualityControlItemId = qualityControlConfirmationItem.QualityControlItem.Id,
          StuffId = qualityControlConfirmationItem.QualityControlItem.StuffId,
          StuffCode = qualityControlConfirmationItem.QualityControlItem.Stuff.Code,
          StuffName = qualityControlConfirmationItem.QualityControlItem.Stuff.Name,
          StuffSerialCode = qualityControlConfirmationItem.QualityControlItem.StuffSerialCode,
          Serial = qualityControlConfirmationItem.QualityControlItem.StuffSerial.Serial,
          QualityControlConfirmationId = qualityControlConfirmationItem.QualityControlConfirmationId,
          RowVersion = qualityControlConfirmationItem.RowVersion
        };
    #endregion
  }
}

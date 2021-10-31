using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.Common;
using lena.Services.Core;
using lena.Models.WarehouseManagement.QtyCorrectionRequest;
using lena.Models.WarehouseManagement.WarehouseInventory;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using lena.Services.Internals.Exceptions;
using lena.Models.ApplicationBase.Unit;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    internal QtyCorrectionRequest AddQtyCorrectionRequest(
        QtyCorrectionRequest qtyCorrectionRequest,
        TransactionBatch transactionBatch,
        int stuffId,
        short warehouseId,
        string serial,
        QtyCorrectionRequestType type,
        string description,
        double qty,
        byte unitId,
        int? stockCheckingTagId)
    {
      var warehouseInventory = App.Internals.WarehouseManagement.GetWarehouseInventories(
                    serial: serial,
                    groupBySerial: true)
                .FirstOrDefault();
      StuffSerial stuffSerial = null;
      if (serial != null)
      {
        stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: serial);
      }
      #region Check input
      if (warehouseInventory?.BlockedAmount > 0.00000001)
        throw new BlockAmountOfSerialCannotCorrectionRequestException(serial);
      if (warehouseInventory != null && warehouseInventory.WarehouseId != warehouseId)
        throw new SerialIsInAnOtherWarehouseException(serial: serial, warehouseInventory.WarehouseName);
      var duplicatedQtyCorrectionRequests = GetQtyCorrectionRequests(
                selector: e => e.Id,
                stuffId: stuffSerial.StuffId,
                stuffSerialCode: stuffSerial.Code,
                status: QtyCorrectionRequestStatus.NotAction);
      if (duplicatedQtyCorrectionRequests.Any())
        throw new HasQtyCorrectionRequestException(code: stuffSerial.Stuff.Code, serial: serial);
      #endregion
      qtyCorrectionRequest = qtyCorrectionRequest ?? repository.Create<QtyCorrectionRequest>();
      qtyCorrectionRequest.WarehouseId = warehouseId;
      qtyCorrectionRequest.StuffId = stuffId;
      if (stuffSerial == null)
      {
        qtyCorrectionRequest.StuffSerialCode = null;
        qtyCorrectionRequest.StuffSerial = null;
      }
      else
      {
        qtyCorrectionRequest.StuffSerialCode = stuffSerial.Code;
        qtyCorrectionRequest.StuffSerial = stuffSerial;
      }
      qtyCorrectionRequest.Type = type;
      qtyCorrectionRequest.Status = QtyCorrectionRequestStatus.NotAction;
      qtyCorrectionRequest.Qty = qty;
      if (qty <= 0)
        throw new QtyInvalidException(qty);
      qtyCorrectionRequest.UnitId = unitId;
      if (qtyCorrectionRequest.Type == QtyCorrectionRequestType.IncreaseStockChecking ||
                qtyCorrectionRequest.Type == QtyCorrectionRequestType.DecreaseStockChecking)
      {
        qtyCorrectionRequest.StockCheckingTagId = stockCheckingTagId;
      }
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: qtyCorrectionRequest,
                    transactionBatch: transactionBatch,
                    description: description);
      return qtyCorrectionRequest;
    }
    #endregion
    #region Edit
    public QtyCorrectionRequest EditQtyCorrectionRequest(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
    TValue<double> qty = null,
    TValue<byte> unitId = null,
    TValue<int> stuffId = null,
    TValue<long?> stuffSerialCode = null,
    TValue<QtyCorrectionRequestType> type = null,
    TValue<QtyCorrectionRequestStatus> status = null,
    TValue<short> warehouseId = null)
    {
      var qtyCorrectionRequest = GetQtyCorrectionRequest(id: id);
      return EditQtyCorrectionRequest(
                    qtyCorrectionRequest: qtyCorrectionRequest,
                    rowVersion: rowVersion,
                    description: description,
                    qty: qty,
                    unitId: unitId,
                    stuffId: stuffId,
                    stuffSerialCode: stuffSerialCode,
                    type: type,
                    status: status,
                    warehouseId: warehouseId);
    }
    public QtyCorrectionRequest EditQtyCorrectionRequest(
        QtyCorrectionRequest qtyCorrectionRequest,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<int> stuffId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<QtyCorrectionRequestType> type = null,
        TValue<QtyCorrectionRequestStatus> status = null,
        TValue<short> warehouseId = null)
    {
      if (qty != null)
        qtyCorrectionRequest.Qty = qty;
      if (unitId != null)
        qtyCorrectionRequest.UnitId = unitId;
      if (stuffId != null)
        qtyCorrectionRequest.StuffId = stuffId;
      if (stuffSerialCode != null)
        qtyCorrectionRequest.StuffSerialCode = stuffSerialCode;
      if (type != null)
        qtyCorrectionRequest.Type = type;
      if (status != null)
        qtyCorrectionRequest.Status = status;
      if (warehouseId != null)
        qtyCorrectionRequest.WarehouseId = warehouseId;
      App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: qtyCorrectionRequest,
                    description: description,
                    rowVersion: rowVersion);
      return qtyCorrectionRequest;
    }
    #endregion
    #region Get
    public QtyCorrectionRequest GetQtyCorrectionRequest(int id) => GetQtyCorrectionRequest(selector: e => e, id: id);
    public TResult GetQtyCorrectionRequest<TResult>(
        Expression<Func<QtyCorrectionRequest, TResult>> selector,
        int id)
    {
      var qtyCorrectionRequest = GetQtyCorrectionRequests(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (qtyCorrectionRequest == null)
        throw new QtyCorrectionRequestNotFoundException(id);
      return qtyCorrectionRequest;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetQtyCorrectionRequests<TResult>(
        Expression<Func<QtyCorrectionRequest, TResult>> selector,
        string serial = null,
        TValue<int> id = null,
        TValue<double> qty = null,
        TValue<int> userId = null,
        TValue<int> unitId = null,
        TValue<string> code = null,
        TValue<int> stuffId = null,
        TValue<bool> isDelete = null,
        TValue<int> warehouseId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<string> description = null,
        TValue<int> stockCheckingTagId = null,
        TValue<int> transactionBatchId = null,
        TValue<QtyCorrectionRequestType> type = null,
        TValue<QtyCorrectionRequestType[]> types = null,
        TValue<QtyCorrectionRequestStatus> status = null,
        TValue<QtyCorrectionRequestStatus[]> statuses = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                     selector: e => e,
                     id: id,
                     code: code,
                     isDelete: isDelete,
                     userId: userId,
                     transactionBatchId: transactionBatchId,
                     description: description);
      var query = baseQuery.OfType<QtyCorrectionRequest>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (qty != null)
        query = query.Where(i => i.Qty == qty);
      if (unitId != null)
        query = query.Where(i => i.UnitId == unitId);
      if (warehouseId != null)
        query = query.Where(i => i.WarehouseId == warehouseId);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (stuffSerialCode != null)
        query = query.Where(i => i.StuffSerialCode == stuffSerialCode);
      if (statuses != null)
        query = query.Where(i => statuses.Value.Contains(i.Status));
      if (types != null)
        query = query.Where(i => types.Value.Contains(i.Type));
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial);
        query = query.Where(i => i.StuffSerial.Serial == serial);
      }
      if (status != null)
        query = query.Where(i => i.Status == status);
      if (type != null)
        query = query.Where(i => i.Type == type);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (stockCheckingTagId != null)
        query = query.Where(i => i.StockCheckingTagId == stockCheckingTagId);
      return query.Select(selector);
    }
    #endregion
    #region GetQtyCorrectionRequestWarehouseInventories
    internal IQueryable<QtyCorrectionRequestInventoryResult> GetQtyCorrectionRequestInventories(
        string serial = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<int> stuffId = null,
        TValue<int> warehouseId = null,
        TValue<QtyCorrectionRequestStatus[]> qtyCorrectionRequestStatuses = null,
        TValue<QtyCorrectionRequestType[]> qtyCorrectionRequestTypes = null)
    {
      var qtyCorrectionRequests = GetQtyCorrectionRequests(
                selector: e => e,
                qty: qty,
                unitId: unitId,
                stuffId: stuffId,
                serial: serial,
                statuses: qtyCorrectionRequestStatuses,
                types: qtyCorrectionRequestTypes,
                warehouseId: warehouseId,
                isDelete: false);
      //var warehouseInventories = GetWarehouseInventories(
      //          stuffId: null,
      //          serial: serial,
      //          stuffCategoryId: null,
      //          groupBySerial: true)
      //      
      //;
      var confirmations = App.Internals.Confirmation.GetBaseEntityConfirmations(selector: e => e,
              baseEntityConfirmTypeId: lena.Models.StaticBaseEntityConfirmTypes.QtyCorrectionRequestConfirmation.Id);
      var qtyCorrectionRequestInventories = (from qtyCorrectionRequest in qtyCorrectionRequests
                                             join tconfirmation in confirmations on qtyCorrectionRequest.Id equals tconfirmation.ConfirmingEntityId into
                                                   tempconfirmations
                                             from confirmation in tempconfirmations.DefaultIfEmpty()
                                               //join tWarehouseInventory in warehouseInventories on
                                               //    new { qtyCorrectionRequest.StuffSerial.StuffId,                          qtyCorrectionRequest.StuffSerialCode }
                                               //    equals new { tWarehouseInventory.StuffId, tWarehouseInventory.StuffSerialCode } into
                                               //    tempWarehouseInventories
                                               //from warehouseInventory in tempWarehouseInventories.DefaultIfEmpty()
                                             select new QtyCorrectionRequestInventoryResult()
                                             {
                                               Id = qtyCorrectionRequest.Id,
                                               Qty = qtyCorrectionRequest.Qty,
                                               UnitId = qtyCorrectionRequest.UnitId,
                                               UnitName = qtyCorrectionRequest.Unit.Name,
                                               StuffId = qtyCorrectionRequest.StuffId,
                                               StuffCode = qtyCorrectionRequest.Stuff.Code,
                                               StuffName = qtyCorrectionRequest.Stuff.Name,
                                               SerialCode = qtyCorrectionRequest.StuffSerialCode,
                                               Serial = qtyCorrectionRequest.StuffSerial.Serial,
                                               Description = qtyCorrectionRequest.Description,
                                               WarehouseId = qtyCorrectionRequest.WarehouseId,
                                               WarehouseName = qtyCorrectionRequest.Warehouse.Name,
                                               Status = qtyCorrectionRequest.Status,
                                               Type = qtyCorrectionRequest.Type,
                                               //AvailableAmount = warehouseInventory.AvailableAmount,
                                               //BlockedAmount = warehouseInventory.BlockedAmount,
                                               //QualityControlAmount = warehouseInventory.QualityControlAmount,
                                               //TotalAmount = warehouseInventory.TotalAmount,
                                               //WasteAmount = warehouseInventory.WasteAmount,
                                               //WarehouseInventoryUnitId = warehouseInventory.UnitId,
                                               //WarehouseInventoryUnitName = warehouseInventory.UnitName,
                                               ConfirmationEmployeeName =
                                                     confirmation.User.Employee.FirstName + " " + confirmation.User.Employee.LastName,
                                               ConfirmationDateTime = confirmation.ConfirmDateTime,
                                               RegistrarFullName = qtyCorrectionRequest.User.Employee.FirstName + " " + qtyCorrectionRequest.User.Employee.LastName,
                                               DateTime = qtyCorrectionRequest.DateTime,
                                               RowVersion = qtyCorrectionRequest.RowVersion
                                             });
      return qtyCorrectionRequestInventories;
    }
    #endregion
    #region Sort
    internal IOrderedQueryable<QtyCorrectionRequestInventoryResult> SortQtyCorrectionRequestInventoryResult(
        IQueryable<QtyCorrectionRequestInventoryResult> query,
        SortInput<QtyCorrectionRequestInventorySortType> options)
    {
      switch (options.SortType)
      {
        case QtyCorrectionRequestInventorySortType.StuffName:
          return query.OrderBy(a => a.StuffName, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.Serial:
          return query.OrderBy(a => a.Serial, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.Qty:
          return query.OrderBy(a => a.Qty, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.UnitName:
          return query.OrderBy(a => a.UnitName, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.BlockedAmount:
          return query.OrderBy(a => a.BlockedAmount, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.QualityControlAmount:
          return query.OrderBy(a => a.QualityControlAmount, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.TotalAmount:
          return query.OrderBy(a => a.TotalAmount, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.WarehouseInventoryUnitName:
          return query.OrderBy(a => a.WarehouseInventoryUnitName, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.WasteAmount:
          return query.OrderBy(a => a.WasteAmount, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.QtyCorrectionRequestStatus:
          return query.OrderBy(a => a.Status, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.QtyCorrectionRequestType:
          return query.OrderBy(a => a.Type, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.ConfirmationEmployeeName:
          return query.OrderBy(a => a.ConfirmationEmployeeName, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.RegistrarFullName:
          return query.OrderBy(a => a.RegistrarFullName, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.DateTime:
          return query.OrderBy(a => a.DateTime, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.ConfirmationDateTime:
          return query.OrderBy(a => a.ConfirmationDateTime, options.SortOrder);
        case QtyCorrectionRequestInventorySortType.Description:
          return query.OrderBy(a => a.Description, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    internal Expression<Func<QtyCorrectionRequest, QtyCorrectionRequestResult>> ToQtyCorrectionRequestResult =
        qtyCorrectioinRequest => new QtyCorrectionRequestResult()
        {
          Qty = qtyCorrectioinRequest.Qty,
          UnitId = qtyCorrectioinRequest.UnitId,
          UnitName = qtyCorrectioinRequest.Unit.Name,
          StuffId = qtyCorrectioinRequest.StuffId,
          StuffCode = qtyCorrectioinRequest.Stuff.Code,
          StuffName = qtyCorrectioinRequest.Stuff.Name,
          SerialCode = qtyCorrectioinRequest.StuffSerialCode,
          Serial = qtyCorrectioinRequest.StuffSerial.Serial,
          WarehouseId = qtyCorrectioinRequest.WarehouseId,
          WarehouseName = qtyCorrectioinRequest.Warehouse.Name,
          Status = qtyCorrectioinRequest.Status,
          Type = qtyCorrectioinRequest.Type,
          RowVersion = qtyCorrectioinRequest.RowVersion
        };
    #endregion
    #region Search
    internal IQueryable<QtyCorrectionRequestInventoryResult> SearchQtyCorrectionRequestInventoryResultQuery(
        IQueryable<QtyCorrectionRequestInventoryResult> query,
        string searchText, AdvanceSearchItem[] advanceSearchItems,
        string warehouseName = null,
        int? stuffId = null,
        string stuffCode = null,
        string serial = null
    )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.UnitName.Contains(searchText) ||
                    item.StuffName.Contains(searchText) ||
                    item.WarehouseName.Contains(searchText) ||
                    item.StuffCode.Contains(searchText) ||
                    item.ConfirmationEmployeeName.Contains(searchText) ||
                    item.RegistrarFullName.Contains(searchText) ||
                    item.WarehouseName.Contains(searchText) ||
                    item.Description.Contains(searchText) ||
                    item.Serial.Contains(searchText)
                select item;
      var isStuffIdNull = stuffId == null;
      var isStuffCodeNull = stuffCode == null;
      var isSerialdNull = serial == null;
      query = from item in query
              where (isStuffIdNull || item.StuffId == stuffId)
              where (isStuffCodeNull || item.StuffCode == stuffCode)
              where (isSerialdNull || item.Serial == serial)
              select item;
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      if (warehouseName != null)
        query = query.Where(x => x.WarehouseName == warehouseName);
      return query;
    }
    #endregion
    #region AcceptQtyCorrectionRequest
    public QtyCorrectionRequest AcceptQtyCorrectionRequestProcess(
        int id,
        byte[] rowVersion,
        string description)
    {
      var qtyCorrectionRequest = GetQtyCorrectionRequest(id: id);
      return AcceptQtyCorrectionRequestProcess(
                    qtyCorrectionRequest: qtyCorrectionRequest,
                    rowVersion: rowVersion,
                    description: description);
    }
    public QtyCorrectionRequest AcceptQtyCorrectionRequestProcess(
        QtyCorrectionRequest qtyCorrectionRequest,
        byte[] rowVersion,
        string description)
    {
      #region Close Open Package
      var serialBuffer = GetSerialBuffers(
              selector: e => e,
              serial: qtyCorrectionRequest.StuffSerial.Serial,
              warehouseId: qtyCorrectionRequest.WarehouseId)
          .SingleOrDefault();
      if (serialBuffer != null)
      {
        var closeSerialBuffer = App.Internals.WarehouseManagement.CloseSerialBufferProcess(
                      transactionBatch: null,
                      serial: qtyCorrectionRequest.StuffSerial.Serial,
                      warehouseId: qtyCorrectionRequest.WarehouseId);
      }
      #endregion
      #region Set Status
      EditQtyCorrectionRequest(
              qtyCorrectionRequest: qtyCorrectionRequest,
              rowVersion: qtyCorrectionRequest.RowVersion,
              status: QtyCorrectionRequestStatus.Accepted);
      #endregion
      #region AcceptBaseEntityConfirmation
      App.Internals.Confirmation.AcceptBaseEntityConfirmation(
              baseEntityConfirmTypeId: Models.StaticData.StaticBaseEntityConfirmTypes.QtyCorrectionRequestConfirmation.Id,
              confirmingEntityId: qtyCorrectionRequest.Id,
              confirmDescription: description);
      #endregion
      #region AddTransactionBatch
      var transactionBatch = AddTransactionBatch();
      #endregion
      var version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
          stuffId: qtyCorrectionRequest.StuffId,
          stuffSerialCode: qtyCorrectionRequest.StuffSerialCode);
      #region GetTransactionType
      var transactionType = Models.StaticData.StaticTransactionTypes.ImportAvailableStockAdjustment;
      if (qtyCorrectionRequest.Type == QtyCorrectionRequestType.DecreaseStockChecking ||
                qtyCorrectionRequest.Type == QtyCorrectionRequestType.DecreaseAmount ||
                qtyCorrectionRequest.Type == QtyCorrectionRequestType.Missing)
        transactionType = Models.StaticData.StaticTransactionTypes.ExportAvailableStockAdjustment;
      #endregion
      if (qtyCorrectionRequest.StuffSerial != null)
      {
        var warehouseInventory = App.Internals.WarehouseManagement.GetWarehouseInventories(
                      stuffId: qtyCorrectionRequest.StuffId,
                      stuffSerialCode: qtyCorrectionRequest.StuffSerialCode,
                      billOfMaterialVersion: version,
                      warehouseId: qtyCorrectionRequest.WarehouseId)
                  .FirstOrDefault();
        if (warehouseInventory != null && warehouseInventory.QualityControlAmount > 0)
        {
          #region GetTransactionType
          transactionType = Models.StaticData.StaticTransactionTypes.ImportQualityControlStockAdjustment;
          if (qtyCorrectionRequest.Type == QtyCorrectionRequestType.DecreaseStockChecking ||
                    qtyCorrectionRequest.Type == QtyCorrectionRequestType.DecreaseAmount ||
                    qtyCorrectionRequest.Type == QtyCorrectionRequestType.Missing)
            transactionType = Models.StaticData.StaticTransactionTypes.ExportQualityControlStockAdjustment;
          #endregion
        }
        else if (warehouseInventory != null && warehouseInventory.BlockedAmount > 0)
        {
          #region GetTransactionType
          transactionType = Models.StaticData.StaticTransactionTypes.ImportBlockedStockAdjustment;
          if (qtyCorrectionRequest.Type == QtyCorrectionRequestType.DecreaseStockChecking ||
                    qtyCorrectionRequest.Type == QtyCorrectionRequestType.DecreaseAmount ||
                    qtyCorrectionRequest.Type == QtyCorrectionRequestType.Missing)
            transactionType = Models.StaticData.StaticTransactionTypes.ExportBlockedStockAdjustment;
          #endregion
        }
        else if (warehouseInventory != null && warehouseInventory.WasteAmount > 0)
        {
          #region GetTransactionType
          transactionType = Models.StaticData.StaticTransactionTypes.ImportWasteStockAdjustment;
          if (qtyCorrectionRequest.Type == QtyCorrectionRequestType.DecreaseStockChecking ||
                    qtyCorrectionRequest.Type == QtyCorrectionRequestType.DecreaseAmount ||
                    qtyCorrectionRequest.Type == QtyCorrectionRequestType.Missing)
            transactionType = Models.StaticData.StaticTransactionTypes.ExportWasteStockAdjustment;
          #endregion
        }
      }
      #region Add AvailableStockAdjustmentTransaction
      var exportBlockTransaction = AddWarehouseTransaction(
              transactionBatchId: transactionBatch.Id,
              effectDateTime: transactionBatch.DateTime,
              stuffId: qtyCorrectionRequest.StuffId,
              billOfMaterialVersion: version,
              stuffSerialCode: qtyCorrectionRequest.StuffSerialCode,
              warehouseId: qtyCorrectionRequest.WarehouseId,
              transactionTypeId: transactionType.Id,
              amount: qtyCorrectionRequest.Qty,
              unitId: qtyCorrectionRequest.UnitId,
              description: description,
              referenceTransaction: null);
      #endregion
      #region Add Open Package
      if (serialBuffer != null && qtyCorrectionRequest.StuffSerial != null)
      {
        var warehouseInventory = App.Internals.WarehouseManagement.GetWarehouseInventories(
                      stuffId: qtyCorrectionRequest.StuffId,
                      stuffSerialCode: qtyCorrectionRequest.StuffSerialCode,
                      billOfMaterialVersion: version,
                      warehouseId: qtyCorrectionRequest.WarehouseId)
                  .FirstOrDefault();
        if (warehouseInventory != null && warehouseInventory.AvailableAmount > 0)
        {
          var addSerialBuffer = App.Internals.WarehouseManagement.AddSerialBufferProcess(
                        transactionBatch: null,
                        serial: qtyCorrectionRequest.StuffSerial.Serial,
                        serialBufferType: serialBuffer.SerialBufferType,
                        productionTerminalId: serialBuffer.ProductionTerminalId,
                        warehouseId: qtyCorrectionRequest.WarehouseId);
        }
      }
      #endregion
      #region qualityControlCorrectQTY
      var qualityControlItem = App.Internals.QualityControl.GetQualityControlItems(
          selector: e => e,
          stuffId: qtyCorrectionRequest.StuffId,
          stuffSerialCode: qtyCorrectionRequest.StuffSerialCode,
          qualityControlStatus: QualityControlStatus.NotAction)
      .FirstOrDefault();
      if (qualityControlItem != null)
      {
        var warehouseInventory = App.Internals.WarehouseManagement.GetWarehouseInventories(
                      stuffId: qtyCorrectionRequest.StuffId,
                      stuffSerialCode: qtyCorrectionRequest.StuffSerialCode,
                      billOfMaterialVersion: version,
                      warehouseId: qtyCorrectionRequest.WarehouseId)
                  .FirstOrDefault();
        App.Internals.QualityControl.EditQualityControlItem(id: qualityControlItem.Id, rowVersion: qualityControlItem.RowVersion, qty: warehouseInventory.TotalAmount);
        var sumQty = from item in qualityControlItem.QualityControl.QualityControlItems
                     select new SumQtyItemInput()
                     {
                       Qty = item.Qty,
                       UnitId = item.UnitId
                     };
        var SumQtyResult = App.Internals.ApplicationBase.SumQty(
                  targetUnitId: null,
                  sumQtys: sumQty.ToArray());
        var qualityControl = App.Internals.QualityControl.GetQualityControls(
                  selector: e => e,
                  serial: qtyCorrectionRequest.StuffSerial.Serial)
              .FirstOrDefault();
        if (qualityControl != null)
        {
          App.Internals.QualityControl.EditQualityControl(
                    id: qualityControl.Id,
                    rowVersion: qualityControl.RowVersion,
                    qty: SumQtyResult.Qty,
                    unitId: SumQtyResult.UnitId);
        }
      }
      #endregion
      return qtyCorrectionRequest;
    }
    #endregion
    #region RejectQtyCorrectionRequest
    public QtyCorrectionRequest RejectQtyCorrectionRequestProcess(
        int id,
        byte[] rowVersion,
        string description)
    {
      var qtyCorrectionRequest = GetQtyCorrectionRequest(id: id);
      return RejectQtyCorrectionRequestProcess(
                    qtyCorrectionRequest: qtyCorrectionRequest,
                    rowVersion: rowVersion,
                    description: description);
    }
    public QtyCorrectionRequest RejectQtyCorrectionRequestProcess(
        QtyCorrectionRequest qtyCorrectionRequest,
        byte[] rowVersion,
        string description)
    {
      #region SetStatus
      var result = EditQtyCorrectionRequest(
              qtyCorrectionRequest: qtyCorrectionRequest,
              rowVersion: qtyCorrectionRequest.RowVersion,
              status: QtyCorrectionRequestStatus.Rejected);
      #endregion
      #region AcceptBaseEntityConfirmation
      App.Internals.Confirmation.RejectBaseEntityConfirmation(
              baseEntityConfirmTypeId: Models.StaticData.StaticBaseEntityConfirmTypes.QtyCorrectionRequestConfirmation.Id,
              confirmingEntityId: qtyCorrectionRequest.Id,
              confirmDescription: description);
      #endregion
      return result;
    }
    #endregion
    #region AcceptQtyCorrectionRequests
    public void AcceptQtyCorrectionRequestsProcess(AcceptQtyCorrectionRequestInput[] acceptQtyCorrectionRequests)
    {
      foreach (var item in acceptQtyCorrectionRequests)
      {
        AcceptQtyCorrectionRequestProcess(
                      id: item.Id,
                      rowVersion: item.RowVersion,
                      description: item.Description);
      }
    }
    #endregion
    #region RejectQtyCorrectionRequests
    public void RejectQtyCorrectionRequestsProcess(RejectQtyCorrectionRequestInput[] rejectQtyCorrectionRequests)
    {
      foreach (var item in rejectQtyCorrectionRequests)
      {
        RejectQtyCorrectionRequestProcess(
                      id: item.Id,
                      rowVersion: item.RowVersion,
                      description: item.Description);
      }
    }
    #endregion
  }
}
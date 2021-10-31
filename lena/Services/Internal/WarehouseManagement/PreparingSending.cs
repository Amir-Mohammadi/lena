using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.ApplicationBase.Unit;
using lena.Models.Common;
using lena.Models.WarehouseManagement.PreparingSending;
using lena.Models.WarehouseManagement.BaseTransaction;
using lena.Models.WarehouseManagement.PreparingSendingItem;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public PreparingSending AddPreparingSending(
        PreparingSending preparingSending,
        TransactionBatch transactionBatch,
        int sendPermissionId,
        double qty,
        byte unitId,
        string description,
        PreparingSendingStatus status)
    {

      preparingSending = preparingSending ?? repository.Create<PreparingSending>();
      preparingSending.SendPermissionId = sendPermissionId;
      preparingSending.Status = status;
      preparingSending.Qty = qty;
      preparingSending.UnitId = unitId;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: preparingSending,
                    transactionBatch: transactionBatch,
                    description: description);
      return preparingSending;
    }
    #endregion
    #region Edit
    public PreparingSending EditPreparingSending(
        int id,
        byte[] rowVersion,
        TValue<int> sendPermissionId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<PreparingSendingStatus> status = null,
        TValue<string> description = null)
    {

      var preparingSending = GetPreparingSending(id: id);
      return EditPreparingSending(
                    preparingSending: preparingSending,
                    rowVersion: rowVersion,
                    sendPermissionId: sendPermissionId,
                    qty: qty,
                    unitId: unitId,
                    status: status,
                    description: description);
    }
    public PreparingSending EditPreparingSending(
        PreparingSending preparingSending,
        byte[] rowVersion,
        TValue<int> sendPermissionId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<PreparingSendingStatus> status = null,
        TValue<string> description = null)
    {

      if (sendPermissionId != null)
        preparingSending.SendPermissionId = sendPermissionId;
      if (status != null)
        preparingSending.Status = status;
      if (qty != null)
        preparingSending.Qty = qty;
      if (unitId != null)
        preparingSending.UnitId = unitId;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: preparingSending,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as PreparingSending;
    }
    #endregion
    #region Get
    public PreparingSending GetPreparingSending(int id) => GetPreparingSending(selector: e => e, id: id);
    public TResult GetPreparingSending<TResult>(
        Expression<Func<PreparingSending, TResult>> selector,
        int id)
    {

      var result = GetPreparingSendings(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new PreparingSendingNotFoundException(id);
      return result;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetPreparingSendings<TResult>(
        Expression<Func<PreparingSending, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> sendPermissionId = null,
        TValue<int> exitReceiptRequestId = null,
        TValue<int> stuffId = null,
        TValue<PreparingSendingStatus> status = null,
        TValue<PreparingSendingStatus[]> statuses = null,
        TValue<int> orderItemId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    ids: ids,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var preparingSending = baseQuery.OfType<PreparingSending>();
      if (sendPermissionId != null)
        preparingSending = preparingSending.Where(r => r.SendPermissionId == sendPermissionId);
      if (exitReceiptRequestId != null)
        preparingSending = preparingSending.Where(r => r.SendPermission.ExitReceiptRequestId == exitReceiptRequestId);
      if (orderItemId != null)
      {
        //todo fix 
        preparingSending = preparingSending.Where(r => (r.SendPermission.ExitReceiptRequest as OrderItemBlock).OrderItemId == orderItemId);
      }
      if (status != null)
        preparingSending = preparingSending.Where(i => i.Status == status);
      if (stuffId != null)
        preparingSending = preparingSending.Where(i => i.SendPermission.ExitReceiptRequest.StuffId == stuffId);
      if (statuses != null)
        preparingSending = preparingSending.Where(i => statuses.Value.Contains(i.Status));
      return preparingSending.Select(selector);
    }
    #endregion
    #region AddProcess
    public PreparingSending AddPreparingSendingProcess(
        SendPermission sendPermission,
        byte[] rowVersion,
        AddPreparingSendingItemInput[] addPreparingSendingItemsInput,
        string description)
    {

      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      var transactionBatch = warehouseManagement.AddTransactionBatch();
      #endregion
      #region SumQty
      var sumQtyInputs = from item in addPreparingSendingItemsInput
                         select new SumQtyItemInput()
                         {
                           Qty = item.Amount,
                           UnitId = item.UnitId
                         };
      var sumQtyResult = App.Internals.ApplicationBase.SumQty(
                targetUnitId: sendPermission.UnitId,
                sumQtys: sumQtyInputs.ToArray());
      #endregion
      #region AddPreparingSending
      var preparingSending = AddPreparingSending(
              preparingSending: null,
              transactionBatch: transactionBatch,
              qty: sumQtyResult.Qty,
              unitId: sumQtyResult.UnitId,
              sendPermissionId: sendPermission.Id,
              status: PreparingSendingStatus.Waiting,
              description: description);
      #endregion
      #region Get BlockedTransactions
      var blockedTransactions = GetWarehouseTransactions(
              selector: ToWarehouseTransactionMinResult,
              transactionBatchId: sendPermission.ExitReceiptRequest.TransactionBatch.Id,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportBlock.Id);
      var blockedTransactionsList = blockedTransactions.ToList();
      #endregion
      #region AddPreparingSendingItems
      var mainUnit = sendPermission.Unit;
      foreach (var preparingSendingItemInput in addPreparingSendingItemsInput)
      {
        #region GetUnit
        var unit = App.Internals.ApplicationBase.GetUnit(id: preparingSendingItemInput.UnitId);
        #endregion
        #region GetStuffSerial
        var stuffSerial = GetStuffSerial(serial: preparingSendingItemInput.Serial);
        #endregion
        #region AddPreparingSendingItem
        var preparingSendingItem = AddPreparingSendingItem(
                preparingSendingItem: null,
                transactionBatch: null,
                preparingSendingId: preparingSending.Id,
                qty: preparingSendingItemInput.Amount,
                unitId: preparingSendingItemInput.UnitId,
                stuffId: preparingSendingItemInput.StuffId,
                stuffSerialCode: stuffSerial.Code,
                description: preparingSendingItemInput.Description);
        #endregion
        #region AddTransactions
        var preparingSendingItemAmount = preparingSendingItemInput.Amount * unit.ConversionRatio;
        var warehouseIds = new List<int>();
        while (preparingSendingItemAmount > 0)
        {
          #region GetBlockedTransaction
          var blockedTransaction = blockedTransactionsList.FirstOrDefault(i => i.Serial == preparingSendingItemInput.Serial && i.Amount > 0);
          if (blockedTransaction == null)
            blockedTransaction = blockedTransactionsList.FirstOrDefault(i => i.Serial == null && i.Amount > 0);
          if (blockedTransaction == null)
            throw new MatchBlockedTransactionNotFoundException(preparingSendingItemInput.Serial);

          if (blockedTransaction.WarehouseId == null)
            throw new BaseTransactionHasNoWarehouseException(id: blockedTransaction.Id);

          if (!warehouseIds.Contains(blockedTransaction.WarehouseId.Value))
            warehouseIds.Add(blockedTransaction.WarehouseId.Value);
          var referenceTransaction = GetBaseTransaction(id: blockedTransaction.Id);
          #endregion
          var transactionAmount = Math.Min(preparingSendingItemAmount, blockedTransaction.Amount);
          var serialBillofMaterialVersion = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
                                            stuffId: blockedTransaction.StuffId,
                                            stuffSerialCode: stuffSerial.Code
                                            );
          #region Export Blocked Transaction
          var exportFromBlockedTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
              transactionBatchId: transactionBatch.Id,
              effectDateTime: transactionBatch.DateTime,
              stuffId: blockedTransaction.StuffId,
              billOfMaterialVersion: blockedTransaction.BillOfMaterialVersion,
              stuffSerialCode: blockedTransaction.StuffSerialCode,
              warehouseId: blockedTransaction.WarehouseId.Value,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportBlock.Id,
              amount: transactionAmount / unit.ConversionRatio,
              unitId: unit.Id,
              description: null,
              referenceTransaction: referenceTransaction);
          #endregion
          #region Import Available Transaction
          var importAvailableTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
                  transactionBatchId: transactionBatch.Id,
                  effectDateTime: transactionBatch.DateTime,
                  stuffId: blockedTransaction.StuffId,
                  billOfMaterialVersion: blockedTransaction.BillOfMaterialVersion,
                  stuffSerialCode: blockedTransaction.StuffSerialCode,
                  warehouseId: blockedTransaction.WarehouseId.Value,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAvailable.Id,
                  amount: transactionAmount / unit.ConversionRatio,
                  unitId: unit.Id,
                  description: null,
                  referenceTransaction: exportFromBlockedTransaction);
          #endregion
          #region Export Available Transaction for Serial
          var exportAvailableTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
                  transactionBatchId: transactionBatch.Id,
                  effectDateTime: transactionBatch.DateTime,
                  stuffId: blockedTransaction.StuffId,
                  billOfMaterialVersion: serialBillofMaterialVersion,
                  stuffSerialCode: stuffSerial.Code,
                  warehouseId: blockedTransaction.WarehouseId.Value,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportAvailable.Id,
                  amount: transactionAmount / unit.ConversionRatio,
                  unitId: unit.Id,
                  description: null,
                  referenceTransaction: importAvailableTransaction);
          #endregion
          #region Import Block Transaction for Serial
          var importBlockTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
                  transactionBatchId: transactionBatch.Id,
                  effectDateTime: transactionBatch.DateTime,
                  stuffId: preparingSendingItemInput.StuffId,
                  billOfMaterialVersion: serialBillofMaterialVersion,
                  stuffSerialCode: stuffSerial.Code,
                  warehouseId: preparingSendingItemInput.WarehouseId,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportBlock.Id,
                  amount: transactionAmount / unit.ConversionRatio,
                  unitId: unit.Id,
                  description: preparingSendingItemInput.Description,
                  referenceTransaction: exportAvailableTransaction);
          #endregion
          preparingSendingItemAmount = preparingSendingItemAmount - transactionAmount;
          blockedTransaction.Amount = blockedTransaction.Amount - transactionAmount;
        }
        #endregion
      }
      #endregion
      #region GetProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: sendPermission.Id,
              scrumTaskType: ScrumTaskTypes.PreparingSendig);
      #endregion
      #region AddSendProductTask
      //check projectWork not null
      if (projectWorkItem != null)
      {
        App.Internals.ProjectManagement.AddProjectWorkItem(
                      projectWorkItem: null,
                      name: $"ارسال محصول {preparingSending.Code}",
                      description: "کالاهای آماده شده را ارسال نمایید",
                      color: "",
                      departmentId: (int)Departments.Warehouse,
                      estimatedTime: 10800,
                      isCommit: false,
                      scrumTaskTypeId: (int)ScrumTaskTypes.SendProduct,
                      userId: null,
                      spentTime: 0,
                      remainedTime: 0,
                      scrumTaskStep: ScrumTaskStep.ToDo,
                      projectWorkId: projectWorkItem.ScrumBackLogId,
                      baseEntityId: preparingSending.Id);
      }

      #endregion
      #region DoneTask if Preaparing All 
      #region GetAllPreparingSending 
      var query = App.Internals.WarehouseManagement.GetPreparingSendings(
              selector: e => e.Qty * e.Unit.ConversionRatio / mainUnit.ConversionRatio,
              sendPermissionId: preparingSending.SendPermissionId,
              isDelete: false);
      var sum = query.Sum(i => i);
      #endregion
      if (sum > sendPermission.Qty)
        throw new SumPreparingSendingQtyGreatherThanSendPermissionQtyException();
      if (sum == sendPermission.Qty)
      {
        //check projectWork not null
        if (projectWorkItem != null)
        {
          App.Internals.ScrumManagement.DoneScrumTask(
                        scrumTask: projectWorkItem,
                        rowVersion: projectWorkItem.RowVersion);
        }
      }
      #endregion
      #region ResetSendPermissionStatus
      App.Internals.SaleManagement.ResetSendPermissionStatus(id: sendPermission.Id);
      #endregion
      return preparingSending;
    }
    public PreparingSending AddPreparingSendingProcess(
        int sendPermissionId,
        byte[] rowVersion,
        AddPreparingSendingItemInput[] addPreparingSendingItemsInput,
        string description)
    {

      var sendPermission = App.Internals.SaleManagement.GetSendPermission(id: sendPermissionId);
      var preparingSending = AddPreparingSendingProcess(
                sendPermission: sendPermission,
                rowVersion: rowVersion,
                addPreparingSendingItemsInput: addPreparingSendingItemsInput,
                description: description);
      return preparingSending;
    }
    #endregion
    #region Search
    public IQueryable<PreparingSendingResult> SearchPreparingSendingResult(
        IQueryable<PreparingSendingResult> query,
        string search,
        int? customerId,
        int? orderItemId,
        int? stuffId,
        int? orderItemBlockId)
    {
      if (!string.IsNullOrEmpty(search))
        query = query.Where(item =>
            item.StuffCode.Contains(search) ||
            item.StuffName.Contains(search));
      if (customerId != null)
        query = query.Where(i => i.CustomerId == customerId);
      if (orderItemId != null)
        query = query.Where(i => i.OrderItemId == orderItemId);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (orderItemBlockId != null)
        query = query.Where(i => i.OrderItemBlockId == orderItemBlockId);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<PreparingSendingResult> SortPreparingSendingResult(IQueryable<PreparingSendingResult> query,
        SortInput<PreparingSendingSortType> sort)
    {
      switch (sort.SortType)
      {
        case PreparingSendingSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case PreparingSendingSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case PreparingSendingSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case PreparingSendingSortType.OrderItemCode:
          return query.OrderBy(a => a.OrderItemCode, sort.SortOrder);
        case PreparingSendingSortType.SendPermissionCode:
          return query.OrderBy(a => a.SendPermissionCode, sort.SortOrder);
        case PreparingSendingSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case PreparingSendingSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region GetStockForPreparingSending
    public WarehouseInventoryResult GetStockForPreparingSending(
        int sendPermissionId,
        string serial,
        string[] selectedSerial)
    {

      var sendPermission = App.Internals.SaleManagement.GetSendPermission(
                    id: sendPermissionId);
      var exitReceiptRequest = sendPermission.ExitReceiptRequest;

      var blockedTransactions = GetWarehouseTransactions(
                    selector: ToWarehouseTransactionMinResult,
                    transactionBatchId: sendPermission.ExitReceiptRequest.TransactionBatch.Id,
                    transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportBlock.Id);
      var blockedTransactionsList = blockedTransactions.ToList();
      var blockedTransaction = blockedTransactionsList.FirstOrDefault(i => i.Serial == serial && i.Amount > 0);
      if (blockedTransaction == null)
        blockedTransaction = blockedTransactionsList.FirstOrDefault(i => i.Serial == null && i.Amount > 0);
      if (blockedTransaction == null)
        throw new MatchBlockedTransactionNotFoundException(serial);

      var warehouseInventory = GetWarehouseInventoryForIssue(
                  warehouseId: exitReceiptRequest.WarehouseId,
                  serial: serial,
                  selectedSerial: selectedSerial);
      if (warehouseInventory.Count() == 0)
        throw new SerialNotExistInWarehouseException(serial: serial, warehouseName: exitReceiptRequest.Warehouse.Name);

      return warehouseInventory.FirstOrDefault();
    }
    #endregion
    #region ToPreparingSendingResult
    public Expression<Func<PreparingSending, PreparingSendingResult>> ToPreparingSendingResult =
        preparingSending => new PreparingSendingResult
        {
          Id = preparingSending.Id,
          Code = preparingSending.Code,
          SendPermissionId = preparingSending.SendPermissionId,
          SendPermissionCode = preparingSending.SendPermission.Code,
          CustomerId = preparingSending.SendPermission.ExitReceiptRequest.CooperatorId,
          CustomerCode = preparingSending.SendPermission.ExitReceiptRequest.Cooperator.Code,
          CustomerName = preparingSending.SendPermission.ExitReceiptRequest.Cooperator.Name,
          OrderItemId = (preparingSending.SendPermission.ExitReceiptRequest as OrderItemBlock).OrderItemId,
          OrderItemCode = (preparingSending.SendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.Code,
          OrderItemUnitId = (preparingSending.SendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.UnitId,
          OrderItemUnitName = (preparingSending.SendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.Unit.Name,
          OrderItemQty = preparingSending.SendPermission.ExitReceiptRequest.Qty,
          OrderItemBlockId = preparingSending.SendPermission.ExitReceiptRequest.Id,
          OrderItemBlockCode = preparingSending.SendPermission.ExitReceiptRequest.Code,
          OrderItemBlockQty = preparingSending.SendPermission.ExitReceiptRequest.Qty,
          OrderItemBlockUnitId = preparingSending.SendPermission.ExitReceiptRequest.UnitId,
          OrderItemBlockUnitName = preparingSending.SendPermission.ExitReceiptRequest.Unit.Name,
          StuffId = preparingSending.SendPermission.ExitReceiptRequest.StuffId,
          StuffCode = preparingSending.SendPermission.ExitReceiptRequest.Stuff.Code,
          StuffName = preparingSending.SendPermission.ExitReceiptRequest.Stuff.Name,
          SendPermissionQty = preparingSending.SendPermission.Qty,
          SendPermissionUnitId = preparingSending.SendPermission.UnitId,
          SendPermissionUnitName = preparingSending.SendPermission.Unit.Name,
          Qty = preparingSending.Qty,
          UnitId = preparingSending.UnitId,
          UnitName = preparingSending.Unit.Name,
          UnitConversionRatio = preparingSending.Unit.ConversionRatio,
          DateTime = preparingSending.DateTime,
          SendProductId = preparingSending.SendProduct.Id,
          SendProductCode = preparingSending.SendProduct.Code,
          ExitReceiptId = preparingSending.SendProduct.ExitReceiptId,
          ExitReceiptCode = preparingSending.SendProduct.ExitReceipt.Code,
          ExitReceiptDateTime = preparingSending.SendProduct.ExitReceipt.DateTime,
          ExitReceiptConfirm = preparingSending.SendProduct.ExitReceipt.Confirmed,
          Status = preparingSending.Status,
          RowVersion = preparingSending.RowVersion
        };
    #endregion
    #region Send
    public PreparingSending SendPreparingSending(
        int id,
        byte[] rowVersion)
    {

      var preparingSending = GetPreparingSending(id: id);
      return SendPreparingSending(
                    preparingSending: preparingSending,
                    rowVersion: rowVersion);
    }
    public PreparingSending SendPreparingSending(
        PreparingSending preparingSending,
        byte[] rowVersion)
    {

      #region Set PreparingSendigStatus To Sent
      EditPreparingSending(
          preparingSending: preparingSending,
          rowVersion: rowVersion,
          status: PreparingSendingStatus.Sent);
      #endregion
      #region MyRegion
      App.Internals.SaleManagement.ResetSendPermissionStatus(id: preparingSending.SendPermissionId);
      #endregion
      return preparingSending;
    }
    #endregion
  }
}

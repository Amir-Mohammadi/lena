using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.WarehouseTransaction;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public RequestWarehouseIssue AddRequestWarehouseIssue(
        RequestWarehouseIssue requestWarehouseIssue,
        TransactionBatch transactionBatch,
        short fromWarehouseId,
        short? toWarehouseId,
        WarehouseIssueStatusType status,
        string description,
        int? toEmployeeId,
        short? toDepartmentId)
    {
      requestWarehouseIssue = requestWarehouseIssue ?? repository.Create<RequestWarehouseIssue>();
      #region CheckWarehouseIssueConfirmDeadline
      var hasDeadline = CheckWarehouseIssueConfirmDeadline(
              fromWarehouseId: fromWarehouseId,
              toWarehouseId: toWarehouseId,
              toDepartmentId: toDepartmentId);
      if (hasDeadline == true)
        throw new WarehouseIssueHasDeadlineException();
      #endregion
      AddWarehouseIssue(
              warehouseIssue: requestWarehouseIssue,
              transactionBatch: transactionBatch,
              fromWarehouseId: fromWarehouseId,
              toWarehouseId: toWarehouseId,
              status: status,
              description: description,
              toEmployeeId: toEmployeeId,
              toDepartmentId: toDepartmentId);
      return requestWarehouseIssue;
    }
    #endregion
    #region Edit
    public RequestWarehouseIssue EditRequestWarehouseIssue(
        int id,
        byte[] rowVersion,
        TValue<short> fromWarehouseId = null,
        TValue<short?> toWarehouseId = null,
        TValue<WarehouseIssueStatusType> status = null,
        TValue<int> responseWarehouseIssueId = null,
        TValue<string> description = null)
    {
      var requestWarehouseIssue = GetRequestWarehouseIssue(id: id);
      return EditRequestWarehouseIssue(
                    requestWarehouseIssue: requestWarehouseIssue,
                    rowVersion: rowVersion,
                    fromWarehouseId: fromWarehouseId,
                    toWarehouseId: toWarehouseId,
                    status: status,
                    responseWarehouseIssueId: responseWarehouseIssueId,
                    description: description);
    }
    public RequestWarehouseIssue EditRequestWarehouseIssue(
        RequestWarehouseIssue requestWarehouseIssue,
        byte[] rowVersion,
        TValue<short> fromWarehouseId = null,
        TValue<short?> toWarehouseId = null,
        TValue<WarehouseIssueStatusType> status = null,
        TValue<int> responseWarehouseIssueId = null,
        TValue<string> description = null)
    {
      var retValue = EditWarehouseIssue(
                    warehouseIssue: requestWarehouseIssue,
                    rowVersion: rowVersion,
                    fromWarehouseId: fromWarehouseId,
                    toWarehouseId: toWarehouseId,
                    status: status,
                    responseWarehouseIssueId: responseWarehouseIssueId,
                    description: description);
      return retValue as RequestWarehouseIssue;
    }
    #endregion
    #region Get
    public RequestWarehouseIssue GetRequestWarehouseIssue(int id) => GetRequestWarehouseIssue(selector: e => e, id: id);
    public TResult GetRequestWarehouseIssue<TResult>(
        Expression<Func<RequestWarehouseIssue, TResult>> selector,
        int id)
    {
      var result = GetRequestWarehouseIssues(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new RequestWarehouseIssueNotFoundException(id);
      return result;
    }
    public RequestWarehouseIssue GetRequestWarehouseIssue(string code) => GetRequestWarehouseIssue(selector: e => e, code: code);
    public TResult GetRequestWarehouseIssue<TResult>(
        Expression<Func<RequestWarehouseIssue, TResult>> selector,
        string code)
    {
      var result = GetRequestWarehouseIssues(
                selector: selector,
                code: code).FirstOrDefault();
      if (result == null)
        throw new RequestWarehouseIssueNotFoundException(code);
      return result;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetRequestWarehouseIssues<TResult>(
        Expression<Func<RequestWarehouseIssue, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> fromWarehouseId = null,
        TValue<int?> toWarehouseId = null,
        TValue<int> responseStuffRequestItemId = null,
        TValue<WarehouseIssueStatusType> status = null)
    {
      var baseQuery = GetWarehouseIssues(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description,
                    fromWarehouseId: fromWarehouseId,
                    toWarehouseId: toWarehouseId,
                    status: status);
      var query = baseQuery.OfType<RequestWarehouseIssue>();
      if (responseStuffRequestItemId != null)
        query = query.Where(i => i.ResponseStuffRequestItems.Select(r => r.Id).Contains(responseStuffRequestItemId));
      return query.Select(selector);
    }
    #endregion
    #region AddProcess
    public List<WarehouseIssue> AddRequestWarehouseIssueProcess(
        short fromWarehouseId,
        short? toWarehouseId,
        AddWarehouseIssueItemInput[] addWarehouseIssueItems,
        int[] responseStuffRequestItemIds,
        string description,
        int? toEmployeeId,
        short? toDepartmentId,
        bool allowChangeOrder)
    {
      var warehouseManagement = App.Internals.WarehouseManagement;
      #region Check IsDeletedWarehouse
      var fromWarehouse = App.Internals.WarehouseManagement.GetWarehouse(e => e, id: fromWarehouseId);
      if (fromWarehouse.IsDeleted == true)
        throw new WarehouseHaseDeletedException(fromWarehouseId);
      if (toWarehouseId != null)
      {
        var toWarehouse = App.Internals.WarehouseManagement.GetWarehouse(e => e, id: toWarehouseId.Value);
        var r = toWarehouse.Id;
        if (toWarehouse.IsDeleted == true)
          throw new WarehouseHaseDeletedException(toWarehouseId);
      }
      #endregion
      #region Check IsDirectWarehouseIssue
      var directWarehouseIssuePermission = CheckIsDirectWarehouseIssue(
              fromWarehouseId: fromWarehouseId,
              toWarehouseId: toWarehouseId ?? fromWarehouseId);
      #endregion
      var result = new List<WarehouseIssue>();
      var addWarehouseIssueItemList = new List<AddWarehouseIssueItemInput>();
      var responseStuffRequestItems = GetResponseStuffRequestItems(
                selector: e => e,
                ids: responseStuffRequestItemIds);
      var responseStuffRequestItemStuffIds = responseStuffRequestItems.Select(i => i.StuffRequestItem.StuffId).Distinct();
      var responseStuffRequestItemUnitIds = responseStuffRequestItems.Select(i => i.StuffRequestItem.UnitId).Distinct();
      if (responseStuffRequestItemStuffIds.Count() > 1)
        throw new ResponseStuffRequestItemStuffsCountIsMoreThanOneException();
      if (responseStuffRequestItemUnitIds.Count() > 1)
        throw new ResponseStuffRequestItemUnitsCountIsMoreThanOneException();
      double sumQty = 0;
      foreach (var responseStuffRequestItem in responseStuffRequestItems)
      {
        var requestUnit = responseStuffRequestItem.StuffRequestItem.Unit;
        var remained = responseStuffRequestItem.Qty * requestUnit.ConversionRatio;
        sumQty = sumQty + responseStuffRequestItem.Qty;
        while (true)
        {
          #region GetFirstTransaction
          var firstTransaction = addWarehouseIssueItems.FirstOrDefault(i => i.Amount > 0);
          if (firstTransaction == null)
          {
            if (allowChangeOrder == false)
              throw new NotEnoughQtyForWarehouseIssueException();
            else
              break;
          }
          #endregion
          #region Get TransactionUnit
          var unit = App.Internals.ApplicationBase.GetUnit(firstTransaction.UnitId);
          #endregion
          #region Calculate TransactoinValue and Remained
          var transactionValue = firstTransaction.Amount * unit.ConversionRatio;
          transactionValue = Math.Min(transactionValue, remained);
          remained -= transactionValue;
          firstTransaction.Amount -= (transactionValue / unit.ConversionRatio);
          #endregion
          #region Append AddTransactionInput
          var addWarehouseIssueItem = new AddWarehouseIssueItemInput
          {
            Amount = transactionValue / requestUnit.ConversionRatio,
            UnitId = requestUnit.Id,
            StuffId = firstTransaction.StuffId,
            Serial = firstTransaction.Serial,
            AssetCode = firstTransaction.AssetCode
          };
          addWarehouseIssueItemList.Add(addWarehouseIssueItem);
          #endregion
          if (remained <= 0)
            break;
        }
      }
      #region Add RequestWarehouseIssue Process
      var isProperty = responseStuffRequestItems.All(
          i => i.StuffRequestItem.StuffRequest.StuffRequestType == StuffRequestType.Property &&
          i.StuffRequestItem.UnitId == 1);
      if (isProperty && addWarehouseIssueItems.Count() != sumQty)
        throw new EnterAssetCodeForEachAssetException();
      var stuffRequestTypeIsProperty = responseStuffRequestItems.All(i => i.StuffRequestItem.StuffRequest.StuffRequestType == StuffRequestType.Property);
      if (stuffRequestTypeIsProperty)
      {
        foreach (var addWarehouseIssueItem in addWarehouseIssueItems)
        {
          var existAsset = GetAssets(selector: e => e,
                    code: addWarehouseIssueItem.AssetCode);
          if (existAsset.Any())
          {
            var asset = existAsset.FirstOrDefault();
            if (addWarehouseIssueItem.Serial != asset.StuffSerial.Serial)
              throw new AssetCodeIsRepetitiveException(addWarehouseIssueItem.AssetCode);
          }
          var deliveredAsset = GetAssets(
                    selector: e => e,
                    serial: addWarehouseIssueItem.Serial);
          if (deliveredAsset.Any())
          {
            var asset = deliveredAsset.FirstOrDefault();
            if (asset.Code != addWarehouseIssueItem.AssetCode)
              throw new InvalidAssetCodeException(assetCode: asset.Code, serial: addWarehouseIssueItem.Serial);
          }
        }
        directWarehouseIssuePermission = AccessType.Denied;
      }
      var transactionBatch = warehouseManagement.AddTransactionBatch();
      WarehouseIssue warehouseIssue = null;
      if (directWarehouseIssuePermission == AccessType.Allowed)
      {
        warehouseIssue = AddDirectRequestWarehouseIssueProcess(
                  transactionBatch: transactionBatch,
                  fromWarehouseId: fromWarehouseId,
                  toWarehouseId: toWarehouseId,
                  responseStuffRequestItems: responseStuffRequestItems,
                  requestStuffId: responseStuffRequestItemStuffIds.FirstOrDefault() ?? 0,
                  requestUnitId: responseStuffRequestItemUnitIds.FirstOrDefault(),
                  addWarehouseIssueItems: addWarehouseIssueItemList.ToArray(),
                  description: description,
                  toDepartmentId: toDepartmentId,
                  toEmployeeId: toEmployeeId);
      }
      else
      {
        warehouseIssue = AddIndirectRequestWarehouseIssueProcess(
                  transactionBatch: transactionBatch,
                  fromWarehouseId: fromWarehouseId,
                  toWarehouseId: toWarehouseId,
                  responseStuffRequestItems: responseStuffRequestItems,
                  requestStuffId: responseStuffRequestItemStuffIds.FirstOrDefault() ?? 0,
                  requestUnitId: responseStuffRequestItemUnitIds.FirstOrDefault(),
                  addWarehouseIssueItems: addWarehouseIssueItemList.ToArray(),
                  description: description,
                  toDepartmentId: toDepartmentId,
                  toEmployeeId: toEmployeeId);
      }
      result.Add(warehouseIssue);
      #endregion
      return result;
    }
    #endregion
    #region AddIndirectProcess
    public WarehouseIssue AddIndirectRequestWarehouseIssueProcess(
        TransactionBatch transactionBatch,
        short fromWarehouseId,
        short? toWarehouseId,
        int requestStuffId,
        byte requestUnitId,
        AddWarehouseIssueItemInput[] addWarehouseIssueItems,
        string description,
        int? toEmployeeId,
        short? toDepartmentId,
        IEnumerable<ResponseStuffRequestItem> responseStuffRequestItems)
    {
      #region AddRequsetWarehouseIssue
      var requestWarehouseIssue = AddRequestWarehouseIssue(
                  requestWarehouseIssue: null,
                  transactionBatch: transactionBatch,
                  fromWarehouseId: fromWarehouseId,
                  toWarehouseId: toWarehouseId,
                  status: WarehouseIssueStatusType.Waiting,
                  description: description,
                  toEmployeeId: toEmployeeId,
                  toDepartmentId: toDepartmentId);
      #endregion
      #region TransactionWithoutSerialFromExportBlockToAvailable
      var batchId = transactionBatch.Id;
      var batchTime = transactionBatch.DateTime;
      if (requestWarehouseIssue != null)
      {
        foreach (var responseStuffRequestItem in responseStuffRequestItems)
        {
          responseStuffRequestItem.RequestWarehouseIssueId = requestWarehouseIssue.Id;
          #region Get ImportBlockTransaction
          var importBlockTransaction = GetWarehouseTransactions(
                  selector: e => e,
                  transactionBatchId: responseStuffRequestItem.TransactionBatch.Id,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportBlock.Id)
              .SingleOrDefault();
          #endregion
          #region Add ExportBlockTransaction
          var exportBlockTransaction = AddWarehouseTransaction(
                  transactionBatchId: batchId,
                  effectDateTime: batchTime,
                  stuffId: requestStuffId,
                  billOfMaterialVersion: null,
                  stuffSerialCode: null,
                  warehouseId: fromWarehouseId,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportBlock.Id,
                  amount: importBlockTransaction.Amount,
                  unitId: requestUnitId,
                  description: description,
                  referenceTransaction: importBlockTransaction);
          #endregion
          #region Add ImportAvailableTransaction
          AddWarehouseTransaction(
                  transactionBatchId: batchId,
                  effectDateTime: batchTime,
                  stuffId: requestStuffId,
                  billOfMaterialVersion: null,
                  stuffSerialCode: null,
                  warehouseId: fromWarehouseId,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAvailable.Id,
                  amount: importBlockTransaction.Amount,
                  unitId: requestUnitId,
                  description: description,
                  referenceTransaction: exportBlockTransaction);
          #endregion
          #region ComplateResponseStuffRequestItemStatus
          ComplateResponseStuffRequestItemStatus(
                  responseStuffRequestItem: responseStuffRequestItem,
                  status: StuffRequestItemStatusType.Issue);
          #endregion
        }
      }
      #endregion
      #region Add WarehouseIssueItems
      foreach (var addWarehouseIssueItem in addWarehouseIssueItems)
      {
        var stuffSerial = GetStuffSerial(serial: addWarehouseIssueItem.Serial);
        AddIndirectWarehouseIssueItemProcess(
                      warehouseIssueId: requestWarehouseIssue.Id,
                      stuffId: addWarehouseIssueItem.StuffId,
                      serial: addWarehouseIssueItem.Serial,
                      stuffSerialCode: stuffSerial.Code,
                      amount: addWarehouseIssueItem.Amount,
                      unitId: addWarehouseIssueItem.UnitId,
                      assetCode: addWarehouseIssueItem.AssetCode,
                      description: addWarehouseIssueItem.Description,
                      isRequestWarehouseIssue: true);
        if (toDepartmentId != null)
        {
          var existAsset = GetAssets(selector: e => e,
                   code: addWarehouseIssueItem.AssetCode)
               .Where(i => i.StuffSerial.Serial == addWarehouseIssueItem.Serial);
          if (existAsset.Any())
          {
            var asset = existAsset.FirstOrDefault();
            EditAssetProcess(id: asset.Id,
                      rowVersion: asset.RowVersion,
                      employeeId: toEmployeeId,
                      departmentId: toDepartmentId,
                      assetStatus: AssetStatus.IsTransferring);
          }
          else
          {
            var asset = AddAssetProcess(code: addWarehouseIssueItem.AssetCode,
                      stuffId: addWarehouseIssueItem.StuffId,
                      employeeId: toEmployeeId,
                      departmentId: toDepartmentId.Value,
                      warehouseId: fromWarehouseId,
                      description: description,
                      serial: addWarehouseIssueItem.Serial);
            EditStuffSerial(
                      stuffId: stuffSerial.StuffId,
                      code: stuffSerial.Code,
                      rowVersion: stuffSerial.RowVersion,
                      assetId: asset.Id);
          }
        }
      }
      #endregion
      return requestWarehouseIssue;
    }
    #endregion
    #region AddDirectProcess
    public WarehouseIssue AddDirectRequestWarehouseIssueProcess(
        TransactionBatch transactionBatch,
        short fromWarehouseId,
        short? toWarehouseId,
        int requestStuffId,
        byte requestUnitId,
        AddWarehouseIssueItemInput[] addWarehouseIssueItems,
        string description,
        int? toEmployeeId,
        short? toDepartmentId,
        IEnumerable<ResponseStuffRequestItem> responseStuffRequestItems)
    {
      #region AddWarehouseIssue
      var requestWarehouseIssue = AddRequestWarehouseIssue(
                  requestWarehouseIssue: null,
                  transactionBatch: transactionBatch,
                  fromWarehouseId: fromWarehouseId,
                  toWarehouseId: toWarehouseId,
                  status: WarehouseIssueStatusType.Accepted,
                  toEmployeeId: toEmployeeId,
                  toDepartmentId: toDepartmentId,
                  description: description);
      #endregion
      #region ExportBlockTransaction for RequestWarehouseIssue
      if (requestWarehouseIssue != null)
      {
        foreach (var responseStuffRequestItem in responseStuffRequestItems)
        {
          responseStuffRequestItem.RequestWarehouseIssueId = requestWarehouseIssue.Id;
          #region Get ImportBlockTransaction
          var importBlockTransaction = GetWarehouseTransactions(
                  selector: e => e,
                  transactionBatchId: responseStuffRequestItem.TransactionBatch.Id,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportBlock.Id)
              .SingleOrDefault();
          #endregion
          #region Add ExportBlockTransaction
          var exportBlockTransaction = AddWarehouseTransaction(
                  transactionBatchId: transactionBatch.Id,
                  effectDateTime: transactionBatch.DateTime,
                  stuffId: requestStuffId,
                  billOfMaterialVersion: null,
                  stuffSerialCode: null,
                  warehouseId: fromWarehouseId,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportBlock.Id,
                  amount: importBlockTransaction.Amount,
                  unitId: requestUnitId,
                  description: description,
                  referenceTransaction: importBlockTransaction);
          #endregion
          #region Add ImportAvailableTransaction
          AddWarehouseTransaction(
                  transactionBatchId: transactionBatch.Id,
                  effectDateTime: transactionBatch.DateTime,
                  stuffId: requestStuffId,
                  billOfMaterialVersion: null,
                  stuffSerialCode: null,
                  warehouseId: fromWarehouseId,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAvailable.Id,
                  amount: importBlockTransaction.Amount,
                  unitId: requestUnitId,
                  description: description,
                  referenceTransaction: exportBlockTransaction);
          #endregion
          #region ComplateResponseStuffRequestItemStatus
          ComplateResponseStuffRequestItemStatus(
                  responseStuffRequestItem: responseStuffRequestItem,
                  status: StuffRequestItemStatusType.Complated);
          #endregion
        }
      }
      #endregion
      #region Add WarehouseIssueItems
      foreach (var addWarehouseIssueItem in addWarehouseIssueItems)
      {
        var stuffSerial = GetStuffSerial(serial: addWarehouseIssueItem.Serial);
        AddDirectWarehouseIssueItemProcess(
                      warehouseIssueId: requestWarehouseIssue.Id,
                      stuffId: addWarehouseIssueItem.StuffId,
                      serial: addWarehouseIssueItem.Serial,
                      stuffSerialCode: stuffSerial.Code,
                      amount: addWarehouseIssueItem.Amount,
                      unitId: addWarehouseIssueItem.UnitId,
                      description: addWarehouseIssueItem.Description,
                      assetCode: addWarehouseIssueItem.AssetCode,
                      isRequestWarehouseIssue: true,
                      referenceTransaction: null);
      }
      #endregion
      return requestWarehouseIssue;
    }
    #endregion
  }
}
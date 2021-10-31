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
using lena.Models.Common;
using lena.Models.UserManagement.Permission;
using lena.Models.WarehouseManagement.WarehouseIssue;
using lena.Models.WarehouseManagement.WarehouseTransaction;
using lena.Models.UserManagement.SecurityAction;
using lena.Models.WarehouseManagement.BaseTransaction;
using lena.Models.WarehouseManagement.WarehouseReports;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public WarehouseIssue AddWarehouseIssue(
        WarehouseIssue warehouseIssue,
        TransactionBatch transactionBatch,
        short fromWarehouseId,
        short? toWarehouseId,
        WarehouseIssueStatusType status,
        string description,
        int? toEmployeeId,
        short? toDepartmentId)
    {
      warehouseIssue = warehouseIssue ?? repository.Create<WarehouseIssue>();
      warehouseIssue.FromWarehouseId = fromWarehouseId;
      warehouseIssue.ToWarehouseId = toWarehouseId;
      warehouseIssue.Status = status;
      warehouseIssue.ToEmployeeId = toEmployeeId;
      warehouseIssue.ToDepartmentId = toDepartmentId;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: warehouseIssue,
                    transactionBatch: transactionBatch,
                    description: description);
      return warehouseIssue;
    }
    #endregion
    #region Edit
    public WarehouseIssue EditWarehouseIssue(
        int id,
        byte[] rowVersion,
        TValue<short> fromWarehouseId = null,
        TValue<short?> toWarehouseId = null,
        TValue<WarehouseIssueStatusType> status = null,
        TValue<int> responseWarehouseIssueId = null,
        TValue<string> description = null,
        TValue<int?> toEmployeeId = null,
        TValue<short?> toDepartmentId = null)
    {
      var warehouseIssue = GetWarehouseIssue(id: id);
      return EditWarehouseIssue(
                    warehouseIssue: warehouseIssue,
                    rowVersion: rowVersion,
                    fromWarehouseId: fromWarehouseId,
                    toWarehouseId: toWarehouseId,
                    status: status,
                    responseWarehouseIssueId: responseWarehouseIssueId,
                    description: description,
                    toEmployeeId: toEmployeeId,
                    toDepartmentId: toDepartmentId);
    }
    public WarehouseIssue EditWarehouseIssue(
        WarehouseIssue warehouseIssue,
        byte[] rowVersion,
        TValue<short> fromWarehouseId = null,
        TValue<short?> toWarehouseId = null,
        TValue<WarehouseIssueStatusType> status = null,
        TValue<int> responseWarehouseIssueId = null,
        TValue<string> description = null,
        TValue<int?> toEmployeeId = null,
        TValue<short?> toDepartmentId = null)
    {
      if (fromWarehouseId != null)
        warehouseIssue.FromWarehouseId = fromWarehouseId;
      if (toWarehouseId != null)
        warehouseIssue.ToWarehouseId = toWarehouseId;
      if (status != null)
        warehouseIssue.Status = status;
      if (description != null)
        warehouseIssue.Description = description;
      if (toEmployeeId != null)
        warehouseIssue.ToEmployeeId = toEmployeeId;
      if (toDepartmentId != null)
        warehouseIssue.ToDepartmentId = toDepartmentId;
      if (responseWarehouseIssueId != null)
        warehouseIssue.ResponseWarehouseIssue = GetResponseWarehouseIssue(id: responseWarehouseIssueId);
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: warehouseIssue,
                    rowVersion: rowVersion,
                    description: description);
      return retValue as WarehouseIssue;
    }
    #endregion
    #region Get
    public WarehouseIssue GetWarehouseIssue(int id) => GetWarehouseIssue(selector: e => e, id: id);
    public TResult GetWarehouseIssue<TResult>(
        Expression<Func<WarehouseIssue, TResult>> selector,
        int id)
    {
      var result = GetWarehouseIssues(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new WarehouseIssueNotFoundException(id);
      return result;
    }
    public WarehouseIssue GetWarehouseIssue(string code) => GetWarehouseIssue(selector: e => e, code: code);
    public TResult GetWarehouseIssue<TResult>(
        Expression<Func<WarehouseIssue, TResult>> selector,
        string code)
    {
      var result = GetWarehouseIssues(
                selector: selector,
                code: code).FirstOrDefault();
      if (result == null)
        throw new WarehouseIssueNotFoundException(code);
      return result;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetWarehouseIssues<TResult>(
        Expression<Func<WarehouseIssue, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> fromWarehouseId = null,
        TValue<int?> toWarehouseId = null,
        TValue<WarehouseIssueStatusType> status = null,
        string serial = null,
        TValue<int?> toEmployeeId = null,
        TValue<int?> toDepartmentId = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<WarehouseIssue>();
      if (fromWarehouseId != null)
        query = query.Where(r => r.FromWarehouseId == fromWarehouseId);
      if (toWarehouseId != null)
        query = query.Where(r => r.ToWarehouseId == toWarehouseId);
      if (status != null)
        query = query.Where(r => r.Status == status);
      if (serial != null)
      {
        var warehouseIssueItem = GetWarehouseIssueItems(
                      selector: e => e.WarehouseIssueId,
                      serial: serial,
                      isDelete: false);
        var warehouseIssueIds = warehouseIssueItem.Distinct();
        query = from item in query
                join warehouseIssueId in warehouseIssueIds on item.Id equals warehouseIssueId
                select item;
      }
      if (toEmployeeId != null)
        query = query.Where(r => r.ToEmployeeId == toEmployeeId);
      if (toDepartmentId != null)
        query = query.Where(r => r.ToDepartmentId == toDepartmentId);
      return query.Select(selector);
    }
    public IQueryable<WarehouseTransactionAggregatedReportResult> GetWarehouseTransactionAggregatedReport(
       DateTime startDate,
       DateTime endDate,
       TValue<int> stuffId = null,
       TValue<int> stuffCategoryId = null,
       TValue<int> warehouseId = null)
    {
      var warehouse = App.Internals.WarehouseManagement;
      #region transaction Query
      var transactions = warehouse.GetWarehouseTransactions(
              selector: warehouse.ToWarehouseTransactionMinResult,
              stuffId: stuffId,
              warehouseId: warehouseId,
              stuffCategoryId: stuffCategoryId);
      #endregion
      #region Inventory Query
      var inventoryBaseQuery = from transaction in transactions
                               group transaction by transaction.StuffId into gItems
                               select new
                               {
                                 StuffId = gItems.Key,
                                 BeginTermQty = (double?)gItems.Where(t => t.EffectDateTime < startDate).Sum(i => i.Value) ?? 0,
                                 BeginTermPrice = 0,
                                 IncomingQty = (double?)gItems.Where(t => t.EffectDateTime >= startDate && t.EffectDateTime <= endDate).Sum(i => i.Value) ?? 0,
                                 IncomingPrice = 0,
                                 IssuedQty = (double?)gItems.Where(t => t.EffectDateTime >= startDate && t.EffectDateTime <= endDate).Sum(i => i.Value) ?? 0,
                                 IssuedPrice = 0,
                                 RemainQty = (double?)gItems.Where(t => t.EffectDateTime > endDate).Sum(i => i.Value) ?? 0,
                                 RemainPrice = 0,
                               };
      #endregion
      #region Get Units
      var units = App.Internals.ApplicationBase.GetUnits(
              selector: i => new { i.Id, i.Name, i.UnitTypeId },
              isMainUnit: true,
              isActive: true);
      #endregion
      #region GetStuffs
      var stuffs = App.Internals.SaleManagement.GetStuffs(
              selector: e => e,
              id: stuffId,
              stuffCategoryId: stuffCategoryId);
      #endregion
      #region BaseQuery 
      var baseQuery = from stuff in stuffs
                      join unit in units on stuff.UnitTypeId equals unit.UnitTypeId
                      select new
                      {
                        StuffId = stuff.Id,
                        StuffName = stuff.Name,
                        StuffNoun = stuff.Noun,
                        StuffTitle = stuff.Title,
                        StuffCode = stuff.Code,
                        UnitId = unit.Id,
                        UnitName = unit.Name
                      };
      #endregion
      #region InventoryQuery
      var result = from inventory in inventoryBaseQuery
                   join stuffInfo in baseQuery on inventory.StuffId equals stuffInfo.StuffId
                   select new WarehouseTransactionAggregatedReportResult
                   {
                     StuffName = stuffInfo.StuffName,
                     StuffNoun = stuffInfo.StuffNoun,
                     StuffTitle = stuffInfo.StuffTitle,
                     StuffCode = stuffInfo.StuffCode,
                     UnitId = stuffInfo.UnitId,
                     UnitName = stuffInfo.UnitName,
                     StuffId = inventory.StuffId,
                     BeginTermQty = inventory.BeginTermQty,
                     BeginTermPrice = inventory.BeginTermPrice,
                     IncomingQty = inventory.IncomingQty,
                     IncomingPrice = inventory.IncomingPrice,
                     IssuedQty = inventory.IssuedQty,
                     IssuedPrice = inventory.IssuedPrice,
                     RemainQty = inventory.RemainQty,
                     RemainPrice = inventory.RemainPrice,
                     CurrencyId = 1,
                     CurrencyName = "as"
                   };
      #endregion
      return result;
    }
    #endregion
    #region AddProcess
    public WarehouseIssue AddWarehouseIssueProcess(
        TransactionBatch transactionBatch,
        short fromWarehouseId,
        short? toWarehouseId,
        AddWarehouseIssueItemInput[] addWarehouseIssueItems,
        string description,
        int? toEmployeeId,
        short? toDepartmentId)
    {
      #region Check IsDeletedWarehouse
      var fromWarehouse = App.Internals.WarehouseManagement.GetWarehouse(e => e, id: fromWarehouseId);
      if (fromWarehouse.IsDeleted == true)
      {
        throw new WarehouseHaseDeletedException(fromWarehouseId);
      }
      if (toWarehouseId != null)
      {
        var toWarehouse = App.Internals.WarehouseManagement.GetWarehouse(e => e, id: toWarehouseId.Value);
        if (toWarehouse.IsDeleted == true)
        {
          throw new WarehouseHaseDeletedException(toWarehouseId);
        }
      }
      #endregion
      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      transactionBatch = transactionBatch ?? warehouseManagement.AddTransactionBatch();
      #endregion
      #region Check IsDirectWarehouseIssue
      var directWarehouseIssuePermission = CheckIsDirectWarehouseIssue(
              fromWarehouseId: fromWarehouseId,
              toWarehouseId: toWarehouseId ?? fromWarehouseId);
      #endregion
      #region AddProcess
      //TODO fix ssss
      WarehouseIssue warehouseIssue = null;
      if (directWarehouseIssuePermission == AccessType.Allowed)
        warehouseIssue = AddDirectWarehouseIssueProcess(
                  transactionBatch: transactionBatch,
                  fromWarehouseId: fromWarehouseId,
                  toWarehouseId: toWarehouseId,
                  addWarehouseIssueItems: addWarehouseIssueItems,
                  description: description,
                  toEmployeeId: toEmployeeId,
                  toDepartmentId: toDepartmentId);
      else
        warehouseIssue = AddIndirectWarehouseIssueProcess(
                  transactionBatch: transactionBatch,
                  fromWarehouseId: fromWarehouseId,
                  toWarehouseId: toWarehouseId,
                  addWarehouseIssueItems: addWarehouseIssueItems,
                  description: description,
                  toEmployeeId: toEmployeeId,
                  toDepartmentId: toDepartmentId);
      #endregion
      return warehouseIssue;
    }
    #endregion
    #region AddIndirectProcess
    public WarehouseIssue AddIndirectWarehouseIssueProcess(
        TransactionBatch transactionBatch,
        short fromWarehouseId,
        short? toWarehouseId,
        AddWarehouseIssueItemInput[] addWarehouseIssueItems,
        string description,
        int? toEmployeeId,
        short? toDepartmentId)
    {
      #region CheckWarehouseIssueConfirmDeadline
      var hasDeadline = CheckWarehouseIssueConfirmDeadline(
              fromWarehouseId: fromWarehouseId,
              toWarehouseId: toWarehouseId,
              toDepartmentId: toDepartmentId);
      if (hasDeadline == true)
        throw new WarehouseIssueHasDeadlineException();
      #endregion
      #region AddWarehouseIssue
      var warehouseIssue = AddWarehouseIssue(
                  warehouseIssue: null,
                  transactionBatch: transactionBatch,
                  fromWarehouseId: fromWarehouseId,
                  toWarehouseId: toWarehouseId,
                  status: WarehouseIssueStatusType.Waiting,
                  description: description,
                  toDepartmentId: toDepartmentId,
                  toEmployeeId: toEmployeeId);
      #endregion
      var stuffIds = addWarehouseIssueItems.GroupBy(i => i.StuffId).Select(i => i.Key).ToArray();
      var stuffs = App.Internals.SaleManagement.GetStuffs(e => new { e.Id, e.IsTraceable }, ids: stuffIds);
      var inputSerials = addWarehouseIssueItems.Select(x => x.Serial).ToArray();
      var serials = GetStuffSerials(e => new { e.Code, e.Serial }, serials: inputSerials);
      #region Add WarehouseIssueItems
      foreach (var addWarehouseIssueItem in addWarehouseIssueItems)
      {
        //var stuff = App.Internals.SaleManagement.GetStuff(addWarehouseIssueItem.StuffId)
        //    
        //;
        var stuff = stuffs.FirstOrDefault(s => s.Id == addWarehouseIssueItem.StuffId);
        long? stuffSerialCode = null;
        if (stuff.IsTraceable)
          stuffSerialCode = serials.FirstOrDefault(i => i.Serial == addWarehouseIssueItem.Serial)?.Code;
        AddIndirectWarehouseIssueItemProcess(
                      warehouseIssueId: warehouseIssue.Id,
                      stuffId: addWarehouseIssueItem.StuffId,
                      stuffSerialCode: stuffSerialCode,
                      amount: addWarehouseIssueItem.Amount,
                      unitId: addWarehouseIssueItem.UnitId,
                      serial: addWarehouseIssueItem.Serial,
                      assetCode: addWarehouseIssueItem.AssetCode,
                      description: addWarehouseIssueItem.Description);
      }
      #endregion
      return warehouseIssue;
    }
    #endregion
    #region AddDirectProcess
    public WarehouseIssue AddDirectWarehouseIssueProcess(
        TransactionBatch transactionBatch,
        short fromWarehouseId,
        short? toWarehouseId,
        AddWarehouseIssueItemInput[] addWarehouseIssueItems,
        string description,
        int? toEmployeeId,
        short? toDepartmentId)
    {
      #region CheckWarehouseIssueConfirmDeadline
      var hasDeadline = CheckWarehouseIssueConfirmDeadline(
              fromWarehouseId: fromWarehouseId,
              toWarehouseId: toWarehouseId,
              toDepartmentId: toDepartmentId);
      if (hasDeadline == true)
        throw new WarehouseIssueHasDeadlineException();
      #endregion
      #region AddWarehouseIssue
      var warehouseIssue = AddWarehouseIssue(
                  warehouseIssue: null,
                  transactionBatch: transactionBatch,
                  fromWarehouseId: fromWarehouseId,
                  toWarehouseId: toWarehouseId,
                  status: WarehouseIssueStatusType.Accepted,
                  description: description,
                  toDepartmentId: toDepartmentId,
                  toEmployeeId: toEmployeeId);
      #endregion
      #region Add WarehouseIssueItems
      foreach (var addWarehouseIssueItem in addWarehouseIssueItems)
      {
        var stuff = App.Internals.SaleManagement.GetStuff(addWarehouseIssueItem.StuffId);
        long? stuffSerialCode = null;
        if (stuff.IsTraceable)
        {
          stuffSerialCode = GetStuffSerial(
                    selector: e => e.Code,
                    serial: addWarehouseIssueItem.Serial);
        }
        AddDirectWarehouseIssueItemProcess(
                      warehouseIssueId: warehouseIssue.Id,
                      stuffId: addWarehouseIssueItem.StuffId,
                      serial: addWarehouseIssueItem.Serial,
                      stuffSerialCode: stuffSerialCode,
                      amount: addWarehouseIssueItem.Amount,
                      unitId: addWarehouseIssueItem.UnitId,
                      assetCode: addWarehouseIssueItem.AssetCode,
                      description: addWarehouseIssueItem.Description);
      }
      #endregion
      return warehouseIssue;
    }
    #endregion
    #region AcceptProcess
    public WarehouseIssue AcceptWarehouseIssueProcess(
        TransactionBatch transactionBatch,
        int id,
        byte[] rowVersion,
        int fromWarehouseId,
        int? toWarehouseId,
        string description)
    {
      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      transactionBatch = transactionBatch ?? warehouseManagement.AddTransactionBatch();
      #endregion
      #region Get WarehouseIssue
      var warehouseIssue = GetWarehouseIssue(id: id);
      #endregion
      #region Check WarehouseIssueStatus
      if (warehouseIssue.Status != WarehouseIssueStatusType.Waiting)
        throw new WarehouseIssueNotInWatingStatusException(id);
      #endregion
      #region Check Warehouses
      if (warehouseIssue.FromWarehouseId != fromWarehouseId ||
          warehouseIssue.ToWarehouseId != toWarehouseId)
        throw new ResponseWarehouseIssueWarehouseNotMatchException(id);
      #endregion
      #region AddResponseWarehouseIssueProcess
      var responseWarehouseIssue = AddResponseWarehouseIssue(
              responseWarehouseIssue: null,
              transactionBatch: transactionBatch,
              warehouseIssueId: id,
              description: description);
      #endregion
      #region EditWarehouseIssue
      EditWarehouseIssue(
          warehouseIssue: warehouseIssue,
          rowVersion: rowVersion,
          responseWarehouseIssueId: responseWarehouseIssue.Id,
          description: responseWarehouseIssue.Description,
          status: WarehouseIssueStatusType.Accepted);
      #endregion
      #region Get WarehouseTransactions
      var transactions = GetWarehouseTransactions(
              selector: e => e,
              transactionBatchId: warehouseIssue.TransactionBatch.Id,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAsBlockedStoreIssue.Id);
      #endregion
      #region Accept WarehouseTransaction
      var stuffIds = new List<int>();
      var warehouseIds = new List<int>();
      foreach (var transaction in transactions)
      {
        if (transaction.WarehouseId == null)
          throw new BaseTransactionHasNoWarehouseException(id: transaction.Id);
        if (!warehouseIds.Contains(transaction.WarehouseId.Value))
          warehouseIds.Add(transaction.WarehouseId.Value);
        #region Add ExportAsConfirmedBlockedStoreIssue
        var warehouseIssueItem = App.Internals.WarehouseManagement.GetWarehouseIssueItems(
                e => e,
                warehouseIssueId: warehouseIssue.Id,
                serial: transaction.StuffSerial.Serial,
                isDelete: false)
            .FirstOrDefault();
        TransactionLevel transactionLevel;
        if (warehouseIssueItem != null)
          transactionLevel = warehouseIssueItem.TransactionLevel;
        else
          continue;
        var serial = warehouseIssueItem.StuffSerial;
        //Update who transfer and confirm serial for warehouse inventory report
        App.Internals.WarehouseManagement.EditStuffSerial(
            stuffId: serial.StuffId,
            code: serial.Code,
            rowVersion: serial.RowVersion,
            issueUserId: warehouseIssue.UserId,
            issueConfirmerUserId: App.Providers.Security.CurrentLoginData.UserId,
            warehouseEnterTime: DateTime.UtcNow);
        if (serial.Asset != null)
        {
          string transactionToEmployee = " حواله اموال با کد اموال: " + serial.Asset.Code + "به پرسنل";
          EditAsset(
                    id: serial.Asset.Id,
                    rowVersion: serial.Asset.RowVersion,
                    status: AssetStatus.Referred);
          description = description + transactionToEmployee;
        }
        var exportBlockTransaction = AddWarehouseTransaction(
                      transactionBatchId: transactionBatch.Id,
                      effectDateTime: transactionBatch.DateTime,
                      stuffId: transaction.StuffId,
                      billOfMaterialVersion: transaction.BillOfMaterialVersion,
                      stuffSerialCode: transaction.StuffSerialCode,
                      warehouseId: transaction.WarehouseId.Value,
                      transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportAsConfirmedBlockedStoreIssue.Id,
                      amount: transaction.Amount,
                      unitId: transaction.UnitId,
                      description: description,
                      referenceTransaction: transaction);
        #endregion
        #region Add ImportAsIndirectStoreIssue
        var transactionTypeImporttId = Models.StaticData.StaticTransactionTypes.ImportAsIndirectStoreIssue.Id;
        if (transactionLevel == TransactionLevel.QualityControl)
          transactionTypeImporttId = Models.StaticData.StaticTransactionTypes.ImportQualityControl.Id;
        if (transactionLevel == TransactionLevel.Waste)
          transactionTypeImporttId = Models.StaticData.StaticTransactionTypes.ImportWaste.Id;
        if (warehouseIssue.ToWarehouseId != null)
        {
          if (!warehouseIds.Contains(warehouseIssue.ToWarehouseId.Value))
            warehouseIds.Add(warehouseIssue.ToWarehouseId.Value);
          var importAccessTransaction = AddWarehouseTransaction(
                          transactionBatchId: transactionBatch.Id,
                          effectDateTime: transactionBatch.DateTime,
                          stuffId: transaction.StuffId,
                          billOfMaterialVersion: exportBlockTransaction.BillOfMaterialVersion,
                          stuffSerialCode: transaction.StuffSerialCode,
                          warehouseId: warehouseIssue.ToWarehouseId.Value,
                          transactionTypeId: transactionTypeImporttId,
                          amount: transaction.Amount,
                          unitId: transaction.UnitId,
                          description: description,
                          referenceTransaction: exportBlockTransaction);
        }
        #endregion
        if (!stuffIds.Contains(transaction.StuffId))
          stuffIds.Add(transaction.StuffId);
      }
      #endregion
      #region ComplateResponseStuffRequestItemStatus
      var requestWarehouseIssue = warehouseIssue as RequestWarehouseIssue;
      if (requestWarehouseIssue != null)
      {
        foreach (var responseStuffRequestItem in requestWarehouseIssue.ResponseStuffRequestItems)
        {
          ComplateResponseStuffRequestItemStatus(
                        responseStuffRequestItem: responseStuffRequestItem,
                        status: StuffRequestItemStatusType.Complated);
        }
      }
      #endregion
      return warehouseIssue;
    }
    #endregion
    #region RejectProcess
    public WarehouseIssue RejectWarehouseIssueProcess(
        TransactionBatch transactionBatch,
        int id,
        byte[] rowVersion,
        int fromWarehouseId,
        int? toWarehouseId,
        string description)
    {
      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      transactionBatch = transactionBatch ?? warehouseManagement.AddTransactionBatch();
      #endregion
      #region Check WarehouseIssueStatus
      var warehouseIssue = GetWarehouseIssue(id: id);
      if (warehouseIssue.Status != WarehouseIssueStatusType.Waiting)
        throw new WarehouseIssueNotInWatingStatusException(id);
      #endregion
      #region Check Warehouses
      if (warehouseIssue.FromWarehouseId != fromWarehouseId ||
          warehouseIssue.ToWarehouseId != toWarehouseId)
        throw new ResponseWarehouseIssueWarehouseNotMatchException(id);
      #endregion
      #region AddResponseWarehouseIssueProcess
      var responseWarehouseIssue = AddResponseWarehouseIssue(
              responseWarehouseIssue: null,
              transactionBatch: transactionBatch,
              warehouseIssueId: id,
              description: description);
      #endregion
      #region EditWarehouseIssue
      EditWarehouseIssue(
          warehouseIssue: warehouseIssue,
          rowVersion: rowVersion,
          responseWarehouseIssueId: responseWarehouseIssue.Id,
          status: WarehouseIssueStatusType.Rejected,
          description: responseWarehouseIssue.Description);
      #endregion
      #region Get WarehouseTransactions
      var transactions = GetWarehouseTransactions(
              selector: e => e,
              transactionBatchId: warehouseIssue.TransactionBatch.Id,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAsBlockedStoreIssue.Id);
      #endregion
      #region Reject WarehouseTransaction
      var stuffIds = new List<int>();
      foreach (var transaction in transactions)
      {
        if (transaction.WarehouseId == null)
          throw new BaseTransactionHasNoWarehouseException(id: transaction.Id);
        var warehouseIssueItem = App.Internals.WarehouseManagement.GetWarehouseIssueItems(
                      e => e,
                      warehouseIssueId: warehouseIssue.Id,
                      serial: transaction.StuffSerial.Serial,
                      isDelete: false)
                  .FirstOrDefault();
        TransactionLevel transactionLevel;
        if (warehouseIssueItem != null)
          transactionLevel = warehouseIssueItem.TransactionLevel;
        else
          continue;
        var asset = warehouseIssueItem.StuffSerial.Asset;
        if (asset != null)
        {
          EditAsset(id: asset.Id,
                    rowVersion: asset.RowVersion,
                    status: AssetStatus.DeliveredToWarehouse);
        }
        #region Add ExportAsRejectedBlockedStoreIssue Transaction
        var exportBlockTransaction = AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: transaction.StuffId,
                billOfMaterialVersion: transaction.BillOfMaterialVersion,
                stuffSerialCode: transaction.StuffSerialCode,
                warehouseId: transaction.WarehouseId.Value,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportAsRejectedBlockedStoreIssue.Id,
                amount: transaction.Amount,
                unitId: transaction.UnitId,
                description: description,
                referenceTransaction: transaction);
        #endregion
        #region Add ImportAsIndirectStoreIssue Transaction
        var transactionTypeImportId = Models.StaticData.StaticTransactionTypes.ImportAsIndirectStoreIssue.Id;
        if (transactionLevel == TransactionLevel.QualityControl)
          transactionTypeImportId = Models.StaticData.StaticTransactionTypes.ImportQualityControl.Id;
        if (transactionLevel == TransactionLevel.Waste)
          transactionTypeImportId = Models.StaticData.StaticTransactionTypes.ImportWaste.Id;
        var importAccessTransaction = AddWarehouseTransaction(
                        transactionBatchId: transactionBatch.Id,
                        effectDateTime: transactionBatch.DateTime,
                        stuffId: transaction.StuffId,
                        billOfMaterialVersion: exportBlockTransaction.BillOfMaterialVersion,
                        stuffSerialCode: transaction.StuffSerialCode,
                        warehouseId: warehouseIssue.FromWarehouseId,
                        transactionTypeId: transactionTypeImportId,
                        amount: transaction.Amount,
                        unitId: transaction.UnitId,
                        description: description,
                        referenceTransaction: exportBlockTransaction);
        #endregion
        if (!stuffIds.Contains(transaction.StuffId))
          stuffIds.Add(transaction.StuffId);
      }
      #endregion
      #region CheckTransactionBatch
      var warehouseIds = new List<int>();
      warehouseIds.Add(fromWarehouseId);
      if (toWarehouseId != null)
        warehouseIds.Add(toWarehouseId.Value);
      #endregion
      #region ComplateResponseStuffRequestItemStatus
      var requestWarehouseIssue = warehouseIssue as RequestWarehouseIssue;
      if (requestWarehouseIssue != null)
      {
        foreach (var responseStuffRequestItem in requestWarehouseIssue.ResponseStuffRequestItems)
        {
          ComplateResponseStuffRequestItemStatus(
                        responseStuffRequestItem: responseStuffRequestItem,
                        status: StuffRequestItemStatusType.RejectedIssue);
        }
      }
      #endregion
      return warehouseIssue;
    }
    #endregion
    #region Search
    public IQueryable<WarehouseIssueResult> SearchWarehouseIssueResult(IQueryable<WarehouseIssueResult> query,
        DateTime? fromDateTime,
        DateTime? toDateTime,
        string search,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(search))
        query = query.Where(item =>
            item.Code.Contains(search));
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<WarehouseIssueResult> SortWarehouseIssueResult(IQueryable<WarehouseIssueResult> query,
        SortInput<WarehouseIssueSortType> sort)
    {
      switch (sort.SortType)
      {
        case WarehouseIssueSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case WarehouseIssueSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case WarehouseIssueSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case WarehouseIssueSortType.FromWarehouseId:
          return query.OrderBy(a => a.FromWarehouseId, sort.SortOrder);
        case WarehouseIssueSortType.FromWarehouseName:
          return query.OrderBy(a => a.FromWarehouseName, sort.SortOrder);
        case WarehouseIssueSortType.ToWarehouseId:
          return query.OrderBy(a => a.ToWarehouseId, sort.SortOrder);
        case WarehouseIssueSortType.ToWarehouseName:
          return query.OrderBy(a => a.ToWarehouseName, sort.SortOrder);
        case WarehouseIssueSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case WarehouseIssueSortType.UserName:
          return query.OrderBy(a => a.UserName, sort.SortOrder);
        case WarehouseIssueSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case WarehouseIssueSortType.ResponseUserName:
          return query.OrderBy(a => a.ResponseUserName, sort.SortOrder);
        case WarehouseIssueSortType.ResponseEmployeeFullName:
          return query.OrderBy(a => a.ResponseEmployeeFullName, sort.SortOrder);
        case WarehouseIssueSortType.ResponseDateTime:
          return query.OrderBy(a => a.ResponseDateTime, sort.SortOrder);
        case WarehouseIssueSortType.ToDepartmentName:
          return query.OrderBy(a => a.ToDepartmentName, sort.SortOrder);
        case WarehouseIssueSortType.ToEmployeeFullName:
          return query.OrderBy(a => a.ToEmployeeFullName, sort.SortOrder);
        case WarehouseIssueSortType.ConfirmDescription:
          return query.OrderBy(a => a.ConfirmDescription, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<SerialIssueDetailResult> SortSerialIssueDetailResult(IQueryable<SerialIssueDetailResult> query,
        SortInput<SerialIssueDetailSortType> sort)
    {
      IOrderedQueryable<SerialIssueDetailResult> sortedQuery = null;
      switch (sort.SortType)
      {
        case SerialIssueDetailSortType.WarehouseIssueId:
          sortedQuery = query.OrderBy(x => x.WarehouseIssueId, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.FromWarehouseName:
          sortedQuery = query.OrderBy(x => x.FromWarehouseName, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.ToWarehouseName:
          sortedQuery = query.OrderBy(x => x.ToWarehouseName, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.ToDepartmentName:
          sortedQuery = query.OrderBy(x => x.ToDepartmentName, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.ToEmployeeName:
          sortedQuery = query.OrderBy(x => x.ToEmployeeName, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.DateTime:
          sortedQuery = query.OrderBy(x => x.DateTime, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.Status:
          sortedQuery = query.OrderBy(x => x.Status, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.Serial:
          sortedQuery = query.OrderBy(x => x.Serial, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.StuffName:
          sortedQuery = query.OrderBy(x => x.StuffName, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.Amount:
          sortedQuery = query.OrderBy(x => x.Amount, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.UnitName:
          sortedQuery = query.OrderBy(x => x.UnitName, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.BillOfMaterialVersionId:
          sortedQuery = query.OrderBy(x => x.BillOfMaterialVersionId, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.TransactionLevel:
          sortedQuery = query.OrderBy(x => x.TransactionLevel, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.ConfirmerName:
          sortedQuery = query.OrderBy(x => x.ConfirmerFullName, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.ConfirmDateTime:
          sortedQuery = query.OrderBy(x => x.ConfirmDateTime, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.EmployeeFullName:
          sortedQuery = query.OrderBy(x => x.EmployeeFullName, sort.SortOrder);
          break;
        case SerialIssueDetailSortType.UserName:
          sortedQuery = query.OrderBy(x => x.UserName, sort.SortOrder);
          break;
        default:
          throw new ArgumentOutOfRangeException("SerialIssueDetailResult Sort Type not Supported!, SortType: " + sort.SortType.ToString());
      }
      return sortedQuery;
    }
    //SortWarehouseTransactionAggregatedReport
    public IOrderedQueryable<WarehouseTransactionAggregatedReportResult> SortWarehouseTransactionAggregatedReport(
        IQueryable<WarehouseTransactionAggregatedReportResult> query,
        SortInput<WarehouseTransactionAggregatedReportSortType> sort)
    {
      switch (sort.SortType)
      {
        case WarehouseTransactionAggregatedReportSortType.StuffNoun:
          return query.OrderBy(a => a.StuffNoun, sort.SortOrder);
        case WarehouseTransactionAggregatedReportSortType.StuffTitle:
          return query.OrderBy(a => a.StuffTitle, sort.SortOrder);
        case WarehouseTransactionAggregatedReportSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case WarehouseTransactionAggregatedReportSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case WarehouseTransactionAggregatedReportSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case WarehouseTransactionAggregatedReportSortType.BeginTermQty:
          return query.OrderBy(a => a.BeginTermQty, sort.SortOrder);
        case WarehouseTransactionAggregatedReportSortType.BeginTermPrice:
          return query.OrderBy(a => a.BeginTermPrice, sort.SortOrder);
        case WarehouseTransactionAggregatedReportSortType.IncomingQty:
          return query.OrderBy(a => a.IncomingQty, sort.SortOrder);
        case WarehouseTransactionAggregatedReportSortType.IncomingPrice:
          return query.OrderBy(a => a.IncomingPrice, sort.SortOrder);
        case WarehouseTransactionAggregatedReportSortType.IssuedQty:
          return query.OrderBy(a => a.IssuedQty, sort.SortOrder);
        case WarehouseTransactionAggregatedReportSortType.IssuedPrice:
          return query.OrderBy(a => a.IssuedPrice, sort.SortOrder);
        case WarehouseTransactionAggregatedReportSortType.RemainQty:
          return query.OrderBy(a => a.RemainQty, sort.SortOrder);
        case WarehouseTransactionAggregatedReportSortType.RemainPrice:
          return query.OrderBy(a => a.RemainPrice, sort.SortOrder);
        case WarehouseTransactionAggregatedReportSortType.CurrencyName:
          return query.OrderBy(a => a.CurrencyName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToWarehouseIssueResult
    public Expression<Func<WarehouseIssue, WarehouseIssueResult>> ToWarehouseIssueResult =
        warehouseIssue => new WarehouseIssueResult
        {
          Id = warehouseIssue.Id,
          Code = warehouseIssue.Code,
          DateTime = warehouseIssue.DateTime,
          FromWarehouseId = warehouseIssue.FromWarehouseId,
          FromWarehouseName = warehouseIssue.FromWarehouse.Name,
          ToWarehouseId = warehouseIssue.ToWarehouseId,
          ToWarehouseName = warehouseIssue.ToWarehouse.Name,
          Status = warehouseIssue.Status,
          UserName = warehouseIssue.User.UserName,
          EmployeeId = warehouseIssue.User.Employee.Id,
          EmployeeFullName = warehouseIssue.User.Employee.FirstName + " " + warehouseIssue.User.Employee.LastName,
          ResponseDateTime = warehouseIssue.ResponseWarehouseIssue.DateTime,
          ResponseUserName = warehouseIssue.ResponseWarehouseIssue.User.UserName,
          ResponseEmployeeFullName = warehouseIssue.ResponseWarehouseIssue.User.Employee.FirstName + " " + warehouseIssue.ResponseWarehouseIssue.User.Employee.LastName,
          ToDepartmentId = warehouseIssue.ToDepartmentId,
          ToDepartmentName = warehouseIssue.ToDepartment.Name,
          ToEmployeeId = warehouseIssue.ToEmployeeId,
          ToEmployeeFullName = warehouseIssue.ToEmployee.FirstName + " " + warehouseIssue.ToEmployee.LastName,
          ConfirmDescription = warehouseIssue.Description,
          RowVersion = warehouseIssue.RowVersion
        };
    #endregion
    #region ToWarehouseIssueFullResult
    public Expression<Func<WarehouseIssue, WarehouseIssueFullResult>> ToWarehouseIssueFullResult =
        warehouseIssue => new WarehouseIssueFullResult
        {
          Id = warehouseIssue.Id,
          Code = warehouseIssue.Code,
          DateTime = warehouseIssue.DateTime,
          FromWarehouseId = warehouseIssue.FromWarehouseId,
          FromWarehouseName = warehouseIssue.FromWarehouse.Name,
          ToWarehouseId = warehouseIssue.ToWarehouseId,
          ToWarehouseName = warehouseIssue.ToWarehouse.Name,
          Status = warehouseIssue.Status,
          UserName = warehouseIssue.User.UserName,
          EmployeeFullName = warehouseIssue.User.Employee.FirstName + " " + warehouseIssue.User.Employee.LastName,
          ResponseUserName = warehouseIssue.ResponseWarehouseIssue.User.UserName,
          ResponseDateTime = warehouseIssue.ResponseWarehouseIssue.DateTime,
          ResponseEmployeeFullName = warehouseIssue.ResponseWarehouseIssue.User.Employee.FirstName + " " + warehouseIssue.ResponseWarehouseIssue.User.Employee.LastName,
          Transactions = warehouseIssue.TransactionBatch.BaseTransactions.AsQueryable()
                .Select(App.Internals.WarehouseManagement.ToBaseTransactionResult),
          RowVersion = warehouseIssue.RowVersion
        };
    #endregion
    #region Check IsDirectWarehouseIssue
    public AccessType CheckIsDirectWarehouseIssue(
        int fromWarehouseId,
        int toWarehouseId)
    {
      #region Check IsDirectWarehouseIssue
      var parameters = new List<ActionParameterInput>();
      //parameters.Add(new ActionParameterInput() { Key = "FromWarehouseId", Value = fromWarehouseId.ToString() });
      parameters.Add(new ActionParameterInput() { Key = "ToWarehouseId", Value = toWarehouseId.ToString() });
      var directWarehouseIssuePermission =
                App.Internals.UserManagement.CheckPermission(
                        actionName: Models.StaticData.StaticActionName.AcceptWarehouseIssue,
                        actionParameters: parameters.ToArray());
      return directWarehouseIssuePermission.AccessType;
      #endregion
    }
    #endregion
    #region CheckWarehouseIssueConfirmDeadline
    public bool CheckWarehouseIssueConfirmDeadline(
        int fromWarehouseId,
        int? toWarehouseId = null,
        int? toDepartmentId = null)
    {
      #region WarehouseIssueConfirmDeadline
      var hours = App.Internals.ApplicationSetting.WarehouseIssueConfirmDeadline();
      #endregion
      var warehouseIssueResults = GetWarehouseIssues
          (
              selector: e => e,
              status: WarehouseIssueStatusType.Waiting,
              toDepartmentId: toDepartmentId,
              isDelete: false)
          .Where
          (
              x => x.FromWarehouseId == fromWarehouseId ||
              x.ToWarehouseId == fromWarehouseId
          );
      var dateDeadline = DateTime.Now.AddHours(-hours);
      return warehouseIssueResults.Any(i => i.DateTime < dateDeadline);
    }
    #endregion
  }
}
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.WarehouseManagement.PreparingSendingItem;
using lena.Models.WarehouseManagement.PreparingSending;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Common;
using System.Collections.Generic;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public PreparingSendingItem AddPreparingSendingItem(
        PreparingSendingItem preparingSendingItem,
        TransactionBatch transactionBatch,
        int preparingSendingId,
        double qty,
        byte unitId,
        int stuffId,
        long stuffSerialCode,
        string description)
    {
      preparingSendingItem = preparingSendingItem ?? repository.Create<PreparingSendingItem>();
      preparingSendingItem.PreparingSendingId = preparingSendingId;
      preparingSendingItem.Qty = qty;
      preparingSendingItem.UnitId = unitId;
      preparingSendingItem.StuffId = stuffId;
      preparingSendingItem.StuffSerialCode = stuffSerialCode;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: preparingSendingItem,
                    transactionBatch: transactionBatch,
                    description: description);
      return preparingSendingItem;
    }
    #endregion
    #region Edit
    public PreparingSendingItem EditPreparingSendingItem(
        int id,
        byte[] rowVersion,
        TValue<int> preparingSendingId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<int> stuffId = null,
        TValue<long> stuffSerialCode = null,
        TValue<string> description = null)
    {
      var preparingSendingItem = GetPreparingSendingItem(id: id);
      return EditPreparingSendingItem(
                    preparingSendingItem: preparingSendingItem,
                    rowVersion: rowVersion,
                    preparingSendingId: preparingSendingId,
                    qty: qty,
                    unitId: unitId,
                    stuffId: stuffId,
                    stuffSerialCode: stuffSerialCode,
                    description: description);
    }
    public PreparingSendingItem EditPreparingSendingItem(
        PreparingSendingItem preparingSendingItem,
        byte[] rowVersion,
        TValue<int> preparingSendingId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<int> stuffId = null,
        TValue<long> stuffSerialCode = null,
        TValue<string> description = null)
    {
      if (preparingSendingId != null)
        preparingSendingItem.PreparingSendingId = preparingSendingId;
      if (qty != null)
        preparingSendingItem.Qty = qty;
      if (unitId != null)
        preparingSendingItem.UnitId = unitId;
      if (stuffId != null)
        preparingSendingItem.StuffId = stuffId;
      if (stuffSerialCode != null)
        preparingSendingItem.StuffSerialCode = stuffSerialCode;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: preparingSendingItem,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as PreparingSendingItem;
    }
    #endregion
    #region Get
    public PreparingSendingItem GetPreparingSendingItem(int id) => GetPreparingSendingItem(selector: e => e, id: id);
    public TResult GetPreparingSendingItem<TResult>(
        Expression<Func<PreparingSendingItem, TResult>> selector,
        int id)
    {
      var result = GetPreparingSendingItems(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new PreparingSendingItemNotFoundException(id);
      return result;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetPreparingSendingItems<TResult>(
        Expression<Func<PreparingSendingItem, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> preparingSendingId = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<int> stuffId = null,
        TValue<string> serial = null,
        TValue<long> stuffSerialCode = null,
        TValue<int?> exitReceiptId = null,
        TValue<int?> outboundCargoId = null,
        TValue<int[]> exitReceiptIds = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<DateTime> fromDateOrder = null,
        TValue<DateTime> toDateOrder = null,
        TValue<int> cooperatorId = null,
        TValue<int> exitReceiptRequestTypeId = null,
        TValue<int> sendPermissionId = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var preparingSendingItem = baseQuery.OfType<PreparingSendingItem>();
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial);
        preparingSendingItem = preparingSendingItem.Where(r => r.StuffSerial.Serial == serial);
      }
      if (preparingSendingId != null)
        preparingSendingItem = preparingSendingItem.Where(r => r.PreparingSendingId == preparingSendingId);
      if (fromDate != null)
        preparingSendingItem = preparingSendingItem.Where(r => r.PreparingSending.SendProduct.ExitReceipt.DateTime >= fromDate);
      if (toDate != null)
        preparingSendingItem = preparingSendingItem.Where(r => r.PreparingSending.SendProduct.ExitReceipt.DateTime <= toDate);
      if (cooperatorId != null)
        preparingSendingItem = preparingSendingItem.Where(r => r.PreparingSending.SendPermission.ExitReceiptRequest.CooperatorId == cooperatorId);
      if (qty != null)
        preparingSendingItem = preparingSendingItem.Where(r => r.Qty == qty);
      if (unitId != null)
        preparingSendingItem = preparingSendingItem.Where(r => r.UnitId == unitId);
      if (stuffId != null)
        preparingSendingItem = preparingSendingItem.Where(r => r.StuffId == stuffId);
      if (stuffSerialCode != null)
        preparingSendingItem = preparingSendingItem.Where(r => r.StuffSerialCode == stuffSerialCode);
      if (exitReceiptId != null)
        preparingSendingItem = preparingSendingItem.Where(r => r.PreparingSending.SendProduct.ExitReceiptId == exitReceiptId);
      if (outboundCargoId != null)
        preparingSendingItem = preparingSendingItem.Where(r => r.PreparingSending.SendProduct.ExitReceipt.OutboundCargoId == outboundCargoId);
      if (exitReceiptIds != null)
        preparingSendingItem = preparingSendingItem.Where(r => exitReceiptIds.Value.Contains(r.PreparingSending.SendProduct.ExitReceiptId));
      if (exitReceiptRequestTypeId != null)
        preparingSendingItem = preparingSendingItem.Where(r => r.PreparingSending.SendPermission.ExitReceiptRequest.ExitReceiptRequestTypeId == exitReceiptRequestTypeId);
      if (sendPermissionId != null)
        preparingSendingItem = preparingSendingItem.Where(r => r.PreparingSending.SendPermissionId == sendPermissionId);
      return preparingSendingItem.Select(selector);
    }
    #endregion
    #region Gets
    public IQueryable<PreparingSendingItemPrintResult> GetPreparingSendingItemPrintResults(int exitReceiptId)
    {
      return GetPreparingSendingItemPrintResults(new[] { exitReceiptId });
    }
    public IQueryable<PreparingSendingItemPrintResult> GetPreparingSendingItemPrintResults(int[] exitReceiptIds)
    {
      #region GetPreparingSendingItemPrintResultsWithIds
      var preparingSendingItems = GetPreparingSendingItems(
              selector: e => new
              {
                StuffId = e.StuffId,
                ExitReceiptRequestId = e.PreparingSending.SendPermission.ExitReceiptRequestId,
                ExitReceiptId = e.PreparingSending.SendProduct.ExitReceiptId,
                Qty = e.Qty * e.Unit.ConversionRatio
              },
             exitReceiptIds: exitReceiptIds,
             isDelete: false);
      #endregion
      #region GetGroupQuery                
      var groupQuery = from item in preparingSendingItems
                       group item by new
                       {
                         item.StuffId,
                         item.ExitReceiptId,
                         item.ExitReceiptRequestId,
                         item.Qty
                       }
          into gItems
                       select new
                       {
                         StuffId = gItems.Key.StuffId,
                         ExitReceiptRequestId = gItems.Key.ExitReceiptRequestId,
                         ExitReceiptId = gItems.Key.ExitReceiptId,
                         Qty = gItems.Key.Qty,
                         Count = gItems.Count(),
                         TotalQty = gItems.Sum(i => i.Qty)
                       };
      #endregion
      #region GetStuffs
      var stuffs = App.Internals.SaleManagement.GetStuffs(selector: e => e);
      #endregion
      #region Get ExitReceiptRequests
      var exitReceiptRequests = App.Internals.SaleManagement.GetExitReceiptRequests(selector: e => e);
      #endregion
      #region Get MainUnits
      var mainUnits = App.Internals.ApplicationBase.GetUnits(
          selector: e => new
          {
            e.UnitTypeId,
            e.Id,
            e.Name
          },
              isMainUnit: true,
              isActive: true);
      #endregion
      #region Get ExitReceipt
      var exitReceipts = App.Internals.WarehouseManagement.GetExitReceipts(
              selector: e => e,
              isDelete: false);
      #endregion
      #region Query
      var query = from item in groupQuery
                  join stuff in stuffs on item.StuffId equals stuff.Id
                  join unit in mainUnits on stuff.UnitTypeId equals unit.UnitTypeId
                  join exitReceiptRequest in exitReceiptRequests on item.ExitReceiptRequestId equals exitReceiptRequest.Id
                  join exitReceipt in exitReceipts on item.ExitReceiptId equals exitReceipt.Id
                  select new PreparingSendingItemPrintResult()
                  {
                    StuffId = item.StuffId,
                    StuffCode = stuff.Code,
                    StuffNoun = stuff.Noun,
                    StuffTitle = stuff.Title,
                    StuffName = stuff.Name,
                    Qty = item.Qty,
                    Count = item.Count,
                    TotalQty = item.TotalQty,
                    CooperatorId = exitReceiptRequest.CooperatorId,
                    CooperatorName = exitReceiptRequest.Cooperator.Name,
                    CooperatorAddress = exitReceiptRequest.Address,
                    ExitReceiptRequestId = exitReceiptRequest.Id,
                    ExitReceiptRequestCode = exitReceiptRequest.Code,
                    ExitReceiptRequestDescription = exitReceiptRequest.Description,
                    ExitReceiptRequestTypeTitle = exitReceiptRequest.ExitReceiptRequestType.Title,
                    UnitId = unit.Id,
                    UnitName = unit.Name,
                    ExitReceiptId = item.ExitReceiptId,
                    ExitReceiptDateTime = exitReceipt.DateTime,
                    OutboundCargoCode = exitReceipt.OutboundCargo.Code,
                    OutboundCargoCarNumber = exitReceipt.OutboundCargo.CarNumber,
                    OutboundCargoDriverName = exitReceipt.OutboundCargo.DriverName,
                    OutboundCargoCarInformation = exitReceipt.OutboundCargo.CarInformation,
                    OutboundCargoShippingCompanyName = exitReceipt.OutboundCargo.ShippingCompanyName,
                    OutboundCargoPhoneNumber = exitReceipt.OutboundCargo.PhoneNumber,
                    UserFullName = App.Providers.Security.CurrentLoginData.UserFullName
                  };
      #endregion
      return query;
    }
    public IQueryable<ExitReceiptLinkSerialsReportResult> GetExitReceiptLinkSerialsReport(int[] exitReceiptIds)
    {
      var preparingSendingItems = GetPreparingSendingItems(
                   selector: e => e,
                   exitReceiptIds: exitReceiptIds,
                   isDelete: false);
      var stuffSerials = preparingSendingItems.Select(psi => psi.StuffSerial);
      var exitReceiptLinkSerialsReportResults = new List<ExitReceiptLinkSerialsReportResult>();
      foreach (var stuffSerial in stuffSerials)
      {
        var serialDetails = GetSerialDetails(
                  selector: e => e,
                  productionSerial: stuffSerial.Serial,
                  stuffIds: null);
        foreach (var serialDetail in serialDetails)
        {
          if (serialDetail.StuffSerial.LinkSerial != null)
          {
            exitReceiptLinkSerialsReportResults.Add(new ExitReceiptLinkSerialsReportResult
            {
              LinkSerial = serialDetail.StuffSerial.LinkSerial.LinkedSerial,
              TechnicalNumber = serialDetail.StuffSerial.LinkSerial.IranKhodroSerial.CustomerStuff.TechnicalNumber
            });
          }
        }
      }
      return exitReceiptLinkSerialsReportResults.AsQueryable();
    }
    #endregion
    #region AddProcess
    //public PreparingSendingItem AddPreparingSendingItemProcess(
    //    SendPermission sendPermission,
    //    byte[] rowVersion,
    //    string description)
    //{
    //    
    //        #region AddTransactionBatch
    //        var warehouseManagement = App.Internals.WarehouseManagement;
    //        var transactionBatch = warehouseManagement.AddTransactionBatch()
    //            
    //;
    //        #endregion
    //        #region AddWarehouseTransactions For PareparingSendingItem
    //        var blockedTransactions = GetWarehouseTransactions(
    //                selector: ToWarehouseTransactionMinResult,
    //                transactionBatchId: sendPermission.OrderItemBlock.TransactionBatch.Id,
    //                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportBlock.Id)
    //            
    //;
    //        var blockedTransactionsList = blockedTransactions.ToList();
    //        foreach (var preparingSendingItemItem in preparingSendingItemItems)
    //        {
    //            #region GetUnit
    //            var unit = App.Internals.ApplicationBase.GetUnit(id: preparingSendingItemItem.UnitId)
    //                
    //;
    //            #endregion
    //            #region GetStuffSerial
    //            var stuffSerial = GetStuffSerial(serial: preparingSendingItemItem.Serial)
    //                
    //;
    //            #endregion
    //            var preparingSendingItemItemAmount = preparingSendingItemItem.Amount * unit.ConversionRatio;
    //            while (preparingSendingItemItemAmount > 0)
    //            {
    //                #region GetBlockedTransaction
    //                var blockedTransaction = blockedTransactionsList.FirstOrDefault(i => i.Serial == preparingSendingItemItem.Serial && i.Amount > 0);
    //                if (blockedTransaction == null)
    //                    blockedTransaction = blockedTransactionsList.FirstOrDefault(i => i.Serial == null && i.Amount > 0);
    //                if (blockedTransaction == null)
    //                    throw new MatchBlockedTransactionNotFoundException(preparingSendingItemItem.Serial);
    //                var referenceTransaction = GetBaseTransaction(id: blockedTransaction.Id)
    //                    
    //;
    //                #endregion
    //                var transactionAmount = Math.Min(preparingSendingItemItemAmount, blockedTransaction.Amount);
    //                #region ExportFromBlocked
    //                var exportFromBlockedTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
    //                    transactionBatchId: transactionBatch.Id,
    //                    effectDateTime: transactionBatch.DateTime,
    //                    stuffId: blockedTransaction.StuffId,
    //                    billOfMaterialVersion: blockedTransaction.BillOfMaterialVersion,
    //                    stuffSerialCode: blockedTransaction.StuffSerialCode,
    //                    warehouseId: blockedTransaction.WarehouseId,
    //                    transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportBlock.Id,
    //                    amount: transactionAmount / unit.ConversionRatio,
    //                    unitId: unit.Id,
    //                    description: null,
    //                    referenceTransaction: referenceTransaction,
    //                     false)
    //                
    //;
    //                #endregion
    //                #region ImportSerialBlock
    //                var importTransactionPlan = App.Internals.WarehouseManagement.AddWarehouseTransaction(
    //                        transactionBatchId: transactionBatch.Id,
    //                        effectDateTime: transactionBatch.DateTime,
    //                        stuffId: preparingSendingItemItem.StuffId,
    //                        billOfMaterialVersion: preparingSendingItemItem.Version,
    //                        stuffSerialCode: stuffSerial.Code,
    //                        warehouseId: preparingSendingItemItem.WarehouseId,
    //                        transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportBlock.Id,
    //                        amount: transactionAmount / unit.ConversionRatio,
    //                        unitId: unit.Id,
    //                        description: preparingSendingItemItem.Description,
    //                        referenceTransaction: exportFromBlockedTransaction,
    //                         false)
    //                    
    //;
    //                #endregion
    //                preparingSendingItemItemAmount = preparingSendingItemItemAmount - transactionAmount;
    //                blockedTransaction.Amount = blockedTransaction.Amount - transactionAmount;
    //            }
    //        }
    //        #region CheckTransactionBatch
    //        App.Internals.WarehouseManagement.CheckTransactionBatch()
    //            
    //;
    //        #endregion
    //        #endregion
    //        #region AddPreparingSendingItem
    //        var preparingSendingItem = AddPreparingSendingItem(
    //                preparingSendingItem: null,
    //                transactionBatch: transactionBatch,
    //                preparingSendingId: sendPermission.Id,
    //                description: description)
    //            
    //;
    //        #endregion
    //        #region GetProjectWorkItem
    //        var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
    //                baseEntityId: sendPermission.Id,
    //                scrumTaskType: ScrumTaskTypes.PreparingSendig)
    //            
    //;
    //        #endregion
    //        #region AddSendProductTask
    //        App.Internals.ProjectManagement.AddProjectWorkItem(
    //                projectWorkItem: null,
    //                name: $"ارسال محصول {preparingSendingItem.Code}",
    //                description: "کالاهای آماده شده را ارسال نمایید",
    //                color: "",
    //                departmentId: (int)Departments.Warehouse,
    //                estimatedTime: 10800,
    //                isCommit: false,
    //                scrumTaskTypeId: (int)ScrumTaskTypes.SendProduct,
    //                userId: null,
    //                spentTime: 0,
    //                remainedTime: 0,
    //                scrumTaskStep: ScrumTaskStep.ToDo,
    //                projectWorkId: projectWorkItem.ScrumBackLogId,
    //                baseEntityId: preparingSendingItem.Id)
    //            
    //;
    //        #endregion
    //        #region DoneTask
    //        App.Internals.ScrumManagement.DoneScrumTask(
    //                scrumTask: projectWorkItem,
    //                rowVersion: projectWorkItem.RowVersion)
    //            
    //;
    //        #endregion
    //        return preparingSendingItem;
    //    });
    //}
    //public PreparingSendingItem AddPreparingSendingItemProcess(
    //    int preparingSendingId,
    //    byte[] rowVersion,
    //    PreparingSendingItemItem[] preparingSendingItemItems,
    //    string description)
    //{
    //    
    //        var sendPermission = App.Internals.SaleManagement.GetSendPermission(id: preparingSendingId)
    //            
    //;
    //        var preparingSendingItem = AddPreparingSendingItemProcess(
    //            sendPermission: sendPermission,
    //            rowVersion: rowVersion,
    //            preparingSendingItemItems: preparingSendingItemItems,
    //            description: description)
    //            
    //;
    //        return preparingSendingItem;
    //    });
    //}
    #endregion
    #region Search
    public IQueryable<PreparingSendingItemResult> SearchPreparingSendingItemResult(
        IQueryable<PreparingSendingItemResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        GetPreparingSendingItemsInput input)
    {
      if (!string.IsNullOrEmpty(input.SearchText))
      {
        query = query.Where(x =>
            x.StuffCode.Contains(input.SearchText) ||
            x.ExitReceiptId.ToString().Contains(input.SearchText) ||
            x.StuffName.Contains(input.SearchText) ||
            x.Code.Contains(input.SearchText) ||
            x.SendProductCode.Contains(input.SearchText) ||
            x.UnitName.Contains(input.SearchText) ||
            x.CooperatorName.Contains(input.SearchText) ||
            x.OutboundCargoCode.Contains(input.SearchText));
      }
      if (input.FromDateOrder != null)
        query = query.Where(x => x.DateTimeOrderItem >= input.FromDateOrder);
      if (input.ToDateOrder != null)
        query = query.Where(x => x.DateTimeOrderItem <= input.ToDateOrder);
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<PreparingSendingItemResult> SortPreparingSendingItemResult(
        IQueryable<PreparingSendingItemResult> query,
        SortInput<PreparingSendingItemSortType> sort)
    {
      switch (sort.SortType)
      {
        case PreparingSendingItemSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case PreparingSendingItemSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case PreparingSendingItemSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case PreparingSendingItemSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case PreparingSendingItemSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case PreparingSendingItemSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case PreparingSendingItemSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case PreparingSendingItemSortType.SendProductCode:
          return query.OrderBy(a => a.SendProductCode, sort.SortOrder);
        case PreparingSendingItemSortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case PreparingSendingItemSortType.ExitReceiptRequestTypeTitle:
          return query.OrderBy(a => a.ExitReceiptRequestTypeTitle, sort.SortOrder);
        case PreparingSendingItemSortType.OutboundCargoCode:
          return query.OrderBy(a => a.OutboundCargoCode, sort.SortOrder);
        case PreparingSendingItemSortType.DateTimeOrderItem:
          return query.OrderBy(a => a.DateTimeOrderItem, sort.SortOrder);
        case PreparingSendingItemSortType.ExitReceiptId:
          return query.OrderBy(a => a.ExitReceiptId, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region GetStockForPreparingSendingItem
    //public StockResult GetStockForPreparingSendingItem(int preparingSendingId, string serial)
    //{
    //    
    //        var sendPermission = App.Internals.SaleManagement.GetSendPermission(
    //                id: preparingSendingId)
    //            
    //;
    //        var orderItemBlock = sendPermission.OrderItemBlock;
    //        var stuff = orderItemBlock.OrderItem.Stuff;
    //        var transactionLevel = TransactionLevel.Available;
    //        if (orderItemBlock is OrderItemSaleBlock)
    //            transactionLevel = TransactionLevel.Available;
    //        else if (orderItemBlock is OrderItemProductionBlock)
    //            transactionLevel = TransactionLevel.Blocked;
    //        StuffSerial stuffSerial = GetStuffSerial(serial: serial);
    //        var warehouseTransactions = GetWarehouseTransactions(
    //                    selector: e => e,
    //                    warehouseId: orderItemBlock.WarehouseId,
    //                    transactionLevel: transactionLevel,
    //                    stuffId: stuff.Id,
    //                    stuffSerialCode: stuffSerial.Code)
    //                
    //;
    //        var unit = stuff.UnitType.Units.SingleOrDefault(i => i.IsMainUnit);
    //        var sum = warehouseTransactions.Sum(i => (double?)((int)i.TransactionType.Factor * i.Amount * i.Unit.ConversionRatio)) ?? 0;
    //        return new StockResult() { Amount = sum, UnitId = unit.Id, UnitName = unit.Name, UnitConversionRatio = unit.ConversionRatio };
    //    });
    //}
    #endregion
    #region ToPreparingSendingItemResult
    public IQueryable<PreparingSendingItemResult> ToFullPreparingSendingItemResult(
        IQueryable<PreparingSendingItem> preparingSendingItems,
        IQueryable<OrderItemBlock> OrderItemBlocks,
        IQueryable<OrderItem> OrderItems)
    {
      var resultQuery = from preparingSendingItem in preparingSendingItems
                        join orderItemBlock in OrderItemBlocks on
                              preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequestId equals orderItemBlock.Id into oItemBlocks
                        from oItemBlock in oItemBlocks.DefaultIfEmpty()
                        join orderItem in OrderItems on
                              oItemBlock.OrderItemId equals orderItem.Id into oItems
                        from oItem in oItems.DefaultIfEmpty()
                        select new PreparingSendingItemResult()
                        {
                          Id = preparingSendingItem.Id,
                          Code = preparingSendingItem.Code,
                          PreparingSendingId = preparingSendingItem.PreparingSendingId,
                          Qty = preparingSendingItem.Qty,
                          UnitId = preparingSendingItem.UnitId,
                          UnitName = preparingSendingItem.Unit.Name,
                          DateTime = preparingSendingItem.PreparingSending.SendProduct.ExitReceipt.DateTime,
                          StuffId = preparingSendingItem.StuffId,
                          StuffCode = preparingSendingItem.Stuff.Code,
                          StuffName = preparingSendingItem.Stuff.Name,
                          StuffSerialCode = preparingSendingItem.StuffSerialCode,
                          Serial = preparingSendingItem.StuffSerial.Serial,
                          SendProductId = preparingSendingItem.PreparingSending.SendProduct.Id,
                          SendProductCode = preparingSendingItem.PreparingSending.SendProduct.Code,
                          CooperatorId = preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequest.CooperatorId,
                          CooperatorName = preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequest.Cooperator.Name,
                          CooperatorCode = preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequest.Cooperator.Code,
                          OutboundCargoId = preparingSendingItem.PreparingSending.SendProduct.ExitReceipt.OutboundCargoId,
                          OutboundCargoCode = preparingSendingItem.PreparingSending.SendProduct.ExitReceipt.OutboundCargo.Code,
                          DateTimeOrderItem = oItem.DateTime,
                          QtyOrderItem = oItem.Qty,
                          ExitReceiptRequestTypeTitle = preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequest.ExitReceiptRequestType.Title,
                          ExitReceiptId = preparingSendingItem.PreparingSending.SendProduct.ExitReceiptId,
                          RowVersion = preparingSendingItem.RowVersion
                        };
      return resultQuery;
    }
    #endregion
    #region RemovePreparingSending
    public void RemovePreparingSending(
        int preparingSendingId,
        byte[] rowVersion)
    {
      var preparingSending = App.Internals.WarehouseManagement.GetPreparingSending(
                    id: preparingSendingId);
      var sendProduct = App.Internals.WarehouseManagement.GetSendProducts(
                    selector: e => e,
                    isDelete: false,
                    preparingSendingId: preparingSending.Id);
      if (sendProduct.Any())
      {
        throw new SendProductExistsException();
      }
      #region AddTransactionBatch
      var newTransactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
              transactionBatchId: newTransactionBatch.Id,
              baseEntity: preparingSending,
              rowVersion: rowVersion);
      App.Internals.SaleManagement.ResetSendPermissionStatus(
                id: preparingSending.SendPermissionId);
      return;
    }
    #endregion
  }
}
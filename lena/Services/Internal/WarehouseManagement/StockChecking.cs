using System;
using System.Linq;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Stuff;
using lena.Models.WarehouseManagement.StockChecking;
using lena.Models.WarehouseManagement.StuffSerial;
using lena.Models.WarehouseManagement.Warehouse;
using lena.Models.WarehouseManagement.WarehouseTransaction;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
using lena.Services.Core.Foundation;

namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    internal StockChecking AddStockChecking(
        string title,
        bool showInventory,
        DateTime startDate,
        DateTime endDate)
    {
      var currentUser = App.Providers.Security.CurrentLoginData;
      var stockChecking = repository.Create<StockChecking>();
      stockChecking.Title = title;
      stockChecking.CreateDate = DateTime.Now.ToUniversalTime();
      stockChecking.StartDate = startDate;
      stockChecking.EndDate = endDate;
      stockChecking.UserId = currentUser.UserId;
      stockChecking.Status = StockCheckingStatus.NotStarted;
      stockChecking.ShowInventory = showInventory;
      repository.Add(stockChecking);
      return stockChecking;
    }
    #endregion
    #region Edit
    internal StockChecking EditStockChecking(
        int id,
        byte[] rowVersion,
        TValue<bool> showInventory = null,
        TValue<string> title = null,
        TValue<DateTime> startDate = null,
        TValue<DateTime?> endDate = null,
        TValue<StockCheckingStatus> status = null)
    {
      var stockChecking = GetStockChecking(id: id);
      var result = EditStockChecking(
                stockChecking: stockChecking,
                rowVersion: rowVersion,
                showInventory: showInventory,
                title: title,
                startDate: startDate,
                endDate: endDate,
                status: status);
      return result;
    }
    internal StockChecking EditStockChecking(
        StockChecking stockChecking,
        byte[] rowVersion,
        TValue<bool> showInventory = null,
        TValue<string> title = null,
        TValue<DateTime> startDate = null,
        TValue<DateTime?> endDate = null,
        TValue<StockCheckingStatus> status = null)
    {
      if (title != null)
        stockChecking.Title = title;
      if (startDate != null)
        stockChecking.StartDate = startDate;
      if (endDate != null)
        stockChecking.EndDate = endDate;
      if (status != null)
        stockChecking.Status = status;
      if (showInventory != null)
        stockChecking.ShowInventory = showInventory;
      if (status == StockCheckingStatus.Finished)
        stockChecking.ActiveTagTypeId = null;
      repository.Update(stockChecking, rowVersion);
      return stockChecking;
    }
    internal StockChecking EditStockCheckingActiveTagType(
        int id,
        byte[] rowVersion,
        TValue<int> activeTagTypeId = null)
    {
      var stockChecking = GetStockChecking(id: id);
      if (activeTagTypeId != null)
        stockChecking.ActiveTagTypeId = activeTagTypeId;
      repository.Update(stockChecking, rowVersion);
      return stockChecking;
    }
    #endregion
    #region Start
    internal void StartStockChecking(byte[] rowVersion, int id)
    {
      EditStockChecking(rowVersion: rowVersion, id: id, status: StockCheckingStatus.Started);
    }
    #endregion
    #region Stop
    internal void StopStockChecking(byte[] rowVersion, int id)
    {
      var stockChecking = EditStockChecking(
                    rowVersion: rowVersion,
                    id: id,
                    status: StockCheckingStatus.Stoped);
    }
    #endregion
    #region Finish
    internal void FinishStockChecking(byte[] rowVersion, int id)
    {
      EditStockChecking(rowVersion: rowVersion, id: id, status: StockCheckingStatus.Finished);
    }
    #endregion
    #region Delete
    internal void DeleteStockChecking(int id)
    {
      var sc = GetStockChecking(id);
      var warehouse = sc.StockCheckingWarehouses.Select(a => new
      {
        a.StockCheckingId,
        a.WarehouseId,
        HasTag = a.StockCheckingTags?.Select(b => b.StockCheckingId).Any() ?? false
      }).ToList();
      foreach (var w in warehouse)
      {
        if (w.HasTag)
          throw new StockCheckingWarehouseHasTagExecption(w.StockCheckingId);
        DeleteStockCheckingWarehouse(w.StockCheckingId, w.WarehouseId);
      }
      while (sc.StockCheckingPersons.Count > 0)
      {
        DeleteStockCheckingPerson(sc.Id, sc.StockCheckingPersons.ElementAt(0).UserId);
      }
      repository.Delete(sc);
    }
    #endregion
    #region Gets
    internal IQueryable<StockChecking> GetStockCheckings(
        TValue<int> id = null,
        TValue<bool> showInventory = null,
        TValue<string> title = null,
        TValue<int> userId = null,
        TValue<DateTime> creationDate = null,
        TValue<DateTime?> startDate = null,
        TValue<DateTime?> endDate = null,
        TValue<StockCheckingStatus> status = null)
    {
      var isIdNUll = id == null;
      var isTitleNUll = title == null;
      var isUserIdNull = userId == null;
      var isCreateDateNull = creationDate == null;
      var isStartDate = startDate == null;
      var isEndDateNUll = endDate == null;
      var isStatusNull = status == null;
      var isShowInventoryNull = showInventory == null;
      var result = from a in repository.GetQuery<StockChecking>()
                   where (id == a.Id || isIdNUll) &&
                               (title == a.Title || isTitleNUll) &&
                               (userId == a.UserId || isUserIdNull) &&
                               (creationDate == a.CreateDate || isCreateDateNull) &&
                               (startDate == a.StartDate || isStartDate) &&
                               (endDate == a.EndDate || isEndDateNUll) &&
                               (showInventory == a.ShowInventory || isShowInventoryNull) &&
                               (status == a.Status || isStatusNull)
                   select a;
      return result;
    }
    #endregion
    #region Get
    internal StockChecking GetStockChecking(int id)
    {
      var data = GetStockCheckings(id: id).FirstOrDefault();
      if (data == null)
        throw new RecordNotFoundException(id, typeof(StockChecking));
      return data;
    }
    #endregion
    #region ToResult
    internal StockCheckingResult ToStockCheckingResult(StockChecking input)
    {
      var employee = input.User.Employee;
      return new StockCheckingResult
      {
        Id = input.Id,
        ActiveTagTypeId = input.ActiveTagTypeId,
        ActiveTagTypeName = input.ActiveTagType?.Name,
        Title = input.Title,
        CreateDate = input.CreateDate,
        StartDate = input.StartDate,
        EndDate = input.EndDate,
        CreatorName = (employee == null) ? "" : employee.FirstName + " " + employee.LastName,
        Status = input.Status,
        CreatorUserId = input.UserId,
        ShowInventory = input.ShowInventory,
        RowVersion = input.RowVersion,
        RelatedPersons = from scp in input.StockCheckingPersons
                         let emp = scp.User.Employee
                         select new EmployeeComboResult()
                         {
                           Id = emp.Id,
                           UserId = scp.UserId,
                           FirstName = emp.FirstName,
                           LastName = emp.LastName,
                           EmployeeCode = emp.Code
                         },
        Warehouses = from scw in input.StockCheckingWarehouses
                     let warehouse = scw.Warehouse
                     select new WarehouseComboResult()
                     {
                       Id = warehouse.Id,
                       Name = warehouse.Name,
                       DisplayOrder = warehouse.DisplayOrder
                     }
                       ,
        RelatedStuffs = from scs in input.StockCheckingStuffs
                        let stuff = scs.Stuff
                        select new StuffComboResult()
                        {
                          Id = stuff.Id,
                          Name = stuff.Name,
                        }
      };
    }
    internal IQueryable<StockCheckingResult> ToStockCheckingResultQuery(
        IQueryable<StockChecking> stockCheckingQuery)
    {
      var data = from sc in stockCheckingQuery
                 let employee = sc.User.Employee
                 select new StockCheckingResult
                 {
                   Id = sc.Id,
                   CreatorUserId = sc.UserId,
                   Title = sc.Title,
                   CreateDate = sc.CreateDate,
                   CreatorName = employee.FirstName + " " + employee.LastName,
                   EndDate = sc.EndDate,
                   ShowInventory = sc.ShowInventory,
                   ActiveTagTypeId = sc.ActiveTagTypeId,
                   ActiveTagTypeName = sc.ActiveTagType.Name,
                   RowVersion = sc.RowVersion,
                   RelatedPersons = from scp in sc.StockCheckingPersons
                                    let sce = scp.User.Employee
                                    select new EmployeeComboResult()
                                    {
                                      Id = (sce == null) ? 0 : sce.Id,
                                      UserId = scp.UserId,
                                      FirstName = (sce == null) ? "" : sce.FirstName,
                                      LastName = (sce == null) ? scp.User.UserName : sce.LastName,
                                      EmployeeCode = (sce == null) ? "0000" : sce.Code,
                                    },
                   Warehouses = from scw in sc.StockCheckingWarehouses
                                select new WarehouseComboResult()
                                {
                                  Id = scw.WarehouseId,
                                  Name = scw.Warehouse.Name,
                                  DisplayOrder = scw.Warehouse.DisplayOrder
                                },
                   StartDate = sc.StartDate,
                   Status = sc.Status
                 };
      return data;
    }
    #endregion
    #region Search
    public IQueryable<StockCheckingResult> SearchStockCheckingResults(
        IQueryable<StockCheckingResult> query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from pTerminal in query
                where
                      pTerminal.Title.Contains(searchText) ||
                      pTerminal.CreatorName.Contains(searchText)
                select pTerminal;
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    internal IOrderedQueryable<StockCheckingResult> SortStockCheckingResult(IQueryable<StockCheckingResult> input, SortInput<StockCheckingSortType> options)
    {
      switch (options.SortType)
      {
        case StockCheckingSortType.Id:
          return input.OrderBy(a => a.Id, options.SortOrder);
        case StockCheckingSortType.Title:
          return input.OrderBy(a => a.Title, options.SortOrder);
        case StockCheckingSortType.StartDate:
          return input.OrderBy(a => a.StartDate, options.SortOrder);
        case StockCheckingSortType.EndDate:
          return input.OrderBy(a => a.EndDate, options.SortOrder);
        case StockCheckingSortType.Status:
          return input.OrderBy(a => a.Status, options.SortOrder);
        case StockCheckingSortType.CreatorName:
          return input.OrderBy(a => a.CreatorName, options.SortOrder);
        case StockCheckingSortType.CreateDate:
          return input.OrderBy(a => a.CreateDate, options.SortOrder);
        case StockCheckingSortType.ShowInventory:
          return input.OrderBy(a => a.ShowInventory, options.SortOrder);
        case StockCheckingSortType.ActiveTagTypeName:
          return input.OrderBy(a => a.ActiveTagTypeName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region AddProcess
    internal StockChecking AddStockCheckingProcess(
        string title,
        DateTime startDate,
        DateTime endDate,
        bool showInventory,
        int[] users,
        short[] warehouses,
        int[] stuffs)
    {
      // create new stock checking
      var sc = AddStockChecking(title: title, showInventory: showInventory, startDate: startDate, endDate: endDate);
      //adding warehouses
      foreach (var warehouseId in warehouses)
        AddStockCheckingWarehouse(stockCheckingId: sc.Id, warehouseId: warehouseId);
      //adding involved users
      foreach (var userId in users)
        AddStockCheckingPerson(stockCheckingId: sc.Id, userId: userId);
      //adding involved stuffs 
      if (stuffs.Any())
      {
        foreach (var stuffId in stuffs)
        {
          AddStockCheckingStuff(stockCheckingId: sc.Id, stuffId: stuffId);
        }
      }
      return sc;
    }
    #endregion
    #region EditProcess
    internal StockChecking EditStockCheckingProcess(
        byte[] rowVersion,
        int id,
        string title,
        bool showInventory,
        DateTime startDate,
        DateTime endDate,
        int[] addedUsers,
        int[] deletedUsers,
        short[] addedWarehouses,
        short[] deletedWarehouses,
        int[] addedStuffs,
        int[] deletedStuffs)
    {
      var ee = EditStockChecking(
                rowVersion: rowVersion,
                id: id,
                showInventory: showInventory,
                title: title,
                startDate: startDate,
                endDate: endDate);
      var usyn = new SyncArray<int>
      {
        Added = addedUsers,
        Removed = deletedUsers
      };
      usyn.Sync(
                a => AddStockCheckingPerson(stockCheckingId: id, userId: a),
                a => DeleteStockCheckingPerson(stockCheckingId: id, userId: a)
            );
      var wsyn = new SyncArray<short>
      {
        Added = addedWarehouses,
        Removed = deletedWarehouses
      };
      wsyn.Sync(a => AddStockCheckingWarehouse(stockCheckingId: id, warehouseId: a),
                a => DeleteStockCheckingWarehouse(stockCheckingId: id, warehouseId: a)
            );
      var susyn = new SyncArray<int>
      {
        Added = addedStuffs,
        Removed = deletedStuffs
      };
      susyn.Sync(                
                a => AddStockCheckingStuff(stockCheckingId: id, stuffId: a),
                a => DeleteStockCheckingStuff(stockCheckingId: id, stuffId: a)
            );
      return ee;
    }
    #endregion
    #region CorrectWarehouseInventoriesProcess
    internal void CorrectWarehouseInventoriesProcess(
        int stockCheckingId,
        int tagTypeId,
        short warehouseId,
        GetStuffSerialInput[] serialInputs)
    {
      #region Check StockCheckingStatus
      var stockChecking = GetStockChecking(id: stockCheckingId);
      if (stockChecking.Status != StockCheckingStatus.Finished)
        throw new StockCheckingNotFinishedExecption(stockCheckingId: stockCheckingId);
      #endregion
      #region Get Selected Serials Info
      var stockTakingVariances = GetStockTakingVariances(
              stockCheckingId: stockCheckingId,
              warehouseId: warehouseId,
              tagTypeId: tagTypeId,
              groupBySerial: true);
      var serials = serialInputs.Select(i => i.Serial).ToArray();
      stockTakingVariances =
                SearchStockTakingVarianceResult(query: stockTakingVariances, serials: serials);
      #endregion
      #region AddTransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      foreach (var stockTakingVariance in stockTakingVariances)
      {
        // اگر موجودی صحیح است، هیچ کاری نکن
        if (stockTakingVariance.TagAmount == stockTakingVariance.StockSerialAmount) continue;
        var stockCheckingTagId = stockTakingVariance.Id;
        string description = $"اصلاح موجودی انبارگردانی شناسه {stockCheckingId}";
        #region اگر قبلا برای این شمارش، اصلاح موجودی اعمال شده باشد
        var qtyCorrectionRequest = GetQtyCorrectionRequests(
            selector: e => e,
            stockCheckingTagId: stockCheckingTagId ?? 0,
            status: QtyCorrectionRequestStatus.Accepted)
        .FirstOrDefault();
        if (qtyCorrectionRequest != null)
          throw new StockCheckingTagHasAcceptedQtyCorrectionRequestException(
                    stockCheckingTagId: stockCheckingTagId ?? 0,
                    serial: stockTakingVariance.Serial);
        #endregion
        #region  اگر سریال دارای درخواست اصلاح موجودی اقدام نشده بود، آن را رد کن
        var noActionQtyCorrectionRequests = GetQtyCorrectionRequests(
                selector: e => e,
                stuffSerialCode: stockTakingVariance.StuffSerialCode,
                stuffId: stockTakingVariance.StuffId,
                status: QtyCorrectionRequestStatus.NotAction);
        foreach (var noActionQtyCorrectionRequest in noActionQtyCorrectionRequests)
        {
          RejectQtyCorrectionRequestProcess(
                    qtyCorrectionRequest: noActionQtyCorrectionRequest,
                    rowVersion: noActionQtyCorrectionRequest.RowVersion,
                    description: description);
        }
        #endregion
        var currentWarehouseInventory = GetWarehouseInventories(
            groupBySerial: true,
            serial: stockTakingVariance.Serial)
        .FirstOrDefault();
        #region اگر سریال در انبار دیگری بود، به انباری حواله کن که در آن شمارش شده است
        if (currentWarehouseInventory != null && currentWarehouseInventory.WarehouseId != warehouseId)
        {
          var addWarehouseIssueItems = new AddWarehouseIssueItemInput[]
                {
                            new AddWarehouseIssueItemInput
                            {
                                Amount = currentWarehouseInventory.TotalAmount ?? 0,
                                UnitId = currentWarehouseInventory.UnitId,
                                Serial = currentWarehouseInventory.Serial,
                                StuffId = currentWarehouseInventory.StuffId,
                                Description = description
                            }
                };
          AddWarehouseIssueProcess(
                    transactionBatch: transactionBatch,
                    fromWarehouseId: currentWarehouseInventory.WarehouseId,
                    toWarehouseId: warehouseId,
                    addWarehouseIssueItems: addWarehouseIssueItems,
                    toDepartmentId: null,
                    toEmployeeId: null,
                    description: description);
        }
        #endregion
        QtyCorrectionRequestType qtyCorrectionRequestType;
        if (stockTakingVariance.TagAmount < stockTakingVariance.StockSerialAmount) // اگر موجودی واقعی کمتر از موجودی سیستمی است
          qtyCorrectionRequestType = QtyCorrectionRequestType.DecreaseStockChecking;
        else // اگر موجودی واقعی بیشتر از موجودی سیستمی است
          qtyCorrectionRequestType = QtyCorrectionRequestType.IncreaseStockChecking;
        if (stockCheckingTagId == null)
        {
          var tagCounting = AddSerialTagCountingProcess(
                    stockCheckingId: stockCheckingId,
                    warehouseId: warehouseId,
                    serial: stockTakingVariance.Serial,
                    amount: 0,
                    unitId: stockTakingVariance.UnitId,
                    replaceIfExist: true);
          stockCheckingTagId = tagCounting.StockCheckingTagId;
        }
        #region اگر سریال، بلوکه شده است، حواله در انتظار آن سریال را رد کن
        if (currentWarehouseInventory != null && currentWarehouseInventory.BlockedAmount > 0.00000001)
        {
          var waitingWarehouseIssues = GetWarehouseIssues(
                        selector: e => e,
                        serial: currentWarehouseInventory.Serial,
                        status: WarehouseIssueStatusType.Waiting);
          foreach (var waitingWarehouseIssue in waitingWarehouseIssues)
          {
            RejectWarehouseIssueProcess(
                      transactionBatch: transactionBatch,
                      id: waitingWarehouseIssue.Id,
                      rowVersion: waitingWarehouseIssue.RowVersion,
                      fromWarehouseId: waitingWarehouseIssue.FromWarehouseId,
                      toWarehouseId: waitingWarehouseIssue.ToWarehouseId,
                      description: description);
          }
        }
        #endregion
        // درخواست اصلاح موجودی ثبت کن
        var addedRequest = AddQtyCorrectionRequest(
            qtyCorrectionRequest: null,
            transactionBatch: transactionBatch,
            warehouseId: warehouseId,
            qty: stockTakingVariance.ContradictionAmount,
            stuffId: stockTakingVariance.StuffId,
            serial: stockTakingVariance.Serial,
            type: qtyCorrectionRequestType,
            unitId: stockTakingVariance.UnitId,
            description: description,
            stockCheckingTagId: stockCheckingTagId);
        // درخواست را تایید کن
        AcceptQtyCorrectionRequestProcess(
            qtyCorrectionRequest: addedRequest,
            rowVersion: addedRequest.RowVersion,
            description: description);
      }
    }
    internal void CorrectSerialWarehouseInventoryProcess(
       int stockCheckingId,
       int tagTypeId,
       short warehouseId,
       int? stockCheckingTagId,
       double tagAmount,
       double stockSerialAmount,
       double contradictionAmount,
       byte unitId,
       string serial,
       long? stuffSerialCode,
       int stuffSerialStuffId)
    {
      #region Check StockCheckingStatus
      var stockChecking = GetStockChecking(id: stockCheckingId);
      if (stockChecking.Status != StockCheckingStatus.Finished)
        throw new StockCheckingNotFinishedExecption(stockCheckingId: stockCheckingId);
      #endregion
      #region AddTransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      // اگر موجودی صحیح است، هیچ کاری نکن
      if (tagAmount == stockSerialAmount) return;
      string description = $"اصلاح موجودی انبارگردانی شناسه {stockCheckingId}";
      #region اگر قبلا برای این شمارش، اصلاح موجودی اعمال شده باشد
      var qtyCorrectionRequest = GetQtyCorrectionRequests(
          selector: e => e,
          stockCheckingTagId: stockCheckingTagId ?? 0,
          status: QtyCorrectionRequestStatus.Accepted)
      .FirstOrDefault();
      if (qtyCorrectionRequest != null)
        throw new StockCheckingTagHasAcceptedQtyCorrectionRequestException(
                  stockCheckingTagId: stockCheckingTagId ?? 0,
                  serial: serial);
      #endregion
      #region  اگر سریال دارای درخواست اصلاح موجودی اقدام نشده بود، آن را رد کن
      var noActionQtyCorrectionRequests = GetQtyCorrectionRequests(
              selector: e => e,
              stuffSerialCode: stuffSerialCode,
              stuffId: stuffSerialStuffId,
              status: QtyCorrectionRequestStatus.NotAction);
      foreach (var noActionQtyCorrectionRequest in noActionQtyCorrectionRequests)
      {
        RejectQtyCorrectionRequestProcess(
                  qtyCorrectionRequest: noActionQtyCorrectionRequest,
                  rowVersion: noActionQtyCorrectionRequest.RowVersion,
                  description: description);
      }
      #endregion
      var currentWarehouseInventory = GetWarehouseInventories(
          groupBySerial: true,
          serial: serial)
      .FirstOrDefault();
      #region اگر سریال در انبار دیگری بود، به انباری حواله کن که در آن شمارش شده است
      if (currentWarehouseInventory != null && currentWarehouseInventory.WarehouseId != warehouseId)
      {
        var addWarehouseIssueItems = new AddWarehouseIssueItemInput[]
              {
                            new AddWarehouseIssueItemInput
                            {
                                Amount = currentWarehouseInventory.TotalAmount ?? 0,
                                UnitId = currentWarehouseInventory.UnitId,
                                Serial = currentWarehouseInventory.Serial,
                                StuffId = currentWarehouseInventory.StuffId,
                                Description = description
                            }
            };
        AddWarehouseIssueProcess(
                  transactionBatch: transactionBatch,
                  fromWarehouseId: currentWarehouseInventory.WarehouseId,
                  toWarehouseId: warehouseId,
                  addWarehouseIssueItems: addWarehouseIssueItems,
                  toDepartmentId: null,
                  toEmployeeId: null,
                  description: description);
      }
      #endregion
      QtyCorrectionRequestType qtyCorrectionRequestType;
      if (tagAmount < stockSerialAmount) // اگر موجودی واقعی کمتر از موجودی سیستمی است
        qtyCorrectionRequestType = QtyCorrectionRequestType.DecreaseStockChecking;
      else // اگر موجودی واقعی بیشتر از موجودی سیستمی است
        qtyCorrectionRequestType = QtyCorrectionRequestType.IncreaseStockChecking;
      if (stockCheckingTagId == null)
      {
        var tagCounting = AddSerialTagCountingProcess(
                  stockCheckingId: stockCheckingId,
                  warehouseId: warehouseId,
                  serial: serial,
                  amount: 0,
                  unitId: unitId,
                  replaceIfExist: true);
        stockCheckingTagId = tagCounting.StockCheckingTagId;
      }
      #region اگر سریال، بلوکه شده است، حواله در انتظار آن سریال را رد کن
      if (currentWarehouseInventory != null && currentWarehouseInventory.BlockedAmount > 0.00000001)
      {
        var waitingWarehouseIssues = GetWarehouseIssues(
                      selector: e => e,
                      serial: currentWarehouseInventory.Serial,
                      status: WarehouseIssueStatusType.Waiting);
        foreach (var waitingWarehouseIssue in waitingWarehouseIssues)
        {
          RejectWarehouseIssueProcess(
                    transactionBatch: transactionBatch,
                    id: waitingWarehouseIssue.Id,
                    rowVersion: waitingWarehouseIssue.RowVersion,
                    fromWarehouseId: waitingWarehouseIssue.FromWarehouseId,
                    toWarehouseId: waitingWarehouseIssue.ToWarehouseId,
                    description: description);
        }
      }
      #endregion
      // درخواست اصلاح موجودی ثبت کن
      var addedRequest = AddQtyCorrectionRequest(
          qtyCorrectionRequest: null,
          transactionBatch: transactionBatch,
          warehouseId: warehouseId,
          qty: contradictionAmount,
          stuffId: stuffSerialStuffId,
          serial: serial,
          type: qtyCorrectionRequestType,
          unitId: unitId,
          description: description,
          stockCheckingTagId: stockCheckingTagId);
      // درخواست را تایید کن
      AcceptQtyCorrectionRequestProcess(
          qtyCorrectionRequest: addedRequest,
          rowVersion: addedRequest.RowVersion,
          description: description);
    }
    #endregion
  }
}
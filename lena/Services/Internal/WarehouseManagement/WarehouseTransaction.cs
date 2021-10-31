using System;
using System.Collections.Generic;
using System.Data.Common;
//using System.Data.Entity;
//using System.Data.Entity.Core.Objects;
//using System.Data.Entity.Infrastructure;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
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
using lena.Models.WarehouseManagement.BaseTransaction;
using lena.Models.WarehouseManagement.Cardex;
using lena.Models.WarehouseManagement.TransactionSummary;
using lena.Models.WarehouseManagement.WarehouseInventory;
using lena.Models.WarehouseManagement.WarehouseTransaction;
using lena.Models.WarehouseManagement.WarehouseIssue;
using lena.Services.Internals.WarehouseManagement;
using lena.Models.WarehouseManagement.WarehouseReports;
using lena.Models.ApplicationBase.CurrencyRate;
using System.Xml.Linq;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Data;
using System.Text;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Addgetwarehouse
    internal BaseTransaction AddWarehouseTransaction(
        int transactionBatchId,
        DateTime effectDateTime,
        int stuffId,
        short? billOfMaterialVersion,
        long? stuffSerialCode,
        short warehouseId,
        short transactionTypeId,
        double amount,
        byte unitId,
        string description,
        BaseTransaction referenceTransaction,
        short? warehouseFiscalPeriodId = null,
        bool checkWarehouseStatuses = true,
        bool checkFIFO = false)
    {

      #region Check has QtyCorrectionRequest
      if (stuffSerialCode != null)
      {
        var qtyCorrectionRequests = GetQtyCorrectionRequests(
                  selector: e => e,
                  stuffSerialCode: stuffSerialCode,
                  stuffId: stuffId,
                  status: QtyCorrectionRequestStatus.NotAction);
        if (qtyCorrectionRequests.Any())
        {
          var getStuff = App.Internals.SaleManagement.GetStuff(id: stuffId);
          var serial = "-";
          if (stuffSerialCode != null)
          {
            var stuffSerials = GetStuffSerials(selector: e => e,
                      code: stuffSerialCode,
                      stuffId: stuffId);
            if (stuffSerials.Any())
            {
              serial = stuffSerials.FirstOrDefault().Serial;
            }
          }
          throw new HasQtyCorrectionRequestException(code: getStuff.Code, serial: serial);
        }
      }
      #endregion#region CheckWarehouseStatus
      var warehouse = GetWarehouse(
              selector: e => e,
              id: warehouseId);
      if (checkWarehouseStatuses)
      {
        if (warehouse.IsDeleted)
          throw new WarehouseIsDeletedException(id: warehouseId);
        if (!warehouse.IsActive)
          throw new WarehouseNotActiveException(id: warehouseId);
      }
      //var ActiveStockChecking = GetStockCheckings(status: StockCheckingStatus.Started);
      var result = GetStockCheckingWarehouses(e => e, warehouseId: warehouse.Id, status: StockCheckingStatus.Started);
      //var result = from stockchecking in ActiveStockChecking
      //             from stockCheckingWareHouse in stockchecking.StockCheckingWarehouses.DefaultIfEmpty()
      //             where stockCheckingWareHouse.WarehouseId == warehouse.Id
      //             select stockCheckingWareHouse;
      foreach (var activStocks in result)
      {
        var stuffs = GetStockCheckingStuffs(
                      selector: e => e,
                      stockCheckingId: activStocks.StockCheckingId);
        if (!stuffs.Any())
        {
          throw new WarehouseHasStartedStockCheckingException(id: warehouseId);
        }
        else
        {
          if (stuffs.Any(i => i.StuffId == stuffId))
          {
            throw new WarehouseHasStartedStockCheckingException(id: warehouseId);
          }
        }
      }
      #endregion
      #region Check BillOfMaterialVersion
      var stuff = App.Internals.SaleManagement.GetStuff(id: stuffId);
      if (stuff.StuffType == StuffType.Product || stuff.StuffType == StuffType.SemiProduct || stuff.StuffType == StuffType.ProductPack)
      {
        if (stuffSerialCode != null && billOfMaterialVersion == null)
        {
          throw new BillOfMaterialVersionIsEmptyException(stuffId);
        }
      }
      else
      {
        if (billOfMaterialVersion != null)
        {
          throw new BillOfMaterialVersionIsNotEmptyException(stuffId);
        }
      }
      #endregion
      #region GetCurrentWarehouseFiscalPeriod
      if (warehouseFiscalPeriodId == null)
      {
        var currentWarehouseFiscalPeriodIds = GetWarehouseFiscalPeriods(
                  selector: e => e.Id,
                  isCurrent: true);
        if (!currentWarehouseFiscalPeriodIds.Any())
          throw new CurrentWarehouseFiscalPeriodIsNotDefinedException();
        if (currentWarehouseFiscalPeriodIds.Count() > 1)
          throw new CurrentWarehouseFiscalPeriodCanNotBeMoreThanOneException();
        warehouseFiscalPeriodId = currentWarehouseFiscalPeriodIds.FirstOrDefault();
      }
      #endregion
      #region WarehouseFiscalPeriod can not be closed
      var warehouseFiscalPeriod = GetWarehouseFiscalPeriod(id: warehouseFiscalPeriodId.Value);
      if (warehouseFiscalPeriod.IsClosed)
        throw new WarehouseFiscalPeriodIsClosedException(id: warehouseFiscalPeriodId.Value);
      #endregion
      #region AddTransaction
      var warehouseTransaction = repository.Create<BaseTransaction>();
      warehouseTransaction.WarehouseId = warehouseId;
      warehouseTransaction.WarehouseFiscalPeriodId = (short)warehouseFiscalPeriodId;
      AddBaseTransaction(
                    baseTransaction: warehouseTransaction,
                    amount: amount,
                    effectDateTime: effectDateTime,
                    description: description,
                    stuffId: stuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    stuffSerialCode: stuffSerialCode,
                    unitId: unitId,
                    transactionTypeId: transactionTypeId,
                    transactionBatchId: transactionBatchId,
                    referenceTransaction: referenceTransaction);
      #endregion
      #region Add Uncommited transaction to Agent
      App.Providers.UncommitedTransactionAgent.Add(warehouseTransaction);
      #endregion
      #region CheckFIFO
      if (stuffSerialCode != null && checkFIFO == true)
        CheckFIFO(stuffId: stuffId,
                      warehouseId: warehouseId,
                      stuffSerialCode: stuffSerialCode.Value,
                      transactionTypeId: transactionTypeId);
      #endregion
      return warehouseTransaction;
    }
    #region EditWarehouseTransaction
    internal BaseTransaction EditWarehouseTransaction(
        byte[] rowVersion,
        BaseTransaction warehouseTransaction,
        TValue<short> warehouseFiscalPeriodId = null)
    {

      if (warehouseFiscalPeriodId != null)
      {
        var warehosueFiscalPeriod = GetWarehouseFiscalPeriod(id: warehouseFiscalPeriodId);
        if (warehosueFiscalPeriod.IsClosed)
          throw new WarehouseFiscalPeriodIsClosedException(id: warehouseFiscalPeriodId);
        warehouseTransaction.WarehouseFiscalPeriodId = warehouseFiscalPeriodId;
      }
      repository.Update(warehouseTransaction, rowVersion);
      return warehouseTransaction;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetWarehouseTransactions<TResult>(
    Expression<Func<BaseTransaction, TResult>> selector,
    TValue<int> id = null,
    TValue<int> warehouseFiscalPeriodId = null,
    TValue<int> excludeWarehouseFiscalPeriodId = null,
    TValue<double> amount = null,
    TValue<DateTime> dateTime = null,
    TValue<DateTime> fromDateTime = null,
    TValue<DateTime> toDateTime = null,
    TValue<DateTime> effectDateTime = null,
    TValue<DateTime> fromEffectDateTime = null,
    TValue<DateTime> toEffectDateTime = null,
    TValue<string> description = null,
    TValue<long?[]> stuffSerialCodes = null,
    TValue<int?> billOfMaterialVersion = null,
    TValue<string> serial = null,
    TValue<int> stuffId = null,
    TValue<int> unitId = null,
    TValue<int> transactionBatchId = null,
    TValue<int> transactionTypeId = null,
    TValue<TransactionLevel> transactionLevel = null,
    TValue<TransactionLevel[]> transactionLevels = null,
    TValue<int[]> transactionTypeIds = null,
    TValue<int[]> excludeTransactionTypeIds = null,
    TValue<int> warehouseId = null,
    TValue<int> stuffCategoryId = null,
    TValue<int> baseEntityId = null,
    TValue<int[]> stuffIds = null,
    TValue<int?[]> warehouseIds = null)
    {

      var baseTransactionQuery = GetBaseTransactions(
                    selector: e => e,
                    id: id,
                    amount: amount,
                    dateTime: dateTime,
                    fromDateTime: fromDateTime,
                    toDateTime: toDateTime,
                    effectDateTime: effectDateTime,
                    fromEffectDateTime: fromEffectDateTime,
                    toEffectDateTime: toEffectDateTime,
                    description: description,
                    stuffSerialCodes: stuffSerialCodes,
                    stuffId: stuffId,
                    serial: serial,
                    unitId: unitId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    transactionBatchId: transactionBatchId,
                    transactionTypeId: transactionTypeId,
                    transactionLevel: transactionLevel,
                    transactionLevels: transactionLevels,
                    transactionTypeIds: transactionTypeIds,
                    excludeTransactionTypeIds: excludeTransactionTypeIds,
                    stuffCategoryId: stuffCategoryId,
                    stuffIds: stuffIds);
      var warehouseTransactions = baseTransactionQuery.OfType<BaseTransaction>();
      if (warehouseFiscalPeriodId != null)
        warehouseTransactions = warehouseTransactions.Where(i => i.WarehouseFiscalPeriodId == warehouseFiscalPeriodId);
      if (excludeWarehouseFiscalPeriodId != null)
        warehouseTransactions = warehouseTransactions.Where(i => i.WarehouseFiscalPeriodId != excludeWarehouseFiscalPeriodId);
      if (warehouseId != null)
        warehouseTransactions = warehouseTransactions.Where(i => i.WarehouseId == warehouseId);
      if (warehouseIds != null)
        warehouseTransactions = warehouseTransactions.Where(i => warehouseIds.Value.Contains(i.WarehouseId));
      return warehouseTransactions.Select(selector);
    }
    internal IQueryable<TResult> GetGroupedCardex<TResult>(
         Expression<Func<CardexGroupedResult, TResult>> selector,
        TValue<int> id = null,
        TValue<double> amount = null,
        TValue<DateTime> dateTime = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<DateTime> effectDateTime = null,
        TValue<DateTime> fromEffectDateTime = null,
        TValue<DateTime> toEffectDateTime = null,
        TValue<string> description = null,
        TValue<long?> stuffSerialCode = null,
        TValue<int?> billOfMaterialVersion = null,
        TValue<string> serial = null,
        TValue<int> stuffId = null,
        TValue<int> unitId = null,
        TValue<int> transactionBatchId = null,
        TValue<int> transactionTypeId = null,
        TValue<TransactionLevel> transactionLevel = null,
        TValue<int[]> transactionTypeIds = null,
        TValue<TransactionLevel[]> transactionLevels = null,
        TValue<int> warehouseId = null,
        TValue<int> stuffCategoryId = null,
        TValue<int> baseEntityId = null,
        TValue<int[]> stuffIds = null,
        TValue<int[]> warehouseIds = null)
    {

      var cardexesResults = GetWarehouseTransactions
                (
                    selector: e => new
                    {
                      Id = e.Id,
                      WarehosueFiscalPeriodId = e.WarehouseFiscalPeriodId,
                      WarehouseId = e.WarehouseId,
                      StuffId = e.StuffId,
                      StuffSerialCode = e.StuffSerialCode,
                      TransactionLevel = e.TransactionType.TransactionLevel,
                      TransactionTypeId = e.TransactionTypeId,
                      Value = e.Amount * e.Unit.ConversionRatio * (int)e.TransactionType.Factor,
                      TransactionBatchId = e.TransactionBatchId,
                      UnitId = e.UnitId
                    },
                     id: id,
                    amount: amount,
                    dateTime: dateTime,
                    fromDateTime: fromDateTime,
                    toDateTime: toDateTime,
                    effectDateTime: effectDateTime,
                    fromEffectDateTime: fromEffectDateTime,
                    toEffectDateTime: toEffectDateTime,
                    description: description,
                    stuffSerialCodes: new long?[] { stuffSerialCode },
                    stuffId: stuffId,
                    serial: serial,
                    unitId: unitId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    transactionBatchId: transactionBatchId,
                    transactionTypeId: transactionTypeId,
                    transactionLevel: transactionLevel,
                    transactionTypeIds: transactionTypeIds,
                    transactionLevels: transactionLevels,
                    stuffCategoryId: stuffCategoryId,
                    stuffIds: stuffIds);
      var groupedResult = (from cardex in cardexesResults
                           group cardex by new
                           {
                             cardex.WarehosueFiscalPeriodId,
                             cardex.TransactionBatchId,
                             cardex.StuffId,
                             cardex.WarehouseId,
                             cardex.StuffSerialCode
                           } into g
                           select new
                           {
                             WarehosueFiscalPeriodId = g.Key.WarehosueFiscalPeriodId,
                             TransnsactionBatchId = g.Key.TransactionBatchId,
                             StuffId = g.Key.StuffId,
                             WarehouseId = g.Key.WarehouseId,
                             AvailableAmount = g.Where(i => i.TransactionLevel == TransactionLevel.Available).Sum(i => i.Value),
                             BlockedAmount = g.Where(i => i.TransactionLevel == TransactionLevel.Blocked).Sum(i => i.Value),
                             PlanAmount = g.Where(i => i.TransactionLevel == TransactionLevel.Plan).Sum(i => i.Value),
                             QualityControlAmount = g.Where(i => i.TransactionLevel == TransactionLevel.QualityControl).Sum(i => i.Value),
                             WasteAmount = g.Where(i => i.TransactionLevel == TransactionLevel.Waste).Sum(i => i.Value),
                             StuffSerialCode = g.Key.StuffSerialCode
                           });
      var stuffs = App.Internals.SaleManagement.GetStuffs(e => e);
      var warehouses = App.Internals.WarehouseManagement.GetWarehouses(e => e);
      var serials = App.Internals.WarehouseManagement.GetStuffSerials(e => e);
      var transactionBatchs = App.Internals.WarehouseManagement.GetTransactionBatchs();
      var units = App.Internals.ApplicationBase.GetUnits(e => e, isMainUnit: true);
      var fiscalPeriods = App.Internals.WarehouseManagement.GetWarehouseFiscalPeriods(selector: e => e);
      var result = from cardex in groupedResult
                   join stuff in stuffs on cardex.StuffId equals stuff.Id
                   join warehouse in warehouses on cardex.WarehouseId equals warehouse.Id
                   join tSerial in serials on new { StuffId = cardex.StuffId, StuffSerialCode = cardex.StuffSerialCode } equals
                         new { StuffId = tSerial.StuffId, StuffSerialCode = (long?)tSerial.Code } into tempSerials
                   from s in tempSerials.DefaultIfEmpty()
                   join transactionBatch in transactionBatchs on cardex.TransnsactionBatchId equals transactionBatch.Id
                   join unit in units on stuff.UnitTypeId equals unit.UnitTypeId
                   join fiscalPeriod in fiscalPeriods on cardex.WarehosueFiscalPeriodId equals fiscalPeriod.Id into tempFiscalPeriods
                   from fiscalPeriod in tempFiscalPeriods.DefaultIfEmpty()
                   select new CardexGroupedResult()
                   {
                     StuffId = cardex.StuffId,
                     StuffCode = stuff.Code,
                     StuffName = stuff.Name,
                     AvailableAmount = cardex.AvailableAmount,
                     BlockedAmount = cardex.BlockedAmount,
                     PlanAmount = cardex.PlanAmount,
                     WasteAmount = cardex.WasteAmount,
                     QualityControlAmount = cardex.QualityControlAmount,
                     Serial = s.Serial,
                     BillOfMaterialVersion = s.BillOfMaterialVersion,
                     WarehouseId = warehouse.Id,
                     WarehouseName = warehouse.Name,
                     StuffSerialCode = cardex.StuffSerialCode,
                     TransnsactionBatchId = cardex.TransnsactionBatchId,
                     WarehouseFiscalPeriodId = cardex.WarehosueFiscalPeriodId,
                     WarehouseFiscalPeriodName = fiscalPeriod.Name,
                     DateTime = transactionBatch.DateTime,
                     EmployeeFullName = transactionBatch.User.Employee.FirstName + " " + transactionBatch.User.Employee.LastName,
                     UserName = transactionBatch.User.UserName,
                     UnitName = unit.Name,
                     BaseEntityCode = transactionBatch.BaseEntity.Description,
                     BaseEntityDescription = transactionBatch.BaseEntity.Code
                   };
      return result.Select(selector);
    }
    #endregion
    #region Get
    internal BaseTransaction GetWarehouseTransaction(int id) => GetWarehouseTransaction(selector: e => e, id: id);
    internal TResult GetWarehouseTransaction<TResult>(
        Expression<Func<BaseTransaction, TResult>> selector,
        int id)
    {

      var warehouseTransaction = GetWarehouseTransactions(
                selector: selector,
                id: id).SingleOrDefault();
      if (warehouseTransaction == null)
        throw new WarehouseTransactionNotFoundException(id);
      return warehouseTransaction;
    }
    #endregion
    #region RollbackWarehouseTransactionProcess
    internal BaseTransaction RollbackWarehouseTransaction(
        int id,
        byte[] rowVersion,
        int transactionBatchId)
    {

      var warehouseTransaction = GetWarehouseTransaction(id: id);
      return RollbackWarehouseTransaction(
                warehouseTransaction: warehouseTransaction,
                rowVersion: rowVersion,
                transactionBatchId: transactionBatchId);
    }
    internal BaseTransaction RollbackWarehouseTransaction(
        BaseTransaction warehouseTransaction,
        byte[] rowVersion,
        int transactionBatchId)
    {

      #region GetTransactionBatch
      var transactionBatch = GetTransactionBatch(id: transactionBatchId);
      #endregion
      #region GetRollbackTransactionType
      var rollbackTransactionType = warehouseTransaction.TransactionType.RollbackTransactionType?.Id;
      if (rollbackTransactionType == null)
        throw new RollBackTransactionTypeNotFound(warehouseTransaction.TransactionTypeId);
      #endregion
      #region Add RollBack WarehouseTransaction
      if (warehouseTransaction.WarehouseId == null)
        throw new BaseTransactionHasNoWarehouseException(id: warehouseTransaction.Id);
      return AddWarehouseTransaction(
                    transactionBatchId: transactionBatchId,
                    effectDateTime: transactionBatch.DateTime,
                    stuffId: warehouseTransaction.StuffId,
                    billOfMaterialVersion: warehouseTransaction.BillOfMaterialVersion,
                    stuffSerialCode: warehouseTransaction.StuffSerialCode,
                    warehouseId: warehouseTransaction.WarehouseId.Value,
                    transactionTypeId: rollbackTransactionType.Value,
                    amount: warehouseTransaction.Amount,
                    unitId: warehouseTransaction.UnitId,
                    description: "",
                    referenceTransaction: warehouseTransaction);
      #endregion
    }
    #endregion
    #region SearchCardex
    public IQueryable<CardexResult> SearchCardexesResults(
        IQueryable<CardexResult> query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from pTerminal in query
                where
                      pTerminal.WarehouseName.Contains(searchText) ||
                      pTerminal.EmployeeFullName.Contains(searchText) ||
                      pTerminal.UnitName.Contains(searchText) ||
                      pTerminal.StuffCode.Contains(searchText) ||
                      pTerminal.StuffName.Contains(searchText) ||
                      pTerminal.TransactionTypeName.Contains(searchText) ||
                      pTerminal.Serial.Contains(searchText)
                select pTerminal;
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region searchCardexGroupedResult
    public IQueryable<CardexGroupedResult> SearchCardexGroupeResults(
        IQueryable<CardexGroupedResult> query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from pTerminal in query
                where
                      pTerminal.WarehouseName.Contains(searchText) ||
                      pTerminal.EmployeeFullName.Contains(searchText) ||
                      pTerminal.UnitName.Contains(searchText) ||
                      pTerminal.StuffCode.Contains(searchText) ||
                      pTerminal.StuffName.Contains(searchText) ||
                      pTerminal.BaseEntityCode.Contains(searchText) ||
                      pTerminal.Serial.Contains(searchText)
                select pTerminal;
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region SortCardexResult
    public IOrderedQueryable<CardexResult> SortCardexResults(IQueryable<CardexResult> query,
        SortInput<CardexSortType> sort)
    {
      switch (sort.SortType)
      {
        case CardexSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case CardexSortType.WarehouseFiscalPeriodName:
          return query.OrderBy(a => a.WarehouseFiscalPeriodName, sort.SortOrder);
        case CardexSortType.ReferenceTransactionId:
          return query.OrderBy(a => a.ReferenceTransactionId, sort.SortOrder);
        case CardexSortType.TransnsactionBatchId:
          return query.OrderBy(a => a.TransnsactionBatchId, sort.SortOrder);
        case CardexSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case CardexSortType.Amount:
          return query.OrderBy(a => a.Amount, sort.SortOrder);
        case CardexSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case CardexSortType.EffectDateTime:
          return query.OrderBy(a => a.EffectDateTime, sort.SortOrder);
        case CardexSortType.StuffSerialCode:
          return query.OrderBy(a => a.StuffSerialCode, sort.SortOrder);
        case CardexSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case CardexSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case CardexSortType.UserName:
          return query.OrderBy(a => a.UserName, sort.SortOrder);
        case CardexSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case CardexSortType.TransactionTypeName:
          return query.OrderBy(a => a.TransactionTypeName, sort.SortOrder);
        case CardexSortType.TransactionLevel:
          return query.OrderBy(a => a.TransactionLevel, sort.SortOrder);
        case CardexSortType.TransactionTypeFactor:
          return query.OrderBy(a => a.TransactionTypeFactor, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<CardexGroupedResult> SortCardexGroupedResults(IQueryable<CardexGroupedResult> query,
        SortInput<CardexSortType> sort)
    {
      switch (sort.SortType)
      {
        case CardexSortType.WarehouseFiscalPeriodName:
          return query.OrderBy(a => a.WarehouseFiscalPeriodName, sort.SortOrder);
        case CardexSortType.TransnsactionBatchId:
          return query.OrderBy(a => a.TransnsactionBatchId, sort.SortOrder);
        case CardexSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case CardexSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case CardexSortType.EffectDateTime:
          return query.OrderBy(a => a.EffectDateTime, sort.SortOrder);
        case CardexSortType.StuffSerialCode:
          return query.OrderBy(a => a.StuffSerialCode, sort.SortOrder);
        case CardexSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case CardexSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case CardexSortType.UserName:
          return query.OrderBy(a => a.UserName, sort.SortOrder);
        case CardexSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case CardexSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case CardexSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case CardexSortType.AvailableAmount:
          return query.OrderBy(a => a.AvailableAmount, sort.SortOrder);
        case CardexSortType.BlockedAmount:
          return query.OrderBy(a => a.BlockedAmount, sort.SortOrder);
        case CardexSortType.QualityControlAmount:
          return query.OrderBy(a => a.QualityControlAmount, sort.SortOrder);
        case CardexSortType.WasteAmount:
          return query.OrderBy(a => a.WasteAmount, sort.SortOrder);
        case CardexSortType.BaseEntityCode:
          return query.OrderBy(a => a.BaseEntityCode, sort.SortOrder);
        case CardexSortType.BaseEntityDescription:
          return query.OrderBy(a => a.BaseEntityDescription, sort.SortOrder);
        default:
          return query.OrderBy(a => a.TransnsactionBatchId, sort.SortOrder);
      }
    }
    #endregion
    #region GetWarehouseInventories
    public IQueryable<StuffAvailableInventoryResult> GetStuffsAvailableInventories(
       short warehouseId,
       IQueryable<int> stuffIds)
    {

      string stuffIdStr = null;
      if (stuffIds != null)
        stuffIdStr = string.Join("@", stuffIds);
      var parameters = new List<SqlParameter>();
      parameters.Add(new SqlParameter() { ParameterName = "@warehouseId", Value = (object)warehouseId ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@stuffIds", Value = (object)stuffIdStr ?? DBNull.Value });
      var query = repository.CreateQuery<StuffAvailableInventoryResult>(@"EXEC dbo.usp_GetStuffsAvailableInventories @warehouseId ,@stuffIds",
                    parameters.ToArray<DbParameter>());
      return query.ToList().AsQueryable();
    }
    public IQueryable<StuffSerialInventoryResult> GetStuffSerialInventories(
        int? warehouseId = null,
        int? stuffId = null,
        long? stuffSerialCode = null,
        string serial = null)
    {

      if (!string.IsNullOrEmpty(serial) && stuffSerialCode == null)
      {
        var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: serial);
        stuffSerialCode = stuffSerial.Code;
        if (stuffId == null)
          stuffId = stuffSerial.StuffId;
        else if (stuffId != stuffSerial.StuffId)
        {
          var stuff = App.Internals.SaleManagement.GetStuff(id: stuffId.Value);
          throw new SerialStuffNotMatchException(serial: serial,
                    serialStuffCode: stuffSerial.Stuff.Code,
                    stuffCode: stuff.Code);
        }
      }
      var parameters = new List<SqlParameter>();
      parameters.Add(new SqlParameter() { ParameterName = "@warehouseId", Value = (object)warehouseId ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@stuffId", Value = (object)stuffId ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@stuffSerialCode", Value = (object)stuffSerialCode ?? DBNull.Value });
      var query = repository.CreateQuery<StuffSerialInventoryResult>(@"EXEC dbo.usp_GetStuffSerialInventory  @warehouseId,@stuffId,@stuffSerialCode",
                    parameters.ToArray<DbParameter>());
      return query;
    }
    public IQueryable<WarehouseInventoryResult> GetWarehouseInventories(
        int? warehouseId = null,
        int? stuffId = null,
        long? stuffSerialCode = null,
        int? billOfMaterialVersion = null,
        int? stuffCategoryId = null,
        string serial = null,
        DateTime? fromEffectDateTime = null,
        DateTime? toEffectDateTime = null,
        bool? groupBySerial = null,
        bool? groupByBillOfMaterialVersion = null,
        IQueryable<int> stuffIds = null,
        TValue<StuffSerialStatus> serialStatus = null,
        TValue<StuffSerialStatus[]> serialStatuses = null
        )
    {

      #region Linq
      //#region GetTransactions
      //var warehouseTransactions = GetWarehouseTransactions(
      //        selector: transaction => new
      //        {
      //            StuffId = transaction.StuffId,
      //            WarehouseId = transaction.WarehouseId,
      //            TransactionLevel = transaction.TransactionType.TransactionLevel,
      //            Serial = groupBySerial ? transaction.StuffSerial.Serial : null,
      //            Value = transaction.Amount * transaction.Unit.ConversionRatio * (int)transaction.TransactionType.Factor,
      //        },
      //        warehouseId: warehouseId,
      //        stuffId: stuffId,
      //        serial: serial,
      //        stuffCategoryId: stuffCategoryId,
      //        fromEffectDateTime: fromEffectDateTime,
      //        toEffectDateTime: toEffectDateTime)
      //    
      //;
      //#region GetBillOfMaterials
      //var billOfMaterials = App.Internals.Planning.GetBillOfMaterials()
      //        
      //;
      //#endregion
      //#region LevelInventories
      //var inventories = from item in warehouseTransactions
      //                  group item by new
      //                  {
      //                      WarehouseId = item.WarehouseId,
      //                      StuffId = item.StuffId,
      //                      Serial = item.Serial
      //                  }
      //    into gItems
      //                  select new
      //                  {
      //                      WarehouseId = gItems.Key.WarehouseId,
      //                      StuffId = gItems.Key.StuffId,
      //                      Serial = gItems.Key.Serial,
      //                      AvailableAmount = (double?)gItems
      //                        .Where(t => t.TransactionLevel == TransactionLevel.Available)
      //                        .Sum(i => i.Value),
      //                      BlockedAmount = (double?)gItems
      //                        .Where(t => t.TransactionLevel == TransactionLevel.Blocked)
      //                        .Sum(i => i.Value),
      //                      QualityControlAmount = (double?)gItems
      //                        .Where(t => t.TransactionLevel == TransactionLevel.QualityControl)
      //                        .Sum(i => i.Value),
      //                      WasteAmount = (double?)gItems
      //                        .Where(t => t.TransactionLevel == TransactionLevel.Waste)
      //                        .Sum(i => i.Value),
      //                      TotalAmount = (double?)gItems
      //                        .Where(t => t.TransactionLevel != TransactionLevel.Waste && t.TransactionLevel != TransactionLevel.Plan)
      //                        .Sum(i => i.Value)
      //                  };
      //#endregion
      //#region GetMainUnits
      //var units = App.Internals.ApplicationBase.GetUnits(
      //        selector: i => new
      //        {
      //            Id = i.Id,
      //            Name = i.Name,
      //            UnitTypeId = i.UnitTypeId
      //        },
      //        isMainUnit: true)
      //    
      //;
      //#endregion
      //#region GetStuffs
      //var stuffs = App.Internals.SaleManagement.GetStuffs(
      //    selector: i => new
      //    {
      //        Id = i.Id,
      //        Name = i.Name,
      //        Code = i.Code,
      //        UnitTypeId = i.UnitTypeId,
      //        StuffCategoryName = i.StuffCategory.Name,
      //        StuffTypeName = i.StuffType
      //    },
      //    id: stuffId)
      //    
      //;
      //#endregion
      ////#region GetBillOfMaterials
      ////var billOfMaterials = App.Internals.Planning.GetBillOfMaterials()
      ////        
      ////;
      ////#endregion
      //#region GetWarehouses
      //var warehouses = App.Internals.WarehouseManagement.GetWarehouses(
      //        selector: i => new
      //        {
      //            Id = i.Id,
      //            Name = i.Name
      //        },
      //        id: warehouseId)
      //    
      //;
      //#endregion
      //#region GetStockPlace
      //var groupedStuffStockPlaces = GetGroupedStuffStockPlaces(
      //    warehouseId: warehouseId,
      //    stuffId: stuffId)
      //    
      //;
      //#endregion
      //#region ToResult
      //var result = from inventory in inventories
      //             join stuff in stuffs on inventory.StuffId equals stuff.Id
      //             //join billOfMaterial in billOfMaterials on inventory.StuffId equals billOfMaterial.StuffId
      //             join warehouse in warehouses on inventory.WarehouseId equals warehouse.Id
      //             join unit in units on stuff.UnitTypeId equals unit.UnitTypeId
      //             join stockPlace in groupedStuffStockPlaces on new { warehouseId = inventory.WarehouseId, stuffId = inventory.StuffId }
      //              equals new { warehouseId = stockPlace.WarehouseId, stuffId = stockPlace.StuffId } into leftJoinResult
      //             from stockPlace in leftJoinResult.DefaultIfEmpty()
      //             select new WarehouseInventoryResult
      //             {
      //                 WarehouseId = warehouse.Id,
      //                 WarehouseName = warehouse.Name,
      //                 StuffId = stuff.Id,
      //                 StuffCategoryName = stuff.StuffCategoryName,
      //                 StuffCode = stuff.Code,
      //                 StuffName = stuff.Name,
      //                 BillOfMaterialVersion = null,
      //                 BillOfMaterialTitle = "",
      //                 Serial = inventory.Serial,
      //                 TotalAmount = inventory.TotalAmount ?? 0,
      //                 AvailableAmount = inventory.AvailableAmount ?? 0,
      //                 BlockedAmount = inventory.BlockedAmount ?? 0,
      //                 QualityControlAmount = inventory.QualityControlAmount ?? 0,
      //                 WasteAmount = inventory.WasteAmount ?? 0,
      //                 UnitId = unit.Id,
      //                 UnitName = unit.Name,
      //                 StuffTypeName = stuff.StuffTypeName,
      //                 StockPlaceCodes = stockPlace.StockPlaceCodes.ToList(),
      //                 StockPlaceTitles = stockPlace.StockPlaceTitles.ToList()
      //             };
      #endregion
      if (!string.IsNullOrEmpty(serial) && stuffSerialCode == null)
      {
        var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: serial);
        stuffSerialCode = stuffSerial.Code;
        if (stuffId == null)
          stuffId = stuffSerial.StuffId;
        else if (stuffId != stuffSerial.StuffId)
        {
          var stuff = App.Internals.SaleManagement.GetStuff(id: stuffId.Value);
          throw new SerialStuffNotMatchException(serial: serial,
                    serialStuffCode: stuffSerial.Stuff.Code,
                    stuffCode: stuff.Code);
        }
      }
      string stuffIdStr = null;
      if (stuffIds != null)
        stuffIdStr = string.Join("@", stuffIds);
      var parameters = new List<SqlParameter>();
      parameters.Add(new SqlParameter() { ParameterName = "@warehouseId", Value = (object)warehouseId ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@stuffId", Value = (object)stuffId ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@billOfMaterialVersion", Value = (object)billOfMaterialVersion ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@stuffSerialCode", Value = (object)stuffSerialCode ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@stuffCategoryId", Value = (object)stuffCategoryId ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@fromEffectDateTime", Value = (object)fromEffectDateTime ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@toEffectDateTime", Value = (object)toEffectDateTime ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@groupBySerial", Value = (object)groupBySerial ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@groupByBillOfMaterialVersion", Value = (object)groupByBillOfMaterialVersion ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@stuffsIds", Value = (object)stuffIdStr ?? DBNull.Value });
      var query = repository.CreateQuery<WarehouseInventoryResult>(@"EXEC dbo.usp_GetWarehouseInventories @warehouseId ,@stuffId ,@billOfMaterialVersion,@stuffSerialCode,@stuffCategoryId,@fromEffectDateTime,@toEffectDateTime,@groupBySerial,@groupByBillOfMaterialVersion,@stuffsIds",
                    parameters.ToArray<DbParameter>());
      return query.ToList().AsQueryable();
    }
    public IQueryable<WarehouseInventoryResult> GetWarehouseInventoryForIssue(
      short warehouseId,
      string serial = null,
      string[] selectedSerial = null
      )
    {

      var warehouse = GetWarehouse(
                    selector: e => e,
                    id: warehouseId);
      if (warehouse.FIFO)
      {
        var stuffserial = GetStuffSerial(
                   selector: e => e,
                   serial: serial);
        var warehouseInventories = GetWarehouseInventories(
                      warehouseId: warehouseId,
                      stuffId: stuffserial.StuffId,
                      groupBySerial: true)


                  .Where(x => x.SerialProfileCode != null && x.AvailableAmount > 0)
                  .ToList();
        var stuffSerialProfiles = GetStuffSerials(
                      selector: e => new
                      {
                        e.StuffId,
                        e.SerialProfileCode,
                        StuffSerialCode = e.Code
                      },
                      stuffId: stuffserial.StuffId)

                  .ToList();
        var serialList = (from warehouseInventory in warehouseInventories
                          join stuffSerialProfile in stuffSerialProfiles on new
                          {
                            StuffId = warehouseInventory.StuffId,
                            StuffSerialCode = warehouseInventory.StuffSerialCode
                          }
                                    equals new
                                    {
                                      StuffId = stuffSerialProfile.StuffId,
                                      StuffSerialCode = (long?)stuffSerialProfile.StuffSerialCode
                                    } into serialprofile
                          from tempserialprofile in serialprofile.DefaultIfEmpty()
                          select new
                          {
                            SerialProfileCode = tempserialprofile.SerialProfileCode,
                            serial = warehouseInventory.Serial
                          }).ToList();
        serialList = serialList.Where(i => i.SerialProfileCode <= stuffserial.SerialProfileCode).ToList();
        if (selectedSerial != null)
        {
          serialList = serialList.Where(i => !selectedSerial.Contains(i.serial)).ToList();
        }
        if (serialList.Count > 0)
        {
          var minSerialProfileCode = serialList.Min(x => x.SerialProfileCode);
          if (stuffserial.SerialProfileCode > minSerialProfileCode)
          {
            throw new SerialOrderException(minSerialProfileCode);
          }
        }
      }
      var warehouseInventorie = GetWarehouseInventories(
                    warehouseId: warehouseId,
                    serial: serial,
                    groupBySerial: true);
      return warehouseInventorie;
    }
    public Result GetWarehouseInventoriesForIssue(
        GetWarehouseInventoryForIssueInput[] getWarehouseInventoriesForIssueInput
     )
    {

      foreach (var getWarehouseInventoryForIssueInput in getWarehouseInventoriesForIssueInput)
      {
        var warehouse = GetWarehouse(
                      selector: e => e,
                      id: getWarehouseInventoryForIssueInput.WarehouseId);
        if (warehouse.FIFO)
        {
          var stuffserial = GetStuffSerial(
                     selector: e => e,
                     serial: getWarehouseInventoryForIssueInput.Serial);
          var warehouseInventories = GetWarehouseInventories(
                        warehouseId: getWarehouseInventoryForIssueInput.WarehouseId,
                        stuffId: stuffserial.StuffId,
                        groupBySerial: true)


                    .Where(x => x.SerialProfileCode != null && x.AvailableAmount > 0)
                    .ToList();
          var stuffSerialProfiles = GetStuffSerials(
                        selector: e => new
                        {
                          e.StuffId,
                          e.SerialProfileCode,
                          StuffSerialCode = e.Code
                        },
                        stuffId: stuffserial.StuffId)


                    .ToList();
          var serialList = (from warehouseInventory in warehouseInventories
                            join stuffSerialProfile in stuffSerialProfiles on new
                            {
                              StuffId = warehouseInventory.StuffId,
                              StuffSerialCode = warehouseInventory.StuffSerialCode
                            }
                                      equals new
                                      {
                                        StuffId = stuffSerialProfile.StuffId,
                                        StuffSerialCode = (long?)stuffSerialProfile.StuffSerialCode
                                      } into serialprofile
                            from tempserialprofile in serialprofile.DefaultIfEmpty()
                            select new
                            {
                              SerialProfileCode = tempserialprofile.SerialProfileCode,
                              serial = warehouseInventory.Serial
                            });
          serialList = serialList.Where(i => !getWarehouseInventoriesForIssueInput
                        .Where(x => x.WarehouseId == getWarehouseInventoryForIssueInput.WarehouseId)
                        .Select(x => x.Serial)
                        .Contains(i.serial))
                    .ToList();
          if (serialList.ToList().Count > 0)
          {
            var minSerialProfileCode = serialList.Min(x => x.SerialProfileCode);
            if (getWarehouseInventoryForIssueInput.SerialProfileCode > minSerialProfileCode)
            {
              throw new SerialOrderException(minSerialProfileCode);
            }
          }
        }
      }
      return new Result();
    }
    public IQueryable<WarehouseEnterExitResult> GetWarehouseEnterExitResult(
        int? fromWarehouseId = null,
        int? toWarehouseId = null,
        int? stuffId = null,
        int? stuffCategoryId = null,
        string serial = null,
        DateTime? fromDateTime = null,
        DateTime? toDateTime = null
        )
    {

      #region grouped-result
      //var warehouseIssues = repository.GetQuery<WarehouseIssue>();
      //var warehouseIssueItems = repository.GetQuery<WarehouseIssueItem>();
      //var units = repository.GetQuery<Unit>();
      //var enterQuery = (from issue in warehouseIssues
      //                  join
      //                  item in warehouseIssueItems on issue.Id equals item.WarehouseIssueId
      //                  join
      //                  unit in units on item.Unit.UnitTypeId equals unit.UnitTypeId
      //                  where issue.ToWarehouseId != null && unit.IsMainUnit && issue.Status == WarehouseIssueStatusType.Accepted
      //                  select new
      //                  {
      //                      StuffId = item.StuffId,
      //                      StuffCode = item.Stuff.Code,
      //                      StuffName = item.Stuff.Name,
      //                      StuffCategoryId = item.Stuff.StuffCategoryId,
      //                      StuffCategoryName = item.Stuff.StuffCategory.Name,
      //                      Serial = item.StuffSerial.Serial,
      //                      WarehouseId = issue.ToWarehouseId,
      //                      WarehouseName = issue.ToWarehouse.Name,
      //                      DepartmentId = issue.ToDepartmentId,
      //                      DepartmentName = issue.ToDepartment.Name,
      //                      ToEmployeeId = issue.ToEmployeeId,
      //                      ToEmployeeName = issue.ToEmployee.FirstName + " " + issue.ToEmployee.LastName,
      //                      Amount = item.Amount * item.Unit.ConversionRatio,
      //                      UnitId = unit.Id,
      //                      UnitName = unit.Name,
      //                      DateTime = issue.DateTime,
      //                      UserId = issue.UserId,
      //                      UserName = issue.User.UserName,
      //                      EmployeeFullName = issue.User.Employee.FirstName + " " + issue.User.Employee.LastName
      //                  });
      //var exitQuery = (from issue in warehouseIssues
      //                 join
      //                 item in warehouseIssueItems on issue.Id equals item.WarehouseIssueId
      //                 join
      //                 unit in units on item.Unit.UnitTypeId equals unit.UnitTypeId
      //                 where unit.IsMainUnit && issue.Status == WarehouseIssueStatusType.Accepted
      //                 select new
      //                 {
      //                     StuffId = item.StuffId,
      //                     StuffCode = item.Stuff.Code,
      //                     StuffName = item.Stuff.Name,
      //                     StuffCategoryId = item.Stuff.StuffCategoryId,
      //                     StuffCategoryName = item.Stuff.StuffCategory.Name,
      //                     Serial = item.StuffSerial.Serial,
      //                     WarehouseId = (int?)issue.FromWarehouseId,
      //                     WarehouseName = issue.FromWarehouse.Name,
      //                     Amount = -1 * item.Amount * item.Unit.ConversionRatio,
      //                     UnitId = unit.Id,
      //                     UnitName = unit.Name,
      //                     DateTime = issue.DateTime,
      //                     UserId = issue.UserId,
      //                     UserName = issue.User.UserName,
      //                     EmployeeFullName = issue.User.Employee.FirstName + " " + issue.User.Employee.LastName
      //                 });
      //var enterExitQuery = enterQuery.Union(exitQuery);
      ////var enterExitQuery = enterQuery;
      //if (stuffCategoryId != null)
      //    enterExitQuery = enterExitQuery.Where(x => x.StuffCategoryId == stuffCategoryId);
      //if (warehouseId != null)
      //    enterExitQuery = enterExitQuery.Where(x => x.WarehouseId == warehouseId);
      //if (stuffId != null)
      //    enterExitQuery = enterExitQuery.Where(x => x.StuffId == stuffId);
      //if (serial != null)
      //    enterExitQuery = enterExitQuery.Where(x => x.Serial == serial);
      //if (fromDateTime != null)
      //    enterExitQuery = enterExitQuery.Where(x => x.DateTime >= fromDateTime);
      //if (toDateTime != null)
      //    enterExitQuery = enterExitQuery.Where(x => x.DateTime <= toDateTime);
      //var result = (from item in enterExitQuery
      //              group item by new { item.WarehouseId, item.WarehouseName, item.DepartmentId, item.DepartmentName, item.ToEmployeeId, item.ToEmployeeName } into g
      //              select new WarehouseEnterExitResult()
      //              {
      //                  WarehouseId = g.Key.WarehouseId,
      //                  WarehouseName = g.Key.WarehouseName,
      //                  DepartmentId = g.Key.DepartmentId,
      //                  DepartmentName = g.Key.DepartmentName,
      //                  ToEmployeeId = g.Key.ToEmployeeId,
      //                  toEmployeeName = g.Key.ToEmployeeName,
      //                  TotalEnterQty = g.Where(x => x.Amount > 0).Sum(x => x.Amount),
      //                  TotalExitQty = g.Where(x => x.Amount < 0).Sum(x => x.Amount),
      //                  Stuffs = g.GroupBy(x => new { x.StuffId, x.StuffCode, x.StuffName }).Select(stuff => new WarehouseEnterExitStuff()
      //                  {
      //                      StuffId = stuff.Key.StuffId,
      //                      StuffCode = stuff.Key.StuffCode,
      //                      StuffName = stuff.Key.StuffName,
      //                      TotalEnterQty = stuff.Where(x => x.Amount > 0).Sum(x => x.Amount),
      //                      TotalExitQty = stuff.Where(x => x.Amount < 0).Sum(x => x.Amount),
      //                      Serials = stuff.Select(s => new WarehouseEnterExitSerial()
      //                      {
      //                          Serial = s.Serial,
      //                          Qty = s.Amount,
      //                          UnitId = s.UnitId,
      //                          UnitName = s.UnitName,
      //                          DateTime = s.DateTime,
      //                          UserId = s.UserId,
      //                          UserName = s.UserName,
      //                          EmployeeFullName = s.EmployeeFullName
      //                      }).OrderBy(x => x.Serial).OrderByDescending(x => x.Qty).AsQueryable()
      //                  }).AsQueryable()
      //              });
      #endregion
      var warehouseIssues = repository.GetQuery<WarehouseIssue>();
      var warehouseIssueItems = repository.GetQuery<WarehouseIssueItem>();
      var units = repository.GetQuery<Unit>();
      var query = (from issue in warehouseIssues
                   join
                         item in warehouseIssueItems on issue.Id equals item.WarehouseIssueId
                   where issue.Status == WarehouseIssueStatusType.Accepted
                   select new WarehouseEnterExitResult()
                   {
                     StuffCategoryId = item.Stuff.StuffCategoryId,
                     StuffCategoryName = item.Stuff.StuffCategory.Name,
                     StuffId = item.StuffId,
                     StuffCode = item.Stuff.Code,
                     StuffName = item.Stuff.Name,
                     Serial = item.StuffSerial.Serial,
                     FromWarehouseId = issue.FromWarehouseId,
                     FromWarehouseName = issue.FromWarehouse.Name,
                     ToWarehouseId = issue.ToWarehouseId,
                     ToWarehouseName = issue.ToWarehouse.Name,
                     ToDepartmentId = issue.ToDepartmentId,
                     ToDepartmentName = issue.ToDepartment.Name,
                     ToEmployeeId = issue.ToEmployeeId,
                     ToEmployeeName = issue.ToEmployee.FirstName + " " + issue.ToEmployee.LastName,
                     Amount = item.Amount * item.Unit.ConversionRatio,
                     UnitId = item.Unit.Id,
                     UnitName = item.Unit.Name,
                     DateTime = issue.DateTime,
                     UserId = issue.UserId,
                     UserName = issue.User.UserName,
                     EmployeeFullName = issue.User.Employee.FirstName + " " + issue.User.Employee.LastName,
                     BillOfMaterialVersion = item.BillOfMaterialVersion,
                     EmployeeId = issue.User.Employee.Id,
                     Status = issue.Status,
                     StuffNone = item.Stuff.Noun,
                     StuffSerialCode = item.StuffSerialCode,
                     TransactionLevel = item.TransactionLevel,
                     WarehouseIssueId = issue.Id,
                     WarehouseIssueItemId = item.Id
                   });
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (fromWarehouseId != null)
        query = query.Where(i => i.FromWarehouseId == fromWarehouseId);
      if (toWarehouseId != null)
        query = query.Where(i => i.ToWarehouseId == toWarehouseId);
      if (stuffCategoryId != null)
        query = query.Where(i => i.StuffCategoryId == stuffCategoryId);
      if (!string.IsNullOrEmpty(serial))
      {
        var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial);
        if (stuffSerial != null)
          query = query.Where(i => i.StuffSerialCode == stuffSerial.Code);
      }
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);
      return query;
    }
    public IQueryable<WarehousePriceReportResult> GetWarehousePriceReportResult(
    int? warehouseId = null,
    int? stuffId = null,
    int? stuffCategoryId = null,
    DateTime? fromDateTime = null,
    DateTime? toDateTime = null,
    DateTime? fromStuffLastTransactionDateTime = null,
    DateTime? toStuffLastTransactionDateTime = null,
    int? currencyId = null,
    CurrencyRateValue[] currencyRateValues = null,
    bool groupByWarehouse = false,
    bool groupByStuffCategory = false,
    bool groupByStuff = false,
    bool calculateByArzeshDaftari = false,
    bool CalculateByLastPrice = false
    )
    {

      var query = GetWarehouseInventoryPrice(warehouseId: warehouseId, stuffId: stuffId, stuffCategoryId: stuffCategoryId, fromEffectDateTime: fromDateTime, toEffectDateTime: toDateTime, currencyRateValues: currencyRateValues, currencyId: currencyId, groupByWarehouse: groupByWarehouse, groupByStuffCategory: groupByStuffCategory, groupByStuff: groupByStuff);
      return query;
    }
    #endregion
    #region GetWarehouseInventoriesPrice
    public IQueryable<WarehousePriceReportResult> GetWarehouseInventoryPrice(
        int? warehouseId = null,
        int? stuffId = null,
        long? stuffSerialCode = null,
        int? billOfMaterialVersion = null,
        int? stuffCategoryId = null,
        string serial = null,
        DateTime? fromEffectDateTime = null,
        DateTime? toEffectDateTime = null,
        DateTime? fromStuffLastTransactionDateTime = null,
        DateTime? toStuffLastTransactionDateTime = null,
        bool? groupBySerial = null,
        bool? groupByBillOfMaterialVersion = null,
        IQueryable<int> stuffIds = null,
        TValue<StuffSerialStatus> serialStatus = null,
        TValue<StuffSerialStatus[]> serialStatuses = null,
        int? currencyId = null,
        CurrencyRateValue[] currencyRateValues = null,
        bool? groupByWarehouse = null,
        bool? groupByStuffCategory = null,
        bool? groupByStuff = null
        )
    {

      if (!string.IsNullOrEmpty(serial) && stuffSerialCode == null)
      {
        var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: serial);
        stuffSerialCode = stuffSerial.Code;
        if (stuffId == null)
          stuffId = stuffSerial.StuffId;
        else if (stuffId != stuffSerial.StuffId)
        {
          var stuff = App.Internals.SaleManagement.GetStuff(id: stuffId.Value);
          throw new SerialStuffNotMatchException(serial: serial,
                    serialStuffCode: stuffSerial.Stuff.Code,
                    stuffCode: stuff.Code);
        }
      }
      string stuffIdStr = null;
      if (stuffIds != null)
        stuffIdStr = string.Join("@", stuffIds);
      var parameters = new List<SqlParameter>();
      parameters.Add(new SqlParameter() { ParameterName = "@warehouseId", Value = (object)warehouseId ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@stuffId", Value = (object)stuffId ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@billOfMaterialVersion", Value = (object)billOfMaterialVersion ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@stuffSerialCode", Value = (object)stuffSerialCode ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@stuffCategoryId", Value = (object)stuffCategoryId ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@fromEffectDateTime", Value = (object)fromEffectDateTime ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@toEffectDateTime", Value = (object)toEffectDateTime ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@fromStuffLastTransactionDateTime", Value = (object)fromStuffLastTransactionDateTime ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@toStuffLastTransactionDateTime", Value = (object)toStuffLastTransactionDateTime ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@groupBySerial", Value = (object)groupBySerial ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@groupByBillOfMaterialVersion", Value = (object)groupByBillOfMaterialVersion ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@stuffsIds", Value = (object)stuffIdStr ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@groupByWarehouse", Value = (object)groupByWarehouse ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@groupByStuffCategory", Value = (object)groupByStuffCategory ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@groupByStuff", Value = (object)groupByStuff ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@currencyId", Value = (object)currencyId ?? DBNull.Value });
      var sb = new StringBuilder();
      string currencyRateFlatValue = null;
      if (currencyRateValues != null)
      {
        int index = 1;
        foreach (var item in currencyRateValues)
        {
          sb.Append(item.FromCurrencyId);
          sb.Append(',');
          sb.Append(item.ToCurrencyId);
          sb.Append(',');
          sb.Append(item.Rate);
          if (index < currencyRateValues.Length)
            sb.Append('|');
          index++;
        }
        currencyRateFlatValue = sb.ToString();
      }
      parameters.Add(new SqlParameter() { ParameterName = "@currencyRateValues", Value = (object)currencyRateFlatValue ?? DBNull.Value });
      var query = repository.CreateQuery<lena.Domains.WarehousePriceReportResult>(@"select * from dbo.GetWarehouseInventoryPrice( @warehouseId, @stuffId, @billOfMaterialVersion, @stuffSerialCode, @stuffCategoryId, @fromEffectDateTime, @toEffectDateTime, @fromStuffLastTransactionDateTime , @toStuffLastTransactionDateTime, @groupBySerial, @groupByBillOfMaterialVersion , @stuffsIds,  @currencyId,@currencyRateValues,
                  @groupByWarehouse , @groupByStuffCategory , @groupByStuff  )
                    ",
                parameters.ToArray<DbParameter>());
      return query;
    }
    #endregion
    #region Search
    public IQueryable<WarehouseInventoryResult> SearchWarehouseInventoryResults(
        IQueryable<WarehouseInventoryResult> query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from pTerminal in query
                where
                      pTerminal.WarehouseName.Contains(searchText) ||
                      pTerminal.StuffCategoryName.Contains(searchText) ||
                      pTerminal.Serial.Contains(searchText) ||
                      pTerminal.StuffCode.Contains(searchText) ||
                      pTerminal.StuffName.Contains(searchText) ||
                      pTerminal.UnitName.Contains(searchText)
                select pTerminal;
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region SortWarehouseInventory
    public IOrderedQueryable<WarehouseInventoryResult> SortWarehouseInventoryResults(IQueryable<WarehouseInventoryResult> query,
        SortInput<WarehouseInventorySortType> sort)
    {
      switch (sort.SortType)
      {
        case WarehouseInventorySortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case WarehouseInventorySortType.StuffCategoryName:
          return query.OrderBy(a => a.StuffCategoryName, sort.SortOrder);
        case WarehouseInventorySortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case WarehouseInventorySortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case WarehouseInventorySortType.BillOfMaterialTitle:
          return query.OrderBy(a => a.BillOfMaterialTitle, sort.SortOrder);
        case WarehouseInventorySortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case WarehouseInventorySortType.TotalAmount:
          return query.OrderBy(a => a.TotalAmount, sort.SortOrder);
        case WarehouseInventorySortType.AvailableAmount:
          return query.OrderBy(a => a.AvailableAmount, sort.SortOrder);
        case WarehouseInventorySortType.BlockedAmount:
          return query.OrderBy(a => a.BlockedAmount, sort.SortOrder);
        case WarehouseInventorySortType.QualityControlAmount:
          return query.OrderBy(a => a.QualityControlAmount, sort.SortOrder);
        case WarehouseInventorySortType.WasteAmount:
          return query.OrderBy(a => a.WasteAmount, sort.SortOrder);
        case WarehouseInventorySortType.SerialBufferAmount:
          return query.OrderBy(a => a.SerialBufferAmount, sort.SortOrder);
        case WarehouseInventorySortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case WarehouseInventorySortType.SerialStatus:
          return query.OrderBy(a => a.SerialStatus, sort.SortOrder);
        case WarehouseInventorySortType.BillOfMaterialVersion:
          return query.OrderBy(a => a.BillOfMaterialVersion, sort.SortOrder);
        case WarehouseInventorySortType.SerialProfileDateTime:
          return query.OrderBy(a => a.SerialProfileDateTime, sort.SortOrder);
        case WarehouseInventorySortType.QualityControlDescription:
          return query.OrderBy(a => a.QualityControlDescription, sort.SortOrder);
        case WarehouseInventorySortType.SerialProfileCode:
          return query.OrderBy(a => a.SerialProfileCode, sort.SortOrder);
        case WarehouseInventorySortType.WarehouseEnterTime:
          return query.OrderBy(a => a.WarehouseEnterTime, sort.SortOrder);
        case WarehouseInventorySortType.IssueUserFullName:
          return query.OrderBy(a => a.IssueUserFullName, sort.SortOrder);
        case WarehouseInventorySortType.IssueConfirmerUserFullName:
          return query.OrderBy(a => a.IssueConfirmerUserFullName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<WarehouseEnterExitResult> SearchWarehouseEnterExitResults(IQueryable<WarehouseEnterExitResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = query.Where(item =>
            item.StuffName.Contains(searchText) ||
            item.StuffCode.Contains(searchText) ||
            item.FromWarehouseName.Contains(searchText) ||
            item.ToWarehouseName.Contains(searchText) ||
            item.EmployeeFullName.Contains(searchText)
            );
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    public IQueryable<WarehousePriceReportResult> SearchWarehousePriceReportResults(IQueryable<WarehousePriceReportResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = query.Where(item =>
            item.StuffName.Contains(searchText) ||
            item.StuffCode.Contains(searchText) ||
            item.WarehouseName.Contains(searchText) ||
            item.StuffCategoryName.Contains(searchText) ||
            item.CurrencyTitle.Contains(searchText) ||
            item.StuffCategoryName.Contains(searchText)
            );
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    public IOrderedQueryable<WarehouseEnterExitResult> SortWarehouseEnterExitResults(IQueryable<WarehouseEnterExitResult> query,
       SortInput<WarehouseEnterExitReportSortType> sort)
    {
      switch (sort.SortType)
      {
        case WarehouseEnterExitReportSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sort.SortOrder);
        case WarehouseEnterExitReportSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sort.SortOrder);
        case WarehouseEnterExitReportSortType.Serial:
          return query.OrderBy(i => i.Serial, sort.SortOrder);
        case WarehouseEnterExitReportSortType.FromWarehouseName:
          return query.OrderBy(i => i.FromWarehouseName, sort.SortOrder);
        case WarehouseEnterExitReportSortType.ToWarehouseName:
          return query.OrderBy(i => i.Serial, sort.SortOrder);
        case WarehouseEnterExitReportSortType.ToDepartmentName:
          return query.OrderBy(i => i.ToDepartmentName, sort.SortOrder);
        case WarehouseEnterExitReportSortType.ToEmployeeName:
          return query.OrderBy(i => i.ToEmployeeName, sort.SortOrder);
        case WarehouseEnterExitReportSortType.Amount:
          return query.OrderBy(i => i.Amount, sort.SortOrder);
        case WarehouseEnterExitReportSortType.UnitName:
          return query.OrderBy(i => i.UnitName, sort.SortOrder);
        case WarehouseEnterExitReportSortType.DateTime:
          return query.OrderBy(i => i.DateTime, sort.SortOrder);
        case WarehouseEnterExitReportSortType.UserName:
          return query.OrderBy(i => i.UserName, sort.SortOrder);
        case WarehouseEnterExitReportSortType.EmployeeFullName:
          return query.OrderBy(i => i.EmployeeFullName, sort.SortOrder);
        case WarehouseEnterExitReportSortType.BillOfMaterialVersion:
          return query.OrderBy(i => i.BillOfMaterialVersion, sort.SortOrder);
        case WarehouseEnterExitReportSortType.Status:
          return query.OrderBy(i => i.Status, sort.SortOrder);
        case WarehouseEnterExitReportSortType.StuffNone:
          return query.OrderBy(i => i.StuffNone, sort.SortOrder);
        case WarehouseEnterExitReportSortType.TransactionLevel:
          return query.OrderBy(i => i.TransactionLevel, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException("Not implement order by for this propery on WarehouseEnterExitResult");
      }
    }
    public IOrderedQueryable<WarehousePriceReportResult> SortWarehousePriceReportResults(IQueryable<WarehousePriceReportResult> query,
       SortInput<WarehousePriceReportSortType> sort)
    {
      switch (sort.SortType)
      {
        case WarehousePriceReportSortType.WarehouseName:
          return query.OrderBy(i => i.WarehouseName, sort.SortOrder);
        case WarehousePriceReportSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sort.SortOrder);
        case WarehousePriceReportSortType.StuffCategoryName:
          return query.OrderBy(i => i.StuffCategoryName, sort.SortOrder);
        case WarehousePriceReportSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sort.SortOrder);
        case WarehousePriceReportSortType.TotalAmount:
          return query.OrderBy(i => i.TotalAmount, sort.SortOrder);
        case WarehousePriceReportSortType.UnitName:
          return query.OrderBy(i => i.UnitName, sort.SortOrder);
        case WarehousePriceReportSortType.StuffLastTransactionDateTime:
          return query.OrderBy(i => i.StuffLastTransactionDateTime, sort.SortOrder);
        case WarehousePriceReportSortType.LastStuffPrice:
          return query.OrderBy(i => i.LastStuffPrice, sort.SortOrder);
        case WarehousePriceReportSortType.LastStuffPriceDateTime:
          return query.OrderBy(i => i.LastStuffPriceDateTime, sort.SortOrder);
        case WarehousePriceReportSortType.TotalAmountPrice:
          return query.OrderBy(i => i.TotalAmountPrice, sort.SortOrder);
        case WarehousePriceReportSortType.CurrencyTitle:
          return query.OrderBy(i => i.CurrencyTitle, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException("Not supported WarehousePriceReportResult sort type");
      }
    }
    #endregion
    #region GetTransactionSummaries
    public IQueryable<TransactionSummaryResult> GetTransactionSummaries(
        int stuffId,
        DateTime fromEffectDateTime,
        DateTime toEffectDateTime)
    {

      #region transaction Query
      var warehouse = App.Internals.WarehouseManagement;
      var baseTransactions = warehouse.GetBaseTransactions(
                    stuffId: stuffId,
                    selector: warehouse.ToBaseTransactionMinResult);
      var transactionPlans = warehouse.GetTransactionPlans(
                    selector: e => new { e.Id, e.IsDelete },
                    stuffId: stuffId);
      var transactions = from baseTransaction in baseTransactions
                         join tp in transactionPlans on baseTransaction.Id equals tp.Id into tempTransactionPlans
                         from transactionPlan in tempTransactionPlans.DefaultIfEmpty()
                         let isDelete = (bool?)transactionPlan.IsDelete
                         where isDelete != true && baseTransaction.TransactionLevel != TransactionLevel.Waste
                         select baseTransaction;
      var transactionQuery = from transaction in transactions
                               //فقط در تراکنش های از نوع برنامه مصرف  ضریب معیوبی کالا در مقدار مصرف اعمال  می شود
                             let faultyPercentage = transaction.TransactionTypeId == Models.StaticData.StaticTransactionTypes.ImportConsumPlan.Id ? transaction.StuffFaultyPercentage : 0
                             let transactionBatchId = transaction.ReferenceTransactionId == null ? transaction.TransactionBatchId : transaction.ReferenceTransactionTransactionBatchId
                             select new
                             {
                               StuffId = transaction.StuffId,
                               EffectDateTime = transaction.EffectDateTime,
                               TransactionLevel = transaction.TransactionLevel,
                               //فقط در تراکنش های از نوع برنامه مصرف  ضریب معیوبی کالا در مقدار مصرف اعمال  می شود
                               Value = Math.Floor(transaction.Value * (100 + faultyPercentage) / 100)
                             };
      #endregion
      #region DateGroupQuery
      var dateGroupQuery = from transaction in transactionQuery
                           group transaction by new
                           {
                             StuffId = transaction.StuffId,
                             EffectDateTime = transaction.EffectDateTime,
                             TransactionLevel = transaction.TransactionLevel
                           }
          into gItems
                           select new
                           {
                             StuffId = gItems.Key.StuffId,
                             EffectDateTime = gItems.Key.EffectDateTime,
                             TransactionLevel = gItems.Key.TransactionLevel,
                             Value = gItems.Sum(i => i.Value)
                           };
      #endregion
      #region RunningTotalQuery
      var runningTotalQuery = from currentItem in dateGroupQuery
                              join preItem in dateGroupQuery on
                                        new { currentItem.StuffId, TransactionLevel = currentItem.TransactionLevel } equals
                                        new { preItem.StuffId, TransactionLevel = preItem.TransactionLevel }
                              where preItem.EffectDateTime <= currentItem.EffectDateTime
                              group preItem by currentItem
          into gItems
                              select new
                              {
                                StuffId = gItems.Key.StuffId,
                                EffectDateTime = gItems.Key.EffectDateTime,
                                TransactionLevel = gItems.Key.TransactionLevel,
                                Value = gItems.Key.Value,
                                Total = gItems.Sum(i => i.Value)
                              };
      #endregion
      #region GetMainUnits
      var units = App.Internals.ApplicationBase.GetUnits(
              selector: i => new
              {
                Id = i.Id,
                Name = i.Name,
                UnitTypeId = i.UnitTypeId
              },
              isMainUnit: true);
      #endregion
      #region GetStuffs
      var stuffs = App.Internals.SaleManagement.GetStuffs(
              selector: i => new
              {
                Id = i.Id,
                Name = i.Name,
                Code = i.Code,
                UnitTypeId = i.UnitTypeId,
                StuffCategoryName = i.StuffCategory.Name
              },
              id: stuffId);
      #endregion
      #region ToResult
      var result = from stuff in stuffs
                   join rtItem in runningTotalQuery on stuff.Id equals rtItem.StuffId
                   join unit in units on stuff.UnitTypeId equals unit.UnitTypeId
                   select new TransactionSummaryResult
                   {
                     EffectDateTime = rtItem.EffectDateTime,
                     TransactionLevel = rtItem.TransactionLevel,
                     StuffId = stuff.Id,
                     StuffCategoryName = stuff.StuffCategoryName,
                     StuffCode = stuff.Code,
                     StuffName = stuff.Name,
                     Amount = rtItem.Value,
                     TotalAmount = rtItem.Total,
                     UnitId = unit.Id,
                     UnitName = unit.Name
                   };
      #endregion
      return result;
    }
    #endregion
    #region SortTransactionSummary
    public IOrderedQueryable<TransactionSummaryResult> SortTransactionSummary(IQueryable<TransactionSummaryResult> query,
        SortInput<TransactionSummarySortType> sort)
    {
      switch (sort.SortType)
      {
        case TransactionSummarySortType.EffectDateTime:
          return query.OrderBy(a => a.EffectDateTime, sort.SortOrder);
        case TransactionSummarySortType.TransactionLevel:
          return query.OrderBy(a => a.TransactionLevel, sort.SortOrder);
        case TransactionSummarySortType.StuffCategoryName:
          return query.OrderBy(a => a.StuffCategoryName, sort.SortOrder);
        case TransactionSummarySortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case TransactionSummarySortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case TransactionSummarySortType.Amount:
          return query.OrderBy(a => a.Amount, sort.SortOrder);
        case TransactionSummarySortType.TotalAmount:
          return query.OrderBy(a => a.TotalAmount, sort.SortOrder);
        case TransactionSummarySortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    internal Expression<Func<BaseTransaction, WarehouseTransactionResult>> ToWarehouseTransactionResult =
        baseTransaction => new WarehouseTransactionResult
        {
          Id = baseTransaction.Id,
          WarehouseId = baseTransaction.WarehouseId,
          WarehouseName = baseTransaction.Warehouse.Name,
          TransnsactionBatchId = baseTransaction.TransactionBatchId,
          BaseEntityId = baseTransaction.TransactionBatch.BaseEntity.Id,
          Amount = baseTransaction.Amount,
          StuffId = baseTransaction.StuffId,
          StuffName = baseTransaction.Stuff.Name,
          StuffCode = baseTransaction.Stuff.Code,
          BillOfMaterialVersion = baseTransaction.BillOfMaterialVersion,
          DateTime = baseTransaction.TransactionBatch.DateTime,
          EffectDateTime = baseTransaction.EffectDateTime,
          StuffSerialCode = baseTransaction.StuffSerialCode,
          Serial = baseTransaction.StuffSerial.Serial,
          UnitId = baseTransaction.UnitId,
          UnitName = baseTransaction.Unit.Name,
          UserId = baseTransaction.TransactionBatch.UserId,
          UserName = baseTransaction.TransactionBatch.User.UserName,
          EmployeeFullName = baseTransaction.TransactionBatch.User.Employee.FirstName + " " +
                               baseTransaction.TransactionBatch.User.Employee.LastName,
          TransactionTypeId = baseTransaction.TransactionTypeId,
          TransactionTypeName = baseTransaction.TransactionType.Name,
          Description = baseTransaction.Description,
          RowVersion = baseTransaction.RowVersion
        };
    internal Expression<Func<BaseTransaction, WarehouseTransactionMinResult>> ToWarehouseTransactionMinResult =
        warehouseTransaction => new WarehouseTransactionMinResult
        {
          Id = warehouseTransaction.Id,
          WarehouseId = warehouseTransaction.WarehouseId,
          BillOfMaterialVersion = warehouseTransaction.BillOfMaterialVersion,
          EffectDateTime = warehouseTransaction.EffectDateTime,
          Amount = warehouseTransaction.Amount * warehouseTransaction.Unit.ConversionRatio,
          StuffId = warehouseTransaction.StuffId,
          StuffSerialCode = warehouseTransaction.StuffSerialCode,
          Serial = warehouseTransaction.StuffSerial.Serial,
          TransactionLevel = warehouseTransaction.TransactionType.TransactionLevel,
          TransactionTypeId = warehouseTransaction.TransactionTypeId,
          TransactionTypeFactor = warehouseTransaction.TransactionType.Factor,
          Value = warehouseTransaction.Amount * warehouseTransaction.Unit.ConversionRatio * (int)warehouseTransaction.TransactionType.Factor,
          TransactionBatchId = warehouseTransaction.TransactionBatchId
        };
    internal Expression<Func<BaseTransaction, CardexResult>> ToCardexResult =
        baseTransaction => new CardexResult
        {
          Id = baseTransaction.Id,
          WarehouseFiscalPeriodId = baseTransaction.WarehouseFiscalPeriodId,
          WarehouseFiscalPeriodName = baseTransaction.WarehouseFiscalPeriod.Name,
          WarehouseId = baseTransaction.WarehouseId,
          WarehouseName = baseTransaction.Warehouse.Name,
          TransnsactionBatchId = baseTransaction.TransactionBatchId,
          Amount = baseTransaction.Amount,
          BillOfMaterialVersion = baseTransaction.BillOfMaterialVersion,
          DateTime = baseTransaction.TransactionBatch.DateTime,
          EffectDateTime = baseTransaction.EffectDateTime,
          StuffSerialCode = baseTransaction.StuffSerialCode,
          Serial = baseTransaction.StuffSerial.Serial,
          UnitName = baseTransaction.Unit.Name,
          UserName = baseTransaction.TransactionBatch.User.UserName,
          EmployeeFullName = baseTransaction.TransactionBatch.User.Employee.FirstName + " " +
                               baseTransaction.TransactionBatch.User.Employee.LastName,
          TransactionTypeId = baseTransaction.TransactionTypeId,
          TransactionTypeName = baseTransaction.TransactionType.Name,
          TransactionLevel = baseTransaction.TransactionType.TransactionLevel,
          TransactionTypeFactor = baseTransaction.TransactionType.Factor,
          Description = baseTransaction.Description,
          ReferenceTransactionId = baseTransaction.ReferenceTransactionId,
          RowVersion = baseTransaction.RowVersion,
          StuffCode = baseTransaction.Stuff.Code,
          StuffName = baseTransaction.Stuff.Name
        };
    internal Expression<Func<BaseTransaction, CardexGroupedResult>> ToCardexGroupedResult =
       baseTransaction => new CardexGroupedResult
       {
         WarehouseFiscalPeriodId = baseTransaction.WarehouseFiscalPeriodId,
         WarehouseFiscalPeriodName = baseTransaction.WarehouseFiscalPeriod.Name,
         WarehouseId = baseTransaction.WarehouseId,
         WarehouseName = baseTransaction.Warehouse.Name,
         TransnsactionBatchId = baseTransaction.TransactionBatchId,
         //Amount = baseTransaction.Amount,
         BillOfMaterialVersion = baseTransaction.BillOfMaterialVersion,
         DateTime = baseTransaction.TransactionBatch.DateTime,
         EffectDateTime = baseTransaction.EffectDateTime,
         StuffSerialCode = baseTransaction.StuffSerialCode,
         Serial = baseTransaction.StuffSerial.Serial,
         UnitName = baseTransaction.Unit.Name,
         UserName = baseTransaction.TransactionBatch.User.UserName,
         EmployeeFullName = baseTransaction.TransactionBatch.User.Employee.FirstName + " " +
                              baseTransaction.TransactionBatch.User.Employee.LastName,
         //TransactionTypeId = baseTransaction.TransactionTypeId,
         //TransactionTypeName = baseTransaction.TransactionType.Name,
         //TransactionLevel = baseTransaction.TransactionType.TransactionLevel,
         //TransactionTypeFactor = baseTransaction.TransactionType.Factor,
         //ReferenceTransactionId = baseTransaction.ReferenceTransactionId,
         AvailableAmount = baseTransaction.TransactionType.TransactionLevel == TransactionLevel.Available ? (baseTransaction.Amount * (int)baseTransaction.TransactionType.Factor) : 0,
         BlockedAmount = baseTransaction.TransactionType.TransactionLevel == TransactionLevel.Blocked ? (baseTransaction.Amount * (int)baseTransaction.TransactionType.Factor) : 0,
         PlanAmount = baseTransaction.TransactionType.TransactionLevel == TransactionLevel.Plan ? (baseTransaction.Amount * (int)baseTransaction.TransactionType.Factor) : 0,
         QualityControlAmount = baseTransaction.TransactionType.TransactionLevel == TransactionLevel.QualityControl ? (baseTransaction.Amount * (int)baseTransaction.TransactionType.Factor) : 0,
         WasteAmount = baseTransaction.TransactionType.TransactionLevel == TransactionLevel.Waste ? (baseTransaction.Amount * (int)baseTransaction.TransactionType.Factor) : 0
       };
    #endregion
    #region CheckFIFO
    internal void CheckFIFO(
        int stuffId,
        short warehouseId,
        long stuffSerialCode,
        short transactionTypeId)
    {

      #region GetTransactionType
      var transactionType = GetTransactionType(id: transactionTypeId);
      #endregion
      if (transactionType.Factor == TransactionTypeFactor.Minus &&
          transactionType.TransactionLevel == TransactionLevel.Available)
      {
        #region GetWarehouse 
        var warehouse = GetWarehouse(id: warehouseId);
        #endregion
        if (warehouse.FIFO == true)
        {
          var stuffSerial = GetStuffSerial(
                        selector: e => new
                        {
                          Serial = e.Serial,
                          StuffCode = e.Stuff.Code,
                          SerialProfileCode = e.SerialProfile.Code
                        },
                        stuffId: stuffId,
                        code: stuffSerialCode);
          #region GetWarehouseInventories
          var warehouseInventories = GetWarehouseInventories(
                  warehouseId: warehouseId,
                  stuffId: stuffId,
                  groupBySerial: true)


              .Where(i => i.StuffSerialCode != null)
              .ToList();
          var fifoException = warehouseInventories.Any(i => i.SerialProfileCode < stuffSerial.SerialProfileCode && i.AvailableAmount > 0);
          if (fifoException)
            throw new FifoException(
                      stuffCode: stuffSerial.StuffCode,
                      warehouseName: warehouse.Name,
                      serial: stuffSerial.Serial);
          #endregion
        }
      }
    }
    #endregion
  }
}
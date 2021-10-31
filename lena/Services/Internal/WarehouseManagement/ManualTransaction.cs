using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.Common;
using lena.Models.WarehouseManagement.ManualTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {

    #region Gets
    internal IQueryable<TResult> GetManualTransactionsProcess<TResult>(
        Expression<Func<ManualTransaction, TResult>> selector,
        TValue<int> id = null,
        TValue<DateTime?> fromDateTime = null,
        TValue<DateTime?> toDateTime = null,
        TValue<string> description = null,
        TValue<int?> stuffId = null,
        TValue<int?> qty = null,
        TValue<int?> warehouseId = null
        )
    {

      var manualTransactionQuery = GetManualTransactions(
                    selector: e => e,
                    id: id,
                    fromDateTime: fromDateTime,
                    toDateTime: toDateTime,
                    description: description,
                    stuffId: stuffId,
                    qty: qty,
                    warehouseId: warehouseId
                    );
      var manualTransactions = manualTransactionQuery.OfType<ManualTransaction>();
      //if (warehouseId != null)
      //    manualTransactions = manualTransactions.Where(i => i.WarehouseId == warehouseId);
      return manualTransactions.Select(selector);
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetManualTransactions<TResult>(
        Expression<Func<ManualTransaction, TResult>> selector,
        TransactionBatch transactionBatch = null,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<DateTime?> dateTime = null,
        TValue<DateTime?> fromDateTime = null,
        TValue<DateTime?> toDateTime = null,
        TValue<string> description = null,
        TValue<int?> billOfMaterialVersion = null,
        TValue<int?> stuffId = null,
        TValue<int?> qty = null,
        TValue<int> unitId = null,
        TValue<int> transactionBatchId = null,
        TValue<int?> warehouseId = null
        )
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var manualTransaction = baseQuery.OfType<ManualTransaction>();
      if (stuffId != null)
        manualTransaction = manualTransaction.Where(r => r.StuffId == stuffId);
      if (qty != null)
        manualTransaction = manualTransaction.Where(r => r.Qty == qty);
      if (warehouseId != null)
        manualTransaction = manualTransaction.Where(r => r.WarehouseId == warehouseId);
      if (fromDateTime != null)
        manualTransaction = manualTransaction.Where(r => r.DateTime >= fromDateTime);
      if (toDateTime != null)
        manualTransaction = manualTransaction.Where(r => r.DateTime <= toDateTime);
      return manualTransaction.Select(selector);
    }

    #endregion
    #region ToResult
    internal Expression<Func<ManualTransaction, ManualTransactionResult>> ToManualTransactionResult =
      manualTransaction => new ManualTransactionResult
      {
        Id = manualTransaction.Id,
        WarehouseId = manualTransaction.WarehouseId,
        WarehouseName = manualTransaction.Warehouse.Name,
        BillOfMaterialVersion = manualTransaction.BillOfMaterialVersion,
        DateTime = manualTransaction.DateTime,
        UnitName = manualTransaction.Unit.Name,
        UserName = manualTransaction.TransactionBatch.User.UserName,
        EmployeeFullName = manualTransaction.User.Employee.FirstName + " " +
                             manualTransaction.User.Employee.LastName,
        Description = manualTransaction.Description,
        RowVersion = manualTransaction.RowVersion,
        StuffCode = manualTransaction.Stuff.Code,
        StuffName = manualTransaction.Stuff.Name,
        StuffId = manualTransaction.StuffId,
        Qty = manualTransaction.Qty,
      };
    #endregion
    #region Search

    public IQueryable<ManualTransactionResult> SearchManualTransactionsResults(
        IQueryable<ManualTransactionResult> query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems)
    {
      System.Globalization.PersianCalendar persianCalandar =
                              new System.Globalization.PersianCalendar();
      if (searchText == null)
      {
        if (advanceSearchItems.Any())
        {
          query = query.Where(advanceSearchItems);
        }
        return query;
      }
      else
      {
        if (searchText.Trim() != "")
          query = from pTerminal in query
                  where
                        pTerminal.WarehouseName.Contains(searchText) ||
                        pTerminal.EmployeeFullName.Contains(searchText) ||
                        pTerminal.UnitName.Contains(searchText) ||
                        pTerminal.StuffCode.Contains(searchText) ||
                        pTerminal.StuffName.Contains(searchText) ||
                        pTerminal.Description.Contains(searchText) ||
                        pTerminal.Id.ToString().Contains(searchText) ||
                        pTerminal.Qty.ToString().Contains(searchText) ||
                        pTerminal.DateTime.ToString().Contains(searchText)
                  select pTerminal;

        if (advanceSearchItems.Any())
        {
          query = query.Where(advanceSearchItems);
        }
        return query;
      }

    }

    #endregion
    #region SortManualTransactionResult
    public IOrderedQueryable<ManualTransactionResult> SortManualTransactionResults(IQueryable<ManualTransactionResult> query,
        SortInput<ManualTransactionSortType> sort)
    {
      switch (sort.SortType)
      {
        case ManualTransactionSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ManualTransactionSortType.ReferenceTransactionId:
          return query.OrderBy(a => a.ReferenceTransactionId, sort.SortOrder);
        case ManualTransactionSortType.TransnsactionBatchId:
          return query.OrderBy(a => a.TransnsactionBatchId, sort.SortOrder);
        case ManualTransactionSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case ManualTransactionSortType.Amount:
          return query.OrderBy(a => a.Amount, sort.SortOrder);
        case ManualTransactionSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case ManualTransactionSortType.EffectDateTime:
          return query.OrderBy(a => a.EffectDateTime, sort.SortOrder);
        case ManualTransactionSortType.StuffSerialCode:
          return query.OrderBy(a => a.StuffSerialCode, sort.SortOrder);
        case ManualTransactionSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case ManualTransactionSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case ManualTransactionSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case ManualTransactionSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case ManualTransactionSortType.UserName:
          return query.OrderBy(a => a.UserName, sort.SortOrder);
        case ManualTransactionSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case ManualTransactionSortType.TransactionTypeName:
          return query.OrderBy(a => a.TransactionTypeName, sort.SortOrder);
        case ManualTransactionSortType.TransactionLevel:
          return query.OrderBy(a => a.TransactionLevel, sort.SortOrder);
        case ManualTransactionSortType.TransactionTypeFactor:
          return query.OrderBy(a => a.TransactionTypeFactor, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }


    #endregion
    #region AddProcess
    public void AddManualTransactionProcess(AddManualTransactionDetailInput[] addManualTransactionDetailArray)
    {

      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      var transactionBatch = warehouseManagement.AddTransactionBatch();
      #endregion

      var warehouseIds = new List<int>();
      foreach (var item in addManualTransactionDetailArray)
      {
        if (!warehouseIds.Contains(item.WarehouseId))
          warehouseIds.Add(item.WarehouseId);
        #region AddManualTransaction
        var manualTransaction = AddManualTransaction(
            transactionBatch: transactionBatch,
            stuffId: item.StuffId,
            billOfMaterialVersion: item.BillOfMaterialVersion,
            providerId: item.ProviderId,
            warehouseId: item.WarehouseId,
            unitId: item.UnitId,
            qty: item.Qty,
            qtyPerBox: item.QtyPerBox);

        #endregion
        #region Get stuff details
        var stuff = App.Internals
                        .SaleManagement
                        .GetStuff(id: item.StuffId);
        #endregion
        StuffSerial[] stuffSerials = new StuffSerial[0];
        if (stuff.IsTraceable)
        {
          #region AddSerialProfile
          var serialProfile = AddSerialProfile(
         serialProfile: null,
         stuffId: item.StuffId,
         cooperatorId: item.ProviderId);
          #endregion
          #region AddStuffSerials
          stuffSerials = AddStuffSerials(
              selector: e => e,
              productionOrderId: null,
              serialProfile: serialProfile,
              partitionStuffSerialId: null,
              stuffId: item.StuffId,
              billOfMaterialVersion: item.BillOfMaterialVersion,
              qty: item.Qty,
              unitId: item.UnitId,
              isPacking: false,
              boxCount: null,
              qtyPerBox: item.QtyPerBox,
              warehouseEnterTime: DateTime.UtcNow,
              issueUserId: App.Providers.Security.CurrentLoginData.UserId,
              issueConfirmerUserId: App.Providers.Security.CurrentLoginData.UserId)

          .ToArray();
          #endregion
        }
        #region AddTransactions
        var unit = App.Internals.ApplicationBase.GetUnit(id: item.UnitId);
        var index = 0;
        var remainQty = item.Qty * unit.ConversionRatio;
        var boxValue = item.QtyPerBox * unit.ConversionRatio;
        while (remainQty > 0)
        {

          var qty = Math.Min(remainQty, boxValue);
          var stuffSerialCode = ResolveStuffSerialCode(stuff: stuff, index: index, stuffSerials: stuffSerials);
          #region Add ImportAvailableTransaction
          var importAvailableTransaction = App.Internals.WarehouseManagement
              .AddWarehouseTransaction(
                  transactionBatchId: transactionBatch.Id,
                  effectDateTime: transactionBatch.DateTime,
                  stuffId: item.StuffId,
                  billOfMaterialVersion: item.BillOfMaterialVersion,
                  stuffSerialCode: stuffSerialCode,
                  warehouseId: item.WarehouseId,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAvailable.Id,
                  amount: qty / unit.ConversionRatio,
                  unitId: item.UnitId,
                  description: null,
                  referenceTransaction: null);
          #endregion

          if (qty == boxValue)
            index++;
          remainQty = remainQty - qty;
        }
        #endregion
      }
    }
    #endregion

    #region Add
    public ManualTransaction AddManualTransaction(
       TransactionBatch transactionBatch,
       int stuffId,
       int? billOfMaterialVersion,
       int providerId,
       short warehouseId,
       byte unitId,
       int qty,
       int qtyPerBox)
    {

      var manualTransaction = repository.Create<ManualTransaction>();
      manualTransaction.BillOfMaterialVersion = billOfMaterialVersion;
      manualTransaction.StuffId = stuffId;
      manualTransaction.ProviderId = providerId;
      manualTransaction.WarehouseId = warehouseId;
      manualTransaction.UnitId = unitId;
      manualTransaction.Qty = qty;
      manualTransaction.QtyPerBox = qtyPerBox;


      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: manualTransaction,
                    transactionBatch: transactionBatch);
      return manualTransaction;
    }
    #endregion

  }
}

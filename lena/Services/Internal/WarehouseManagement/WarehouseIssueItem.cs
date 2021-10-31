using System;
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
using lena.Models.WarehouseManagement.WarehouseIssueItem;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public WarehouseIssueItem AddWarehouseIssueItem(
        WarehouseIssueItem warehouseIssueItem,
        TransactionBatch transactionBatch,
        TransactionLevel transactionLevel,
        int warehouseIssueId,
        int stuffId,
        long? stuffSerialCode,
        double amount,
        byte unitId,
        string assetCode,
        string description)
    {
      var billOfMaterialVersion = GetBillOfMaterialVersionOfSerial(
                    stuffSerialCode: stuffSerialCode,
                    stuffId: stuffId);
      warehouseIssueItem = warehouseIssueItem ?? repository.Create<WarehouseIssueItem>();
      warehouseIssueItem.WarehouseIssueId = warehouseIssueId;
      warehouseIssueItem.StuffId = stuffId;
      warehouseIssueItem.BillOfMaterialVersion = billOfMaterialVersion;
      warehouseIssueItem.Amount = amount;
      warehouseIssueItem.UnitId = unitId;
      warehouseIssueItem.TransactionLevel = transactionLevel;
      warehouseIssueItem.AssetCode = assetCode;
      warehouseIssueItem.StuffSerialCode = stuffSerialCode;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: warehouseIssueItem,
                    transactionBatch: transactionBatch,
                    description: description);
      return warehouseIssueItem;
    }
    #endregion
    #region Edit
    public WarehouseIssueItem EditWarehouseIssueItem(
        int id,
        byte[] rowVersion,
        TValue<int> warehouseIssueId = null,
        TValue<int> stuffId = null,
        TValue<int?> billOfMaterialVersion = null,
        TValue<long?> stuffSerialCode = null,
        TValue<double> amount = null,
        TValue<byte> unitId = null,
        TValue<string> description = null)
    {
      var warehouseIssueItem = GetWarehouseIssueItem(id: id);
      return EditWarehouseIssueItem(
                    warehouseIssueItem: warehouseIssueItem,
                    rowVersion: rowVersion,
                    warehouseIssueId: warehouseIssueId,
                    stuffId: stuffId,
                    stuffSerialCode: stuffSerialCode,
                    amount: amount,
                    unitId: unitId,
                    description: description);
    }
    public WarehouseIssueItem EditWarehouseIssueItem(
        WarehouseIssueItem warehouseIssueItem,
        byte[] rowVersion,
        TValue<int> warehouseIssueId = null,
        TValue<int> stuffId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<double> amount = null,
        TValue<byte> unitId = null,
        TValue<string> description = null)
    {
      if (warehouseIssueId != null)
        warehouseIssueItem.WarehouseIssueId = warehouseIssueId;
      if (stuffId != null)
        warehouseIssueItem.StuffId = stuffId;
      if (stuffSerialCode != null)
        warehouseIssueItem.StuffSerialCode = stuffSerialCode;
      if (stuffId != null || stuffSerialCode != null)
      {
        var billOfMaterialVersion = GetBillOfMaterialVersionOfSerial(
                      stuffSerialCode: warehouseIssueItem.StuffSerialCode,
                      stuffId: warehouseIssueItem.StuffId);
        warehouseIssueItem.BillOfMaterialVersion = billOfMaterialVersion;
      }
      if (amount != null)
        warehouseIssueItem.Amount = amount;
      if (unitId != null)
        warehouseIssueItem.UnitId = unitId;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: warehouseIssueItem,
                    rowVersion: rowVersion,
                    description: description);
      return retValue as WarehouseIssueItem;
    }
    #endregion
    #region Get
    public WarehouseIssueItem GetWarehouseIssueItem(int id) => GetWarehouseIssueItem(selector: e => e, id: id);
    public TResult GetWarehouseIssueItem<TResult>(
        Expression<Func<WarehouseIssueItem, TResult>> selector,
        int id)
    {
      var result = GetWarehouseIssueItems(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new WarehouseIssueItemNotFoundException(id);
      return result;
    }
    public WarehouseIssueItem GetWarehouseIssueItem(string code) => GetWarehouseIssueItem(selector: e => e, code: code);
    public TResult GetWarehouseIssueItem<TResult>(
        Expression<Func<WarehouseIssueItem, TResult>> selector,
        string code)
    {
      var result = GetWarehouseIssueItems(
                selector: selector,
                code: code).FirstOrDefault();
      if (result == null)
        throw new WarehouseIssueItemNotFoundException(code);
      return result;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetWarehouseIssueItems<TResult>(
        Expression<Func<WarehouseIssueItem, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> warehouseIssueId = null,
        TValue<int> stuffId = null,
        TValue<int?> billOfMaterialVersion = null,
        TValue<long?> stuffSerialCode = null,
        TValue<double> amount = null,
        TValue<int> unitId = null,
        TValue<string> serial = null,
        TValue<int> stuffCategoryId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<string> warehouseIssueCode = null,
        TValue<DateTime> warehouseIssueFromDateTime = null,
        TValue<int> fromWarehouseId = null,
        TValue<int?> toWarehouseId = null,
        TValue<WarehouseIssueStatusType> status = null,
        TValue<int?> toEmployeeId = null,
        TValue<int?> toDepartmentId = null,
        TValue<DateTime> warehouseIssueToDateTime = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description,
                    fromDateTime: fromDateTime,
                    toDateTime: toDateTime);
      var warehouseIssueItem = baseQuery.OfType<WarehouseIssueItem>();
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial);
        var serialInfo = App.Internals.WarehouseManagement.GetStuffSerial(serial);
        warehouseIssueItem = warehouseIssueItem.Where(x => x.StuffId == serialInfo.StuffId && x.StuffSerialCode == serialInfo.Code);
        //warehouseIssueItem = warehouseIssueItem.Where(r => r.StuffSerial.Serial == serial);
      }
      if (warehouseIssueId != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.WarehouseIssueId == warehouseIssueId);
      if (stuffId != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.StuffId == stuffId);
      if (stuffCategoryId != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.Stuff.StuffCategoryId == stuffCategoryId);
      if (billOfMaterialVersion != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.BillOfMaterialVersion == billOfMaterialVersion);
      if (stuffSerialCode != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.StuffSerialCode == stuffSerialCode);
      if (amount != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.Amount == amount);
      if (unitId != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.UnitId == unitId);
      if (toWarehouseId != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.WarehouseIssue.ToWarehouseId == toWarehouseId);
      if (fromWarehouseId != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.WarehouseIssue.FromWarehouseId == fromWarehouseId);
      if (status != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.WarehouseIssue.Status == status);
      if (toEmployeeId != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.WarehouseIssue.ToEmployeeId == toEmployeeId);
      if (toDepartmentId != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.WarehouseIssue.ToDepartmentId == toDepartmentId);
      if (warehouseIssueCode != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.WarehouseIssue.Code == warehouseIssueCode);
      if (warehouseIssueFromDateTime != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.WarehouseIssue.DateTime >= warehouseIssueFromDateTime);
      if (warehouseIssueToDateTime != null)
        warehouseIssueItem = warehouseIssueItem.Where(r => r.WarehouseIssue.DateTime <= warehouseIssueToDateTime);
      return warehouseIssueItem.Select(selector);
    }
    #endregion
    #region AddDirectProcess
    public WarehouseIssueItem AddDirectWarehouseIssueItemProcess(
        int warehouseIssueId,
        int stuffId,
        string serial,
        long? stuffSerialCode,
        double amount,
        byte unitId,
        string description,
        string assetCode,
        bool isRequestWarehouseIssue = false,
        BaseTransaction referenceTransaction = null
        )
    {
      var transactionLevel = TransactionLevel.Available;
      var warehouseInventory = App.Internals.WarehouseManagement.GetStuffSerialInventories(
                    stuffId: stuffId,
                    stuffSerialCode: stuffSerialCode)
                .FirstOrDefault();
      if (isRequestWarehouseIssue && warehouseInventory.AvailableAmount == 0)
      {
        throw new RequestWarehouseIssueNotAvailableException(serial);
      }
      if (warehouseInventory.AvailableAmount != 0)
      {
        transactionLevel = TransactionLevel.Available;
      }
      if (warehouseInventory.QualityControlAmount != 0)
      {
        transactionLevel = TransactionLevel.QualityControl;
      }
      if (warehouseInventory.WasteAmount != 0)
      {
        transactionLevel = TransactionLevel.Waste;
      }
      #region AddWarehouseIssueItem
      var warehouseIssueItem = AddWarehouseIssueItem(
              warehouseIssueItem: null,
              transactionBatch: null,
              transactionLevel: transactionLevel,
              warehouseIssueId: warehouseIssueId,
              stuffId: stuffId,
              stuffSerialCode: stuffSerialCode,
              assetCode: assetCode,
              amount: amount,
              unitId: unitId,
              description: description);
      #endregion
      #region AddWarehouseTransactions
      #region Add ExportAsDirectStoreIssueTransaction
      var transactionTypeExportId = Models.StaticData.StaticTransactionTypes.ExportAsDirectStoreIssue.Id;
      if (transactionLevel == TransactionLevel.QualityControl)
        transactionTypeExportId = Models.StaticData.StaticTransactionTypes.ExportQualityControl.Id;
      if (transactionLevel == TransactionLevel.Waste)
        transactionTypeExportId = Models.StaticData.StaticTransactionTypes.ExportWaste.Id;
      var exportAccessTransaction = AddWarehouseTransaction(
                    transactionBatchId: warehouseIssueItem.WarehouseIssue.TransactionBatch.Id,
                    effectDateTime: warehouseIssueItem.WarehouseIssue.TransactionBatch.DateTime,
                    stuffId: stuffId,
                    billOfMaterialVersion: warehouseIssueItem.BillOfMaterialVersion,
                    stuffSerialCode: stuffSerialCode,
                    warehouseId: warehouseIssueItem.WarehouseIssue.FromWarehouseId,
                    transactionTypeId: transactionTypeExportId,
                    amount: amount,
                    unitId: unitId,
                    description: description,
                    referenceTransaction: referenceTransaction,
                    checkFIFO: true);
      #endregion
      #region Add ImportAsDirectStoreIssue
      var transactionTypeImportId = Models.StaticData.StaticTransactionTypes.ImportAsDirectStoreIssue.Id;
      if (transactionLevel == TransactionLevel.QualityControl)
        transactionTypeImportId = Models.StaticData.StaticTransactionTypes.ImportQualityControl.Id;
      if (transactionLevel == TransactionLevel.Waste)
        transactionTypeImportId = Models.StaticData.StaticTransactionTypes.ImportWaste.Id;
      if (warehouseIssueItem.WarehouseIssue.ToWarehouseId != null)
      {
        var importAccessTransaction = AddWarehouseTransaction(
                        transactionBatchId: warehouseIssueItem.WarehouseIssue.TransactionBatch.Id,
                        effectDateTime: warehouseIssueItem.WarehouseIssue.TransactionBatch.DateTime,
                        stuffId: stuffId,
                        billOfMaterialVersion: warehouseIssueItem.BillOfMaterialVersion,
                        stuffSerialCode: stuffSerialCode,
                        warehouseId: warehouseIssueItem.WarehouseIssue.ToWarehouseId.Value,
                        transactionTypeId: transactionTypeImportId,
                        amount: amount,
                        unitId: unitId,
                        description: description,
                        referenceTransaction: exportAccessTransaction);
      }
      #endregion
      #endregion
      return warehouseIssueItem;
    }
    #endregion
    #region AddIndirectProcess
    public WarehouseIssueItem AddIndirectWarehouseIssueItemProcess(
        int warehouseIssueId,
        int stuffId,
        long? stuffSerialCode,
        string serial,
        double amount,
        byte unitId,
        string description,
        string assetCode,
        bool isRequestWarehouseIssue = false,
        BaseTransaction referenceTransaction = null)
    {
      var transactionLevel = TransactionLevel.Available;
      var warehouseInventory = App.Internals.WarehouseManagement.GetStuffSerialInventories(
                    stuffId: stuffId,
                    stuffSerialCode: stuffSerialCode,
                    serial: serial)
                .FirstOrDefault();
      if (isRequestWarehouseIssue && warehouseInventory.AvailableAmount == 0)
      {
        throw new RequestWarehouseIssueNotAvailableException(serial);
      }
      if (warehouseInventory.AvailableAmount != 0)
      {
        transactionLevel = TransactionLevel.Available;
      }
      if (warehouseInventory.QualityControlAmount != 0)
      {
        transactionLevel = TransactionLevel.QualityControl;
      }
      if (warehouseInventory.WasteAmount != 0)
      {
        transactionLevel = TransactionLevel.Waste;
      }
      #region AddWarehouseIssueItem
      var warehouseIssueItem = AddWarehouseIssueItem(
          transactionLevel: transactionLevel,
          warehouseIssueItem: null,
          transactionBatch: null,
          warehouseIssueId: warehouseIssueId,
          stuffId: stuffId,
          stuffSerialCode: stuffSerialCode,
          amount: amount,
          unitId: unitId,
          assetCode: assetCode,
          description: description);
      #endregion
      #region AddWarehouseTransactions
      #region Add ExportAsIndirectStoreIssueTransaction
      var transactionTypeExportId = Models.StaticData.StaticTransactionTypes.ExportAsIndirectStoreIssue.Id;
      if (transactionLevel == TransactionLevel.QualityControl)
        transactionTypeExportId = Models.StaticData.StaticTransactionTypes.ExportQualityControl.Id;
      if (transactionLevel == TransactionLevel.Waste)
        transactionTypeExportId = Models.StaticData.StaticTransactionTypes.ExportWaste.Id;
      var exportAccessTransaction = AddWarehouseTransaction(
                     transactionBatchId: warehouseIssueItem.WarehouseIssue.TransactionBatch.Id,
                     effectDateTime: warehouseIssueItem.WarehouseIssue.TransactionBatch.DateTime,
                     stuffId: stuffId,
                     billOfMaterialVersion: warehouseIssueItem.BillOfMaterialVersion,
                     stuffSerialCode: stuffSerialCode,
                     warehouseId: warehouseIssueItem.WarehouseIssue.FromWarehouseId,
                     transactionTypeId: transactionTypeExportId,
                     amount: amount,
                     unitId: unitId,
                     description: description,
                     referenceTransaction: referenceTransaction,
                     checkFIFO: true);
      #endregion
      #region Add ImportAsBlockedStoreIssueTransaction
      var transactionTypeImportId = Models.StaticData.StaticTransactionTypes.ImportAsBlockedStoreIssue.Id;
      var importBlockTransaction = AddWarehouseTransaction(
                      transactionBatchId: warehouseIssueItem.WarehouseIssue.TransactionBatch.Id,
                      effectDateTime: warehouseIssueItem.WarehouseIssue.TransactionBatch.DateTime,
                      stuffId: stuffId,
                      billOfMaterialVersion: warehouseIssueItem.BillOfMaterialVersion,
                      stuffSerialCode: stuffSerialCode,
                      warehouseId: warehouseIssueItem.WarehouseIssue.FromWarehouseId,
                      transactionTypeId: transactionTypeImportId,
                      amount: amount,
                      unitId: unitId,
                      description: description,
                      referenceTransaction: exportAccessTransaction);
      #endregion
      #endregion
      return warehouseIssueItem;
    }
    #endregion
    #region Search
    public IQueryable<WarehouseIssueItemResult> SearchWarehouseIssueItemResult(IQueryable<WarehouseIssueItemResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = query.Where(item =>
            item.Code.Contains(search));
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<WarehouseIssueItemResult> SortWarehouseIssueItemResult(IQueryable<WarehouseIssueItemResult> query,
        SortInput<WarehouseIssueItemSortType> sort)
    {
      switch (sort.SortType)
      {
        case WarehouseIssueItemSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case WarehouseIssueItemSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case WarehouseIssueItemSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case WarehouseIssueItemSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case WarehouseIssueItemSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case WarehouseIssueItemSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case WarehouseIssueItemSortType.BillOfMaterialVersion:
          return query.OrderBy(a => a.BillOfMaterialVersion, sort.SortOrder);
        case WarehouseIssueItemSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case WarehouseIssueItemSortType.SerialCode:
          return query.OrderBy(a => a.SerialCode, sort.SortOrder);
        case WarehouseIssueItemSortType.Amount:
          return query.OrderBy(a => a.Amount, sort.SortOrder);
        case WarehouseIssueItemSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToWarehouseIssueItemResult
    public Expression<Func<WarehouseIssueItem, WarehouseIssueItemResult>> ToWarehouseIssueItemResult =
        warehouseIssueItem => new WarehouseIssueItemResult
        {
          Id = warehouseIssueItem.Id,
          WarehouseIssueId = warehouseIssueItem.WarehouseIssueId,
          Code = warehouseIssueItem.Code,
          DateTime = warehouseIssueItem.DateTime,
          UserId = warehouseIssueItem.UserId,
          EmployeeFullName = warehouseIssueItem.User.Employee.FirstName + " " + warehouseIssueItem.User.Employee.LastName,
          StuffId = warehouseIssueItem.StuffId,
          StuffName = warehouseIssueItem.Stuff.Name,
          StuffCode = warehouseIssueItem.Stuff.Code,
          Serial = warehouseIssueItem.StuffSerial.Serial,
          SerialCode = warehouseIssueItem.StuffSerialCode,
          BillOfMaterialVersion = warehouseIssueItem.BillOfMaterialVersion,
          Amount = warehouseIssueItem.Amount,
          UnitId = warehouseIssueItem.UnitId,
          UnitName = warehouseIssueItem.Unit.Name,
          QualityControlDescription = warehouseIssueItem.StuffSerial.QualityControlDescription,
          IsDelete = warehouseIssueItem.IsDelete,
          AssetCode = warehouseIssueItem.AssetCode,
          RowVersion = warehouseIssueItem.RowVersion
        };
    #endregion
    #region ToFullWarehouseIssueItemResult
    public Expression<Func<WarehouseIssueItem, WarehouseIssueItemFullResult>> ToFullWarehouseIssueItemResult =
        warehouseIssueItem => new WarehouseIssueItemFullResult
        {
          Id = warehouseIssueItem.Id,
          WarehouseIssueId = warehouseIssueItem.WarehouseIssueId,
          Code = warehouseIssueItem.Code,
          DateTime = warehouseIssueItem.DateTime,
          UserId = warehouseIssueItem.UserId,
          EmployeeFullName = warehouseIssueItem.User.Employee.FirstName + " " + warehouseIssueItem.User.Employee.LastName,
          StuffId = warehouseIssueItem.StuffId,
          StuffName = warehouseIssueItem.Stuff.Name,
          StuffCode = warehouseIssueItem.Stuff.Code,
          Serial = warehouseIssueItem.StuffSerial.Serial,
          SerialCode = warehouseIssueItem.StuffSerialCode,
          BillOfMaterialVersion = warehouseIssueItem.BillOfMaterialVersion,
          Amount = warehouseIssueItem.Amount,
          UnitId = warehouseIssueItem.UnitId,
          UnitName = warehouseIssueItem.Unit.Name,
          QualityControlDescription = warehouseIssueItem.StuffSerial.QualityControlDescription,
          IsDelete = warehouseIssueItem.IsDelete,
          AssetCode = warehouseIssueItem.AssetCode,
          RowVersion = warehouseIssueItem.RowVersion,
          WarehouseIssueCode = warehouseIssueItem.WarehouseIssue.Code,
          WarehouseIssueDateTime = warehouseIssueItem.WarehouseIssue.DateTime,
          WarehouseIssueFromWarehouseId = warehouseIssueItem.WarehouseIssue.FromWarehouseId,
          WarehouseIssueFromWarehouseName = warehouseIssueItem.WarehouseIssue.FromWarehouse.Name,
          WarehouseIssueToWarehouseId = warehouseIssueItem.WarehouseIssue.ToWarehouseId,
          WarehouseIssueToWarehouseName = warehouseIssueItem.WarehouseIssue.ToWarehouse.Name,
          WarehouseIssueStatus = warehouseIssueItem.WarehouseIssue.Status,
          WarehouseIssueUserName = warehouseIssueItem.WarehouseIssue.User.UserName,
          WarehouseIssueEmployeeId = warehouseIssueItem.WarehouseIssue.User.Employee.Id,
          WarehouseIssueEmployeeFullName = warehouseIssueItem.WarehouseIssue.User.Employee.FirstName + " " + warehouseIssueItem.WarehouseIssue.User.Employee.LastName,
          WarehouseIssueResponseDateTime = warehouseIssueItem.WarehouseIssue.ResponseWarehouseIssue.DateTime,
          WarehouseIssueResponseUserName = warehouseIssueItem.WarehouseIssue.ResponseWarehouseIssue.User.UserName,
          WarehouseIssueResponseEmployeeFullName = warehouseIssueItem.WarehouseIssue.ResponseWarehouseIssue.User.Employee.FirstName + " " + warehouseIssueItem.WarehouseIssue.ResponseWarehouseIssue.User.Employee.LastName,
          WarehouseIssueToDepartmentId = warehouseIssueItem.WarehouseIssue.ToDepartmentId,
          WarehouseIssueToDepartmentName = warehouseIssueItem.WarehouseIssue.ToDepartment.Name,
          WarehouseIssueToEmployeeId = warehouseIssueItem.WarehouseIssue.ToEmployeeId,
          WarehouseIssueToEmployeeFullName = warehouseIssueItem.WarehouseIssue.ToEmployee.FirstName + " " + warehouseIssueItem.WarehouseIssue.ToEmployee.LastName,
          WarehouseIssueConfirmDescription = warehouseIssueItem.WarehouseIssue.Description,
          WarehouseIssueRowVersion = warehouseIssueItem.WarehouseIssue.RowVersion
        };
    #endregion
    #region SearchFull
    public IQueryable<WarehouseIssueItemFullResult> SearchFullWarehouseIssueItemResult(IQueryable<WarehouseIssueItemFullResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = query.Where(item =>
            item.WarehouseIssueCode.Contains(search));
      return query;
    }
    #endregion
    #region SortFull
    public IOrderedQueryable<WarehouseIssueItemFullResult> SortFullWarehouseIssueItemResult(IQueryable<WarehouseIssueItemFullResult> query,
        SortInput<WarehouseIssueItemSortType> sort)
    {
      switch (sort.SortType)
      {
        case WarehouseIssueItemSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case WarehouseIssueItemSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case WarehouseIssueItemSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case WarehouseIssueItemSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case WarehouseIssueItemSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case WarehouseIssueItemSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case WarehouseIssueItemSortType.BillOfMaterialVersion:
          return query.OrderBy(a => a.BillOfMaterialVersion, sort.SortOrder);
        case WarehouseIssueItemSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case WarehouseIssueItemSortType.SerialCode:
          return query.OrderBy(a => a.SerialCode, sort.SortOrder);
        case WarehouseIssueItemSortType.Amount:
          return query.OrderBy(a => a.Amount, sort.SortOrder);
        case WarehouseIssueItemSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueId:
          return query.OrderBy(a => a.WarehouseIssueId, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueCode:
          return query.OrderBy(a => a.WarehouseIssueCode, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueDateTime:
          return query.OrderBy(a => a.WarehouseIssueDateTime, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueFromWarehouseId:
          return query.OrderBy(a => a.WarehouseIssueFromWarehouseId, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueFromWarehouseName:
          return query.OrderBy(a => a.WarehouseIssueFromWarehouseName, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueToWarehouseId:
          return query.OrderBy(a => a.WarehouseIssueToWarehouseId, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueToWarehouseName:
          return query.OrderBy(a => a.WarehouseIssueToWarehouseName, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueStatus:
          return query.OrderBy(a => a.WarehouseIssueStatus, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueUserName:
          return query.OrderBy(a => a.WarehouseIssueUserName, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueEmployeeFullName:
          return query.OrderBy(a => a.WarehouseIssueEmployeeFullName, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueResponseUserName:
          return query.OrderBy(a => a.WarehouseIssueResponseUserName, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueResponseDateTime:
          return query.OrderBy(a => a.WarehouseIssueResponseDateTime, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueResponseEmployeeFullName:
          return query.OrderBy(a => a.WarehouseIssueResponseEmployeeFullName, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueToDepartmentName:
          return query.OrderBy(a => a.WarehouseIssueToDepartmentName, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueToEmployeeFullName:
          return query.OrderBy(a => a.WarehouseIssueToEmployeeFullName, sort.SortOrder);
        case WarehouseIssueItemSortType.WarehouseIssueConfirmDescription:
          return query.OrderBy(a => a.WarehouseIssueConfirmDescription, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Delete
    public WarehouseIssueItem RemoveWarehouseIssueItemProcess(
      int id,
      byte[] rowVersion)
    {
      var warehouseIssueItem = App.Internals.WarehouseManagement.GetWarehouseIssueItem(id: id); ; var warehouseIssue = App.Internals.WarehouseManagement.GetWarehouseIssue(id: warehouseIssueItem.WarehouseIssueId);
      #region Get WarehouseTransaction
      var transaction = GetWarehouseTransactions(
              selector: e => e,
              transactionBatchId: warehouseIssue.TransactionBatch.Id,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAsBlockedStoreIssue.Id,
              stuffId: warehouseIssueItem.StuffId,
              stuffSerialCodes: new long?[] { warehouseIssueItem.StuffSerialCode })
          .FirstOrDefault();
      #endregion
      var transactionLevel = warehouseIssueItem.TransactionLevel;
      #region Add ExportAsRejectedBlockedStoreIssue Transaction
      var exportBlockTransaction = AddWarehouseTransaction(
              transactionBatchId: warehouseIssue.TransactionBatch.Id,
              effectDateTime: warehouseIssue.TransactionBatch.DateTime,
              stuffId: warehouseIssueItem.StuffId,
              billOfMaterialVersion: warehouseIssueItem.BillOfMaterialVersion,
              stuffSerialCode: warehouseIssueItem.StuffSerialCode,
              warehouseId: warehouseIssue.FromWarehouseId,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportAsRejectedBlockedStoreIssue.Id,
              amount: warehouseIssueItem.Amount,
              unitId: warehouseIssueItem.UnitId,
              description: warehouseIssue.Description,
              referenceTransaction: transaction);
      #endregion
      #region Add ImportAsIndirectStoreIssue Transaction
      var transactionTypeImportId = Models.StaticData.StaticTransactionTypes.ImportAsIndirectStoreIssue.Id;
      if (transactionLevel == TransactionLevel.QualityControl)
        transactionTypeImportId = Models.StaticData.StaticTransactionTypes.ImportQualityControl.Id;
      if (transactionLevel == TransactionLevel.Waste)
        transactionTypeImportId = Models.StaticData.StaticTransactionTypes.ImportWaste.Id;
      var importAccessTransaction = AddWarehouseTransaction(
                      transactionBatchId: warehouseIssue.TransactionBatch.Id,
                      effectDateTime: warehouseIssue.TransactionBatch.DateTime,
                      stuffId: warehouseIssueItem.StuffId,
                      billOfMaterialVersion: exportBlockTransaction.BillOfMaterialVersion,
                      stuffSerialCode: warehouseIssueItem.StuffSerialCode,
                      warehouseId: warehouseIssue.FromWarehouseId,
                      transactionTypeId: transactionTypeImportId,
                      amount: warehouseIssueItem.Amount,
                      unitId: warehouseIssueItem.UnitId,
                      description: warehouseIssue.Description,
                      referenceTransaction: exportBlockTransaction);
      #endregion
      #region Remove
      var res = App.Internals.ApplicationBase.RemoveBaseEntityProcess(
          transactionBatchId: warehouseIssue.TransactionBatch.Id,
          baseEntity: warehouseIssueItem,
          rowVersion: rowVersion);
      #endregion
      #region RemoveWarehouseIssue
      var WarehouseIssues = App.Internals.WarehouseManagement.GetWarehouseIssueItems(
          selector: e => e,
          warehouseIssueId: warehouseIssueItem.WarehouseIssueId,
          isDelete: false);
      if (!WarehouseIssues.Any())
      {
        var result = App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                  transactionBatchId: warehouseIssue.TransactionBatch.Id,
                  baseEntity: warehouseIssue,
                  rowVersion: warehouseIssue.RowVersion);
      }
      #endregion
      return null;
    }
    #endregion
  }
}
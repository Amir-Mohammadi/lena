using System;
// using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.WarehouseManagement.ReturnOfSale;
using lena.Models.Common;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add

    public ReturnOfSale AddReturnOfSale(
        ReturnOfSale returnOfSale,
        TransactionBatch transactionBatch,
        int? sendProductId,
        int stuffId,
        int mainStuffId,
        long? stuffSerialCode,
        double qty,
        byte unitId,
        string description,
        string exitReceiptCode,
        ReturnOfSaleType type,
        int returnStoreReceiptId)
    {

      returnOfSale = returnOfSale ?? repository.Create<ReturnOfSale>();
      returnOfSale.SendProductId = sendProductId;
      returnOfSale.StuffId = stuffId;
      returnOfSale.MainStuffId = mainStuffId;
      returnOfSale.StuffSerialCode = stuffSerialCode;
      returnOfSale.Qty = qty;
      returnOfSale.UnitId = unitId;
      returnOfSale.Type = type;
      returnOfSale.Status = ReturnOfSaleStatus.Waiting;
      returnOfSale.ReturnStoreReceiptId = returnStoreReceiptId;
      returnOfSale.ExitReceiptCode = exitReceiptCode;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: returnOfSale,
                    transactionBatch: transactionBatch,
                    description: description);
      return returnOfSale;
    }

    #endregion

    #region Edit

    public ReturnOfSale EditReturnOfSale(
        int id,
        byte[] rowVersion,
        TValue<int?> sendProductId = null,
        TValue<int> stuffId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<string> description = null,
        TValue<ReturnOfSaleType> type = null,
        TValue<ReturnOfSaleStatus> status = null,
        TValue<int> returnStoreReceiptId = null)
    {

      var returnOfSale = GetReturnOfSale(id: id);
      return EditReturnOfSale(
                    returnOfSale: returnOfSale,
                    rowVersion: rowVersion,
                    sendProductId: sendProductId,
                    stuffId: stuffId,
                    stuffSerialCode: stuffSerialCode,
                    qty: qty,
                    unitId: unitId,
                    description: description,
                    type: type,
                    status: status,
                    returnStoreReceiptId: returnStoreReceiptId);
    }

    public ReturnOfSale EditReturnOfSale(
        ReturnOfSale returnOfSale,
        byte[] rowVersion,
        TValue<int?> sendProductId = null,
        TValue<int> stuffId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<string> description = null,
        TValue<ReturnOfSaleType> type = null,
        TValue<ReturnOfSaleStatus> status = null,
        TValue<int> returnStoreReceiptId = null)
    {

      if (sendProductId != null)
        returnOfSale.SendProductId = sendProductId;
      if (stuffId != null)
        returnOfSale.StuffId = stuffId;
      if (stuffSerialCode != null)
        returnOfSale.StuffSerialCode = stuffSerialCode;
      if (qty != null)
        returnOfSale.Qty = qty;
      if (unitId != null)
        returnOfSale.UnitId = unitId;
      if (type != null)
        returnOfSale.Type = type;
      if (status != null)
        returnOfSale.Status = status;
      if (returnStoreReceiptId != null)
        returnOfSale.ReturnStoreReceiptId = returnStoreReceiptId;

      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: returnOfSale,
                    rowVersion: rowVersion,
                    description: description);
      return retValue as ReturnOfSale;
    }

    #endregion
    #region Get

    public ReturnOfSale GetReturnOfSale(int id) => GetReturnOfSale(selector: e => e, id: id);
    public TResult GetReturnOfSale<TResult>(
        Expression<Func<ReturnOfSale, TResult>> selector,

        int id)
    {

      var orderItemBlock = GetReturnOfSales(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (orderItemBlock == null)
        throw new ReturnOfSaleNotFoundException(id);
      return orderItemBlock;
    }

    #endregion
    #region Gets

    public IQueryable<TResult> GetReturnOfSales<TResult>(
        Expression<Func<ReturnOfSale, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<int?> sendProductId = null,
        TValue<int> stuffId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<string> description = null,
        TValue<ReturnOfSaleType> type = null,
        TValue<ReturnOfSaleStatus> status = null,
        TValue<ReturnOfSaleStatus[]> statuses = null,
        TValue<int> returnStoreReceiptId = null,
        TValue<int> inboundCargoId = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<string> serial = null,
        TValue<int> customerId = null
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
      var returnOfSale = baseQuery.OfType<ReturnOfSale>();
      if (sendProductId != null)
        returnOfSale = returnOfSale.Where(r => r.SendProductId == sendProductId);
      if (stuffId != null) returnOfSale =
                returnOfSale.Where(r => r.StuffId == stuffId);

      if (serial != null)
        stuffSerialCode = GetStuffSerial(serial).Code;

      if (stuffSerialCode != null)
        returnOfSale = returnOfSale.Where(r => r.StuffSerialCode == stuffSerialCode);
      if (qty != null)
        returnOfSale = returnOfSale.Where(r => r.Qty == qty);
      if (unitId != null)
        returnOfSale = returnOfSale.Where(r => r.UnitId == unitId);
      if (type != null)
        returnOfSale = returnOfSale.Where(r => r.Type == type);
      if (status != null)
        returnOfSale = returnOfSale.Where(r => r.Status == status);
      if (statuses != null && statuses.Value.Length != 0)
      {
        var s = ReturnOfSaleStatus.Waiting;
        foreach (var item in statuses.Value)
          s = s | item;
        returnOfSale = returnOfSale.Where(i => (i.Status & s) > 0);
      }

      if (returnStoreReceiptId != null)
        returnOfSale = returnOfSale.Where(r => r.ReturnStoreReceiptId == returnStoreReceiptId);
      if (inboundCargoId != null)
        returnOfSale = returnOfSale.Where(r => r.ReturnStoreReceipt.InboundCargoId == inboundCargoId);
      if (fromDate != null)
        returnOfSale = returnOfSale.Where(r => r.DateTime >= fromDate);
      if (toDate != null)
        returnOfSale = returnOfSale.Where(r => r.DateTime <= toDate);
      if (customerId != null)
        returnOfSale = returnOfSale.Where(r => r.ReturnStoreReceipt.CooperatorId == customerId);

      return returnOfSale.Select(selector);
    }

    #endregion
    #region ToResult
    public Expression<Func<ReturnOfSale, ReturnOfSaleResult>> ToReturnOfSaleResult =
        returnOfSale => new ReturnOfSaleResult
        {
          Id = returnOfSale.Id,
          MainStuffId = returnOfSale.MainStuffId,
          MainStuffCode = "",
          MainStuffName = "",
          MainStuffNone = "",
          Code = returnOfSale.Code,
          ReceiptId = returnOfSale.ReturnStoreReceipt.ReceiptId,
          WarehouseId = returnOfSale.ReturnStoreReceipt.WarehouseId,
          WarehouseName = returnOfSale.ReturnStoreReceipt.Warehouse.Name,
          InboundCargoId = returnOfSale.ReturnStoreReceipt.InboundCargoId,
          InboundCargoCode = returnOfSale.ReturnStoreReceipt.InboundCargo.Code,
          ExitReceiptCode = returnOfSale.ExitReceiptCode,
          Amount = returnOfSale.ReturnStoreReceipt.Amount,
          //Todo ???
          QtyPerBox = 0,
          BoxNo = 0,
          Serial = returnOfSale.StuffSerial.Serial,
          StuffId = returnOfSale.StuffId,
          StuffCode = returnOfSale.Stuff.Code,
          StuffName = returnOfSale.Stuff.Name,
          StuffNoun = returnOfSale.Stuff.Noun,
          Qty = returnOfSale.Qty,
          UnitId = returnOfSale.UnitId,
          UnitName = returnOfSale.Unit.Name,
          BillOfMaterialVersion = returnOfSale.ReturnStoreReceipt.BillOfMaterialVersion,
          CooperatorId = returnOfSale.ReturnStoreReceipt.CooperatorId,
          CooperatorName = returnOfSale.ReturnStoreReceipt.Cooperator.Name,
          Status = returnOfSale.Status,
          SendProductId = returnOfSale.SendProductId,
          SendProductCode = returnOfSale.SendProduct.Code,
          StoreReceiptId = returnOfSale.ReturnStoreReceiptId,
          StoreReceiptCode = returnOfSale.ReturnStoreReceipt.Code,
          EmployeeFullName = returnOfSale.User.Employee.FirstName + " " + returnOfSale.User.Employee.LastName,
          DateTime = returnOfSale.DateTime,
          RowVersion = returnOfSale.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<ReturnOfSaleResult> SearchReturnOfSaleResult(IQueryable<ReturnOfSaleResult> query,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where item.Code.Contains(searchText) ||
                item.CooperatorName.Contains(searchText) ||
                item.StuffName.Contains(searchText) ||
                item.StuffNoun.Contains(searchText) ||
                item.WarehouseName.Contains(searchText) ||
                item.MainStuffName.Contains(searchText)
                select item;
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ReturnOfSaleResult> SortReturnOfSaleResult(IQueryable<ReturnOfSaleResult> query,
        SortInput<ReturnOfSaleSortType> sort)
    {
      switch (sort.SortType)
      {
        case ReturnOfSaleSortType.Id:
          return query.OrderBy(x => x.Id, sort.SortOrder);
        case ReturnOfSaleSortType.Code:
          return query.OrderBy(x => x.Code, sort.SortOrder);
        case ReturnOfSaleSortType.StoreReceiptCode:
          return query.OrderBy(x => x.StoreReceiptCode, sort.SortOrder);
        case ReturnOfSaleSortType.Serial:
          return query.OrderBy(x => x.Serial, sort.SortOrder);
        case ReturnOfSaleSortType.StuffCode:
          return query.OrderBy(x => x.StuffCode, sort.SortOrder);
        case ReturnOfSaleSortType.StuffName:
          return query.OrderBy(x => x.StuffName, sort.SortOrder);
        case ReturnOfSaleSortType.Qty:
          return query.OrderBy(x => x.Qty, sort.SortOrder);
        case ReturnOfSaleSortType.UnitName:
          return query.OrderBy(x => x.UnitName, sort.SortOrder);
        case ReturnOfSaleSortType.BillOfMaterialVersion:
          return query.OrderBy(x => x.BillOfMaterialVersion, sort.SortOrder);
        case ReturnOfSaleSortType.Status:
          return query.OrderBy(x => x.Status, sort.SortOrder);
        case ReturnOfSaleSortType.CooperatorName:
          return query.OrderBy(x => x.CooperatorName, sort.SortOrder);
        case ReturnOfSaleSortType.SendProductId:
          return query.OrderBy(x => x.SendProductId, sort.SortOrder);
        case ReturnOfSaleSortType.MainStuffCode:
          return query.OrderBy(x => x.MainStuffCode, sort.SortOrder);
        case ReturnOfSaleSortType.ExitReceiptCode:
          return query.OrderBy(x => x.ExitReceiptCode, sort.SortOrder);
        case ReturnOfSaleSortType.EmployeeFullName:
          return query.OrderBy(x => x.EmployeeFullName, sort.SortOrder);
        case ReturnOfSaleSortType.DateTime:
          return query.OrderBy(x => x.DateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
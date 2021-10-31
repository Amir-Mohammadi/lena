using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.QualityControl.QualityControlItem;
using lena.Models.WarehouseManagement.ReturnOfSaleSerial;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {



    #region Gets
    public IQueryable<ReturnOfSaleSerialResult> GetReturnOfSaleSerials(
        TValue<int> returnStoreReceiptId = null,
        TValue<string> serial = null,
        TValue<int> stuffId = null
       )
    {

      IQueryable<ReturnOfSaleSerialResult> query = null;

      if (returnStoreReceiptId != null)
      {


        var stuffs = App.Internals.SaleManagement.GetStuffs(
              selector: e => e);

        var returnOfSales = App.Internals.WarehouseManagement.GetReturnOfSales(
                  returnStoreReceiptId: returnStoreReceiptId,
                  selector: e => e);

        query = from stuff in stuffs
                join returnOfSale in returnOfSales on stuff.Id equals returnOfSale.StuffId
                select new ReturnOfSaleSerialResult()
                {
                  StuffId = returnOfSale.StuffId,
                  StuffCode = returnOfSale.Stuff.Code,
                  StuffName = returnOfSale.Stuff.Name,
                  Serial = returnOfSale.StuffSerial.Serial,
                  StuffSerialCode = returnOfSale.StuffSerialCode,
                  Qty = returnOfSale.Qty,
                  Status = returnOfSale.Status,
                  MainStuffId = returnOfSale.MainStuffId,
                  MainStuffName = stuff.Name,
                  MainStuffCode = stuff.Code,
                  ExitReceiptCode = returnOfSale.SendProduct.ExitReceipt.Code,
                  UnitId = returnOfSale.UnitId,
                  UnitName = returnOfSale.Unit.Name,
                  Type = returnOfSale.Type,
                  RowVersion = returnOfSale.RowVersion
                };

        if (stuffId != null)
          query = query.Where(i => i.StuffId == stuffId);

        if (serial != null)
          query = query.Where(i => i.Serial == serial);

      }

      return query;


    }
    #endregion



    #region Search
    public IQueryable<ReturnOfSaleSerialResult> SearchReturnOfSaleSerialResult(IQueryable<ReturnOfSaleSerialResult> query,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                select item;
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ReturnOfSaleSerialResult> SortReturnOfSaleSerialResult(IQueryable<ReturnOfSaleSerialResult> query,
        SortInput<ReturnOfSaleSerialSortType> sort)
    {
      switch (sort.SortType)
      {
        case ReturnOfSaleSerialSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case ReturnOfSaleSerialSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case ReturnOfSaleSerialSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case ReturnOfSaleSerialSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case ReturnOfSaleSerialSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case ReturnOfSaleSerialSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case ReturnOfSaleSerialSortType.MainStuffName:
          return query.OrderBy(a => a.MainStuffName, sort.SortOrder);
        case ReturnOfSaleSerialSortType.MainStuffCode:
          return query.OrderBy(a => a.MainStuffCode, sort.SortOrder);
        case ReturnOfSaleSerialSortType.ExitReceiptCode:
          return query.OrderBy(a => a.ExitReceiptCode, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
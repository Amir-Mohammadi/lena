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
using lena.Models.WarehouseManagement.NewShoppingSerial;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {



    #region Gets
    public IQueryable<NewShoppingSerialResult> GetNewShoppingSerials(
        TValue<int> storeReceiptId = null,
        TValue<string> serial = null,
        TValue<int> stuffId = null)
    {

      IQueryable<NewShoppingSerialResult> query = null;

      if (storeReceiptId != null)
      {
        var storeReceiptSerialProfiles = App.Internals.WarehouseManagement.GetStoreReceiptSerialProfiles(
                  selector: e => e,
                  storeReceiptId: storeReceiptId);
        var stuffSerials = App.Internals.WarehouseManagement.GetStuffSerials(
                    selector: e => e);

        query = from storeReceiptSerialProfile in storeReceiptSerialProfiles
                join stuffSerial in stuffSerials
                            on new
                            {
                              StuffId = storeReceiptSerialProfile.StuffId,
                              Code = storeReceiptSerialProfile.Code

                            } equals new
                            {
                              StuffId = stuffSerial.StuffId,
                              Code = stuffSerial.SerialProfileCode
                            }
                select new NewShoppingSerialResult()
                {
                  StuffId = stuffSerial.StuffId,
                  StuffCode = stuffSerial.Stuff.Code,
                  StuffName = stuffSerial.Stuff.Name,
                  StuffSerialCode = stuffSerial.Code,
                  Serial = stuffSerial.Serial,
                  InitQty = stuffSerial.InitQty,
                  UnitId = stuffSerial.InitUnitId,
                  UnitName = stuffSerial.InitUnit.Name,
                  PartitionStuffSerialQty = stuffSerial.PartitionStuffSerial.Qty,
                  MainSerial = stuffSerial.PartitionStuffSerial.MainStuffSerial.Serial
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
    public IQueryable<NewShoppingSerialResult> SearchNewShoppingSerialResult(IQueryable<NewShoppingSerialResult> query,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                select item;
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<NewShoppingSerialResult> SortNewShoppingSerialResult(IQueryable<NewShoppingSerialResult> query,
        SortInput<NewShoppingSerialSortType> sort)
    {
      switch (sort.SortType)
      {
        case NewShoppingSerialSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case NewShoppingSerialSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case NewShoppingSerialSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case NewShoppingSerialSortType.InitQty:
          return query.OrderBy(a => a.InitQty, sort.SortOrder);
        case NewShoppingSerialSortType.Unit:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case NewShoppingSerialSortType.PartitionStuffSerialQty:
          return query.OrderBy(a => a.PartitionStuffSerialQty, sort.SortOrder);
        case NewShoppingSerialSortType.MainSerial:
          return query.OrderBy(a => a.MainSerial, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
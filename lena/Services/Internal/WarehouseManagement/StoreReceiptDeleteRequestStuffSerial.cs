using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.WarehouseManagement.StoreReceiptDeleteRequestStuffSerialResult;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add

    public StoreReceiptDeleteRequestStuffSerial AddStoreReceiptDeleteRequestStuffSerial(
      int storeReceiptDeleteRequestId,
      int stuffSerialStuffId,
      long stuffSerialStuffCode,
      double amount,
      byte unitId)
    {

      var storeReceiptDeleteRequestStuffSerial = repository.Create<StoreReceiptDeleteRequestStuffSerial>();
      storeReceiptDeleteRequestStuffSerial.StoreReceiptDeleteRequestId = storeReceiptDeleteRequestId;
      storeReceiptDeleteRequestStuffSerial.StuffSerialId = stuffSerialStuffId;
      storeReceiptDeleteRequestStuffSerial.StuffSerialCode = stuffSerialStuffCode;
      storeReceiptDeleteRequestStuffSerial.Amount = amount;
      storeReceiptDeleteRequestStuffSerial.UnitId = unitId;
      repository.Add(storeReceiptDeleteRequestStuffSerial);
      return storeReceiptDeleteRequestStuffSerial;
    }

    #endregion

    #region Get

    public StoreReceiptDeleteRequestStuffSerial GetStoreReceiptDeleteRequestStuffSerial(int id)
        => GetStoreReceiptDeleteRequestStuffSerial(selector: e => e, id: id);
    public TResult GetStoreReceiptDeleteRequestStuffSerial<TResult>(
        Expression<Func<StoreReceiptDeleteRequestStuffSerial, TResult>> selector,
        int id)
    {

      var orderItemBlock = GetStoreReceiptDeleteRequestStuffSerials(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (orderItemBlock == null)
        throw new ReturnOfSaleNotFoundException(id);
      return orderItemBlock;
    }


    #endregion

    #region Gets

    public IQueryable<TResult> GetStoreReceiptDeleteRequestStuffSerials<TResult>(
        Expression<Func<StoreReceiptDeleteRequestStuffSerial, TResult>> selector,
            TValue<int> id = null,
            TValue<int> storeReceiptDeleteRequestId = null,
            TValue<int> stuffSerialId = null,
            TValue<long> stuffSerialCode = null
        )
    {


      var query = repository.GetQuery<StoreReceiptDeleteRequestStuffSerial>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (storeReceiptDeleteRequestId != null)
        query = query.Where(r => r.StoreReceiptDeleteRequestId == storeReceiptDeleteRequestId);
      if (stuffSerialId != null)
        query = query.Where(r => r.StuffSerialId == stuffSerialId);
      if (stuffSerialCode != null)
        query = query.Where(r => r.StuffSerialCode == stuffSerialCode);


      return query.Select(selector);
    }

    #endregion

    #region ToResult
    public Expression<Func<StoreReceiptDeleteRequestStuffSerial, StoreReceiptDeleteRequestStuffSerialResult>> ToStoreReceiptDeleteRequestStuffSerialResult =
        storeReceiptDeleteRequestStuffSerialResult => new StoreReceiptDeleteRequestStuffSerialResult
        {
          Id = storeReceiptDeleteRequestStuffSerialResult.Id,
          StoreReceiptDeleteRequestId = storeReceiptDeleteRequestStuffSerialResult.StoreReceiptDeleteRequest.Id,
          StuffSerialId = storeReceiptDeleteRequestStuffSerialResult.StuffSerial.StuffId,
          StuffSerialCode = storeReceiptDeleteRequestStuffSerialResult.StuffSerial.Code,
          Serial = storeReceiptDeleteRequestStuffSerialResult.StuffSerial.Serial,
          StuffCode = storeReceiptDeleteRequestStuffSerialResult.StuffSerial.Stuff.Code,
          StuffName = storeReceiptDeleteRequestStuffSerialResult.StuffSerial.Stuff.Name,
          Amount = storeReceiptDeleteRequestStuffSerialResult.Amount,
          UnitId = storeReceiptDeleteRequestStuffSerialResult.UnitId,
          UnitName = storeReceiptDeleteRequestStuffSerialResult.Unit.Name,
          RowVersion = storeReceiptDeleteRequestStuffSerialResult.RowVersion
        };
    #endregion

    #region Sort
    public IOrderedQueryable<StoreReceiptDeleteRequestStuffSerialResult> SortStoreReceiptDeleteRequestStuffSerialResult(IQueryable<StoreReceiptDeleteRequestStuffSerialResult> query,
            SortInput<StoreReceiptDeleteRequestStuffSerialSortType> sort)
    {
      switch (sort.SortType)
      {
        case StoreReceiptDeleteRequestStuffSerialSortType.Id:
          return query.OrderBy(x => x.Id, sort.SortOrder);
        case StoreReceiptDeleteRequestStuffSerialSortType.StuffSerialId:
          return query.OrderBy(x => x.StuffSerialId, sort.SortOrder);
        case StoreReceiptDeleteRequestStuffSerialSortType.StuffSerialCode:
          return query.OrderBy(x => x.StuffSerialCode, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<StoreReceiptDeleteRequestStuffSerialResult> SearchStoreReceiptDeleteRequestStuffSerialResult(
        IQueryable<StoreReceiptDeleteRequestStuffSerialResult> query,
        string searchText,
        TValue<int> storeReceiptDeleteRequestId,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.StuffName.Contains(searchText)
                select item;


      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      if (storeReceiptDeleteRequestId != null)
        query = query.Where(i => i.StoreReceiptDeleteRequestId == storeReceiptDeleteRequestId);
      return query;
    }
  }
  #endregion
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Common;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.StuffStockPlace;


using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Get
    public StuffStockPlace GetStuffStockPlace(
        int stuffId,
        int stockPlaceId) => GetStuffStockPlace(
        selector: e => e,
        stuffId: stuffId,
        stockPlaceId: stockPlaceId);
    public TResult GetStuffStockPlace<TResult>(
        Expression<Func<StuffStockPlace, TResult>> selector,
        int stuffId,
        int stockPlaceId)
    {

      var stuffStockPlace = GetStuffStockPlaces(
                    selector: selector,
                    stuffId: stuffId,
                    stockPlaceId: stockPlaceId)


                .FirstOrDefault();
      if (stuffStockPlace == null)
        throw new StuffStockPlaceNotFoundException(
                  stuffId: stuffId,
                  stockPlaceId: stockPlaceId);
      return stuffStockPlace;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStuffStockPlaces<TResult>(
            Expression<Func<StuffStockPlace, TResult>> selector,
            TValue<int> stuffId = null,
            TValue<int> stockPlaceId = null,
            TValue<int> warehouseId = null)
    {

      var query = repository.GetQuery<StuffStockPlace>();
      if (stuffId != null)
        query = query.Where(x => x.StuffId == stuffId);
      if (warehouseId != null)
        query = query.Where(x => x.StockPlace.WarehouseId == warehouseId);
      if (stockPlaceId != null)
        query = query.Where(x => x.StockPlace.Id == stockPlaceId);
      return query.Select(selector);
    }
    #endregion


    #region Gets
    public IQueryable<GroupedStuffStockPlaceResult> GetGroupedStuffStockPlaces(
            TValue<int> stuffId = null,
            TValue<int> warehouseId = null)
    {

      var query = GetStuffStockPlaces(
                selector: e => e,
                stuffId: stuffId,
                warehouseId: warehouseId);

      var result = from item in query
                   group item by new { item.StockPlace.WarehouseId, item.StuffId } into gItems
                   select new GroupedStuffStockPlaceResult
                   {
                     WarehouseId = gItems.Key.WarehouseId,
                     StuffId = gItems.Key.StuffId,
                     StockPlaceCodes = gItems.Select(i => i.StockPlace.Code).AsQueryable(),
                     StockPlaceTitles = gItems.Select(i => i.StockPlace.Title).AsQueryable()
                   };
      return result;
    }
    #endregion


    #region Add
    public StuffStockPlace AddStuffStockPlace(
        int stuffId,
        int stockPlaceId)
    {

      var query = GetStuffStockPlaces(
                    selector: e => e,
                    stuffId: stuffId,
                    stockPlaceId: stockPlaceId)


                .FirstOrDefault();

      var stuffStockPlace = repository.Create<StuffStockPlace>();
      if (query == null)
      {
        stuffStockPlace.StuffId = stuffId;
        stuffStockPlace.StockPlaceId = stockPlaceId;
        repository.Add(stuffStockPlace);
      }
      else
      {
        throw new DuplicateStuffStockPlaceException(
                  stuffId: stuffId,
                  stockPlaceId: stockPlaceId);
      }
      return stuffStockPlace;
    }
    #endregion
    #region EditProcess
    public void EditStuffStockPlaceProcesses(EditStuffStockPlaceInput input)
    {

      #region Get StuffStockPlace
      var stuffStockPlace = GetStuffStockPlace(
          e => e,
          stockPlaceId: input.StockPlaceId,
          stuffId: input.StuffId);
      #endregion
      if (stuffStockPlace.StockPlaceId != input.NewStockPlaceId)
      {
        #region Delete
        DeleteStuffStockPlace(stuffStockPlace);
        #endregion
        #region Add
        AddStuffStockPlace(
              stuffId: input.NewStuffId,
              stockPlaceId: input.NewStockPlaceId);
        #endregion
      }
    }
    #endregion
    #region Delete
    public void DeleteStuffStockPlace(
        int stuffId,
        int stockPlaceId)
    {

      var stuffStockPlace = GetStuffStockPlace(
                stuffId: stuffId,
                stockPlaceId: stockPlaceId); ; DeleteStuffStockPlace(stuffStockPlace);
    }
    public void DeleteStuffStockPlace(
        StuffStockPlace stuffStockPlace)
    {

      repository.Delete(stuffStockPlace);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffStockPlaceResult> SortStuffStockPlaceResult(
        IQueryable<StuffStockPlaceResult> query,
        SortInput<StuffStockPlaceSortType> sort)
    {
      switch (sort.SortType)
      {
        case StuffStockPlaceSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case StuffStockPlaceSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case StuffStockPlaceSortType.StuffTitle:
          return query.OrderBy(a => a.StuffTitle, sort.SortOrder);
        case StuffStockPlaceSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case StuffStockPlaceSortType.StockPlaceCode:
          return query.OrderBy(a => a.StockPlaceCode, sort.SortOrder);
        case StuffStockPlaceSortType.StockPlaceTitle:
          return query.OrderBy(a => a.StockPlaceTitle, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<StuffStockPlaceResult> SearchStuffStockPlaceResult(
        IQueryable<StuffStockPlaceResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.StuffCode.Contains(searchText) ||
                    item.StuffName.Contains(searchText) ||
                    item.StuffTitle.Contains(searchText) ||
                    item.StockPlaceCode.Contains(searchText) ||
                    item.StockPlaceTitle.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<StuffStockPlace, StuffStockPlaceResult>> ToStuffStockPlaceResult =
                stuffStockPlace => new StuffStockPlaceResult
                {
                  StuffId = stuffStockPlace.StuffId,
                  StuffCode = stuffStockPlace.Stuff.Code,
                  StuffName = stuffStockPlace.Stuff.Name,
                  StuffTitle = stuffStockPlace.Stuff.Title,
                  StockPlaceId = stuffStockPlace.StockPlaceId,
                  StockPlaceCode = stuffStockPlace.StockPlace.Code,
                  StockPlaceTitle = stuffStockPlace.StockPlace.Title,
                  WarehouseId = stuffStockPlace.StockPlace.WarehouseId,
                  WarehouseName = stuffStockPlace.StockPlace.Warehouse.Name,
                  RowVersion = stuffStockPlace.RowVersion
                };
    #endregion
  }
}

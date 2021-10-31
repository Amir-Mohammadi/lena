using System;
using System.Collections.Generic;
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
using lena.Models.UserManagement.Permission;
using lena.Models.WarehouseManagement.StockPlace;
using lena.Models.WarehouseManagement.WarehouseTransaction;
using lena.Models.UserManagement.SecurityAction;
using lena.Models.WarehouseManagement.BaseTransaction;
using lena.Models.WarehouseManagement.WarehouseReports;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public StockPlace AddStockPlace(
    string title,
    short warehouseId,
    string code)
    {

      var stockPlace = repository.Create<StockPlace>();
      stockPlace.Title = title;
      stockPlace.WarehouseId = warehouseId;
      stockPlace.Code = code;
      repository.Add(stockPlace);
      return stockPlace;
    }
    #endregion
    #region Adds
    public void AddStockPlaces(
        short warehouseId,
        WarehouseLayoutItemInput[] warehouseLayoutItems
    )
    {

      #region GenerateLayout and Get AddStockPlaceses
      List<AddStockPlaceInput> stockPlaces = null;
      foreach (var warehouseLayoutItem in warehouseLayoutItems)
      {
        var tempStockPlaces = new List<AddStockPlaceInput>();
        for (var i = 1; i <= warehouseLayoutItem.Count; i++)
        {
          var item = new AddStockPlaceInput()
          {
            Code = warehouseLayoutItem.Code + i.ToString(),
            Title = warehouseLayoutItem.Title + " " + i.ToString(),
            WarehouseId = warehouseId
          };
          tempStockPlaces.Add(item);
        }
        if (stockPlaces == null)
          stockPlaces = tempStockPlaces;
        else
        {
          var query = from item in stockPlaces
                      from tItem in tempStockPlaces
                      select new AddStockPlaceInput()
                      {
                        Code = item.Code + "-" + tItem.Code,
                        Title = item.Title + "-" + tItem.Title,
                        WarehouseId = warehouseId
                      };
          stockPlaces = query.ToList();
        }
      }
      #endregion
      #region Add StockPlaces
      if (stockPlaces != null)
      {
        foreach (var addStockPlace in stockPlaces)
        {
          AddStockPlace(
                        title: addStockPlace.Title,
                        warehouseId: addStockPlace.WarehouseId,
                        code: addStockPlace.Code);
        }
      }
      #endregion
    }
    #endregion
    #region Edit
    internal StockPlace EditStockPlace(
        int id,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> title = null,
        TValue<short> warehouseId = null)
    {

      var stockPlace = GetStockPlace(id: id);
      if (code != null)
        stockPlace.Code = code;
      if (title != null)
        stockPlace.Title = title;
      if (warehouseId != null)
        stockPlace.WarehouseId = warehouseId;
      repository.Update(stockPlace, rowVersion);
      return stockPlace;
    }
    #endregion
    #region Get
    public StockPlace GetStockPlace(int id) => GetStockPlace(selector: e => e, id: id);
    public TResult GetStockPlace<TResult>(
        Expression<Func<StockPlace, TResult>> selector,
        int id)
    {

      var stockPlace = GetStockPlaces(
                selector: selector,
                id: id).FirstOrDefault();
      if (stockPlace == null)
        throw new StockPlaceNotFoundException(id);
      return stockPlace;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStockPlaces<TResult>(
            Expression<Func<StockPlace, TResult>> selector,
            TValue<int> id = null,
             TValue<string> code = null,
        TValue<string> title = null,
        TValue<int> warehouseId = null
        )
    {

      var query = repository.GetQuery<StockPlace>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (code != null)
        query = query.Where(x => x.Code == code);
      if (title != null)
        query = query.Where(r => r.Title == title);
      if (warehouseId != null)
        query = query.Where(r => r.WarehouseId == warehouseId);
      return query.Select(selector);
    }
    #endregion
    #region ToStockPlaceResult
    public Expression<Func<StockPlace, StockPlaceResult>> ToStockPlaceResult =
        stockPlace => new StockPlaceResult
        {
          Id = stockPlace.Id,
          Code = stockPlace.Code,
          Title = stockPlace.Title,
          WarehouseId = stockPlace.WarehouseId,
          WarehouseName = stockPlace.Warehouse.Name,
          RowVersion = stockPlace.RowVersion
        };
    public Expression<Func<StockPlace, StockPlaceComboResult>> ToStockPlaceComboResult =
    stockPlace => new StockPlaceComboResult
    {
      Id = stockPlace.Id,
      Code = stockPlace.Code,
      Name = stockPlace.Title,
      RowVersion = stockPlace.RowVersion
    };
    #endregion
    #region Delete
    public void DeleteStockPlaceProcess(int id)
    {

      var stockPlace = GetStockPlace(id: id);
      var stuffStockPlaces = GetStuffStockPlaces(
                    selector: e => e,
                    stockPlaceId: id);
      foreach (var stuffStockPlace in stuffStockPlaces)
      {
        DeleteStuffStockPlace(stuffStockPlace);
      }
      repository.Delete(stockPlace);
    }
    #endregion
    #region Search
    public IQueryable<StockPlaceResult> SearchStockPlaceResult(IQueryable<StockPlaceResult> query,
        string search,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.Code.Contains(search) ||
                    item.Title.Contains(search) ||
                    item.WarehouseName.Contains(search)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StockPlaceResult> SortStockPlaceResult(IQueryable<StockPlaceResult> query,
        SortInput<StockPlaceSortType> sort)
    {
      switch (sort.SortType)
      {
        case StockPlaceSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case StockPlaceSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case StockPlaceSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case StockPlaceSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);


        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

  }
}

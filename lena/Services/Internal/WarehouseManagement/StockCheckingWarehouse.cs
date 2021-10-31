using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.StockCheckingWarehouse;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    internal StockCheckingWarehouse AddStockCheckingWarehouse(
        int stockCheckingId,
        short warehouseId)
    {
      var stockCheckingWarehouse = repository.Create<StockCheckingWarehouse>();
      stockCheckingWarehouse.StockCheckingId = stockCheckingId;
      stockCheckingWarehouse.WarehouseId = warehouseId;
      repository.Add(stockCheckingWarehouse);
      return stockCheckingWarehouse;
    }
    #endregion
    #region Get
    internal StockCheckingWarehouse GetStockCheckingWarehouse(int stockCheckingId, short warehouseId) =>
        GetStockCheckingWarehouse(selector: e => e, stockCheckingId: stockCheckingId, warehouseId: warehouseId);
    internal TResult GetStockCheckingWarehouse<TResult>(
        Expression<Func<StockCheckingWarehouse, TResult>> selector,
        int stockCheckingId,
        short warehouseId)
    {
      var data = GetStockCheckingWarehouses(
                selector: selector,
                stockCheckingId: stockCheckingId,
                warehouseId: warehouseId)
                .FirstOrDefault();
      if (data == null)
        throw new StockCheckingWarehouseNotFoundException();
      return data;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetStockCheckingWarehouses<TResult>(
        Expression<Func<StockCheckingWarehouse, TResult>> selector,
        TValue<int> stockCheckingId = null,
        TValue<int> warehouseId = null,
        TValue<StockCheckingStatus> status = null)
    {
      var query = repository.GetQuery<StockCheckingWarehouse>();
      if (stockCheckingId != null)
        query = query.Where(i => i.StockCheckingId == stockCheckingId);
      if (warehouseId != null)
        query = query.Where(i => i.WarehouseId == warehouseId);
      if (status != null)
        query = query.Where(i => i.StockChecking.Status == status);
      return query.Select(selector);
    }
    #endregion
    #region Delete
    internal void DeleteStockCheckingWarehouse(
        int stockCheckingId,
        short warehouseId)
    {
      var data = GetStockCheckingWarehouse(
                    stockCheckingId: stockCheckingId,
                    warehouseId: warehouseId);
      repository.Delete(data);
    }
    #endregion
    #region ToResult
    public Expression<Func<StockCheckingWarehouse, StockCheckingWarehouseResult>> ToStockCheckingWarehouseResult =
        stockCheckingWarehouse => new StockCheckingWarehouseResult
        {
          StockCheckingId = stockCheckingWarehouse.StockCheckingId,
          StockCheckingTitle = stockCheckingWarehouse.StockChecking.Title,
          WarehouseId = stockCheckingWarehouse.WarehouseId,
          WarehouseName = stockCheckingWarehouse.Warehouse.Name,
          RowVersion = stockCheckingWarehouse.RowVersion
        };
    #endregion
  }
}
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockCheckingWarehouse
{
  public class GetStockCheckingWarehousesInput : SearchInput<StockCheckingWarehouseSortType>
  {
    public int? StockCheckingId { get; set; }
    public int? WarehouseId { get; set; }

    public GetStockCheckingWarehousesInput(PagingInput pagingInput, StockCheckingWarehouseSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

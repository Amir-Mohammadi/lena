using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockPlace
{
  public class GetStockPlacesInput : SearchInput<StockPlaceSortType>
  {
    public GetStockPlacesInput(PagingInput pagingInput, StockPlaceSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? WarehouseId { get; set; }
    public string Code { get; set; }

  }
}

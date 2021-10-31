using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffStockPlace
{
  public class GetStuffStockPlacesInput : SearchInput<StuffStockPlaceSortType>
  {
    public int? StuffId { get; set; }
    public int? StockPlaceId { get; set; }
    public int? WarehouseId { get; set; }

    public GetStuffStockPlacesInput(PagingInput pagingInput, StuffStockPlaceSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

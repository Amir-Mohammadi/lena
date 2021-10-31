using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockCheckingTag
{
  public class GetStockTakingVariancesInput : SearchInput<StockTakingVarianceSortType>
  {
    public GetStockTakingVariancesInput(PagingInput pagingInput, StockTakingVarianceSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int StockCheckingId { get; set; }
    public short WarehouseId { get; set; }
    public int TagTypeId { get; set; }
    public int? StuffId { get; set; }
    public string Serial { get; set; }
    public int[] StuffIds { get; set; }
    public string[] Serials { get; set; }
    public StuffType? StuffType { get; set; }
    public int? StuffCategoryId { get; set; }
    public bool GroupBySerial { get; set; }
    public StockCheckingTagStatus[] Statuses { get; set; }
  }
}

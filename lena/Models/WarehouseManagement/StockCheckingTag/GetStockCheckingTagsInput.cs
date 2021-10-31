using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockCheckingTag
{
  public class GetStockCheckingTagsInput : SearchInput<StockCheckingTagSortType>
  {
    public int? Id { get; set; }
    public int StockCheckingId { get; set; }
    public short WarehouseId { get; set; }
    public int? Number { get; set; }
    public int? StuffId { get; set; }
    public StuffType? StuffType { get; set; }
    public int? StuffCategoryId { get; set; }
    public bool? HasTag { get; set; }
    public bool? IsExist { get; set; }
    public int TagTypeId { get; set; }
    public string Serial { get; set; }
    public int[] Ids { get; set; }

    public GetStockCheckingTagsInput(PagingInput pagingInput, StockCheckingTagSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

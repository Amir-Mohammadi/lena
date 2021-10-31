using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.TagCounting
{
  public class GetTagCountingsInput : SearchInput<TagCountingSortType>
  {
    public GetTagCountingsInput(PagingInput pagingInput, TagCountingSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? StockCheckingTagId { get; set; }
    public int? StockCheckingId { get; set; }
    public int? WarehouseId { get; set; }
    public int? TagTypeId { get; set; }
    public int? StuffId { get; set; }
    public int? EmployeeId { get; set; }
    public string Serial { get; set; }
  }
}

using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseIssueItem
{
  public class GetWarehouseIssueItemsInput : SearchInput<WarehouseIssueItemSortType>
  {
    public GetWarehouseIssueItemsInput(PagingInput pagingInput, WarehouseIssueItemSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? WarehouseIssueId { get; set; }
  }
}

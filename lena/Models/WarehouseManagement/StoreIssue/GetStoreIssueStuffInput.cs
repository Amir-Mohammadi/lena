using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreIssue
{
  public class GetStoreIssueStuffInput : SearchInput<StoreIssueDetailSortType>
  {
    public int[] StoreIssueIds { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? FromStoreId { get; set; }
    public int? ToStoreId { get; set; }
    public StoreIssueState? State { get; set; }

    public GetStoreIssueStuffInput(PagingInput pagingInput, StoreIssueDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

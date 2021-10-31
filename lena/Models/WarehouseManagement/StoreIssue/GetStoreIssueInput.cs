using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreIssue
{
  public class GetStoreIssueInput : SearchInput<StoreIssueSortType>
  {
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public int? FromWarehoueId { get; set; }

    public int? ToWarehouseId { get; set; }

    public StoreIssueState? State { get; set; }

    public GetStoreIssueInput(PagingInput pagingInput, StoreIssueSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

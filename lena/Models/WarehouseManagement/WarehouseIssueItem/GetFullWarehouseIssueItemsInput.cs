using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains.Enums;
using System;


using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseIssueItem
{
  public class GetFullWarehouseIssueItemsInput : SearchInput<WarehouseIssueItemSortType>
  {
    public GetFullWarehouseIssueItemsInput(PagingInput pagingInput, WarehouseIssueItemSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? WarehouseIssueId { get; set; }
    public string WarehouseIssueCode { get; set; }
    public int? FromWarehouseId { get; set; }
    public int? ToWarehouseId { get; set; }
    public WarehouseIssueStatusType? Status { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public string Serial { get; set; }
    public int? ToEmployeeId { get; set; }
    public int? ToDepartmentId { get; set; }
  }
}

using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseIssue
{
  public class GetWarehouseIssuesInput : SearchInput<WarehouseIssueSortType>
  {
    public GetWarehouseIssuesInput(PagingInput pagingInput, WarehouseIssueSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? FromWarehouseId { get; set; }
    public int? ToWarehouseId { get; set; }
    public WarehouseIssueStatusType? Status { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public string Serial { get; set; }
    public string Code { get; set; }
    public int? ToEmployeeId { get; set; }
    public int? ToDepartmentId { get; set; }
  }
}

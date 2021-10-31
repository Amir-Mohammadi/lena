using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseReport
{
  public class GetPurchaseReportInput : SearchInput<PurchaseReportSortType>
  {
    public GetPurchaseReportInput(PagingInput pagingInput, PurchaseReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? StuffId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public int? PurchaseRequestUserId { get; set; }
    public int? EmployeeId { get; set; }
  }
}

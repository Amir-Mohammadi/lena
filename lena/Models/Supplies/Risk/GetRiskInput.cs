using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Risk
{
  public class GetRiskInput : SearchInput<RiskSortType>
  {
    public int? PurchaseRequestId { get; set; }
    public int? PurchaseOrderId { get; set; }
    public int? CargotItemId { get; set; }
    public int? CustomerComplaintId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public GetRiskInput(PagingInput pagingInput, RiskSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialAccount
{
  public class GetPayRequestsInput : SearchInput<PayRequestSortType>
  {
    public int[] Ids { get; set; }
    public string QualityControlCode { get; set; }
    public int? StuffId { get; set; }
    public PayRequestStatus? Status { get; set; }
    public int? EmployeeId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public int? ProviderId { get; set; }
    public int? StoreReceiptId { get; set; }
    public string StoreReceiptCode { get; set; }


    public GetPayRequestsInput(PagingInput pagingInput, PayRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

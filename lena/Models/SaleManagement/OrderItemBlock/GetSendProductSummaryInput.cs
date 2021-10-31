using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemBlock
{
  public class GetSendProductSummaryInput : SearchInput<SendProductSummarySortType>
  {
    public int? StuffId { get; set; }
    public int? CustomerId { get; set; }
    public int? ExitReceiptRequestTypeId { get; set; }
    public DateTime? FromDateOrder { get; set; }
    public DateTime? ToDateOrder { get; set; }
    public DateTime? FromDateTransfer { get; set; }
    public DateTime? ToDateTransfer { get; set; }
    public bool DividedByDate { get; set; }
    public bool DividedByCustomer { get; set; }
    public short CeofficientSet { get; set; }
    public bool? CeofficientSetType { get; set; }
    public short SumCeofficientSet { get; set; }

    public GetSendProductSummaryInput(PagingInput pagingInput, SendProductSummarySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

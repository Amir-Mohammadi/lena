using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ExitReceipt
{
  public class ExitReceiptInput : SearchInput<ExitReceiptSortType>
  {
    public ExitReceiptInput(PagingInput pagingInput, ExitReceiptSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? Id { get; set; }
    public string Code { get; set; }
    public DateTime? ToDateTime { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToTransportDateTime { get; set; }
    public DateTime? FromTransportDateTime { get; set; }
    public int? OutboundCargoId { set; get; }
    public int? ExitReceiptUserId { set; get; }
    public int? OutboundCargoUserId { set; get; }
    public string ShippingCompanyName { set; get; }
    public int? CooperatorId { set; get; }
    public int? ExitReceiptRequestId { set; get; }
    public bool? Confirmed { get; set; }
    public int? StuffId { set; get; }

  }
}

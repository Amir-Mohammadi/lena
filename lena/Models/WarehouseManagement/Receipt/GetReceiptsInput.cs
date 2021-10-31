using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Receipt
{
  public class GetReceiptsInput : SearchInput<ReceiptSortType>
  {
    public GetReceiptsInput(PagingInput pagingInput, ReceiptSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? CooperatorId { get; set; }
    public int? UserId { get; set; }
    public int? ReceiptId { get; set; }
    public int? StuffId { get; set; }
    public string ReceiptCode { get; set; }
    public string LadingCode { get; set; }
    public string CargoItemCode { get; set; }
    public string PurchaseOrderCode { get; set; }
    public StoreReceiptType? StoreReceiptType { get; set; }
    public int[] ReceiptIds { get; set; }
    public ReceiptStatus? ReceiptStatus { get; set; }
    public ReceiptStatus[] ReceiptStatuses { get; set; }
    public ReceiptStatus[] ReceiptNotHasStatuses { get; set; }
  }
}

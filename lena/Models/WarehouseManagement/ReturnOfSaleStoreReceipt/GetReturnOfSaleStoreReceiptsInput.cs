using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceipt
{
  public class GetReturnOfSaleStoreReceiptsInput : SearchInput<ReturnOfSaleStoreReceiptSortType>
  {
    public GetReturnOfSaleStoreReceiptsInput(PagingInput pagingInput, ReturnOfSaleStoreReceiptSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int[] Ids { get; set; }
    public int? StuffId { get; set; }
    public string InboundCargoCode { get; set; }
    public StoreReceiptType? StoreReceiptType { get; set; }
    public string PurchaseOrderCode { get; set; }
    public string CargoCode { get; set; }
    public string CooperatorName { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? ReceiptId { get; set; }
    public string ReceiptCode { get; set; }
    public ReceiptStatus[] ReceiptStatuses { get; set; }
    public ReceiptStatus? ReceiptStatus { get; set; }
    public ReceiptStatus[] ReceiptNotHasStatuses { get; set; }
  }
}
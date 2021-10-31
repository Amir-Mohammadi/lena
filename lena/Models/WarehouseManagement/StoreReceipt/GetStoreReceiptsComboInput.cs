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
  public class GetStoreReceiptsComboInput : SearchInput<StoreReceiptSortType>
  {
    public GetStoreReceiptsComboInput(PagingInput pagingInput, StoreReceiptSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? StuffId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? ReceiptId { get; set; }
    public string ReceiptCode { get; set; }
    public int? WarehouseId { get; set; }
    public int? CooperatorId { get; set; }
    public ReceiptStatus ReceiptStatus { get; set; }
    public ReceiptStatus[] ReceiptStatuses { get; set; }
    public ReceiptStatus[] ReceiptNotHasStatuses { get; set; }
  }
}
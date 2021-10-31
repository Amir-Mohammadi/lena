using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceipt
{
  public class GetStoreReceiptDeleteRequestInput : SearchInput<StoreReceiptDeleteRequestSortType>
  {
    public GetStoreReceiptDeleteRequestInput(PagingInput pagingInput, StoreReceiptDeleteRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? Id { get; set; }
    public bool IsDelete { get; set; }
    public string Description { set; get; }
    public int? StoreReceiptId { set; get; }
    public string StoreReceiptCode { set; get; }
    public int? StuffId { set; get; }
    public int? CreatorUserId { set; get; }
    public DateTime? DateTime { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? ChangeStatusDateTime { get; set; }
    public DateTime? ReceiptDateTime { get; set; }
    public int? ChangeStatusUserId { get; set; }
    public StoreReceiptDeleteRequestStatus? Status { get; set; }
    public StoreReceiptType? StoreReceiptType { get; set; }
  }
}
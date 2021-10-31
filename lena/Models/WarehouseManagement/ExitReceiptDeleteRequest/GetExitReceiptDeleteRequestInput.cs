using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ExitReceipt
{
  public class GetExitReceiptDeleteRequestInput : SearchInput<ExitReceiptDeleteRequestSortType>
  {
    public GetExitReceiptDeleteRequestInput(PagingInput pagingInput, ExitReceiptDeleteRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? Id { get; set; }
    public int? CreatorUserId { set; get; }
    public int? StuffId { set; get; }
    public string Description { set; get; }
    public int? ExitReceiptId { set; get; }
    public string ExitReceiptCode { set; get; }
    public DateTime? FromCreateDateTime { get; set; }
    public DateTime? ToCreateDateTime { get; set; }
    public DateTime? FromChangeStatusDateTime { get; set; }
    public DateTime? ToChangeStatusDateTime { get; set; }
    public ExitReceiptDeleteRequestStatus? Status { get; set; }
  }
}

using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.PreparingSending
{
  public class GetPreparingSendingItemsInput : SearchInput<PreparingSendingItemSortType>
  {
    public GetPreparingSendingItemsInput(PagingInput pagingInput, PreparingSendingItemSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public string serial { get; set; }
    public DateTime? ToDateOrder { get; set; }
    public DateTime? FromDateOrder { get; set; }
    public DateTime? ToDateTransfer { get; set; }
    public DateTime? FromDateTransfer { get; set; }
    public int? CooperatorId { get; set; }
    public int? StuffId { get; set; }
    public int? ExitReceiptRequestTypeId { get; set; }
    public int? PreparingSendingId { get; set; }
    public int? ExitReceiptId { get; set; }
    public int? OutboundCargoId { get; set; }
    public int? SendPermissionId { get; set; }
  }
}

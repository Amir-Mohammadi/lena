using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.ExitReceiptRequest
{
  public class GetExitReceiptRequestsInput : SearchInput<ExitReceiptRequestSortType>
  {
    public GetExitReceiptRequestsInput(PagingInput pagingInput, ExitReceiptRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? Id { get; set; }
    public int? CooperatorId { get; set; }
    public int? StuffId { get; set; }
    public int? ExitReceiptRequestTypeId { get; set; }
    public int? OrderItemId { get; set; }
    public ExitReceiptRequestStatus? Status { get; set; }
    public ExitReceiptRequestStatus[] Statuses { get; set; }
    public ExitReceiptRequestSortType[] ExitReceiptRequestSortType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public ExitReceiptRequestStatus[] NotHasStatuses { get; set; }
  }
}

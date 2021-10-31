using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.SendPermission
{
  public class GetSendPermissionsInput : SearchInput<SendPermissionSortType>
  {
    public int? Id { get; set; }
    public int? StuffId { get; set; }
    public int? CustomerId { get; set; }
    public int? OrderItemId { get; set; }
    public int? ExitReceiptRequestId { get; set; }
    public int? ExitReceiptRequestTypeId { get; set; }
    public ExitReceiptRequestStatus[] ExitReceiptRequestStatuses { get; set; }
    public SendPermissionStatusType[] SendPermissionStatuses { get; set; }
    public ExitReceiptRequestStatus[] ExitReceiptRequestNotHasStatuses { get; set; }
    public SendPermissionStatusType[] SendPermissionNotHasStatuses { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public GetSendPermissionsInput(PagingInput pagingInput, SendPermissionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

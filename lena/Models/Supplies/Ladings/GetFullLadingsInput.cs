using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class GetFullLadingsInput : SearchInput<LadingItemSortType>
  {
    public int? LadingId { get; set; }
    public int? LadingItemId { get; set; }
    public int[] LadingItemIds { get; set; }
    public int[] LadingIds { get; set; }
    public string LadingCode { get; set; }
    public string PurchaseOrderCode { get; set; }
    public string CargoItemCode { get; set; }
    public int? CargoId { get; set; }
    public int? CargoItemId { get; set; }
    public int? BankOrderId { get; set; }
    public int? CustomhouseId { get; set; }
    public int? StuffId { get; set; }
    public int? ProviderId { get; set; }
    public DateTime? FromDeliveryDate { get; set; }
    public DateTime? ToDeliveryDate { get; set; }
    public string BankOrderNumber { get; set; }
    public string KotazhCode { get; set; }
    public string SataCode { get; set; }
    public int? UserId { get; set; }
    public int? FinancialTransactionBatchId { get; set; }
    public int? FinancialDocumentId { get; set; }
    public int? BankOrderStatusId { get; set; }
    public int? CustomhouseStatusId { get; set; }
    public DateTime? FromTransportDate { get; set; }
    public DateTime? ToTransportDate { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public ProviderType? ProviderType { get; set; }
    public bool? IsDelete { get; set; }
    public bool? IsLocked { get; set; }
    public string CargoCode { get; set; }
    public int? EmployeeId { get; set; }
    public int? PlanCodeId { get; set; }
    public int[] SelectedPlanCodeIds { get; set; }
    public GetFullLadingsInput(PagingInput pagingInput, LadingItemSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

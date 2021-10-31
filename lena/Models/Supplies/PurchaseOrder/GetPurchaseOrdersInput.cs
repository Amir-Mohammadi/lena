using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrder
{
  public class GetPurchaseOrdersInput : SearchInput<PurchaseOrderSortType>
  {
    public GetPurchaseOrdersInput(PagingInput pagingInput, PurchaseOrderSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? StuffId { get; set; }
    public int? StuffCategoryId { get; set; }
    public string Code { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public DateTime? FromDeadlineDateTime { get; set; }
    public DateTime? ToDeadlineDateTime { get; set; }
    public PurchaseOrderStatus? PurchaseOrderStatus { get; set; }
    public int? HowToBuyId { get; set; }
    public int? CargoId { get; set; }
    public int? ProviderId { get; set; }
    public int? CurrencyId { get; set; }
    public int? EmployeeId { get; set; }
    public int? SupplierId { get; set; }

    public RiskLevelStatus? RiskLevelStatus { get; set; }
    public PurchaseOrderType? PurchaseOrderType { get; set; }
    public int[] PurchaseOrderIds { get; set; }
    public PurchaseOrderStatus[] PurchaseOrderStatuses { get; set; }
    public PurchaseOrderStatus[] PurchaseOrderNotHasStatuses { get; set; }
    public int[] Ids { get; set; }
    public int? FinancialTransactionBatchId { get; set; }
    public int? FinancialDocumentId { get; set; }
    public FinancialDocumentTypeResult? FinancialDocumentTypeResult { get; set; }
    public int? PlanCodeId { get; set; }
    public int[] SelectedPlanCodeIds { get; set; }
    public string PurchaseRequsetDescription { get; set; }
    public string PurchaseOrderGroupCode { get; set; }
    public int? PurchaseOrderGroupId { get; set; }
    public bool? IsArchived { get; set; }
    public StuffType? StuffType { get; set; }
    public FinanceAllocationStatus? FinanceAllocationStatus { get; set; }
  }
}

using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using System;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequest
{
  public class GetPurchaseRequestsInput : SearchInput<PurchaseRequestSortType>
  {
    public int? StuffId { get; set; }

    public StuffType? StuffType { get; set; }
    public string Code { get; set; }
    public DateTime? FromRequestDate { get; set; }
    public DateTime? ToRequestDate { get; set; }
    public DateTime? FromDeadline { get; set; }
    public DateTime? ToDeadline { get; set; }
    public DateTime? FromConfirmDate { get; set; }
    public DateTime? ToConfirmDate { get; set; }
    public RiskLevelStatus? RiskLevelStatus { get; set; }
    public PurchaseRequestStatus? Status { get; set; }
    public PurchaseRequestStatus[] Statuses { get; set; }
    public int? StuffCategoryId { get; set; }
    public int? Id { get; set; }
    public int? DepartmentId { get; set; }
    public int? ResponsibleEmployeeId { get; set; }
    public PurchaseRequestStatus[] NotHasStatuses { get; set; }
    public int[] Ids { get; set; }
    public string PlanCode { get; set; }
    public string ProjectCode { get; set; }
    public int? EmployeeId { get; set; }

    public int? PlanCodeId { get; set; }
    public bool? IsArchived { get; set; }
    public int[] SelectedPlanCodeIds { get; set; }
    public GetPurchaseRequestsInput(PagingInput pagingInput, PurchaseRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

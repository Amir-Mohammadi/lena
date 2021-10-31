using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrderWorkSpace
{
  public class GetPurchaseOrderWorkSpaceInput : SearchInput<PurchaseOrderWorkSpaceSortType>
  {
    public GetPurchaseOrderWorkSpaceInput(PagingInput pagingInput, PurchaseOrderWorkSpaceSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public bool PurchaseOrderDetailShow { get; set; }
    public bool CargoItemDetailShow { get; set; }
    public bool LadingItemDetailShow { get; set; }
    public bool NewShoppingDetailShow { get; set; }

    public int? StuffId { get; set; }
    public string Code { get; set; }
    public DateTime? FromRequestDate { get; set; }
    public DateTime? ToRequestDate { get; set; }
    public DateTime? FromDeadline { get; set; }
    public DateTime? ToDeadline { get; set; }
    public DateTime? FromConfirmDate { get; set; }
    public DateTime? ToConfirmDate { get; set; }
    public PurchaseRequestStatus? Status { get; set; }
    public PurchaseRequestStatus[] Statuses { get; set; }
    public int? StuffCategoryId { get; set; }
    public int? Id { get; set; }
    public int? DepartmentId { get; set; }
    public int? ResponsibleEmployeeId { get; set; }
    public PurchaseRequestStatus[] NotHasStatuses { get; set; }
    public int[] Ids { get; set; }
    public string PlanCode { get; set; }
    public int? EmployeeId { get; set; }

    public bool? IsArchived { get; set; }
  }
}

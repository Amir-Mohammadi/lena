using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionPlan
{
  public class GetProductionPlanInput : SearchInput<ProductionPlanSortType>
  {
    public GetProductionPlanInput(PagingInput pagingInput, ProductionPlanSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? Id { get; set; }
    public int? StuffId { get; set; }
    public int? Version { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsPublished { get; set; }
    public BillOfMaterialPublishRequestStatus? BillOfMaterialPublishRequestStatus { get; set; }
    public BillOfMaterialPublishRequestType? BillOfMaterialPublishRequestType { get; set; }
    public bool IsTemporary { get; set; }
    public string OrderItemCode { get; set; }
    public DateTime? FromEstimatedDate { get; set; }
    public DateTime? ToEstimatedDate { get; set; }
    public DateTime? FromDeadlineDate { get; set; }
    public DateTime? ToDeadlineDate { get; set; }
    public int? ProductionRequestId { get; set; }
    public ProductionPlanStatus? Status { get; set; }
    public ProductionPlanStatus[] Statuses { get; set; }
    public ProductionPlanStatus[] NotHasStatuses { get; set; }
    public int? PlannerUserId { get; set; }
  }
}

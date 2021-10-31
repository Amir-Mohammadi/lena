using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionPlan
{
  public class ProductionPlanResult
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public string OrderItemCode { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public string OrderTypeName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsPublished { get; set; }
    public BillOfMaterialPublishRequestStatus? BillOfMaterialPublishRequestStatus { get; set; }
    public BillOfMaterialPublishRequestType? BillOfMaterialPublishRequestType { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime EstimatedDate { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public int? ProductionRequestId { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public string ProductionRequestCode { get; set; }
    public ProductionPlanStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

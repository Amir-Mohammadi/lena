using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.ProductionRequest
{
  public class ProductionRequestResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string OrderDescription { get; set; }
    public int OrderItemId { get; set; }
    public string OrderItemCode { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public string OrderTypeName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public double Qty { get; set; }
    public string UnitName { get; set; }
    public byte UnitId { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime DeadlineDate { get; set; }
    public int CheckOrderItemId { get; set; }
    public ProductionRequestStatus Status { get; set; }
    public double PlannedQty { get; set; }
    public double ScheduledQty { get; set; }
    public double ProducedQty { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

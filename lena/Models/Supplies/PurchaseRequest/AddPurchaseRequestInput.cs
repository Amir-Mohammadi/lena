using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequest
{
  public class AddPurchaseRequestInput
  {
    public int StuffId { get; set; }
    public byte UnitId { get; set; }
    public double RequestQty { get; set; }
    public int CostcenterId { get; set; }
    public string ProjectCode { get; set; }
    public DateTime DeadLine { get; set; }
    public string Description { get; set; }
    public int? PlanCodeId { get; set; }
    public Nullable<Guid> DocumentId { get; set; }
    public int? EmployeeRequsterId { get; set; }
    public string Link { get; set; }
    public string FileKey { get; set; }
    public PurchaseRequestStatus PurchaseRequestStatus { get; set; }
    public PurchaseRequestSupplyType? SupplyType { get; set; }
  }
}

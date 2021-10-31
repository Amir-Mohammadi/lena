using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequest
{
  public class EditPurchaseRequestInput
  {
    public int Id { get; set; }
    public int? PlanCodeId { get; set; }
    public int? CostCenterId { get; set; }
    public string ProjectCode { get; set; }
    public double? RequestQty { get; set; }
    public DateTime? DeadLine { get; set; }
    public string Description { get; set; }
    public int? EmployeeRequesterId { get; set; }
    public string FileKey { get; set; }
    public string Link { get; set; }
    public string EditLogDescription { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

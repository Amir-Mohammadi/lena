using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.ProductionRequest
{
  public class AddProductionRequestInput
  {
    public string Description { get; set; }
    public DateTime DeadlineDate { get; set; }
    public byte UnitId { get; set; }
    public double Qty { get; set; }
  }
}

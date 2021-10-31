using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.ProductionRequest
{
  public class EditProductionRequestInput
  {
    public byte[] RowVersion { get; set; }
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime DeadlineDate { get; set; }
    public byte UnitId { get; set; }
    public double Qty { get; set; }
  }
}

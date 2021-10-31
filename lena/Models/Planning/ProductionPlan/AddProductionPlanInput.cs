using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionPlan
{
  public class AddProductionPlanInput
  {
    public string Description { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public short BillOfMaterialVersion { get; set; }
    public int BillOfMaterialStuffId { get; set; }
    public DateTime EstimatedDate { get; set; }
  }
}

using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionPlan
{
  public class EditProductionPlanInput
  {
    public byte[] RowVersion { get; set; }
    public int Id { get; set; }
    public string Description { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public short BillOfMaterialVersion { get; set; }
    public int BillOfMaterialStuffId { get; set; }

    public DateTime EstimatedDate { get; set; }
  }
}

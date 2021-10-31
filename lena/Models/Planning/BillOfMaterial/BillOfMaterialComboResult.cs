using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class BillOfMaterialComboResult
  {
    public int StuffId { get; set; }
    public int QtyPerBox { get; set; }
    public int Version { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public int ProductionStepId { get; set; }
    public string ProductionStepName { get; set; }
    public BillOfMaterialVersionType BillOfMaterialVersionType { get; set; }
  }
}

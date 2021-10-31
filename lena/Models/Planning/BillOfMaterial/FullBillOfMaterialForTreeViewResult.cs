using System.Collections.Generic;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class FullBillOfMaterialForTreeViewResult
  {
    public FullBillOfMaterialForTreeViewResult()
    {
      this.BillOfMaterialDetails = new List<FullBillOfMaterialForTreeViewResult>();
    }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int Version { get; set; }
    public string Title { get; set; }
    public double Value { get; set; }
    public double? ForQty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public string Description { get; set; }
    public BillOfMaterialDetailType BillOfMaterialDetailType { get; set; }
    public IList<FullBillOfMaterialForTreeViewResult> BillOfMaterialDetails { get; set; }
    public int ProductionStepId { get; set; }
    public string ProductionStepName { get; set; }
    public int Level { get; set; }
    public bool NotHasAnyActiveAndPublishedVersion { get; set; }
    public int EquivalentStuffsCount { get; set; }

  }
}
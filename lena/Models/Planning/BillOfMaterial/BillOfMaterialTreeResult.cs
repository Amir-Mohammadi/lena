using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class BillOfMaterialTreeResult
  {
    public BillOfMaterialTreeResult()
    {
      this.InnerBom = new List<BillOfMaterialTreeResult>();
    }
    public int OperationId { get; set; }
    public int? ParentStuffId { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int? Version { get; set; }
    public string Title { get; set; }
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int Level { get; set; }
    List<BillOfMaterialTreeResult> InnerBom { get; set; }

  }


}

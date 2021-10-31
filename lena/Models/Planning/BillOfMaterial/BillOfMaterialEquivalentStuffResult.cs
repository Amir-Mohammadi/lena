using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class BillOfMaterialEquivalentStuffResult
  {
    public int EquivalentStuffId { get; set; }
    public int EquivalentStuffDetailId { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int? Version { get; set; }
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double ForQty { get; set; }
  }
}

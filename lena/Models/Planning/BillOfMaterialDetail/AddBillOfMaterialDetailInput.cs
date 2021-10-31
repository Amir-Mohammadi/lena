using lena.Domains.Enums;
using lena.Models.Planning.EquivalentStuff;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialDetail
{
  public class AddBillOfMaterialDetailInput
  {
    public int Index { get; set; }
    public double Value { get; set; }
    public bool Reservable { get; set; }
    public byte UnitId { get; set; }
    public int StuffId { get; set; }
    public double ForQty { get; set; }
    public bool IsPackingMaterial { get; set; }
    public short? SemiProductBillOfMaterialVersion { get; set; }
    public string Description { get; set; }
    public BillOfMaterialDetailType BillOfMaterialDetailType { get; set; }
    public AddEquivalentStuffInput[] EquivalentStuffs { get; set; }
  }
}

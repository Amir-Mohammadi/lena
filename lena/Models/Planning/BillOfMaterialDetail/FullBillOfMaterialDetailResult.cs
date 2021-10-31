using lena.Models.Planning.BillOfMaterial;
using lena.Models.Planning.EquivalentStuff;
using lena.Models.WarehouseManagement.Common;
using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialDetail
{
  public class FullBillOfMaterialDetailResult
  {
    public FullEquivalentStuffResult[] EquivalentStuffs { get; set; }
    public int Id { get; set; }
    public int Index { get; set; }
    public double Value { get; set; }
    public bool Reservable { get; set; }
    public byte UnitId { get; set; }
    public double UnitConversionRatio { get; set; }
    //public string UnitName { get; set; }
    public string StuffName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public int? SemiProductBillOfMaterialVersion { get; set; }
    public BillOfMaterialDetailType BillOfMaterialDetailType { get; set; }
    public byte[] RowVersion { get; set; }
    public BillOfMaterialComboResult[] BillOfMaterials { get; set; }
    public UnitComboResult[] Units { get; set; }
    public double ForQty { get; set; }
    public bool IsPackingMaterial { get; set; }
    public string Description { get; set; }
  }
}
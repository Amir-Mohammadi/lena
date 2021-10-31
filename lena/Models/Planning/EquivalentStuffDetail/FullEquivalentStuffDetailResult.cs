using lena.Models.Planning.BillOfMaterial;
using lena.Models.WarehouseManagement.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.EquivalentStuffDetail
{
  public class FullEquivalentStuffDetailResult
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double ForQty { get; set; }
    public int? SemiProductBillOfMaterialVersion { get; set; }
    //public int EquivalentStuffDetailId { get; set; }
    public byte[] RowVersion { get; set; }
    public UnitComboResult[] Units { get; set; }
    public BillOfMaterialComboResult[] BillOfMaterials { get; set; }
  }
}

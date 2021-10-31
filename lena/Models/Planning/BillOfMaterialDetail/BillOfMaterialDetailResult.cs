using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialDetail
{
  public class BillOfMaterialDetailResult
  {
    public int Id { get; set; }
    public int Index { get; set; }
    public double Value { get; set; }
    public bool Reservable { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public StuffType StuffType { get; set; }
    public BillOfMaterialDetailType BillOfMaterialDetailType { get; set; }
    public int StuffId { get; set; }
    public int? SemiProductBillOfMaterialVersion { get; set; }
    public int BillOfMaterialStuffId { get; set; }
    public int BillOfMaterialVersion { get; set; }
    public string BillOfMaterialStuffCode { get; set; }
    public StuffType BillOfMaterialStuffType { get; set; }
    public string StuffNoun { get; set; }
    public double ForQty { get; set; }
    public bool IsPackingMaterial { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

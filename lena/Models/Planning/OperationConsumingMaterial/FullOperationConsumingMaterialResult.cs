using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.OperationConsumingMaterial
{
  public class FullOperationConsumingMaterialResult
  {
    public int Id { get; set; }
    public int BillOfMaterialDetailId { get; set; }
    public BillOfMaterialDetailType BillOfMaterialDetailType { get; set; }
    public short? SemiProductBillOfMaterialVersion { get; set; }
    public double Value { get; set; }
    public double Consumed { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double UnitConversionRatio { get; set; }
    public int OperationSequenceId { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double ForQty { get; set; }
    public bool IsPackingMaterial { get; set; }
    public bool LimitedSerialBuffer { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

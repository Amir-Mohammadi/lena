using lena.Domains.Enums;
namespace lena.Models.Planning.OperationConsumingMaterial
{
  public class AddOperationConsumingMaterialInput
  {
    public double Value { get; set; }
    public int BillOfMaterialDetailId { get; set; }
    public bool LimitedSerialBuffer { get; set; }
  }
}

using lena.Domains.Enums;
namespace lena.Models.Planning.OperationConsumingMaterial
{
  public class EditOperationConsumingMaterialInput
  {
    public int Id { get; set; }
    public int BillOfMaterialDetailId { get; set; }
    public double Value { get; set; }
    public bool LimitedSerialBuffer { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

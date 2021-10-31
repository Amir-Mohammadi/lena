using lena.Domains.Enums;
namespace lena.Models.Planning.OperationConsumingMaterial
{
  public class OperationConsumingMaterialResult
  {
    public int Id { get; set; }
    public int BillOfMaterialDetailId { get; set; }
    public double Value { get; set; }
    public int OperationSequenceId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

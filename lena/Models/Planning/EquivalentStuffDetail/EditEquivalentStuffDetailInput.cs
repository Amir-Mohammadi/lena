using lena.Domains.Enums;
namespace lena.Models.Planning.EquivalentStuffDetail
{
  public class EditEquivalentStuffDetailInput
  {
    public int EquivalentStuffId { get; set; }
    public int Id { get; set; }
    public int StuffId { get; set; }
    public short? StuffBillOfMaterialVersion { get; set; }
    public double ForQty { get; set; }
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

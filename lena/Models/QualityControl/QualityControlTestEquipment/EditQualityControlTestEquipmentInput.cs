using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class EditQualityControlTestEquipmentInput
  {
    public int Id { get; set; }
    public int TestEquipmentId { get; set; }
    public long QualityControlTestId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

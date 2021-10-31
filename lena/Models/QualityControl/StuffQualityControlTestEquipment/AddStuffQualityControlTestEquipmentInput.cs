using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class AddStuffQualityControlTestEquipmentInput
  {
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public long QualityControlTestEquipmentQualityControlTestId { get; set; }
    public int QualityControlEquipmentTestEquipmentId { get; set; }
  }
}

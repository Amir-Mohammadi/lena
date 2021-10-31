using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class AddQualityControlTestEquipmentInput
  {
    public long QualityControlTestId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
  }
}

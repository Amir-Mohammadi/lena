using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class QualityControlTestEquipmentComboResult
  {
    public string Name { get; set; }
    public int TestEquipmentId { get; set; }
    public long QualityControlTestId { get; set; }
    public string QualityControlTestName { get; set; }
  }
}

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlTestCondition
{
  public class AddQualityControlTestConditionInput
  {
    public long QualityControlTestId { get; set; }
    public string Condition { get; set; }
  }
}

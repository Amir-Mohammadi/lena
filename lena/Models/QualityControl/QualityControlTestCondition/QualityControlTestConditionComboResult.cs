using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlTestCondition
{
  public class QualityControlTestConditionComboResult
  {
    public string Condition { get; set; }
    public int TestConditionId { get; set; }
    public long QualityControlTestId { get; set; }
    public string QualityControlTestName { get; set; }
  }
}

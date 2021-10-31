using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlTestCondition
{
  public class QualityControlTestConditionResult
  {
    public int TestConditionId { get; set; }
    public string Condition { get; set; }
    public long QualityControlTestId { get; set; }
    public string QualityControlTestName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

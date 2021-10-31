using lena.Domains.Enums;
namespace lena.Models.QualityControl.TestCondition
{
  public class DeleteTestConditionInput
  {
    public int TestConditionId { get; set; }
    public int QualityControlTestId { get; set; }

  }
}

using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffQualityControlTestCondition
{
  public class DeleteStuffQualityControlTestConditionInput
  {
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public long QualityControlTestConditionQualityControlTestId { get; set; }
    public int QualityControlConditionTestConditionId { get; set; }

  }
}

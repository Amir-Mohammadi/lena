using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlTestCondition
{
  public class EditQualityControlTestConditionInput
  {
    public int Id { get; set; }
    public int TestConditionId { get; set; }
    public long QualityControlTestId { get; set; }
    public string Condition { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

using lena.Domains.Enums;
namespace lena.Models.QualityControl.TestCondition
{
  public class EditTestConditionInput
  {
    public int Id { get; set; }
    public string Condition { get; set; }
    public int QualityControlTestConditionId { get; set; }
    public byte[] QualityControlTestConditionRowVersion { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffQualityControlTestCondition
{
  public class EditStuffQualityControlTestConditionInput
  {
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public long QualityControlTestConditionQualityControlTestId { get; set; }
    public int QualityControlConditionTestConditionId { get; set; }

    public double Min { get; set; }
    public double Max { get; set; }
    public double TestedQty { get; set; }
    public int QualityControlTestUnitId { get; set; }
    public string AcceptanceLimit { get; set; }
    public ToleranceType ToleranceType { get; set; }
    public string Description { get; set; }

    public byte[] RowVersion { get; set; }
  }
}

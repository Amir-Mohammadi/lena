using lena.Models.QualityControl.QualityControlSample;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmationItem
{
  public class AddQualityControlConfirmationItemInput
  {
    public int QualityControlItemId { get; set; }
    public double TestQty { get; set; }
    public double ConsumeQty { get; set; }
    public byte UnitId { get; set; }
    public string Description { get; set; }

    public EditQualityControlSampleInput[] EditQualityControlSampleInputs { get; set; }
  }
}

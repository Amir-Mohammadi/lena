using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlSample
{
  public class AddQualityControlSampleInput
  {
    public double Qty { get; set; }
    public int QualityControlId { get; set; }
    public int QualityControlItemId { get; set; }
    public QualityControlSampleStatus Status { get; set; }
  }
}

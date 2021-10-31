using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlSample
{
  public class EditQualityControlSampleInput
  {
    public int Id { get; set; }
    public int Qty { get; set; }
    public double? TestQty { get; set; }
    public double? ConsumeQty { get; set; }
    public int QualityControlItemId { get; set; }
    public bool DelivarySampleToWarehouse { get; set; }
    public QualityControlSampleStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

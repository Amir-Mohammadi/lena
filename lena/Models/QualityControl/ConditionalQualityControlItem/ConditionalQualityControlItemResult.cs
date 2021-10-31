using lena.Domains.Enums;
namespace lena.Models.QualityControl.ConditionalQualityControlItem
{
  public class ConditionalQualityControlItemResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public string Description { get; set; }
    public int StuffId { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public int QualityControlConfirmationItemId { get; set; }
    public double QualityControlConfirmationItemRemainedQty { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

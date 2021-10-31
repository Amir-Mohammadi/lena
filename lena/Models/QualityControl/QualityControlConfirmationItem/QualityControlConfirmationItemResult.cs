using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmationItem
{
  public class QualityControlConfirmationItemResult
  {
    public int Id { get; set; }
    public double RemainedQty { get; set; }
    public double TestQty { get; set; }
    public double ConsumeQty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int QualityControlItemId { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public int QualityControlConfirmationId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

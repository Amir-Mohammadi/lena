using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlSample
{
  public class QCSampleDeliverWarehouseInput
  {
    public int Id { get; set; }
    public int QualityControlId { get; set; }
    public int QualityControlItemId { get; set; }
    public double TestQty { get; set; }
    public double ConsumeQty { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlItem
{
  public class AddQualityControlItemTransactionInput
  {
    public long? StuffSerialCode { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public int? ReturnOfSaleId { get; set; }
    public string Description { get; set; }
  }
}

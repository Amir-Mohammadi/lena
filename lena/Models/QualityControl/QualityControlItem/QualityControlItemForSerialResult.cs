using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlItem
{
  public class QualityControlItemForReturnOfSaleResult
  {
    public int? StuffId { get; set; }
    public long? StuffSerialCode { get; set; }
    public double? Qty { get; set; }
    public double? QualityControlPassedQty { get; set; }
    public double? QualityControlFailedQty { get; set; }
    public double? QualityControlConsumedQty { get; set; }
    public double? ReceiptQualityControlPassedQty { get; set; }
    public double? ReceiptQualityControlFailedQty { get; set; }
    public double? ReceiptQualityControlConsumedQty { get; set; }
    public int? StoreReceiptId { get; set; }
    public int? ReturnOfSaleId { get; set; }
    public int? ExitReceiptId { get; set; }
  }
}

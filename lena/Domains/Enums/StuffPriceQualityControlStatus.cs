using lena.Domains.Enums;
namespace lena.Domains.Enums
{

  public enum StuffPriceQualityControlStatus : byte
  {
    ReceiptedAndNotQulaityControled = 0,
    NotReceiptedAndNotQualityControled = 1,
    ReceiptedAndQualityControlConfirmed = 2,
    ReceiptedAndInQualityControl = 3,
    ReceiptedAndQualityControlFailed = 4,
  }
}

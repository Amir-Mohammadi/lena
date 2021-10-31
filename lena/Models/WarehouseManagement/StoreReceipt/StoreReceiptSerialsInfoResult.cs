using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceipt
{
  public class StoreReceiptSerialsInfoResult
  {
    public int SerialProfileCode { get; set; }
    public int StoreReceiptId { get; set; }
    public int StuffId { get; set; }
    public string MinSerial { get; set; }
    public string MaxSerial { get; set; }
  }
}

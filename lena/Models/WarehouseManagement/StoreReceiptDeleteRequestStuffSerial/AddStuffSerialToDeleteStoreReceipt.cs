using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceiptDeleteRequestStuffSerial
{
  public class AddStuffSerialToDeleteStoreReceipt
  {
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public int StuffId { get; set; }
    public long StuffSerialCode { get; set; }
    public string Serial { get; set; }
  }
}

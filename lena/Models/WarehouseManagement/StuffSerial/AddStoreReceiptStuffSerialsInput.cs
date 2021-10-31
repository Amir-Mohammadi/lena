using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSerial
{
  public class AddStoreReceiptStuffSerialsInput
  {
    public int StoreReceiptId { get; set; }
    public int StuffId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public double QtyPerBox { get; set; }
  }
}

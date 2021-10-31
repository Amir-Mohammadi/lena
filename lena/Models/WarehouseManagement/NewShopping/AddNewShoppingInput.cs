using lena.Models.WarehouseManagement.StoreReceipt;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.NewShopping
{
  public class AddNewShoppingInput : AddStoreReceiptInput
  {
    public int BoxNo { get; set; }
    public int LadingId { get; set; }
    public double QtyPerBox { get; set; }
    public int LadingItemId { get; set; }
    public LadingItemDetails[] LadingItemDetails { get; set; }
  }
  public class LadingItemDetails
  {
    public int LadingItemDetailId { get; set; }
    public double LadingItemDetailQty { get; set; }
  }
}

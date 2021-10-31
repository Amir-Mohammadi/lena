using lena.Models.WarehouseManagement.StoreReceiptDeleteRequestStuffSerial;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceiptDeleteRequestInput
{
  public class AddStoreReceiptDeleteRequestInput
  {
    public int StoreReceiptId { get; set; }
    public string Description { get; set; }
    public AddStuffSerialToDeleteStoreReceipt[] StuffSerials { get; set; }

  }
}
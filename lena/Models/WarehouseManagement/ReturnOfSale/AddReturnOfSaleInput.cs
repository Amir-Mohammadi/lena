using lena.Models.WarehouseManagement.StoreReceipt;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ReturnOfSale
{
  public class AddReturnOfSaleInput : AddStoreReceiptInput
  {
    public string Serial { get; set; }
    public int? SendProductId { get; set; }
    public int PreparingSendingStuffId { get; set; }
    public string ExitReceiptCode { get; set; }

  }
}

using lena.Domains.Enums;
using lena.Models.WarehouseManagement.NewShopping;
using lena.Models.WarehouseManagement.ReturnOfSale;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceipt
{
  public class AddStoreReceiptsInput
  {
    public int? PrinterId { get; set; }
    public SerialPrintType? PrintType { get; set; }
    public AddNewShoppingInput[] AddNewShoppings { get; set; }
    public AddReturnOfSaleInput[] AddReturnOfSales { get; set; }
    public int InboundCargoId { get; set; }
    public bool? PrintBarcodeFooterText { get; set; }
    public string Description { get; set; }
  }
}

using lena.Domains.Enums;
namespace lena.Models.Accounting.PurchasePrice
{
  public class SavePurchasePricesInput
  {
    public int ReceiptId { get; set; }

    public byte[] RowVersion { get; set; }
    public AddPurchasePriceInput[] AddPurchasePrices { get; set; }
    public EditPurchasePriceInput[] EditPurchasePrices { get; set; }
    public DeletePurchasePriceInput[] DeletePurchasePrices { get; set; }

  }
}

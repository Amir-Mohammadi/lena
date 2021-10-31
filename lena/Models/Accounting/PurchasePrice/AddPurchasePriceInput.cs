using lena.Domains.Enums;
namespace lena.Models.Accounting.PurchasePrice
{
  public class AddPurchasePriceInput
  {
    public int StoreReceiptId { get; set; }
    public int StuffId { get; set; }
    public double Price { get; set; }
    public double RialPrice { get; set; }
    public double DutyCost { get; set; }
    public double TransferCost { get; set; }
    public double OtherCost { get; set; }
    public double Discount { get; set; }
    public byte CurrencyId { get; set; }
    public double CurrencyRate { get; set; }
  }
}

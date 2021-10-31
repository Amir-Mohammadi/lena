using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class LadingItemPriceInput
  {
    public int StuffId { get; set; }
    public double LadingItemStuffValue { get; set; }
    public int LadingItemCurrencyId { get; set; }

    public int BankOrderCurrencyId { get; set; }

  }
}

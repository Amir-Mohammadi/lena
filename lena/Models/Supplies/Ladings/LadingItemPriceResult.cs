using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class LadingItemPriceResult
  {

    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }

    public double LadingItemStuffValue { get; set; }
    public int LadingItemCurrencyId { get; set; }
    public string LadingItemCurrencyTitle { get; set; }
    public string LadingItemBaseCurrencySign { get; set; }

    public double LadingItemConvertedPrice { get; set; }
    public int BankOrderCurrencyId { get; set; }
    public string BaseCurrencySign { get; set; }

  }
}

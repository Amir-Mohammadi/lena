using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class DutyCostLadingItemsResult : FullLadingItemsResult
  {
    public double? StuffPriceCurrencyToRialRate { get; set; }
    public double? StuffPriceInRial => StuffPriceCurrencyToRialRate * StuffPrice;
    public double? TotalStuffPriceInRial => StuffPriceInRial * LadingItemQty;
  }
}

using lena.Domains.Enums;
namespace lena.Models.Accounting.BillOfMaterialPriceHistoryCurrencyRates
{
  public class GetBillOfMaterialPriceHistoryCurrencyRateInput
  {
    public int? BillOfMaterialPriceHistoryId { get; set; }
    public int? FromCurrencyId { get; set; }
    public int? ToCurrencyId { get; set; }
    public double? Rate { get; set; }
  }
}

using lena.Domains.Enums;
namespace lena.Models.Accounting.BillOfMaterialPriceHistoryCurrencyRates
{
  public class BillOfMaterialPriceHistoryCurrencyRateResult
  {
    public int Id { get; set; }
    public int BillOfMaterialPriceHistoryId { get; set; }
    public int FromCurrencyId { get; set; }
    public string FromCurrencyTitle { get; set; }
    public int ToCurrencyId { get; set; }
    public string ToCurrencyTitle { get; set; }
    public double Rate { get; set; }
    public byte[] RowVersion { get; set; }

  }
}

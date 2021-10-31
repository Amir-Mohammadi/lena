using lena.Models.ApplicationBase.CurrencyRate;

using lena.Domains.Enums;
namespace lena.Models.Accounting.BillOfMaterialPriceHistory
{
  public class AddBillOfMaterialPriceHistoryInput
  {
    public int StuffId { get; set; }
    public int Version { get; set; }
    public byte CurrencyId { get; set; }
    public double TotalPrice { get; set; }
    public CurrencyRateValue[] CurrencyRates { get; set; }
    public AddBillOfMaterialPriceHistoryDetailInput[] Details { get; set; }
  }
}

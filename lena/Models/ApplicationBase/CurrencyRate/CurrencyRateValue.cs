using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.CurrencyRate
{
  public class CurrencyRateValue
  {
    public byte FromCurrencyId { get; set; }
    public byte ToCurrencyId { get; set; }
    public double Rate { get; set; }
  }
}

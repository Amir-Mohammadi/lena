using System;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.CurrencyRate
{
  public class AddCurrencyRateInput
  {
    public DateTime FromDate { get; set; }
    public double CommissionRate { get; set; }
    public double ConversionRate { get; set; }
    public byte FromCurrencyId { get; set; }
    public byte ToCurrencyId { get; set; }
    public int ExchangeId { get; set; }
  }
}

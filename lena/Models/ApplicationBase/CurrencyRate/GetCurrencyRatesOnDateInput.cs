using System;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.CurrencyRate
{
  public class GetCurrencyRatesOnDateInput
  {
    public int? FromCurrencyId { get; set; }
    public int? ToCurrencyId { get; set; }
    public DateTime? DateTime { get; set; }
  }
}

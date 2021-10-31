using System;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.CurrencyRate
{
  public class CurrencyRateResult
  {
    public int Id { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime CreationTime { get; set; }
    public double Commission { get; set; }
    public double Rate { get; set; }
    public int FromCurrencyId { get; set; }
    public string FromCurrencyTitle { get; set; }
    public string FromCurrencyCode { get; set; }
    public string FromCurrencySign { get; set; }
    public int ToCurrencyId { get; set; }
    public string ToCurrencyTitle { get; set; }
    public string ToCurrencyCode { get; set; }
    public string ToCurrencySign { get; set; }
    public int ExchangeId { get; set; }
    public string ExchangeCode { get; set; }
    public string ExchangeName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

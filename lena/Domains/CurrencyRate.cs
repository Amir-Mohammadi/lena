using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CurrencyRate : IEntity
  {
    protected internal CurrencyRate()
    {
    }
    public int Id { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime DateTime { get; set; }
    public double Commission { get; set; }
    public double Rate { get; set; }
    public byte FromCurrencyId { get; set; }
    public byte ToCurrencyId { get; set; }
    public int ExchangeId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Currency FromCurrency { get; set; }
    public virtual Currency ToCurrency { get; set; }
    public virtual Cooperator Exchange { get; set; }
  }
}

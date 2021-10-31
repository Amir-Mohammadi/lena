using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class BillOfMaterialPriceHistoryCurrencyRate : IEntity
  {
    public int Id { get; set; }
    public int BillOfMaterialPriceHistoryId { get; set; }
    public virtual BillOfMaterialPriceHistory BillOfMaterialHistory { get; set; }
    public byte FromCurrencyId { get; set; }
    public Currency FromCurrency { get; set; }
    public byte ToCurrencyId { get; set; }
    public Currency ToCurrency { get; set; }
    public double Rate { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

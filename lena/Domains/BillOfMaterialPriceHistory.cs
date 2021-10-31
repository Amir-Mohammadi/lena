using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class BillOfMaterialPriceHistory : IEntity
  {
    public BillOfMaterialPriceHistory()
    {
      this.BillOfMaterialPriceHistoryDetails = new HashSet<BillOfMaterialPriceHistoryDetail>();
      this.BillOfMaterialPriceHistoryCurrencyRates = new HashSet<BillOfMaterialPriceHistoryCurrencyRate>();
    }
    public int Id { get; set; }
    public int StuffId { get; set; }
    public virtual Stuff Stuff { get; set; }
    public int? Version { get; set; }
    public byte CurrencyId { get; set; }
    public Currency Currency { get; set; }
    public double TotalPrice { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<BillOfMaterialPriceHistoryDetail> BillOfMaterialPriceHistoryDetails { get; set; }
    public virtual ICollection<BillOfMaterialPriceHistoryCurrencyRate> BillOfMaterialPriceHistoryCurrencyRates { get; set; }
  }
}

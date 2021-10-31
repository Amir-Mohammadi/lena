using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Currency : IEntity
  {
    protected internal Currency()
    {
      this.StuffPriceDiscrepancies = new HashSet<StuffPriceDiscrepancy>();
      this.PurchaseOrders = new HashSet<PurchaseOrder>();
      this.StuffPrices = new HashSet<StuffPrice>();
      this.StuffBasePriceCustoms = new HashSet<StuffBasePriceCustoms>();
      this.FinancialLimits = new HashSet<FinancialLimit>();
      this.BankOrders = new HashSet<BankOrder>();
      this.FinancialAccounts = new HashSet<FinancialAccount>();
      this.ToCurrencyRates = new HashSet<CurrencyRate>();
      this.FromCurrencyRates = new HashSet<CurrencyRate>();
      this.Finances = new HashSet<Finance>();
      this.BillOfMaterialPriceHistoryFromCurrencyRates = new HashSet<BillOfMaterialPriceHistoryCurrencyRate>();
      this.BillOfMaterialPriceHistoryToCurrencyRates = new HashSet<BillOfMaterialPriceHistoryCurrencyRate>();
      this.BillOfMaterialPriceHistories = new HashSet<BillOfMaterialPriceHistory>();
      this.PriceAnnunciationItems = new HashSet<PriceAnnunciationItem>();
      this.PriceInquries = new HashSet<PriceInquiry>();
    }
    public byte Id { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public string Sign { get; set; }
    public bool IsMain { get; set; }
    public byte DecimalDigitCount { get; set; }
    public CurrencyType Type { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<StuffPriceDiscrepancy> StuffPriceDiscrepancies { get; set; }
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    public virtual ICollection<StuffPrice> StuffPrices { get; set; }
    public virtual ICollection<StuffBasePriceCustoms> StuffBasePriceCustoms { get; set; }
    public virtual ICollection<FinancialLimit> FinancialLimits { get; set; }
    public virtual ICollection<BankOrder> BankOrders { get; set; }
    public virtual ICollection<FinancialAccount> FinancialAccounts { get; set; }
    public virtual ICollection<CurrencyRate> FromCurrencyRates { get; set; }
    public virtual ICollection<CurrencyRate> ToCurrencyRates { get; set; }
    public virtual ICollection<Finance> Finances { get; set; }
    public virtual ICollection<BillOfMaterialPriceHistoryCurrencyRate> BillOfMaterialPriceHistoryFromCurrencyRates { get; set; }
    public virtual ICollection<BillOfMaterialPriceHistoryCurrencyRate> BillOfMaterialPriceHistoryToCurrencyRates { get; set; }
    public virtual ICollection<BillOfMaterialPriceHistory> BillOfMaterialPriceHistories { get; set; }
    public virtual ICollection<PriceAnnunciationItem> PriceAnnunciationItems { get; set; }
    public virtual ICollection<PriceInquiry> PriceInquries { get; set; }
  }
}
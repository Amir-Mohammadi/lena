using System;


using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrder
{
  public class PurchaseOrderWithFinancingsResult
  {
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string ProviderCode { get; set; }
    public int HowToBuyId { get; set; }
    public string HowToBuyTitle { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double Price { get; set; }
    public int CurrencuyId { get; set; }
    public string CurrencyTitle { get; set; }
    public string CurrencySign { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public DateTime Deadline { get; set; }
    public string Description { get; set; }
  }
}

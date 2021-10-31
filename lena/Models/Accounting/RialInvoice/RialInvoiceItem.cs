using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  public class RialInvoiceItem
  {
    public int StuffId { get; set; }
    public int Amount { get; set; }
    public byte UnitId { get; set; }
    public int SourceCurrencyId { get; set; }
    public double CurrencyRate { get; set; }
    public double? UnitPriceInSourceCurrency { get; set; }
    public double UnitPriceInRial { get; set; }
    public double TotalGrossPrice { get; set; }
    public double UnitTransferCost { get; set; }
    public double TotalTransferCost { get; set; }
    public double UnitDutyCost { get; set; }
    public double TotalDutyCost { get; set; }
    public double UnitOtherCost { get; set; }
    public double TotalOtherCost { get; set; }
    public double UnitDiscount { get; set; }
    public double TotalDiscount { get; set; }
    public double UnitNetPrice { get; set; }
    public double TotalNetPrice { get; set; }
  }
}

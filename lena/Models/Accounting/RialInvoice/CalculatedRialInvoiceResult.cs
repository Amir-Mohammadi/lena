using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  public class CalculatedRialInvoiceResult
  {
    public int Row { get; set; }
    public int ReceiptId { get; set; }
    public string CooperatorName { get; set; }
    public int StoreReceiptId { get; set; }
    public int CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double Amount { get; set; }
    public int SourceCurrencyId { get; set; }
    public string SourceCurrencyTitle { get; set; }
    public double CurrencyRate { get; set; }
    public double UnitPriceInSourceCurrency { get; set; }
    public double UnitPriceInRial => CurrencyRate * UnitPriceInSourceCurrency;
    public double TotalGrossPrice => Amount * UnitPriceInRial;
    public double TotalGrossPriceInSourceCurrency => Amount * UnitPriceInSourceCurrency;
    public double UnitTransferCost { get; set; }
    public double TotalTransferCost => UnitTransferCost * Amount;
    public double UnitDutyCost { get; set; }
    public double TotalDutyCost => UnitDutyCost * Amount;
    public double UnitOtherCost { get; set; }
    public double TotalOtherCost => UnitOtherCost * Amount;
    public double UnitDiscount { get; set; }
    public double TotalDiscount => UnitDiscount * Amount;
    public double UnitNetPrice => UnitPriceInRial + UnitTransferCost + UnitDutyCost + UnitOtherCost - UnitDiscount;
    public double TotalNetPrice => Amount * UnitNetPrice;
  }
}

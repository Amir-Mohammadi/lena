using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  public class GetRialInvoicesResult
  {
    public int StuffPriceId { get; set; }
    public int ReceiptId { get; set; }
    public string ReceiptCode { get; set; }
    public int LadingId { get; set; }
    public string LadingCode { get; set; }
    public int? LadingItemId { get; set; }
    public string LadingItemCode { get; set; }
    public int CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public int PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double Amount { get; set; }
    public int? SourceCurrencyId { get; set; }
    public string SourceCurrencyTitle { get; set; }
    public double CurrencyRate { get; set; }
    public double? UnitPriceInSourceCurrency { get; set; }
    public double? UnitPriceInRial { get; set; }
    public double? TotalGrossPrice { get; set; }
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

    public DateTime DateTime { get; set; }
    public DateTime ReceiptDateTime { get; set; }
    public string EmployeeFullName { get; set; }
    public int EmployeeId { get; set; }
    public byte[] RowVersion { get; set; }

  }
}

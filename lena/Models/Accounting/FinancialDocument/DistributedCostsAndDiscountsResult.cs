using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class DistributedCostsAndDiscountsResult
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }

    public int FinancialDocumentId { get; set; }
    public int? FinancialDocumentCostId { get; set; }
    public int? FinancialDocumentDiscountId { get; set; }
    public FinancialDocumentTypeResult? TypeResult { get; set; }
    public double Qty { get; set; }
    public double Amount { get; set; }
    public int AmountCurrencyId { get; set; }
    public string AmountCurrencyTitle { get; set; }
    public double? AmountInRial { get; set; }
    public double? AmountRialRate { get; set; }
    public int? UnitId { get; set; }
    public string UnitName { get; set; }
    public double? GrossWeight { get; set; }
    public double? TotalGrossWeight { get; set; }
    public double? Price { get; set; }
    public double? TotalPrice { get; set; }
    public double? RialRate { get; set; }
    public double? PriceInRials { get; set; }
    public double? TotalPriceInRials { get; set; }
    public int? PriceCurrencyId { get; set; }
    public string PriceCurrencyTitle { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int? ProviderId { get; set; }
    public string ProviderName { get; set; }

    public int? PurchaseOrderGroupId { get; set; }
    public string PurchaseOrderGroupCode { get; set; }
    public int? PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }

    public int? CargoId { get; set; }
    public string CargoCode { get; set; }
    public int? CargoItemId { get; set; }
    public string CargoItemCode { get; set; }

    public int? LadingId { get; set; }
    public string LadingCode { get; set; }
    public int? LadingItemId { get; set; }
    public string LadingItemCode { get; set; }
    public bool? IsTemp { get; set; }
  }
}

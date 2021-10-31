using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class LadingItemResult
  {
    public string Code { get; set; }
    public int Id { get; set; }
    public double? Qty { get; set; }
    public double? ReceiptedQty { get; set; }
    public LadingItemStatus Status { get; set; }
    public int LadingId { get; set; }

    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public string CargoItemProviderName { get; set; }
    public string CargoItemProviderCode { get; set; }
    public string UnitName { get; set; }
    public byte UnitId { get; set; }

    public double? StuffGrossWeight { get; set; }
    public double? StuffTotalGrossWeight { get; set; }

    public double CargoItemQty { get; set; }
    public double CargoItemWithoutLadingQty { get; set; }
    public double? CargoItemReceiptedQty { get; set; }

    public double? StuffValue { get; set; }
    public int? PurchaseOrderCurrencyId { get; set; }
    public string PurchaseOrderCurrencyTitle { get; set; }

    public byte[] RowVersion { get; set; }
    public string CargoCode { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; }
    public DateTime DateTime { get; set; }
    public double? ConversionRatio { get; set; }
    public int DecimalDigitCount { get; set; }
  }
}
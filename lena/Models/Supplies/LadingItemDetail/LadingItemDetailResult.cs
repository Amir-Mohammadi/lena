using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingItemDetail
{
  public class LadingItemDetailResult
  {
    public int Id { get; set; }
    public double Qty { get; set; }
    public string Code { get; set; }
    public DateTime DateTime { get; set; }
    public double ReceiptedQty { get; set; }
    public double RemainedQty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public byte[] RowVersion { get; set; }

    public int LadingId { get; set; }
    public string LadingCode { get; set; }
    public int LadingItemId { get; set; }
    public double LadingItemQty { get; set; }
    public DateTime LadingItemDateTime { get; set; }
    public LadingItemStatus LadingItemStatus { get; set; }
    public double LadingItemReceiptedQty { get; set; }
    public double LadingItemRemainedQty { get; set; }


    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }

    public int? ProviderId { get; set; }
    public string ProviderName { get; set; }
    public int PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public DateTime PurchaseOrderDeadline { get; set; }
    public string EmployeeFullName { get; set; }
    public int HowToBuyId { get; set; }
    public string HowToBuyTitle { get; set; }
    public string LadingDesctiption { get; set; }



    public string CargoCode { get; set; }
    public int CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public double CargoItemQty { get; set; }
    public DateTime CargoItemDateTime { get; set; }

    public int CargoItemDetailId { get; set; }
    public string CargoItemDetailCode { get; set; }
    public double CargoItemDetailQty { get; set; }
    public double CargoItemDetailWithoutLadingQty { get; set; }
    public double? CargoItemDetailReceiptedQty { get; set; }
    public double ConversionRatio { get; set; }
    public int DecimalDigitCount { get; set; }
    public int CargoItemUnitId { get; set; }
    public string CargoItemUnitName { get; set; }
  }
}
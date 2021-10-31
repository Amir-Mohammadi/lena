using lena.Domains.Enums;

using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class FullLadingItemsResult
  {
    public FullLadingItemsResult()
    {

    }
    public int LadingItemId { get; set; }
    public DateTime LadingItemDateTime { get; set; }
    public string LadingCode { get; set; }
    public LadingItemStatus LadingItemStatus { get; set; }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double? StuffNetWeight { get; set; }
    public double? StuffGrossWeight { get; set; }
    public double? StuffTotalGrossWeight { get; set; }
    public double? StuffPrice { get; set; }
    public int? CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public int? ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string PurchaseOrderCode { get; set; }
    public int? CargoId { get; set; }
    public string CargoCode { get; set; }
    public int? CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public double? CargoItemQty { get; set; }
    public DateTime CargoItemDateTime { get; set; }
    public string CargoItemProviderName { get; set; }
    public string CargoItemProviderCode { get; set; }
    public CargoItemStatus CargoItemStatus { get; set; }
    public string CargoItemEmployeeFullName { get; set; }
    public DateTime CargoItemEstimateDateTime { get; set; }
    public DateTime PurchaseOrderDeadline { get; set; }
    public string PurchaseOrderCurrencyTitle { get; set; }
    public string UnitName { get; set; }
    public int? UnitId { get; set; }
    public double LadingItemQty { get; set; }
    public double? CargoItemWithoutLadingQty { get; set; }
    public double? CargoItemReceiptedQty { get; set; }
    public int? UserId { get; set; }
    public string UserFullName { get; set; }
    public int LadingId { get; set; }
    public string KotazhCode { get; set; }
    public string SataCode { get; set; }
    public int? BankOrderId { get; set; }
    public string BankName { get; set; }
    public string BankOrderCode { get; set; }
    public string BankOrderNumber { get; set; }
    public int? CustomhouseId { get; set; }
    public string CustomhouseName { get; set; }
    public DateTime? TransportDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime DateTime { get; set; }
    public string CurrentBankOrderLogDescription { get; set; }
    public string CurrentBankOrderStatusName { get; set; }
    public string CurrentCustomhouseStatusName { get; set; }
    public string CurrentCustomhouseLogDescription { get; set; }
    public int EmployeeId { get; set; }
    public string PlanCode { get; set; }
  }
}

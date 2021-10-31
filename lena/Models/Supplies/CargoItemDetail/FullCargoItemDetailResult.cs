using System;
using System.Linq;
using lena.Domains.Enums;
using lena.Models.Supplies.LadingItemDetail;
using lena.Models.Supplies.Ladings;

//using lena.Services.Common.Helpers;

using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItemDetail
{
  public class FullCargoItemDetailResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public double? Qty { get; set; }
    public double? ReceiptedQty { get; set; }

    public byte[] RowVersion { get; set; }
    public IQueryable<LadingItemResult> LadingItems { get; set; }

    public IQueryable<LadingItemDetailResult> LadingItemDetails { get; set; }

    public int CargoId { get; set; }
    public string CargoCode { get; set; }

    public int CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public CargoItemStatus CargoItemStatus { get; set; }
    public DateTime PurchaseOrderDeadline { get; set; }
    public DateTime? CargoItemDateTime { get; set; }
    public BuyingProcess? BuyingProcess { get; set; }
    public int DecimalDigitCount { get; set; }
    public RiskLevelStatus RiskLevelStatus { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double QualityControlPassedQty { get; set; }
    public double QualityControlFailedQty { get; set; }
    public string EmployeeFullName { get; set; }
    public string HowToBuyTitle { get; set; }
    public string ProviderName { get; set; }
    public PurchaseOrderType PurchaseOrderType { get; set; }
    public double? StuffGrossWeight { get; set; }
    public double? StuffNetWeight { get; set; }
    public double? PurchaseOrderPrice { get; set; }
    public string PurchaseOrderCurrencyTitle { get; set; }
    public double RemainedCargoItemQty { get; set; }
    public int stuffId { get; set; }
    public int? HowToBuyId { get; set; }
    public int ProviderId { get; set; }
    public DateTime? EstimateDateTime { get; set; }
    public DateTime? CargoItemDetailDeadLine { get; set; }

    public string PlanCode { get; set; }

    public bool IsArchived { get; set; }
    public string UnitName { get; set; }
    public string HowToBuyDetailTitle { get; set; }
    public DateTime? CurrentPurchaseStepDateTime { get; set; }
    public DateTime? CurrentPurchaseStepFollowUpDateTime { get; set; }
    public IQueryable<LadingsWithLadinItem> Ladings { get; set; }
    public string CurrentPurchaseStepEmployeeFullName { get; set; }
    public string LadingCodes => string.Join(", ", Ladings.ToList().Select(i => i.Code));
    public string LadingQtys => string.Join(", ", Ladings.ToList().Select(i => i.Qty));
    public string LadingDescription => string.Join(", ", Ladings.ToList().Select(i => $"(کد: {i.Code} , مقدار: {i.Qty} , تاریخ بارنامه: {DateTimeHelper.ConvertUtcToPersianDateTime(i.DateTime)})"));

  }
}

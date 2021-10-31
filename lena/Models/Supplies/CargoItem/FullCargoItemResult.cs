using lena.Domains.Enums;
using lena.Models.Supplies.Ladings;
//using lena.Services.Common.Helpers;
using System;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class FullCargoItemResult
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }

    public int StuffId { get; set; }

    public int EmployeeId { get; set; }

    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public StuffType StuffType { get; set; }
    public double? StuffNetWeight { get; set; }
    public double? StuffGrossWeight { get; set; }
    public IQueryable<LadingsWithLadinItem> Ladings { get; set; }
    public string LadingCodes => string.Join(", ", Ladings.ToList().Select(i => i.Code));
    public string LadingQtys => string.Join(", ", Ladings.ToList().Select(i => i.Qty));
    public string LadingDescription => string.Join(", ", Ladings.ToList().Select(i => $"(کد: {i.Code} , مقدار: {i.Qty} , تاریخ بارنامه: {DateTimeHelper.ConvertUtcToPersianDateTime(i.DateTime)})"));

    public int CargoId { get; set; }
    public string CargoCode { get; set; }

    public int? CurrentPurchaseStepId { get; set; }
    public int? CurrentPurchaseStepUserId { get; set; }
    public DateTime? CurrentPurchaseStepDateTime { get; set; }
    public DateTime? CurrentPurchaseStepFollowUpDateTime { get; set; }
    public string CurrentPurchaseStepEmployeeFullName { get; set; }

    public int? HowToBuyId { get; set; }
    public string HowToBuyTitle { get; set; }
    public int? HowToBuyDetailId { get; set; }
    public string HowToBuyDetailTitle { get; set; }

    public Guid? ForwarderDocumentId { get; set; }
    public int? ProviderId { get; set; }
    public string ProviderName { get; set; }
    public int? ForwarderId { get; set; }
    public string ForwarderName { get; set; }
    public int PurchaseOrderId { get; set; }
    public DateTime PurchaseOrderDeadline { get; set; }
    public PurchaseOrderType PurchaseOrderType { get; set; }
    public string PurchaseOrderCode { get; set; }

    public double? PurchaseOrderPrice { get; set; }
    public int? PurchaseOrderCurrencyId { get; set; }
    public string PurchaseOrderCurrencyTitle { get; set; }

    public double? Qty { get; set; }
    public double? ReceiptedQty { get; set; }

    public double? QualityControlPassedQty { get; set; }
    public double? QualityControlFailedQty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public string EmployeeFullName { get; set; }
    public double? RemainedCargoItemQty { get; set; }
    public DateTime EstimateDateTime { get; set; }
    public DateTime? LatestBaseEntityDocumentDateTime { get; set; }
    public DateTime CargoItemDateTime { get; set; }
    public string CargoItemCode { get; set; }
    public int CargoItemId { get; set; }
    public CargoItemStatus Status { get; set; }
    public string LatestBaseEntityDocumentDescription { get; set; }
    public bool IsArchived { get; set; }
    public BuyingProcess? BuyingProcess { get; set; }
    public RiskLevelStatus RiskLevelStatus { get; set; }
    public string LatestRiskTitle { get; set; }
    public DateTime? LatestRiskCreateDateTime { get; set; }
    public string PlanCode { get; set; }
    public int PlanCodeId { get; set; }

  }
}

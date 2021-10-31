using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequest
{
  public class PurchaseRequestResult
  {
    public int Id { get; set; }
    public string RequestCode { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public StuffType StuffType { get; set; }
    public string PlanCode { get; set; }
    public int? PlanCodeId { get; set; }
    public int? ConfirmerEmployeeId { get; set; }
    public int CostCenterId { get; set; }
    public string StuffPurchaseCategoryName { get; set; }
    public string CostCenterName { get; set; }
    public string CostCenterDescription { get; set; }
    public string ProjectCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime? ConfirmDate { get; set; }
    public string ConfirmerFullName { get; set; }
    public DateTime Deadline { get; set; }
    public double? Qty { get; set; }
    public double? RequestQty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double ConversionRatio { get; set; }
    public string Description { get; set; }
    public string ConfirmationDescription { get; set; }
    public double? RemainedQty { get; set; }
    public PurchaseRequestStatus Status { get; set; }
    public int StuffCategoryId { get; set; }
    public string StuffCategoryName { get; set; }
    public int? StuffCategoryParentId { get; set; }
    public string StuffCategoryParentName { get; set; }
    public string EmployeeFullName { get; set; }
    public int? DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int? ResponsibleEmployeeId { get; set; }
    public string ResponsibleEmployeeFullName { get; set; }
    public int UnitTypeId { get; set; }
    public double? OrderedQty { get; set; }
    public double? CargoedQty { get; set; }
    public double? NotCargoedQty { get; set; }
    public double? NoneReceiptedQty { get; set; }
    public double? ReceiptedQty { get; set; }
    public double? QualityControlPassedQty { get; set; }
    public double? QualityControlFailedQty { get; set; }
    public bool IsArchived { get; set; }
    public string LatestBaseEntityDocumentDescription { get; set; }
    public DateTime? LatestBaseEntityDocumentDateTime { get; set; }
    public string LatestRiskTitle { get; set; }
    public DateTime? LatestRiskCreateDateTime { get; set; }
    public RiskLevelStatus RiskLevelStatus { get; set; }

    public byte[] RowVersion { get; set; }

    public int? PurchaseRequestStepDetailId { get; set; }
    public DateTime? PurchaseRequestStepChangeTime { get; set; }
    public string PurchaseRequestStepChangeUserFullName { get; set; }
    public int? PurchaseRequestStepId { get; set; }
    public string PurchaseRequestStepName { get; set; }
    public DateTime? MaxEstimateDateTime { get; set; }
    public string PurchaseRequestStepDetailDescription { get; set; }
    public double? GrossWeight { get; set; }
    public double? NetWeight { get; set; }
    public int DecimalDigitCount { get; set; }
    public double? CurrentStuffBasePrice { get; set; }
    public int? CurrentStuffBasePriceCurrencyId { get; set; }
    public string CurrentStuffBasePriceCurrencyTitle { get; set; }
    public bool Essential { get; set; }
    public int? EmployeeRequesterId { get; set; }
    public string EmployeeRequesterFullName { get; set; }
    public Guid? DocumentId { get; set; }
    public string Link { get; set; }
    public PurchaseRequestSupplyType? SupplyType { get; set; }
  }
}

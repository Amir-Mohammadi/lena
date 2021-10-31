using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrderWorkSpace
{
  public class PurchaseOrderWorkSpaceResult
  {
    #region PurchaseRequest
    public int PurchaseRequestId { get; set; }
    public string RequestCode { get; set; }
    public int StuffId { get; set; }

    public string StuffCode { get; set; }
    public string PlanCode { get; set; }
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
    public string Description { get; set; }
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
    public double? RemainedQty { get; set; }
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
    public byte[] RowVersion { get; set; }

    public int? PurchaseRequestStepDetailId { get; set; }
    public DateTime? PurchaseRequestStepChangeTime { get; set; }
    public string PurchaseRequestStepChangeUserFullName { get; set; }
    public int? PurchaseRequestStepId { get; set; }
    public string PurchaseRequestStepName { get; set; }
    public DateTime? MaxEstimateDateTime { get; set; }
    public string PurchaseRequestStepDetailDescription { get; set; }

    #endregion

    #region PurchaseOrderDetail
    public int? PurchaseOrderDetailId { get; set; }
    public string PurchaseOrderDetailCode { get; set; }
    public int? PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }


    public double? PurchaseOrderDetailQty { get; set; }
    public int? PurchaseOrderDetailUnitId { get; set; }
    public string PurchaseOrderDetailUnitName { get; set; }
    public double? PurchaseOrderDetailCargoedQty { get; set; }
    public double? PurchaseOrderDetailReceiptedQty { get; set; }
    public double? PurchaseOrderDetailRemainedQty { get; set; }
    public double? PurchaseOrderDetailQualityControlPassedQty { get; set; }



    public double? PurchaseOrderQty { get; set; }
    public int? PurchaseOrderUnitId { get; set; }
    public string PurchaseOrderUnitName { get; set; }
    public int? PurchaseOrderProviderId { get; set; }
    public string PurchaseOrderProviderName { get; set; }
    public double? PurchaseOrderCargoedQty { get; set; }
    public double? PurchaseOrderReceiptQty { get; set; }
    public double? PurchaseOrderRemainedQty { get; set; }
    public DateTime? PurchaseOrderDeadline { get; set; }

    public double? Price { get; set; }
    public int? CurrencyId { get; set; }
    public string ProviderName { get; set; }
    public int? providerId { get; set; }
    public string CurrencyTitle { get; set; }

    public string PurchaseOrderStepDetailDescription { get; set; }
    public DateTime? PurchaseOrderStepChangeTime { get; set; }
    public int? PurchaseOrderStepDetailId { get; set; }

    #endregion

    #region CargoItemDetail
    public int? CargoItemDetailId { get; set; }
    public string CargoItemDetailCode { get; set; }
    public double? CargoItemDetailQty { get; set; }
    public double? CargoItemDetailReceiptQty { get; set; }
    public int? CargoItemDetailUnitId { get; set; }
    public string CargoItemDetailUnitName { get; set; }


    public int? CargoId { get; set; }
    public string CargoCode { get; set; }

    public int? CargoItemId { get; set; }
    public string CargoItemCode { get; set; }

    public double? CargoItemQty { get; set; }
    public double? CargoItemReceiptQty { get; set; }
    public int? CargoItemUnitId { get; set; }
    public string CargoItemUnitName { get; set; }


    public int? CargoItemHowToBuyId { get; set; }

    public string CargoItemHowToBuyTitle { get; set; }

    public int? ForwarderId { get; set; }
    public string ForwarderName { get; set; }
    public BuyingProcess? BuyingProcess { get; set; }


    #endregion

    #region landignItemDetail
    public int? LandignItemDetailId { get; set; }
    public double? LandignItemDetailQty { get; set; }
    public string LandignItemDetailCode { get; set; }
    public DateTime? LandignItemDetailDateTime { get; set; }
    public double? LandignItemDetailReceiptedQty { get; set; }
    public double? LandignItemDetailRemainedQty { get; set; }

    #endregion

    #region NewShoppingDetail
    public double? NewShoppingDetailQty { get; set; }
    public int? NewShoppingDetailUnitId { get; set; }
    public string NewShoppingDetailUnitName { get; set; }
    public double? QualityControlConsumedQty { get; set; }


    #endregion
  }
}

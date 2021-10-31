using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.PlanningWorkSpace
{
  public class PlanningWorkSpaceResult
  {
    #region oredrProperty
    public int OrderItemId { get; set; }
    public string OrderItemCode { get; set; }
    public int OrderItemOrderId { get; set; }
    public int OrderItemStuffId { get; set; }
    public string OrderItemStuffCode { get; set; }
    public string OrderItemStuffName { get; set; }
    public string OrderItemStuffNoun { get; set; }
    public int OrderItemUnitId { get; set; }
    public string OrderItemUnitName { get; set; }
    public int? OrderItemBillOfMaterialVersion { get; set; }
    public string OrderItemBillOfMaterialTitle { get; set; }
    public double OrderItemQty { get; set; }
    public int OrderItemCustomerId { get; set; }
    public string OrderItemCustomerName { get; set; }
    public string OrderItemCustomerCode { get; set; }
    public string OrderItemOrderer { get; set; }
    public int OrderItemOrderTypeId { get; set; }
    public string OrderItemOrderTypeName { get; set; }
    public string OrderItemDocumentNumber { get; set; }
    public OrderDocumentType? OrderItemDocumentType { get; set; }
    public string OrderItemEmployeeFullName { get; set; }
    public DateTime OrderItemRequestDate { get; set; }
    public DateTime OrderItemDeliveryDate { get; set; }
    public DateTime OrderItemDateTime { get; set; }
    public string OrderItemDescription { get; set; }
    public string OrderItemOrderDescription { get; set; }
    public double OrderItemProducedQty { get; set; }
    public double OrderItemPlannedQty { get; set; }
    public double OrderItemBlockedQty { get; set; }
    public double OrderItemPermissionQty { get; set; }
    public double OrderItemSendedQty { get; set; }
    public double OrderItemNotPostedQty { get; set; }
    public OrderItemStatus OrderItemStatus { get; set; }
    public bool OrderItemHasChange { get; set; }
    public bool OrderItemIsArchive { get; set; }
    public bool? OrderItemConfirmationConfirmed { get; set; }
    public DateTime? OrderItemConfirmationDateTime { get; set; }
    public bool? OrderItemCheckOrderItemConfirmed { get; set; }
    public DateTime? OrderItemCheckOrderItemDateTime { get; set; }
    public OrderItemChangeStatus OrderItemChangeStatus { get; set; }

    public byte[] OrderItemRowVersion { get; set; }
    public byte[] OrderItemOrderRowVersion { get; set; }
    #endregion

    #region RequestProperty

    public int? ProductionRequestId { get; set; }
    public string ProductionRequestCode { get; set; }
    public double? ProductionRequestQty { get; set; }
    public string ProductionRequestUnitName { get; set; }
    public int? ProductionRequestUnitId { get; set; }
    public DateTime? ProductionRequestDate { get; set; }
    public DateTime? ProductionRequestDeliveryDate { get; set; }
    public DateTime? ProductionRequestDeadlineDate { get; set; }
    public ProductionRequestStatus? ProductionRequestStatus { get; set; }
    public double? ProductionRequestPlannedQty { get; set; }
    public double? ProductionRequestScheduledQty { get; set; }
    public string ProductionRequestDescription { get; set; }
    public double? ProductionRequestProducedQty { get; set; }

    public byte[] ProductionRequestRowVersion { get; set; }

    #endregion

    #region planProperty

    public int? ProductionPlanId { get; set; }
    public bool? ProductionPlanIsActive { get; set; }
    public bool? ProductionPlanIsPublished { get; set; }

    public double? ProductionPlanQty { get; set; }
    public int? ProductionPlanUnitId { get; set; }
    public string ProductionPlanUnitName { get; set; }

    public ProductionPlanStatus? ProductionPlanStatus { get; set; }

    public DateTime? ProductionPlanEstimatedDate { get; set; }

    public string ProductionPlanDescription { get; set; }

    public byte[] ProductionPlanRowVersion { get; set; }
    #endregion

    #region ScheduleProperty
    public string ProductionScheduleSemiProductStuffName { get; set; }
    public string ProductionScheduleSemiProductStuffCode { get; set; }
    public string ProductionScheduleCode { get; set; }

    public int? ProductionScheduleId { get; set; }
    public string ProductionScheduleStepName { get; set; }
    public string ProductionScheduleLineName { get; set; }
    public bool? ProductionScheduleIsPublished { get; set; }
    public ProductionScheduleStatus? ProductionScheduleStatus { get; set; }
    public double? ProductionScheduleQty { get; set; }
    public string ProductionScheduleUnitName { get; set; }
    public double? ProductionScheduleProducedQty { get; set; }
    public string ProductionScheduleProducedUnitName { get; set; }
    public DateTime? ProductionScheduleDateTime { get; set; }
    public long? ProductionScheduleDuration { get; set; }

    public DateTime? ProductionScheduleToDateTime
    {
      get
      {
        if (ProductionScheduleDateTime.HasValue && ProductionScheduleDuration.HasValue)

          return ProductionScheduleDateTime.Value.AddSeconds(ProductionScheduleDuration.Value);
        else
          return null;

      }

    }
    // public DateTime? ScheduleToDateTime => ScheduleDateTime.Value.AddSeconds(Duration??0);
    public byte[] ProductionScheduleRowVersion { get; set; }
    #endregion


  }
}

using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Planning.PlanningWorkSpace
{
  public class GetPlanningWorkSpaceInput : SearchInput<PlanningWorkSpaceSortType>
  {

    public int? OrderItemCustomerId { get; set; }
    public int? OrderItemStuffId { get; set; }
    public DateTime? OrderItemFromRequestDate { get; set; }
    public DateTime? OrderItemToRequestDate { get; set; }
    public DateTime? OrderItemFromDeliveryDate { get; set; }
    public DateTime? OrderItemToDeliveryDate { get; set; }
    public int? OrderId { get; set; }
    public string OrderItemCode { get; set; }
    public OrderItemStatus? OrderItemStatus { get; set; }
    public OrderItemStatus[] OrderItemStatuses { get; set; }
    public OrderItemStatus[] OrderItemNotHasStatuses { get; set; }
    public bool? OrderItemHasChange { get; set; }
    public bool? OrderItemIsArchive { get; set; }

    public DateTime? ProductionRequestFromDeadline { get; set; }
    public DateTime? ProductionRequestToDeadline { get; set; }
    public ProductionRequestStatus? ProductionRequestStatus { get; set; }
    public ProductionRequestStatus[] ProductionRequestStatuses { get; set; }
    public ProductionRequestStatus[] ProductionRequestNotHasStatuses { get; set; }


    public DateTime? ProductionPlanFromEstimatedDate { get; set; }
    public DateTime? ProductionPlanToEstimatedDate { get; set; }
    public ProductionPlanStatus? ProductionPlanStatus { get; set; }
    public ProductionPlanStatus[] ProductionPlanStatuses { get; set; }
    public ProductionPlanStatus[] ProductionPlanNotHasStatuses { get; set; }

    public bool? ProductionPlanIsActive { get; set; }
    public bool? ProductionPlanIsPublished { get; set; }

    public int? ProductionScheduleSemiStuffId { get; set; }

    public bool? ProductionScheduleIsPublished { get; set; }

    public int? ProductionScheduleWorkStationId { get; set; }
    public int? ProductionScheduleProductionPlanId { get; set; }
    public int? ProductionScheduleProductionStepId { get; set; }
    public int? ProductionPlanDetailId { get; set; }
    public DateTime? ProductionScheduleFromDateTime { get; set; }
    public DateTime? ProductionScheduleToDateTime { get; set; }

    public ProductionScheduleStatus[] ProductionScheduleStatuses { get; set; }
    public ProductionScheduleStatus[] ProductionScheduleNotHasStatuses { get; set; }

    public int? ProductionScheduleProductionLineId { get; set; }
    public bool ProductionRequestItemShow { get; set; }
    public bool ProductionPlanItemShow { get; set; }

    public bool ProductionScheduleItemShow { get; set; }

    public GetPlanningWorkSpaceInput(PagingInput pagingInput, PlanningWorkSpaceSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

  }
}

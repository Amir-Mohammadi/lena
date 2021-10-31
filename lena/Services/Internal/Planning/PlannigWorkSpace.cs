using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.PlanningWorkSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Get
    public IQueryable<PlanningWorkSpaceResult> GetPlanningWorkSpaceItem(
            TValue<bool> RequestShow = null,
            TValue<bool> PlanShow = null,
            TValue<bool> ScheduleShow = null,
            TValue<int> OrderItemCustomerId = null,
            TValue<int> OrderItemStuffId = null,
            TValue<DateTime> OrderItemFromRequestDate = null,
            TValue<DateTime> OrderItemToRequestDate = null,
            TValue<DateTime> OrderItemFromDeliveryDate = null,
            TValue<DateTime> OrderItemToDeliveryDate = null,
            TValue<int> OrderId = null,
            TValue<string> OrderItemCode = null,
            TValue<OrderItemStatus> OrderItemStatus = null,
            TValue<OrderItemStatus[]> OrderItemStatuses = null,
            TValue<OrderItemStatus[]> OrderItemNotHasStatuses = null,
            TValue<bool?> OrderItemHasChange = null,
            TValue<bool> OrderItemIsArchive = null,
            TValue<DateTime> ProductionRequestFromDeadline = null,
            TValue<DateTime> ProductionRequestToDeadline = null,
            TValue<ProductionRequestStatus> ProductionRequestStatus = null,
            TValue<ProductionRequestStatus[]> ProductionRequestStatuses = null,
            TValue<ProductionRequestStatus[]> ProductionRequestNotHasStatuses = null,
            TValue<DateTime> ProductionPlanFromEstimatedDate = null,
            TValue<DateTime> ProductionPlanToEstimatedDate = null,
            TValue<ProductionPlanStatus> ProductionPlanStatus = null,
            TValue<ProductionPlanStatus[]> ProductionPlanStatuses = null,
            TValue<ProductionPlanStatus[]> ProductionPlanNotHasStatuses = null,
            TValue<int> ProductionScheduleSemiStuffId = null,
            TValue<bool> ProductionScheduleIsPublished = null,
            TValue<int> ProductionScheduleProductionPlanId = null,
            TValue<int> ProductionScheduleProductionStepId = null,
            TValue<int> ProductionPlanDetailId = null,
            TValue<DateTime> ProductionScheduleFromDateTime = null,
            TValue<DateTime> ProductionScheduleToDateTime = null,
            TValue<int> ProductionScheduleProductionLineId = null,
            TValue<ProductionScheduleStatus[]> ProductionScheduleStatuses = null,
            TValue<ProductionScheduleStatus[]> ProductionScheduleNotHasStatuses = null
            )
    {



      IQueryable<PlanningWorkSpaceResult> result = null;
      if ((RequestShow && PlanShow && ScheduleShow) || (RequestShow && !PlanShow && ScheduleShow) || (!RequestShow && PlanShow && ScheduleShow) || (!RequestShow && !PlanShow && ScheduleShow))
      {
        var orderItemList = App.Internals.SaleManagement.GetOrderItems
              (
              e => e,
              customerId: OrderItemCustomerId,
              stuffId: OrderItemStuffId,
              FromRequestDate: OrderItemFromRequestDate,
              ToRequestDate: OrderItemToRequestDate,
              FromdeliveryDate: OrderItemFromDeliveryDate,
              TodeliveryDate: OrderItemToDeliveryDate,
              orderId: OrderId,
              code: OrderItemCode,
              status: OrderItemStatus,
              statuses: OrderItemStatuses,
              notHasStatuses: OrderItemNotHasStatuses,
              hasChange: OrderItemHasChange,
              isArchive: OrderItemIsArchive);

        var productionRequest = App.Internals.SaleManagement.GetProductionRequests
              (e => e,
              FromdeadlineDate: ProductionRequestFromDeadline,
              TodeadlineDate: ProductionRequestToDeadline,
              status: ProductionRequestStatus,
              statuses: ProductionRequestStatuses,
              notHasStatuses: ProductionRequestNotHasStatuses
              );

        var productionPlan = App.Internals.Planning.GetProductionPlans
              (e => e,
              FromestimatedDate: ProductionPlanFromEstimatedDate,
              ToestimatedDate: ProductionPlanToEstimatedDate,
              status: ProductionPlanStatus,
              statuses: ProductionPlanStatuses,
              notHasStatuses: ProductionPlanNotHasStatuses
              );

        var productionScheduel = App.Internals.Planning.GetProductionSchedules
              (e => e,
              stuffId: ProductionScheduleSemiStuffId,
              isPublished: ProductionScheduleIsPublished,
              workPlanId: ProductionScheduleProductionPlanId,
              workPlanStepId: ProductionScheduleProductionStepId,
              productionLineId: ProductionScheduleProductionLineId,
              statuses: ProductionScheduleStatuses,
              notHasStatuses: ProductionScheduleNotHasStatuses,
              fromDateTime: ProductionScheduleFromDateTime,
              toDateTime: ProductionScheduleToDateTime
              );

        result = from x in orderItemList
                 from ci in x.OrderItemConfirmations.DefaultIfEmpty()
                 from chko in ci.CheckOrderItems.DefaultIfEmpty()
                 join Tprequest in productionRequest on chko.Id equals Tprequest.CheckOrderItemId into PrequestTable
                 from prequest in PrequestTable.DefaultIfEmpty()
                 join Tpplan in productionPlan on prequest.Id equals Tpplan.ProductionRequestId into PPlanTable
                 from pplan in PPlanTable.DefaultIfEmpty()
                 from pplanDetail in pplan.ProductionPlanDetails.DefaultIfEmpty()
                 from pschedule in pplanDetail.ProductionSchedules.DefaultIfEmpty()
                 join TpSchedule in productionScheduel on pplan.Id equals TpSchedule.ProductionPlanDetailId into PScheduleTable
                 from pSchedule in PScheduleTable.DefaultIfEmpty()
                 select new PlanningWorkSpaceResult
                 {
                   #region Order
                   OrderItemId = x.Id,
                   OrderItemCode = x.Code,
                   OrderItemOrderDescription = x.Order.Description,
                   OrderItemDescription = x.Description,
                   OrderItemStuffCode = x.Stuff.Code,
                   OrderItemStuffName = x.Stuff.Title,
                   OrderItemStuffNoun = x.Stuff.Noun,
                   OrderItemStuffId = x.StuffId,
                   OrderItemDeliveryDate = x.DeliveryDate,
                   OrderItemOrderId = x.OrderId,
                   OrderItemCustomerId = x.Order.CustomerId,
                   OrderItemCustomerName = x.Order.Customer.Name,
                   OrderItemCustomerCode = x.Order.Customer.Code,
                   OrderItemOrderer = x.Order.Orderer,
                   OrderItemBillOfMaterialVersion = x.BillOfMaterialVersion,
                   OrderItemBillOfMaterialTitle = x.BillOfMaterial == null ? "" : x.BillOfMaterial.Title,
                   OrderItemQty = x.Qty,
                   OrderItemUnitId = x.UnitId,
                   OrderItemUnitName = x.Unit.Name,
                   OrderItemOrderTypeId = x.Order.OrderTypeId,
                   OrderItemOrderTypeName = x.Order.OrderType.Name,
                   OrderItemRequestDate = x.RequestDate,
                   OrderItemDateTime = x.DateTime,
                   OrderItemEmployeeFullName = x.User.Employee.FirstName + " " + x.User.Employee.LastName,
                   OrderItemProducedQty = x.OrderItemSummary.ProducedQty,
                   OrderItemPermissionQty = x.OrderItemSummary.PermissionQty,
                   OrderItemBlockedQty = x.OrderItemSummary.BlockedQty,
                   OrderItemSendedQty = x.OrderItemSummary.SendedQty,
                   OrderItemNotPostedQty = x.Qty - x.OrderItemSummary.SendedQty,
                   OrderItemPlannedQty = x.OrderItemSummary.PlannedQty,
                   OrderItemStatus = x.Status,
                   OrderItemDocumentNumber = x.Order.DocumentNumber,
                   OrderItemDocumentType = x.Order.DocumentType,
                   OrderItemHasChange = x.HasChange,
                   OrderItemConfirmationConfirmed = x.OrderItemConfirmationConfirmed,
                   OrderItemConfirmationDateTime = x.OrderItemConfirmationDateTime,
                   OrderItemCheckOrderItemConfirmed = x.CheckOrderItemConfirmed,
                   OrderItemCheckOrderItemDateTime = x.CheckOrderItemDateTime,
                   OrderItemChangeStatus = x.OrderItemChangeStatus,
                   OrderItemIsArchive = x.IsArchive,
                   OrderItemRowVersion = x.RowVersion,
                   OrderItemOrderRowVersion = x.Order.RowVersion,
                   #endregion
                   #region ProductionRequest
                   ProductionRequestId = prequest.Id,
                   ProductionRequestCode = prequest.Code,
                   ProductionRequestQty = prequest.Qty,
                   ProductionRequestUnitName = prequest.Unit.Name,
                   ProductionRequestUnitId = prequest.UnitId,
                   ProductionRequestDate = prequest.DateTime,
                   ProductionRequestDeadlineDate = prequest.DeadlineDate,
                   ProductionRequestStatus = prequest.Status,
                   ProductionRequestPlannedQty = prequest.ProductionRequestSummary.PlannedQty,
                   ProductionRequestScheduledQty = prequest.ProductionRequestSummary.ScheduledQty,
                   ProductionRequestDescription = prequest.Description,
                   ProductionRequestProducedQty = prequest.ProductionRequestSummary.ProducedQty,
                   ProductionRequestRowVersion = prequest.RowVersion,
                   #endregion
                   #region ProductionPlan
                   ProductionPlanId = pplan.Id,
                   ProductionPlanIsActive = pplan.BillOfMaterial.IsActive,
                   ProductionPlanIsPublished = pplan.BillOfMaterial.IsPublished,
                   ProductionPlanQty = pplan.Qty,
                   ProductionPlanUnitId = pplan.UnitId,
                   ProductionPlanUnitName = pplan.Unit.Name,
                   ProductionPlanStatus = pplan.Status,
                   ProductionPlanEstimatedDate = pplan.EstimatedDate,
                   ProductionPlanDescription = pplan.Description,
                   ProductionPlanRowVersion = pplan.RowVersion,
                   #endregion
                   #region ProductionSchedule
                   ProductionScheduleSemiProductStuffName = pschedule.ProductionPlanDetail.BillOfMaterial.Stuff.Name,
                   ProductionScheduleSemiProductStuffCode = pschedule.ProductionPlanDetail.BillOfMaterial.Stuff.Code,
                   ProductionScheduleCode = pschedule.Code,
                   ProductionScheduleId = pschedule.Id,
                   ProductionScheduleStepName = pschedule.WorkPlanStep.ProductionStep.Name,
                   ProductionScheduleLineName = pschedule.WorkPlanStep.ProductionLine.Name,
                   ProductionScheduleIsPublished = pschedule.IsPublished,
                   ProductionScheduleStatus = pschedule.Status,
                   ProductionScheduleQty = pschedule.Qty,
                   ProductionScheduleUnitName = pschedule.ProductionPlanDetail.Unit.Name,
                   ProductionScheduleProducedQty = pschedule.ProductionScheduleSummary.ProducedQty,
                   ProductionScheduleProducedUnitName = pschedule.ProductionPlanDetail.Unit.Name,
                   ProductionScheduleDateTime = pschedule.CalendarEvent.DateTime,
                   ProductionScheduleDuration = pschedule.CalendarEvent.Duration,
                   ProductionScheduleRowVersion = pschedule.RowVersion
                   #endregion

                 };
      }
      if ((RequestShow && PlanShow && !ScheduleShow) || (!RequestShow && PlanShow && !ScheduleShow))
      {
        var orderItemList = App.Internals.SaleManagement.GetOrderItems
               (
               e => e,
               customerId: OrderItemCustomerId,
               stuffId: OrderItemStuffId,
               FromRequestDate: OrderItemFromRequestDate,
               ToRequestDate: OrderItemToRequestDate,
               FromdeliveryDate: OrderItemFromDeliveryDate,
               TodeliveryDate: OrderItemToDeliveryDate,
               orderId: OrderId,
               code: OrderItemCode,
               status: OrderItemStatus,
               statuses: OrderItemStatuses,
               notHasStatuses: OrderItemNotHasStatuses,
               hasChange: OrderItemHasChange,
               isArchive: OrderItemIsArchive
               );
        var productionRequest = App.Internals.SaleManagement.GetProductionRequests
              (e => e,
              FromdeadlineDate: ProductionRequestFromDeadline,
              TodeadlineDate: ProductionRequestToDeadline,
              status: ProductionRequestStatus,
              statuses: ProductionRequestStatuses,
              notHasStatuses: ProductionRequestNotHasStatuses
              );
        var productionPlan = App.Internals.Planning.GetProductionPlans
              (e => e,
              FromestimatedDate: ProductionPlanFromEstimatedDate,
              ToestimatedDate: ProductionPlanToEstimatedDate,
              status: ProductionPlanStatus,
              statuses: ProductionPlanStatuses,
              notHasStatuses: ProductionPlanNotHasStatuses
              );


        result = from x in orderItemList
                 from ci in x.OrderItemConfirmations.DefaultIfEmpty()
                 from chko in ci.CheckOrderItems.DefaultIfEmpty()
                 join Tprequest in productionRequest on chko.Id equals Tprequest.CheckOrderItemId into PrequestTable
                 from prequest in PrequestTable.DefaultIfEmpty()
                 join Tpplan in productionPlan on prequest.Id equals Tpplan.ProductionRequestId into PPlanTable
                 from pplan in PPlanTable.DefaultIfEmpty()
                 select new PlanningWorkSpaceResult
                 {
                   #region Order
                   OrderItemId = x.Id,
                   OrderItemCode = x.Code,
                   OrderItemOrderDescription = x.Order.Description,
                   OrderItemDescription = x.Description,
                   OrderItemStuffCode = x.Stuff.Code,
                   OrderItemStuffName = x.Stuff.Title,
                   OrderItemStuffNoun = x.Stuff.Noun,
                   OrderItemStuffId = x.StuffId,
                   OrderItemDeliveryDate = x.DeliveryDate,
                   OrderItemOrderId = x.OrderId,
                   OrderItemCustomerId = x.Order.CustomerId,
                   OrderItemCustomerName = x.Order.Customer.Name,
                   OrderItemCustomerCode = x.Order.Customer.Code,
                   OrderItemOrderer = x.Order.Orderer,
                   OrderItemBillOfMaterialVersion = x.BillOfMaterialVersion,
                   OrderItemBillOfMaterialTitle = x.BillOfMaterial == null ? "" : x.BillOfMaterial.Title,
                   OrderItemQty = x.Qty,
                   OrderItemUnitId = x.UnitId,
                   OrderItemUnitName = x.Unit.Name,
                   OrderItemOrderTypeId = x.Order.OrderTypeId,
                   OrderItemOrderTypeName = x.Order.OrderType.Name,
                   OrderItemRequestDate = x.RequestDate,
                   OrderItemDateTime = x.DateTime,
                   OrderItemEmployeeFullName = x.User.Employee.FirstName + " " + x.User.Employee.LastName,
                   OrderItemProducedQty = x.OrderItemSummary.ProducedQty,
                   OrderItemPermissionQty = x.OrderItemSummary.PermissionQty,
                   OrderItemBlockedQty = x.OrderItemSummary.BlockedQty,
                   OrderItemSendedQty = x.OrderItemSummary.SendedQty,
                   OrderItemNotPostedQty = x.Qty - x.OrderItemSummary.SendedQty,
                   OrderItemPlannedQty = x.OrderItemSummary.PlannedQty,
                   OrderItemStatus = x.Status,
                   OrderItemDocumentNumber = x.Order.DocumentNumber,
                   OrderItemDocumentType = x.Order.DocumentType,
                   OrderItemHasChange = x.HasChange,
                   OrderItemConfirmationConfirmed = x.OrderItemConfirmationConfirmed,
                   OrderItemConfirmationDateTime = x.OrderItemConfirmationDateTime,
                   OrderItemCheckOrderItemConfirmed = x.CheckOrderItemConfirmed,
                   OrderItemCheckOrderItemDateTime = x.CheckOrderItemDateTime,
                   OrderItemChangeStatus = x.OrderItemChangeStatus,
                   OrderItemIsArchive = x.IsArchive,
                   OrderItemRowVersion = x.RowVersion,
                   OrderItemOrderRowVersion = x.Order.RowVersion,
                   #endregion
                   #region ProductionRequest
                   ProductionRequestId = prequest.Id,
                   ProductionRequestCode = prequest.Code,
                   ProductionRequestQty = prequest.Qty,
                   ProductionRequestUnitName = prequest.Unit.Name,
                   ProductionRequestUnitId = prequest.UnitId,
                   ProductionRequestDate = prequest.DateTime,
                   ProductionRequestDeadlineDate = prequest.DeadlineDate,
                   ProductionRequestStatus = prequest.Status,
                   ProductionRequestPlannedQty = prequest.ProductionRequestSummary.PlannedQty,
                   ProductionRequestScheduledQty = prequest.ProductionRequestSummary.ScheduledQty,
                   ProductionRequestDescription = prequest.Description,
                   ProductionRequestProducedQty = prequest.ProductionRequestSummary.ProducedQty,
                   ProductionRequestRowVersion = prequest.RowVersion,

                   #endregion
                   #region ProductionPlan
                   ProductionPlanId = pplan.Id,
                   ProductionPlanIsActive = pplan.BillOfMaterial.IsActive,
                   ProductionPlanIsPublished = pplan.BillOfMaterial.IsPublished,
                   ProductionPlanQty = pplan.Qty,
                   ProductionPlanUnitId = pplan.UnitId,
                   ProductionPlanUnitName = pplan.Unit.Name,
                   ProductionPlanStatus = pplan.Status,
                   ProductionPlanEstimatedDate = pplan.EstimatedDate,
                   ProductionPlanDescription = pplan.Description,
                   ProductionPlanRowVersion = pplan.RowVersion,
                   #endregion
                 };
      }

      if (RequestShow && !PlanShow && !ScheduleShow)
      {
        var orderItemList = App.Internals.SaleManagement.GetOrderItems
              (
              e => e,
              customerId: OrderItemCustomerId,
              stuffId: OrderItemStuffId,
              FromRequestDate: OrderItemFromRequestDate,
              ToRequestDate: OrderItemToRequestDate,
              FromdeliveryDate: OrderItemFromDeliveryDate,
              TodeliveryDate: OrderItemToDeliveryDate,
              orderId: OrderId,
              code: OrderItemCode,
              status: OrderItemStatus,
              statuses: OrderItemStatuses,
              notHasStatuses: OrderItemNotHasStatuses,
              hasChange: OrderItemHasChange,
              isArchive: OrderItemIsArchive
              );
        var productionRequest = App.Internals.SaleManagement.GetProductionRequests
               (e => e,
               FromdeadlineDate: ProductionRequestFromDeadline,
               TodeadlineDate: ProductionRequestToDeadline,
               status: ProductionRequestStatus,
               statuses: ProductionRequestStatuses,
               notHasStatuses: ProductionRequestNotHasStatuses
               );
        result = from x in orderItemList
                 from ci in x.OrderItemConfirmations.DefaultIfEmpty()
                 from chko in ci.CheckOrderItems.DefaultIfEmpty()
                 join Tprequest in productionRequest on chko.Id equals Tprequest.CheckOrderItemId into PrequestTable
                 from prequest in PrequestTable.DefaultIfEmpty()
                 select new PlanningWorkSpaceResult
                 {
                   #region Order
                   OrderItemId = x.Id,
                   OrderItemCode = x.Code,
                   OrderItemOrderDescription = x.Order.Description,
                   OrderItemDescription = x.Description,
                   OrderItemStuffCode = x.Stuff.Code,
                   OrderItemStuffName = x.Stuff.Title,
                   OrderItemStuffNoun = x.Stuff.Noun,
                   OrderItemStuffId = x.StuffId,
                   OrderItemDeliveryDate = x.DeliveryDate,
                   OrderItemOrderId = x.OrderId,
                   OrderItemCustomerId = x.Order.CustomerId,
                   OrderItemCustomerName = x.Order.Customer.Name,
                   OrderItemCustomerCode = x.Order.Customer.Code,
                   OrderItemOrderer = x.Order.Orderer,
                   OrderItemBillOfMaterialVersion = x.BillOfMaterialVersion,
                   OrderItemBillOfMaterialTitle = x.BillOfMaterial == null ? "" : x.BillOfMaterial.Title,
                   OrderItemQty = x.Qty,
                   OrderItemUnitId = x.UnitId,
                   OrderItemUnitName = x.Unit.Name,
                   OrderItemOrderTypeId = x.Order.OrderTypeId,
                   OrderItemOrderTypeName = x.Order.OrderType.Name,
                   OrderItemRequestDate = x.RequestDate,
                   OrderItemDateTime = x.DateTime,
                   OrderItemEmployeeFullName = x.User.Employee.FirstName + " " + x.User.Employee.LastName,
                   OrderItemProducedQty = x.OrderItemSummary.ProducedQty,
                   OrderItemPermissionQty = x.OrderItemSummary.PermissionQty,
                   OrderItemBlockedQty = x.OrderItemSummary.BlockedQty,
                   OrderItemSendedQty = x.OrderItemSummary.SendedQty,
                   OrderItemNotPostedQty = x.Qty - x.OrderItemSummary.SendedQty,
                   OrderItemPlannedQty = x.OrderItemSummary.PlannedQty,
                   OrderItemStatus = x.Status,
                   OrderItemDocumentNumber = x.Order.DocumentNumber,
                   OrderItemDocumentType = x.Order.DocumentType,
                   OrderItemHasChange = x.HasChange,
                   OrderItemConfirmationConfirmed = x.OrderItemConfirmationConfirmed,
                   OrderItemConfirmationDateTime = x.OrderItemConfirmationDateTime,
                   OrderItemCheckOrderItemConfirmed = x.CheckOrderItemConfirmed,
                   OrderItemCheckOrderItemDateTime = x.CheckOrderItemDateTime,
                   OrderItemChangeStatus = x.OrderItemChangeStatus,
                   OrderItemIsArchive = x.IsArchive,
                   OrderItemRowVersion = x.RowVersion,
                   OrderItemOrderRowVersion = x.Order.RowVersion,
                   #endregion

                   #region ProductionRequest
                   ProductionRequestId = prequest.Id,
                   ProductionRequestCode = prequest.Code,
                   ProductionRequestQty = prequest.Qty,
                   ProductionRequestUnitName = prequest.Unit.Name,
                   ProductionRequestUnitId = prequest.UnitId,
                   ProductionRequestDate = prequest.DateTime,
                   ProductionRequestDeadlineDate = prequest.DeadlineDate,
                   ProductionRequestStatus = prequest.Status,
                   ProductionRequestPlannedQty = prequest.ProductionRequestSummary.PlannedQty,
                   ProductionRequestScheduledQty = prequest.ProductionRequestSummary.ScheduledQty,
                   ProductionRequestDescription = prequest.Description,
                   ProductionRequestProducedQty = prequest.ProductionRequestSummary.ProducedQty,
                   ProductionRequestRowVersion = prequest.RowVersion,

                   #endregion
                 };


      }


      if (!RequestShow && !PlanShow && !ScheduleShow)
      {
        var orderItemList = App.Internals.SaleManagement.GetOrderItems
              (
              e => e,
              customerId: OrderItemCustomerId,
              stuffId: OrderItemStuffId,
              FromRequestDate: OrderItemFromRequestDate,
              ToRequestDate: OrderItemToRequestDate,
              FromdeliveryDate: OrderItemFromDeliveryDate,
              TodeliveryDate: OrderItemToDeliveryDate,
              orderId: OrderId,
              code: OrderItemCode,
              status: OrderItemStatus,
              statuses: OrderItemStatuses,
              notHasStatuses: OrderItemNotHasStatuses,
              hasChange: OrderItemHasChange,
              isArchive: OrderItemIsArchive
              );
        result = from x in orderItemList
                 select new PlanningWorkSpaceResult
                 {
                   #region Order
                   OrderItemId = x.Id,
                   OrderItemCode = x.Code,
                   OrderItemOrderDescription = x.Order.Description,
                   OrderItemDescription = x.Description,
                   OrderItemStuffCode = x.Stuff.Code,
                   OrderItemStuffName = x.Stuff.Title,
                   OrderItemStuffNoun = x.Stuff.Noun,
                   OrderItemStuffId = x.StuffId,
                   OrderItemDeliveryDate = x.DeliveryDate,
                   OrderItemOrderId = x.OrderId,
                   OrderItemCustomerId = x.Order.CustomerId,
                   OrderItemCustomerName = x.Order.Customer.Name,
                   OrderItemCustomerCode = x.Order.Customer.Code,
                   OrderItemOrderer = x.Order.Orderer,
                   OrderItemBillOfMaterialVersion = x.BillOfMaterialVersion,
                   OrderItemBillOfMaterialTitle = x.BillOfMaterial == null ? "" : x.BillOfMaterial.Title,
                   OrderItemQty = x.Qty,
                   OrderItemUnitId = x.UnitId,
                   OrderItemUnitName = x.Unit.Name,
                   OrderItemOrderTypeId = x.Order.OrderTypeId,
                   OrderItemOrderTypeName = x.Order.OrderType.Name,
                   OrderItemRequestDate = x.RequestDate,
                   OrderItemDateTime = x.DateTime,
                   OrderItemEmployeeFullName = x.User.Employee.FirstName + " " + x.User.Employee.LastName,
                   OrderItemProducedQty = x.OrderItemSummary.ProducedQty,
                   OrderItemPermissionQty = x.OrderItemSummary.PermissionQty,
                   OrderItemBlockedQty = x.OrderItemSummary.BlockedQty,
                   OrderItemSendedQty = x.OrderItemSummary.SendedQty,
                   OrderItemNotPostedQty = x.Qty - x.OrderItemSummary.SendedQty,
                   OrderItemPlannedQty = x.OrderItemSummary.PlannedQty,
                   OrderItemStatus = x.Status,
                   OrderItemDocumentNumber = x.Order.DocumentNumber,
                   OrderItemDocumentType = x.Order.DocumentType,
                   OrderItemHasChange = x.HasChange,
                   OrderItemConfirmationConfirmed = x.OrderItemConfirmationConfirmed,
                   OrderItemConfirmationDateTime = x.OrderItemConfirmationDateTime,
                   OrderItemCheckOrderItemConfirmed = x.CheckOrderItemConfirmed,
                   OrderItemCheckOrderItemDateTime = x.CheckOrderItemDateTime,
                   OrderItemChangeStatus = x.OrderItemChangeStatus,
                   OrderItemIsArchive = x.IsArchive,
                   OrderItemRowVersion = x.RowVersion,
                   OrderItemOrderRowVersion = x.Order.RowVersion,
                   #endregion
                 };
      }

      return result;


    }
    #endregion

    #region search
    public IQueryable<PlanningWorkSpaceResult> SearchPlanningWorkSpaceResult(
      IQueryable<PlanningWorkSpaceResult> query,
      string searchText,
      AdvanceSearchItem[] advanceSearchItems,
      bool? isActive,
      bool? isPublished
      )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(i =>
                             i.OrderItemCode.Contains(searchText) ||
                             i.OrderItemStuffCode.Contains(searchText) ||
                             i.OrderItemStuffName.ToString().Contains(searchText) ||
                             i.OrderItemStuffNoun.Contains(searchText) ||
                             i.OrderItemUnitName.Contains(searchText) ||
                             i.OrderItemBillOfMaterialTitle.Contains(searchText) ||
                             i.OrderItemCustomerName.Contains(searchText) ||
                             i.OrderItemCustomerCode.Contains(searchText) ||
                             i.OrderItemOrderer.ToString().Contains(searchText) ||
                             i.OrderItemOrderTypeName.Contains(searchText) ||
                             i.OrderItemDocumentNumber.Contains(searchText) ||
                             i.OrderItemEmployeeFullName.Contains(searchText) ||
                             i.OrderItemDescription.Contains(searchText) ||
                             i.OrderItemOrderDescription.Contains(searchText) ||
                             i.ProductionRequestCode.ToString().Contains(searchText) ||
                             i.ProductionRequestUnitName.Contains(searchText) ||
                             i.ProductionPlanUnitName.Contains(searchText) ||
                             i.ProductionPlanDescription.Contains(searchText) ||
                             i.ProductionScheduleSemiProductStuffName.Contains(searchText) ||
                             i.ProductionScheduleSemiProductStuffCode.Contains(searchText) ||
                             i.ProductionScheduleCode.Contains(searchText) ||
                             i.ProductionScheduleStepName.Contains(searchText) ||
                             i.ProductionScheduleLineName.Contains(searchText) ||
                             i.ProductionScheduleUnitName.Contains(searchText) ||
                             i.ProductionScheduleProducedUnitName.Contains(searchText));
      }
      if (isActive != null)
      {
        query = query.Where(x => x.ProductionPlanIsActive == isActive);
      }
      if (isPublished != null)
      {
        query = query.Where(x => x.ProductionPlanIsPublished == isPublished);
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IQueryable<PlanningWorkSpaceResult> SortPlanningWorkSpaceResult(IQueryable<PlanningWorkSpaceResult> query, SortInput<PlanningWorkSpaceSortType> sort)
    {
      //have some miss type
      switch (sort.SortType)
      {
        case PlanningWorkSpaceSortType.OrderItemOrderId:
          query = query.OrderBy(x => x.OrderItemOrderId, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemCode:
          query = query.OrderBy(x => x.OrderItemCode, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemCustomerName:
          query = query.OrderBy(x => x.OrderItemCustomerName, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemCustomerCode:
          query = query.OrderBy(x => x.OrderItemCustomerCode, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemStuffCode:
          query = query.OrderBy(x => x.OrderItemStuffCode, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemStuffName:
          query = query.OrderBy(x => x.OrderItemStuffName, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemStuffNoun:
          query = query.OrderBy(x => x.OrderItemStuffNoun, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemQty:
          query = query.OrderBy(x => x.OrderItemQty, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemUnitName:
          query = query.OrderBy(x => x.OrderItemUnitName, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemStatus:
          query = query.OrderBy(x => x.OrderItemStatus, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemPlannedQty:
          query = query.OrderBy(x => x.OrderItemPlannedQty, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemBlockedQty:
          query = query.OrderBy(x => x.OrderItemBlockedQty, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemProducedQty:
          query = query.OrderBy(x => x.OrderItemProducedQty, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemPermissionQty:
          query = query.OrderBy(x => x.OrderItemPermissionQty, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemSendedQty:
          query = query.OrderBy(x => x.OrderItemSendedQty, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemNotPostedQty:
          query = query.OrderBy(x => x.OrderItemNotPostedQty, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemRequestDate:
          query = query.OrderBy(x => x.OrderItemRequestDate, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemDeliveryDate:
          query = query.OrderBy(x => x.OrderItemDeliveryDate, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemDateTime:
          query = query.OrderBy(x => x.OrderItemDateTime, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemEmployeeFullName:
          query = query.OrderBy(x => x.OrderItemEmployeeFullName, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemBillOfMaterialVersion:
          query = query.OrderBy(x => x.OrderItemBillOfMaterialVersion, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemOrderer:
          query = query.OrderBy(x => x.OrderItemOrderer, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemOrderTypeName:
          query = query.OrderBy(x => x.OrderItemOrderTypeName, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemHasChange:
          query = query.OrderBy(x => x.OrderItemHasChange, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemIsArchive:
          query = query.OrderBy(x => x.OrderItemIsArchive, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemConfirmationConfirmed:
          query = query.OrderBy(x => x.OrderItemConfirmationConfirmed, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemConfirmationDateTime:
          query = query.OrderBy(x => x.OrderItemConfirmationDateTime, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemCheckOrderItemConfirmed:
          query = query.OrderBy(x => x.OrderItemCheckOrderItemConfirmed, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemCheckOrderItemDateTime:
          query = query.OrderBy(x => x.OrderItemCheckOrderItemDateTime, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemChangeStatus:
          query = query.OrderBy(x => x.OrderItemChangeStatus, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemDocumentNumber:
          query = query.OrderBy(x => x.OrderItemDocumentNumber, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.OrderItemDocumentType:
          query = query.OrderBy(x => x.OrderItemDocumentType, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.OrderItemDescription:
          query = query.OrderBy(x => x.OrderItemDescription, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.OrderItemOrderDescription:
          query = query.OrderBy(x => x.OrderItemOrderDescription, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.ProductionRequestCode:
          query = query.OrderBy(x => x.ProductionRequestCode, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.ProductionRequestQty:
          query = query.OrderBy(x => x.ProductionRequestQty, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.ProductionRequestUnitName:
          query = query.OrderBy(x => x.ProductionRequestUnitName, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.ProductionRequestDeadlineDate:
          query = query.OrderBy(x => x.ProductionRequestDeadlineDate, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.ProductionRequestDate:
          query = query.OrderBy(x => x.ProductionRequestDate, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.ProductionRequestStatus:
          query = query.OrderBy(x => x.ProductionRequestStatus, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.ProductionRequestPlannedQty:
          query = query.OrderBy(x => x.ProductionRequestPlannedQty, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.ProductionRequestScheduledQty:
          query = query.OrderBy(x => x.ProductionRequestScheduledQty, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.ProductionRequestProducedQty:
          query = query.OrderBy(x => x.ProductionRequestProducedQty, sort.SortOrder);
          break;
        case PlanningWorkSpaceSortType.ProductionRequestDescription:
          query = query.OrderBy(x => x.ProductionRequestDescription, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionPlanIsActive:
          query = query.OrderBy(x => x.ProductionPlanIsActive, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionPlanIsPublished:
          query = query.OrderBy(x => x.ProductionPlanIsPublished, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionPlanQty:
          query = query.OrderBy(x => x.ProductionPlanQty, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionPlanUnitName:
          query = query.OrderBy(x => x.ProductionPlanUnitName, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionPlanStatus:
          query = query.OrderBy(x => x.ProductionPlanStatus, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionPlanEstimatedDate:
          query = query.OrderBy(x => x.ProductionPlanEstimatedDate, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionPlanDescription:
          query = query.OrderBy(x => x.ProductionPlanDescription, sort.SortOrder);
          break;


        case PlanningWorkSpaceSortType.ProductionScheduleSemiProductStuffCode:
          query = query.OrderBy(x => x.ProductionScheduleSemiProductStuffCode, sort.SortOrder);
          break;



        case PlanningWorkSpaceSortType.ProductionScheduleSemiProductStuffName:
          query = query.OrderBy(x => x.ProductionScheduleSemiProductStuffName, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionScheduleCode:
          query = query.OrderBy(x => x.ProductionScheduleCode, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionScheduleStepName:
          query = query.OrderBy(x => x.ProductionScheduleStepName, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionScheduleLineName:
          query = query.OrderBy(x => x.ProductionScheduleLineName, sort.SortOrder);
          break;


        case PlanningWorkSpaceSortType.ProductionScheduleIsPublished:
          query = query.OrderBy(x => x.ProductionScheduleIsPublished, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionScheduleStatus:
          query = query.OrderBy(x => x.ProductionScheduleStatus, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionScheduleQty:
          query = query.OrderBy(x => x.ProductionScheduleQty, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionScheduleUnitName:
          query = query.OrderBy(x => x.ProductionScheduleUnitName, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionScheduleProducedQty:
          query = query.OrderBy(x => x.ProductionScheduleProducedQty, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionScheduleProducedUnitName:
          query = query.OrderBy(x => x.ProductionScheduleProducedUnitName, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionScheduleDateTime:
          query = query.OrderBy(x => x.ProductionScheduleDateTime, sort.SortOrder);
          break;

        case PlanningWorkSpaceSortType.ProductionScheduleToDateTime:
          query = query.OrderBy(x => x.ProductionScheduleToDateTime, sort.SortOrder);
          break;
        default:
          query = query.OrderBy(x => x.OrderItemId, sort.SortOrder);
          break;
      }
      return query;
    }
    #endregion
  }
}

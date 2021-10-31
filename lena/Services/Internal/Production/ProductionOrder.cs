using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using Microsoft.EntityFrameworkCore;
using lena.Models.Common;
using lena.Models.Production.ProductionOperationSequence;
using lena.Models.Production.ProductionOperator;
using lena.Models.Production.ProductionOrder;
using lena.Models.WarehouseManagement.BaseTransaction;
//using System.Data.Entity;
using lena.Services.Internals.Production.Exception;
//using System.Data.Entity.SqlServer;
using lena.Models.Production.ProductionOrderReport;
using lena.Models.Planning.EmployeeOperatorType;
using lena.Models.StaticData;
////using Microsoft.Ajax.Utilities;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Add
    public ProductionOrder AddProductionOrder(
        TransactionBatch transactionBatch,
        string description,
        CalendarEvent productionCalendarEvent,
        double qty,
        byte unitId,
        double toleranceQty,
        int workPlanStepId,
        int? productionScheduleId,
        int supervisorEmployeeId)
    {
      var productionOrder = repository.Create<ProductionOrder>();
      productionOrder.Qty = qty;
      productionOrder.UnitId = unitId;
      productionOrder.WorkPlanStepId = workPlanStepId;
      productionOrder.ProductionScheduleId = productionScheduleId;
      productionOrder.CalendarEvent = productionCalendarEvent;
      productionOrder.Status = ProductionOrderStatus.NotAction;
      productionOrder.ToleranceQty = toleranceQty;
      productionOrder.SupervisorEmployeeId = supervisorEmployeeId;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: productionOrder,
                    transactionBatch: transactionBatch,
                    description: description);
      return productionOrder;
    }
    #endregion
    #region AddProcess
    public ProductionOrder AddProductionOrderProcess(
        string description,
        double qty,
        byte unitId,
        double toleranceQty,
        DateTime dateTime,
        long duration,
        int? productionScheduleId,
        int workPlanStepId,
        AddProductionOperatorInput[] addProductionOperators,
        int supervisorEmployeeId)
    {
      #region defineTolerancePermission
      var defineTolerancePermission = App.Internals.UserManagement.CheckPermission(
          actionName: StaticActionName.DefineProductionTolerance,
          actionParameters: null);
      if (defineTolerancePermission.AccessType == AccessType.Denied)
      {
        toleranceQty = 0;
      }
      #endregion
      #region check active BOM
      var workPlanStep = App.Internals.Planning.GetWorkPlanStep(id: workPlanStepId);
      if (workPlanStep.WorkPlan.BillOfMaterial.IsActive == false)
        throw new BillOfMaterialInActiveException();
      #endregion
      #region TransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region ProductionCalendarEvent
      var productionCalendarEvent =
          AddProductionCalendarEvent(
                  dateTime: dateTime,
                  duration: duration,
                  productionOrder: null);
      #endregion
      #region AddProductionOrder
      var productionOrder = AddProductionOrder(
              transactionBatch: transactionBatch,
              description: description,
              toleranceQty: toleranceQty,
              productionCalendarEvent: productionCalendarEvent,
              qty: qty,
              unitId: unitId,
              workPlanStepId: workPlanStepId,
              productionScheduleId: productionScheduleId,
              supervisorEmployeeId: supervisorEmployeeId);
      #endregion
      #region AddProductionOrderSummary
      AddProductionOrderSummary(
              producedQty: 0,
              inProductionQty: 0,
              productionOrderId: productionOrder.Id);
      #endregion
      #region Change ProductionSchedule Status
      if (productionScheduleId != null)
      {
        var productionSchedule = App.Internals.Planning.ResetProductionScheduleStatus(id: productionScheduleId.Value);
      }
      #endregion
      #region Add ProductionOperators
      foreach (var addProductionOperator in addProductionOperators)
      {
        App.Internals.Production.AddProductionOperatorProcess(
                      operationSequenceId: addProductionOperator.OperationSequenceId,
                      machineTypeOperatorTypeId: addProductionOperator.MachineTypeOperatorTypeId,
                      operationId: addProductionOperator.OperationId,
                      defaultTime: addProductionOperator.DefaultTime,
                      operatorTypeId: addProductionOperator.OperatorTypeId,
                      productionOrderId: productionOrder.Id,
                      wrongLimitCount: addProductionOperator.WrongLimitCount,
                      //productionTerminalId: addProductionOperator.ProductionTerminalId,
                      addProductionOperatorEmployees: addProductionOperator.AddProductionOperatorMachineEmployees);
      }
      #endregion
      #region Check ProjectWorkItem 
      if (productionScheduleId != null)
        CheckProductionOrderTask(productionScheduleId: productionScheduleId.Value);
      #endregion
      #region AddProductionMaterialRequestTask
      AddProductionMaterialRequestTask(
              productionOrder: productionOrder);
      #endregion
      return productionOrder;
    }
    #endregion
    #region CheckTask
    public void CheckProductionOrderTask(ProductionSchedule productionSchedule)
    {
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: productionSchedule.Id,
              scrumTaskType: ScrumTaskTypes.ProductionOrder);
      if (projectWorkItem == null)
      {
        projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                      baseEntityId: productionSchedule.Id,
                      scrumTaskType: ScrumTaskTypes.ProductionOrder);
      }
      #endregion
      #region check ProductionSchedules and DoneProjectWorkItem
      var productionOrders = GetProductionOrders(
              selector: e => e,
              productionScheduleId: productionSchedule.Id,
              isDelete: false);
      var totalOrderedQty = productionOrders.Any()
                ? productionOrders.Sum(i => i.Qty * i.Unit.ConversionRatio)
                : 0;
      var scheduledQty = productionSchedule.Qty *
                               productionSchedule.ProductionPlanDetail.Unit.ConversionRatio;
      if (totalOrderedQty >= scheduledQty && projectWorkItem != null && projectWorkItem.ScrumTaskStep != ScrumTaskStep.Done)
      {
        #region DoneTask
        App.Internals.ScrumManagement.DoneScrumTask(
                scrumTask: projectWorkItem,
                rowVersion: projectWorkItem.RowVersion);
        #endregion
      }
      else if (totalOrderedQty < scheduledQty && projectWorkItem != null && projectWorkItem.ScrumTaskStep == ScrumTaskStep.Done)
      {
        #region  AddNewTask
        App.Internals.ProjectManagement.AddProjectWorkItem(
                projectWorkItem: null,
                name: $"ثبت دستور تولید زمانبندی کد {productionSchedule.Code}",
                description: "برای زمانبندی مورد نظر دستور تولید صادر نمایید ",
                color: "",
                departmentId: (int)Departments.Production,
                estimatedTime: 10800,
                isCommit: false,
                scrumTaskTypeId: (int)ScrumTaskTypes.ProductionOrder,
                userId: null,
                spentTime: 0,
                remainedTime: 0,
                scrumTaskStep: ScrumTaskStep.ToDo,
                projectWorkId: projectWorkItem.ScrumBackLogId,
                baseEntityId: productionSchedule.Id);
        #endregion
      }
      #endregion
    }
    public void CheckProductionOrderTask(int productionScheduleId)
    {
      var productionSchedule = App.Internals.Planning.GetProductionSchedule(id: productionScheduleId); ; CheckProductionOrderTask(productionSchedule: productionSchedule);
    }
    #endregion
    #region Edit
    public ProductionOrder EditProductionOrder(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<double> toleranceQty = null,
        TValue<int> workPlanStepId = null,
        TValue<int> billOfMaterialVersion = null,
        TValue<CalendarEvent> productionCalendarEvent = null,
        TValue<int?> productionScheduleId = null,
        TValue<bool> isDelete = null,
        TValue<int> supervisorEmployeeId = null,
        TValue<ProductionOrderStatus> status = null
        )
    {
      var productionOrder = GetProductionOrder(id: id);
      EditProductionOrder(
                    productionOrder: productionOrder,
                    rowVersion: rowVersion,
                    description: description,
                    qty: qty,
                    toleranceQty: toleranceQty,
                    unitId: unitId,
                    workPlanStepId: workPlanStepId,
                    productionCalendarEvent: productionCalendarEvent,
                    productionScheduleId: productionScheduleId,
                    isDelete: isDelete,
                    supervisorEmployeeId: supervisorEmployeeId,
                    status: status);
      return productionOrder;
    }
    public ProductionOrder EditProductionOrder(
        ProductionOrder productionOrder,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<double> toleranceQty = null,
        TValue<int> workPlanStepId = null,
        TValue<int> supervisorEmployeeId = null,
        TValue<CalendarEvent> productionCalendarEvent = null,
        TValue<int?> productionScheduleId = null,
        TValue<bool> isDelete = null,
        TValue<ProductionOrderStatus> status = null)
    {
      if (description != null)
        productionOrder.Description = description;
      if (qty != null)
        productionOrder.Qty = qty;
      if (unitId != null)
        productionOrder.UnitId = unitId;
      if (workPlanStepId != null)
        productionOrder.WorkPlanStepId = workPlanStepId;
      if (productionCalendarEvent != null)
        productionOrder.CalendarEvent = productionCalendarEvent;
      if (toleranceQty != null)
        productionOrder.ToleranceQty = toleranceQty;
      if (status != null)
        productionOrder.Status = status;
      if (supervisorEmployeeId != null)
        productionOrder.SupervisorEmployeeId = supervisorEmployeeId;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: productionOrder,
                    isDelete: isDelete,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as ProductionOrder;
    }
    #endregion
    #region Edit Process
    public ProductionOrder EditProductionOrderProcess(
        int id,
        byte[] rowVersion,
        string description,
        double qty,
        byte unitId,
        double toleranceQty,
        DateTime calenarDate,
        long duration,
        int? productionScheduleId,
        int workPlanStepId,
        int billOfMaterialVersion,
        AddProductionOperatorInput[] addProductionOperators,
        EditProductionOperatorInput[] editProductionOperators,
        int[] deleteProductionOperatorIds,
        int supervisorEmployeeId)
    {
      #region defineTolerancePermission
      var defineTolerancePermission = App.Internals.UserManagement.CheckPermission(
          actionName: StaticActionName.DefineProductionTolerance,
          actionParameters: null);
      if (defineTolerancePermission.AccessType == AccessType.Denied)
      {
        toleranceQty = 0;
      }
      #endregion
      #region TransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region GetProductionOrder
      var productionOrder = GetProductionOrder(id: id);
      var oldProductionScheduleId = productionOrder.ProductionScheduleId;
      #endregion
      #region EditProductionOrder
      EditProductionOrder(
          id: id,
          rowVersion: rowVersion,
          description: description,
          qty: qty,
          toleranceQty: toleranceQty,
          unitId: unitId,
          workPlanStepId: workPlanStepId,
          billOfMaterialVersion: billOfMaterialVersion,
          productionScheduleId: productionScheduleId ?? new TValue<int?>(),
          supervisorEmployeeId: supervisorEmployeeId);
      #endregion
      #region EditProductionCalendarEvent
      EditProductionCalendarEvent(
              productionCalendarEvent: productionOrder.CalendarEvent,
              rowVersion: productionOrder.CalendarEvent.RowVersion,
              dateTime: calenarDate,
              duration: duration);
      #endregion
      #region Check ProjectWorkItem
      if (productionOrder.ProductionScheduleId != null)
        CheckProductionOrderTask(productionScheduleId: productionOrder.ProductionScheduleId.Value);
      if (oldProductionScheduleId != null && productionOrder.ProductionScheduleId != oldProductionScheduleId)
        CheckProductionOrderTask(productionScheduleId: oldProductionScheduleId.Value);
      #endregion
      #region AddProductionMaterialRequestTask
      AddProductionMaterialRequestTask(
              productionOrder: productionOrder);
      #endregion
      #region Add ProductionOperators
      foreach (var addProductionOperator in addProductionOperators)
      {
        App.Internals.Production.AddProductionOperatorProcess(
                      operationSequenceId: addProductionOperator.OperationSequenceId,
                      machineTypeOperatorTypeId: addProductionOperator.MachineTypeOperatorTypeId,
                      operationId: addProductionOperator.OperationId,
                      defaultTime: addProductionOperator.DefaultTime,
                      operatorTypeId: addProductionOperator.OperatorTypeId,
                      productionOrderId: productionOrder.Id,
                      wrongLimitCount: addProductionOperator.WrongLimitCount,
                      //productionTerminalId: addProductionOperator.ProductionTerminalId,
                      addProductionOperatorEmployees: addProductionOperator.AddProductionOperatorMachineEmployees);
      }
      #endregion
      #region Delete ProductionOperators
      foreach (var deleteProductionOperatorId in deleteProductionOperatorIds)
      {
        App.Internals.Production.DeleteProductionOperator(
                      id: deleteProductionOperatorId);
      }
      #endregion
      #region Edit ProductionOperators
      foreach (var editProductionOperator in editProductionOperators)
      {
        App.Internals.Production.EditProductionOperatorProcess(
                      id: editProductionOperator.Id,
                      rowVersion: editProductionOperator.RowVersion,
                      operationSequenceId: editProductionOperator.OperationSequenceId,
                      operationId: editProductionOperator.OperationId,
                      defaultTime: editProductionOperator.DefaultTime,
                      machineTypeOperatorTypeId: editProductionOperator.MachineTypeOperatorTypeId,
                      operatorTypeId: editProductionOperator.OperatorTypeId,
                      productionOrderId: productionOrder.Id,
                      wrongLimitCount: editProductionOperator.WrongLimitCount,
                      addProductionOperatorMachineEmployees: editProductionOperator.AddProductionOperatorMachineEmployees,
                      editProductionOperatorMachineEmployees: editProductionOperator.EditProductionOperatorMachineEmployees,
                      deleteProductionOperatorMachineEmployees: editProductionOperator.DeleteProductionOperatorMachineEmployees);
      }
      #endregion
      return productionOrder;
    }
    public ProductionOrderSummary EditProductionPerformanceDescription(
        int id,
        byte[] rowVersion,
        string description)
    {
      var productionOrder = GetProductionOrder(selector: e => e, id: id);
      return EditProductionOrderSummary(
                    productionOrderSummary: productionOrder.ProductionOrderSummary,
                    description: description,
                    rowVersion: productionOrder.ProductionOrderSummary.RowVersion);
    }
    #endregion
    #region AddProductionMaterialRequestTask
    public void AddProductionMaterialRequestTask(ProductionOrder productionOrder)
    {
      #region GetOrAddProjectWork
      int? projectWorkId = null;
      if (productionOrder.ProductionScheduleId != null)
      {
        #region Get ProjectWorkItem
        var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                baseEntityId: productionOrder.ProductionScheduleId.Value,
                scrumTaskType: ScrumTaskTypes.ProductionOrder);
        if (projectWorkItem == null)
        {
          projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                        baseEntityId: productionOrder.ProductionScheduleId.Value,
                        scrumTaskType: ScrumTaskTypes.ProductionOrder);
        }
        if (projectWorkItem != null)
        {
          projectWorkId = projectWorkItem.ScrumBackLogId;
        }
        #endregion
      }
      else
      {
        #region GetOrAddCommonProjectGroup
        var projectGroup = App.Internals.ScrumManagement.GetOrAddCommonScrumProjectGroup(
                departmentId: (int)Departments.Production);
        #endregion
        #region GetOrAddCommonProject
        var project = App.Internals.ProjectManagement.GetOrAddCommonProject(
                departmentId: (int)Departments.Production);
        #endregion
        #region GetOrAddCommonProjectStep
        var projectStep = App.Internals.ProjectManagement.GetOrAddCommonProjectStep(
                departmentId: (int)Departments.Production);
        #endregion
        #region Add ProjectWork
        var projectWork = App.Internals.ProjectManagement.AddProjectWork(
                projectWork: null,
                name: $"پروسه تولید {productionOrder.Code}",
                description: "",
                color: "",
                departmentId: (int)Departments.Supplies,
                estimatedTime: 18000,
                isCommit: false,
                projectStepId: projectStep.Id,
                baseEntityId: productionOrder.Id
            );
        #endregion
        if (projectWork != null)
          projectWorkId = projectWork.Id;
      }
      #endregion
      #region Get DescriptionForTask
      var stuffId = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterialStuffId;
      var stuff = App.Internals.SaleManagement.GetStuffs(id: stuffId, selector: e => new
      {
        StuffNoun = e.Noun,
        StuffCode = e.Code,
      }).FirstOrDefault();
      #endregion
      //check projectWork not null
      if (projectWorkId != null)
      {
        #region Add ProductionMaterialRequestTask
        App.Internals.ProjectManagement.AddProjectWorkItem(
                projectWorkItem: null,
                name: $"ثبت درخواست مواد برای دستور کار {productionOrder.Code}",
                description: $"عنوان کالا:{stuff.StuffNoun} , کد کالا:{stuff.StuffCode}",
                color: "",
                departmentId: (int)Departments.Production,
                estimatedTime: 10800,
                isCommit: false,
                scrumTaskTypeId: (int)ScrumTaskTypes.ProductionMaterialRequest,
                userId: null,
                spentTime: 0,
                remainedTime: 0,
                scrumTaskStep: ScrumTaskStep.ToDo,
                projectWorkId: projectWorkId.Value,
                baseEntityId: productionOrder.Id);
        #endregion
      }
    }
    public void AddProductionMaterialRequestTask(int productionOrderId)
    {
      var productionOrder = App.Internals.Production.GetProductionOrder(id: productionOrderId); ; AddProductionMaterialRequestTask(productionOrder: productionOrder);
    }
    #endregion
    #region ToResult
    public Expression<Func<ProductionOrder, ProductionOrderResult>> ToProductionOrderResult =
        productionOrder =>
                    new ProductionOrderResult()
                    {
                      Id = productionOrder.Id,
                      Code = productionOrder.Code,
                      StuffId = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.StuffId,
                      StuffName = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Stuff.Name,
                      StuffCode = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Stuff.Code,
                      BillOfMaterialVersion = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterialVersion,
                      BillOfMaterialValue = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Value,
                      BillOfMaterialTitle = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Title,
                      BillOfMaterialUnitId = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.UnitId,
                      BillOfMaterialUnitConversionRatio = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Unit.ConversionRatio,
                      WorkPlanId = productionOrder.WorkPlanStep.WorkPlanId,
                      WorkPlanTitle = productionOrder.WorkPlanStep.WorkPlan.Title,
                      WorkPlanVersion = productionOrder.WorkPlanStep.WorkPlan.Version,
                      WorkPlanStepId = productionOrder.WorkPlanStepId,
                      WorkPlanStepTitle = productionOrder.WorkPlanStep.Title,
                      Qty = productionOrder.Qty,
                      ToleranceQty = productionOrder.ToleranceQty,
                      UnitId = productionOrder.UnitId,
                      UnitName = productionOrder.Unit.Name,
                      ProducedQty = productionOrder.ProductionOrderSummary.ProducedQty,
                      InProductionQty = productionOrder.ProductionOrderSummary.InProductionQty,
                      DateTime = productionOrder.DateTime,
                      StartDateTime = productionOrder.CalendarEvent.DateTime,
                      ToDateTime = productionOrder.CalendarEvent.DateTime.AddSeconds(productionOrder.CalendarEvent.Duration),
                      Duration = productionOrder.CalendarEvent.Duration,
                      ProductionScheduleId = productionOrder.ProductionScheduleId,
                      UnitConversionRatio = productionOrder.Unit.ConversionRatio,
                      ProductionScheduleCode = productionOrder.ProductionSchedule.Code,
                      OrderCode = productionOrder.ProductionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Code,
                      ProductionPlanCode = productionOrder.ProductionSchedule.ProductionPlanDetail.ProductionPlan.Code,
                      ProductionRequestCode = productionOrder.ProductionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.Code,
                      ProductionStepId = productionOrder.WorkPlanStep.ProductionStepId,
                      ProductionLineId = productionOrder.WorkPlanStep.ProductionLineId,
                      ProductionStepName = productionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionStep.Name,
                      ProductionLineName = productionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionLine.Name,
                      ConsumeWarehouseId = productionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionLine.ConsumeWarehouseId,
                      ConsumeWarehouseName = productionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionLine.ConsumeWarehouse.Name,
                      ProductWarehouseId = productionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionLine.ProductWarehouseId,
                      ProductWarehouseName = productionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionLine.ProductWarehouse.Name,
                      Status = productionOrder.Status,
                      Barcode = "PO" + MyDbFunctions.Right("00000000" + productionOrder.Id, 8),
                      SupervisorEmployeeId = productionOrder.SupervisorEmployeeId,
                      SupervisorEmployeeFullName = productionOrder.Employee.FirstName + " " + productionOrder.Employee.LastName,
                      EmployeeFullName = productionOrder.User.Employee.FirstName + " " + productionOrder.User.Employee.LastName,
                      RowVersion = productionOrder.RowVersion,
                    };
    public Expression<Func<ProductionOrder, ProductionPerformanceResult>> ToProductionPerformanceResult =
        productionOrder =>
            new ProductionPerformanceResult
            {
              Id = productionOrder.Id,
              Code = productionOrder.Code,
              StuffId = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.StuffId,
              StuffName = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Stuff.Name,
              StuffCode = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Stuff.Code,
              BillOfMaterialVersion = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterialVersion,
              BillOfMaterialValue = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Value,
              BillOfMaterialTitle = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Title,
              BillOfMaterialUnitId = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.UnitId,
              BillOfMaterialUnitConversionRatio =
                    productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Unit.ConversionRatio,
              WorkPlanId = productionOrder.WorkPlanStep.WorkPlanId,
              WorkPlanTitle = productionOrder.WorkPlanStep.WorkPlan.Title,
              WorkPlanVersion = productionOrder.WorkPlanStep.WorkPlan.Version,
              WorkPlanStepId = productionOrder.WorkPlanStepId,
              WorkPlanStepTitle = productionOrder.WorkPlanStep.Title,
              Qty = productionOrder.Qty,
              UnitId = productionOrder.UnitId,
              UnitName = productionOrder.Unit.Name,
              ProducedQty = productionOrder.ProductionOrderSummary.ProducedQty,
              InProductionQty = productionOrder.ProductionOrderSummary.InProductionQty,
              ProductionDeviationQty = productionOrder.ProductionOrderSummary.ProducedQty -
                                         productionOrder.Qty,
              DateTime = productionOrder.DateTime,
              StartDateTime = productionOrder.CalendarEvent.DateTime,
              ToDateTime = productionOrder.CalendarEvent.DateTime.AddSeconds(productionOrder.CalendarEvent.Duration),
              Duration = productionOrder.CalendarEvent.Duration,
              ProductionScheduleId = productionOrder.ProductionScheduleId,
              UnitConversionRatio = productionOrder.Unit.ConversionRatio,
              ProductionScheduleCode = productionOrder.ProductionSchedule.Code,
              OrderCode = productionOrder.ProductionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest
                    .CheckOrderItem.OrderItemConfirmation.OrderItem.Code,
              ProductionPlanCode = productionOrder.ProductionSchedule.ProductionPlanDetail.ProductionPlan.Code,
              ProductionRequestCode = productionOrder.ProductionSchedule.ProductionPlanDetail.ProductionPlan
                    .ProductionRequest.Code,
              ProductionStepId = productionOrder.WorkPlanStep.ProductionStepId,
              ProductionLineId = productionOrder.WorkPlanStep.ProductionLineId,
              ProductionStepName = productionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionStep.Name,
              ProductionLineName = productionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionLine.Name,
              ConsumeWarehouseId = productionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionLine
                    .ConsumeWarehouseId,
              ConsumeWarehouseName = productionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionLine
                    .ConsumeWarehouse.Name,
              ProductWarehouseId = productionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionLine
                    .ProductWarehouseId,
              ProductWarehouseName = productionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionLine
                    .ProductWarehouse.Name,
              Status = productionOrder.Status,
              Barcode = "PO" + MyDbFunctions.Right("00000000" + productionOrder.Id, 8),
              Description = productionOrder.ProductionOrderSummary.Description,
              RowVersion = productionOrder.RowVersion,
            };
    #endregion
    #region ToProductionOrderComboResult
    public Expression<Func<ProductionOrder, ProductionOrderComboResult>> ToProductionOrderComboResult =
        productionOrder => new ProductionOrderComboResult()
        {
          Id = productionOrder.Id,
          Code = productionOrder.Code,
          RowVersion = productionOrder.RowVersion
        };
    #endregion
    #region ToFullResult
    public Expression<Func<ProductionOrder, FullProductionOrderResult>> ToFullProductionOrderResult =
        productionOrder => new FullProductionOrderResult()
        {
          Id = productionOrder.Id,
          Code = productionOrder.Code,
          StuffId = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.StuffId,
          StuffName = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Stuff.Name,
          StuffCode = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Stuff.Code,
          BillOfMaterialVersion = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterialVersion,
          BillOfMaterialValue = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Value,
          BillOfMaterialTitle = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Title,
          BillOfMaterialUnitId = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.UnitId,
          BillOfMaterialUnitConversionRatio = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Unit.ConversionRatio,
          WorkPlanId = productionOrder.WorkPlanStep.WorkPlanId,
          WorkPlanTitle = productionOrder.WorkPlanStep.WorkPlan.Title,
          WorkPlanVersion = productionOrder.WorkPlanStep.WorkPlan.Version,
          WorkPlanStepId = productionOrder.WorkPlanStepId,
          WorkPlanStepTitle = productionOrder.WorkPlanStep.Title,
          Qty = productionOrder.Qty,
          ToleranceQty = productionOrder.ToleranceQty,
          DateTime = productionOrder.DateTime,
          StartDateTime = productionOrder.CalendarEvent.DateTime,
          ToDateTime = productionOrder.CalendarEvent.DateTime.AddSeconds(productionOrder.CalendarEvent.Duration),
          Duration = productionOrder.CalendarEvent.Duration,
          ProductionScheduleId = productionOrder.ProductionScheduleId,
          UnitId = productionOrder.UnitId,
          UnitName = productionOrder.Unit.Name,
          ProducedQty = productionOrder.ProductionOrderSummary.ProducedQty,
          InProductionQty = productionOrder.ProductionOrderSummary.InProductionQty,
          UnitConversionRatio = productionOrder.Unit.ConversionRatio,
          ProductionScheduleCode = productionOrder.ProductionSchedule.Code,
          OrderCode = productionOrder.ProductionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Code,
          ProductionPlanCode = productionOrder.ProductionSchedule.ProductionPlanDetail.ProductionPlan.Code,
          ProductionRequestCode = productionOrder.ProductionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.Code,
          ProductionStepId = productionOrder.WorkPlanStep.ProductionStepId,
          ProductionLineId = productionOrder.WorkPlanStep.ProductionLineId,
          ProductionLineName = productionOrder.WorkPlanStep.ProductionLine.Name,
          ProductionStepName = productionOrder.WorkPlanStep.ProductionStep.Name,
          ProductionOperators = productionOrder.ProductionOperators.AsQueryable().Select(App.Internals.Production.ToProductionOperatorResult),
          Status = productionOrder.Status,
          ConsumeWarehouseId = productionOrder.WorkPlanStep.ProductionLine.ConsumeWarehouseId,
          ConsumeWarehouseName = productionOrder.WorkPlanStep.ProductionLine.ConsumeWarehouse.Name,
          ProductWarehouseId = productionOrder.WorkPlanStep.ProductionLine.ProductWarehouseId,
          ProductWarehouseName = productionOrder.WorkPlanStep.ProductionLine.ProductWarehouse.Name,
          SupervisorEmployeeId = productionOrder.SupervisorEmployeeId,
          SupervisorEmployeeFullName = productionOrder.Employee.FirstName + " " + productionOrder.Employee.LastName,
          EmployeeFullName = productionOrder.User.Employee.FirstName + " " + productionOrder.User.Employee.LastName,
          Barcode = "PO" + productionOrder.Id.ToString(),
          RowVersion = productionOrder.RowVersion,
        };
    #endregion
    #region Get
    public ProductionOrder GetProductionOrder(int id) => GetProductionOrder(id: id, selector: e => e);
    public TResult GetProductionOrder<TResult>(
        Expression<Func<ProductionOrder, TResult>> selector,
        int id)
    {
      var productionOrder = GetProductionOrders(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (productionOrder == null)
        throw new ProductionOrderNotFoundException(id);
      return productionOrder;
    }
    public ProductionOrder GetProductionOrder(string code) => GetProductionOrder(code: code, selector: e => e);
    public TResult GetProductionOrder<TResult>(
        Expression<Func<ProductionOrder, TResult>> selector,
        string code)
    {
      var productionOrder = GetProductionOrders(
                    selector: selector,
                    code: code)
                .FirstOrDefault();
      if (productionOrder == null)
        throw new ProductionOrderNotFoundException(code);
      return productionOrder;
    }
    #endregion
    #region GetProductionOrderLastEmployee
    public IQueryable<ProductionOrderLastEmployeeResult> GetProductionOrderLastEmployee(
       int workPlanStepId)
    {
      var productionOrderLastEmployee = GetProductionOrderLastEmployees(
                workPlanStepId: workPlanStepId);
      return productionOrderLastEmployee;
    }
    #endregion
    #region  GetProductionOrderLastEmployeeProcess
    IQueryable<ProductionOrderLastEmployeeResult> GetProductionOrderLastEmployees(
        int workPlanStepId)
    {
      var productionOrder = App.Internals.Production.GetProductionOrders(
                selector: e => e,
                workPlanStepId: workPlanStepId)
                .OrderByDescending(i => i.Id)
                .FirstOrDefault();
      if (productionOrder == null)
        throw new ProductionOrderLastEmployeeNotFoundException();
      var productionOperatorMachineEmployees = App.Internals.Production.GetProductionOperatorMachineEmployees(
                productionOrderId: productionOrder.Id,
                selector: e => e);
      var Employees = from productionOperatorMachineEmployee in productionOperatorMachineEmployees
                      select new ProductionOrderLastEmployeeResult()
                      {
                        Id = productionOperatorMachineEmployee.Id,
                        OperatorTypeId = productionOperatorMachineEmployee.ProductionOperator.OperatorTypeId,
                        EmployeeId = productionOperatorMachineEmployee.EmployeeId,
                        EmployeeFullName = productionOperatorMachineEmployee.Employee.FirstName + " " + productionOperatorMachineEmployee.Employee.LastName,
                        ProductionTerminalId = productionOperatorMachineEmployee.ProductionTerminalId,
                        OperationId = productionOperatorMachineEmployee.ProductionOperator.OperationId
                      };
      return Employees;
    }
    #endregion
    #region ToProductionOrderReportResult
    public IQueryable<ProductionOrderReportResult> ToProductionOrderReportResult(GetProductionOrderReportInput input)
    {
      IQueryable<lena.Domains.ProductionOrder> productionOrder = App.Internals.Production.GetProductionOrders(e => e, code: input.Code);
      IQueryable<lena.Domains.ProductionOperator> productionOperator = App.Internals.Production.GetProductionOperators(selector: e => e);
      IQueryable<lena.Domains.ProductionOperatorMachineEmployee> productionOperatorMachinEmployee = App.Internals.Production.GetProductionOperatorMachineEmployees(selector: e => e);
      IQueryable<ProductionOperationSequenceResult> productionOperationSequences = App.Internals.Planning.GetProductionOperationSequences(
                workPlanStepId: input.WorkPlanStepId,
                productionTerminalId: input.ProductionTerminalId,
                productionOrderCode: input.Code);
      var result = from pOrder in productionOrder
                   join pOperator in productionOperator on pOrder.Id equals pOperator.ProductionOrderId
                   join pOperationSequence in productionOperationSequences on pOperator.OperationSequenceId equals pOperationSequence.Id
                   join pOperatorMachinEmployee in productionOperatorMachinEmployee on pOperator.Id equals pOperatorMachinEmployee.ProductionOperatorId
                   select new ProductionOrderReportResult
                   {
                     Index = pOperationSequence.Index,
                     OperationCount = pOperationSequence.OperationCount,
                     BatchCount = pOperationSequence.BatchCount,
                     SwitchTime = pOperationSequence.SwitchTime,
                     StuffName = pOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Stuff.Name,
                     StuffCode = pOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Stuff.Code,
                     BillOfMaterialVersion = pOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Version,
                     ProductionStepName = pOrder.WorkPlanStep.ProductionStep.Name,
                     WorkPlanTitle = pOrder.WorkPlanStep.WorkPlan.Title,
                     Qty = pOrder.Qty,
                     UnitName = pOrder.Unit.Name,
                     ProducedQty = pOrder.ProductionOrderSummary.ProducedQty,
                     ProductionLineName = pOrder.WorkPlanStep.ProductionLine.Name,
                     ConsumeWarehouseName = pOrder.WorkPlanStep.ProductionLine.ConsumeWarehouse.Name,
                     ProductWarehouseName = pOrder.WorkPlanStep.ProductionLine.ProductWarehouse.Name,
                     OperationTitle = pOperator.Operation.Title,
                     OperatorTypeName = pOperator.OperatorType.Name,
                     DefaultTime = pOperator.DefaultTime,
                     MachineTypeOperatorTypeTitle = pOperator.MachineTypeOperatorType.Title,
                     EmployeeFullName = pOperatorMachinEmployee.Employee.FirstName + " " + pOperatorMachinEmployee.Employee.LastName,
                     ProductionTerminalId = pOperatorMachinEmployee.ProductionTerminal.Id,
                     ProductionTerminalDescription = pOperatorMachinEmployee.ProductionTerminal.Description,
                   };
      return result;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionOrders<TResult>(
        Expression<Func<ProductionOrder, TResult>> selector,
        TValue<int> id = null,
        TValue<DateTime> dateTime = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int?> productionScheduleId = null,
        TValue<double> qty = null,
        TValue<int> workPlanStepId = null,
        TValue<int> workPlanId = null,
        TValue<int> stuffId = null,
        TValue<int> version = null,
        TValue<int> unitId = null,
        TValue<ProductionOrderStatus> status = null,
        TValue<ProductionOrderStatus[]> statuses = null,
        TValue<ProductionOrderStatus[]> notHasStatuses = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var productionOrders = baseQuery.OfType<ProductionOrder>();
      if (productionScheduleId != null)
        productionOrders = productionOrders.Where(i => i.ProductionScheduleId == productionScheduleId);
      if (qty != null)
        productionOrders = productionOrders.Where(i => i.Qty == qty);
      if (unitId != null)
        productionOrders = productionOrders.Where(i => i.UnitId == unitId);
      if (workPlanStepId != null)
        productionOrders = productionOrders.Where(i => i.WorkPlanStepId == workPlanStepId);
      if (workPlanId != null)
        productionOrders = productionOrders.Where(i => i.WorkPlanStep.WorkPlan.Id == workPlanId);
      if (stuffId != null)
        productionOrders = productionOrders.Where(i => i.WorkPlanStep.WorkPlan.BillOfMaterial.StuffId == stuffId);
      if (version != null)
        productionOrders = productionOrders.Where(i => i.WorkPlanStep.WorkPlan.BillOfMaterial.Version == version);
      if (status != null)
        productionOrders = productionOrders.Where(i => i.Status.HasFlag(status));
      if (statuses != null)
      {
        var s = ProductionOrderStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        productionOrders = productionOrders.Where(i => (i.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = ProductionOrderStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        productionOrders = productionOrders.Where(i => (i.Status & s) == 0);
      }
      if (fromDateTime != null && toDateTime == null)
        productionOrders = productionOrders.Where(i => i.CalendarEvent.DateTime.AddSeconds(i.CalendarEvent.Duration) >= fromDateTime);
      if (toDateTime != null && fromDateTime == null)
        productionOrders = productionOrders.Where(i => i.CalendarEvent.DateTime < toDateTime);
      if (fromDateTime != null && toDateTime != null)
        productionOrders = productionOrders.Where(i =>
                  (i.CalendarEvent.DateTime <= toDateTime && i.CalendarEvent.DateTime >= fromDateTime) ||
                  (i.CalendarEvent.DateTime.AddSeconds(i.CalendarEvent.Duration) <= toDateTime && i.CalendarEvent.DateTime.AddSeconds(i.CalendarEvent.Duration) >= fromDateTime) ||
                  (i.CalendarEvent.DateTime <= fromDateTime && i.CalendarEvent.DateTime.AddSeconds(i.CalendarEvent.Duration) >= toDateTime));
      return productionOrders.Select(selector);
    }
    #endregion
    #region Delete
    public ProductionOrder DeleteProductionOrder(int id, byte[] rowVersion)
    {
      var productionOrder = GetProductionOrder(id);
      var hasStatus = ProductionOrderStatus.None | ProductionOrderStatus.Produced | ProductionOrderStatus.InProduction | ProductionOrderStatus.Finished;
      if ((productionOrder.Status & hasStatus) > 0)
        throw new ProductionOrderNotInNotActionAndProductionMaterialRequestedStatusException(productionOrder.Id);
      else
      {
        #region Remove BaseEntity
        App.Internals.ApplicationBase.RemoveBaseEntityProcess(
        transactionBatchId: null,
        baseEntity: productionOrder,
        rowVersion: rowVersion);
        #endregion
      }
      return productionOrder;
    }
    #endregion
    #region FinishProductionOrderStatus
    public ProductionOrder FinishProductionOrderStatus(
        int id,
        byte[] rowVersion)
    {
      var productionOrder = GetProductionOrder(id);
      var finishedProductionOrderStatus = productionOrder.Status | ProductionOrderStatus.Finished;
      #region Edit ProductionOrder
      if (productionOrder.Status != finishedProductionOrderStatus)
        EditProductionOrder(
                      productionOrder: productionOrder,
                      rowVersion: rowVersion,
                      status: finishedProductionOrderStatus);
      #endregion
      return productionOrder;
    }
    #endregion
    #region UndoFinishedProductionOrderStatus
    public ProductionOrder UndoFinishedProductionOrderStatus(
        int id,
        byte[] rowVersion)
    {
      var productionOrder = GetProductionOrder(id);
      var UndoFinishedProductionOrderStatus = productionOrder.Status & ~(ProductionOrderStatus.Finished);
      #region Edit ProductionOrder
      if (productionOrder.Status != UndoFinishedProductionOrderStatus)
        EditProductionOrder(
                      productionOrder: productionOrder,
                      rowVersion: rowVersion,
                      status: UndoFinishedProductionOrderStatus);
      #endregion
      return productionOrder;
    }
    #endregion
    #region Search
    public IQueryable<ProductionOrderResult> SearchProductionScheduleResult(
        IQueryable<ProductionOrderResult> query,
        string search,
        string orderCode,
        string productionRequestCode,
        string productionPlanCode,
        string productionScheduleCode,
        int? productionStepId,
        int? productionLineId,
        int? stuffId,
        int? version,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where item.BillOfMaterialTitle.Contains(search) ||
                item.ConsumeWarehouseName.Contains(search) ||
                item.StuffName.Contains(search) ||
                item.UnitName.Contains(search) ||
                item.Code.Contains(search) ||
                item.ProductionScheduleCode.Contains(search) ||
                item.OrderCode.Contains(search) ||
                item.ProductionPlanCode.Contains(search) ||
                item.ProductionRequestCode.Contains(search) ||
                item.StuffCode.Contains(search) ||
                item.WorkPlanStepTitle.Contains(search)
                select item;
      if (!string.IsNullOrWhiteSpace(orderCode))
        query = query.Where(i => i.OrderCode == orderCode);
      if (!string.IsNullOrWhiteSpace(productionRequestCode))
        query = query.Where(i => i.ProductionRequestCode == productionRequestCode);
      if (!string.IsNullOrWhiteSpace(productionPlanCode))
        query = query.Where(i => i.ProductionPlanCode == productionPlanCode);
      if (!string.IsNullOrWhiteSpace(productionScheduleCode))
        query = query.Where(i => i.ProductionScheduleCode == productionScheduleCode);
      if (productionStepId != null)
        query = query.Where(i => i.ProductionStepId == productionStepId);
      if (productionLineId != null)
        query = query.Where(i => i.ProductionLineId == productionLineId);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (version != null)
        query = query.Where(i => i.BillOfMaterialVersion == version);
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    public IQueryable<ProductionPerformanceResult> SearchProductionPerformanceResult(
       IQueryable<ProductionPerformanceResult> query,
       string search,
       string orderCode,
       string productionRequestCode,
       string productionPlanCode,
       string productionScheduleCode,
       int? productionStepId,
       int? productionLineId,
       int? stuffId,
       int? version,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where item.BillOfMaterialTitle.Contains(search) ||
                item.ConsumeWarehouseName.Contains(search) ||
                item.StuffName.Contains(search) ||
                item.UnitName.Contains(search) ||
                item.Code.Contains(search) ||
                item.ProductionScheduleCode.Contains(search) ||
                item.OrderCode.Contains(search) ||
                item.ProductionPlanCode.Contains(search) ||
                item.ProductionRequestCode.Contains(search) ||
                item.StuffCode.Contains(search) ||
                item.WorkPlanStepTitle.Contains(search)
                select item;
      if (!string.IsNullOrWhiteSpace(orderCode))
        query = query.Where(i => i.OrderCode == orderCode);
      if (!string.IsNullOrWhiteSpace(productionRequestCode))
        query = query.Where(i => i.ProductionRequestCode == productionRequestCode);
      if (!string.IsNullOrWhiteSpace(productionPlanCode))
        query = query.Where(i => i.ProductionPlanCode == productionPlanCode);
      if (!string.IsNullOrWhiteSpace(productionScheduleCode))
        query = query.Where(i => i.ProductionScheduleCode == productionScheduleCode);
      if (productionStepId != null)
        query = query.Where(i => i.ProductionStepId == productionStepId);
      if (productionLineId != null)
        query = query.Where(i => i.ProductionLineId == productionLineId);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (version != null)
        query = query.Where(i => i.BillOfMaterialVersion == version);
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ProductionOrderResult> SortProductionOrderResult(
        IQueryable<ProductionOrderResult> query,
        SortInput<ProductionOrderSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case ProductionOrderSortType.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case ProductionOrderSortType.Code:
          return query.OrderBy(r => r.Code, sortInput.SortOrder);
        case ProductionOrderSortType.ProductionScheduleCode:
          return query.OrderBy(r => r.ProductionScheduleCode, sortInput.SortOrder);
        case ProductionOrderSortType.StuffName:
          return query.OrderBy(r => r.StuffName, sortInput.SortOrder);
        case ProductionOrderSortType.StuffCode:
          return query.OrderBy(r => r.StuffCode, sortInput.SortOrder);
        case ProductionOrderSortType.BillOfMaterialVersion:
          return query.OrderBy(r => r.BillOfMaterialVersion, sortInput.SortOrder);
        case ProductionOrderSortType.BillOfMaterialTitle:
          return query.OrderBy(r => r.BillOfMaterialTitle, sortInput.SortOrder);
        case ProductionOrderSortType.WorkPlanTitle:
          return query.OrderBy(r => r.WorkPlanTitle, sortInput.SortOrder);
        case ProductionOrderSortType.WorkPlanStepTitle:
          return query.OrderBy(r => r.WorkPlanStepTitle, sortInput.SortOrder);
        case ProductionOrderSortType.WorkPlanVersion:
          return query.OrderBy(r => r.WorkPlanVersion, sortInput.SortOrder);
        case ProductionOrderSortType.Qty:
          return query.OrderBy(r => r.Qty, sortInput.SortOrder);
        case ProductionOrderSortType.UnitName:
          return query.OrderBy(r => r.UnitName, sortInput.SortOrder);
        case ProductionOrderSortType.FromDateTime:
          return query.OrderBy(r => r.StartDateTime, sortInput.SortOrder);
        case ProductionOrderSortType.ToDateTime:
          return query.OrderBy(r => r.ToDateTime, sortInput.SortOrder);
        case ProductionOrderSortType.ProductionLineName:
          return query.OrderBy(r => r.ProductionLineName, sortInput.SortOrder);
        case ProductionOrderSortType.ProductionStepName:
          return query.OrderBy(r => r.ProductionStepName, sortInput.SortOrder);
        case ProductionOrderSortType.Status:
          return query.OrderBy(r => r.Status, sortInput.SortOrder);
        case ProductionOrderSortType.ProducedQty:
          return query.OrderBy(r => r.ProducedQty, sortInput.SortOrder);
        case ProductionOrderSortType.InProductionQty:
          return query.OrderBy(r => r.ProducedQty, sortInput.SortOrder);
        case ProductionOrderSortType.SupervisorEmployeeFullName:
          return query.OrderBy(r => r.SupervisorEmployeeFullName, sortInput.SortOrder);
        case ProductionOrderSortType.EmployeeFullName:
          return query.OrderBy(r => r.EmployeeFullName, sortInput.SortOrder);
        case ProductionOrderSortType.DateTime:
          return query.OrderBy(r => r.DateTime, sortInput.SortOrder);
        case ProductionOrderSortType.ToleranceQty:
          return query.OrderBy(r => r.ToleranceQty, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException(nameof(sortInput.SortOrder), sortInput.SortType, null);
      }
    }
    public IOrderedQueryable<ProductionPerformanceResult> SortProductionPerformanceResult(
       IQueryable<ProductionPerformanceResult> query,
       SortInput<ProductionPerformanceSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case ProductionPerformanceSortType.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case ProductionPerformanceSortType.Code:
          return query.OrderBy(r => r.Code, sortInput.SortOrder);
        case ProductionPerformanceSortType.ProductionScheduleCode:
          return query.OrderBy(r => r.ProductionScheduleCode, sortInput.SortOrder);
        case ProductionPerformanceSortType.StuffName:
          return query.OrderBy(r => r.StuffName, sortInput.SortOrder);
        case ProductionPerformanceSortType.StuffCode:
          return query.OrderBy(r => r.StuffCode, sortInput.SortOrder);
        case ProductionPerformanceSortType.BillOfMaterialVersion:
          return query.OrderBy(r => r.BillOfMaterialVersion, sortInput.SortOrder);
        case ProductionPerformanceSortType.BillOfMaterialTitle:
          return query.OrderBy(r => r.BillOfMaterialTitle, sortInput.SortOrder);
        case ProductionPerformanceSortType.WorkPlanTitle:
          return query.OrderBy(r => r.WorkPlanTitle, sortInput.SortOrder);
        case ProductionPerformanceSortType.WorkPlanStepTitle:
          return query.OrderBy(r => r.WorkPlanStepTitle, sortInput.SortOrder);
        case ProductionPerformanceSortType.WorkPlanVersion:
          return query.OrderBy(r => r.WorkPlanVersion, sortInput.SortOrder);
        case ProductionPerformanceSortType.Qty:
          return query.OrderBy(r => r.Qty, sortInput.SortOrder);
        case ProductionPerformanceSortType.UnitName:
          return query.OrderBy(r => r.UnitName, sortInput.SortOrder);
        case ProductionPerformanceSortType.FromDateTime:
          return query.OrderBy(r => r.StartDateTime, sortInput.SortOrder);
        case ProductionPerformanceSortType.ToDateTime:
          return query.OrderBy(r => r.ToDateTime, sortInput.SortOrder);
        case ProductionPerformanceSortType.ProductionLineName:
          return query.OrderBy(r => r.ProductionLineName, sortInput.SortOrder);
        case ProductionPerformanceSortType.ProductionStepName:
          return query.OrderBy(r => r.ProductionStepName, sortInput.SortOrder);
        case ProductionPerformanceSortType.Status:
          return query.OrderBy(r => r.Status, sortInput.SortOrder);
        case ProductionPerformanceSortType.ProducedQty:
          return query.OrderBy(r => r.ProducedQty, sortInput.SortOrder);
        case ProductionPerformanceSortType.InProductionQty:
          return query.OrderBy(r => r.ProducedQty, sortInput.SortOrder);
        case ProductionPerformanceSortType.ProductionDeviationQty:
          return query.OrderBy(r => r.ProductionDeviationQty, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException(nameof(sortInput.SortOrder), sortInput.SortType, null);
      }
    }
    #endregion
    #region ResetProductionOrderStatus
    public ProductionOrder ResetProductionOrderStatus(int id)
    {
      var productionOrder = GetProductionOrder(id: id); ; return ResetProductionOrderStatus(productionOrder: productionOrder);
    }
    public ProductionOrder ResetProductionOrderStatus(ProductionOrder productionOrder)
    {
      #region ResetProductionOrderSummary
      var productionOrderSummary = ResetProductionOrderSummaryByProductionOrderId(productionOrder.Id);
      #endregion
      #region Define Status
      var status = ProductionOrderStatus.None;
      if (productionOrderSummary.ProducedQty > 0)
      {
        if (productionOrderSummary.ProducedQty >= productionOrderSummary.ProductionOrder.Qty)
          status = status | ProductionOrderStatus.Produced;
        else
          status = status | ProductionOrderStatus.InProduction;
      }
      if ((productionOrder.Status & ProductionOrderStatus.Finished) == ProductionOrderStatus.Finished)
        status = status | ProductionOrderStatus.Finished;
      #region GetProductionMaterialRequests
      var productionMaterialRequests = App.Internals.WarehouseManagement.GetProductionMaterialRequests(
              selector: e => e,
              productionOrderId: productionOrder.Id,
              isDelete: false);
      if (productionMaterialRequests.Any())
        status = status | ProductionOrderStatus.ProductionMaterialRequested;
      #endregion
      if (status == ProductionOrderStatus.None)
        status = ProductionOrderStatus.NotAction;
      #endregion
      #region Edit ProductionOrder
      if (productionOrder.Status != status)
        EditProductionOrder(
                      productionOrder: productionOrder,
                      rowVersion: productionOrder.RowVersion,
                      status: status);
      #endregion
      return productionOrder;
    }
    #endregion
    #region GetProductionOrderBillOfMaterialDetails
    public IQueryable<ProductionOrderBillOfMaterialDetailResult> GetProductionOrderBillOfMaterialDetails(
        int productionOrderId)
    {
      #region GetProductionOrder
      var productionOrder = GetProductionOrder(id: productionOrderId);
      #endregion
      return GetProductionOrderBillOfMaterialDetails(
          productionOrder: productionOrder);
    }
    public IQueryable<ProductionOrderBillOfMaterialDetailResult> GetProductionOrderBillOfMaterialDetails(
        ProductionOrder productionOrder)
    {
      #region GetFactor
      var billOfMaterial = productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial;
      var factor = productionOrder.Qty * productionOrder.Unit.ConversionRatio /
                         billOfMaterial.Unit.ConversionRatio;
      #endregion
      #region GetConsumeWarehouseId
      var consumeWarehouse = productionOrder.WorkPlanStep.ProductionLine.ConsumeWarehouse;
      #endregion
      #region Get BillOfMaterialDetails
      var billOfMaterialDetailQuery = App.Internals.Planning.GetBillOfMaterialDetails(
              billOfMaterialStuffId: billOfMaterial.StuffId,
              billOfMaterialVersion: billOfMaterial.Version,
              billOfMaterialDetailType: BillOfMaterialDetailType.Material);
      var mainUnits = App.Internals.ApplicationBase.GetUnits(e => e, isMainUnit: true);
      var billOfMaterialDetails = (from bomDetail in billOfMaterialDetailQuery
                                   join mainUnit in mainUnits on bomDetail.Unit.UnitTypeId equals mainUnit.UnitTypeId
                                   select new
                                   {
                                     StuffId = bomDetail.StuffId,
                                     StuffCode = bomDetail.Stuff.Code,
                                     StuffName = bomDetail.Stuff.Name,
                                     Version = bomDetail.SemiProductBillOfMaterialVersion,
                                     BillOfMaterialDetailValue = bomDetail.Value,
                                     Qty = (factor * bomDetail.Value / bomDetail.ForQty),
                                     FaultyPercentage = bomDetail.Stuff.FaultyPercentage,
                                     UnitId = bomDetail.UnitId,
                                     UnitName = bomDetail.Unit.Name,
                                     DefaultWarehouseId = bomDetail.Stuff.StuffCategory.DefaultWarehouseId,
                                     DefaultWarehouseName = bomDetail.Stuff.StuffCategory.DefaultWarehouse.Name,
                                     DecimalDigitCount = bomDetail.Unit.DecimalDigitCount,
                                     MainUnitQty = (factor * bomDetail.Value / bomDetail.ForQty) * mainUnit.ConversionRatio,
                                     MainUnitDecimalDigitCount = mainUnit.DecimalDigitCount,
                                   }).ToList();
      #endregion
      #region CreateStuffIds
      var stuffIds = from item in billOfMaterialDetailQuery
                     select item.StuffId;
      #endregion
      #region GetStockForStuffs
      var consumeInventories = App.Internals.WarehouseManagement.GetWarehouseInventories(
              warehouseId: consumeWarehouse.Id,
              groupByBillOfMaterialVersion: true,
              groupBySerial: false,
              stuffIds: stuffIds)
          .ToList();
      #endregion
      #region GetStockForStuffs
      var defaultInventories = new List<WarehouseInventoryResult>();
      foreach (var billOfMaterialDetail in billOfMaterialDetails)
      {
        var inventory = App.Internals.WarehouseManagement.GetWarehouseInventories(
                      warehouseId: billOfMaterialDetail.DefaultWarehouseId,
                      stuffId: billOfMaterialDetail.StuffId,
                      billOfMaterialVersion: billOfMaterialDetail.Version)
                  .ToList();
        defaultInventories.AddRange(inventory);
      }
      #endregion
      #region Create ProductionOrderBillOfMaterialDetailResult
      var result = from bomDetail in billOfMaterialDetails
                   join tDefaultStock in defaultInventories on
                         new
                         {
                           StuffId = bomDetail.StuffId,
                           WarehouseId = bomDetail.DefaultWarehouseId,
                           Version = bomDetail.Version
                         }
                             equals
                             new
                             {
                               StuffId = tDefaultStock.StuffId,
                               WarehouseId = tDefaultStock.WarehouseId,
                               Version = tDefaultStock.BillOfMaterialVersion
                             }
                             into defaultStocks
                   from defaultStock in defaultStocks.DefaultIfEmpty()
                   join tConsumeStock in consumeInventories on
                             new
                             {
                               StuffId = bomDetail.StuffId,
                               WarehouseId = consumeWarehouse.Id,
                               Version = bomDetail.Version
                             }
                             equals
                             new
                             {
                               StuffId = tConsumeStock.StuffId,
                               WarehouseId = tConsumeStock.WarehouseId,
                               Version = tConsumeStock.BillOfMaterialVersion
                             }
                             into consumeStocks
                   from consumeStock in consumeStocks.DefaultIfEmpty()
                   select new ProductionOrderBillOfMaterialDetailResult
                   {
                     StuffId = bomDetail.StuffId,
                     StuffCode = bomDetail.StuffCode,
                     StuffName = bomDetail.StuffName,
                     Version = bomDetail.Version,
                     BillOfMaterialDetailValue = bomDetail.BillOfMaterialDetailValue,
                     Qty = bomDetail.Qty,
                     FaultyPercentage = bomDetail.FaultyPercentage,
                     UnitId = bomDetail.UnitId,
                     UnitName = bomDetail.UnitName,
                     ConsumeWarehouseId = consumeWarehouse.Id,
                     ConsumeWarehouseName = consumeWarehouse.Name,
                     ConsumeWarehouseAmount = consumeStock?.AvailableAmount ?? 0,
                     DefaultWarehouseId = bomDetail.DefaultWarehouseId,
                     DefaultWarehouseName = bomDetail.DefaultWarehouseName,
                     DefaultWarehouseAmount = defaultStock?.AvailableAmount ?? 0,
                     StockUnitId = defaultStock?.UnitId ?? consumeStock?.UnitId ?? bomDetail.UnitId,
                     StockUnitName = defaultStock?.UnitName ?? defaultStock?.UnitName ?? bomDetail.UnitName,
                     DecimalDigitCount = bomDetail.DecimalDigitCount,
                     MainUnitQty = bomDetail.MainUnitQty,
                     MainUnitDecimalDigitCount = bomDetail.MainUnitDecimalDigitCount,
                   };
      #endregion
      return result.AsQueryable();
    }
    #endregion
  }
}
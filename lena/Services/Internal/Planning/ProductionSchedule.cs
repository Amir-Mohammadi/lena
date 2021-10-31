using System;
//using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.ProductionSchedule;
using lena.Models.Planning.ProductionLine;
using lena.Models.Planning.ProductionLineWorkShift;
using lena.Models.ApplicationBase.CalendarEvent;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region save
    public SaveProductionScheduleInput SaveProductionSchedules(
      AddProductionScheduleInput[] addProductionScheduleInputs = null,
      EditProductionScheduleInput[] editProductionScheduleInputs = null,
      DeleteProductionScheduleInput[] deleteProductionScheduleInputs = null)
    {
      #region AddeProductionScheduleInput
      if (addProductionScheduleInputs != null)
      {
        foreach (var addProductionScheduleInput in addProductionScheduleInputs)
        {
          var newProductionSchedul = AddProductionScheduleProcess(dateTime: addProductionScheduleInput.DateTime,
                    workPlanStepId: addProductionScheduleInput.WorkPlanStepId,
                    duration: addProductionScheduleInput.Duration,
                    productionPlanDetailId: addProductionScheduleInput.ProductionPlanDetailId,
                    qty: addProductionScheduleInput.Qty,
                    applySwitchTime: addProductionScheduleInput.ApplySwitchTime,
                    operatorCount: addProductionScheduleInput.OperatorCount,
                    planningWithoutMachineLimit: addProductionScheduleInput.PlanningWithoutMachineLimit,
                    switchTime: addProductionScheduleInput.SwitchTime);
        }
      }
      #endregion
      #region EditeProductionScheduleInput
      if (editProductionScheduleInputs != null)
        foreach (var editProductionScheduleInput in editProductionScheduleInputs)
        {
          var editProductionSchedul = EditProductionScheduleProcess(
                        id: editProductionScheduleInput.Id,
                        rowVersion: editProductionScheduleInput.RowVersion,
                        dateTime: editProductionScheduleInput.DateTime,
                        duration: editProductionScheduleInput.Duration,
                        qty: editProductionScheduleInput.Qty,
                        applySwitchTime: editProductionScheduleInput.ApplySwitchTime,
                        operatorCount: editProductionScheduleInput.OperatorCount,
                        planningWithoutMachineLimit: editProductionScheduleInput.PlanningWithoutMachineLimit,
                        productionPlanDetailId: editProductionScheduleInput.ProductionPlanDetailId,
                        workPlanStepId: editProductionScheduleInput.WorkPlanStepId,
                        switchTime: editProductionScheduleInput.SwitchTime);
        }
      #endregion
      #region DeleteeProductionScheduleInput
      if (deleteProductionScheduleInputs != null)
        foreach (var deleteProductionSchedule in deleteProductionScheduleInputs)
          DeleteProductionScheduleProcess(id: deleteProductionSchedule.Id,
                          rowVersion: deleteProductionSchedule.RowVersion);
      #endregion
      return new SaveProductionScheduleInput();
    }
    #endregion
    #region Add
    public ProductionSchedule AddProductionSchedule(
        ProductionSchedule productionSchedule,
        TransactionBatch transactionBatch,
        string description,
        double qty,
        bool applySwitchTime,
        int productionPlanDetailId,
        int workPlanStepId,
        CalendarEvent calendarEvent,
        int operatorCount,
        bool planningWithoutMachineLimit,
        int switchTime)
    {
      productionSchedule = productionSchedule ?? repository.Create<ProductionSchedule>();
      productionSchedule.WorkPlanStepId = workPlanStepId;
      productionSchedule.Qty = qty;
      productionSchedule.IsPublished = false;
      productionSchedule.ApplySwitchTime = applySwitchTime;
      productionSchedule.ProductionPlanDetailId = productionPlanDetailId;
      productionSchedule.CalendarEvent = calendarEvent;
      productionSchedule.PlanningWithoutMachineLimit = planningWithoutMachineLimit;
      productionSchedule.OperatorCount = operatorCount;
      productionSchedule.SwitchTime = switchTime;
      productionSchedule.Status = ProductionScheduleStatus.NotAction;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: productionSchedule,
                    transactionBatch: transactionBatch,
                    description: description);
      return productionSchedule;
    }
    #endregion
    #region Edit
    public ProductionSchedule EditProductionSchedule(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<bool> applySwitchTime = null,
        TValue<int> workPlanStepId = null,
        TValue<int> productionPlanId = null,
        TValue<int> calendarEventId = null,
        TValue<bool> isPublished = null,
        TValue<int> operatorCount = null,
        TValue<bool> planningWithoutMachineLimit = null,
        TValue<int> switchTime = null,
        TValue<ProductionScheduleStatus> status = null)
    {
      var productionSchedule = GetProductionSchedule(id: id);
      return EditProductionSchedule(
                    productionSchedule: productionSchedule,
                    rowVersion: rowVersion,
                    description: description,
                    qty: qty,
                    applySwitchTime: applySwitchTime,
                    workPlanStepId: workPlanStepId,
                    calendarEventId: calendarEventId,
                    productionPlanDetailId: productionPlanId,
                    isPublished: isPublished,
                    operatorCount: operatorCount,
                    planningWithoutMachineLimit: planningWithoutMachineLimit,
                    switchTime: switchTime,
                    status: status);
    }
    public ProductionSchedule EditProductionSchedule(
        ProductionSchedule productionSchedule,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<bool> applySwitchTime = null,
        TValue<int> workPlanStepId = null,
        TValue<int> calendarEventId = null,
        TValue<int> productionPlanDetailId = null,
        TValue<bool> isPublished = null,
        TValue<int> operatorCount = null,
        TValue<bool> planningWithoutMachineLimit = null,
        TValue<int> switchTime = null,
        TValue<ProductionScheduleStatus> status = null)
    {
      if (productionPlanDetailId != null)
        productionSchedule.ProductionPlanDetailId = productionPlanDetailId;
      if (qty != null)
        productionSchedule.Qty = qty;
      if (isPublished != null)
        productionSchedule.IsPublished = isPublished;
      if (applySwitchTime != null)
        productionSchedule.ApplySwitchTime = applySwitchTime;
      if (workPlanStepId != null)
        productionSchedule.WorkPlanStepId = workPlanStepId;
      if (planningWithoutMachineLimit != null)
        productionSchedule.PlanningWithoutMachineLimit = planningWithoutMachineLimit;
      if (operatorCount != null)
        productionSchedule.OperatorCount = operatorCount;
      if (switchTime != null)
        productionSchedule.SwitchTime = switchTime;
      if (calendarEventId != null)
        productionSchedule.CalendarEvent = App.Internals.Planning.GetScheduleCalendarEvent(
                  id: calendarEventId);
      if (status != null)
        productionSchedule.Status = status;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: productionSchedule,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as ProductionSchedule;
    }
    #endregion
    #region Get
    public ProductionSchedule GetProductionSchedule(int id) => GetProductionSchedule(selector: e => e,
        id: id);
    public TResult GetProductionSchedule<TResult>(Expression<Func<ProductionSchedule, TResult>> selector, int id)
    {
      var productionSchedule = GetProductionSchedules(selector: selector,
                    id: id)
                .FirstOrDefault();
      if (productionSchedule == null)
        throw new ProductionScheduleNotFoundException(id);
      return productionSchedule;
    }
    public ProductionSchedule GetProductionSchedule(string code) => GetProductionSchedule(selector: e => e,
        code: code);
    public TResult GetProductionSchedule<TResult>(Expression<Func<ProductionSchedule, TResult>> selector, string code)
    {
      var productionSchedule = GetProductionSchedules(selector: selector,
                    code: code)
                .FirstOrDefault();
      if (productionSchedule == null)
        throw new ProductionScheduleNotFoundException(code);
      return productionSchedule;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionSchedules<TResult>(
        Expression<Func<ProductionSchedule, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> productionPlanDetailId = null,
        TValue<double> qty = null,
        TValue<bool> applySwitchTime = null,
        TValue<int> calendarEventId = null,
        TValue<ProductionScheduleStatus> status = null,
        TValue<ProductionScheduleStatus[]> statuses = null,
        TValue<ProductionScheduleStatus[]> notHasStatuses = null,
        TValue<bool> isPublished = null,
        TValue<int> productionStepId = null,
        TValue<int> productionLineId = null,
        TValue<int[]> productionLineIds = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<int> stuffId = null,
        TValue<int> version = null,
        TValue<int> productionPlanId = null,
        TValue<int> workPlanStepId = null,
        TValue<int> workPlanId = null,
        TValue<int> productionRequestId = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var productionSchedules = baseQuery.OfType<ProductionSchedule>();
      if (qty != null)
        productionSchedules = productionSchedules.Where(i => Math.Abs(i.Qty - qty) < 0.000001);
      if (stuffId != null)
        productionSchedules = productionSchedules.Where(i => i.ProductionPlanDetail.BillOfMaterialStuffId == stuffId);
      if (version != null)
        productionSchedules = productionSchedules.Where(i => i.ProductionPlanDetail.BillOfMaterialVersion == version);
      if (applySwitchTime != null)
        productionSchedules = productionSchedules.Where(i => i.ApplySwitchTime == applySwitchTime);
      if (calendarEventId != null)
        productionSchedules = productionSchedules.Where(i => i.CalendarEvent.Id == calendarEventId);
      if (productionPlanDetailId != null)
        productionSchedules = productionSchedules.Where(i => i.ProductionPlanDetailId == productionPlanDetailId);
      if (isPublished != null)
        productionSchedules = productionSchedules.Where(x => x.IsPublished == isPublished);
      if (productionStepId != null)
        productionSchedules = productionSchedules.Where(x => x.WorkPlanStep.ProductionStepId == productionStepId);
      if (productionLineId != null)
        productionSchedules = productionSchedules.Where(x => x.WorkPlanStep.ProductionLineId == productionLineId);
      if (productionLineIds != null)
        productionSchedules = productionSchedules.Where(x => productionLineIds.Value.Contains(x.WorkPlanStep.ProductionLineId));
      if (fromDateTime != null)
        productionSchedules = productionSchedules.Where(x => x.CalendarEvent.DateTime >= fromDateTime);
      if (toDateTime != null)
        productionSchedules = productionSchedules.Where(x => x.CalendarEvent.DateTime <= toDateTime);
      if (productionPlanId != null)
        productionSchedules = productionSchedules.Where(x => x.ProductionPlanDetail.ProductionPlanId == productionPlanId);
      if (workPlanStepId != null)
        productionSchedules = productionSchedules.Where(x => x.WorkPlanStepId == workPlanStepId);
      if (workPlanId != null)
        productionSchedules = productionSchedules.Where(x => x.WorkPlanStep.WorkPlanId == workPlanId);
      if (productionRequestId != null)
        productionSchedules = productionSchedules.Where(x => x.ProductionPlanDetail.ProductionPlan.ProductionRequestId == productionRequestId);
      if (status != null)
        productionSchedules = productionSchedules.Where(i => i.Status.HasFlag(status));
      if (statuses != null)
      {
        var s = ProductionScheduleStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        productionSchedules = productionSchedules.Where(i => (i.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = ProductionScheduleStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        productionSchedules = productionSchedules.Where(i => (i.Status & s) == 0);
      }
      return productionSchedules.Select(selector);
    }
    #endregion
    #region AddProcess
    public ProductionSchedule AddProductionScheduleProcess(
        int productionPlanDetailId,
        int workPlanStepId,
        DateTime dateTime,
        long duration,
        double? qty,
        bool applySwitchTime,
        int operatorCount,
        bool planningWithoutMachineLimit,
        int switchTime)
    {
      var productionPlanDetail = GetProductionPlanDetail(productionPlanDetailId); ; var workPlanStep = GetWorkPlanStep(id: workPlanStepId);
      return AddProductionScheduleProcess(
                    productionPlanDetail: productionPlanDetail,
                    workPlanStep: workPlanStep,
                    dateTime: dateTime,
                    duration: duration,
                    qty: qty,
                    applySwitchTime: applySwitchTime,
                    operatorCount: operatorCount,
                    planningWithoutMachineLimit: planningWithoutMachineLimit,
                    switchTime: switchTime);
    }
    public ProductionSchedule AddProductionScheduleProcess(
        ProductionPlanDetail productionPlanDetail,
        WorkPlanStep workPlanStep,
        DateTime dateTime,
        long duration,
        double? qty,
        bool applySwitchTime,
        int operatorCount,
        bool planningWithoutMachineLimit,
        int switchTime)
    {
      #region Insert CalendarEvent
      var scheduleCalendarEvent = AddScheduleCalendarEvent(
              dateTime: dateTime,
              duration: duration,
              productionSchedule: null);
      #endregion
      #region Insert ProductionSchedule Process
      #region Get WorkPlan and BOM
      var workPlan = workPlanStep.WorkPlan;
      var bom = workPlan.BillOfMaterial;
      #endregion
      #region Check Interference
      var calendarEvent = CheckInterferenceScheduleCalendarEvent(
              psFromDateTime: dateTime,
              duration: duration,
              productionLineId: workPlanStep.ProductionLineId)
          .AsQueryable();
      //if (hasCalendarEvent)
      //    throw new InterferenceScheduleCalendarEventException();
      var calendarEventWithStuffId = App.Internals.ApplicationBase.ToCalendarEventResultQueryWithStuffId(calendarEvent).ToList();
      if (calendarEventWithStuffId.Count != 0)
        throw new InterferenceScheduleCalendarEventException(calendarEvent: calendarEventWithStuffId);
      #endregion
      #region Check duration
      var pureTime = duration;
      if (applySwitchTime)
        pureTime = pureTime - workPlanStep.SwitchTime;
      pureTime = pureTime - workPlanStep.InitialTime;
      if (pureTime < 0)
        throw new ProductionScheduleIncorrectDurationException(duration: duration,
                  switchTime: workPlanStep.SwitchTime);
      #endregion
      #region Calculate qty
      var estimatedQty = 0d;
      if (planningWithoutMachineLimit)
      {
        var totalTime = workPlanStep.OperationSequences.Sum(i => i.DefaultTime);
        estimatedQty = (duration * (double)operatorCount) / totalTime;
      }
      else
        estimatedQty = Math.Floor((double)pureTime / workPlanStep.BatchTime * workPlanStep.BatchCount);
      qty = qty ?? estimatedQty;
      if (Math.Round((qty.Value - estimatedQty), bom.Unit.DecimalDigitCount) > 0)
      {
        throw new QtyGreaterThanEstimatedQtyException(qty.Value, estimatedQty);
      }
      #endregion
      #region Insert TransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      var transactionBatch = warehouseManagement.AddTransactionBatch();
      #endregion
      #region Insert TransactionPlans
      foreach (var bomDetail in bom.BillOfMaterialDetails)
      {
        #region TransactionType
        TransactionType transactionType;
        switch (bomDetail.BillOfMaterialDetailType)
        {
          case BillOfMaterialDetailType.Material:
            transactionType = Models.StaticData.StaticTransactionTypes.ImportConsumPlan;
            break;
          case BillOfMaterialDetailType.StepProduct:
            transactionType = Models.StaticData.StaticTransactionTypes.ImportProductionPlan;
            break;
          case BillOfMaterialDetailType.Waste:
            transactionType = Models.StaticData.StaticTransactionTypes.ImportWastePlan;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        #endregion
        #region GetBaseEstimatedTransaction
        var baseEstimatedTransactionPlan = App.Internals.WarehouseManagement.GetTransactionPlans(
                    selector: e => e,
                    isDelete: false,
                    isEstimated: true,
                    stuffId: bomDetail.StuffId,
                    billOfMaterialVersion: bomDetail.SemiProductBillOfMaterialVersion,
                    transactionTypeId: transactionType.Id,
                    transactionLevel: TransactionLevel.Plan,
                    baseEntityId: productionPlanDetail.Id)
                .FirstOrDefault();
        #endregion
        BaseTransaction referenceEstimatedTransaction = null;
        if (baseEstimatedTransactionPlan != null)
        {
          #region Insert ReferenceEstimatedTransaction
          referenceEstimatedTransaction = warehouseManagement.AddTransactionPlanProcess(
                  transactionBatchId: transactionBatch.Id,
                  effectDateTime: baseEstimatedTransactionPlan.EffectDateTime,
                  stuffId: bomDetail.StuffId,
                  billOfMaterialVersion: bomDetail.SemiProductBillOfMaterialVersion,
                  stuffSerialCode: null,
                  transactionTypeId: transactionType.RollbackTransactionType.Id,
                  amount: bomDetail.Value * qty.Value,
                  unitId: bomDetail.UnitId,
                  description: "",
                  isEstimated: true,
                  referenceTransaction: baseEstimatedTransactionPlan);
          #endregion
        }
        #region Insert TransactionPlan
        //deactive batch
        var effectDateTime = dateTime;
        //var batchCount = workPlanStep.BatchCount;
        //var batchTime = workPlanStep.BatchTime;
        //var rQty = qty.Value;
        //while (rQty > 0)
        //{
        //    var transactionQty = Math.Min(rQty, batchCount);
        //    var time = transactionQty * batchTime / batchCount;
        var time = duration;
        if (bomDetail.BillOfMaterialDetailType == BillOfMaterialDetailType.StepProduct ||
                      bomDetail.BillOfMaterialDetailType == BillOfMaterialDetailType.Waste)
          effectDateTime = effectDateTime.AddSeconds(time);
        var transactionPlan = warehouseManagement.AddTransactionPlanProcess(
                      transactionBatchId: transactionBatch.Id,
                      effectDateTime: effectDateTime,
                      stuffId: bomDetail.StuffId,
                      billOfMaterialVersion: bomDetail.SemiProductBillOfMaterialVersion,
                      stuffSerialCode: null,
                      transactionTypeId: transactionType.Id,
                      amount: bomDetail.Value * qty.Value,
                      //amount: bomDetail.Value * transactionQty,
                      unitId: bomDetail.UnitId,
                      description: "",
                      isEstimated: false,
                      referenceTransaction: referenceEstimatedTransaction);
        //    if (bomDetail.BillOfMaterialDetailType == BillOfMaterialDetailType.Material)
        //        effectDateTime = effectDateTime.AddSeconds(time);
        //    rQty = rQty - transactionQty;
        //}
        #endregion
      }
      #endregion
      #region Insert ProductionTransactionPlan
      {
        var effectDateTime = dateTime;
        var time = duration;
        effectDateTime = effectDateTime.AddSeconds(time);
        var productTransactionPlan = warehouseManagement.AddTransactionPlanProcess(
                      transactionBatchId: transactionBatch.Id,
                      effectDateTime: effectDateTime,
                      stuffId: workPlan.BillOfMaterialStuffId,
                      billOfMaterialVersion: workPlan.BillOfMaterialVersion,
                      stuffSerialCode: null,
                      transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportProductionPlan.Id,
                      amount: bom.Value * qty.Value,
                      unitId: bom.UnitId,
                      description: null,
                      isEstimated: false,
                      referenceTransaction: null);
        //amount: bomDetail.Value * transactionQty,
        #endregion
        #region Insert ProductionSchedule
        var scheduleQty = qty.Value * bom.Unit.ConversionRatio / productionPlanDetail.Unit.ConversionRatio;
        var productionSchedule = AddProductionSchedule(
                      productionSchedule: null,
                      transactionBatch: transactionBatch,
                      description: "",
                      qty: scheduleQty,
                      applySwitchTime: applySwitchTime,
                      productionPlanDetailId: productionPlanDetail.Id,
                      workPlanStepId: workPlanStep.Id,
                      calendarEvent: scheduleCalendarEvent,
                      operatorCount: operatorCount,
                      planningWithoutMachineLimit: planningWithoutMachineLimit,
                      switchTime: switchTime);
        #endregion
        #region AddProductionScheduleSummary
        AddProductionScheduleSummary(
                producedQty: 0,
                productionScheduleId: productionSchedule.Id);
        #endregion
        #region ResetProductionScheduleSummary
        ResetProductionPlanDetailStatus(id: productionSchedule.ProductionPlanDetailId);
        #endregion
        #endregion
        #region Get ProjectWorkItem
        var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                baseEntityId: productionPlanDetail.ProductionPlanId,
                scrumTaskType: ScrumTaskTypes.ProductionSchedule);
        #endregion
        #region CheckTask
        CheckProductionScheduleTask(productionPlanDetail: productionPlanDetail);
        #endregion
        #region Get production ProjectWork
        var projectWorks = App.Internals.ProjectManagement.GetProjectWorks(
            selector: e => e,
            departmentId: (int)Departments.Production,
            baseEntityId: productionPlanDetail.ProductionPlanId,
            isDelete: false);
        ProjectWork projectWork = null;
        if (projectWorks.Any())
          projectWork = projectWorks.FirstOrDefault();
        else
        {
          projectWork = App.Internals.ProjectManagement.AddProjectWork(
                    projectWork: null,
                    name: $"تولید {productionPlanDetail.ProductionPlan.Code}",
                    description: "",
                    color: "",
                    departmentId: (int)Departments.Production,
                    estimatedTime: 0,
                    isCommit: false,
                    projectStepId: projectWorkItem.ScrumBackLog.ScrumSprintId,
                    baseEntityId: productionPlanDetail.ProductionPlanId);
        }
        #endregion
        //check projectWork not null
        if (projectWork != null)
        {
          #region  Add ProductionOrder Task
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
                  projectWorkId: projectWork.Id,
                  baseEntityId: productionSchedule.Id);
          #endregion
        }
        return productionSchedule;
      }
    }
    #endregion
    #region CheckTask
    public void CheckProductionScheduleTask(ProductionPlanDetail productionPlanDetail)
    {
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: productionPlanDetail.ProductionPlanId,
              scrumTaskType: ScrumTaskTypes.ProductionSchedule);
      if (projectWorkItem == null)
      {
        projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                  baseEntityId: productionPlanDetail.ProductionPlanId,
                  scrumTaskType: ScrumTaskTypes.ProductionSchedule);
      }
      #endregion
      #region check ProductionSchedules and DoneProjectWorkItem
      var productionPlanDetails = GetProductionPlanDetails(
              selector: e => e,
              productionPlanId: productionPlanDetail.ProductionPlanId,
              isDelete: false);
      var taskIsDone = productionPlanDetails.All(i => i.ProductionPlanDetailSummary.ScheduledQty >= i.Qty);
      if (taskIsDone && projectWorkItem != null && projectWorkItem.ScrumTaskStep != ScrumTaskStep.Done)
      {
        #region DoneTask
        App.Internals.ScrumManagement.DoneScrumTask(
                scrumTask: projectWorkItem,
                rowVersion: projectWorkItem.RowVersion);
        #endregion
      }
      else if (taskIsDone == false && projectWorkItem != null && projectWorkItem.ScrumTaskStep == ScrumTaskStep.Done)
      {
        #region  AddNewTask
        App.Internals.ProjectManagement.AddProjectWorkItem(
                projectWorkItem: null,
                name: $"زمانبندی تولید  {productionPlanDetail.ProductionPlan.Code}",
                description: "مراحل تولید را زمانبندی نمایید ",
                color: "",
                departmentId: (int)Departments.Planning,
                estimatedTime: 10800,
                isCommit: false,
                scrumTaskTypeId: (int)ScrumTaskTypes.ProductionSchedule,
                userId: null,
                spentTime: 0,
                remainedTime: 0,
                scrumTaskStep: ScrumTaskStep.ToDo,
                projectWorkId: projectWorkItem.ScrumBackLogId,
                baseEntityId: productionPlanDetail.ProductionPlanId);
        #endregion
      }
      #endregion
    }
    public void CheckProductionScheduleTask(int productionPlanDetailId)
    {
      var productionPlanDetail = GetProductionPlanDetail(id: productionPlanDetailId); ; CheckProductionScheduleTask(productionPlanDetail: productionPlanDetail);
    }
    #endregion
    #region EditProcess
    public ProductionSchedule EditProductionScheduleProcess(
                int id,
                byte[] rowVersion,
                DateTime dateTime,
                long duration,
                double? qty,
                bool applySwitchTime,
                int operatorCount,
                int productionPlanDetailId,
                int workPlanStepId,
                bool planningWithoutMachineLimit,
                int switchTime)
    {
      #region IsDelete ProductionSchedule
      var oldProductionSchedule = DeleteProductionScheduleProcess(id, rowVersion);
      #endregion
      #region Add ProductionSchedule
      var productionSchedule = AddProductionScheduleProcess(
              productionPlanDetailId: productionPlanDetailId,
              workPlanStepId: workPlanStepId,
              dateTime: dateTime,
              duration: duration,
              qty: qty,
              applySwitchTime: applySwitchTime,
              operatorCount: operatorCount,
              planningWithoutMachineLimit: planningWithoutMachineLimit,
              switchTime: switchTime);
      #endregion
      return productionSchedule;
    }
    #endregion
    #region Delete
    public ProductionSchedule DeleteProductionScheduleProcess(int id, byte[] rowVersion)
    {
      #region IsDelete ProductionSchedule
      var productionSchedule = GetProductionSchedule(id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: null,
                    baseEntity: productionSchedule,
                    rowVersion: rowVersion);
      #endregion
      #region CheckTask
      CheckProductionScheduleTask(productionPlanDetailId: productionSchedule.ProductionPlanDetailId);
      #endregion
      return productionSchedule;
    }
    #endregion
    #region ToResult
    public Expression<Func<ProductionSchedule, ProductionScheduleResult>> ToProductionScheduleResult =
        productionSchedule => new ProductionScheduleResult
        {
          Id = productionSchedule.Id,
          Code = productionSchedule.Code,
          DateTime = productionSchedule.CalendarEvent.DateTime,
          Duration = productionSchedule.CalendarEvent.Duration,
          ProductionPlanDetailId = productionSchedule.ProductionPlanDetailId,
          ProductionPlanId = productionSchedule.ProductionPlanDetail.ProductionPlanId,
          ProductionPlanRowVersion = productionSchedule.ProductionPlanDetail.RowVersion,
          ProductionPlanCode = productionSchedule.ProductionPlanDetail.ProductionPlan.Code,
          WorkPlanStepId = productionSchedule.WorkPlanStepId,
          SemiProductStuffId = productionSchedule.ProductionPlanDetail.BillOfMaterialStuffId,
          IsPublished = productionSchedule.IsPublished,
          Status = productionSchedule.Status,
          SemiProductStuffCode = productionSchedule.ProductionPlanDetail.BillOfMaterial.Stuff.Code,
          SemiProductStuffName = productionSchedule.ProductionPlanDetail.BillOfMaterial.Stuff.Name,
          ProductionLineId = productionSchedule.WorkPlanStep.ProductionLineId,
          ProductionLineName = productionSchedule.WorkPlanStep.ProductionLine.Name,
          ProductionStepId = productionSchedule.WorkPlanStep.ProductionStepId,
          ProductionStepName = productionSchedule.WorkPlanStep.ProductionStep.Name,
          Qty = productionSchedule.Qty,
          UnitId = productionSchedule.ProductionPlanDetail.UnitId,
          UnitName = productionSchedule.ProductionPlanDetail.Unit.Name,
          ProducedQty = productionSchedule.ProductionScheduleSummary.ProducedQty,
          ProducedUnitId = productionSchedule.ProductionPlanDetail.UnitId,
          ProducedUnitName = productionSchedule.ProductionPlanDetail.Unit.Name,
          ApplySwitchTime = productionSchedule.ApplySwitchTime,
          SwitchTime = productionSchedule.SwitchTime,
          OperatorCount = productionSchedule.OperatorCount,
          PlanningWithoutMachineLimit = productionSchedule.PlanningWithoutMachineLimit,
          RowVersion = productionSchedule.RowVersion,
        };
    #endregion
    #region ToFullResult
    public Expression<Func<ProductionSchedule, ProductionScheduleFullResult>> ToProductionScheduleFullResult =
        productionSchedule => new ProductionScheduleFullResult
        {
          Id = productionSchedule.Id,
          Code = productionSchedule.Code,
          DateTime = productionSchedule.CalendarEvent.DateTime,
          Duration = productionSchedule.CalendarEvent.Duration,
          TransactionBatchId = productionSchedule.TransactionBatch.Id,
          ProductionPlanDetailId = productionSchedule.ProductionPlanDetailId,
          ProductionPlanId = productionSchedule.ProductionPlanDetail.ProductionPlanId,
          ProductionPlanCode = productionSchedule.ProductionPlanDetail.ProductionPlan.Code,
          ProductionRequestId = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequestId,
          ProductionRequestCode = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.Code,
          OrderItemId = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItemId,
          OrderItemCode = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Code,
          RequestDate = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.RequestDate,
          DeliveryDate = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.DeliveryDate,
          OrderId = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.OrderId,
          CustomerCode = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Order.Customer.Code,
          CustomerName = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Order.Customer.Name,
          ProductStuffId = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.StuffId,
          ProductStuffCode = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Stuff.Code,
          ProductStuffName = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Stuff.Name,
          ProductBillOfMaterialVersion = productionSchedule.ProductionPlanDetail.ProductionPlan.BillOfMaterialVersion,
          ProductStuffNoun = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Stuff.Noun,
          StuffId = productionSchedule.ProductionPlanDetail.BillOfMaterialStuffId,
          StuffCode = productionSchedule.ProductionPlanDetail.BillOfMaterial.Stuff.Code,
          StuffName = productionSchedule.ProductionPlanDetail.BillOfMaterial.Stuff.Name,
          BillOfMaterialVersion = productionSchedule.ProductionPlanDetail.BillOfMaterialVersion,
          WorkPlanId = productionSchedule.WorkPlanStep.WorkPlanId,
          WorkPlanStepId = productionSchedule.WorkPlanStepId,
          ProductionLineId = productionSchedule.WorkPlanStep.ProductionLineId,
          ProductionStepId = productionSchedule.WorkPlanStep.ProductionStepId,
          OrderItemQty = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Qty,
          OrderItemUnitId = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.UnitId,
          OrderItemUnitName = productionSchedule.ProductionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Unit.Name,
          Qty = productionSchedule.Qty,
          ApplySwitchTime = productionSchedule.ApplySwitchTime,
          SwitchTime = productionSchedule.SwitchTime,
          UnitId = productionSchedule.ProductionPlanDetail.UnitId,
          UnitName = productionSchedule.ProductionPlanDetail.Unit.Name,
          RowVersion = productionSchedule.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<ProductionScheduleResult> SearchProductionScheduleResultQuery(
        IQueryable<ProductionScheduleResult> query,
        DateTime? fromDate,
        DateTime? toDate)
    {
      if (fromDate != null && toDate != null)
        query = query.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        query = query.Where(i => i.DateTime <= toDate);
      return query;
    }
    #endregion
    #region ToProductionLineProductionSchedulesResult
    public IQueryable<ProductionLineProductionSchedulesResult> ToProductionLineProductionSchedulesResultQuery(
        IQueryable<ProductionLineWorkTimeResult> productionLineWorkTimeResultQuery,
        IQueryable<ProductionScheduleResult> productionScheduleResultQuery,
        IQueryable<ProductionLineResult> productionLineResultQuery)
    {
      #region Group
      var gwswt = from item in productionLineWorkTimeResultQuery
                  group item by item.ProductionLineId
                  into gItems
                  select new
                  {
                    ProductionLineId = gItems.Key,
                    ProductionLineWorkTimes = gItems
                  };
      var gwsps = from item in productionScheduleResultQuery
                  group item by item.ProductionLineId
          into gItems
                  select new
                  {
                    ProductionLineId = gItems.Key,
                    ProductionSchedules = gItems
                  };
      #endregion
      #region CreateResult
      var result = from productionline in productionLineResultQuery
                   join twswt in gwswt on productionline.Id equals twswt.ProductionLineId into wswts
                   from wswt in wswts.DefaultIfEmpty()
                   join twsps in gwsps on productionline.Id equals twsps.ProductionLineId into wspss
                   from wsps in wspss.DefaultIfEmpty()
                   select new ProductionLineProductionSchedulesResult
                   {
                     SortIndex = productionline.SortIndex,
                     ProductionLineId = productionline.Id,
                     ProductionLineName = productionline.Name,
                     ProductionLineWorkTimes = wswt.ProductionLineWorkTimes.AsQueryable(),
                     ProductionSchedules = wsps.ProductionSchedules.AsQueryable(),
                   };
      #endregion
      return result;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ProductionScheduleResult> SortProductionScheduleResult(
        IQueryable<ProductionScheduleResult> query,
        SortInput<ProductionScheduleSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case ProductionScheduleSortType.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case ProductionScheduleSortType.Code:
          return query.OrderBy(r => r.Code, sortInput.SortOrder);
        case ProductionScheduleSortType.DateTime:
          return query.OrderBy(r => r.DateTime, sortInput.SortOrder);
        case ProductionScheduleSortType.Duration:
          return query.OrderBy(r => r.Duration, sortInput.SortOrder);
        case ProductionScheduleSortType.Qty:
          return query.OrderBy(r => r.Qty, sortInput.SortOrder);
        case ProductionScheduleSortType.UnitName:
          return query.OrderBy(r => r.UnitName, sortInput.SortOrder);
        case ProductionScheduleSortType.ProducedQty:
          return query.OrderBy(r => r.ProducedQty, sortInput.SortOrder);
        case ProductionScheduleSortType.ProducedUnitName:
          return query.OrderBy(r => r.ProducedUnitName, sortInput.SortOrder);
        case ProductionScheduleSortType.SemiProductStuffCode:
          return query.OrderBy(r => r.SemiProductStuffCode, sortInput.SortOrder);
        case ProductionScheduleSortType.SemiProductStuffName:
          return query.OrderBy(r => r.SemiProductStuffName, sortInput.SortOrder);
        case ProductionScheduleSortType.ProductionStepName:
          return query.OrderBy(r => r.ProductionStepName, sortInput.SortOrder);
        case ProductionScheduleSortType.ProductionLineName:
          return query.OrderBy(r => r.ProductionLineName, sortInput.SortOrder);
        case ProductionScheduleSortType.IsPublished:
          return query.OrderBy(r => r.IsPublished, sortInput.SortOrder);
        case ProductionScheduleSortType.Status:
          return query.OrderBy(r => r.Status, sortInput.SortOrder);
        case ProductionScheduleSortType.ToDateTime:
          return query.OrderBy(r => new { r.DateTime, time = r.Duration }, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException(nameof(sortInput.SortOrder), sortInput.SortType, null);
      }
    }
    #endregion
    #region SearchQuery
    public IQueryable<ProductionScheduleResult> SearchProductionSchedule(
        IQueryable<ProductionScheduleResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Code.Contains(searchText) ||
                item.UnitName.Contains(searchText) ||
                item.Qty.ToString().Contains(searchText) ||
                item.ProductionStepName.Contains(searchText) ||
                item.SemiProductStuffCode.Contains(searchText) ||
                item.SemiProductStuffName.Contains(searchText)
                select item;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region ResetStatus
    public ProductionSchedule ResetProductionScheduleStatus(int id)
    {
      #region GetProductionSchedule
      var productionSchedule = GetProductionSchedule(id: id);
      #endregion
      return ResetProductionScheduleStatus(productionSchedule: productionSchedule);
    }
    public ProductionSchedule ResetProductionScheduleStatus(ProductionSchedule productionSchedule)
    {
      #region ResetProductionScheduleSummary
      var productionScheduleSummary = ResetProductionScheduleSummaryByProductionScheduleId(productionScheduleId: productionSchedule.Id);
      #endregion
      #region Define Status
      var status = ProductionScheduleStatus.None;
      if (productionSchedule.Status.HasFlag(ProductionScheduleStatus.Finished))
        status = status | ProductionScheduleStatus.Finished;
      if (productionScheduleSummary.ProducedQty >= 0)
      {
        if (productionScheduleSummary.ProducedQty >= productionScheduleSummary.ProductionSchedule.Qty)
          status = status | ProductionScheduleStatus.Produced;
        else
          status = status | ProductionScheduleStatus.InProduction;
      }
      if (status == ProductionScheduleStatus.None)
        status = ProductionScheduleStatus.NotAction;
      #endregion
      #region Edit ProductionRequest
      if (status != productionSchedule.Status)
        EditProductionSchedule(
                      productionSchedule: productionSchedule,
                      rowVersion: productionSchedule.RowVersion,
                      status: status);
      #endregion
      return productionSchedule;
    }
    #endregion
    #region FinishProcess
    public ProductionSchedule FinishProductionScheduleProcess(
        int id,
        byte[] rowVersion)
    {
      #region GetProductionSchedule
      var productionSchedule = GetProductionSchedule(id: id);
      #endregion
      //#region MyRegion
      //#endregion
      #region Edit ProductionRequest Set Finish Status
      var status = productionSchedule.Status | ProductionScheduleStatus.Finished;
      if (status != productionSchedule.Status)
        EditProductionSchedule(
                      productionSchedule: productionSchedule,
                      rowVersion: productionSchedule.RowVersion,
                      status: status);
      #endregion
      return productionSchedule;
    }
    #endregion
  }
}
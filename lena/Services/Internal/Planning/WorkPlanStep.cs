using System;
using System.Linq;
using System.Linq.Expressions;
////using Microsoft.Ajax.Utilities;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Planning.OperationSequence;
using lena.Models.Planning.WorkPlanStep;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public IQueryable<TResult> GetWorkPlanSteps<TResult>(
        Expression<Func<WorkPlanStep, TResult>> selector,
        TValue<int> id = null,
        TValue<string> title = null,
        TValue<long> initialTime = null,
        TValue<long> switchTime = null,
        TValue<long> batchTime = null,
        TValue<double> batchCount = null,
        TValue<int> workPlanId = null,
        TValue<int> productionStepId = null,
        TValue<int> productionLineId = null,
        TValue<string> description = null,
        TValue<bool> isActive = null,
        TValue<int[]> workPlanIds = null)
    {

      var workPlanSteps = repository.GetQuery<WorkPlanStep>();
      if (id != null)
        workPlanSteps = workPlanSteps.Where(i => i.Id == id);
      if (title != null)
        workPlanSteps = workPlanSteps.Where(i => i.Title == title);
      if (initialTime != null)
        workPlanSteps = workPlanSteps.Where(i => i.InitialTime == initialTime);
      if (switchTime != null)
        workPlanSteps = workPlanSteps.Where(i => i.SwitchTime == switchTime);
      if (batchTime != null)
        workPlanSteps = workPlanSteps.Where(i => i.BatchTime == batchTime);
      if (batchCount != null)
        workPlanSteps = workPlanSteps.Where(i => i.BatchCount == batchCount);
      if (workPlanId != null)
        workPlanSteps = workPlanSteps.Where(i => i.WorkPlanId == workPlanId);
      if (productionStepId != null)
        workPlanSteps = workPlanSteps.Where(i => i.ProductionStepId == productionStepId);
      if (productionLineId != null)
        workPlanSteps = workPlanSteps.Where(i => i.ProductionLineId == productionLineId);
      if (description != null)
        workPlanSteps = workPlanSteps.Where(i => i.Description == description);
      if (isActive != null)
        workPlanSteps = workPlanSteps.Where(i => i.IsActive == isActive);
      if (workPlanIds != null)
        workPlanSteps = workPlanSteps.Where(i => workPlanIds.Value.Contains(i.WorkPlanId));

      return workPlanSteps.Select(selector: selector);
    }

    public IQueryable<WorkPlanStepResult> GetWorkPlanStepsProcess(
        int productionStepId,
        int billOfMaterialVersion,
        int billOfMaterialStuffId)
    {

      var workPlanSteps = GetWorkPlanSteps(
                selector: e => e,
                productionStepId: productionStepId);

      var workPlans = App.Internals.Planning.GetWorkPlans(
                    billOfMaterialVersion: billOfMaterialVersion,
                    billOfMaterialStuffId: billOfMaterialStuffId);

      var workPlanStepResults = from workPlanStep in workPlanSteps
                                join workPlan in workPlans on workPlanStep.WorkPlanId equals workPlan.Id
                                select new WorkPlanStepResult
                                {
                                  Id = workPlanStep.Id,
                                  Title = workPlanStep.Title,
                                  InitialTime = workPlanStep.InitialTime,
                                  SwitchTime = workPlanStep.SwitchTime,
                                  BatchTime = workPlanStep.BatchTime,
                                  BatchCount = workPlanStep.BatchCount,
                                  UnitId = workPlanStep.WorkPlan.BillOfMaterial.UnitId,
                                  UnitName = workPlanStep.WorkPlan.BillOfMaterial.Unit.Name,
                                  ConversionRatio = workPlanStep.WorkPlan.BillOfMaterial.Unit.ConversionRatio,
                                  WorkPlanId = workPlanStep.WorkPlanId,
                                  WorkPlanBillOfMaterialStuffId = workPlanStep.WorkPlan.BillOfMaterialStuffId,
                                  WorkPlanBillOfMaterialVersion = workPlanStep.WorkPlan.BillOfMaterialVersion,
                                  WorkPlanTitle = workPlanStep.WorkPlan.Title,
                                  ProductionStepId = workPlanStep.WorkPlan.BillOfMaterial.ProductionStepId,
                                  ProductionStepName = workPlanStep.ProductionStep.Name,
                                  ProductionLineId = workPlanStep.ProductionLineId,
                                  ProductionLineName = workPlanStep.ProductionLine.Name,
                                  Description = workPlanStep.Description,
                                  IsActive = workPlanStep.IsActive,
                                  WorkPlanIsPublished = workPlanStep.WorkPlan.IsPublished,
                                  NeedToQualityControl = workPlanStep.NeedToQualityControl,
                                  PlanningWithoutMachineLimit = workPlanStep.PlanningWithoutMachineLimit,
                                  SumOfOperationSequenceTime = workPlanStep.OperationSequences.Sum(s => s.DefaultTime),
                                  RowVersion = workPlanStep.RowVersion
                                };

      return workPlanStepResults
                .Distinct()
                .OrderBy(x => x.Id)
                .AsQueryable();

    }

    public WorkPlanStep GetWorkPlanStep(int id) => GetWorkPlanStep(selector: e => e, id: id);
    public TResult GetWorkPlanStep<TResult>(
         Expression<Func<WorkPlanStep, TResult>> selector,
         int id)
    {

      var workPlanStep = GetWorkPlanSteps(
                selector: selector,
                id: id)


                .FirstOrDefault();
      if (workPlanStep == null)
        throw new WorkPlanStepNotFoundException(id);
      return workPlanStep;
    }
    public WorkPlanStep AddWorkPlanStep(
        string title,
        long initialTime,
        long switchTime,
        long batchTime,
        double batchCount,
        int workPlanId,
        int productionStepId,
        int productionLineId,
        string description,
        bool isActive,
        bool needToQualityControl,
        bool planningWithoutMachineLimit,
        short productWarehouseId,
        short consumeWarehouseId)
    {

      var workPlanStep = repository.Create<WorkPlanStep>();
      workPlanStep.Title = title;
      workPlanStep.InitialTime = initialTime;
      workPlanStep.SwitchTime = switchTime;
      workPlanStep.BatchTime = batchTime;
      workPlanStep.BatchCount = batchCount;
      workPlanStep.WorkPlanId = workPlanId;
      workPlanStep.ProductionStepId = productionStepId;
      workPlanStep.ProductionLineId = productionLineId;
      workPlanStep.Description = description;
      workPlanStep.IsActive = isActive;
      workPlanStep.NeedToQualityControl = needToQualityControl;
      workPlanStep.PlanningWithoutMachineLimit = planningWithoutMachineLimit;
      workPlanStep.ProductWarehouseId = productWarehouseId;
      workPlanStep.ConsumeWarehouseId = consumeWarehouseId;
      repository.Add(workPlanStep);
      return workPlanStep;
    }
    public WorkPlanStep EditWorkPlanStep(
        byte[] rowVersion,
        int id,
        TValue<string> title = null,
        TValue<long> initialTime = null,
        TValue<long> switchTime = null,
        TValue<long> batchTime = null,
        TValue<double> batchCount = null,
        TValue<int> workPlanId = null,
        TValue<int> productionStepId = null,
        TValue<int> productionLineId = null,
        TValue<string> description = null,
        TValue<bool> isActive = null,
        TValue<bool> needToQualityControl = null,
        TValue<bool> planningWithoutMachineLimit = null,
        TValue<short> productWarehouseId = null,
        TValue<short> consumeWarehouseId = null)
    {

      var workPlanStep = GetWorkPlanStep(id: id);
      if (title != null)
        workPlanStep.Title = title;
      if (switchTime != null)
        workPlanStep.SwitchTime = switchTime;
      if (initialTime != null)
        workPlanStep.InitialTime = initialTime;
      if (batchTime != null)
        workPlanStep.BatchTime = batchTime;
      if (batchCount != null)
        workPlanStep.BatchCount = batchCount;
      if (workPlanId != null)
        workPlanStep.WorkPlanId = workPlanId;

      if (productionStepId != null)
        workPlanStep.ProductionStepId = productionStepId;
      if (productionLineId != null)
        workPlanStep.ProductionLineId = productionLineId;
      if (description != null)
        workPlanStep.Description = description;
      if (isActive != null)
        workPlanStep.IsActive = isActive;
      if (needToQualityControl != null)
        workPlanStep.NeedToQualityControl = needToQualityControl;
      if (planningWithoutMachineLimit != null)
        workPlanStep.PlanningWithoutMachineLimit = planningWithoutMachineLimit;
      if (productWarehouseId != null)
        workPlanStep.ProductWarehouseId = productWarehouseId;
      if (consumeWarehouseId != null)
        workPlanStep.ConsumeWarehouseId = consumeWarehouseId;
      repository.Update(rowVersion: rowVersion, entity: workPlanStep);
      return workPlanStep;
    }
    public void DeleteWorkPlanStepProcess(int id)
    {

      var workPlanStep = GetWorkPlanStep(id: id);
      foreach (var operationSequence in workPlanStep.OperationSequences.ToList())
        DeleteOperationSequenceProcess(operationSequence.Id);
      repository.Delete(workPlanStep);
    }
    public WorkPlanStep AddWorkPlanStepProcess(
        string title,
        long initialTime,
        long switchTime,
        long batchTime,
        double batchCount,
        int workPlanId,
        int productionStepId,
        int productionLineId,
        string description,
        bool isActive,
        bool needToQualityControl,
        bool planningWithoutMachineLimit,
        short productWarehouseId,
        short consumeWarehouseId,
        AddOperationSequenceInput[] addOperationSequenceInputs)
    {

      var workPlanStep = AddWorkPlanStep(
                    title: title,
                    initialTime: initialTime,
                    switchTime: switchTime,
                    batchTime: batchTime,
                    batchCount: batchCount,
                    workPlanId: workPlanId,
                    productionStepId: productionStepId,
                    productionLineId: productionLineId,
                    description: description,
                    isActive: isActive,
                    needToQualityControl: needToQualityControl,
                    planningWithoutMachineLimit: planningWithoutMachineLimit,
                    productWarehouseId: productWarehouseId,
                    consumeWarehouseId: consumeWarehouseId);
      foreach (var addOperationSequenceInput in addOperationSequenceInputs)
      {
        AddOperationSequenceProcess(
                      workStationId: addOperationSequenceInput.WorkStationId,
                      index: addOperationSequenceInput.Index,
                      isOptional: addOperationSequenceInput.IsOptional,
                      isRepairReturnPoint: addOperationSequenceInput.IsRepairReturnPoint,
                      operationId: addOperationSequenceInput.WorkStationOperationId,
                      defaultTime: addOperationSequenceInput.DefaultTime,
                      workPlanStepId: workPlanStep.Id,
                      workStationPartId: addOperationSequenceInput.WorkStationPartId,
                      description: addOperationSequenceInput.Description,
                      addOperationConsumingMaterialInputs: addOperationSequenceInput
                          .OperationConsumingMaterialInputs,
                      addOperationSequenceMachineTypeParameterInputs: addOperationSequenceInput
                          .OperationSequenceMachineTypeParameterInputs,
                      workStationPartCount: addOperationSequenceInput.WorkStationPartCount);
      }
      return workPlanStep;
    }
    public WorkPlanStep EditWorkPlanStepProcess(
        byte[] rowVersion,
        int id,
        TValue<string> title = null,
        TValue<long> initialTime = null,
        TValue<long> switchTime = null,
        TValue<long> batchTime = null,
        TValue<double> batchCount = null,
        TValue<int> workPlanId = null,
        TValue<int> productionStepId = null,
        TValue<int> productionLineId = null,
        TValue<string> description = null,
        TValue<bool> isActive = null,
        TValue<bool> needToQualityControl = null,
        TValue<bool> planningWithoutMachineLimit = null,
        TValue<short> productWarehouseId = null,
        TValue<short> consumeWarehouseId = null,
        AddOperationSequenceInput[] addOperationSequenceInputs = null,
        EditOperationSequenceInput[] editOperationSequenceInputs = null,
        int[] deleteOperationSequenceInputs = null)
    {

      var workPlanStep = EditWorkPlanStep(
                    rowVersion: rowVersion,
                    id: id,
                    title: title,
                    initialTime: initialTime,
                    switchTime: switchTime,
                    batchTime: batchTime,
                    batchCount: batchCount,
                    workPlanId: workPlanId,
                    productionStepId: productionStepId,
                    productionLineId: productionLineId,
                    description: description,
                    isActive: isActive,
                    needToQualityControl: needToQualityControl,
                    planningWithoutMachineLimit: planningWithoutMachineLimit,
                    productWarehouseId: productWarehouseId,
                    consumeWarehouseId: consumeWarehouseId);
      if (addOperationSequenceInputs != null)
        foreach (var addOperationSequenceInput in addOperationSequenceInputs)
        {
          AddOperationSequenceProcess(
                        workStationId: addOperationSequenceInput.WorkStationId,
                        index: addOperationSequenceInput.Index,
                        isOptional: addOperationSequenceInput.IsOptional,
                        isRepairReturnPoint: addOperationSequenceInput.IsRepairReturnPoint,
                        operationId: addOperationSequenceInput.WorkStationOperationId,
                        defaultTime: addOperationSequenceInput.DefaultTime,
                        workPlanStepId: workPlanStep.Id,
                        workStationPartId: addOperationSequenceInput.WorkStationPartId,
                        description: addOperationSequenceInput.Description,
                        addOperationConsumingMaterialInputs: addOperationSequenceInput
                            .OperationConsumingMaterialInputs,
                        addOperationSequenceMachineTypeParameterInputs: addOperationSequenceInput
                            .OperationSequenceMachineTypeParameterInputs,
                        workStationPartCount: addOperationSequenceInput.WorkStationPartCount);
        }
      if (editOperationSequenceInputs != null)
        foreach (var editOperationSequenceInput in editOperationSequenceInputs)
        {
          EditOperationSequenceProcess(
                        rowVersion: editOperationSequenceInput.RowVersion,
                        id: editOperationSequenceInput.Id,
                        workStationId: editOperationSequenceInput.WorkStationId,
                        index: editOperationSequenceInput.Index,
                        isOptional: editOperationSequenceInput.IsOptional,
                        isRepairReturnPoint: editOperationSequenceInput.IsRepairReturnPoint,
                        operationId: editOperationSequenceInput.WorkStationOperationId,
                        defaultTime: editOperationSequenceInput.DefaultTime,
                        workPlanStepId: workPlanStep.Id,
                        workStationPartId: editOperationSequenceInput.WorkStationPartId,
                        description: editOperationSequenceInput.Description,
                        workStationPartCount: editOperationSequenceInput.WorkStationPartCount,
                        addOperationConsumingMaterialInputs: editOperationSequenceInput
                            .AddOperationConsumingMaterialInputs,
                        editOperationConsumingMaterialInputs: editOperationSequenceInput
                            .EditOperationConsumingMaterialInputs,
                        deleteOperationConsumingMaterialInputs: editOperationSequenceInput
                            .DeleteOperationConsumingMaterialInputs,
                        addOperationSequenceMachineTypeParameterInputs: editOperationSequenceInput
                            .AddOperationSequenceMachineTypeParameterInputs);
        }
      if (deleteOperationSequenceInputs != null)
        foreach (var deleteOperationSequenceInput in deleteOperationSequenceInputs)
        {
          DeleteOperationSequenceProcess(id: deleteOperationSequenceInput);

        }
      return workPlanStep;
    }
    public void ActiveWorkPlanStep(byte[] rowVersion, int id)
    {

      EditWorkPlanStep(rowVersion: rowVersion, id: id, isActive: true);
    }
    public void DeactiveWorkPlanStep(byte[] rowVersion, int id)
    {

      EditWorkPlanStep(rowVersion: rowVersion, id: id, isActive: false);
    }
    public WorkPlanStepResult ToWorkPlanStepResult(WorkPlanStep workPlanStep)
    {
      var productionLine = workPlanStep.ProductionLine;
      var productionStep = workPlanStep.ProductionStep;
      return new WorkPlanStepResult
      {
        Id = workPlanStep.Id,
        Title = workPlanStep.Title,
        InitialTime = workPlanStep.InitialTime,
        SwitchTime = workPlanStep.SwitchTime,
        BatchTime = workPlanStep.BatchTime,
        BatchCount = workPlanStep.BatchCount,
        UnitId = workPlanStep.WorkPlan.BillOfMaterial.UnitId,
        UnitName = workPlanStep.WorkPlan.BillOfMaterial.Unit.Name,
        ConversionRatio = workPlanStep.WorkPlan.BillOfMaterial.Unit.ConversionRatio,
        WorkPlanId = workPlanStep.WorkPlanId,
        WorkPlanBillOfMaterialStuffId = workPlanStep.WorkPlan.BillOfMaterialStuffId,
        WorkPlanBillOfMaterialVersion = workPlanStep.WorkPlan.BillOfMaterialVersion,
        WorkPlanTitle = workPlanStep.WorkPlan.Title,
        ProductionStepId = workPlanStep.ProductionStepId,
        ProductionStepName = productionStep.Name,
        ProductionLineId = workPlanStep.ProductionLineId,
        ProductionLineName = productionLine.Name,
        Description = workPlanStep.Description,
        IsActive = workPlanStep.IsActive,
        WorkPlanIsPublished = workPlanStep.WorkPlanIsPublished,
        NeedToQualityControl = workPlanStep.NeedToQualityControl,
        PlanningWithoutMachineLimit = workPlanStep.PlanningWithoutMachineLimit,
        SumOfOperationSequenceTime = workPlanStep.OperationSequences.Sum(s => s.DefaultTime),
        RowVersion = workPlanStep.RowVersion,
      };
    }
    public FullWorkPlanStepResult ToFullWorkPlanStepResult(WorkPlanStep workPlanStep)
    {
      return new FullWorkPlanStepResult
      {
        Id = workPlanStep.Id,
        Title = workPlanStep.Title,
        InitialTime = workPlanStep.InitialTime,
        SwitchTime = workPlanStep.SwitchTime,
        BatchTime = workPlanStep.BatchTime,
        BatchCount = workPlanStep.BatchCount,
        WorkPlanId = workPlanStep.WorkPlanId,
        ProductionStepId = workPlanStep.ProductionStepId,
        ProductinoStepName = workPlanStep.ProductionStep.Name,
        ProductionLineId = workPlanStep.ProductionLineId,
        ProductionLineName = workPlanStep.ProductionLine.Name,
        Description = workPlanStep.Description,
        IsActive = workPlanStep.IsActive,
        NeedToQualityControl = workPlanStep.NeedToQualityControl,
        PlanningWithoutMachineLimit = workPlanStep.PlanningWithoutMachineLimit,
        OperationSequences = workPlanStep.OperationSequences.ToList().Select(ToFullOperationSequenceResult).ToArray(),
        WorkStations = workPlanStep.ProductionStep.ProductionLineProductionSteps.SelectMany(i => i.ProductionLine.WorkStations).ToList().Select(ToWorkStationResult).ToArray(),
        WorkStationParts = workPlanStep.ProductionLine.WorkStations.SelectMany(i => i.WorkStationParts).ToList().Select(ToWorkStationPartComboResult).ToArray(),
        WorkStationOperations = workPlanStep.ProductionLine.WorkStations.SelectMany(i => i.WorkStationOperations).ToList().Select(ToWorkStationOperationResult).ToArray(),
        ProductWarehouseId = workPlanStep.ProductWarehouseId,
        ConsumeWarehouseId = workPlanStep.ConsumeWarehouseId,
        RowVersion = workPlanStep.RowVersion,
      };
    }
    public IQueryable<WorkPlanStepResult> ToWorkPlanStepResultQuery(IQueryable<WorkPlanStep> query)
    {
      return from workPlanStep in query
             let productionLine = workPlanStep.ProductionLine
             let productionStep = workPlanStep.ProductionStep
             select new WorkPlanStepResult
             {
               Id = workPlanStep.Id,
               Title = workPlanStep.Title,
               InitialTime = workPlanStep.InitialTime,
               SwitchTime = workPlanStep.SwitchTime,
               BatchTime = workPlanStep.BatchTime,
               BatchCount = workPlanStep.BatchCount,
               UnitId = workPlanStep.WorkPlan.BillOfMaterial.UnitId,
               UnitName = workPlanStep.WorkPlan.BillOfMaterial.Unit.Name,
               ConversionRatio = workPlanStep.WorkPlan.BillOfMaterial.Unit.ConversionRatio,
               WorkPlanId = workPlanStep.WorkPlanId,
               WorkPlanBillOfMaterialStuffId = workPlanStep.WorkPlan.BillOfMaterialStuffId,
               WorkPlanBillOfMaterialVersion = workPlanStep.WorkPlan.BillOfMaterialVersion,
               WorkPlanTitle = workPlanStep.WorkPlan.Title,
               ProductionStepId = workPlanStep.WorkPlan.BillOfMaterial.ProductionStepId,
               ProductionStepName = productionStep.Name,
               ProductionLineId = workPlanStep.ProductionLineId,
               ProductionLineName = productionLine.Name,
               Description = workPlanStep.Description,
               IsActive = workPlanStep.IsActive,
               WorkPlanIsPublished = workPlanStep.WorkPlanIsPublished,
               NeedToQualityControl = workPlanStep.NeedToQualityControl,
               PlanningWithoutMachineLimit = workPlanStep.PlanningWithoutMachineLimit,
               SumOfOperationSequenceTime = workPlanStep.OperationSequences.Sum(s => s.DefaultTime),
               RowVersion = workPlanStep.RowVersion
             };
    }
    public IQueryable<WorkPlanStepResult> SearchWorkPlanStepResult(IQueryable<WorkPlanStepResult> query, TValue<string> search)
    {
      if (search == null) return query;
      return from workPlanStep in query
             where
             workPlanStep.Title.Contains(search) ||
             workPlanStep.Description.Contains(search)
             select workPlanStep;
    }
    public IOrderedQueryable<WorkPlanStepResult> SortWorkPlanStepResult(IQueryable<WorkPlanStepResult> query, SortInput<WorkPlanStepSortType> sort)
    {
      switch (sort.SortType)
      {
        case WorkPlanStepSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case WorkPlanStepSortType.ProductionStepId:
          return query.OrderBy(a => a.ProductionStepId, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}

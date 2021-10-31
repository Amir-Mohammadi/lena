using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.OperationConsumingMaterial;
using lena.Models.Planning.OperationSequence;
using lena.Models.Production.ProductionOperationSequence;
using lena.Services.Core;
using lena.Models.Planning.Machine;
using lena.Models.Planning.OperationSequenceMachineTypeParameter;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Gets

    public IQueryable<TResult> GetOperationSequences<TResult>(
        Expression<Func<OperationSequence, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<int> workStationId = null,
        TValue<int> index = null,
        TValue<int> operationId = null,
        TValue<float> defaultTime = null,
        TValue<int> workPlanStepId = null,
        TValue<int> workStationPartId = null,
        TValue<string> description = null,
        TValue<int> productionOrderId = null,
        TValue<string> serial = null)
    {


      var query = repository.GetQuery<OperationSequence>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (workStationId != null)
        query = query.Where(i => i.WorkStationId == workStationId);
      if (index != null)
        query = query.Where(i => i.Index == index);
      if (operationId != null)
        query = query.Where(i => i.OperationId == operationId);
      if (defaultTime != null)
        query = query.Where(i => i.DefaultTime == defaultTime);
      if (workPlanStepId != null)
        query = query.Where(i => i.WorkPlanStepId == workPlanStepId);
      if (workStationPartId != null)
        query = query.Where(i => i.WorkStationPartId == workStationPartId);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (ids != null)
        query = query.Where(i => ids.Value.Contains(i.Id));
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial: serial);
        if (serial != null)
        {
          var productions = App.Internals.Production.GetProductions(selector: e => e,
                        serial: serial);
          query = from item in query
                  join production in productions on item.WorkPlanStepId equals production.ProductionOrder
                            .WorkPlanStepId
                  select item;
        }
      }

      return query.Select(selector);
    }
    #endregion
    #region GetProductionOperationSequences
    public IQueryable<ProductionOperationSequenceResult> GetProductionOperationSequences(
        int workPlanStepId,
        int? productionTerminalId,
        string productionOrderCode)
    {


      #region Get OperationSequences
      var operationSequences = GetOperationSequences(
              selector: e => e,
              workPlanStepId: workPlanStepId);
      #endregion
      #region operationStatics                
      var productionOperations = App.Internals.Production.GetProductionOperations(
      selector: e => e,
      productionOrderCode: productionOrderCode,
      productionTerminalId: productionTerminalId);

      var operationStatics = (from p in productionOperations
                              group p by p.OperationId into g
                              select new
                              {
                                OperationId = g.Key,
                                OperationCount = g.Count()
                              }
                    );
      #endregion
      var machineProductionOperationSequences = from operationSequence in operationSequences
                                                where operationSequence.WorkStationPart is MachineType
                                                //join tStatic in operationStatics on operationSequence.OperationId equals tStatic.OperationId into tempStatics
                                                //from staticItem in tempStatics.DefaultIfEmpty()
                                                let machineType = operationSequence.WorkStationPart as MachineType
                                                let workStation = operationSequence.WorkStation
                                                from machineTypeOperatorType in machineType.MachineTypeOperatorTypes.DefaultIfEmpty()

                                                select new ProductionOperationSequenceResult()
                                                {
                                                  Id = operationSequence.Id,
                                                  Index = operationSequence.Index,
                                                  IsOptional = operationSequence.IsOptional,
                                                  WorkStationId = operationSequence.WorkStationId,
                                                  WorkStationName = workStation.Name,
                                                  MachineTypeId = operationSequence.WorkStationPartId,
                                                  MachineTypeName = machineType.Name,
                                                  OperationId = operationSequence.OperationId,
                                                  BatchCount = operationSequence.WorkPlanStep.BatchCount,
                                                  SwitchTime = operationSequence.WorkPlanStep.SwitchTime,
                                                  OperationTitle = operationSequence.Operation.Title,
                                                  WorkPlanStepId = workPlanStepId,
                                                  WorkStationPartId = operationSequence.WorkStationPartId,
                                                  WorkStationPartCount = operationSequence.WorkStationPartCount,
                                                  OperatorTypeId = machineTypeOperatorType.OperatorTypeId,
                                                  OperatorTypeName = machineTypeOperatorType.OperatorType.Name,
                                                  MachineTypeOperatorTypeId = (int?)machineTypeOperatorType.Id,
                                                  MachineTypeOperatorTypeTitle = machineTypeOperatorType.Title,
                                                  DefaultTime = operationSequence.DefaultTime,
                                                  OperationCount = 0,//((int?)staticItem.OperationCount) ?? 0,
                                                  RowVersion = operationSequence.RowVersion,
                                                };


      var operatorProductionOperationSequences = from operationSequence in operationSequences
                                                 where operationSequence.WorkStationPart is OperatorType
                                                 //join tStatic in operationStatics on operationSequence.OperationId equals tStatic.OperationId into tempStatics
                                                 //from staticItem in tempStatics.DefaultIfEmpty()
                                                 let operatorType = operationSequence.WorkStationPart as OperatorType
                                                 let workStation = operationSequence.WorkStation
                                                 select new ProductionOperationSequenceResult()
                                                 {
                                                   Id = operationSequence.Id,
                                                   Index = operationSequence.Index,
                                                   IsOptional = operationSequence.IsOptional,
                                                   WorkStationId = operationSequence.WorkStationId,
                                                   WorkStationName = workStation.Name,
                                                   MachineTypeId = null,
                                                   MachineTypeName = null,
                                                   OperationId = operationSequence.OperationId,
                                                   BatchCount = operationSequence.WorkPlanStep.BatchCount,
                                                   SwitchTime = operationSequence.WorkPlanStep.SwitchTime,
                                                   OperationTitle = operationSequence.Operation.Title,
                                                   WorkPlanStepId = workPlanStepId,
                                                   WorkStationPartId = operationSequence.WorkStationPartId,
                                                   WorkStationPartCount = operationSequence.WorkStationPartCount,
                                                   OperatorTypeId = operatorType.Id,
                                                   OperatorTypeName = operatorType.Name,
                                                   MachineTypeOperatorTypeId = null,
                                                   MachineTypeOperatorTypeTitle = "",
                                                   DefaultTime = operationSequence.DefaultTime,
                                                   OperationCount = 0,// ((int?)staticItem.OperationCount) ?? 0,
                                                   RowVersion = operationSequence.RowVersion,
                                                 };


      var productionOperationSequences = operatorProductionOperationSequences
                .Union(machineProductionOperationSequences);
      return productionOperationSequences.OrderBy(i => i.Index);
    }

    #endregion

    #region Get

    public OperationSequence GetOperationSequence(int id) => GetOperationSequence(selector: e => e, id: id);
    public TResult GetOperationSequence<TResult>(
        Expression<Func<OperationSequence, TResult>> selector,
        int id)
    {

      var operationSequence = GetOperationSequences(selector: selector, id: id)


                .FirstOrDefault();
      if (operationSequence == null)
        throw new OperationSequenceNotFoundException(id);
      return operationSequence;
    }
    #endregion
    #region Add
    public OperationSequence AddOperationSequence(
        short workStationId,
        int index,
        bool isOptional,
        short operationId,
        float defaultTime,
        int workPlanStepId,
        short workStationPartId,
        string description,
        int workStationPartCount)
    {

      var operationSequence = repository.Create<OperationSequence>();
      operationSequence.WorkStationId = workStationId;
      operationSequence.Index = index;
      operationSequence.IsOptional = isOptional;
      operationSequence.OperationId = operationId;
      operationSequence.DefaultTime = defaultTime;
      operationSequence.WorkPlanStepId = workPlanStepId;
      operationSequence.WorkStationPartId = workStationPartId;
      operationSequence.Description = description;
      operationSequence.WorkStationPartCount = workStationPartCount;
      repository.Add(operationSequence);
      return operationSequence;
    }
    #endregion
    #region Edit
    public OperationSequence EditOperationSequence(
        byte[] rowVersion,
        int id,
        TValue<short> workStationId,
        TValue<int> index,
        TValue<bool> isOptional,
        TValue<bool> isRepairReturnPoint,
        TValue<short> operationId,
        TValue<float> defaultTime,
        TValue<int> workPlanStepId,
        TValue<short> workStationPartId,
        TValue<string> description,
        TValue<int> workStationPartCount)
    {

      var operationSequence = GetOperationSequence(id: id);
      if (workStationId != null)
        operationSequence.WorkStationId = workStationId;
      if (index != null)
        operationSequence.Index = index;
      if (isOptional != null)
        operationSequence.IsOptional = isOptional;
      if (isRepairReturnPoint != null)
        operationSequence.IsRepairReturnPoint = isRepairReturnPoint;
      if (operationId != null)
        operationSequence.OperationId = operationId;
      if (defaultTime != null)
        operationSequence.DefaultTime = defaultTime;
      if (workPlanStepId != null)
        operationSequence.WorkPlanStepId = workPlanStepId;
      if (workStationPartId != null)
        operationSequence.WorkStationPartId = workStationPartId;
      if (description != null)
        operationSequence.Description = description;
      if (workStationPartCount != null)
        operationSequence.WorkStationPartCount = workStationPartCount;
      repository.Update(rowVersion: rowVersion, entity: operationSequence);
      return operationSequence;
    }
    #endregion
    #region Delete
    public void DeleteOperationSequenceProcess(int id)
    {

      var operationSequence = GetOperationSequence(id: id);
      foreach (var operationConsumingMaterial in operationSequence.OperationConsumingMaterials.ToList())
        DeleteOperationConsumingMaterial(operationConsumingMaterial.Id);
      repository.Delete(operationSequence);
    }
    #endregion
    #region AddProcess
    public OperationSequence AddOperationSequenceProcess(
        short workStationId,
        int index,
        bool isOptional,
        bool isRepairReturnPoint,
        short operationId,
        float defaultTime,
        int workPlanStepId,
        short workStationPartId,
        string description,
        AddOperationConsumingMaterialInput[] addOperationConsumingMaterialInputs,
        AddOperationSequenceMachineTypeParameterInput[] addOperationSequenceMachineTypeParameterInputs,
        int workStationPartCount)
    {

      var operationSequence = AddOperationSequence(
                     workStationId: workStationId,
                     index: index,
                     isOptional: isOptional,
                     operationId: operationId,
                     defaultTime: defaultTime,
                     workPlanStepId: workPlanStepId,
                     workStationPartId: workStationPartId,
                     description: description,
                     workStationPartCount: workStationPartCount
                    );
      foreach (var addOperationConsumingMaterialInput in addOperationConsumingMaterialInputs)
      {
        if (addOperationConsumingMaterialInput.Value != 0)
        {
          var operationConsumingMaterialBomDetail = GetBillOfMaterialDetail(
                        id: addOperationConsumingMaterialInput.BillOfMaterialDetailId);
          if (operationConsumingMaterialBomDetail.BillOfMaterialVersion != operationSequence.WorkPlanStep.WorkPlan.BillOfMaterialVersion
                 || operationConsumingMaterialBomDetail.BillOfMaterialStuffId != operationSequence.WorkPlanStep.WorkPlan.BillOfMaterialStuffId)
            throw new WorkPlanStepBomIsNotEqualToOperationConsumingMaterialBomException();

          AddOperationConsumingMaterial(
                        billOfMaterialDetailId: addOperationConsumingMaterialInput.BillOfMaterialDetailId,
                        value: addOperationConsumingMaterialInput.Value,
                        limitedSerialBuffer: addOperationConsumingMaterialInput.LimitedSerialBuffer,
                        operationSequenceId: operationSequence.Id);
        }
      }
      foreach (var addOperationSequenceMachineTypeParameterInput in addOperationSequenceMachineTypeParameterInputs)
      {
        AddOperationSequenceMachineTypeParameter(
                      operationSequenceId: operationSequence.Id,
                      machineTypeParameterId: addOperationSequenceMachineTypeParameterInput.MachineTypeParameterId,
                      value: addOperationSequenceMachineTypeParameterInput.Value);
      }
      return operationSequence;
    }
    #endregion
    #region EditProcess
    public OperationSequence EditOperationSequenceProcess(
        byte[] rowVersion,
        int id,
        TValue<short> workStationId = null,
        TValue<int> index = null,
        TValue<bool> isOptional = null,
        TValue<bool> isRepairReturnPoint = null,
        TValue<short> operationId = null,
        TValue<float> defaultTime = null,
        TValue<int> workPlanStepId = null,
        TValue<short> workStationPartId = null,
        TValue<string> description = null,
        TValue<int> workStationPartCount = null,
        AddOperationConsumingMaterialInput[] addOperationConsumingMaterialInputs = null,
        EditOperationConsumingMaterialInput[] editOperationConsumingMaterialInputs = null,
        int[] deleteOperationConsumingMaterialInputs = null,
        AddOperationSequenceMachineTypeParameterInput[] addOperationSequenceMachineTypeParameterInputs = null)
    {

      var operationSequence = EditOperationSequence(
                    rowVersion: rowVersion,
                    id: id,
                    workStationId: workStationId,
                    index: index,
                    isOptional: isOptional,
                    isRepairReturnPoint: isRepairReturnPoint,
                    operationId: operationId,
                    defaultTime: defaultTime,
                    workPlanStepId: workPlanStepId,
                    workStationPartId: workStationPartId,
                    description: description,
                    workStationPartCount: workStationPartCount);

      if (addOperationConsumingMaterialInputs != null)
      {
        foreach (var addOperationConsumingMaterialInput in addOperationConsumingMaterialInputs)
        {
          if (addOperationConsumingMaterialInput.Value != 0)
          {
            var operationConsumingMaterialBomDetail = GetBillOfMaterialDetail(
                      id: addOperationConsumingMaterialInput.BillOfMaterialDetailId);
            if (operationConsumingMaterialBomDetail.BillOfMaterialVersion != operationSequence.WorkPlanStep.WorkPlan.BillOfMaterialVersion
                   || operationConsumingMaterialBomDetail.BillOfMaterialStuffId != operationSequence.WorkPlanStep.WorkPlan.BillOfMaterialStuffId)
              throw new WorkPlanStepBomIsNotEqualToOperationConsumingMaterialBomException();

            AddOperationConsumingMaterial(
                      billOfMaterialDetailId: addOperationConsumingMaterialInput.BillOfMaterialDetailId,
                      value: addOperationConsumingMaterialInput.Value,
                      limitedSerialBuffer: addOperationConsumingMaterialInput.LimitedSerialBuffer,
                      operationSequenceId: operationSequence.Id);
          }
        }
      }
      if (editOperationConsumingMaterialInputs != null)
        foreach (var editOperationConsumingMaterialInput in editOperationConsumingMaterialInputs)
        {
          if (editOperationConsumingMaterialInput.Value != 0)
            EditOperationConsumingMaterial(
                      rowVersion: editOperationConsumingMaterialInput.RowVersion,
                      id: editOperationConsumingMaterialInput.Id,
                      billOfMaterialDetailId: editOperationConsumingMaterialInput.BillOfMaterialDetailId,
                      value: editOperationConsumingMaterialInput.Value,
                      limitedSerialBuffer: editOperationConsumingMaterialInput.LimitedSerialBuffer,
                      operationSequenceId: operationSequence.Id);
        }
      if (deleteOperationConsumingMaterialInputs != null)
        foreach (var deleteOperationConsumingMaterialInput in deleteOperationConsumingMaterialInputs)
        {
          DeleteOperationConsumingMaterial(id: deleteOperationConsumingMaterialInput);
        }

      foreach (var operationSequenceMachineTypeParameter in operationSequence.OperationSequenceMachineTypeParameters.ToList())
      {
        DeleteOperationSequenceMachineTypeParameter(operationSequenceMachineTypeParameter);
      }
      if (addOperationSequenceMachineTypeParameterInputs != null)
        foreach (var addOperationSequenceMachineTypeParameterInput in addOperationSequenceMachineTypeParameterInputs)
        {
          AddOperationSequenceMachineTypeParameter(
                        operationSequenceId: operationSequence.Id,
                        machineTypeParameterId: addOperationSequenceMachineTypeParameterInput.MachineTypeParameterId,
                        value: addOperationSequenceMachineTypeParameterInput.Value);
        }

      return operationSequence;
    }
    #endregion
    #region ToResult
    public OperationSequenceResult ToOperationSequenceResult(OperationSequence operationSequence)
    {
      return new OperationSequenceResult
      {
        Id = operationSequence.Id,
        WorkStationId = operationSequence.WorkStationId,
        WorkStationName = operationSequence.WorkStation.Name,
        Index = operationSequence.Index,
        IsOptional = operationSequence.IsOptional,
        OperationId = operationSequence.OperationId,
        DefaultTime = operationSequence.DefaultTime,
        WorkPlanStepId = operationSequence.WorkPlanStepId,
        WorkStationPartId = operationSequence.WorkStationPartId,
        WorkStationPartName = operationSequence.WorkStationPart.Name,
        OperationTitle = operationSequence.Operation.Title,
        WorkStationPartCount = operationSequence.WorkStationPartCount,
        Description = operationSequence.Description,
        RowVersion = operationSequence.RowVersion
      };
    }
    public IQueryable<OperationSequenceResult> ToOperationSequenceResultQuery(IQueryable<OperationSequence> query)
    {
      return from operationSequence in query
             select new OperationSequenceResult
             {
               Id = operationSequence.Id,
               WorkStationId = operationSequence.WorkStationId,
               WorkStationName = operationSequence.WorkStation.Name,
               Index = operationSequence.Index,
               IsOptional = operationSequence.IsOptional,
               DefaultTime = operationSequence.DefaultTime,
               WorkPlanStepId = operationSequence.WorkPlanStepId,
               OperationId = operationSequence.OperationId,
               OperationTitle = operationSequence.Operation.Title,
               WorkStationPartId = operationSequence.WorkStationPartId,
               WorkStationPartName = operationSequence.WorkStationPart.Name,
               WorkStationPartCount = operationSequence.WorkStationPartCount,
               Description = operationSequence.Description,
               RowVersion = operationSequence.RowVersion
             };
    }
    #endregion
    #region ToFullResult
    public FullOperationSequenceResult ToFullOperationSequenceResult(OperationSequence operationSequence)
    {
      return new FullOperationSequenceResult
      {
        Id = operationSequence.Id,
        WorkStationId = operationSequence.WorkStationId,
        WorkStationName = operationSequence.WorkStation.Name,
        Index = operationSequence.Index,
        IsOptional = operationSequence.IsOptional,
        IsRepairReturnPoint = operationSequence.IsRepairReturnPoint,
        WorkStationOperationId = operationSequence.OperationId,
        DefaultTime = operationSequence.DefaultTime,
        WorkPlanStepId = operationSequence.WorkPlanStepId,
        WorkStationPartId = operationSequence.WorkStationPartId,
        WorkStationPartCount = operationSequence.WorkStationPartCount,
        Description = operationSequence.Description,
        OperationConsumingMaterials = operationSequence.OperationConsumingMaterials.AsQueryable().Select(ToFullOperationConsumingMaterialResult),
        RowVersion = operationSequence.RowVersion
      };
    }
    public IQueryable<FullOperationSequenceResult> ToFullOperationSequenceResultQuery(IQueryable<OperationSequence> query)
    {
      var result = from operationSequence in query
                   select new FullOperationSequenceResult
                   {
                     Id = operationSequence.Id,
                     WorkStationId = operationSequence.WorkStationId,
                     WorkStationName = operationSequence.WorkStation.Name,
                     Index = operationSequence.Index,
                     IsOptional = operationSequence.IsOptional,
                     WorkStationOperationId = operationSequence.OperationId,
                     DefaultTime = operationSequence.DefaultTime,
                     WorkPlanStepId = operationSequence.WorkPlanStepId,
                     WorkStationPartId = operationSequence.WorkStationPartId,
                     Description = operationSequence.Description,
                     WorkStationPartCount = operationSequence.WorkStationPartCount,
                     OperationConsumingMaterials = operationSequence.OperationConsumingMaterials.AsQueryable().Select(ToFullOperationConsumingMaterialResult),
                     RowVersion = operationSequence.RowVersion
                   };
      return result;
    }
    #endregion
    #region Search
    public IQueryable<OperationSequenceResult> SearchOperationSequenceResult(IQueryable<OperationSequenceResult> query, TValue<string> search)
    {
      if (search == null) return query;
      return from operationSequence in query
             where
                 operationSequence.WorkStationName.Contains(search) ||
                 operationSequence.Description.Contains(search)
             select operationSequence;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<OperationSequenceResult> SortOperationSequenceResult(IQueryable<OperationSequenceResult> query, SortInput<OperationSequenceSortType> sort)
    {
      switch (sort.SortType)
      {
        case OperationSequenceSortType.WorkStationName:
          return query.OrderBy(a => a.WorkStationName, sort.SortOrder);
        case OperationSequenceSortType.Index:
          return query.OrderBy(a => a.Index, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region SortFull
    public IOrderedQueryable<FullOperationSequenceResult> SortFullOperationSequenceResult(IQueryable<FullOperationSequenceResult> query, SortInput<OperationSequenceSortType> sort)
    {
      switch (sort.SortType)
      {
        case OperationSequenceSortType.WorkStationName:
          return query.OrderBy(a => a.WorkStationName, sort.SortOrder);
        case OperationSequenceSortType.Index:
          return query.OrderBy(a => a.Index, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}

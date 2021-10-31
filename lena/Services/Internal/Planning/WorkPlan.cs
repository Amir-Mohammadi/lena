using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
//using Parlar.DAL;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Planning.WorkPlan;
using lena.Models.Planning.WorkPlanStep;
using System.Collections.Generic;
using lena.Services.Core;
using lena.Models.Production.ProductionOrder;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public IQueryable<WorkPlan> GetWorkPlans(
        TValue<int> id = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<int> billOfMaterialVersion = null,
        TValue<int> version = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<bool> isActive = null,
        TValue<bool> isPublished = null)
    {
      var isIdNull = id == null;
      var isBillOfMaterialStuffIdNull = billOfMaterialStuffId == null;
      var isBillOfMaterialVersionNull = billOfMaterialVersion == null;
      var isVersionNull = version == null;
      var isDescriptionNull = description == null;
      var isTitleNull = title == null;
      var isIsActiveNull = isActive == null;
      var isPublishedNull = isPublished == null;
      var workPlans = from item in repository.GetQuery<WorkPlan>()
                      where
                                (isIdNull || item.Id == id) &&
                                (isBillOfMaterialStuffIdNull || item.BillOfMaterialStuffId == billOfMaterialStuffId) &&
                                (isBillOfMaterialVersionNull || item.BillOfMaterialVersion == billOfMaterialVersion) &&
                                (isVersionNull || item.Version == version) &&
                                (isDescriptionNull || item.Description == description) &&
                                (isTitleNull || item.Title == title) &&
                                (isIsActiveNull || item.IsActive == isActive) &&
                                (isPublishedNull || item.IsPublished == isPublished)
                      select item;
      return workPlans;
    }
    public WorkPlan GetWorkPlan(int id)
    {
      var workPlan = GetWorkPlans(id: id)
                          .FirstOrDefault();
      if (workPlan == null)
        throw new WorkPlanNotFoundException(id);
      return workPlan;
    }
    public WorkPlan AddWorkPlan(
        int billOfMaterialStuffId,
        short billOfMaterialVersion,
        int version,
        string title,
        string description,
        bool isActive)
    {
      var workPlan = repository.Create<WorkPlan>();
      workPlan.Title = title;
      workPlan.BillOfMaterialStuffId = billOfMaterialStuffId;
      workPlan.BillOfMaterialVersion = billOfMaterialVersion;
      workPlan.Version = version;
      workPlan.Description = description;
      workPlan.IsActive = isActive;
      workPlan.CreateDate = DateTime.Now.ToUniversalTime();
      workPlan.IsPublished = false;
      repository.Add(workPlan);
      return workPlan;
    }
    public WorkPlan EditWorkPlan(
       WorkPlan workPlan,
       byte[] rowVersion,
       TValue<int> billOfMaterialStuffId = null,
       TValue<short> billOfMaterialVersion = null,
       TValue<int> version = null,
       TValue<string> title = null,
       TValue<string> description = null,
       TValue<bool> isActive = null,
       TValue<bool> isPublished = null)
    {
      if (billOfMaterialStuffId != null)
        workPlan.BillOfMaterialStuffId = billOfMaterialStuffId;
      if (billOfMaterialVersion != null)
        workPlan.BillOfMaterialVersion = billOfMaterialVersion;
      if (version != null)
        workPlan.Version = version;
      if (description != null)
        workPlan.Description = description;
      if (title != null)
        workPlan.Title = title;
      if (isActive != null)
        workPlan.IsActive = isActive;
      if (isPublished != null)
        workPlan.IsPublished = isPublished;
      repository.Update(rowVersion: rowVersion, entity: workPlan);
      return workPlan;
    }
    public WorkPlan EditWorkPlan(
        byte[] rowVersion,
        int id,
        TValue<int> billOfMaterialStuffId = null,
        TValue<short> billOfMaterialVersion = null,
        TValue<int> version = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<bool> isActive = null,
        TValue<bool> isPublished = null)
    {
      var workPlan = GetWorkPlan(id: id);
      EditWorkPlan(
                workPlan: workPlan,
                rowVersion: rowVersion,
                billOfMaterialStuffId: billOfMaterialStuffId,
                billOfMaterialVersion: billOfMaterialVersion,
                version: version,
                title: title,
                description: description,
                isActive: isActive,
                isPublished: isPublished);
      return workPlan;
    }
    public void DeleteWorkPlan(int id)
    {
      var workPlan = GetWorkPlan(id: id);
      var workPlansSteps = GetWorkPlanSteps(selector: e => e, workPlanId: id);
      foreach (var step in workPlansSteps)
      {
        if (step.ProductionOrders.Any())
        {
          var productionOrder = App.Internals.Production.GetProductionOrder(
                               id: step.ProductionOrders.FirstOrDefault().Id);
          if (productionOrder != null)
          {
            throw new WorkPlanHasProductionOrderException(productionOrderId: productionOrder.Id);
          }
        }
        var sequences = GetOperationSequences(
                  selector: e => e,
                  workPlanStepId: step.Id);
        foreach (var sequence in sequences)
        {
          var materials = GetOperationConsumingMaterials(x => x, operationSequenceId: sequence.Id);
          foreach (var material in materials)
            repository.Delete(material);
          repository.Delete(sequence);
        }
        repository.Delete(step);
      }
      repository.Delete(workPlan);
    }
    private int GetWorkPlanNewVersion(int stuffId)
    {
      var query = GetWorkPlans(billOfMaterialStuffId: stuffId);
      int maxVersion = query.Any() ? query.Max(i => i.Version) : 0;
      return maxVersion + 1;
    }
    public WorkPlan AddWorkPlanProcess(
        int billOfMaterialStuffId,
        short billOfMaterialVersion,
        string title, string
        description,
        AddWorkPlanStepInput[] addWorkPlanStepInputs)
    {
      var activeWorkPlans = GetWorkPlans(
                    billOfMaterialStuffId: billOfMaterialStuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    isActive: true);
      var isActive = !activeWorkPlans.Any();
      var workPlan = AddWorkPlan(
                    billOfMaterialStuffId: billOfMaterialStuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    version: GetWorkPlanNewVersion(billOfMaterialStuffId),
                    title: title,
                    isActive: isActive,
                    description: description);
      if (workPlan.BillOfMaterial.BillOfMaterialVersionType == BillOfMaterialVersionType.Price)
        throw new BillOfMaterialIsPriceTypeException(workPlan.BillOfMaterial.Stuff.Code, workPlan.BillOfMaterial.StuffId, workPlan.BillOfMaterial.Version);
      foreach (var addWorkPlanStepInput in addWorkPlanStepInputs)
      {
        AddWorkPlanStepProcess(
                      title: addWorkPlanStepInput.Title,
                      initialTime: addWorkPlanStepInput.InitialTime,
                      switchTime: addWorkPlanStepInput.SwitchTime,
                      batchTime: addWorkPlanStepInput.BatchTime,
                      batchCount: addWorkPlanStepInput.BatchCount,
                      workPlanId: workPlan.Id,
                      productionStepId: workPlan.BillOfMaterial.ProductionStepId,
                      productionLineId: addWorkPlanStepInput.ProductionLineId,
                      description: addWorkPlanStepInput.Description,
                      isActive: addWorkPlanStepInput.IsActive,
                      needToQualityControl: addWorkPlanStepInput.NeedToQualityControl,
                      planningWithoutMachineLimit: addWorkPlanStepInput.PlanningWithoutMachineLimit,
                      addOperationSequenceInputs: addWorkPlanStepInput.OperationSequenceInputs,
                      productWarehouseId: addWorkPlanStepInput.ProductWarehouseId,
                      consumeWarehouseId: addWorkPlanStepInput.ConsumeWarehouseId
                      );
      }
      return workPlan;
    }
    public WorkPlan EditWorkPlanProcess(byte[] rowVersion,
        int id,
        TValue<int> billOfMaterialStuffId = null,
        TValue<short> billOfMaterialVersion = null,
        TValue<int> version = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<bool> isActive = null,
        AddWorkPlanStepInput[] addWorkPlanStepInputs = null,
        EditWorkPlanStepInput[] editWorkPlanStepInputs = null,
        int[] deleteWorkPlanStepInputs = null
        )
    {
      #region GetWorkPlan
      var workPlan = GetWorkPlan(id: id);
      if (workPlan.BillOfMaterialStuffId != billOfMaterialStuffId ||
                workPlan.BillOfMaterialVersion != billOfMaterialVersion)
      {
        throw new EditingTheWorkPlanIsNotAllowedException(workPlanId: workPlan.Id);
      }
      #endregion
      #region EditWorkPlan
      workPlan = EditWorkPlan(
             workPlan: workPlan,
             rowVersion: rowVersion,
             billOfMaterialStuffId: billOfMaterialStuffId,
             billOfMaterialVersion: billOfMaterialVersion,
             version: version,
             title: title,
             description: description);
      #endregion
      #region Add WorkPlanSteps
      if (addWorkPlanStepInputs != null)
        foreach (var addWorkPlanStepInput in addWorkPlanStepInputs)
        {
          AddWorkPlanStepProcess(
                        title: addWorkPlanStepInput.Title,
                        initialTime: addWorkPlanStepInput.InitialTime,
                        switchTime: addWorkPlanStepInput.SwitchTime,
                        batchTime: addWorkPlanStepInput.BatchTime,
                        batchCount: addWorkPlanStepInput.BatchCount,
                        workPlanId: workPlan.Id,
                        productionStepId: workPlan.BillOfMaterial.ProductionStepId,
                        productionLineId: addWorkPlanStepInput.ProductionLineId,
                        description: addWorkPlanStepInput.Description,
                        isActive: addWorkPlanStepInput.IsActive,
                        needToQualityControl: addWorkPlanStepInput.NeedToQualityControl,
                        planningWithoutMachineLimit: addWorkPlanStepInput.PlanningWithoutMachineLimit,
                        addOperationSequenceInputs: addWorkPlanStepInput.OperationSequenceInputs,
                        productWarehouseId: addWorkPlanStepInput.ProductWarehouseId,
                        consumeWarehouseId: addWorkPlanStepInput.ConsumeWarehouseId
                    );
        }
      #endregion
      #region Edit WorkPlanSteps
      if (editWorkPlanStepInputs != null)
        foreach (var editWorkPlanStepInput in editWorkPlanStepInputs)
        {
          EditWorkPlanStepProcess(
                        rowVersion: editWorkPlanStepInput.RowVersion,
                        id: editWorkPlanStepInput.Id,
                        title: editWorkPlanStepInput.Title,
                        switchTime: editWorkPlanStepInput.SwitchTime,
                        batchTime: editWorkPlanStepInput.BatchTime,
                        batchCount: editWorkPlanStepInput.BatchCount,
                        workPlanId: workPlan.Id,
                        productionStepId: workPlan.BillOfMaterial.ProductionStepId,
                        productionLineId: editWorkPlanStepInput.ProductionLineId,
                        description: editWorkPlanStepInput.Description,
                        isActive: editWorkPlanStepInput.IsActive,
                        needToQualityControl: editWorkPlanStepInput.NeedToQualityControl,
                        planningWithoutMachineLimit: editWorkPlanStepInput.PlanningWithoutMachineLimit,
                        productWarehouseId: editWorkPlanStepInput.ProductWarehouseId,
                        consumeWarehouseId: editWorkPlanStepInput.ConsumeWarehouseId,
                        addOperationSequenceInputs: editWorkPlanStepInput.AddOperationSequenceInputs,
                        editOperationSequenceInputs: editWorkPlanStepInput.EditOperationSequenceInputs,
                        deleteOperationSequenceInputs: editWorkPlanStepInput.DeleteOperationSequenceInputs
                    );
        }
      #endregion
      #region Delete WorkPlanStep
      if (deleteWorkPlanStepInputs != null)
        foreach (var deleteWorkPlanStepInput in deleteWorkPlanStepInputs)
        {
          DeleteWorkPlanStepProcess(id: deleteWorkPlanStepInput);
        }
      #endregion
      return workPlan;
    }
    public void ActiveWorkPlan(byte[] rowVersion, int id)
    {
      var workPlan = GetWorkPlan(id: id);
      var activeWorkPlans = GetWorkPlans(
                billOfMaterialStuffId: workPlan.BillOfMaterialStuffId,
                billOfMaterialVersion: workPlan.BillOfMaterialVersion,
                isActive: true);
      activeWorkPlans = activeWorkPlans.Where(i => i.Id != id);
      foreach (var activeWorkPlan in activeWorkPlans)
      {
        DeactiveWorkPlan(id: activeWorkPlan.Id, rowVersion: activeWorkPlan.RowVersion);
      }
      EditWorkPlan(rowVersion: rowVersion, id: id, isActive: true);
    }
    public void DeactiveWorkPlan(byte[] rowVersion, int id)
    {
      EditWorkPlan(rowVersion: rowVersion, id: id, isActive: false, isPublished: false);
    }
    public void PublishedWorkPlan(byte[] rowVersion, int id)
    {
      var workPlan = GetWorkPlan(id: id);
      var PublishWorkPlans = GetWorkPlans(
                billOfMaterialStuffId: workPlan.BillOfMaterialStuffId,
                isPublished: true);
      if (!workPlan.IsActive)
      {
        throw new TheDeactiveWorkPlanCannotPublishException();
      }
      if (PublishWorkPlans.Any())
      {
        throw new MoreThanPublishWorkPlanCannotBeExistException();
      }
      EditWorkPlan(rowVersion: rowVersion, id: id, isPublished: true);
    }
    public void UnPlublishedWorkPlan(byte[] rowVersion, int id)
    {
      EditWorkPlan(rowVersion: rowVersion, id: id, isPublished: false);
    }
    public WorkPlanResult ToWorkPlanResult(WorkPlan workPlan)
    {
      var bom = workPlan.BillOfMaterial;
      var stuff = workPlan.BillOfMaterial.Stuff;
      return new WorkPlanResult
      {
        Id = workPlan.Id,
        Title = workPlan.Title,
        Version = workPlan.Version,
        IsActive = workPlan.IsActive,
        CreateDate = workPlan.CreateDate,
        BillOfMaterialStuffId = workPlan.BillOfMaterialStuffId,
        BillOfMaterialTitle = bom.Title,
        BillOfMaterialVersion = bom.Version,
        StuffCode = stuff.Code,
        StuffName = stuff.Name,
        Description = workPlan.Description,
        IsPublished = workPlan.IsPublished,
        RowVersion = workPlan.RowVersion
      };
    }
    public FullWorkPlanResult ToFullWorkPlanResult(WorkPlan item)
    {
      var bom = item.BillOfMaterial;
      var stuff = item.BillOfMaterial.Stuff;
      return new FullWorkPlanResult
      {
        Id = item.Id,
        Title = item.Title,
        Version = item.Version,
        IsActive = item.IsActive,
        IsPublished = item.IsPublished,
        CreateDate = item.CreateDate,
        BillOfMaterialStuffId = item.BillOfMaterialStuffId,
        BillOfMaterialTitle = bom.Title,
        BillOfMaterialVersion = bom.Version,
        ProductionStepId = bom.ProductionStepId,
        ProductionStepName = bom.ProductionStep.Name,
        StuffCode = stuff.Code,
        StuffName = stuff.Name,
        Description = item.Description,
        QtyPerBox = bom.QtyPerBox,
        WorkPlanSteps = item.WorkPlanSteps.ToList().Select(ToFullWorkPlanStepResult).ToArray(),
        BillOfMaterialDetails = item.BillOfMaterial.BillOfMaterialDetails.ToList().Select(ToBillOfMaterialDetailResult).ToArray(),
        ProductionLineProductionSteps = item.BillOfMaterial.ProductionStep.ProductionLineProductionSteps.ToList().Select(ToProductionLineProductionStepResult).ToArray(),
        RowVersion = item.RowVersion
      };
    }
    public IQueryable<WorkPlanResult> ToWorkPlanResultQuery(IQueryable<WorkPlan> query)
    {
      return from item in query
             let bom = item.BillOfMaterial
             let stuff = bom.Stuff
             select new WorkPlanResult
             {
               Id = item.Id,
               Title = item.Title,
               Version = item.Version,
               IsActive = item.IsActive,
               IsPublished = item.IsPublished,
               CreateDate = item.CreateDate,
               BillOfMaterialStuffId = item.BillOfMaterialStuffId,
               BillOfMaterialTitle = bom.Title,
               QtyPerBox = bom.QtyPerBox,
               BillOfMaterialVersion = bom.Version,
               StuffCode = stuff.Code,
               StuffName = stuff.Name,
               Description = item.Description,
               RowVersion = item.RowVersion
             };
    }
    public IQueryable<FullWorkPlanResult> ToFullWorkPlanResultQueryForPython(IQueryable<WorkPlan> query)
    {
      return from item in query
             let bom = item.BillOfMaterial
             let stuff = item.BillOfMaterial.Stuff
             select new FullWorkPlanResult
             {
               Id = item.Id,
               Title = item.Title,
               Version = item.Version,
               IsActive = item.IsActive,
               CreateDate = item.CreateDate,
               BillOfMaterialStuffId = item.BillOfMaterialStuffId,
               BillOfMaterialTitle = bom.Title,
               BillOfMaterialVersion = bom.Version,
               ProductionStepId = bom.ProductionStepId,
               ProductionStepName = bom.ProductionStep.Name,
               StuffCode = stuff.Code,
               StuffName = stuff.Name,
               Description = item.Description,
               WorkPlanSteps = item.WorkPlanSteps.ToList().Select(ToFullWorkPlanStepResult).ToArray(),
               BillOfMaterialDetails = item.BillOfMaterial.BillOfMaterialDetails.ToList().Select(ToBillOfMaterialDetailResult).ToArray(),
               ProductionLineProductionSteps = item.BillOfMaterial.ProductionStep.ProductionLineProductionSteps.ToList().Select(ToProductionLineProductionStepResult).ToArray(),
               RowVersion = item.RowVersion
             };
    }
    public IQueryable<WorkPlanResult> SearchWorkPlanResult(
        IQueryable<WorkPlanResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                     item.Title.Contains(searchText) ||
                     item.BillOfMaterialTitle.Contains(searchText) ||
                     item.Description.Contains(searchText) ||
                     item.StuffCode.Contains(searchText) ||
                     item.StuffName.Contains(searchText)
                select item;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    public IOrderedQueryable<WorkPlanResult> SortWorkPlanResult(IQueryable<WorkPlanResult> query, SortInput<WorkPlanSortType> sort)
    {
      switch (sort.SortType)
      {
        case WorkPlanSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case WorkPlanSortType.Version:
          return query.OrderBy(a => a.Version, sort.SortOrder);
        case WorkPlanSortType.BillOfMaterialVersion:
          return query.OrderBy(a => a.BillOfMaterialVersion, sort.SortOrder);
        case WorkPlanSortType.BillOfMaterialTitle:
          return query.OrderBy(a => a.BillOfMaterialTitle, sort.SortOrder);
        case WorkPlanSortType.IsAcitve:
          return query.OrderBy(a => a.IsActive, sort.SortOrder);
        case WorkPlanSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case WorkPlanSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case WorkPlanSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case WorkPlanSortType.CreateDate:
          return query.OrderBy(a => a.CreateDate, sort.SortOrder);
        case WorkPlanSortType.IsPublished:
          return query.OrderBy(a => a.IsPublished, sort.SortOrder);
        case WorkPlanSortType.QtyPerBox:
          return query.OrderBy(a => a.QtyPerBox, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<WorkPlanComboResult> ToWorkPlanComboResultQuery(IQueryable<WorkPlan> workPlans)
    {
      return from item in workPlans
             select new WorkPlanComboResult()
             {
               Id = item.Id,
               Title = item.Title,
               Version = item.Version,
               IsActive = item.IsActive,
               IsPublished = item.IsPublished,
               RowVersion = item.RowVersion
             };
    }
  }
}
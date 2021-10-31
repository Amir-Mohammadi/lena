using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.Common;
using lena.Models.Supplies.PurchaseStep;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public PurchaseStep AddPurchaseStep(
        PurchaseStep purchaseStep,
        TransactionBatch transactionBatch,
        string description,
        int howToBuyDetailId,
        int cargoItemId,
        bool isCurrentStep,
        DateTime followUpDateTime)
    {

      purchaseStep = purchaseStep ?? repository.Create<PurchaseStep>();
      purchaseStep.CargoItemId = cargoItemId;
      purchaseStep.HowToBuyDetailId = howToBuyDetailId;
      purchaseStep.FollowUpDateTime = followUpDateTime;
      purchaseStep.IsCurrentStep = isCurrentStep;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: purchaseStep,
                    transactionBatch: transactionBatch,
                    description: description);
      return purchaseStep;
    }
    #endregion
    #region AddProcess
    public PurchaseStep AddPurchaseStepProcess(
        TransactionBatch transactionBatch,
        string description,
        int howToBuyDetailId,
        int cargoItemId,
        DateTime followUpDateTime,
        byte[] cargoRowVersion
    )
    {


      #region EditCargoItem
      var cargoItem = EditCargoItem(
               id: cargoItemId,
               rowVersion: cargoRowVersion);
      #endregion
      #region Edit  CurrentPurchaseStep Of Cargo
      var currentPurchaseSteps = GetPurchaseSteps(selector: e => e,
              cargoItemId: cargoItemId,
              isCurrentStep: true);
      foreach (var currentPurchaseStep in currentPurchaseSteps)
      {
        EditPurchaseStep(purchaseStep: currentPurchaseStep,
                      rowVersion: currentPurchaseStep.RowVersion,
                      isCurrentStep: false);
      }
      #endregion
      #region Add PurchaseStep
      var purchaseStep = AddPurchaseStep(
              purchaseStep: null,
              transactionBatch: null,
              description: description,
              howToBuyDetailId: howToBuyDetailId,
              cargoItemId: cargoItemId,
              isCurrentStep: true,
              followUpDateTime: followUpDateTime);
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: cargoItemId,
              scrumTaskType: ScrumTaskTypes.ShippingTracking);
      #endregion
      //check projectWork not null
      if (projectWorkItem != null)
      {
        #region DoneTask

        App.Internals.ScrumManagement.DoneScrumTask(
                scrumTask: projectWorkItem,
                rowVersion: projectWorkItem.RowVersion);

        #endregion
        #region Add ProjectWorkItem

        App.Internals.ProjectManagement.AddProjectWorkItem(
                projectWorkItem: null,
                name: $"پیگیری محموله شماره  {cargoItem.Code} ",
                description:
                $"پیگیری محموله شماره  {cargoItem.Code} در مرحله {purchaseStep.HowToBuyDetail.HowToBuy.Title} - {purchaseStep.HowToBuyDetail.Title}",
                color: "",
                departmentId: (int)Departments.Supplies,
                estimatedTime: 10800,
                isCommit: false,
                scrumTaskTypeId: (int)ScrumTaskTypes.ShippingTracking,
                userId: null,
                spentTime: 0,
                remainedTime: 0,
                scrumTaskStep: ScrumTaskStep.ToDo,
                projectWorkId: projectWorkItem.ScrumBackLogId,
                baseEntityId: cargoItem.Id);

        #endregion
      }
      return purchaseStep;
    }
    #endregion
    #region Edit
    public PurchaseStep EditPurchaseStep(
        int id,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<int> howToBuyDetailId = null,
        TValue<int> cargoId = null,
        TValue<bool> isCurrentStep = null,
        TValue<DateTime> followUpDateTime = null)
    {

      var purchaseStep = GetPurchaseStep(id: id);
      return EditPurchaseStep(
                    purchaseStep: purchaseStep,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description,
                    howToBuyDetailId: howToBuyDetailId,
                    cargoItemId: cargoId,
                    isCurrentStep: isCurrentStep,
                    followUpDateTime: followUpDateTime);
    }
    public PurchaseStep EditPurchaseStep(
        PurchaseStep purchaseStep,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<int> howToBuyDetailId = null,
        TValue<int> cargoItemId = null,
        TValue<bool> isCurrentStep = null,
        TValue<DateTime> followUpDateTime = null
        )
    {

      if (howToBuyDetailId != null)
        purchaseStep.HowToBuyDetailId = howToBuyDetailId;
      if (cargoItemId != null)
        purchaseStep.CargoItemId = cargoItemId;
      if (followUpDateTime != null)
        purchaseStep.FollowUpDateTime = followUpDateTime;
      if (isCurrentStep != null)
        purchaseStep.IsCurrentStep = isCurrentStep;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: purchaseStep,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description);
      return retValue as PurchaseStep;
    }
    #endregion
    #region Get
    public PurchaseStep GetPurchaseStep(int id) => GetPurchaseStep(selector: e => e, id: id);
    public TResult GetPurchaseStep<TResult>(
        Expression<Func<PurchaseStep, TResult>> selector,
            int id
        )
    {

      var purchaseStep = GetPurchaseSteps(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (purchaseStep == null)
        throw new PurchaseStepNotFoundException(id);
      return purchaseStep;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetPurchaseSteps<TResult>(
        Expression<Func<PurchaseStep, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> howToBuyDetailId = null,
        TValue<int?[]> cargoItemIds = null,
        TValue<int> cargoItemId = null,
        TValue<bool> isCurrentStep = null,
        TValue<DateTime> followUpDateTime = null
        )
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<PurchaseStep>();
      if (howToBuyDetailId != null)
        query = query.Where(i => i.HowToBuyDetailId == howToBuyDetailId);
      if (cargoItemId != null)
        query = query.Where(i => i.CargoItemId == cargoItemId);
      if (cargoItemIds != null)
        query = query.Where(i => cargoItemIds.Value.Contains(i.CargoItemId));
      if (followUpDateTime != null)
        query = query.Where(i => i.FollowUpDateTime == followUpDateTime);
      if (isCurrentStep != null)
        query = query.Where(i => i.IsCurrentStep == isCurrentStep);
      return query.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<PurchaseStep, PurchaseStepResult>> ToPurchaseStepResult =
        purchaseStep =>
        new PurchaseStepResult
        {
          Id = purchaseStep.Id,
          Code = purchaseStep.Code,
          CargoItemId = purchaseStep.CargoItemId,
          CargoCode = purchaseStep.Code,
          UserId = purchaseStep.UserId,
          EmployeeName = purchaseStep.User.Employee.FirstName + " " + purchaseStep.User.Employee.LastName,
          DateTime = purchaseStep.DateTime,
          Description = purchaseStep.Description,
          HowToBuyDetailId = purchaseStep.HowToBuyDetailId,
          HowToBuyDetailOrder = purchaseStep.HowToBuyDetail.Order,
          HowToBuyDetailTitle = purchaseStep.HowToBuyDetail.Title,
          FollowUpDateTime = purchaseStep.FollowUpDateTime,
          HowToBuyId = purchaseStep.HowToBuyDetail.HowToBuyId,
          HowToBuyTitle = purchaseStep.HowToBuyDetail.HowToBuy.Title,
          RowVersion = purchaseStep.RowVersion,
        };
    #endregion
    #region Search
    public IQueryable<PurchaseStepResult> SearchPurchaseStepResults(
        IQueryable<PurchaseStepResult> query,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = query.Where(i =>
            i.Code.Contains(searchText) ||
            i.HowToBuyTitle.Contains(searchText) ||
            i.HowToBuyDetailTitle.Contains(searchText) ||
            i.EmployeeName.Contains(searchText) ||
            i.Description.Contains(searchText));
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<PurchaseStepResult> SortPurchaseStepResults(
        IQueryable<PurchaseStepResult> query,
        SortInput<PurchaseStepSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case PurchaseStepSortType.Code:
          return query.OrderBy(i => i.Code, sortInput.SortOrder);
        case PurchaseStepSortType.CargoCode:
          return query.OrderBy(i => i.CargoCode, sortInput.SortOrder);
        case PurchaseStepSortType.EmployeeName:
          return query.OrderBy(i => i.EmployeeName, sortInput.SortOrder);
        case PurchaseStepSortType.DateTime:
          return query.OrderBy(i => i.DateTime, sortInput.SortOrder);
        case PurchaseStepSortType.HowToBuyDetailOrder:
          return query.OrderBy(i => i.HowToBuyDetailOrder, sortInput.SortOrder);
        case PurchaseStepSortType.HowToBuyTitle:
          return query.OrderBy(i => i.HowToBuyTitle, sortInput.SortOrder);
        case PurchaseStepSortType.HowToBuyDetailTitle:
          return query.OrderBy(i => i.HowToBuyDetailTitle, sortInput.SortOrder);
        case PurchaseStepSortType.FollowUpDateTime:
          return query.OrderBy(i => i.FollowUpDateTime, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}

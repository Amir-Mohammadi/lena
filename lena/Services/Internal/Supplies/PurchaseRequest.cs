using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Internals.Supplies.Exception;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.PurchaseRequest;
using System.Linq.Dynamic;
using lena.Models.StaticData;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public PurchaseRequest AddPurchaseRequest(
        PurchaseRequest purchaseRequest,
        TransactionBatch transactionBatch,
        DateTime deadline,
        double qty,
        double requestQty,
        int costCenterId,
        string projectCode,
        byte unitId,
        int stuffId,
        bool essential,
        string description,
        int? planCodeId,
        int? responsibleEmployeeId,
        Guid? documentId,
        string link,
        int? requestEmployeeId,
        PurchaseRequestSupplyType? supplyType
        )
    {
      purchaseRequest = purchaseRequest ?? repository.Create<PurchaseRequest>();
      purchaseRequest.Essential = essential;
      purchaseRequest.Deadline = deadline;
      purchaseRequest.Qty = requestQty;
      purchaseRequest.RequestQty = requestQty;
      purchaseRequest.CostCenterId = costCenterId;
      purchaseRequest.ProjectCode = projectCode;
      purchaseRequest.UnitId = unitId;
      purchaseRequest.StuffId = stuffId;
      purchaseRequest.PlanCodeId = planCodeId;
      purchaseRequest.Status = PurchaseRequestStatus.Waiting;
      purchaseRequest.ResponsibleEmployeeId = responsibleEmployeeId;
      purchaseRequest.EmployeeRequesterId = requestEmployeeId;
      purchaseRequest.DocumentId = documentId;
      purchaseRequest.Link = link;
      purchaseRequest.SupplyType = supplyType;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: purchaseRequest,
                    transactionBatch: transactionBatch,
                    description: description);
      return purchaseRequest;
    }
    #endregion
    #region Delete
    public void RemovePurchaseRequest(
        int id,
        byte[] rowVersion)
    {
      var purchaseRequest = GetPurchaseRequest(id: id);
      if (purchaseRequest.Status == PurchaseRequestStatus.Rejected || purchaseRequest.Status == PurchaseRequestStatus.Canceled)
        throw new CanNotDeletePurchaseRequestException(id);
      else if (purchaseRequest.Status == PurchaseRequestStatus.Accepted || purchaseRequest.Status == PurchaseRequestStatus.Waiting)
      {
        if (purchaseRequest.Status == PurchaseRequestStatus.Accepted)
        {
          var purchaseOrders = App.Internals.Supplies.GetPurchaseOrderDetails(e => e, purchaseOrderId: id, isDelete: false);
          if (purchaseOrders.Any())
            throw new CanNotDeletePurchaseRequestHasPurchaseOrderException(id);
        }
        foreach (var item in purchaseRequest.BaseEntityConfirmations)
          App.Internals.Confirmation.DeleteBaseEntityConfirmation(id: item.Id);
        App.Internals.ApplicationBase.RemoveBaseEntityProcess(
              transactionBatchId: null,
              baseEntity: purchaseRequest,
                  rowVersion: rowVersion);
        #region Notify To Department
        App.Internals.Notification.NotifyToDepartment(
           departmentId: (int)Departments.Planning,
           title: $"حذف درخواست خرید {purchaseRequest.Code}",
           description: purchaseRequest.Description,
           scrumEntityId: null);
        #endregion
      }
      else
        throw new CanNotDeletePurchaseRequestHasPurchaseOrderException(id);
    }
    #endregion
    #region AddProcess
    public void AddPurchaseRequestProcesses(AddPurchaseRequestInput[] addPurchaseRequestInputs)
    {
      var currentUser = App.Providers.Security.CurrentLoginData;
      foreach (var item in addPurchaseRequestInputs)
      {
        if (currentUser.DepartmentId != (int)Departments.Planning)
        {
          item.ProjectCode = null;
          item.PlanCodeId = null;
        }
        AddPurchaseRequestProcess(
                      deadline: item.DeadLine,
                      qty: item.RequestQty,
                      requestQty: item.RequestQty,
                      costCenterId: item.CostcenterId,
                      projectCode: item.ProjectCode,
                      stuffId: item.StuffId,
                      unitId: item.UnitId,
                      purchaseRequestStatus: item.PurchaseRequestStatus,
                      planCodeId: item.PlanCodeId,
                      description: item.Description,
                      requestEmployeeId: item.EmployeeRequsterId,
                      link: item.Link,
                      supplyType: item.SupplyType,
                     uploadFileData: string.IsNullOrWhiteSpace(item.FileKey)
              ? null
              : Core.App.Providers.Session.GetAs<UploadFileData>(item.FileKey));
      }
    }
    public void EditPurchaseRequestProcesses(EditPurchaseRequestInput[] editPurchaseRequestInputs)
    {
      var currentUser = App.Providers.Security.CurrentLoginData;
      var logedEmployeeId = currentUser.UserEmployeeId;
      foreach (var input in editPurchaseRequestInputs)
      {
        if (currentUser.DepartmentId != (int)Departments.Planning)
        {
          input.ProjectCode = null;
          input.PlanCodeId = null;
        }
        var purchaseRequest = ToPurchaseRequestResult(GetPurchaseRequests(e => e, id: input.Id)).FirstOrDefault();
        var confirmerEmployeeId = purchaseRequest.ConfirmerEmployeeId;
        #region editPurchaseRequestWithoutCheckOrderPermission
        var editPurchaseRequestWithoutCheckOrderPermission = App.Internals.UserManagement.CheckPermission(
            actionName: StaticActionName.EditPurchaseRequestWithoutCheckOrder,
            actionParameters: null);
        #endregion
        if (editPurchaseRequestWithoutCheckOrderPermission.AccessType == AccessType.Denied)
        {
          if (confirmerEmployeeId != null && confirmerEmployeeId != logedEmployeeId)
          {
            throw new UserCantEditPurchaseRequestException(input.Id);
          }
        }
        var stuff = App.Internals.SaleManagement.GetStuff(purchaseRequest.StuffId);
        if (stuff.StuffPurchaseCategoryId != null)
        {
          var catDetails = App.Internals.Supplies.GetStuffPurchaseCategoryDetails(
                    e => e,
                    stuffPurchaseCategoryId: stuff.StuffPurchaseCategoryId.Value);
          var applicatorIds = catDetails.Select(i => i.ApplicatorUserGroupId).ToArray();
          var memberships = App.Internals.UserManagement.GetMemberships(
                    selector: e => e,
                    userId: App.Providers.Security.CurrentLoginData.UserId,
                    userGroupIds: applicatorIds);
          var userGroups = App.Internals.UserManagement.GetUserGroups(ids: applicatorIds)
                    .Select(i => i.Name);
          if (editPurchaseRequestWithoutCheckOrderPermission.AccessType == AccessType.Denied)
          {
            if (!memberships.Any())
              throw new NotHavePermissionToStuffPurchaseRequestException(userGroups: string.Join("، ", userGroups));
          }
        }
        var unit = App.Internals.ApplicationBase.GetUnit(
                  id: purchaseRequest.UnitId);
        if (input.RequestQty.HasValue)
          input.RequestQty = Math.Round((double)input.RequestQty, unit.DecimalDigitCount);
        if (purchaseRequest == null)
          throw new PurchaseRequestNotFoundException(input.Id);
        if (purchaseRequest.RemainedQty == 0)
          if (editPurchaseRequestWithoutCheckOrderPermission.AccessType == AccessType.Denied)
          {
            throw new PurchaseRequestFinishedException(input.Id);
          }
        if (purchaseRequest.Status == PurchaseRequestStatus.Rejected || purchaseRequest.Status == PurchaseRequestStatus.Canceled)
          throw new CanNotEditPurchaseRequestException(input.Id);
        //if (purchaseRequest.Status == PurchaseRequestStatus.Accepted)
        //{
        //    var purchaseOrders = App.Internals.Supplies.GetPurchaseOrderDetails(e => e, purchaseOrderId: input.Id, isDelete: false)
        //    
        //;
        //    if (purchaseOrders.Any())
        //        throw new CanNotDeletePurchaseRequestHasPurchaseOrderException(input.Id);
        //}
        var orderQty = purchaseRequest.OrderedQty;
        if (purchaseRequest.Status == PurchaseRequestStatus.Accepted && input.RequestQty < orderQty)
          throw new PurchaseRequestEditQtyShouldNotBeLessThanOrderQtyException(input.Id, (double)input.RequestQty, orderQty.Value);
        var purchaseRequestResult = GetPurchaseRequest(id: input.Id);
        #region LogPurchaseRequestDeadlineModification
        if (input.DeadLine.HasValue && (input.DeadLine != purchaseRequest.Deadline))
        {
          AddPurchaseRequestEditLog(
                    purchaseRequestId: input.Id,
                    beforeDeadLineDateTime: purchaseRequest.Deadline,
                    afterDeadLineDateTime: input.DeadLine.Value,
                    description: input.EditLogDescription);
        }
        if (input.RequestQty.HasValue && (input.RequestQty != purchaseRequest.RequestQty))
        {
          AddPurchaseRequestEditLog(
                    purchaseRequestId: input.Id,
                    beforeDeadLineDateTime: null,
                    afterDeadLineDateTime: null,
                    beforeRequestQty: purchaseRequest.Qty,
                    afterRequestQty: input.RequestQty.Value,
                    description: input.EditLogDescription);
        }
        #endregion
        var editPurchaseRequest = EditPurchaseRequest(
            id: input.Id,
            planCodeId: input.PlanCodeId,
            costCenterId: input.CostCenterId,
            rowVersion: input.RowVersion,
            projectCode: input.ProjectCode,
            deadline: input.DeadLine,
            description: input.Description,
            qty: input.RequestQty,
            link: input.Link,
            employeeRequesterId: input.EmployeeRequesterId);
        if (editPurchaseRequest.Qty < orderQty)
        {
          throw new PurchaseRequestConfirmedQtyIsLessThanOrderQtyException(editPurchaseRequest.Id, (double)editPurchaseRequest.Qty, orderQty.Value);
        }
        if (input.RequestQty.HasValue && purchaseRequestResult.RequestQty != input.RequestQty)
        {
          #region RemoveTransactionBatch
          App.Internals.WarehouseManagement.RemoveTransactionBatchProcess(
              oldTransactionBathId: purchaseRequestResult.TransactionBatch.Id,
              newTransactionBatchId: purchaseRequestResult.TransactionBatch.Id);
          #endregion
          #region AddTransactionPlan
          App.Internals.WarehouseManagement.AddTransactionPlanProcess(
              transactionBatchId: purchaseRequestResult.TransactionBatch.Id,
              effectDateTime: purchaseRequestResult.Deadline,
              stuffId: purchaseRequestResult.StuffId,
              billOfMaterialVersion: null,
              stuffSerialCode: null,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportPurchaseRequest.Id,
              amount: (double)input.RequestQty,
              unitId: purchaseRequestResult.UnitId,
              description: "",
              isEstimated: false,
              referenceTransaction: null);
          #endregion
        }
      }
    }
    public PurchaseRequest AddPurchaseRequestProcess(
        DateTime deadline,
        double requestQty,
        double qty,
        byte unitId,
        int stuffId,
        string projectCode,
        PurchaseRequestStatus purchaseRequestStatus,
        PurchaseRequestSupplyType? supplyType,
        string description,
        int? planCodeId,
        int? requestEmployeeId = null,
        UploadFileData uploadFileData = null,
        TValue<int> costCenterId = null,
        string link = null
       )
    {
      var stuff = App.Internals.SaleManagement.GetStuff(stuffId);
      #region Check StuffPurchaseCategory Permissions
      if (stuff.StuffPurchaseCategoryId == null)
        throw new StuffHasNoPurchaseCategoryException(stuffId: stuff.Id, stuffCode: stuff.Code);
      var catDetails = App.Internals.Supplies.GetStuffPurchaseCategoryDetails(
                selector: e => e,
                stuffPurchaseCategoryId: stuff.StuffPurchaseCategoryId.Value);
      var applicatorIds = catDetails.Select(i => i.ApplicatorUserGroupId).ToArray();
      var memberships = App.Internals.UserManagement.GetMemberships(
                selector: e => e,
                userId: App.Providers.Security.CurrentLoginData.UserId,
                userGroupIds: applicatorIds);
      var userGroups = App.Internals.UserManagement.GetUserGroups(ids: applicatorIds)
                    .Select(i => i.Name);
      if (!memberships.Any())
        throw new NotHavePermissionToStuffPurchaseRequestException(userGroups: string.Join("، ", userGroups));
      #endregion
      #region AddTransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region GetDefaultSuppliesPurchaserUser
      var user = GetDefaultSuppliesPurchaserUser(stuffId: stuffId);
      #endregion
      var unit = App.Internals.ApplicationBase.GetUnit(id: unitId);
      #region AddPurchaseRequest
      var essentialDaysCount = App.Internals.ApplicationSetting.GetPurchaseRequestEssentialDateTimeValue();
      var dateTiemNow = DateTime.Now.ToLocalTime();
      var essential = false;
      int days = (int)deadline.Subtract(dateTiemNow).TotalDays;
      if (days < essentialDaysCount)
        essential = true;
      var stuffProviders = GetStuffProviders(
                stuffId: stuffId);
      var instantType = stuffProviders.Any(
                i => i.InstantLeadTime > days
                );
      if (supplyType == PurchaseRequestSupplyType.MOQ)
        supplyType = PurchaseRequestSupplyType.MOQ;
      else if (instantType)
        supplyType = PurchaseRequestSupplyType.Instant;
      else
        supplyType = PurchaseRequestSupplyType.Normal;
      #region uploadFile
      Guid? documentId = null;
      if (uploadFileData != null)
      {
        var document = App.Internals.ApplicationBase.AddDocument(
              name: uploadFileData.FileName,
              fileStream: uploadFileData.FileData);
        documentId = document.Id;
      }
      #endregion
      var purchaseRequest = AddPurchaseRequest(
          purchaseRequest: null,
          essential: essential,
          transactionBatch: transactionBatch,
          deadline: deadline,
          qty: Math.Round(qty, unit.DecimalDigitCount),
          requestQty: Math.Round(requestQty, unit.DecimalDigitCount),
          planCodeId: planCodeId,
          costCenterId: costCenterId,
          projectCode: projectCode,
          unitId: unitId,
          stuffId: stuffId,
          description: description,
          documentId: documentId,
          requestEmployeeId: requestEmployeeId,
          responsibleEmployeeId: user?.Employee.Id,
          link: link,
          supplyType: supplyType);
      #endregion
      #region AddPurchaseRequestSummary
      AddPurchaseRequestSummary(
              orderedQty: 0,
              cargoedQty: 0,
              receiptedQty: 0,
              qualityControlPassedQty: 0,
              qualityControlFailedQty: 0,
                  purchaseRequestId: purchaseRequest.Id);
      #endregion
      #region AddTransactionPlan
      App.Internals.WarehouseManagement.AddTransactionPlanProcess(
              transactionBatchId: transactionBatch.Id,
              effectDateTime: deadline,
              stuffId: stuffId,
              billOfMaterialVersion: null,
              stuffSerialCode: null,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportPurchaseRequest.Id,
              amount: requestQty,
              unitId: unitId,
              description: "",
              isEstimated: false,
              referenceTransaction: null);
      #endregion
      #region Add or Get ProjectStep
      var projectStep = App.Internals.ProjectManagement.GetOrAddCommonProjectStep(
              departmentId: (int)Departments.Supplies);
      #endregion
      #region Add ProjectWork
      var projectWork = App.Internals.ProjectManagement.AddProjectWork(
              projectWork: null,
              name: $"پروسه خرید کالا {purchaseRequest.Stuff.Code}",
              description: "",
              color: "",
              departmentId: (int)Departments.Supplies,
              estimatedTime: 18000,
              isCommit: false,
              projectStepId: projectStep.Id,
              baseEntityId: null
          );
      #endregion
      #region Add ProjectWorkItem
      //check projectWork not null
      if (projectWork != null)
      {
        var projectWorkItem = App.Internals.ProjectManagement.AddProjectWorkItem(
                      projectWorkItem: null,
                      name: $"بررسی درخواست خرید {purchaseRequest.Stuff.Code} ",
                      description: $"عنوان کالا:{purchaseRequest.Stuff.Name}",
                      color: "",
                      departmentId: (int)Departments.Warehouse,
                      estimatedTime: 10800,
                      isCommit: false,
                      scrumTaskTypeId: (int)ScrumTaskTypes.ConfirmPurchaseRequest,
                      userId: user?.Id,
                      spentTime: 0,
                      remainedTime: 0,
                      scrumTaskStep: ScrumTaskStep.ToDo,
                      projectWorkId: projectWork.Id,
                      baseEntityId: purchaseRequest.Id);
      }
      #endregion
      return purchaseRequest;
    }
    #endregion
    #region Edit
    public PurchaseRequest EditPurchaseRequest(
        int id,
        byte[] rowVersion,
        TValue<DateTime> deadline = null,
        TValue<double> qty = null,
        TValue<int> planCodeId = null,
        TValue<int> costCenterId = null,
        TValue<string> projectCode = null,
        TValue<double> requestQty = null,
        TValue<byte> unitId = null,
        TValue<int> stuffId = null,
        TValue<PurchaseRequestStatus> status = null,
        TValue<int?> responsibleEmployeeId = null,
        TValue<string> description = null,
        TValue<bool> isArchived = null,
        TValue<bool> essential = null,
        TValue<int> purchaseRequestStepDetailId = null,
        TValue<int> employeeRequesterId = null,
        TValue<string> link = null,
        TValue<Risk> risk = null,
        UploadFileData uploadFileData = null)
    {
      PurchaseRequest purchaseRequest = GetPurchaseRequest(id: id);
      var editPurchaseRequest = EditPurchaseRequest(
                    purchaseRequest: purchaseRequest,
                    rowVersion: rowVersion,
                    deadline: deadline,
                    essential: essential,
                    qty: qty,
                    planCodeId: planCodeId,
                    costCenterId: costCenterId,
                    projectCode: projectCode,
                    requestQty: requestQty,
                    unitId: unitId,
                    stuffId: stuffId,
                    status: status,
                    responsibleEmployeeId: responsibleEmployeeId,
                    description: description,
                    isArchived: isArchived,
                    risk: risk,
                    employeeRequesterId: employeeRequesterId,
                    link: link,
                    purchaseRequestStepDetailId: purchaseRequestStepDetailId);
      #region Notify To Department
      App.Internals.Notification.NotifyToDepartment(
         departmentId: (int)Departments.Planning,
         title: $"ویرایش درخواست خرید {purchaseRequest.Code}",
         description: purchaseRequest.Description,
         scrumEntityId: null);
      #endregion
      #region document
      var purchaserRequests = GetPurchaseRequests(
          selector: e => e,
          id: id);
      Guid? documentId;
      if (uploadFileData != null)
      {
        documentId = App.Internals.ApplicationBase.AddDocument(
                  name: uploadFileData.FileName,
                  fileStream: uploadFileData.FileData)
              .Id;
      }
      else
      {
        documentId = purchaseRequest.DocumentId;
      }
      #endregion
      return editPurchaseRequest;
    }
    public PurchaseRequest EditPurchaseRequest(
        PurchaseRequest purchaseRequest,
        byte[] rowVersion,
        TValue<DateTime> deadline = null,
        TValue<double> qty = null,
        TValue<int> planCodeId = null,
        TValue<int> costCenterId = null,
        TValue<string> projectCode = null,
        TValue<double> requestQty = null,
        TValue<byte> unitId = null,
        TValue<int> stuffId = null,
        TValue<PurchaseRequestStatus> status = null,
        TValue<int?> responsibleEmployeeId = null,
        TValue<string> description = null,
        TValue<bool> isArchived = null,
        TValue<bool> essential = null,
        TValue<int> purchaseRequestStepDetailId = null,
        TValue<Risk> risk = null,
        TValue<Guid> documentId = null,
        TValue<int> employeeRequesterId = null,
        TValue<string> link = null)
    {
      if (deadline != null)
        purchaseRequest.Deadline = deadline;
      if (essential != null)
        purchaseRequest.Essential = essential;
      if (qty != null)
        purchaseRequest.Qty = qty;
      if (requestQty != null)
        purchaseRequest.RequestQty = requestQty;
      if (costCenterId != null)
        purchaseRequest.CostCenterId = costCenterId;
      if (projectCode != null)
        purchaseRequest.ProjectCode = projectCode;
      if (unitId != null)
        purchaseRequest.UnitId = unitId;
      if (stuffId != null)
        purchaseRequest.StuffId = stuffId;
      if (description != null)
        purchaseRequest.Description = description;
      if (status != null)
        purchaseRequest.Status = status;
      if (responsibleEmployeeId != null)
        purchaseRequest.ResponsibleEmployeeId = responsibleEmployeeId;
      if (isArchived != null)
        purchaseRequest.IsArchived = isArchived;
      if (purchaseRequestStepDetailId != null)
        purchaseRequest.PurchaseRequestStepDetailId = purchaseRequestStepDetailId;
      if (risk != null)
        purchaseRequest.LatestRisk = risk;
      if (planCodeId != null)
        purchaseRequest.PlanCodeId = planCodeId;
      if (documentId != null)
        purchaseRequest.DocumentId = documentId;
      if (employeeRequesterId != null)
        purchaseRequest.EmployeeRequesterId = employeeRequesterId;
      if (link != null)
        purchaseRequest.Link = link;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: purchaseRequest,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as PurchaseRequest;
    }
    #endregion
    #region AssignResponsible
    public PurchaseRequest AssignResponsiblePurchaseRequest(
        int id,
        byte[] rowVersion,
        int? responsibleEmployeeId)
    {
      var employeeId = new TValue<int?>(responsibleEmployeeId);
      var result = EditPurchaseRequest(
                     id: id,
                     rowVersion: rowVersion,
                     responsibleEmployeeId: employeeId);
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
          baseEntityId: id,
          scrumTaskType: ScrumTaskTypes.PurchaseOrder);
      if (projectWorkItem == null)
        projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                  baseEntityId: id,
                  scrumTaskType: ScrumTaskTypes.PurchaseOrder);
      #endregion
      if (responsibleEmployeeId == null)
      {
        App.Internals.ScrumManagement.AssignScrumTask(
                  id: projectWorkItem.Id,
                  rowVersion: projectWorkItem.RowVersion,
                  userId: null);
      }
      else
      {
        App.Internals.ScrumManagement.AssignScrumTask(
                  id: projectWorkItem.Id,
                  rowVersion: projectWorkItem.RowVersion,
                  userId: result.ResponsibleEmployee.User.Id);
      }
      return result;
    }
    #endregion
    #region AssignResponsibles
    public void AssignResponsiblePurchaseRequests(
        AssignResponsiblePurchaseRequestInput[] assignResponsiblePurchaseRequests)
    {
      foreach (var assignResponsiblePurchaseRequest in assignResponsiblePurchaseRequests)
      {
        AssignResponsiblePurchaseRequest(
                      id: assignResponsiblePurchaseRequest.Id,
                      rowVersion: assignResponsiblePurchaseRequest.RowVersion,
                      responsibleEmployeeId: assignResponsiblePurchaseRequest.ResponsibleEmployeeId);
      }
    }
    #endregion
    #region Get
    public PurchaseRequest GetPurchaseRequest(int id) => GetPurchaseRequest(selector: e => e, id: id);
    public TResult GetPurchaseRequest<TResult>(Expression<Func<PurchaseRequest, TResult>> selector, int id)
    {
      var orderItemBlock = GetPurchaseRequests(selector: selector, id: id).FirstOrDefault();
      if (orderItemBlock == null)
        throw new PurchaseRequestNotFoundException(id);
      return orderItemBlock;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetPurchaseRequests<TResult>(
        Expression<Func<PurchaseRequest, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<DateTime> deadline = null,
        TValue<double> qty = null,
        TValue<double> requestQty = null,
        TValue<int> unitId = null,
        TValue<int> stuffId = null,
        TValue<StuffType> stuffType = null,
        TValue<int> departmentId = null,
        TValue<string> planCode = null,
        TValue<int[]> selectedPlanCodeIds = null,
        TValue<string> projectCode = null,
        TValue<int?> employeeId = null,
        TValue<DateTime?> fromRequestDate = null,
        TValue<DateTime?> toRequestDate = null,
        TValue<DateTime?> fromDeadline = null,
        TValue<DateTime?> toDeadline = null,
        TValue<int?> responsibleEmployeeId = null,
        TValue<PurchaseRequestStatus> status = null,
        TValue<PurchaseRequestStatus[]> statuses = null,
        TValue<PurchaseRequestStatus[]> notHasStatuses = null,
        TValue<bool> isArchived = null,
        TValue<int?> planCodeId = null,
        TValue<int> employeeRequesterId = null,
        TValue<string> link = null,
        TValue<Guid> documentId = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    departmentId: departmentId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var purchaseRequest = baseQuery.OfType<PurchaseRequest>();
      if (deadline != null)
        purchaseRequest = purchaseRequest.Where(r => r.Deadline == deadline);
      if (qty != null)
        purchaseRequest = purchaseRequest.Where(r => Math.Abs(r.Qty - qty) < 0.000001);
      if (requestQty != null)
        purchaseRequest = purchaseRequest.Where(r => Math.Abs(r.RequestQty - requestQty) < 0.000001);
      if (unitId != null)
        purchaseRequest = purchaseRequest.Where(r => r.UnitId == unitId);
      if (stuffId != null)
        purchaseRequest = purchaseRequest.Where(r => r.StuffId == stuffId);
      if (stuffType != null)
        purchaseRequest = purchaseRequest.Where(r => r.Stuff.StuffType == stuffType);
      if (status != null)
        purchaseRequest = purchaseRequest.Where(i => i.Status.HasFlag(status));
      if (responsibleEmployeeId != null)
        purchaseRequest = purchaseRequest.Where(r => r.ResponsibleEmployeeId == responsibleEmployeeId);
      if (employeeId != null)
        purchaseRequest = purchaseRequest.Where(r => r.User.Employee.Id == employeeId);
      if (fromRequestDate != null)
        purchaseRequest = purchaseRequest.Where(r => r.DateTime >= fromRequestDate);
      if (toRequestDate != null)
        purchaseRequest = purchaseRequest.Where(r => r.DateTime <= toRequestDate);
      if (fromDeadline != null)
        purchaseRequest = purchaseRequest.Where(r => r.Deadline >= fromDeadline);
      if (toDeadline != null)
        purchaseRequest = purchaseRequest.Where(r => r.Deadline <= toDeadline);
      if (ids != null)
        purchaseRequest = purchaseRequest.Where(i => ids.Value.Contains(i.Id));
      if (projectCode != null && projectCode != "")
      {
        purchaseRequest = purchaseRequest.Where(i => i.ProjectCode.Contains(projectCode));
      }
      if (isArchived != null)
        purchaseRequest = purchaseRequest.Where(i => i.IsArchived == isArchived);
      if (statuses != null)
      {
        var s = PurchaseRequestStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        purchaseRequest = purchaseRequest.Where(i => (i.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = PurchaseRequestStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        purchaseRequest = purchaseRequest.Where(i => (i.Status & s) == 0);
      }
      if (planCodeId != null)
        purchaseRequest = purchaseRequest.Where(i => i.PlanCodeId == planCodeId);
      if (employeeRequesterId != null)
        purchaseRequest = purchaseRequest.Where(i => i.EmployeeRequesterId == employeeRequesterId);
      if (link != null)
        purchaseRequest = purchaseRequest.Where(i => i.Link == link);
      if (documentId != null)
        purchaseRequest = purchaseRequest.Where(i => i.DocumentId == documentId);
      if (selectedPlanCodeIds != null)
        purchaseRequest = purchaseRequest.Where(i => selectedPlanCodeIds.Value.Contains(i.PlanCodeId.Value));
      return purchaseRequest.Select(selector);
    }
    #endregion
    #region Search
    public IQueryable<PurchaseRequestResult> SearchPurchaseRequestResultQuery(
        IQueryable<PurchaseRequestResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText = null,
        DateTime? fromConfirmDate = null,
        DateTime? toConfirmDate = null,
        int? stuffCategoryId = null,
        int? id = null,
        RiskLevelStatus? riskLevelStatus = null
        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from r in query
                where r.StuffName.Contains(searchText) ||
                      r.EmployeeFullName.Contains(searchText) ||
                      r.LatestRiskTitle.Contains(searchText) ||
                      r.Description.Contains(searchText) ||
                      r.ResponsibleEmployeeFullName.Contains(searchText) ||
                      r.ConfirmationDescription.Contains(searchText) ||
                      r.DepartmentName.Contains(searchText) ||
                      r.StuffCategoryName.Contains(searchText)
                select r;
      if (fromConfirmDate != null)
        query = query.Where(r => r.ConfirmDate >= fromConfirmDate);
      if (toConfirmDate != null)
        query = query.Where(r => r.ConfirmDate <= toConfirmDate);
      if (riskLevelStatus != null)
        query = query.Where(r => r.RiskLevelStatus == riskLevelStatus);
      if (stuffCategoryId != null)
        query = query.Where(r => r.StuffCategoryId == stuffCategoryId);
      if (id != null)
        query = query.Where(r => r.Id == id);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<PurchaseRequestResult> SortPurchaseRequestResult(
        IQueryable<PurchaseRequestResult> input,
        SortInput<PurchaseRequestSortType> options)
    {
      switch (options.SortType)
      {
        case PurchaseRequestSortType.MaxEstimateDateTime:
          return input.OrderBy(i => i.MaxEstimateDateTime, options.SortOrder);
        case PurchaseRequestSortType.ConfirmDate:
          return input.OrderBy(i => i.ConfirmDate, options.SortOrder);
        case PurchaseRequestSortType.Description:
          return input.OrderBy(i => i.Description, options.SortOrder);
        case PurchaseRequestSortType.LatestBaseEntityDocumentDescription:
          return input.OrderBy(i => i.LatestBaseEntityDocumentDescription, options.SortOrder);
        case PurchaseRequestSortType.LatestBaseEntityDocumentDateTime:
          return input.OrderBy(i => i.LatestBaseEntityDocumentDateTime, options.SortOrder);
        case PurchaseRequestSortType.Deadline:
          return input.OrderBy(i => i.Deadline, options.SortOrder);
        case PurchaseRequestSortType.OrderedQty:
          return input.OrderBy(i => i.OrderedQty, options.SortOrder);
        case PurchaseRequestSortType.Qty:
          return input.OrderBy(i => i.Qty, options.SortOrder);
        case PurchaseRequestSortType.RemainedQty:
          return input.OrderBy(i => i.RemainedQty, options.SortOrder);
        case PurchaseRequestSortType.RequestCode:
          return input.OrderBy(i => i.RequestCode, options.SortOrder);
        case PurchaseRequestSortType.RequestDate:
          return input.OrderBy(i => i.RequestDate, options.SortOrder);
        case PurchaseRequestSortType.Status:
          return input.OrderBy(i => i.Status, options.SortOrder);
        case PurchaseRequestSortType.ConfirmerFullName:
          return input.OrderBy(i => i.ConfirmerFullName, options.SortOrder);
        case PurchaseRequestSortType.StuffCode:
          return input.OrderBy(i => i.StuffCode, options.SortOrder);
        case PurchaseRequestSortType.StuffName:
          return input.OrderBy(i => i.StuffName, options.SortOrder);
        case PurchaseRequestSortType.UnitName:
          return input.OrderBy(i => i.UnitName, options.SortOrder);
        case PurchaseRequestSortType.StuffCategoryName:
          return input.OrderBy(i => i.StuffCategoryName, options.SortOrder);
        case PurchaseRequestSortType.EmployeeFullName:
          return input.OrderBy(i => i.EmployeeFullName, options.SortOrder);
        case PurchaseRequestSortType.CargoedQty:
          return input.OrderBy(i => i.CargoedQty, options.SortOrder);
        case PurchaseRequestSortType.NotCargoedQty:
          return input.OrderBy(i => i.NotCargoedQty, options.SortOrder);
        case PurchaseRequestSortType.ReceiptedQty:
          return input.OrderBy(i => i.ReceiptedQty, options.SortOrder);
        case PurchaseRequestSortType.NoneReceiptedQty:
          return input.OrderBy(i => i.NoneReceiptedQty, options.SortOrder);
        case PurchaseRequestSortType.QualityControlPassedQty:
          return input.OrderBy(i => i.QualityControlPassedQty, options.SortOrder);
        case PurchaseRequestSortType.QualityControlFailedQty:
          return input.OrderBy(i => i.QualityControlFailedQty, options.SortOrder);
        case PurchaseRequestSortType.DepartmentName:
          return input.OrderBy(i => i.DepartmentName, options.SortOrder);
        case PurchaseRequestSortType.ResponsibleEmployeeFullName:
          return input.OrderBy(i => i.ResponsibleEmployeeFullName, options.SortOrder);
        case PurchaseRequestSortType.PlanCode:
          return input.OrderBy(i => i.PlanCode, options.SortOrder);
        case PurchaseRequestSortType.ProjectCode:
          return input.OrderBy(i => i.ProjectCode, options.SortOrder);
        case PurchaseRequestSortType.IsArchived:
          return input.OrderBy(i => i.IsArchived, options.SortOrder);
        case PurchaseRequestSortType.PurchaseRequestStepName:
          return input.OrderBy(i => i.PurchaseRequestStepName, options.SortOrder);
        case PurchaseRequestSortType.PurchaseRequestStepDetailDescription:
          return input.OrderBy(i => i.PurchaseRequestStepDetailDescription, options.SortOrder);
        case PurchaseRequestSortType.PurchaseRequestStepChangeTime:
          return input.OrderBy(i => i.PurchaseRequestStepChangeTime, options.SortOrder);
        case PurchaseRequestSortType.GrossWeight:
          return input.OrderBy(i => i.GrossWeight, options.SortOrder);
        case PurchaseRequestSortType.NetWeight:
          return input.OrderBy(i => i.NetWeight, options.SortOrder);
        case PurchaseRequestSortType.RiskLevelStatus:
          return input.OrderBy(i => i.RiskLevelStatus, options.SortOrder);
        case PurchaseRequestSortType.CurrentStuffBasePrice:
          return input.OrderBy(i => i.CurrentStuffBasePrice, options.SortOrder);
        case PurchaseRequestSortType.CurrentStuffBasePriceCurrencyTitle:
          return input.OrderBy(i => i.CurrentStuffBasePriceCurrencyTitle, options.SortOrder);
        case PurchaseRequestSortType.StuffPurchaseCategoryName:
          return input.OrderBy(i => i.StuffPurchaseCategoryName, options.SortOrder);
        case PurchaseRequestSortType.CostCenterName:
          return input.OrderBy(i => i.CostCenterName, options.SortOrder);
        case PurchaseRequestSortType.Essential:
          return input.OrderBy(i => i.Essential, options.SortOrder);
        case PurchaseRequestSortType.LatestRiskTitle:
          return input.OrderBy(i => i.LatestRiskTitle, options.SortOrder);
        case PurchaseRequestSortType.LatestRiskCreateDateTime:
          return input.OrderBy(i => i.LatestRiskCreateDateTime, options.SortOrder);
        case PurchaseRequestSortType.StuffType:
          return input.OrderBy(i => i.StuffType, options.SortOrder);
        case PurchaseRequestSortType.SupplyType:
          return input.OrderBy(i => i.SupplyType, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToPurchaseRequestResult
    public IQueryable<PurchaseRequestResult> ToPurchaseRequestResult(IQueryable<PurchaseRequest> purchaseRequests)
    {
      var confirmations = purchaseRequests.SelectMany(i => i.BaseEntityConfirmations)
          .Where(i => i.IsDelete == false);
      var result = from purchaseRequest in purchaseRequests
                   join tConfirm in confirmations on purchaseRequest.Id equals tConfirm.ConfirmingEntityId into tConfirmations
                   from confirm in tConfirmations.DefaultIfEmpty()
                   select new PurchaseRequestResult
                   {
                     Id = purchaseRequest.Id,
                     Deadline = purchaseRequest.Deadline,
                     ConfirmDate = confirm.ConfirmDateTime,
                     ConfirmerEmployeeId = confirm.Confirmer.Employee.Id,
                     ConfirmerFullName = confirm.Confirmer.Employee.FirstName + " " + confirm.Confirmer.Employee.LastName,
                     Qty = purchaseRequest.Qty,
                     RequestQty = purchaseRequest.RequestQty,
                     RequestCode = purchaseRequest.Code,
                     PlanCode = purchaseRequest.PlanCode.Code,
                     PlanCodeId = purchaseRequest.PlanCode.Id,
                     ProjectCode = purchaseRequest.ProjectCode,
                     RequestDate = purchaseRequest.DateTime,
                     StuffId = purchaseRequest.StuffId,
                     StuffCode = purchaseRequest.Stuff.Code,
                     StuffName = purchaseRequest.Stuff.Name,
                     StuffNoun = purchaseRequest.Stuff.Noun,
                     UnitId = purchaseRequest.UnitId,
                     UnitTypeId = purchaseRequest.Unit.UnitTypeId,
                     UnitName = purchaseRequest.Unit.Name,
                     Description = purchaseRequest.Description,
                     ConfirmationDescription = confirm.ConfirmDescription,
                     RemainedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.OrderedQty,
                     StuffCategoryId = purchaseRequest.Stuff.StuffCategoryId,
                     StuffCategoryName = purchaseRequest.Stuff.StuffCategory.Name,
                     StuffCategoryParentId = purchaseRequest.Stuff.StuffCategory.ParentStuffCategoryId,
                     StuffCategoryParentName = purchaseRequest.Stuff.StuffCategory.ParentStuffCategory.Name,
                     Status = purchaseRequest.Status,
                     EmployeeFullName = purchaseRequest.User.Employee.FirstName + " " + purchaseRequest.User.Employee.LastName,
                     DepartmentId = purchaseRequest.User.Employee.DepartmentId,
                     DepartmentName = purchaseRequest.User.Employee.Department.Name,
                     ResponsibleEmployeeId = purchaseRequest.ResponsibleEmployeeId,
                     ResponsibleEmployeeFullName = purchaseRequest.ResponsibleEmployee.FirstName + " " + purchaseRequest.ResponsibleEmployee.LastName,
                     CargoedQty = purchaseRequest.PurchaseRequestSummary.CargoedQty,
                     NotCargoedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.CargoedQty,
                     OrderedQty = purchaseRequest.PurchaseRequestSummary.OrderedQty,
                     QualityControlPassedQty = purchaseRequest.PurchaseRequestSummary.QualityControlPassedQty,
                     QualityControlFailedQty = purchaseRequest.PurchaseRequestSummary.QualityControlFailedQty,
                     ReceiptedQty = purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                     NoneReceiptedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                     EmployeeRequesterId = purchaseRequest.EmployeeRequester.Id,
                     Link = purchaseRequest.Link,
                     EmployeeRequesterFullName = purchaseRequest.EmployeeRequester.FirstName + " " + purchaseRequest.EmployeeRequester.LastName,
                     DocumentId = purchaseRequest.DocumentId,
                     SupplyType = purchaseRequest.SupplyType,
                     RowVersion = purchaseRequest.RowVersion
                   };
      return result;
    }
    #endregion
    #region ToPurchaseRequestResultQuery
    public IQueryable<PurchaseRequestResult> ToPurchaseRequestResultQuery(
        IQueryable<PurchaseRequest> purchaseRequests,
        IQueryable<BaseEntityDocument> latestBaseEntityDocuments,
        IQueryable<CargoItem> cargoItems,
        IQueryable<StuffPrice> stuffPrices)
    {
      var confirmations = purchaseRequests.SelectMany(i => i.BaseEntityConfirmations)
          .Where(i => i.IsDelete == false);
      var cargoItemm = from purchaseRequest in purchaseRequests
                       from purchaseOrderD in purchaseRequest.PurchaseOrderDetails
                       from cargoItemD in purchaseOrderD.CargoItemDetails
                       join cargoItem in cargoItems on cargoItemD.CargoItemId equals cargoItem.Id
                       group new { purchaseRequest, cargoItem } by purchaseRequest.Id into pod
                       select new
                       {
                         MaxEstimateDateTime = pod.Max(i => i.cargoItem.EstimateDateTime),
                         PurchaseRequestId = pod.Key
                       };
      var currentStuffBasePrices = from stuffPrice in stuffPrices
                                   where stuffPrice.Type == StuffPriceType.BasePrice && stuffPrice.Status.HasFlag(StuffPriceStatus.Current)
                                   select new
                                   {
                                     StuffId = stuffPrice.StuffId,
                                     Price = stuffPrice.Price,
                                     Currency = stuffPrice.Currency
                                   };
      var result = from purchaseRequest in purchaseRequests
                   join tConfirm in confirmations on purchaseRequest.Id equals tConfirm.ConfirmingEntityId into tConfirmations
                   from confirm in tConfirmations.DefaultIfEmpty()
                   join latestBaseEntityDocument in latestBaseEntityDocuments
                   on purchaseRequest.Id equals latestBaseEntityDocument.BaseEntityId
                   into tLatestBaseEntityDocuments
                   from latestBaseEntityDocument in tLatestBaseEntityDocuments.DefaultIfEmpty()
                   join cargoItem in cargoItemm on purchaseRequest.Id equals cargoItem.PurchaseRequestId
                   into tCargoItems
                   from cargoItem in tCargoItems.DefaultIfEmpty()
                   join currentStuffBasePrice in currentStuffBasePrices on purchaseRequest.StuffId equals currentStuffBasePrice.StuffId
                   into tCurrentStuffBasePrices
                   from currentStuffBasePrice in tCurrentStuffBasePrices.DefaultIfEmpty()
                   select new PurchaseRequestResult
                   {
                     Id = purchaseRequest.Id,
                     Deadline = purchaseRequest.Deadline,
                     ConfirmDate = confirm.ConfirmDateTime,
                     ConfirmerFullName = confirm.Confirmer.Employee.FirstName + " " + confirm.Confirmer.Employee.LastName,
                     Qty = purchaseRequest.Qty,
                     RequestQty = purchaseRequest.RequestQty,
                     PlanCode = purchaseRequest.PlanCode.Code,
                     PlanCodeId = purchaseRequest.PlanCode.Id,
                     CostCenterId = purchaseRequest.CostCenterId,
                     CostCenterName = purchaseRequest.CostCenter.Name,
                     CostCenterDescription = purchaseRequest.CostCenter.Description,
                     ProjectCode = purchaseRequest.ProjectCode,
                     RequestCode = purchaseRequest.Code,
                     RequestDate = purchaseRequest.DateTime,
                     StuffId = purchaseRequest.StuffId,
                     StuffCode = purchaseRequest.Stuff.Code,
                     StuffName = purchaseRequest.Stuff.Name,
                     StuffType = purchaseRequest.Stuff.StuffType,
                     StuffNoun = purchaseRequest.Stuff.Noun,
                     StuffPurchaseCategoryName = purchaseRequest.Stuff.StuffPurchaseCategory.Title,
                     UnitId = purchaseRequest.UnitId,
                     UnitTypeId = purchaseRequest.Unit.UnitTypeId,
                     DecimalDigitCount = purchaseRequest.Unit.DecimalDigitCount,
                     UnitName = purchaseRequest.Unit.Name,
                     ConversionRatio = purchaseRequest.Unit.ConversionRatio,
                     Description = purchaseRequest.Description,
                     ConfirmationDescription = confirm.ConfirmDescription,
                     RemainedQty = Math.Round(purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.OrderedQty, purchaseRequest.Unit.DecimalDigitCount),
                     StuffCategoryId = purchaseRequest.Stuff.StuffCategoryId,
                     StuffCategoryName = purchaseRequest.Stuff.StuffCategory.Name,
                     StuffCategoryParentId = purchaseRequest.Stuff.StuffCategory.ParentStuffCategoryId,
                     StuffCategoryParentName = purchaseRequest.Stuff.StuffCategory.ParentStuffCategory.Name,
                     Status = purchaseRequest.Status,
                     EmployeeFullName = purchaseRequest.User.Employee == null ? null : (purchaseRequest.User.Employee.FirstName + " " + purchaseRequest.User.Employee.LastName),
                     DepartmentId = purchaseRequest.User.Employee.DepartmentId,
                     DepartmentName = purchaseRequest.User.Employee.Department.Name,
                     ResponsibleEmployeeId = purchaseRequest.ResponsibleEmployeeId,
                     ResponsibleEmployeeFullName = purchaseRequest.ResponsibleEmployee == null ? null : (purchaseRequest.ResponsibleEmployee.FirstName + " " + purchaseRequest.ResponsibleEmployee.LastName),
                     CargoedQty = purchaseRequest.PurchaseRequestSummary.CargoedQty,
                     NotCargoedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.CargoedQty,
                     OrderedQty = purchaseRequest.PurchaseRequestSummary.OrderedQty,
                     QualityControlPassedQty = purchaseRequest.PurchaseRequestSummary.QualityControlPassedQty,
                     QualityControlFailedQty = purchaseRequest.PurchaseRequestSummary.QualityControlFailedQty,
                     ReceiptedQty = purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                     NoneReceiptedQty = Math.Round(purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.ReceiptedQty, purchaseRequest.Unit.DecimalDigitCount),
                     LatestBaseEntityDocumentDescription = latestBaseEntityDocument.Description,
                     LatestBaseEntityDocumentDateTime = latestBaseEntityDocument.DateTime,
                     IsArchived = purchaseRequest.IsArchived,
                     RowVersion = purchaseRequest.RowVersion,
                     PurchaseRequestStepDetailId = purchaseRequest.PurchaseRequestStepDetailId,
                     PurchaseRequestStepChangeTime = purchaseRequest.PurchaseRequestStepDetail.DateTime,
                     PurchaseRequestStepChangeUserFullName = purchaseRequest.PurchaseRequestStepDetail.User.Employee.FirstName + " " +
                       purchaseRequest.PurchaseRequestStepDetail.User.Employee.LastName,
                     PurchaseRequestStepId = purchaseRequest.PurchaseRequestStepDetail.PurchaseRequestStepId,
                     PurchaseRequestStepName = purchaseRequest.PurchaseRequestStepDetail.PurchaseRequestStep.Name,
                     GrossWeight = (double)purchaseRequest.Stuff.GrossWeight,
                     NetWeight = (double)purchaseRequest.Stuff.NetWeight,
                     MaxEstimateDateTime = cargoItem.MaxEstimateDateTime,
                     PurchaseRequestStepDetailDescription = purchaseRequest.PurchaseRequestStepDetail.Description,
                     RiskLevelStatus = purchaseRequest.LatestRisk == null ? RiskLevelStatus.Low : purchaseRequest.LatestRisk.LatestRiskStatus.RiskParameter.RiskLevelStatus,
                     LatestRiskTitle = purchaseRequest.LatestRisk.Title,
                     LatestRiskCreateDateTime = purchaseRequest.LatestRisk.CreateDateTime,
                     CurrentStuffBasePrice = currentStuffBasePrice.Price,
                     CurrentStuffBasePriceCurrencyId = currentStuffBasePrice.Currency.Id,
                     CurrentStuffBasePriceCurrencyTitle = currentStuffBasePrice.Currency.Title,
                     Essential = purchaseRequest.Essential,
                     DocumentId = purchaseRequest.DocumentId,
                     Link = purchaseRequest.Link,
                     EmployeeRequesterFullName = purchaseRequest.EmployeeRequester.FirstName + " " + purchaseRequest.EmployeeRequester.LastName,
                     EmployeeRequesterId = purchaseRequest.EmployeeRequesterId,
                     SupplyType = purchaseRequest.SupplyType
                   };
      return result;
    }
    #endregion
    #region ResetStatus
    public PurchaseRequest ResetPurchaseRequestStatus(int id)
    {
      #region GetPurchaseRequest
      var purchaseRequest = GetPurchaseRequest(id: id);
      #endregion
      return ResetPurchaseRequestStatus(purchaseRequest: purchaseRequest);
    }
    public PurchaseRequest ResetPurchaseRequestStatus(PurchaseRequest purchaseRequest)
    {
      #region ResetPurchaseRequestSummary
      var purchaseRequestSummary = ResetPurchaseRequestSummaryByPurchaseRequestId(
      purchaseRequestId: purchaseRequest.Id);
      #endregion
      #region Define Status
      var status = PurchaseRequestStatus.None;
      #region SetCurrentStatus
      if (purchaseRequest.Status.HasFlag(PurchaseRequestStatus.Accepted))
        status = status | PurchaseRequestStatus.Accepted;
      if (purchaseRequest.Status.HasFlag(PurchaseRequestStatus.Rejected))
        status = status | PurchaseRequestStatus.Rejected;
      if (purchaseRequest.Status.HasFlag(PurchaseRequestStatus.Waiting))
        status = status | PurchaseRequestStatus.Waiting;
      if (purchaseRequest.Status.HasFlag(PurchaseRequestStatus.Canceled))
        status = status | PurchaseRequestStatus.Canceled;
      #endregion
      if (purchaseRequestSummary.OrderedQty > 0)
      {
        if (purchaseRequestSummary.OrderedQty >= purchaseRequestSummary.PurchaseRequest.Qty)
          status = status | PurchaseRequestStatus.Ordered;
        else
          status = status | PurchaseRequestStatus.Ordering;
      }
      if (purchaseRequestSummary.CargoedQty > 0)
      {
        if (purchaseRequestSummary.CargoedQty >= purchaseRequestSummary.PurchaseRequest.Qty)
          status = status | PurchaseRequestStatus.Cargoed;
        else
          status = status | PurchaseRequestStatus.Cargoing;
      }
      if (purchaseRequestSummary.ReceiptedQty > 0)
      {
        if (purchaseRequestSummary.ReceiptedQty >= purchaseRequestSummary.PurchaseRequest.Qty)
          status = status | PurchaseRequestStatus.Receipted;
        else
          status = status | PurchaseRequestStatus.Receipting;
      }
      if (purchaseRequestSummary.QualityControlPassedQty > 0)
      {
        if (purchaseRequestSummary.QualityControlPassedQty >= purchaseRequestSummary.PurchaseRequest.Qty)
          status = status | PurchaseRequestStatus.QualityControled;
        else
          status = status | PurchaseRequestStatus.QualityControling;
      }
      #endregion
      #region Edit PurchaseRequest
      if (status != purchaseRequest.Status)
        EditPurchaseRequest(
                      purchaseRequest: purchaseRequest,
                      rowVersion: purchaseRequest.RowVersion,
                      status: status);
      #endregion
      return purchaseRequest;
    }
    #endregion
    #region CancelPurchaseRequest
    public PurchaseRequest CancelPurchaseRequestProcess(
        int id,
        byte[] rowVersion,
        string description)
    {
      var purchaseRequest = GetPurchaseRequest(id: id);
      return CancelPurchaseRequestProcess(
                    purchaseRequest: purchaseRequest,
                    rowVersion: rowVersion,
                    description: description);
    }
    public PurchaseRequest CancelPurchaseRequestProcess(
        PurchaseRequest purchaseRequest,
        byte[] rowVersion,
        string description)
    {
      var stuff = App.Internals.SaleManagement.GetStuff(purchaseRequest.StuffId);
      if (stuff.StuffPurchaseCategoryId != null)
      {
        var catDetails = App.Internals.Supplies.GetStuffPurchaseCategoryDetails(e => e, stuffPurchaseCategoryId: stuff.StuffPurchaseCategoryId.Value);
        var applicatorConfirmerIds = catDetails.Select(i => i.ApplicatorUserGroupId).ToArray();
        var memberships = App.Internals.UserManagement.GetMemberships(
                  selector: e => e,
                  userId: App.Providers.Security.CurrentLoginData.UserId,
                  userGroupIds: applicatorConfirmerIds);
        var userGroups = App.Internals.UserManagement.GetUserGroups(ids: applicatorConfirmerIds)
                  .Select(i => i.Name);
        if (!memberships.Any())
          throw new NotHavePermissionToStuffPurchaseRequestException(userGroups: string.Join("، ", userGroups));
      }
      #region Set Status
      var status = PurchaseRequestStatus.Accepted | PurchaseRequestStatus.Canceled;
      EditPurchaseRequest(
            purchaseRequest: purchaseRequest,
            rowVersion: purchaseRequest.RowVersion,
            status: status);
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
      baseEntityId: purchaseRequest.Id,
      scrumTaskType: ScrumTaskTypes.ConfirmPurchaseRequest);
      #endregion
      //check projectWork not null
      if (projectWorkItem != null)
      {
        #region DoneTask
        App.Internals.ScrumManagement.DoneScrumTask(
            scrumTask: projectWorkItem,
            rowVersion: projectWorkItem.RowVersion);
        #endregion
      }
      #region RemoveTransactionBatch
      App.Internals.WarehouseManagement.RemoveTransactionBatchProcess(
      oldTransactionBathId: purchaseRequest.TransactionBatch.Id,
      newTransactionBatchId: null);
      #endregion
      return purchaseRequest;
    }
    #endregion
    #region AcceptPurchaseRequest
    public PurchaseRequest AcceptPurchaseRequestProcess(
        int id,
        byte[] rowVersion,
        string description,
        double qty)
    {
      var purchaseRequest = GetPurchaseRequest(id: id);
      var unit = App.Internals.ApplicationBase.GetUnit(id: purchaseRequest.UnitId);
      if ((purchaseRequest.Status & ((PurchaseRequestStatus)PurchaseReportStatus.Waiting | (PurchaseRequestStatus)PurchaseReportStatus.Accepted)) <= 0)
      {
        throw new PurchaseRequestInValidConfirmException(id);
      }
      return AcceptPurchaseRequestProcess(
                    purchaseRequest: purchaseRequest,
                    rowVersion: rowVersion,
                    description: description,
                    qty: Math.Round(qty, unit.DecimalDigitCount));
    }
    public PurchaseRequest AcceptPurchaseRequestProcess(
        PurchaseRequest purchaseRequest,
        byte[] rowVersion,
        string description,
        double qty)
    {
      var stuff = App.Internals.SaleManagement.GetStuff(purchaseRequest.StuffId);
      if (stuff.StuffPurchaseCategoryId != null)
      {
        var catDetails = App.Internals.Supplies.GetStuffPurchaseCategoryDetails(e => e, stuffPurchaseCategoryId: stuff.StuffPurchaseCategoryId.Value);
        var applicatorConfirmerIds = catDetails.Select(i => i.ApplicatorConfirmerUserGroupId).ToArray();
        var memberships = App.Internals.UserManagement.GetMemberships(
                  selector: e => e,
                  userId: App.Providers.Security.CurrentLoginData.UserId,
                  userGroupIds: applicatorConfirmerIds);
        var userGroups = App.Internals.UserManagement.GetUserGroups(ids: applicatorConfirmerIds)
                 .Select(i => i.Name);
        if (!memberships.Any())
          throw new NotHavePermissionToRejectOrConfirmStuffPurchaseRequestException(userGroups: string.Join("، ", userGroups));
      }
      #region Set Status
      EditPurchaseRequest(
      purchaseRequest: purchaseRequest,
      rowVersion: purchaseRequest.RowVersion,
      status: PurchaseRequestStatus.Accepted,
      qty: qty);
      #endregion
      #region AcceptBaseEntityConfirmation
      if (purchaseRequest.Status == PurchaseRequestStatus.Accepted)
      {
        foreach (var baseEntityConfirmation in purchaseRequest.BaseEntityConfirmations)
        {
          baseEntityConfirmation.IsDelete = true;
        };
      }
      App.Internals.Confirmation.AcceptBaseEntityConfirmation(
            baseEntityConfirmTypeId: Models.StaticData.StaticBaseEntityConfirmTypes.PurchaseRequestConfirmation.Id,
            confirmingEntityId: purchaseRequest.Id,
            confirmDescription: description);
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
      baseEntityId: purchaseRequest.Id,
      scrumTaskType: ScrumTaskTypes.ConfirmPurchaseRequest);
      #endregion
      //check projectWork not null
      if (projectWorkItem != null)
      {
        #region DoneTask
        App.Internals.ScrumManagement.DoneScrumTask(
            scrumTask: projectWorkItem,
            rowVersion: projectWorkItem.RowVersion);
        #endregion
      }
      if (purchaseRequest.RequestQty != qty)
      {
        #region RemoveTransactionBatch
        App.Internals.WarehouseManagement.RemoveTransactionBatchProcess(
            oldTransactionBathId: purchaseRequest.TransactionBatch.Id,
            newTransactionBatchId: purchaseRequest.TransactionBatch.Id);
        #endregion
        #region AddTransactionPlan
        App.Internals.WarehouseManagement.AddTransactionPlanProcess(
            transactionBatchId: purchaseRequest.TransactionBatch.Id,
            effectDateTime: purchaseRequest.Deadline,
            stuffId: purchaseRequest.StuffId,
            billOfMaterialVersion: null,
            stuffSerialCode: null,
            transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportPurchaseRequest.Id,
            amount: qty,
            unitId: purchaseRequest.UnitId,
            description: "",
            isEstimated: false,
            referenceTransaction: null);
        #endregion
      }
      #region Add PurchaseOrder ProjectWorkItem
      //check projectWork not null
      if (projectWorkItem != null)
      {
        #region Get DescriptionForTask
        var requestQty = purchaseRequest.RequestQty;
        var confirmationQty = qty;
        var stuffName = purchaseRequest.Stuff.Noun;
        #endregion
        App.Internals.ProjectManagement.AddProjectWorkItem(
                projectWorkItem: null,
                name: $"ثبت سفارش خرید {purchaseRequest.Stuff.Code} ",
                description: $"عنوان کالا:{stuffName}, مقدار درخواست:{requestQty}, مقدار تایید:{confirmationQty}",
                color: "",
                departmentId: (int)Departments.Supplies,
                estimatedTime: 10800,
                isCommit: false,
                scrumTaskTypeId: (int)ScrumTaskTypes.PurchaseOrder,
                userId: null,
                spentTime: 0,
                remainedTime: 0,
                scrumTaskStep: ScrumTaskStep.ToDo,
                projectWorkId: projectWorkItem.ScrumBackLogId,
                baseEntityId: purchaseRequest.Id);
      }
      #endregion
      return purchaseRequest;
    }
    #endregion
    #region RejectPurchaseRequest
    public PurchaseRequest RejectPurchaseRequestProcess(
        int id,
        byte[] rowVersion,
        string description)
    {
      var purchaseRequest = GetPurchaseRequest(id: id);
      return RejectPurchaseRequestProcess(
                    purchaseRequest: purchaseRequest,
                    rowVersion: rowVersion,
                    description: description);
    }
    public PurchaseRequest RejectPurchaseRequestProcess(
        PurchaseRequest purchaseRequest,
        byte[] rowVersion,
        string description)
    {
      var stuff = App.Internals.SaleManagement.GetStuff(purchaseRequest.StuffId);
      if (stuff.StuffPurchaseCategoryId != null)
      {
        var catDetails = App.Internals.Supplies.GetStuffPurchaseCategoryDetails(e => e, stuffPurchaseCategoryId: stuff.StuffPurchaseCategoryId.Value);
        var applicatorConfirmerIds = catDetails.Select(i => i.ApplicatorConfirmerUserGroupId).ToArray();
        var memberships = App.Internals.UserManagement.GetMemberships(
                  selector: e => e,
                  userId: App.Providers.Security.CurrentLoginData.UserId,
                  userGroupIds: applicatorConfirmerIds);
        var userGroups = App.Internals.UserManagement.GetUserGroups(ids: applicatorConfirmerIds)
                 .Select(i => i.Name);
        if (!memberships.Any())
          throw new NotHavePermissionToRejectOrConfirmStuffPurchaseRequestException(userGroups: string.Join("، ", userGroups));
      }
      #region SetStatus
      var result = EditPurchaseRequest(
      purchaseRequest: purchaseRequest,
      rowVersion: purchaseRequest.RowVersion,
      status: PurchaseRequestStatus.Rejected);
      #endregion
      #region RejectBaseEntityConfirmation
      App.Internals.Confirmation.RejectBaseEntityConfirmation(
      baseEntityConfirmTypeId: Models.StaticData.StaticBaseEntityConfirmTypes.PurchaseRequestConfirmation.Id,
      confirmingEntityId: purchaseRequest.Id,
      confirmDescription: description);
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
      baseEntityId: purchaseRequest.Id,
      scrumTaskType: ScrumTaskTypes.ConfirmPurchaseRequest);
      #endregion
      //check projectWork not null
      if (projectWorkItem != null)
      {
        #region DoneTask
        App.Internals.ScrumManagement.DoneScrumTask(
            scrumTask: projectWorkItem,
            rowVersion: projectWorkItem.RowVersion);
        #endregion
      }
      #region RemoveTransactionBatch
      App.Internals.WarehouseManagement.RemoveTransactionBatchProcess(
      oldTransactionBathId: purchaseRequest.TransactionBatch.Id,
      newTransactionBatchId: null);
      #endregion
      #region NotifyToUser
      App.Internals.Notification.NotifyToUser(
      userId: purchaseRequest.UserId,
      title: "رد درخواست خرید کد " + purchaseRequest.Code,
      description:
      $"درخواست خرید کالای {purchaseRequest.Stuff.Name}-{purchaseRequest.Stuff.Code}  رد شده است ",
      scrumEntityId: null);
      #endregion
      return result;
    }
    #endregion
    #region AcceptPurchaseRequests
    public void AcceptPurchaseRequestsProcess(AcceptPurchaseRequestInput[] acceptPurchaseRequests)
    {
      foreach (var item in acceptPurchaseRequests)
      {
        AcceptPurchaseRequestProcess(
                      id: item.Id,
                      rowVersion: item.RowVersion,
                      description: item.Description,
                      qty: item.Qty);
      }
    }
    #endregion
    #region RejectPurchaseRequests
    public void RejectPurchaseRequestsProcess(RejectPurchaseRequestInput[] rejectPurchaseRequests)
    {
      foreach (var item in rejectPurchaseRequests)
      {
        RejectPurchaseRequestProcess(
                      id: item.Id,
                      rowVersion: item.RowVersion,
                      description: item.Description);
      }
    }
    #endregion
  }
}
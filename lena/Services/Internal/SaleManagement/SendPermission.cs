using System;
//using System.Data.Entity;
//using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using lena.Services.Common;
using lena.Services.Common.Helpers;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.SaleManagement.SendPermission;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region AddSendPermission
    public SendPermission AddSendPermission(
       SendPermission sendPermission,
       TransactionBatch transactionBatch,
       string description,
       int exitReceiptRequestId,
       double qty,
       byte unitId,
       SendPermissionStatusType sendPermissionStatusType)
    {

      sendPermission = sendPermission ?? repository.Create<SendPermission>();
      sendPermission.SendPermissionStatusType = sendPermissionStatusType;
      sendPermission.ExitReceiptRequestId = exitReceiptRequestId;
      sendPermission.Qty = qty;
      sendPermission.UnitId = unitId;

      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: sendPermission,
                  transactionBatch: transactionBatch,
                  description: description);
      return sendPermission;
    }
    #endregion
    #region EditSendPermission
    public SendPermission EditSendPermission(
        int id,
        byte[] rowVersion,
        TValue<SendPermissionStatusType> sendPermissionStatusType = null,
        TValue<int> exitReceiptRequestId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<int> confirmerUserId = null,
        TValue<DateTime> confirmDate = null,
        TValue<string> description = null,
         TValue<string> confirmDescription = null
        )
    {

      var sendPermission = GetSendPermission(id: id);
      var updatedPermission = EditSendPermission(
                sendPermission: sendPermission,
                rowVersion: rowVersion,
                sendPermissionStatusType: sendPermissionStatusType,
                exitReceiptRequestId: exitReceiptRequestId,
                qty: qty,
                unitId: unitId,
                confirmerUserId: confirmerUserId,
                confirmDate: confirmDate,
                description: description,
                confirmDescription: confirmDescription);

      ResetExitReceiptRequestStatus(updatedPermission.ExitReceiptRequestId);

      return updatedPermission;
    }
    public SendPermission EditSendPermission(
        SendPermission sendPermission,
        byte[] rowVersion,
        TValue<SendPermissionStatusType> sendPermissionStatusType = null,
        TValue<int> exitReceiptRequestId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<int> confirmerUserId = null,
        TValue<DateTime> confirmDate = null,
        TValue<string> description = null,
        TValue<string> confirmDescription = null)
    {

      if (sendPermissionStatusType != null)
        sendPermission.SendPermissionStatusType = sendPermissionStatusType;
      //todo fix 
      //if (exitReceiptRequestId != null)
      //    sendPermission.ExitReceiptRequestId = exitReceiptRequestId;
      if (qty != null)
        sendPermission.Qty = qty;
      if (unitId != null)
        sendPermission.UnitId = unitId;
      if (confirmerUserId != null)
        sendPermission.ConfirmerUserId = confirmerUserId;
      if (confirmDate != null)
        sendPermission.ConfirmDate = confirmDate;
      if (confirmDescription != null)
        sendPermission.ConfirmDescription = confirmDescription;
      if (description != null)
        sendPermission.Description = description;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: sendPermission,
                    description: description,
                    rowVersion: sendPermission.RowVersion);
      return retValue as SendPermission;
    }
    #endregion
    #region GetSendPermission
    public SendPermission GetSendPermission(int id) => GetSendPermission(selector: e => e, id: id);
    public TResult GetSendPermission<TResult>(
        Expression<Func<SendPermission, TResult>> selector,
        int id)
    {

      var exitReceiptRequest = GetSendPermissions(selector: selector,
                    id: id)


                .FirstOrDefault();
      if (exitReceiptRequest == null)
        throw new SendPermissionNotFoundException(id);
      return exitReceiptRequest;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetSendPermissions<TResult>(
        Expression<Func<SendPermission, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<SendPermissionStatusType> sendPermissionStatusType = null,
        TValue<double> qty = null,
        TValue<double> sendedQty = null,
        TValue<double> preparingSendingQty = null,
        TValue<int> unitId = null,
        TValue<int> stuffId = null,
        TValue<int> customerId = null,
        TValue<int> orderItemId = null,
        TValue<int> exitReceiptRequestId = null,
        TValue<int> exitReceiptRequestTypeId = null,
        TValue<ExitReceiptRequestStatus[]> exitReceiptRequestStatuses = null,
        TValue<SendPermissionStatusType[]> sendPermissionStatusTypes = null,
        TValue<ExitReceiptRequestStatus[]> exitReceiptRequestNotHasStatuses = null,
        TValue<SendPermissionStatusType[]> sendPermissionStatusNotHasTypes = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var sendPermission = baseQuery.OfType<SendPermission>();

      if (sendPermissionStatusType != null)
        sendPermission = sendPermission.Where(r => r.SendPermissionStatusType.HasFlag(sendPermissionStatusType));


      if (qty != null)
        sendPermission = sendPermission.Where(r => r.Qty == qty);
      if (sendedQty != null)
        sendPermission = sendPermission.Where(r => r.SendPermissionSummary.SendedQty == sendedQty);
      if (preparingSendingQty != null)
        sendPermission = sendPermission.Where(r => r.SendPermissionSummary.PreparingSendingQty == preparingSendingQty);
      if (unitId != null)
        sendPermission = sendPermission.Where(i => i.UnitId == unitId);
      if (description != null)
        sendPermission = sendPermission.Where(i => i.Description == description);

      if (stuffId != null)
        sendPermission = sendPermission.Where(i => i.ExitReceiptRequest.StuffId == stuffId);
      if (customerId != null)
        sendPermission = sendPermission.Where(i => i.ExitReceiptRequest.CooperatorId == customerId);
      if (orderItemId != null)
        sendPermission = sendPermission.Where(i => (i.ExitReceiptRequest as OrderItemBlock).OrderItemId == orderItemId);

      if (exitReceiptRequestStatuses != null)
      {

        var s = ExitReceiptRequestStatus.None;
        foreach (var item in exitReceiptRequestStatuses.Value)
          s = s | item;
        sendPermission = sendPermission.Where(i => (i.ExitReceiptRequest.Status & s) > 0);
      }
      if (sendPermissionStatusTypes != null)
      {

        var s = SendPermissionStatusType.None;
        foreach (var item in sendPermissionStatusTypes.Value)
          s = s | item;
        sendPermission = sendPermission.Where(i => (i.SendPermissionStatusType & s) > 0);
      }

      if (exitReceiptRequestNotHasStatuses != null)
      {
        var s = ExitReceiptRequestStatus.None;
        foreach (var item in exitReceiptRequestNotHasStatuses.Value)
          s = s | item;
        sendPermission = sendPermission.Where(i => (i.ExitReceiptRequest.Status & s) == 0);
      }
      if (sendPermissionStatusNotHasTypes != null)
      {
        var s = SendPermissionStatusType.None;
        foreach (var item in sendPermissionStatusNotHasTypes.Value)
          s = s | item;
        sendPermission = sendPermission.Where(i => (i.SendPermissionStatusType & s) == 0);
      }

      if (exitReceiptRequestId != null)
        sendPermission = sendPermission.Where(r => r.ExitReceiptRequestId == exitReceiptRequestId);


      if (exitReceiptRequestTypeId != null)
        sendPermission = sendPermission.Where(i => i.ExitReceiptRequest.ExitReceiptRequestTypeId == exitReceiptRequestTypeId);

      if (fromDate != null)
        sendPermission = sendPermission.Where(i => i.DateTime >= fromDate);// i.DateTime) >= fromDate.Value.Date);
      if (toDate != null)
        sendPermission = sendPermission.Where(i => i.DateTime <= toDate); //i.DateTime) <= toDate.Value.Date);

      return sendPermission.Select(selector);
    }
    #endregion
    #region SortSendPermission
    public IOrderedQueryable<SendPermissionResult> SortSendPermissionResult(IQueryable<SendPermissionResult> input, SortInput<SendPermissionSortType> options)
    {

      switch (options.SortType)
      {
        case SendPermissionSortType.Id:
          return input.OrderBy(i => i.Id, options.SortOrder);
        case SendPermissionSortType.Code:
          return input.OrderBy(i => i.Code, options.SortOrder);
        case SendPermissionSortType.CustomerCode:
          return input.OrderBy(i => i.CustomerCode, options.SortOrder);
        case SendPermissionSortType.CustomerName:
          return input.OrderBy(i => i.CustomerName, options.SortOrder);
        case SendPermissionSortType.DateTime:
          return input.OrderBy(i => i.DateTime, options.SortOrder);
        case SendPermissionSortType.ExitReceiptRequestCode:
          return input.OrderBy(i => i.ExitReceiptRequestCode, options.SortOrder);
        case SendPermissionSortType.ExitReceiptRequestStatus:
          return input.OrderBy(i => i.ExitReceiptRequestStatus, options.SortOrder);
        case SendPermissionSortType.ExitReceiptRequestTypeTitle:
          return input.OrderBy(i => i.ExitReceiptRequestTypeTitle, options.SortOrder);
        case SendPermissionSortType.OrderItemCode:
          return input.OrderBy(i => i.OrderItemCode, options.SortOrder);
        case SendPermissionSortType.PreparingSendingQty:
          return input.OrderBy(i => i.PreparingSendingQty, options.SortOrder);
        case SendPermissionSortType.Qty:
          return input.OrderBy(i => i.Qty, options.SortOrder);
        case SendPermissionSortType.SendedQty:
          return input.OrderBy(i => i.SendedQty, options.SortOrder);
        case SendPermissionSortType.StuffCode:
          return input.OrderBy(i => i.StuffCode, options.SortOrder);
        case SendPermissionSortType.StuffName:
          return input.OrderBy(i => i.StuffName, options.SortOrder);
        case SendPermissionSortType.UnitName:
          return input.OrderBy(i => i.UnitName, options.SortOrder);
        case SendPermissionSortType.WarehouseName:
          return input.OrderBy(i => i.WarehouseName, options.SortOrder);
        case SendPermissionSortType.Status:
          return input.OrderBy(i => i.SendPermissionStatusType, options.SortOrder);
        case SendPermissionSortType.EmployeeFullName:
          return input.OrderBy(i => i.EmployeeFullName, options.SortOrder);
        case SendPermissionSortType.ConfirmerFullname:
          return input.OrderBy(i => i.ConfirmerFullName, options.SortOrder);
        case SendPermissionSortType.ConfirmDate:
          return input.OrderBy(i => i.ConfirmDate, options.SortOrder);
        case SendPermissionSortType.ConfirmDescription:
          return input.OrderBy(i => i.ConfirmDescription, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region SearchSendPermission
    public IQueryable<SendPermissionResult> SearchSendPermissionResult(
        IQueryable<SendPermissionResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        int? orderItemId)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Code.Contains(searchText) ||
                item.CustomerCode.Contains(searchText) ||
                item.CustomerName.Contains(searchText) ||
                item.StuffName.Contains(searchText) ||
                item.StuffCode.Contains(searchText) ||
                item.OrderItemCode.Contains(searchText) ||
                item.CustomerCode.Contains(searchText) ||
                item.WarehouseName.Contains(searchText) ||
                //todo fix
                //item.ExitReceiptRequestCode.Contains(searchText) ||
                item.UnitName.Contains(searchText)
                select item;
      }

      if (orderItemId != null)
      {
        query = query.Where(i => i.OrderItemId == orderItemId);
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region ToSendPermissionResult
    public Expression<Func<SendPermission, SendPermissionResult>> ToSendPermissionResult =
        sendPermission => new SendPermissionResult
        {
          Id = sendPermission.Id,
          Code = sendPermission.Code,
          OrderItemId = (sendPermission.ExitReceiptRequest as OrderItemBlock).OrderItemId,
          OrderItemCode = (sendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.Code,
          OrderItemQty = (sendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.Qty,
          OrderItemUnitId = (sendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.UnitId,
          OrderItemUnitName = (sendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.Unit.Name,
          ExitReceiptRequestId = sendPermission.ExitReceiptRequestId,
          ExitReceiptRequestCode = sendPermission.ExitReceiptRequest.Code,
          ExitReceiptRequestQty = sendPermission.ExitReceiptRequest.Qty,
          ExitReceiptRequestUnitId = sendPermission.ExitReceiptRequest.UnitId,
          ExitReceiptRequestUnitName = sendPermission.ExitReceiptRequest.Unit.Name,
          ExitReceiptRequestTypeId = sendPermission.ExitReceiptRequest.ExitReceiptRequestType.Id,
          ExitReceiptRequestTypeTitle = sendPermission.ExitReceiptRequest.ExitReceiptRequestType.Title,
          ExitReceiptRequestStatus = sendPermission.ExitReceiptRequest.Status,
          StuffId = sendPermission.ExitReceiptRequest.StuffId,
          StuffCode = sendPermission.ExitReceiptRequest.Stuff.Code,
          StuffName = sendPermission.ExitReceiptRequest.Stuff.Name,
          BillOfMaterialVersion = (sendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.BillOfMaterialVersion,
          BillOfMaterialTitle = (sendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.BillOfMaterial.Title,
          Qty = sendPermission.Qty,
          UnitId = sendPermission.UnitId,
          UnitName = sendPermission.Unit.Name,
          WarehouseId = sendPermission.ExitReceiptRequest.WarehouseId,
          WarehouseName = sendPermission.ExitReceiptRequest.Warehouse.Name,
          DateTime = sendPermission.DateTime,
          SendedQty = sendPermission.SendPermissionSummary.SendedQty,
          PreparingSendingQty = sendPermission.SendPermissionSummary.PreparingSendingQty,
          SendPermissionStatusType = sendPermission.SendPermissionStatusType,
          CustomerId = sendPermission.ExitReceiptRequest.CooperatorId,
          CustomerName = sendPermission.ExitReceiptRequest.Cooperator.Name,
          CustomerCode = sendPermission.ExitReceiptRequest.Cooperator.Code,
          DeliveryDate = (sendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.DeliveryDate,
          RequestDate = (sendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.RequestDate,
          OrderItemBlockCode = (sendPermission.ExitReceiptRequest as OrderItemBlock).Code,
          OrderItemBlockId = (sendPermission.ExitReceiptRequest as OrderItemBlock).Id,
          EmployeeFullName = sendPermission.User.Employee.FirstName + " " + sendPermission.User.Employee.LastName,
          ConfirmerFullName = sendPermission.Confirmer.Employee.FirstName + " " + sendPermission.Confirmer.Employee.LastName,
          ConfirmDate = sendPermission.ConfirmDate,
          ConfirmDescription = sendPermission.ConfirmDescription,
          RowVersion = sendPermission.RowVersion,
          CurrencyTitle = sendPermission.ExitReceiptRequest.PriceAnnunciationItem.Currency.Title,
          Price = sendPermission.ExitReceiptRequest.PriceAnnunciationItem.Price,
          TotalPrice = (sendPermission.ExitReceiptRequest.PriceAnnunciationItem.Price != null) ?
                         (sendPermission.ExitReceiptRequest.PriceAnnunciationItem.Price * sendPermission.Qty) : 0
        };

    #endregion
    #region ToSendPermissionComboResult
    public Expression<Func<SendPermission, SendPermissionComboResult>> ToSendPermissionComboResult =
    sendPermission => new SendPermissionComboResult()
    {
      Id = sendPermission.Id,
      Code = sendPermission.Code,
      DateTime = sendPermission.DateTime,
      Qty = sendPermission.Qty,
      UnitId = sendPermission.UnitId,
      UnitName = sendPermission.Unit.Name,
      RowVersion = sendPermission.RowVersion,
      SendPermissionStatusType = sendPermission.SendPermissionStatusType,
    };
    #endregion
    #region AddSendPermissionProcess
    public SendPermission AddSendPermissionProcess(
        int exitReceiptRequestId,
        double qty,
        byte unitId,
        string description)
    {

      var exitReceiptRequest = App.Internals.SaleManagement.GetExitReceiptRequest(id: exitReceiptRequestId);
      return AddSendPermissionProcess(
                    exitReceiptRequest: exitReceiptRequest,
                    qty: qty,
                    unitId: unitId,
                    description: description);
    }
    public SendPermission EditSendPermissionProcess(
        int id,
        byte[] rowVersion,
        int exitReceiptRequestId,
        double qty,
        byte unitId,
        string description
        )
    {

      var exitReceiptRequest = App.Internals.SaleManagement.GetExitReceiptRequest(id: exitReceiptRequestId);
      return EditSendPermission(
                    id: id,
                    rowVersion: rowVersion,
                    exitReceiptRequestId: exitReceiptRequestId,
                    qty: qty,
                    unitId: unitId,
                    description: description);
    }
    public SendPermission AddSendPermissionProcess(
                ExitReceiptRequest exitReceiptRequest,
                double qty,
                byte unitId,
                string description)
    {


      //var sendPermissions = GetSendPermissions(
      //    selector: ToSendPermissionResult,
      //    exitReceiptRequestId: exitReceiptRequest.Id)
      //    
      //;
      //var totalQty = sendPermissions.Sum(x => x.Qty)  + qty;

      //if(totalQty > exitReceiptRequest.Qty)
      //{
      //    throw new InValidSendPermissionQtyException(exitReceiptRequest.Id, totalQty);
      //}


      #region AddSendPermission
      var sendPermission = AddSendPermission(
           sendPermission: null,
           transactionBatch: null,
           description: description,
           exitReceiptRequestId: exitReceiptRequest.Id,
           qty: qty,
           unitId: unitId,
           sendPermissionStatusType: SendPermissionStatusType.Waiting
           );
      #endregion
      #region AddSendPermissionSummary
      AddSendPermissionSummary(
              preparingSendingQty: 0,
              sendedQty: 0,
              sendPermissionId: sendPermission.Id);
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
          baseEntityId: exitReceiptRequest.Id,
          scrumTaskType: ScrumTaskTypes.SendPermission);
      #endregion
      #region AddSendPermissionTask
      //check projectWork not null
      if (projectWorkItem != null)
      {
        App.Internals.ProjectManagement.AddProjectWorkItem(
                      projectWorkItem: null,
                      name: $"بررسی مجوز ارسال   {sendPermission.Code}",
                      description: "بررسی مشتری و سفارش از نظر مالی و تایید مجوز ارسال محصول به مشتری",
                      color: "",
                      departmentId: (int)Departments.Accounting,
                      estimatedTime: 1800,
                      isCommit: false,
                      scrumTaskTypeId: (int)ScrumTaskTypes.ConfirmSendPermission,
                      userId: null,
                      spentTime: 0,
                      remainedTime: 1800,
                      scrumTaskStep: ScrumTaskStep.ToDo,
                      projectWorkId: projectWorkItem.ScrumBackLogId,
                      baseEntityId: sendPermission.Id);
      }

      #endregion
      #region check SendPermissions and DoneProjectWorkItem
      CheckSendPermissionTask(sendPermission: sendPermission);
      #endregion
      #region ResetExitReceiptRequestStatus
      ResetExitReceiptRequestStatus(id: sendPermission.ExitReceiptRequestId);
      #endregion
      return sendPermission;
    }
    #endregion
    #region AddConfirmSendPermission
    public SendPermission AddConfirmSendPermission(
        int sendPermissionId,
        byte[] rowVersion,
        bool confirmed,
        string description)
    {


      #region Confirm SendPermission
      var sendPermission = App.Internals.SaleManagement.GetSendPermission(sendPermissionId);
      var accounting = App.Internals.Accounting;

      if (sendPermission.ExitReceiptRequest.PriceAnnunciationItem != null)
      {
        var priceAnnunciationItems = sendPermission.ExitReceiptRequest.PriceAnnunciationItem.PriceAnnunciation.PriceAnnunciationItems;
        var cooperatorId = sendPermission.ExitReceiptRequest.CooperatorId;
        var cooperatorFinancialAccounts = accounting.GetCooperatorFinancialAccounts(selector: e => e, cooperatorId: cooperatorId, currencyId: sendPermission.ExitReceiptRequest.PriceAnnunciationItem.CurrencyId);
        if (!cooperatorFinancialAccounts.Any())
        {
          throw new NotDefinedCooperatorFinancialAccountException(sendPermission.ExitReceiptRequest.Cooperator.Name);
        }

        foreach (var priceAnnunciationItem in priceAnnunciationItems)
        {

          PriceAnnunciationItemStatus status = confirmed ? PriceAnnunciationItemStatus.Accepted : PriceAnnunciationItemStatus.Rejected;
          string desc = confirmed ? "تایید اتوماتیک زمان تایید مجوز ارسال" : "رد اتوماتیک زمان رد مجوز ارسال";

          App.Internals.SaleManagement.EditPriceAnnunciationItemProcess(
                    id: priceAnnunciationItem.Id,
                    status: status,
                    description: desc
                    );
        }
      }

      sendPermission = ConfirmSendPermissionProcess(
                    sendPermission: sendPermission,
                    confirmed: confirmed,
                    description: description,
                    rowVersion: rowVersion);
      #endregion
      return sendPermission;
    }
    #endregion
    #region ConfrimSendPermissionProcess
    public SendPermission ConfirmSendPermissionProcess(
        SendPermission sendPermission,
        bool confirmed,
        string description,
        byte[] rowVersion
        )
    {

      int confirmerUserId = App.Providers.Security.CurrentLoginData.UserId;
      DateTime confirmDate = DateTime.UtcNow;
      if (confirmed)
      {
        var sendPermissions = GetSendPermissions(
              selector: ToSendPermissionResult,
              sendPermissionStatusType: SendPermissionStatusType.Accepted,
              exitReceiptRequestId: sendPermission.ExitReceiptRequestId,
              isDelete: false);

        var exitReceiptRequest = GetExitReceiptRequest(
                      selector: ToExitReceiptRequestResult,
                      id: sendPermission.ExitReceiptRequestId);

        var totalQty = (sendPermissions.Sum(x => x.Qty) ?? 0) + sendPermission.Qty;

        if (totalQty > exitReceiptRequest.Qty)
        {
          throw new InValidConfirmSendPermissionQtyException(sendPermission.ExitReceiptRequestId, totalQty);
        }
      }

      #region Edit SendPermission
      var newStatusType = confirmed ? SendPermissionStatusType.Accepted : SendPermissionStatusType.Rejected;
      EditSendPermission(
                    sendPermission: sendPermission,
                    rowVersion: rowVersion,
                    sendPermissionStatusType: newStatusType,
                    confirmerUserId: confirmerUserId,
                    confirmDate: confirmDate,
                    confirmDescription: description
                );
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
          baseEntityId: sendPermission.Id,
          scrumTaskType: ScrumTaskTypes.ConfirmSendPermission);
      #endregion
      if (confirmed)
      {
        #region Add ProjectWorkItem for PreparingSendig
        //check projectWork not null
        if (projectWorkItem != null)
        {
          #region Get DescriptionForTask
          var orderQty = sendPermission.Qty;
          var confirmerUserName = sendPermission.Confirmer.UserName;
          var exitReceiptRequest = App.Internals.SaleManagement.GetExitReceiptRequest(e =>
               new
               {
                 StuffNoun = e.Stuff.Noun,
                 WarehouseName = e.Warehouse.Name,
               },
                id: sendPermission.ExitReceiptRequestId
                    );

          #endregion
          App.Internals.ProjectManagement.AddProjectWorkItem(
                  projectWorkItem: null,
                  name: "آماده سازی ارسال",
                  description: $"برای مجور تایید شده سریال های مورد نظر برای ارسال را مشخص نمایید;کالای {exitReceiptRequest.StuffNoun}از انبار {exitReceiptRequest.WarehouseName}به تعداد {orderQty}سفارش و تایید کننده {confirmerUserName}",
                  color: "",
                  departmentId: (int)Departments.Warehouse,
                  estimatedTime: 10800,
                  isCommit: false,
                  scrumTaskTypeId: (int)ScrumTaskTypes.PreparingSendig,
                  userId: null,
                  spentTime: 0,
                  remainedTime: 0,
                  scrumTaskStep: ScrumTaskStep.ToDo,
                  projectWorkId: projectWorkItem.ScrumBackLogId,
                  baseEntityId: sendPermission.Id);
        }

        #endregion
      }
      #region DoneTask
      //check projectWork not null
      if (projectWorkItem != null)
      {
        App.Internals.ScrumManagement.DoneScrumTask(
                      scrumTask: projectWorkItem,
                      rowVersion: projectWorkItem.RowVersion);
      }

      #endregion
      #region CheckSendPermissionTask
      CheckSendPermissionTask(sendPermission: sendPermission);
      #endregion
      #region ResetExitReceiptRequestStatus
      ResetExitReceiptRequestStatus(id: sendPermission.ExitReceiptRequestId);
      #endregion
      return sendPermission;
    }


    #endregion
    #region CheckSendPermissionTask
    public void CheckSendPermissionTask(SendPermission sendPermission)
    {


      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: sendPermission.ExitReceiptRequestId,
              scrumTaskType: ScrumTaskTypes.SendPermission);
      if (projectWorkItem == null)
      {
        projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                  baseEntityId: sendPermission.ExitReceiptRequestId,
                  scrumTaskType: ScrumTaskTypes.SendPermission);
      }
      #endregion

      #region check SendPermissions and DoneProjectWorkItem
      var sendPermissionStatusTypes = new SendPermissionStatusType[]
      {
                    SendPermissionStatusType.Accepted ,
                    SendPermissionStatusType.Preparing ,
                    SendPermissionStatusType.Prepared
    };
      var sendPermissions = GetSendPermissions(selector: e => e,
                    exitReceiptRequestId: sendPermission.ExitReceiptRequestId,
                    sendPermissionStatusTypes: sendPermissionStatusTypes,
                    isDelete: false);
      var sendPermissionsAmount = sendPermissions.Any() ? sendPermissions.Sum(i => i.Qty * i.Unit.ConversionRatio) : 0d;
      var taskIsDone = sendPermissions.Any() ? sendPermissionsAmount >= sendPermission.ExitReceiptRequest.Qty * sendPermission.ExitReceiptRequest.Unit.ConversionRatio : false;
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
        #region Get DescriptionForTask
        var orderQty = sendPermission.Qty;
        var confirmerUserName = sendPermission.Confirmer.UserName;
        var exitReceiptRequest = App.Internals.SaleManagement.GetExitReceiptRequest(e => new
        {
          StuffNoun = e.Stuff.Noun,
          WarehouseName = e.Warehouse.Name,
        },
                  id: sendPermission.ExitReceiptRequestId);
        #endregion
        #region  AddNewTask

        App.Internals.ProjectManagement.AddProjectWorkItem(
                projectWorkItem: null,
                name: $"مجوز ارسال بلوکه {sendPermission.ExitReceiptRequest.Code}",
                description: $" ثبت مجوز برای ارسال محصول به مشتری; کالای {exitReceiptRequest.StuffNoun} به تعداد سفارش {orderQty} از انبار {exitReceiptRequest.WarehouseName} و تایید کننده{confirmerUserName}",
                color: "",
                departmentId: (int)Departments.Sales,
                estimatedTime: 1800,
                isCommit: false,
                scrumTaskTypeId: (int)ScrumTaskTypes.SendPermission,
                userId: null,
                spentTime: 0,
                remainedTime: 1800,
                scrumTaskStep: ScrumTaskStep.ToDo,
                projectWorkId: projectWorkItem.ScrumBackLogId,
                baseEntityId: sendPermission.ExitReceiptRequestId);
        #endregion
      }
      #endregion

    }

    public void CheckSendPermissionTask(int sendPermissionId)
    {

      var sendPermission = GetSendPermission(id: sendPermissionId); ; CheckSendPermissionTask(sendPermission: sendPermission);
    }
    #endregion
    #region ResetSendPermissionStatus

    public SendPermission ResetSendPermissionStatus(int id)
    {

      var sendPermission = GetSendPermission(id: id); ; return ResetSendPermissionStatus(sendPermission: sendPermission);
    }

    public SendPermission ResetSendPermissionStatus(SendPermission sendPermission)
    {

      var orderItemUnitConversionRatio = sendPermission.Unit.ConversionRatio;
      #region ResetSendPermissionSummary
      var sendPermissionSummary =
          ResetSendPermissionSummaryBySendPermissionId(sendPermissionId: sendPermission.Id);
      #endregion

      var blockQty = sendPermissionSummary.SendPermission.ExitReceiptRequest.Qty;
      #region Define Status

      var status = sendPermission.SendPermissionStatusType &
                   (SendPermissionStatusType.Waiting |
                    SendPermissionStatusType.Accepted |
                    SendPermissionStatusType.Rejected);





      if (sendPermissionSummary.PreparingSendingQty > 0)
      {
        if (sendPermissionSummary.PreparingSendingQty >= blockQty)
          status = status | SendPermissionStatusType.Prepared;
        else
          status = status | SendPermissionStatusType.Preparing;
      }
      if (sendPermissionSummary.SendedQty > 0)
      {
        if (sendPermissionSummary.SendedQty >= blockQty)
          status = status | SendPermissionStatusType.Sended;
        else
          status = status | SendPermissionStatusType.Sending;
      }
      if (status == SendPermissionStatusType.None)
        status = SendPermissionStatusType.Waiting;

      EditSendPermission(
                id: sendPermission.Id,
                rowVersion: sendPermission.RowVersion,
                sendPermissionStatusType: status);

      #endregion

      return sendPermission;
    }

    #endregion


    #region RemoveSendPermission
    public void RemoveSendPermission(
        int sendPermissionId,
        byte[] rowVersion)
    {

      var sendPermission = App.Internals.SaleManagement.GetSendPermission(
                    id: sendPermissionId);

      //if (sendPermission.SendPermissionStatusType != SendPermissionStatusType.Waiting &&
      //    sendPermission.SendPermissionStatusType != SendPermissionStatusType.Accepted)
      //{
      //    throw new SendPermissionCanNotDeleteException(sendPermission.Id);
      //}

      var preparingSendings = App.Internals.WarehouseManagement.GetPreparingSendings(
              selector: e => e,
              isDelete: false,
              sendPermissionId: sendPermission.Id);
      #region Calculate and Check PreparingSendingSumQty
      double preparingSendingSumQty = 0;
      foreach (var preparingSending in preparingSendings)
      {
        preparingSendingSumQty += preparingSending.Qty;
      }

      if (preparingSendingSumQty == sendPermission.Qty)
      {
        throw new PreparingSendingExistsException();
      }
      #endregion
      if (preparingSendingSumQty > 0)
      {
        #region EditSendPermissionProcess
        sendPermission = App.Internals.SaleManagement.EditSendPermissionProcess(
                  id: sendPermission.Id,
                  rowVersion: sendPermission.RowVersion,
                 exitReceiptRequestId: sendPermission.ExitReceiptRequestId,
                 qty: preparingSendingSumQty,
                 unitId: sendPermission.UnitId,
                 description: sendPermission.Description);
        #endregion

        #region ResetSendPermissionStatus
        App.Internals.SaleManagement.ResetSendPermissionStatus(id: sendPermission.Id);
        #endregion
        #region ResetExitReceiptRequestStatus
        App.Internals.SaleManagement.ResetExitReceiptRequestStatus(
           exitReceiptRequest: sendPermission.ExitReceiptRequest);
        #endregion
        App.Internals.SaleManagement.CheckSendPermissionTask(
                sendPermission: sendPermission);
      }

      //if (preparingSendings.Any(u => !u.IsDelete))
      //{
      //    throw new PreparingSendingExistsException();
      //}

      if (preparingSendingSumQty == 0)
      {

        #region AddTransactionBatch
        var newTransactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
        #endregion

        App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                transactionBatchId: newTransactionBatch.Id,
                baseEntity: sendPermission,
                rowVersion: rowVersion);

        App.Internals.SaleManagement.ResetExitReceiptRequestStatus(
                      exitReceiptRequest: sendPermission.ExitReceiptRequest);

        App.Internals.SaleManagement.CheckSendPermissionTask(
                      sendPermission: sendPermission);
      }
    }
    #endregion
  }
}

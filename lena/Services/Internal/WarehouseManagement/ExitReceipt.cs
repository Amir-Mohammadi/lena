using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.ExitReceipt;
using lena.Models.WarehouseManagement.Receipt;
using lena.Models.StaticData;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region  AddExitReceipt
    public ExitReceipt AddExitReceipt(
        ExitReceipt exitReceipt,
        TransactionBatch transactionBatch,
        bool? confirmed,
        int customerId,
        string description)
    {

      exitReceipt = exitReceipt ?? repository.Create<ExitReceipt>();
      exitReceipt.Confirmed = confirmed;
      exitReceipt.CooperatorId = customerId;
      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: exitReceipt,
                  transactionBatch: transactionBatch,
                  description: description);
      return exitReceipt;
    }
    #endregion
    #region EditExitReceipt
    public ExitReceipt EditExitReceipt(
        int id,
        byte[] rowVersion,
        TValue<bool?> confirmed = null,
        TValue<string> description = null,
        TValue<int?> outboundCargoId = null,
        TValue<bool> isDelete = null)
    {

      var exitReceipt = GetExitReceipt(id: id);
      return EditExitReceipt(
                exitReceipt: exitReceipt,
                rowVersion: rowVersion,
                confirmed: confirmed,
                description: description,
                isDelete: isDelete);
    }
    public ExitReceipt EditExitReceipt(
        ExitReceipt exitReceipt,
        byte[] rowVersion,
        TValue<bool?> confirmed = null,
        TValue<string> description = null,
        TValue<int?> outboundCargoId = null,
        TValue<bool> isDelete = null)
    {

      if (confirmed != null)
        exitReceipt.Confirmed = confirmed;
      if (outboundCargoId != null)
        exitReceipt.OutboundCargoId = outboundCargoId;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: exitReceipt,
                    description: description,
                    isDelete: isDelete,
                    rowVersion: rowVersion);
      return retValue as ExitReceipt;
    }
    #endregion
    #region GetExitReceipt
    public ExitReceipt GetExitReceipt(int id) => GetExitReceipt(selector: e => e, id: id);
    public TResult GetExitReceipt<TResult>(
        Expression<Func<ExitReceipt, TResult>> selector,
        int id)
    {

      var orderItemBlock = GetExitReceipts(
                    selector: selector,
                    id: id,
                    isDelete: false)


                .FirstOrDefault();
      if (orderItemBlock == null)
        throw new ExitReceiptNotFoundException(id);
      return orderItemBlock;
    }
    public ExitReceipt GetExitReceipt(string code) => GetExitReceipt(selector: e => e, code: code);
    public TResult GetExitReceipt<TResult>(
        Expression<Func<ExitReceipt, TResult>> selector,
        string code)
    {

      var orderItemBlock = GetExitReceipts(
                    selector: selector,
                    code: code,
                    isDelete: false)


                .FirstOrDefault();
      if (orderItemBlock == null)
        throw new ExitReceiptNotFoundException(code);
      return orderItemBlock;
    }
    public ExitReceipt GetNotOutBoundCargo(string code) => GetNotOutBoundCargo(selector: e => e, code: code);
    public TResult GetNotOutBoundCargo<TResult>(
        Expression<Func<ExitReceipt, TResult>> selector,
        string code)
    {

      var exitReceipt = GetExitReceipts(
                    selector: selector,
                    code: code,
                    outboundCargoId: new TValue<int?>(null),
                    isDelete: false)


                .FirstOrDefault();
      if (exitReceipt == null)
        throw new OutBoundCargoNotFoundException(code);
      return exitReceipt;
    }
    public ExitReceipt GetNotOutBoundCargo(int id) => GetNotOutBoundCargo(selector: e => e, id: id);
    public TResult GetNotOutBoundCargo<TResult>(
        Expression<Func<ExitReceipt, TResult>> selector,
        int id)
    {

      var exitReceipt = GetExitReceipts(
                    selector: selector,
                    id: id,
                    outboundCargoId: new TValue<int?>(null),
                    isDelete: false)


                .FirstOrDefault();
      if (exitReceipt == null)
        throw new OutBoundCargoNotFoundException(id);
      return exitReceipt;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetExitReceipts<TResult>(
        Expression<Func<ExitReceipt, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<bool?> confirmed = null,
        TValue<int?> outboundCargoId = null,
        TValue<int> cooperatorId = null,
        TValue<int> stuffId = null,
        TValue<int> exitReceiptRequestId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var exitReceipt = baseQuery.OfType<ExitReceipt>();
      if (confirmed != null)
        exitReceipt = exitReceipt.Where(r => r.Confirmed == confirmed);
      if (outboundCargoId != null)
        exitReceipt = exitReceipt.Where(i => i.OutboundCargoId == outboundCargoId);
      if (cooperatorId != null)
        exitReceipt = exitReceipt.Where(i => i.CooperatorId == cooperatorId);
      if (exitReceiptRequestId != null)
        exitReceipt = exitReceipt.Where(i => i.SendProducts.Any(j => j.PreparingSending.SendPermission.ExitReceiptRequestId == exitReceiptRequestId));
      if (stuffId != null)
      {
        var exitReceiptIds = GetPreparingSendings(
                      selector: e => e.SendProduct.ExitReceiptId,
                      stuffId: stuffId);
        exitReceipt = from item in exitReceipt
                      join exitReceiptId in exitReceiptIds on item.Id equals exitReceiptId
                      select item;
      }
      return exitReceipt.Select(selector);
    }
    #endregion
    #region  AddExitReceiptProcess
    public ExitReceipt AddExitReceiptProcess(
        int[] preparingSendingIds)
    {

      var preparingSendings = GetPreparingSendings(x => x, ids: preparingSendingIds);
      var customerIds = preparingSendings.GroupBy(x => x.SendPermission.ExitReceiptRequest.CooperatorId).Select(x => x.Key).ToList();
      if (customerIds.Count > 1)
        throw new CanNotAddExitReceiptForMoreThanOneCustomerException();
      var customerId = customerIds.FirstOrDefault();
      #region AddTransactionBatch
      var transactionBatch = AddTransactionBatch();
      #endregion
      #region AddExitReceipt
      var exitReceipt = AddExitReceipt(
          exitReceipt: null,
          transactionBatch: transactionBatch,
          customerId: customerId,
          confirmed: null,
          description: null
          );
      #endregion
      #region AddPreparingSendings
      foreach (var preparingSendingId in preparingSendingIds)
      {
        AddSendProductProcess(
                  transactionBatch: transactionBatch,
                  preparingSendingId: preparingSendingId,
                  exitReceiptId: exitReceipt.Id,
                  description: null
              );
      }
      #endregion
      #region Add Task
      #region GetOrAddCommonProjectGroup
      var projectGroup = App.Internals.ScrumManagement.GetOrAddCommonScrumProjectGroup(
              departmentId: (int)Departments.Guard);
      #endregion
      #region GetOrAddCommonProject
      var project = App.Internals.ProjectManagement.GetOrAddCommonProject(
              departmentId: (int)Departments.Guard);
      #endregion
      #region GetOrAddCommonProjectStep
      var projectStep = App.Internals.ProjectManagement.GetOrAddCommonProjectStep(
              departmentId: (int)Departments.Guard);
      #endregion
      #region Add ProjectWork
      var projectWork = App.Internals.ProjectManagement.AddProjectWork(
              projectWork: null,
              name: $"پروسه محموله خروج برگه خروج {exitReceipt.Code}",
              description: "",
              color: "",
              departmentId: (int)Departments.Guard,
              estimatedTime: 18000,
              isCommit: false,
              projectStepId: projectStep.Id,
              baseEntityId: exitReceipt.Id
          );
      #endregion
      #region AddSaveOutboundCargoTask
      //check projectWork not null
      if (projectWork != null)
      {
        App.Internals.ProjectManagement.AddProjectWorkItem(
                      projectWorkItem: null,
                      name: $"ثبت محموله خروج {exitReceipt.Code}",
                      description: $"برای برگه خروج کد {exitReceipt.Code} محموله خروج ثبت کنید ",
                      color: "",
                      departmentId: (int)Departments.Guard,
                      estimatedTime: 10800,
                      isCommit: false,
                      scrumTaskTypeId: (int)ScrumTaskTypes.SaveOutboundCargo,
                      userId: null,
                      spentTime: 0,
                      remainedTime: 0,
                      scrumTaskStep: ScrumTaskStep.ToDo,
                      projectWorkId: projectWork.Id,
                      baseEntityId: exitReceipt.Id);
      }
      #endregion
      #endregion
      foreach (var preparingSending in preparingSendings)
      {
        ExitReceiptRequestType sendProductExitReceiptRequestType = preparingSending.SendPermission.ExitReceiptRequest.ExitReceiptRequestType;
        // فقط برای برگه های خروج نوع ضایعات و مرجوعی کار شده است
        int? financialTransactionTypeId = null;
        if (sendProductExitReceiptRequestType.Id == StaticExitReceiptRequestTypes.DisposalOfWasteExitReceiptRequest.Id)
          financialTransactionTypeId = StaticFinancialTransactionTypes.SaleOfWaste.Id;
        else if (sendProductExitReceiptRequestType.Id == StaticExitReceiptRequestTypes.GivebackExitReceiptRequest.Id)
          financialTransactionTypeId = StaticFinancialTransactionTypes.Giveback.Id;
        if (financialTransactionTypeId == null) continue;
        var sendProduct = preparingSending.SendProduct;
        var accounting = App.Internals.Accounting;
        ///حذف تراکنش های مالی قبلی
        if (sendProduct.FinancialTransactionBatch != null)
        {
          var financialTransactions = accounting.GetFinancialTransactions(selector: e => e, financialTransactionBatchId: sendProduct.FinancialTransactionBatch.Id);
          foreach (var financialTransaction in financialTransactions)
          {
            accounting.DeleteFinancialTransaction(financialTransaction);
          }
        }
        #region AddFinancialTransaction
        var cooperatorId = preparingSending.SendPermission.ExitReceiptRequest.CooperatorId;
        var cooperatorFinancialAccounts = accounting.GetCooperatorFinancialAccounts(selector: e => e, cooperatorId: cooperatorId, currencyId: sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.PriceAnnunciationItem.CurrencyId);
        if (!cooperatorFinancialAccounts.Any())
        {
          throw new NotDefinedCooperatorFinancialAccountException(preparingSending.SendPermission.ExitReceiptRequest.Cooperator.Name);
        }
        var cooperatorFinancialAccount = cooperatorFinancialAccounts.FirstOrDefault();
        #region AddFinancialTransactionBatch
        var financialTransactionBatch = accounting.AddFinancialTransactionBatch();
        #endregion
        var financialTransactionType = accounting.GetFinancialTransactionType(financialTransactionTypeId.Value);
        App.Internals.Accounting.AddFinancialTransactionProcess(
                      financialTransaction: null,
                      amount: preparingSending.SendPermission.ExitReceiptRequest.PriceAnnunciationItem.Price * preparingSending.SendPermission.ExitReceiptRequest.PriceAnnunciationItem.Count.Value,
                      effectDateTime: DateTime.Now.ToUniversalTime(),
                      description: " برگه خروج  " + exitReceipt.Id.ToString(),
                      financialAccountId: cooperatorFinancialAccount.Id,
                      financialTransactionType: financialTransactionType,
                      financialTransactionBatchId: financialTransactionBatch.Id,
                      referenceFinancialTransaction: null);
        #endregion
        App.Internals.ApplicationBase.EditBaseEntity(
                financialTransactionBatch: financialTransactionBatch,
                baseEntity: sendProduct,
                rowVersion: sendProduct.RowVersion);
      }
      return exitReceipt;
    }
    #endregion
    #region ConfirmExitReceipt
    public ExitReceipt ConfirmExitReceipt(
        int id,
        byte[] rowVersion,
        int outboundCargoId)
    {

      var exitReceipt = GetExitReceipt(id: id);
      return ConfirmExitReceipt(
                exitReceipt: exitReceipt,
                rowVersion: rowVersion,
                outboundCargoId: outboundCargoId);
    }
    public ExitReceipt ConfirmExitReceipt(
        ExitReceipt exitReceipt,
        byte[] rowVersion,
        int outboundCargoId)
    {

      #region EditExitReceipt
      exitReceipt = EditExitReceipt(
              exitReceipt: exitReceipt,
              rowVersion: rowVersion,
              confirmed: true,
              outboundCargoId: outboundCargoId);
      #endregion
      #region GetProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: exitReceipt.Id,
              scrumTaskType: ScrumTaskTypes.SaveOutboundCargo);
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
      return exitReceipt;
    }
    #endregion
    #region ConfirmExitReceipt
    public ExitReceipt UnConfirmExitReceipt(
        int id,
        byte[] rowVersion)
    {

      var exitReceipt = GetExitReceipt(id: id);
      return UnConfirmExitReceipt(
                exitReceipt: exitReceipt,
                rowVersion: rowVersion);
    }
    public ExitReceipt UnConfirmExitReceipt(
        ExitReceipt exitReceipt,
        byte[] rowVersion)
    {

      #region EditExitReceipt
      exitReceipt = EditExitReceipt(
              exitReceipt: exitReceipt,
              rowVersion: rowVersion,
              confirmed: new TValue<bool?>(null),
              outboundCargoId: new TValue<int?>(null));
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: exitReceipt.Id,
              scrumTaskType: ScrumTaskTypes.SaveOutboundCargo);
      if (projectWorkItem == null)
      {
        projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                  baseEntityId: exitReceipt.Id,
                  scrumTaskType: ScrumTaskTypes.SaveOutboundCargo);
      }
      #endregion
      #region Add New ProjectWorkItem if Done
      if (projectWorkItem != null && projectWorkItem.ScrumTaskStep == ScrumTaskStep.Done)
      {
        #region  AddNewTask
        App.Internals.ProjectManagement.AddProjectWorkItem(
                projectWorkItem: null,
                name: $"ثبت محموله خروج {exitReceipt.Code}",
                description: $"برای برگه خروج کد {exitReceipt.Code} محموله خروج ثبت کنید ",
                color: "",
                departmentId: (int)Departments.Guard,
                estimatedTime: 10800,
                isCommit: false,
                scrumTaskTypeId: (int)ScrumTaskTypes.SaveOutboundCargo,
                userId: null,
                spentTime: 0,
                remainedTime: 0,
                scrumTaskStep: ScrumTaskStep.ToDo,
                projectWorkId: projectWorkItem.ScrumBackLogId,
                baseEntityId: exitReceipt.Id);
        #endregion
      }
      #endregion
      return exitReceipt;
    }
    #endregion
    #region Search
    public IQueryable<ExitReceiptResult> SearchExitReceiptResult(
        IQueryable<ExitReceiptResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        DateTime? fromTransportDateTime = null,
        DateTime? toTransportDateTime = null,
        int? exitReceiptUserId = null,
        int? outboundCargoUserId = null,
        string shippingCompanyName = null,
        bool? confirmed = null,
        int? stuffId = null)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = query.Where(item =>
            item.Id.ToString().Contains(searchText) ||
            item.Code.Contains(searchText) ||
            item.ExitReceiptUserName.Contains(searchText) ||
            item.OutboundCargoCode.Contains(searchText) ||
            item.OutboundCargoUserName.Contains(searchText) ||
            item.ShippingCompanyName.Contains(searchText) ||
            item.DriverName.Contains(searchText) ||
            item.CarNumber.Contains(searchText) ||
            item.Description.Contains(searchText));
      if (fromDate != null)
        query = query.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        query = query.Where(i => i.DateTime <= toDate);
      if (fromTransportDateTime != null)
        query = query.Where(i => i.TransportDateTime >= fromTransportDateTime);
      if (toTransportDateTime != null)
        query = query.Where(i => i.TransportDateTime <= toTransportDateTime);
      if (exitReceiptUserId != null)
        query = query.Where(i => i.ExitReceiptUserId == exitReceiptUserId);
      if (outboundCargoUserId != null)
        query = query.Where(i => i.OutboundCargoUserId == outboundCargoUserId);
      if (shippingCompanyName != null)
        query = query.Where(i => i.ShippingCompanyName == shippingCompanyName);
      if (confirmed != null)
        query = query.Where(i => i.Confirmed == confirmed);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ExitReceiptResult> SortExitReceiptResult(
        IQueryable<ExitReceiptResult> query,
        SortInput<ExitReceiptSortType> sort)
    {
      switch (sort.SortType)
      {
        case ExitReceiptSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ExitReceiptSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ExitReceiptSortType.Confirmed:
          return query.OrderBy(a => a.Confirmed, sort.SortOrder);
        case ExitReceiptSortType.ExitReceiptUserName:
          return query.OrderBy(a => a.ExitReceiptUserName, sort.SortOrder);
        case ExitReceiptSortType.OutboundCargoCode:
          return query.OrderBy(a => a.OutboundCargoCode, sort.SortOrder);
        case ExitReceiptSortType.OutboundCargoUserName:
          return query.OrderBy(a => a.OutboundCargoUserName, sort.SortOrder);
        case ExitReceiptSortType.ShippingCompanyName:
          return query.OrderBy(a => a.ShippingCompanyName, sort.SortOrder);
        case ExitReceiptSortType.DriverName:
          return query.OrderBy(a => a.DriverName, sort.SortOrder);
        case ExitReceiptSortType.CarNumber:
          return query.OrderBy(a => a.CarNumber, sort.SortOrder);
        case ExitReceiptSortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case ExitReceiptSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case ExitReceiptSortType.TransportDateTime:
          return query.OrderBy(a => a.TransportDateTime, sort.SortOrder);
        case ExitReceiptSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToFullResult
    public Expression<Func<ExitReceipt, ExitReceiptFullResult>> ToExitReceiptFullResult =
        exitReceipt => new ExitReceiptFullResult
        {
          Id = exitReceipt.Id,
          ExitReceiptId = exitReceipt.Id,
          Code = exitReceipt.Code,
          Confirmed = exitReceipt.Confirmed,
          OutboundCargoId = exitReceipt.OutboundCargoId,
          OutboundCargoCode = exitReceipt.OutboundCargo.Code,
          DateTime = (DateTime?)exitReceipt.OutboundCargo.DateTime,
          TransportDateTime = (DateTime?)exitReceipt.OutboundCargo.TransportDateTime,
          CarNumber = exitReceipt.OutboundCargo.CarNumber,
          RowVersion = exitReceipt.RowVersion,
          SendProducts = exitReceipt.SendProducts.AsQueryable().Select(App.Internals.WarehouseManagement.ToSendProductFullResult)
        };
    #endregion
    #region ToResult
    public Expression<Func<ExitReceipt, ExitReceiptResult>> ToExitReceiptResult =
        exitReceipt => new ExitReceiptResult
        {
          Id = exitReceipt.Id,
          Code = exitReceipt.Code,
          Confirmed = exitReceipt.Confirmed,
          OutboundCargoId = exitReceipt.OutboundCargoId,
          OutboundCargoCode = exitReceipt.OutboundCargo.Code,
          DateTime = exitReceipt.DateTime,
          TransportDateTime = exitReceipt.OutboundCargo.TransportDateTime,
          CarNumber = exitReceipt.OutboundCargo.CarNumber,
          RowVersion = exitReceipt.RowVersion,
          Description = exitReceipt.Description,
          DriverName = exitReceipt.OutboundCargo.DriverName,
          ExitReceiptUserId = exitReceipt.UserId,
          ExitReceiptUserName = exitReceipt.User.Employee.FirstName + " " + exitReceipt.User.Employee.LastName,
          ShippingCompanyName = exitReceipt.OutboundCargo.ShippingCompanyName,
          OutboundCargoUserId = exitReceipt.OutboundCargo.UserId,
          OutboundCargoUserName = exitReceipt.OutboundCargo.User.Employee.FirstName + " " + exitReceipt.OutboundCargo.User.Employee.LastName,
          CooperatorId = exitReceipt.CooperatorId,
          CooperatorCode = exitReceipt.Cooperator.Code,
          CooperatorName = exitReceipt.Cooperator.Name
        };
    #endregion
    #region Delete
    public void DeleteExitReceipt(int exitReceiptId)
    {

      var exitReceipt = GetExitReceipt(exitReceiptId);
      EditExitReceipt(
                id: exitReceipt.Id,
                rowVersion: exitReceipt.RowVersion,
                isDelete: true);
    }
    #endregion
    #region DeleteProcess
    public void DeleteExitReceiptProcess(int exitReceiptDeleteRequestId)
    {

      var salesModule = App.Internals.SaleManagement;
      #region change outboundCargo status to not action
      var exitReceiptDeleteRequest = GetExitReceiptDeleteRequest(
          id: exitReceiptDeleteRequestId);
      var exitReceipt = GetExitReceipt(
                id: exitReceiptDeleteRequest.ExitReceiptId);
      if (exitReceipt.OutboundCargoId.HasValue)
      {
        var outboundCargos = App.Internals.Guard.GetOutboundCargo(
                  id: exitReceipt.OutboundCargoId.Value);
        App.Internals.Guard.EditOutboundCargo(
                  id: (int)exitReceipt.OutboundCargoId,
                  status: TransportStatus.NotAction,
                  rowVersion: outboundCargos.RowVersion);
      }
      #endregion
      #region Get Stuff Serials
      var exitReceiptDeleteRequestStuffSerials = GetExitReceiptDeleteRequestStuffSerials(
          selector: e => new
          {
            e.StuffSerialId,
            e.StuffSerialCode,
            e.StuffSerial.Serial,
            e.StuffSerial.BillOfMaterialVersion,
            e.UnitId,
            e.Amount
          },
          exitReceiptDeleteRequestId: exitReceiptDeleteRequestId);
      #region AddTransactionBatch
      var addTransactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      foreach (var stuffSerial in exitReceiptDeleteRequestStuffSerials)
      {
        #region check stuff serials in warehouseInventory
        var warehouseInventories = GetWarehouseInventories(
            stuffId: stuffSerial.StuffSerialId,
            stuffSerialCode: stuffSerial.StuffSerialCode);
        if (warehouseInventories.Any())
          throw new SerialHasInWarehouseInventoryException(stuffSerial.Serial);
        #endregion
        #region warehouseTransaction
        var lastTransaction = GetWarehouseTransactions(
            selector: e => new
            {
              e.Id,
              e.WarehouseId,
              e.EffectDateTime,
              e.BillOfMaterialVersion
            },
            serial: stuffSerial.Serial)


        .OrderByDescending(i => i.EffectDateTime)
        .FirstOrDefault();
        if (lastTransaction.WarehouseId == null)
          throw new BaseTransactionHasNoWarehouseException(id: lastTransaction.Id);
        short transactionTypeId = Models.StaticData.StaticTransactionTypes.ImportBlock.Id;
        AddWarehouseTransaction(
                  transactionBatchId: addTransactionBatch.Id,
                  effectDateTime: DateTime.Now.ToUniversalTime(),
                  stuffId: stuffSerial.StuffSerialId,
                  billOfMaterialVersion: lastTransaction.BillOfMaterialVersion,
                  stuffSerialCode: stuffSerial.StuffSerialCode,
                  warehouseId: lastTransaction.WarehouseId.Value,
                  transactionTypeId: transactionTypeId,
                  amount: stuffSerial.Amount,
                  unitId: stuffSerial.UnitId,
                  description: "حذف برگه خروج",
                  referenceTransaction: null);
        #endregion
      }
      #endregion
      #region removeProcess
      DeleteExitReceipt(
          exitReceiptId: exitReceipt.Id);
      #endregion
      #region delete sendProduct
      foreach (var sendProduct in exitReceipt.SendProducts.ToList())
      {
        salesModule.ResetSendPermissionStatus(
                  id: sendProduct.PreparingSending.SendPermissionId);
        var orderItemBlock = salesModule.GetOrderItemBlocks(
                  selector: e => e,
                  id: sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Id)

              .FirstOrDefault();
        if (orderItemBlock != null)
        {
          salesModule.ResetOrderItemStatus(id: orderItemBlock.OrderItemId);
        }
        EditPreparingSending(
                  id: sendProduct.PreparingSending.Id,
                  rowVersion: sendProduct.PreparingSending.RowVersion,
                  status: PreparingSendingStatus.Waiting);
        RemoveSendProduct(
                 sendProductId: sendProduct.Id);
      }
      #endregion
    }
    #endregion
  }
}
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using Microsoft.EntityFrameworkCore;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.ApplicationBase.Unit;
using lena.Models.Common;
using lena.Models.QualityControl.QualityControl;
using lena.Models.QualityControl.QualityControlDelayPercentageIndex;
using lena.Models.QualityControl.QualityControlItem;
using System;
//using System.Data.Entity.SqlServer;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using lena.Models.QualityControl.PaymentSuggestStatusLog;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Add
    public lena.Domains.QualityControl AddQualityControl(
        lena.Domains.QualityControl qualityControl,
        TransactionBatch transactionBatch,
        string description,
        QualityControlStatus status,
        int stuffId,
        QualityControlType qualityControlType,
        short warehouseId,
        byte unitId,
        double qty,
        short departmentId,
        int? employeeId)
    {
      qualityControl = qualityControl ?? repository.Create<lena.Domains.QualityControl>();
      qualityControl.Status = status;
      qualityControl.StuffId = stuffId;
      qualityControl.QualityControlType = qualityControlType;
      qualityControl.WarehouseId = warehouseId;
      qualityControl.UnitId = unitId;
      qualityControl.Qty = qty;
      qualityControl.DepartmentId = departmentId;
      qualityControl.EmployeeId = employeeId;
      qualityControl.DocumentId = null;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: qualityControl,
                    transactionBatch: transactionBatch,
                    description: description);
      return qualityControl;
    }
    #endregion
    #region Edit
    public lena.Domains.QualityControl EditQualityControl(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<QualityControlStatus> status = null,
        TValue<int> stuffId = null,
        TValue<QualityControlType> qualityControlType = null,
        TValue<short> warehouseId = null,
        TValue<byte> unitId = null,
        TValue<double> qty = null,
        TValue<UploadFileData> uploadFileData = null,
        TValue<DateTime?> confirmationDateTime = null,
        TValue<int?> confirmationUserId = null,
        TValue<bool> isDelete = null)
    {
      var qualityControl = GetQualityControl(id: id);
      return EditQualityControl(
                    qualityControl: qualityControl,
                    rowVersion: rowVersion,
                    description: description,
                    status: status,
                    stuffId: stuffId,
                    qualityControlType: qualityControlType,
                    warehouseId: warehouseId,
                    unitId: unitId,
                    qty: qty,
                    uploadFileData: uploadFileData,
                    confirmationDateTime: confirmationDateTime,
                    confirmationUserId: confirmationUserId,
                    isDelete: isDelete);
    }
    public lena.Domains.QualityControl EditQualityControl(
        lena.Domains.QualityControl qualityControl,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<QualityControlStatus> status = null,
        TValue<int> stuffId = null,
        TValue<QualityControlType> qualityControlType = null,
        TValue<short> warehouseId = null,
        TValue<byte> unitId = null,
        TValue<double> qty = null,
        TValue<UploadFileData> uploadFileData = null,
        TValue<DateTime?> confirmationDateTime = null,
        TValue<int?> confirmationUserId = null,
        TValue<bool> isDelete = null)
    {
      if (uploadFileData != null)
      {
        if (qualityControl.DocumentId != null)
          App.Internals.ApplicationBase.DeleteDocument(qualityControl.DocumentId.Value);
        var document = App.Internals.ApplicationBase.AddDocument(
                      name: uploadFileData.Value.FileName,
                      fileStream: uploadFileData.Value.FileData);
        qualityControl.DocumentId = document.Id;
      }
      if (status != null)
        qualityControl.Status = status;
      if (stuffId != null)
        qualityControl.StuffId = stuffId;
      if (qualityControlType != null)
        qualityControl.QualityControlType = qualityControlType;
      if (warehouseId != null)
        qualityControl.WarehouseId = warehouseId;
      if (unitId != null)
        qualityControl.UnitId = unitId;
      if (confirmationUserId != null)
        qualityControl.ConfirmationUserId = confirmationUserId;
      if (confirmationDateTime != null)
        qualityControl.ConfirmationDateTime = confirmationDateTime;
      if (qty != null)
        qualityControl.Qty = qty;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: qualityControl,
                    description: description,
                    isDelete: isDelete,
                    rowVersion: rowVersion);
      return retValue as lena.Domains.QualityControl;
    }
    #endregion
    #region AddProcess
    public lena.Domains.QualityControl AddQualityControlProcess(
        lena.Domains.QualityControl qualityControl,
        TransactionBatch transactionBatch,
        int stuffId,
        short warehouseId,
        string description,
        AddQualityControlItemTransactionInput[] addQualityControlItemTransactionInputs)
    {
      #region AddTransactionBatch
      transactionBatch = transactionBatch ?? App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region GetSumQty
      var sumQtys = from item in addQualityControlItemTransactionInputs
                    select new SumQtyItemInput()
                    {
                      Qty = item.Amount,
                      UnitId = item.UnitId
                    };
      var sumQtyResult = App.Internals.ApplicationBase.SumQty(
                    targetUnitId: null,
                    sumQtys: sumQtys.ToArray());
      #endregion
      #region Get Stuff
      var stuff = App.Internals.SaleManagement.GetStuff(e => new { e.Id, e.QualityControlDepartmentId, e.QualityControlEmployeeId }, id: stuffId);
      if (stuff.QualityControlDepartmentId == null)
        throw new QualityControlDepartmentNotDefinedException(stuffId: stuff.Id);
      #endregion
      #region AddQualityControl
      qualityControl = AddQualityControl(
              qualityControl: qualityControl,
              transactionBatch: transactionBatch,
              description: description,
              status: QualityControlStatus.NotAction,
              stuffId: stuffId,
              qualityControlType: qualityControl.QualityControlType,
              warehouseId: warehouseId,
              unitId: sumQtyResult.UnitId,
              qty: sumQtyResult.Qty,
              departmentId: stuff.QualityControlDepartmentId.Value,
              employeeId: stuff.QualityControlEmployeeId);
      #endregion
      #region AddQualityControlSummary
      AddQualityControlSummary(
              acceptedQty: 0,
              failedQty: 0,
              conditionalRequestQty: 0,
              conditionalQty: 0,
              conditionalRejectedQty: 0,
              returnedQty: 0,
              consumedQty: 0,
              qualityControlId: qualityControl.Id);
      #endregion
      #region Add QualityControlItem Process
      foreach (var addQualityControlItemTransactionInput in addQualityControlItemTransactionInputs)
      {
        AddQualityControlItemProcess(
                      transactionBatch: null,
                      description: addQualityControlItemTransactionInput.Description,
                      qualityControlId: qualityControl.Id,
                      stuffId: stuffId,
                      warehouseId: warehouseId,
                      stuffSerialCode: addQualityControlItemTransactionInput.StuffSerialCode,
                      qty: addQualityControlItemTransactionInput.Amount,
                      unitId: addQualityControlItemTransactionInput.UnitId,
                      returnOfSaleId: addQualityControlItemTransactionInput.ReturnOfSaleId);
      }
      #endregion
      #region Add or Get ProjectStep
      var projectStep = App.Internals.ProjectManagement.GetOrAddCommonProjectStep(
              departmentId: (int)Departments.QualityControl);
      #endregion
      #region Add ProjectWork
      var projectWork = App.Internals.ProjectManagement.AddProjectWork(
              projectWork: null,
              name: $"پروسه کنترل کیفی کد {qualityControl.Code}",
              description: "",
              color: "",
              departmentId: stuff.QualityControlDepartmentId.Value,
              estimatedTime: 18000,
              isCommit: false,
              projectStepId: projectStep.Id,
              baseEntityId: qualityControl.Id);
      #endregion
      #region Add QualityControlConfirmation Task
      #region Get DescriptionForTask
      var stuffName = qualityControl.Stuff.Name;
      var stuffCode = qualityControl.Stuff.Code;
      #endregion
      //check projectWork not null
      if (projectWork != null)
        App.Internals.ProjectManagement.AddProjectWorkItem(
                      projectWorkItem: null,
                      name: $"تایید کنترل کیفی {qualityControl.Code} ",
                      description: $"عنوان کالا:{stuffName}, کد کالا:{stuffCode}",
                      color: "",
                      departmentId: stuff.QualityControlDepartmentId.Value,
                      estimatedTime: 10800,
                      isCommit: false,
                      scrumTaskTypeId: (int)ScrumTaskTypes.QualityControlConfirmation,
                      userId: stuff.QualityControlEmployeeId,
                      spentTime: 0,
                      remainedTime: 0,
                      scrumTaskStep: ScrumTaskStep.ToDo,
                      projectWorkId: projectWork.Id,
                      baseEntityId: qualityControl.Id);
      #endregion
      return qualityControl;
    }
    #endregion
    #region Get
    public lena.Domains.QualityControl GetQualityControl(int id) => GetQualityControl(selector: e => e, id: id);
    public TResult GetQualityControl<TResult>(
        Expression<Func<lena.Domains.QualityControl, TResult>> selector,
        int id)
    {
      var qualityControl = GetQualityControls(
                selector: selector,
                id: id)
                .FirstOrDefault();
      if (qualityControl == null)
        throw new QualityControlNotFoundException(id);
      return qualityControl;
    }
    public QualityControlResult GetQualityControlResult(
        int id)
    {
      var qualityControls = GetQualityControls(
                selector: e => e,
                id: id,
                isDelete: false);
      var qualityControlResult = ToQualityControlResult(
                query: qualityControls
                )
                .FirstOrDefault();
      if (qualityControlResult == null)
        throw new QualityControlNotFoundException(id);
      return qualityControlResult;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetQualityControls<TResult>(
        Expression<Func<lena.Domains.QualityControl, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<QualityControlStatus> status = null,
        TValue<QualityControlStatus> hasNotstatus = null,
        TValue<int> stuffId = null,
        TValue<int?> employeeId = null,
        TValue<int> departmentId = null,
        TValue<int> stuffPurchaseCategoryQualityControlDepartmentId = null,
        TValue<int> stuffPurchaseCategoryId = null,
        TValue<int> qualityControlDepartmentId = null,
        TValue<QualityControlType> qualityControlType = null,
        TValue<int> warehouseId = null,
        TValue<int> unitId = null,
        TValue<double> qty = null,
        TValue<double> acceptedQty = null,
        TValue<double> failedQty = null,
        TValue<double> conditionalRequestQty = null,
        TValue<double> conditionalQty = null,
        TValue<double> conditionalRejectedQty = null,
        TValue<double> returnedQty = null,
        TValue<double> consumedQty = null,
        TValue<int> storeReceiptId = null,
        TValue<int> recepitId = null,
        TValue<int> cooperatorId = null,
        TValue<DateTime> dateTime = null,
        TValue<DateTime> confirmationDateTime = null,
        TValue<string> serial = null,
        TValue<string> receiptCode = null,
        TValue<bool> isEmergency = null,
        TValue<string> storeReceiptCode = null,
        TValue<bool> getAllQCTypes = null,
        TValue<string[]> serials = null,
        TValue<int> qualityControlItemId = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    ids: ids,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<lena.Domains.QualityControl>();
      if (status != null)
        query = query.Where(i => i.Status == status);
      if (hasNotstatus != null)
        query = query.Where(i => i.Status != hasNotstatus);
      if (departmentId != null)
        query = query.Where(i => i.DepartmentId == departmentId);
      if (stuffPurchaseCategoryQualityControlDepartmentId != null)
        query = query.Where(i => i.Stuff.StuffPurchaseCategory.QualityControlDepartmentId == stuffPurchaseCategoryQualityControlDepartmentId);
      if (stuffPurchaseCategoryId != null)
        query = query.Where(i => i.Stuff.StuffPurchaseCategoryId == stuffPurchaseCategoryId);
      if (qualityControlDepartmentId != null)
        query = query.Where(i => i.Stuff.StuffPurchaseCategory.QualityControlDepartmentId == qualityControlDepartmentId);
      if (employeeId != null)
        query = query.Where(i => i.EmployeeId == employeeId);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (qualityControlType != null)
        query = query.Where(i => i.QualityControlType == qualityControlType);
      if (warehouseId != null)
        query = query.Where(i => i.WarehouseId == warehouseId);
      if (unitId != null)
        query = query.Where(i => i.UnitId == unitId);
      if (qty != null)
        query = query.Where(i => i.Qty == qty);
      if (dateTime != null)
        query = query.Where(i => i.DateTime == dateTime);
      if (confirmationDateTime != null)
        query = query.Where(i => i.ConfirmationDateTime == confirmationDateTime);
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial);
        query = query.Where(i => i.QualityControlItems.Any(j => j.StuffSerial.Serial == serial));
      }
      if (serials != null)
      {
        query = query.Where(i => i.QualityControlItems.Any(q => serials.Value.Contains(q.StuffSerial.Serial)));
      }
      if (recepitId != null)
      {
        var receiptQualityControl = query.OfType<ReceiptQualityControl>();
        receiptQualityControl = receiptQualityControl.Where(i => i.StoreReceipt.ReceiptId == recepitId);
        query = receiptQualityControl;
      }
      if (storeReceiptId != null)
      {
        var receiptQualityControl = query.OfType<ReceiptQualityControl>();
        receiptQualityControl = receiptQualityControl.Where(i => i.StoreReceiptId == storeReceiptId);
        query = receiptQualityControl;
      }
      if (receiptCode != null && receiptCode != "")
      {
        var receiptQualityControl = query.OfType<ReceiptQualityControl>();
        receiptQualityControl = receiptQualityControl.Where(i => i.StoreReceipt.Receipt.Code == receiptCode);
        if (getAllQCTypes == null || (getAllQCTypes != null && !getAllQCTypes.Value))
        {
          query = receiptQualityControl;
        }
        else if (getAllQCTypes != null)
        {
          var qcItems = GetQualityControlItems(e => new { e.Id, e.QualityControlId, e.StuffSerialCode, e.StuffId });
          var receiptStuffSerialCodes = (from srQc in receiptQualityControl
                                         join qcItem in qcItems on srQc.Id equals qcItem.QualityControlId
                                         select new { qcItem.StuffSerialCode, qcItem.StuffId });
          var qcs = GetQualityControls(
                    e => e,
                    isDelete: isDelete);
          query = (from qc in qcs
                   join qcItem in qcItems on qc.Id equals qcItem.QualityControlId
                   join receiptSerial in receiptStuffSerialCodes on new { qcItem.StuffId, qcItem.StuffSerialCode } equals new { receiptSerial.StuffId, receiptSerial.StuffSerialCode }
                   select qc).Distinct();
        }
      }
      if (storeReceiptCode != null)
      {
        var storeReceiptQualityControl = query.OfType<ReceiptQualityControl>();
        storeReceiptQualityControl = storeReceiptQualityControl.Where(i => i.StoreReceipt.Code == storeReceiptCode);
        if (getAllQCTypes == null || (getAllQCTypes != null && !getAllQCTypes.Value))
        {
          query = storeReceiptQualityControl;
        }
        else if (getAllQCTypes != null)
        {
          var qcItems = GetQualityControlItems(e => new { e.Id, e.QualityControlId, e.StuffSerialCode, e.StuffId });
          var receiptStuffSerialCodes = (from srQc in storeReceiptQualityControl
                                         join qcItem in qcItems on srQc.Id equals qcItem.QualityControlId
                                         select new { qcItem.StuffSerialCode, qcItem.StuffId });
          var qcs = GetQualityControls(
                    e => e,
                    isDelete: isDelete);
          query = (from qc in qcs
                   join qcItem in qcItems on qc.Id equals qcItem.QualityControlId
                   join receiptSerial in receiptStuffSerialCodes on new { qcItem.StuffId, qcItem.StuffSerialCode } equals new { receiptSerial.StuffId, receiptSerial.StuffSerialCode }
                   select qc).Distinct();
        }
      }
      if (cooperatorId != null)
      {
        var cooperator = repository.GetQuery<StoreReceipt>().FirstOrDefault(x => x.CooperatorId == cooperatorId);
        var qualityControlCooperator = query.OfType<ReceiptQualityControl>();
        query = qualityControlCooperator;
      }
      if (isEmergency != null)
      {
        query = from q in query
                let IsEmergency = q.QualityControlConfirmation.IsEmergency
                where IsEmergency == isEmergency
                select q;
      }
      if (qualityControlItemId != null)
      {
        query = query.Where(i => i.QualityControlItems.Select(qci => qci.Id).Contains(qualityControlItemId));
      }
      return query.Select(selector);
    }
    #endregion
    #region GetPaymentSuggestStatusLogs
    public IQueryable<TResult> GetPaymentSuggestStatusLogs<TResult>(
    Expression<Func<PaymentSuggestStatusLog, TResult>> selector,
    TValue<int> employeeId = null,
    TValue<int> qualityControlId = null)
    {
      var query = repository.GetQuery<PaymentSuggestStatusLog>();
      if (qualityControlId != null)
        query = query.Where(i => i.QualityControlId == qualityControlId);
      if (employeeId != null)
        query = query.Where(i => i.RegisterarUser.Employee.Id == employeeId);
      return query.Select(selector);
    }
    #endregion
    #region ToPaymentSuggestStatusLogResult
    public IQueryable<PaymentSuggestStatusLogResult> ToPaymentSuggestStatusLogResult(
       IQueryable<PaymentSuggestStatusLog> paymentSuggestStatusLogs
       )
    {
      var resultQuery = from paymentSuggestStatusLog in paymentSuggestStatusLogs
                        select new PaymentSuggestStatusLogResult
                        {
                          Id = paymentSuggestStatusLog.Id,
                          QualityControlId = paymentSuggestStatusLog.QualityControlId,
                          RegisterarEmployeeName = paymentSuggestStatusLog.RegisterarUser.Employee.FirstName + " " + paymentSuggestStatusLog.RegisterarUser.Employee.LastName,
                          RegisterDateTime = paymentSuggestStatusLog.RegisterDateTime,
                          QualityControlPaymentSuggestStatus = paymentSuggestStatusLog.QualityControlPaymentSuggestStatus,
                          QualityControlCode = paymentSuggestStatusLog.QualityControl.Code,
                          Description = paymentSuggestStatusLog.Description
                        };
      return resultQuery;
    }
    #endregion
    #region SearchPaymentSuggestStatusLogResult
    public IQueryable<PaymentSuggestStatusLogResult> SearchPaymentSuggestStatusLogResult(
        IQueryable<PaymentSuggestStatusLogResult> query,
        string search,
        AdvanceSearchItem[] advanceSearchItems,
        int? qualityControlId)
    {
      if (qualityControlId != null)
        query = query.Where(i => i.QualityControlId == qualityControlId);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<PaymentSuggestStatusLogResult> SortPaymentSuggestStatusLogResult(
        IQueryable<PaymentSuggestStatusLogResult> query,
        SortInput<PaymentSuggestStatusLogsSortType> sort)
    {
      switch (sort.SortType)
      {
        case PaymentSuggestStatusLogsSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case PaymentSuggestStatusLogsSortType.QualityControlCode:
          return query.OrderBy(a => a.QualityControlCode, sort.SortOrder);
        case PaymentSuggestStatusLogsSortType.QualityControlPaymentSuggestStatus:
          return query.OrderBy(a => a.QualityControlPaymentSuggestStatus, sort.SortOrder);
        case PaymentSuggestStatusLogsSortType.RegisterarEmployeeName:
          return query.OrderBy(a => a.RegisterarEmployeeName, sort.SortOrder);
        case PaymentSuggestStatusLogsSortType.RegisterDateTime:
          return query.OrderBy(a => a.RegisterDateTime, sort.SortOrder);
        case PaymentSuggestStatusLogsSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region AddPaymentSuggestStatusLog
    public PaymentSuggestStatusLog AddPaymentSuggestStatusLog(
        int qualityControlId,
        QualityControlPaymentSuggestStatus? qualityControlPaymentSuggestStatus,
        TValue<string> description = null)
    {
      var paymentSuggestStatusLog = repository.Create<PaymentSuggestStatusLog>();
      paymentSuggestStatusLog.QualityControlId = qualityControlId;
      paymentSuggestStatusLog.QualityControlPaymentSuggestStatus = qualityControlPaymentSuggestStatus;
      paymentSuggestStatusLog.RegisterarUserId = App.Providers.Security.CurrentLoginData.UserId;
      paymentSuggestStatusLog.RegisterDateTime = DateTime.UtcNow;
      paymentSuggestStatusLog.Description = description;
      repository.Add(paymentSuggestStatusLog);
      return paymentSuggestStatusLog;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<QualityControlResult> SortQualityControlResult(
        IQueryable<QualityControlResult> query,
        SortInput<QualityControlSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case QualityControlSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case QualityControlSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case QualityControlSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case QualityControlSortType.QualityControlType:
          return query.OrderBy(a => a.QualityControlType, sort.SortOrder);
        case QualityControlSortType.StoreReceiptType:
          return query.OrderBy(a => a.StoreReceiptType, sort.SortOrder);
        case QualityControlSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case QualityControlSortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case QualityControlSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case QualityControlSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case QualityControlSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case QualityControlSortType.AcceptedQty:
          return query.OrderBy(a => a.AcceptedQty, sort.SortOrder);
        case QualityControlSortType.FailedQty:
          return query.OrderBy(a => a.FailedQty, sort.SortOrder);
        case QualityControlSortType.ConditionalQty:
          return query.OrderBy(a => a.ConditionalQty, sort.SortOrder);
        case QualityControlSortType.ReturnedQty:
          return query.OrderBy(a => a.ReturnedQty, sort.SortOrder);
        case QualityControlSortType.ConsumedQty:
          return query.OrderBy(a => a.ConsumedQty, sort.SortOrder);
        case QualityControlSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case QualityControlSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case QualityControlSortType.ConfirmationEmployeeFullName:
          return query.OrderBy(a => a.ConfirmationEmployeeFullName, sort.SortOrder);
        case QualityControlSortType.ConfirmationDateTime:
          return query.OrderBy(a => a.ConfirmationDateTime, sort.SortOrder);
        case QualityControlSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case QualityControlSortType.StoreReceiptDateTime:
          return query.OrderBy(a => a.StoreReceiptDateTime, sort.SortOrder);
        case QualityControlSortType.InboundCargoDateTime:
          return query.OrderBy(a => a.InboundCargoDateTime, sort.SortOrder);
        case QualityControlSortType.StoreReceiptCode:
          return query.OrderBy(a => a.StoreReceiptCode, sort.SortOrder);
        case QualityControlSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case QualityControlSortType.HasUploadedDocument:
          return query.OrderBy(a => a.HasUploadedDocument, sort.SortOrder);
        case QualityControlSortType.ReceiptCode:
          return query.OrderBy(a => a.ReceiptCode, sort.SortOrder);
        case QualityControlSortType.ReceiptStatus:
          return query.OrderBy(a => a.ReceiptStatus, sort.SortOrder);
        case QualityControlSortType.PayRequestStatus:
          return query.OrderBy(a => a.PayRequestStatus, sort.SortOrder);
        case QualityControlSortType.NeedToQualityControlDocumentUpload:
          return query.OrderBy(a => a.NeedToQualityControlDocumentUpload, sort.SortOrder);
        case QualityControlSortType.StuffPurchaseCategoryTitle:
          return query.OrderBy(a => a.StuffPurchaseCategoryTitle, sort.SortOrder);
        case QualityControlSortType.StuffPurchaseCategoryQualityControlDepartmentName:
          return query.OrderBy(a => a.StuffPurchaseCategoryQualityControlDepartmentName, sort.SortOrder);
        case QualityControlSortType.StuffPurchaseCategoryQualityControlUserGroupName:
          return query.OrderBy(a => a.StuffPurchaseCategoryQualityControlUserGroupName, sort.SortOrder);
        case QualityControlSortType.QualityControlConfirmationDescription:
          return query.OrderBy(a => a.QualityControlConfirmationDescription, sort.SortOrder);
        case QualityControlSortType.ResponsibleFullNames:
          return query.OrderBy(a => a.ResponsibleFullNames, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<QualityControlComboResult> SortQualityControlComboResult(
        IQueryable<QualityControlComboResult> query)
    {
      return query.OrderBy(a => a.Code, SortOrder.Ascending);
    }
    public IOrderedQueryable<QualityControlDelayPercentageIndexResult> SortQualityControlDelayPercentageIndexResult(
      IQueryable<QualityControlDelayPercentageIndexResult> query,
      SortInput<QualityControlDelayPercentageIndexSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlDelayPercentageIndexSortType.Count:
          return query.OrderBy(a => a.Count, sort.SortOrder);
        case QualityControlDelayPercentageIndexSortType.DelayDay:
          return query.OrderBy(a => a.DelayDayInt, sort.SortOrder);
        case QualityControlDelayPercentageIndexSortType.Percentage:
          return query.OrderBy(a => a.Percentage, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<QualityControlResult> SearchQualityControlResult(
        IQueryable<QualityControlResult> query,
        string search,
        AdvanceSearchItem[] advanceSearchItems,
        int? cooperatorId,
        string cooperatorName,
        StoreReceiptType? storeReceiptType = null,
        DateTime? fromConfirmationDate = null,
        DateTime? toConfirmationDate = null,
        DateTime? fromStoreReceiptDate = null,
        DateTime? toStoreReceiptDate = null,
        DateTime? fromInboundCargoDate = null,
        DateTime? toInboundCargoDate = null,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.Code.Contains(search) ||
                    item.StuffCode.Contains(search) ||
                    item.StuffName.Contains(search) ||
                    item.WarehouseName.Contains(search) ||
                    item.CooperatorName.Contains(search)
                select item;
      if (cooperatorId != null)
        query = query.Where(i => i.CooperatorId == cooperatorId);
      if (!string.IsNullOrWhiteSpace(cooperatorName))
        query = query.Where(i => i.CooperatorName == cooperatorName);
      if (fromConfirmationDate != null)
        query = query.Where(i => i.ConfirmationDateTime >= fromConfirmationDate);
      if (storeReceiptType != null)
        query = query.Where(i => i.StoreReceiptType == storeReceiptType);
      if (toConfirmationDate != null)
        query = query.Where(i => i.ConfirmationDateTime <= toConfirmationDate);
      if (fromStoreReceiptDate != null)
        query = query.Where(i => i.StoreReceiptDateTime >= fromStoreReceiptDate);
      if (toStoreReceiptDate != null)
        query = query.Where(i => i.StoreReceiptDateTime <= toStoreReceiptDate);
      if (fromInboundCargoDate != null)
        query = query.Where(i => i.InboundCargoDateTime >= fromInboundCargoDate);
      if (toInboundCargoDate != null)
        query = query.Where(i => i.InboundCargoDateTime <= toInboundCargoDate);
      if (fromDate != null)
        query = query.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        query = query.Where(i => i.DateTime <= toDate);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    public IQueryable<QualityControlDelayPercentageIndexResult> SearchQualityControlDelayPercentageIndexResult(
     IQueryable<QualityControlDelayPercentageIndexResult> query,
     string search,
     AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.DelayDay.Contains(search) ||
                    item.Count.ToString().Contains(search) ||
                    item.Percentage.ToString().Contains(search)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region ToResult
    public IQueryable<QualityControlResult> ToQualityControlResult(
      IQueryable<lena.Domains.QualityControl> query)
    {
      var RqcQuery =
                GetReceiptQualityControls(selector: e => new
                {
                  Id = e.Id,
                  StoreReceiptId = e.StoreReceiptId,
                  StoreReceiptCode = e.StoreReceipt.Code,
                  StoreReceiptReceiptId = e.StoreReceipt.ReceiptId,
                  StoreReceiptReceiptStatus = e.StoreReceipt.Receipt.Status,
                  StoreReceiptStoreReceiptType = e.StoreReceipt.StoreReceiptType,
                  CooperatorId = e.StoreReceipt.CooperatorId,
                  CooperatorName = e.StoreReceipt.Cooperator.Name,
                  InboundCargoDateTime = e.StoreReceipt.InboundCargo.DateTime,
                  StoreReceiptDateTime = e.StoreReceipt.DateTime,
                });
      var newShoppings = App.Internals.WarehouseManagement.GetNewShoppings(selector: e => e);
      var newShoppingsQuery = from newShopping in newShoppings
                              select new
                              {
                                Id = newShopping.Id,
                                CargoId = newShopping.LadingItem.CargoItem.CargoId,
                                CargoCode = newShopping.LadingItem.CargoItem.Cargo.Code,
                                CargoItemCode = newShopping.LadingItem.CargoItem.Code,
                                CargoItemId = newShopping.LadingItem.CargoItemId,
                                LadingCode = newShopping.LadingItem.Lading.Code,
                                PurchaseOrderCode = newShopping.LadingItem.CargoItem.PurchaseOrder.Code,
                                PurchaseOrderId = newShopping.LadingItem.CargoItem.PurchaseOrderId,
                                UnitPrice = newShopping.LadingItem.CargoItem.PurchaseOrder.Price,
                                CurrencyId = newShopping.LadingItem.CargoItem.PurchaseOrder.CurrencyId,
                                CurrencyTitle = newShopping.LadingItem.CargoItem.PurchaseOrder.Currency.Title,
                                Price = newShopping.LadingItem.CargoItem.PurchaseOrder.Price
                              };
      var stuffDocuments = App.Internals.SaleManagement.GetLatestStuffDocument(
                    e => e,
                    stuffDocumentType: StuffDocumentType.QualityControlDocument);
      var result = from qualityControl in query
                   let stuffPurchaseCategory = qualityControl.Stuff.StuffPurchaseCategory
                   join receiptQualityControl in RqcQuery on qualityControl.Id equals receiptQualityControl.Id into tbl_receiptQualityControl
                   from subRqc in tbl_receiptQualityControl.DefaultIfEmpty()
                   join newShopping in newShoppingsQuery on subRqc.StoreReceiptId equals newShopping.Id into tbl_newShopping
                   from subNewShopping in tbl_newShopping.DefaultIfEmpty()
                   join stuffDocument in stuffDocuments on qualityControl.StuffId equals stuffDocument.StuffId into tbl_stuffDocument
                   from subStuffDocument in tbl_stuffDocument.DefaultIfEmpty()
                   select new QualityControlResult
                   {
                     Id = qualityControl.Id,
                     Code = qualityControl.Code,
                     QualityControlPaymentSuggestStatus = qualityControl.QualityControlPaymentSuggestStatus,
                     StoreReceiptCode = subRqc.StoreReceiptCode,
                     ReceiptCode = subRqc.StoreReceiptCode,
                     ReceiptId = subRqc.StoreReceiptReceiptId,
                     ReceiptStatus = subRqc.StoreReceiptReceiptStatus,
                     StuffId = qualityControl.StuffId,
                     StuffCode = qualityControl.Stuff.Code,
                     StuffName = qualityControl.Stuff.Name,
                     NeedToQualityControlDocumentUpload = qualityControl.Stuff.NeedToQualityControlDocumentUpload,
                     QualityControlType = qualityControl.QualityControlType,
                     StoreReceiptType = subRqc.StoreReceiptStoreReceiptType,
                     WarehouseId = qualityControl.WarehouseId,
                     WarehouseName = qualityControl.Warehouse.Name,
                     CooperatorId = subRqc.CooperatorId,
                     CooperatorName = subRqc.CooperatorName,
                     IsEmergency = qualityControl.QualityControlConfirmation.IsEmergency,
                     Status = qualityControl.Status,
                     PayRequestId = qualityControl.PayRequest.Id,
                     PayRequestStatus = qualityControl.PayRequest.Status,
                     Qty = qualityControl.Qty,
                     UnitId = qualityControl.UnitId,
                     UnitName = qualityControl.Unit.Name,
                     AcceptedQty = (double?)qualityControl.QualityControlSummary.AcceptedQty,
                     FailedQty = (double?)qualityControl.QualityControlSummary.FailedQty,
                     ConditionalRequestQty = (double?)qualityControl.QualityControlSummary.ConditionalRequestQty,
                     ConditionalQty = (double?)qualityControl.QualityControlSummary.ConditionalQty,
                     ConditionalRejectedQty = (double?)qualityControl.QualityControlSummary.ConditionalRejectedQty,
                     ReturnedQty = (double?)qualityControl.QualityControlSummary.ReturnedQty,
                     ConsumedQty = (double?)qualityControl.QualityControlSummary.ConsumedQty,
                     Description = qualityControl.Description,
                     DepartmentId = qualityControl.DepartmentId,
                     DepartmentName = qualityControl.Department.Name,
                     EmployeeId = qualityControl.EmployeeId,
                     EmployeeFullName = qualityControl.Employee.FirstName + " " + qualityControl.Employee.LastName,
                     StuffPurchaseCategoryId = stuffPurchaseCategory != null ? stuffPurchaseCategory.Id : (int?)null,
                     StuffPurchaseCategoryTitle = stuffPurchaseCategory != null ? stuffPurchaseCategory.Title : null,
                     StuffPurchaseCategoryQualityControlDepartmentId = stuffPurchaseCategory != null ? stuffPurchaseCategory.QualityControlDepartmentId : (int?)null,
                     StuffPurchaseCategoryQualityControlDepartmentName = stuffPurchaseCategory != null ? stuffPurchaseCategory.QualityControlDepartment.Name : null,
                     StuffPurchaseCategoryQualityControlUserGroupId = stuffPurchaseCategory != null ? stuffPurchaseCategory.QualityControlUserGroupId : (int?)null,
                     StuffPurchaseCategoryQualityControlUserGroupName = stuffPurchaseCategory != null ? stuffPurchaseCategory.QualityControlUserGroup.Name : null,
                     DateTime = qualityControl.DateTime,
                     InboundCargoDateTime = (DateTime?)subRqc.InboundCargoDateTime,
                     StoreReceiptDateTime = (DateTime?)subRqc.StoreReceiptDateTime,
                     ConfirmationUserId = qualityControl.ConfirmationUserId,
                     ConfirmationEmployeeId = qualityControl.ConfirmationUser.Employee.Id,
                     ConfirmationEmployeeFullName = qualityControl.ConfirmationUser.Employee.FirstName + " " + qualityControl.ConfirmationUser.Employee.LastName,
                     ConfirmationDateTime = qualityControl.ConfirmationDateTime,
                     CargoId = subNewShopping.CargoId,
                     CargoCode = subNewShopping.CargoCode,
                     CargoItemCode = subNewShopping.CargoItemCode,
                     LadingCode = subNewShopping.LadingCode,
                     PurchaseOrderCode = subNewShopping.PurchaseOrderCode,
                     CargoItemId = subNewShopping.CargoItemId,
                     PurchaseOrderId = subNewShopping.PurchaseOrderId,
                     UnitPrice = subNewShopping.UnitPrice,
                     TotalPrice = subNewShopping.UnitPrice * qualityControl.QualityControlSummary.FailedQty,
                     CurrencyId = subNewShopping.CurrencyId,
                     CurrencyTitle = subNewShopping.CurrencyTitle,
                     DocumentId = qualityControl.DocumentId ?? subStuffDocument.DocumentId,
                     RowVersion = qualityControl.RowVersion,
                     QualityControlItems = qualityControl.QualityControlItems.AsQueryable().Select(App.Internals.QualityControl.ToQualityControlItemResult),
                     HasUploadedDocument = qualityControl.DocumentId != null,
                     QualityControlConfirmationDescription = qualityControl.QualityControlConfirmation.Description,
                     QualityControlConfirmationId = qualityControl.QualityControlConfirmation.Id,
                     Price = subNewShopping.Price
                   };
      return result;
    }
    #endregion
    #region ToComboResult
    public Expression<Func<lena.Domains.QualityControl, QualityControlComboResult>> ToQualityControlComboResult =
        qualityControl => new QualityControlComboResult
        {
          Id = qualityControl.Id,
          Code = qualityControl.Code,
          QualityControlType = qualityControl.QualityControlType,
          RowVersion = qualityControl.RowVersion
        };
    #endregion
    #region GetQualityControlDelayPercentageIndex
    public IQueryable<QualityControlDelayPercentageIndexResult> GetQualityControlDelayPercentageIndex(
        DateTime fromDateTime,
        DateTime toDateTime,
        int onTimeIndicatorWeightId,
        int departmentId,
        int lackOfTimeIndicatorWeightId
        )
    {
      IQueryable<QualityControlDelayPercentageIndexResult> result = Enumerable.Empty<QualityControlDelayPercentageIndexResult>().AsQueryable();
      var qualityControls = GetReceiptQualityControls(selector: e => new
      {
        StoreReceiptDateTime = e.StoreReceipt.DateTime,
        QualityControlId = e.Id,
        QualityControlItems = e.QualityControlItems,
        QualityControlCheckDuration = e.Stuff.QualityControlCheckDuration,
        StoreReceiptAmount = e.StoreReceipt.Amount * e.Unit.ConversionRatio,
        StoreReceiptId = e.StoreReceiptId
      },
            isDelete: false,
            fromDateTime: fromDateTime,
            toDateTime: toDateTime,
            qualityControlDepartmentId: departmentId);
      var getQualityControlItems = GetQualityControlItems(
                e => e,
                isDelete: false);
      #region WeightDays
      var onTimeWeightDays = App.Internals.QualityAssurance.GetWeightDays(
          e => new
          {
            Id = e.Id,
            IndicatorWeightId = e.IndicatorWeightId,
            Day = e.Day,
            Amount = e.Amount,
            IsOnTime = true
          },
          indicatorWeightId: onTimeIndicatorWeightId);
      var lackOfTimeWeightDays = App.Internals.QualityAssurance.GetWeightDays(
                e => new
                {
                  Id = e.Id,
                  IndicatorWeightId = e.IndicatorWeightId,
                  Day = e.Day,
                  Amount = e.Amount,
                  IsOnTime = false
                },
                indicatorWeightId: lackOfTimeIndicatorWeightId);
      var mainWeightDays = onTimeWeightDays.Union(lackOfTimeWeightDays);
      #endregion
      var qualityControlItems = from qualityControl in qualityControls
                                join qualityControlItem in getQualityControlItems on qualityControl.QualityControlId equals qualityControlItem.QualityControlId
                                select new
                                {
                                  QualityControlId = qualityControl.QualityControlId,
                                  StoreReceiptId = qualityControl.StoreReceiptId,
                                  QualityControlItemId = qualityControlItem.Id,
                                  DateDiff = EF.Functions.DateDiffDay(qualityControl.StoreReceiptDateTime, DateTime.UtcNow),
                                  ConfirmationDateTime = qualityControlItem.QualityControlConfirmationItem == null ? (DateTime?)null : qualityControlItem.QualityControlConfirmationItem.DateTime,
                                  StoreReceiptAmount = qualityControl.StoreReceiptAmount,
                                  QualityControlCheckDuration = qualityControl.QualityControlCheckDuration,
                                  DelayDay = EF.Functions.DateDiffDay(qualityControl.StoreReceiptDateTime, qualityControlItem.QualityControlConfirmationItem == null ? (DateTime?)null : qualityControlItem.QualityControlConfirmationItem.DateTime),
                                };
      var categorizedQualityControlItems = from qualityControlItem in qualityControlItems
                                           select new
                                           {
                                             QualityControlId = qualityControlItem.QualityControlId,
                                             StoreReceiptId = qualityControlItem.StoreReceiptId,
                                             QualityControlItemId = qualityControlItem.QualityControlItemId,
                                             DateDiff = qualityControlItem.DateDiff,
                                             ConfirmationDateTime = qualityControlItem.ConfirmationDateTime,
                                             StoreReceiptAmount = qualityControlItem.StoreReceiptAmount,
                                             QualityControlCheckDuration = qualityControlItem.QualityControlCheckDuration,
                                             DelayDay = qualityControlItem.DelayDay,
                                             IsOnTime = qualityControlItem.DelayDay == null ? qualityControlItem.QualityControlCheckDuration <= qualityControlItem.DateDiff : qualityControlItem.DelayDay <= qualityControlItem.QualityControlCheckDuration
                                           };
      var groupQcItemDelayDays = from categorizedQualityControlItem in categorizedQualityControlItems
                                 group categorizedQualityControlItem by new { categorizedQualityControlItem.DelayDay, categorizedQualityControlItem.IsOnTime } into g
                                 select new
                                 {
                                   DelayDay = g.Key.DelayDay == null ? -1 : g.Key.DelayDay,
                                   IsOnTime = g.Key.IsOnTime,
                                   Count = g.Count(),
                                   QualityControlIds = g.Select(m => m.QualityControlId).Distinct(),
                                   QualityControlItemIds = g.Select(m => m.QualityControlItemId).Distinct()
                                 };
      var joinQcItemDelayDays = from groupQcItemDelayDay in groupQcItemDelayDays
                                join mainWeightDay in mainWeightDays on new { DelayDay = groupQcItemDelayDay.DelayDay, IsOnTime = groupQcItemDelayDay.IsOnTime } equals new { DelayDay = (int?)mainWeightDay.Day, IsOnTime = mainWeightDay.IsOnTime } into tWeightDays
                                from tWeightDay in tWeightDays.DefaultIfEmpty()
                                select new
                                {
                                  DelayDay = groupQcItemDelayDay.DelayDay,
                                  WeightDay = (double?)tWeightDay.Amount == null ? 0 : tWeightDay.Amount,
                                  QualityControlCount = groupQcItemDelayDay.Count,
                                  QualityControlIds = groupQcItemDelayDay.QualityControlIds,
                                  QualityControlItemIds = groupQcItemDelayDay.QualityControlItemIds,
                                  AppliedWeight = (groupQcItemDelayDay.Count) * ((double?)tWeightDay.Amount == null ? 0 : tWeightDay.Amount),
                                  IsOnTime = groupQcItemDelayDay.IsOnTime
                                };
      var sum = joinQcItemDelayDays.Any() ? joinQcItemDelayDays.Sum(m => m.AppliedWeight) : 0;
      result = from query in joinQcItemDelayDays
               select new QualityControlDelayPercentageIndexResult()
               {
                 DelayDay = query.DelayDay == -1 ? "بررسی نشده" : query.DelayDay.ToString(),
                 DelayDayInt = query.DelayDay.Value,
                 WeightDay = query.WeightDay,
                 Count = query.QualityControlCount,
                 AppliedWeight = query.AppliedWeight,
                 QualityControlIds = query.QualityControlIds,
                 QualityControlItemIds = query.QualityControlItemIds,
                 Percentage = (query.AppliedWeight / sum) * 100,
                 IsOnTime = query.IsOnTime,
                 TotalWeight = sum
               };
      return result;
    }
    #endregion
    #region GetQualityControlDelayPercentageIndex
    public QualityControlDelayPercentageIndexResult GetQualityControlUnitEvaluationDelayPercentageIndex(
        DateTime fromDateTime,
        DateTime toDateTime
        )
    {
      IQueryable<QualityControlDelayPercentageIndexResult> result = Enumerable.Empty<QualityControlDelayPercentageIndexResult>().AsQueryable();
      var qualityControls = GetReceiptQualityControls(selector: e => new
      {
        StoreReceiptDateTime = e.StoreReceipt.DateTime,
        QualityControlId = e.Id,
        QualityControlItems = e.QualityControlItems,
        QualityControlCheckDuration = e.Stuff.QualityControlCheckDuration,
        StoreReceiptAmount = e.StoreReceipt.Amount * e.Unit.ConversionRatio,
        StoreReceiptId = e.StoreReceiptId
      },
            isDelete: false,
            fromDateTime: fromDateTime,
            toDateTime: toDateTime,
            qualityControlDepartmentId: (int)Departments.QualityControl);
      var getQualityControlItems = GetQualityControlItems(
                e => e,
                isDelete: false);
      #region WeightDays
      var oneTimeIndicatorWeightCode = SettingKey.QCIOD.ToString(); // شناسه وزن های مربوط به کنترل کیفی به موقع
      var lackOfTimeIndicatorWeightCode = SettingKey.QCILD.ToString(); // شناسه وزن های مربوط به کنترل کیفی عدم موقع
      var onTimeWeightDays = App.Internals.QualityAssurance.GetWeightDays(
          e => new
          {
            Id = e.Id,
            IndicatorWeightId = e.IndicatorWeightId,
            Day = e.Day,
            Amount = e.Amount,
            IsOnTime = true
          },
          indicatorWeightCode: oneTimeIndicatorWeightCode);
      var lackOfTimeWeightDays = App.Internals.QualityAssurance.GetWeightDays(
                e => new
                {
                  Id = e.Id,
                  IndicatorWeightId = e.IndicatorWeightId,
                  Day = e.Day,
                  Amount = e.Amount,
                  IsOnTime = false
                },
                indicatorWeightCode: lackOfTimeIndicatorWeightCode);
      var mainWeightDays = onTimeWeightDays.Union(lackOfTimeWeightDays);
      #endregion
      var xc = mainWeightDays.ToList();
      var qualityControlItems = from qualityControl in qualityControls
                                join qualityControlItem in getQualityControlItems on qualityControl.QualityControlId equals qualityControlItem.QualityControlId
                                select new
                                {
                                  QualityControlId = qualityControl.QualityControlId,
                                  StoreReceiptId = qualityControl.StoreReceiptId,
                                  QualityControlItemId = qualityControlItem.Id,
                                  DateDiff = EF.Functions.DateDiffDay(qualityControl.StoreReceiptDateTime, DateTime.UtcNow),
                                  ConfirmationDateTime = qualityControlItem.QualityControlConfirmationItem == null ? (DateTime?)null : qualityControlItem.QualityControlConfirmationItem.DateTime,
                                  StoreReceiptAmount = qualityControl.StoreReceiptAmount,
                                  QualityControlCheckDuration = qualityControl.QualityControlCheckDuration,
                                  DelayDay = EF.Functions.DateDiffDay(qualityControl.StoreReceiptDateTime, qualityControlItem.QualityControlConfirmationItem == null ? (DateTime?)null : qualityControlItem.QualityControlConfirmationItem.DateTime),
                                };
      var categorizedQualityControlItems = from qualityControlItem in qualityControlItems
                                           select new
                                           {
                                             QualityControlId = qualityControlItem.QualityControlId,
                                             StoreReceiptId = qualityControlItem.StoreReceiptId,
                                             QualityControlItemId = qualityControlItem.QualityControlItemId,
                                             DateDiff = qualityControlItem.DateDiff,
                                             ConfirmationDateTime = qualityControlItem.ConfirmationDateTime,
                                             StoreReceiptAmount = qualityControlItem.StoreReceiptAmount,
                                             QualityControlCheckDuration = qualityControlItem.QualityControlCheckDuration,
                                             DelayDay = qualityControlItem.DelayDay,
                                             IsOnTime = qualityControlItem.DelayDay == null ? qualityControlItem.QualityControlCheckDuration <= qualityControlItem.DateDiff : qualityControlItem.DelayDay <= qualityControlItem.QualityControlCheckDuration
                                           };
      var groupQcItemDelayDays = from categorizedQualityControlItem in categorizedQualityControlItems
                                 group categorizedQualityControlItem by new { categorizedQualityControlItem.DelayDay, categorizedQualityControlItem.IsOnTime } into g
                                 select new
                                 {
                                   DelayDay = g.Key.DelayDay == null ? -1 : g.Key.DelayDay,
                                   IsOnTime = g.Key.IsOnTime,
                                   Count = g.Count(),
                                   QualityControlIds = g.Select(m => m.QualityControlId).Distinct()
                                 };
      var joinQcItemDelayDays = from groupQcItemDelayDay in groupQcItemDelayDays
                                join mainWeightDay in mainWeightDays on new { DelayDay = groupQcItemDelayDay.DelayDay, IsOnTime = groupQcItemDelayDay.IsOnTime } equals new { DelayDay = (int?)mainWeightDay.Day, IsOnTime = mainWeightDay.IsOnTime } into tWeightDays
                                from tWeightDay in tWeightDays.DefaultIfEmpty()
                                select new
                                {
                                  DelayDay = groupQcItemDelayDay.DelayDay,
                                  WeightDay = (double?)tWeightDay.Amount == null ? 0 : tWeightDay.Amount,
                                  QualityControlCount = groupQcItemDelayDay.Count,
                                  QualityControlIds = groupQcItemDelayDay.QualityControlIds,
                                  AppliedWeight = (groupQcItemDelayDay.Count) * ((double?)tWeightDay.Amount == null ? 0 : tWeightDay.Amount),
                                  IsOnTime = groupQcItemDelayDay.IsOnTime
                                };
      var sum = joinQcItemDelayDays.Any() ? joinQcItemDelayDays.Sum(m => m.AppliedWeight) : 0;
      result = from query in joinQcItemDelayDays
               select new QualityControlDelayPercentageIndexResult()
               {
                 Percentage = (query.AppliedWeight / sum) * 100,
                 IsOnTime = query.IsOnTime,
               };
      var totalPercentage = result.Where(i => i.IsOnTime == true).Sum(m => m.Percentage);
      var finalResult = new QualityControlDelayPercentageIndexResult()
      {
        Amount = totalPercentage
      };
      return finalResult;
    }
    #endregion
    #region Delete
    public void DeleteQualityControl(
        int id,
        byte[] rowVersion)
    {
      EditQualityControl(
                id: id,
                isDelete: true,
                rowVersion: rowVersion);
    }
    #endregion
    #region AddQualityControlPaymentSuggestProcess       
    public lena.Domains.QualityControl AddQualityControlPaymentSuggestProcess(
        int qualityControlId,
        QualityControlPaymentSuggestStatus? qualityControlPaymentSuggestStatus,
        TValue<string> description = null)
    {
      var qualityControl = GetQualityControl(id: qualityControlId);
      qualityControl.QualityControlPaymentSuggestStatus = qualityControlPaymentSuggestStatus;
      EditQualityControl(
                qualityControl: qualityControl,
                rowVersion: qualityControl.RowVersion);
      AddPaymentSuggestStatusLog(
                qualityControlId: qualityControlId,
                qualityControlPaymentSuggestStatus: qualityControlPaymentSuggestStatus,
                description: description
                );
      return qualityControl;
    }
    #endregion
  }
}
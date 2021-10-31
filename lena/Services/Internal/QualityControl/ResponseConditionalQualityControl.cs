using System;
using System.Collections.Generic;
using lena.Models.Common;
using lena.Domains;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Domains.Enums;
using lena.Services.Internals.WarehouseManagement.Exception;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Add
    public ResponseConditionalQualityControl AddResponseConditionalQualityControl(
        ResponseConditionalQualityControl responseConditionalQualityControl,
        TransactionBatch transactionBatch,
        string description,
    int conditionalQualityControlId,
    ConditionalQualityControlStatus conditionalQualityControlStatus
        )
    {


      responseConditionalQualityControl = responseConditionalQualityControl ?? repository.Create<ResponseConditionalQualityControl>();
      var conditionalQualityControl = GetConditionalQualityControl(id: conditionalQualityControlId);
      responseConditionalQualityControl.ConditionalQualityControl = conditionalQualityControl;
      responseConditionalQualityControl.Status = conditionalQualityControlStatus;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: responseConditionalQualityControl,
                    transactionBatch: transactionBatch,
                    description: description);
      return responseConditionalQualityControl;
    }
    #endregion
    #region AddProcess
    public ResponseConditionalQualityControl AddResponseConditionalQualityControlProcess(
        string description,
        int conditionalQualityControlId,
        ConditionalQualityControlStatus conditionalQualityControlStatus,
        byte[] conditionalQualityControlRowVersoin)
    {

      var conditionalQualityControl = GetConditionalQualityControl(
                selector: e => e,
                id: conditionalQualityControlId);

      var userGroup = App.Internals.UserManagement.GetMemberships(
                    selector: e => e,
                    userId: App.Providers.Security.CurrentLoginData.UserId)

                .ToList();

      if (userGroup.All(x => x.UserGroupId != conditionalQualityControl.QualityControlAccepter.UserGroupId))
      {
        throw new ResponseConditionalQualityControlNotAccessException(conditionalQualityControl.Id);
      }

      #region AddTransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region AddResponseConditionalQualityControl
      var responseConditionalQualityControl = AddResponseConditionalQualityControl(
              responseConditionalQualityControl: null,
              transactionBatch: transactionBatch,
              description: description,
              conditionalQualityControlId: conditionalQualityControlId,
              conditionalQualityControlStatus: conditionalQualityControlStatus
              );
      #endregion
      #region Get ConditionalQualityControlItem
      var conditionalQualityControlItems = GetConditionalQualityControlItems(
              selector: e => new
              {
                Id = e.Id,
                TransactionBatchId = (int?)e.TransactionBatch.Id,
                StuffId = e.QualityControlConfirmationItem.QualityControlItem.StuffId,
                StuffSerialCode = e.QualityControlConfirmationItem.QualityControlItem.StuffSerialCode,
                Qty = e.Qty,
                UnitId = e.UnitId,

              },
              conditionalQualityControlId: conditionalQualityControlId,
              isDelete: false);
      #endregion
      #region Add Transactions
      var stuffIds = new List<int>();
      var warehouseIds = new List<int>();
      foreach (var conditionalQualityControlItem in conditionalQualityControlItems)
      {

        var version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
                      stuffSerialCode: conditionalQualityControlItem.StuffSerialCode,
                      stuffId: conditionalQualityControlItem.StuffId);

        var transactionTypeIds = new int[] { Models.StaticData.StaticTransactionTypes.ImportQualityControl.Id, Models.StaticData.StaticTransactionTypes.ImportPartitionStuffSerialQualityControl.Id };

        #region Get ImportQualityControl Transaction
        var importWasteTransaction = App.Internals.WarehouseManagement.GetWarehouseTransactions(
                selector: e => e,
                stuffSerialCodes: new long?[] { conditionalQualityControlItem.StuffSerialCode },
                stuffId: conditionalQualityControlItem.StuffId,
                // transactionBatchId: conditionalQualityControlItem.TransactionBatchId,
                transactionTypeIds: transactionTypeIds)


            .ToList()
            .LastOrDefault();
        #endregion
        if (importWasteTransaction.WarehouseId == null)
          throw new BaseTransactionHasNoWarehouseException(id: importWasteTransaction.Id);

        if (!warehouseIds.Contains(importWasteTransaction.WarehouseId.Value))
          warehouseIds.Add(importWasteTransaction.WarehouseId.Value);
        #region Add ExportQualityControl Transaction
        var exportQualityControlTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: conditionalQualityControlItem.StuffId,
                billOfMaterialVersion: version,
                stuffSerialCode: conditionalQualityControlItem.StuffSerialCode,
                warehouseId: importWasteTransaction.WarehouseId.Value,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportQualityControl.Id,
                amount: conditionalQualityControlItem.Qty,
                unitId: conditionalQualityControlItem.UnitId,
                description: "",
                referenceTransaction: importWasteTransaction);

        if (exportQualityControlTransaction.WarehouseId == null)
          throw new BaseTransactionHasNoWarehouseException(id: exportQualityControlTransaction.Id);

        #endregion
        #region Add Final Transaction
        lena.Domains.TransactionType finalTransactionType = Models.StaticData.StaticTransactionTypes.ImportAvailable;
        if (conditionalQualityControlStatus == ConditionalQualityControlStatus.Rejected)
        {
          finalTransactionType = Models.StaticData.StaticTransactionTypes.ImportWaste;
        }
        if (conditionalQualityControlStatus == ConditionalQualityControlStatus.NoReviews)
        {
          finalTransactionType = Models.StaticData.StaticTransactionTypes.ImportWaste;
        }

        App.Internals.WarehouseManagement.AddWarehouseTransaction(
                      transactionBatchId: transactionBatch.Id,
                      effectDateTime: transactionBatch.DateTime,
                      stuffId: conditionalQualityControlItem.StuffId,
                      billOfMaterialVersion: version,
                      stuffSerialCode: conditionalQualityControlItem.StuffSerialCode,
                      warehouseId: exportQualityControlTransaction.WarehouseId.Value,
                      transactionTypeId: finalTransactionType.Id,
                      amount: conditionalQualityControlItem.Qty,
                      unitId: conditionalQualityControlItem.UnitId,
                      description: "",
                      referenceTransaction: exportQualityControlTransaction);

        #endregion
        if (!stuffIds.Contains(conditionalQualityControlItem.StuffId))
          stuffIds.Add(conditionalQualityControlItem.StuffId);

      }
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: conditionalQualityControlId,
              scrumTaskType: ScrumTaskTypes.ResponseConditionalQualityControl);
      #endregion
      #region Edit ConditionalQualityControl 
      EditConditionalQualityControl(id: conditionalQualityControlId,
              rowVersion: conditionalQualityControlRowVersoin,
              conditionalQualityControlStatus: conditionalQualityControlStatus,
              responseConditionalConfirmationUserId: responseConditionalQualityControl.UserId,
              responseConditionalConfirmationDate: responseConditionalQualityControl.DateTime);
      #endregion
      #region ResetQualityControlSummaryByQualityControlId
      ResetQualityControlSummaryByQualityControlId(qualityControlId: responseConditionalQualityControl.ConditionalQualityControl
              .QualityControlConfirmation.QualityControl.Id);
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

      #region Notify To Department               
      var currentEmployeeId = App.Providers.Security.CurrentLoginData.UserEmployeeId ?? 0;
      var employee = App.Internals.UserManagement.GetEmployee(selector: e => e, id: currentEmployeeId);
      var departmentId = employee.DepartmentId ?? (int)Departments.QualityControl;
      var employeeFullName = employee.FirstName + " " + employee.LastName;

      string title = null;
      if (conditionalQualityControlStatus == ConditionalQualityControlStatus.Accepted)
        title = " تایید کنترل کیفی مشروط توسط " + employeeFullName;
      if (conditionalQualityControlStatus == ConditionalQualityControlStatus.Rejected)
        title = "رد کنترل کیفی مشروط توسط: " + employeeFullName;
      if (conditionalQualityControlStatus == ConditionalQualityControlStatus.NoReviews)
        title = "عدم بررسی کنترل کیفی مشروط توسط: " + employeeFullName;

#if !DEBUG

                App.Internals.Notification.NotifyToDepartment(
                departmentId: departmentId,
                title: title,
                description: description,
                scrumEntityId: null)
                ;

#endif
      #endregion

      return responseConditionalQualityControl;
    }
    #endregion
    #region Edit
    public ResponseConditionalQualityControl EditResponseConditionalQualityControl(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<int> conditionalQualityControlId = null,
        TValue<ConditionalQualityControlStatus> conditionalQualityControlStatus = null
        )
    {

      var responseConditionalQualityControl = GetResponseConditionalQualityControl(id: id);
      return EditResponseConditionalQualityControl(
                    responseConditionalQualityControl: responseConditionalQualityControl,
                    rowVersion: rowVersion,
                    description: description,
                    conditionalQualityControlId: conditionalQualityControlId,
                    conditionalQualityControlStatus: conditionalQualityControlStatus
                    );
    }
    public ResponseConditionalQualityControl EditResponseConditionalQualityControl(
        ResponseConditionalQualityControl responseConditionalQualityControl,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<int> conditionalQualityControlId = null,
        TValue<ConditionalQualityControlStatus> conditionalQualityControlStatus = null
        )
    {

      if (conditionalQualityControlId != null)
        responseConditionalQualityControl.ConditionalQualityControl.Id = conditionalQualityControlId;
      if (conditionalQualityControlStatus != null)
        responseConditionalQualityControl.Status = conditionalQualityControlStatus;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: responseConditionalQualityControl,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as ResponseConditionalQualityControl;
    }
    #endregion
    #region Get
    public ResponseConditionalQualityControl GetResponseConditionalQualityControl(int id) => GetResponseConditionalQualityControl(selector: e => e, id: id);
    public TResult GetResponseConditionalQualityControl<TResult>(
        Expression<Func<ResponseConditionalQualityControl, TResult>> selector,
        int id)
    {

      var responseConditionalQualityControl = GetResponseConditionalQualityControls(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (responseConditionalQualityControl == null)
        throw new ResponseConditionalQualityControlNotFoundException(id);
      return responseConditionalQualityControl;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetResponseConditionalQualityControls<TResult>(
        Expression<Func<ResponseConditionalQualityControl, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> conditionalQualityControlId = null,
        TValue<ConditionalQualityControlStatus> status = null,
        TValue<int> qualityControlId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<ResponseConditionalQualityControl>();
      if (conditionalQualityControlId != null)
        query = query.Where(i => i.ConditionalQualityControl.Id == conditionalQualityControlId);
      if (status != null)
        query = query.Where(i => i.Status == status);
      if (qualityControlId != null)
        query = query.Where(i => i.ConditionalQualityControl.QualityControlConfirmation.QualityControl.Id == qualityControlId);
      return query.Select(selector);
    }
    #endregion
  }
}

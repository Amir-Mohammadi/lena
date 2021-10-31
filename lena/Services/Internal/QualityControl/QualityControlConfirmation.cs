using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
//using LinqLib.Sort;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.QualityControl.QualityControlConfirmation;
using lena.Models.QualityControl.QualityControlConfirmationItem;
using lena.Models.QualityControl.QualityControlConfirmationTest;
using lena.Models.QualityControl.QualityControlItem;


using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region AddProcess
    public QualityControlConfirmation AddQualityControlConfirmationProcess(
        TransactionBatch transactionBatch,
        string description,
        int qualityControlId,
        byte[] qualityControlRowVersion,
        bool confirmed,
        bool applayInAllItems,
        string fileKey,
        AddQualityControlConfirmationItemInput[] qualityControlConfirmationItems,
        AddQualityControlConfirmationTestInput[] qualityControlConfirmationTests)
    {

      var uncheckedQualityControlItem = new List<QualityControlItem>();

      #region Get Status
      var status = confirmed ? QualityControlStatus.Accepted : QualityControlStatus.Rejected;
      #endregion
      #region Add QualityControlConfirmation

      var QC = App.Internals.QualityControl.GetQualityControl(id: qualityControlId);

      var stuff = App.Internals.SaleManagement.GetStuff(selector: e => new
      {
        QualityControlDepartmentId = e.QualityControlDepartmentId,
        QualityControlEmployeeId = e.QualityControlEmployeeId,
        e.StuffPurchaseCategoryId,
        Code = e.Code
      },
                id: QC.StuffId);

      if (stuff.StuffPurchaseCategoryId != null)
      {
        var stuffPurchaseCategory = App.Internals.Supplies.GetStuffPurchaseCategory(
                  e => e,
                  id: stuff.StuffPurchaseCategoryId.Value);

        var memberships = App.Internals.UserManagement.GetMemberships(
                  selector: e => e,
                  userId: App.Providers.Security.CurrentLoginData.UserId,
                  userGroupId: stuffPurchaseCategory.QualityControlUserGroupId);

        var qcUserGroup = App.Internals.UserManagement.GetUserGroup(
                      id: stuffPurchaseCategory.QualityControlUserGroupId);

        if (!memberships.Any())
        {
          throw new NotHavePermissionToQualityControlStuffException(
                    stuffCode: stuff.Code,
                    validUserGroupId: stuffPurchaseCategory.QualityControlUserGroupId,
                    validUserGroupName: qcUserGroup.Name);
        }
      }

      var qualityControlConfirmation = AddQualityControlConfirmation(
                    qualityControlConfirmation: null,
                    transactionBatch: null,
                    description: description,
                    qualityControlId: qualityControlId,
                    status: status);
      #endregion
      #region Edit QualityControl
      var qualityControl = EditQualityControl(
                              id: qualityControlId,
                              rowVersion: qualityControlRowVersion,
                              status: status,
                              confirmationUserId: qualityControlConfirmation.UserId,
                              confirmationDateTime: qualityControlConfirmation.DateTime,
                              uploadFileData: fileKey != null ? App.Providers.Session.GetAs<UploadFileData>(fileKey) : null);
      #endregion


      #region AddQualityControlPaymentSuggest

      if (status == QualityControlStatus.Rejected)
      {
        AddQualityControlPaymentSuggestProcess(
                      qualityControlId: qualityControlId,
                      qualityControlPaymentSuggestStatus: QualityControlPaymentSuggestStatus.NoPayement,
                      description: "ثبت اتوماتیک به دلیل رد شدن توسط کنترل کیفی"
                      );
      }
      else if (status == QualityControlStatus.Accepted)
      {
        AddQualityControlPaymentSuggestProcess(
                     qualityControlId: qualityControlId,
                     qualityControlPaymentSuggestStatus: QualityControlPaymentSuggestStatus.FullPayement,
                     description: "ثبت اتوماتیک به دلیل تایید شدن توسط کنترل کیفی"
                     );
      }
      #endregion

      #region Add QualityControlConfirmationTests
      foreach (var qualityControlConfirmationTest in qualityControlConfirmationTests)
      {
        var qualityControlConfirmationTestResult = AddQualityControlConfirmationTest(
                      qualityControlConfirmationId: qualityControlConfirmation.Id,
                      stuffId: qualityControlConfirmationTest.StuffId,
                      qualityControlTestId: qualityControlConfirmationTest.QualityControlTestId,
                      testConditionId: qualityControlConfirmationTest.TestConditionId,
                      status: qualityControlConfirmationTest.Status,
                      aqlAmount: qualityControlConfirmationTest.AQLAmount,
                      description: qualityControlConfirmationTest.Description
                      );

        #region Add QualityControlConfirmationTestItems
        foreach (var qualityControlConfirmationTestItem in qualityControlConfirmationTest.AddQualityControlConfirmationTestItemsInput)
        {
          var qualityControlConfirmationTestItemResult = AddQualityControlConfirmationTestItem(
                                                                    qualityControlConfirmationTestId: qualityControlConfirmationTestResult.Id,
                                                                    testerUserId: qualityControlConfirmationTestItem.TesterUserId,
                                                                    sampleName: qualityControlConfirmationTestItem.SampleName,
                                                                    obtainAmount: qualityControlConfirmationTestItem.ObtainAmount,
                                                                    minObtainAmount: qualityControlConfirmationTestItem.MinObtainAmount,
                                                                    maxObtainAmount: qualityControlConfirmationTestItem.MaxObtainAmount);
        }
        #endregion
      }
      #endregion
      #region Get QualityControlItems
      var qualityControlItems = GetQualityControlItems(
              selector: e => e,
              isDelete: false,
              qualityControlId: qualityControlId)


          .ToList();
      #endregion
      #region Add QualityControlConfirmationItems
      foreach (var qualityControlItem in qualityControlItems)
      {
        var qualityControlConfirmationItem =
                  qualityControlConfirmationItems.FirstOrDefault(i =>
                      i.QualityControlItemId == qualityControlItem.Id);
        if (applayInAllItems || qualityControlConfirmationItem != null)
        {
          if (qualityControlConfirmationItem != null)
          {
            foreach (var qcSample in qualityControlConfirmationItem.EditQualityControlSampleInputs)
            {
              #region Check QCSampleStatus
              if (qcSample.Status == QualityControlSampleStatus.InWarehouse && qcSample.DelivarySampleToWarehouse)
                throw new QualityControlSampleStatusInWarehouseStatusException(id: qcSample.Id);

              #endregion
              EditQualityControlSampleProcess(
                  id: qcSample.Id,
                  status: qcSample.Status,
                  testQty: qcSample.TestQty,
                  consumeQty: qcSample.ConsumeQty,
                  qualityControlId: qualityControlId,
                  qualityControlItemId: qcSample.QualityControlItemId,
                  delivarySampleToWarehouse: qcSample.DelivarySampleToWarehouse,
                  rowVersion: qcSample.RowVersion);
            }
          }

          AddQualityControlConfirmationItemProcess(
                        transactionBatch: null,
                        description: qualityControlConfirmationItem?.Description,
                        testQty: qualityControlConfirmationItem?.TestQty ?? 0,
                        consumeQty: qualityControlConfirmationItem?.ConsumeQty ?? 0,
                        unitId: qualityControlConfirmationItem?.UnitId ?? qualityControlItem.UnitId,
                        qualityControlItemId: qualityControlConfirmationItem?.QualityControlItemId ??
                                              qualityControlItem.Id,
                        qualityControlConfirmationId: qualityControlConfirmation.Id);

          if (!string.IsNullOrEmpty(qualityControlConfirmationItem?.Description))
            App.Internals.WarehouseManagement.SetStuffSerialQualityControlDescription(
                          stuffSerial: qualityControlItem.StuffSerial,
                          rowVersion: qualityControlItem.StuffSerial.RowVersion,
                          qualityControlDescription: qualityControlConfirmationItem.Description);
        }
        else
        {
          uncheckedQualityControlItem.Add(qualityControlItem);
        }
      }
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: qualityControlId,
              scrumTaskType: ScrumTaskTypes.QualityControlConfirmation);
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
      #region Add QualityControl if need
      if (uncheckedQualityControlItem.Any())
      {
        #region AddTransactionBatch
        var newTransactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
        #endregion
        #region Remove QualityControlItem and add to AddQualityControlItemTransactionInputs
        var addQualityControlItemTransactionInputs = new List<AddQualityControlItemTransactionInput>();
        foreach (var qualityControlItem in uncheckedQualityControlItem)
        {
          RemoveQualityControlItemProcess(
                        transactionBatchId: newTransactionBatch.Id,
                        id: qualityControlItem.Id,
                        rowVersion: qualityControlItem.RowVersion);
          var addQualityControlItemTransactionInput = new AddQualityControlItemTransactionInput
          {
            StuffSerialCode = qualityControlItem.StuffSerialCode,
            Amount = qualityControlItem.Qty,
            UnitId = qualityControlItem.UnitId,
            Description = null
          };
          addQualityControlItemTransactionInputs.Add(addQualityControlItemTransactionInput);
        }
        #endregion

        #region AddQualityControlProcess
        var uncheckedQty = uncheckedQualityControlItem.Sum(i => i.Qty * i.Unit.ConversionRatio) /
                  qualityControl.Unit.ConversionRatio;
        if (qualityControl.QualityControlType == QualityControlType.ReceiptQualityControl)
        {
          AddReceiptQualityControlProcess(
                        receiptQualityControl: null,
                        transactionBatch: null,
                        transactionType: Models.StaticData.StaticTransactionTypes.ExportQualityControl,
                        storeReceiptId: (qualityControl as ReceiptQualityControl).StoreReceiptId,
                        stuffId: qualityControl.StuffId,
                        warehouseId: qualityControl.WarehouseId,
                        description: qualityControl.Description,
                        addQualityControlItemTransactionInputs: addQualityControlItemTransactionInputs
                            .ToArray());
        }
        else if (qualityControl.QualityControlType == QualityControlType.CustomQualityControl)
        {
          AddCustomQualityControlProcess(
                        customQualityControl: null,
                        transactionBatch: null,
                        stuffId: qualityControl.StuffId,
                        warehouseId: qualityControl.WarehouseId,
                        description: qualityControl.Description,
                        addQualityControlItemTransactionInputs: addQualityControlItemTransactionInputs
                            .ToArray());
        }
        else if (qualityControl.QualityControlType == QualityControlType.ProductionQualityControl)
        {
          AddProductionQualityControlProcess(
                        productionQualityControl: null,
                        transactionBatch: null,
                        stuffId: qualityControl.StuffId,
                        warehouseId: qualityControl.WarehouseId,
                        description: qualityControl.Description,
                        addQualityControlItemTransactionInputs: addQualityControlItemTransactionInputs
                            .ToArray());
        }
        #endregion
      }
      #endregion
      #region ResetQualityControlSummaryByQualityControlId
      ResetQualityControlSummaryByQualityControlId(qualityControlId: qualityControl.Id);
      #endregion

      #region Notify To Department             

      var qualityControlResult = GetQualityControl(e => new
      {
        CooperatorName = (e as ReceiptQualityControl).StoreReceipt.Cooperator.Name,
        StuffCode = e.Stuff.Code,
        Code = e.Code,
        Qty = e.Qty,
      },
           id: qualityControlId);

      var qualityControlCooperatorName = qualityControlResult.CooperatorName;
      var qualityControlStuffCode = qualityControlResult.StuffCode;
      var qualityControlCode = qualityControlResult.Code;
      var qualityControlQty = qualityControlResult.Qty;
      var title = "";

      if (status == QualityControlStatus.Accepted)
        title = $"تایید کنترل کیفی: { qualityControlCode }" + Environment.NewLine +
                      $"تعداد کنترل کیفی: { qualityControlQty }" + Environment.NewLine +
                      $"کد کالا: { qualityControlStuffCode }" + Environment.NewLine +
                      $"تامین کننده: { qualityControlCooperatorName }";
      else if (status == QualityControlStatus.Rejected)
        title = $"رد کنترل کیفی {qualityControlCode}" + Environment.NewLine +
                      $"تعداد کنترل کیفی  {qualityControlQty}" + Environment.NewLine +
                      $"کد کالا {qualityControlStuffCode}" + Environment.NewLine +
                      $"تامین کننده: {qualityControlCooperatorName}";

      App.Internals.Notification.NotifyToDepartment(
                departmentId: (int)Departments.Planning,
                title: title,
                description: description,
                scrumEntityId: null);

      App.Internals.Notification.NotifyToDepartment(
                departmentId: (int)Departments.Warehouse,
                title: title,
                description: description,
                scrumEntityId: null);

      #endregion
      return qualityControlConfirmation;
    }
    #endregion
    #region Add
    public QualityControlConfirmation AddQualityControlConfirmation(
        QualityControlConfirmation qualityControlConfirmation,
        TransactionBatch transactionBatch,
        string description,
        int qualityControlId,
        QualityControlStatus status)
    {

      qualityControlConfirmation = qualityControlConfirmation ?? repository.Create<QualityControlConfirmation>();
      var qualityControl = GetQualityControl(id: qualityControlId);
      qualityControlConfirmation.QualityControl = qualityControl;
      qualityControlConfirmation.Status = status;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: qualityControlConfirmation,
                    transactionBatch: transactionBatch,
                    description: description);
      return qualityControlConfirmation;
    }
    #endregion
    #region Edit
    public QualityControlConfirmation EditQualityControlConfirmation(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<int> qualityControlId = null,
        TValue<QualityControlStatus> status = null)
    {

      var qualityControlConfirmation = GetQualityControlConfirmation(id: id);
      return EditQualityControlConfirmation(
                    qualityControlConfirmation: qualityControlConfirmation,
                    rowVersion: rowVersion,
                    description: description,
                    qualityControlId: qualityControlId,
                    status: status);
    }
    public QualityControlConfirmation EditQualityControlConfirmation(
        QualityControlConfirmation qualityControlConfirmation,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<int> qualityControlId = null,
        TValue<QualityControlStatus> status = null
    )
    {

      if (qualityControlId != null)
      {
        var qualityControl = GetQualityControl(id: qualityControlId);
        qualityControlConfirmation.QualityControl = qualityControl;

      }
      if (status != null)
        qualityControlConfirmation.Status = status;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: qualityControlConfirmation,
                    rowVersion: rowVersion,
                    description: description);
      return retValue as QualityControlConfirmation;
    }
    #endregion
    #region Get
    public QualityControlConfirmation GetQualityControlConfirmation(int id) => GetQualityControlConfirmation(selector: e => e, id: id);
    public TResult GetQualityControlConfirmation<TResult>(
            Expression<Func<QualityControlConfirmation, TResult>> selector,
            int id)
    {

      var qualityControlConfirmation = GetQualityControlConfirmations(
                selector: selector,
                id: id)


            .FirstOrDefault();
      if (qualityControlConfirmation == null)
        throw new QualityControlConfirmationNotFoundException(id: id);
      return qualityControlConfirmation;
    }
    public QualityControlConfirmation GetQualityControlConfirmationByQualityControlId(int qualityControlId) =>
        GetQualityControlConfirmationByQualityControlId(selector: e => e, qualityControlId: qualityControlId);
    public TResult GetQualityControlConfirmationByQualityControlId<TResult>(
        Expression<Func<QualityControlConfirmation, TResult>> selector,
        int qualityControlId)
    {

      var qualityControlConfirmation = GetQualityControlConfirmations(
                    selector: selector,
                    qualityControlId: qualityControlId,
                    isDelete: false)


                .FirstOrDefault();
      if (qualityControlConfirmation == null)
        throw new QualityControlConfirmationNotFoundException(qualityControlId: qualityControlId);
      return qualityControlConfirmation;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetQualityControlConfirmations<TResult>(
        Expression<Func<QualityControlConfirmation, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> qualityControlId = null,
        TValue<QualityControlStatus> status = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<lena.Domains.QualityControlConfirmation>();
      if (qualityControlId != null)
        query = query.Where(i => i.QualityControl.Id == qualityControlId);
      if (status != null)
        query = query.Where(i => i.Status == status);
      return query.Select(selector);
    }
    #endregion
    #region CheckTask
    //public void CheckQualityControlConfirmationTask(lena.Domains.QualityControlConfirmation qualityControlConfirmation)
    //{
    //    
    //        #region Get QualityControl
    //        var qualityControl = GetQualityControl(id: qualityControlConfirmation.Id)
    //        
    //;
    //        #endregion
    //        #region Get ProjectWorkItem
    //        var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
    //                baseEntityId: qualityControl.Id,
    //                scrumTaskType: ScrumTaskTypes.QualityControlConfirmation)
    //            
    //;
    //        if (projectWorkItem == null)
    //        {
    //            projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
    //                baseEntityId: qualityControl.Id,
    //                scrumTaskType: ScrumTaskTypes.QualityControlConfirmation)
    //            
    //;
    //        }
    //        #endregion
    //        #region check QualityControlConfirmationItems and DoneProjectWorkItem
    //        var qualityControlQty = qualityControl.Qty * qualityControl.Unit.ConversionRatio;
    //        var qualityControlConfirmationItems = GetQualityControlConfirmationItems(
    //                selector: e => e,
    //                qualityControlId: qualityControl.Id,
    //                isDelete: false)
    //            
    //;
    //        var sumQualityControlConfirmationItemsQty = qualityControlConfirmationItems.Any()
    //                ? qualityControlConfirmationItems.Sum(i => i.QualityControlItem.Qty)
    //                : 0;
    //        var taskIsDone = qualityControlQty == sumQualityControlConfirmationItemsQty;
    //        if (taskIsDone && projectWorkItem.ScrumTaskStep != ScrumTaskStep.Done)
    //        {
    //            #region DoneTask
    //            App.Internals.ScrumManagement.DoneScrumTask(
    //                    scrumTask: projectWorkItem,
    //                    rowVersion: projectWorkItem.RowVersion)
    //                
    //;
    //            #endregion
    //        }
    //        else if (taskIsDone == false && projectWorkItem.ScrumTaskStep == ScrumTaskStep.Done)
    //        {
    //            #region Add QualityControlConfirmation Task
    //            App.Internals.ProjectManagement.AddProjectWorkItem(
    //                    projectWorkItem: null,
    //                    name: $"تایید کنترل کیفی {qualityControl.Code} ",
    //                    description: "",
    //                    color: "",
    //                    departmentId: (int)Departments.QualityControl,
    //                    estimatedTime: 10800,
    //                    isCommit: false,
    //                    scrumTaskTypeId: (int)ScrumTaskTypes.QualityControlConfirmation,
    //                    userId: null,
    //                    spentTime: 0,
    //                    remainedTime: 0,
    //                    scrumTaskStep: ScrumTaskStep.ToDo,
    //                    projectWorkId: projectWorkItem.ScrumBackLogId,
    //                    baseEntityId: qualityControl.Id)
    //                
    //;
    //            #endregion
    //        }
    //        #endregion
    //        if(qualityControlConfirmation.Status == )
    //        #region Add
    //        #endregion
    //    });
    //}
    //public void CheckQualityControlConfirmationTask(int qualityControlConfirmationId)
    //{
    //    
    //        var qualityControlConfirmation = GetQualityControlConfirmation(id: qualityControlConfirmationId)
    //            
    //;
    //        CheckQualityControlConfirmationTask(qualityControlConfirmation: qualityControlConfirmation)
    //           
    //;
    //    });
    //}
    #endregion
    #region Sort
    public IOrderedQueryable<QualityControlConfirmationResult> SortQualityControlConfirmationResult(
        IQueryable<QualityControlConfirmationResult> query,
        SortInput<QualityControlConfirmationSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlConfirmationSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case QualityControlConfirmationSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case QualityControlConfirmationSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<QualityControlConfirmationResult> SearchQualityControlConfirmationResult(
        IQueryable<QualityControlConfirmationResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.Description.Contains(search)
                select item;
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<QualityControlConfirmation, QualityControlConfirmationResult>> ToQualityControlConfirmationResult =
        qualityControlConfirmation => new QualityControlConfirmationResult
        {
          Id = qualityControlConfirmation.Id,
          Code = qualityControlConfirmation.Code,
          QualityControlId = qualityControlConfirmation.QualityControl.Id,
          QualityControlCode = qualityControlConfirmation.QualityControl.Code,
          StuffId = qualityControlConfirmation.QualityControl.StuffId,
          StuffCode = qualityControlConfirmation.QualityControl.Stuff.Code,
          StuffName = qualityControlConfirmation.QualityControl.Stuff.Name,
          QualityControlType = qualityControlConfirmation.QualityControl.QualityControlType,
          WarehouseId = qualityControlConfirmation.QualityControl.WarehouseId,
          WarehouseName = qualityControlConfirmation.QualityControl.Warehouse.Name,
          Status = qualityControlConfirmation.Status,
          Qty = qualityControlConfirmation.QualityControl.Qty,
          UnitId = qualityControlConfirmation.QualityControl.UnitId,
          UnitName = qualityControlConfirmation.QualityControl.Unit.Name,
          AcceptedQty = (double?)qualityControlConfirmation.QualityControl.QualityControlSummary.AcceptedQty,
          FailedQty = (double?)qualityControlConfirmation.QualityControl.QualityControlSummary.FailedQty,
          ConditionalRequestQty = (double?)qualityControlConfirmation.QualityControl.QualityControlSummary.ConditionalRequestQty,
          ConditionalQty = (double?)qualityControlConfirmation.QualityControl.QualityControlSummary.ConditionalQty,
          ReturnedQty = (double?)qualityControlConfirmation.QualityControl.QualityControlSummary.ReturnedQty,
          ConsumedQty = (double?)qualityControlConfirmation.QualityControl.QualityControlSummary.ConsumedQty,
          QualityControlDescription = qualityControlConfirmation.QualityControl.Description,
          QualityControlConfirmationTests = qualityControlConfirmation.QualityControlConfirmationTests.AsQueryable().Select(App.Internals.QualityControl.ToQualityControlConfirmationTestResult),
          Description = qualityControlConfirmation.Description,
          RowVersion = qualityControlConfirmation.RowVersion,

        };
    #endregion
  }
}

using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.QualityControl.QualityControlSample;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {

    #region Get QualityControlSample
    public QualityControlSample GetQualityControlSample(int id) => GetQualityControlSample(selector: e => e, id: id);
    public TResult GetQualityControlSample<TResult>(
        Expression<Func<QualityControlSample, TResult>> selector,
        int id)
    {

      var qualityControlSample = GetQualityControlSamples(selector: selector, id: id)


                .FirstOrDefault();

      if (qualityControlSample == null)
        throw new QualityControlSampleNotFoundException(id);
      return qualityControlSample;
    }
    #endregion

    #region Gets GetQualityControlSample     

    public IQueryable<TResult> GetQualityControlSamples<TResult>(
        Expression<Func<QualityControlSample, TResult>> selector,
        TValue<long> id = null,
        TValue<int> qty = null,
        TValue<int> qualityControlItemId = null,
        TValue<QualityControlSampleStatus> status = null)
    {

      var query = repository.GetQuery<QualityControlSample>();
      if (id != null)
        query = query.Where(m => m.Id == id);
      if (qty != null)
        query = query.Where(m => m.Qty == qty);
      if (status != null)
        query = query.Where(m => m.Status == status);
      if (qualityControlItemId != null)
        query = query.Where(m => m.QualityControlItemId == qualityControlItemId);
      return query.Select(selector);
    }
    #endregion

    #region Gets FullQualityControlSample
    public IQueryable<QualityControlSampleFullResult> GetFullQualityControlSamples(
        TValue<int> qualityControlId = null,
        TValue<int> qualityControlItemId = null,
        TValue<int> stuffId = null,
        TValue<string> serial = null,
        TValue<int> warehouseId = null,
        TValue<QualityControlSampleStatus> status = null,
        TValue<QualityControlSampleStatus[]> statuses = null
        )
    {

      var qualityControlSamples = GetQualityControlSamples(e => e);
      var qualityControls = App.Internals.QualityControl.GetQualityControls(
                e => e,
                isDelete: false); ; var qualityControlItems = App.Internals.QualityControl.GetQualityControlItems(e => e, qualityControlId: qualityControlId);

      var query = (
                from qualityControlItem in qualityControlItems
                join qualityControl in qualityControls on qualityControlItem.QualityControlId equals qualityControl.Id
                join qualityControlSample in qualityControlSamples on qualityControlItem.Id equals qualityControlSample.QualityControlItemId into temp
                from qcSample in temp.DefaultIfEmpty()
                select new QualityControlSampleFullResult()
                {
                  QualityControlId = qualityControl.Id,
                  QualityControlCode = qualityControl.Code,
                  QualityControlQty = qualityControl.Qty,
                  QualityControlStatus = qualityControl.Status,

                  QualityControlItemId = qualityControlItem.Id,
                  QualityControlItemQty = qualityControlItem.Qty,
                  QualityControlItemStatus = qualityControlItem.Status,
                  QualityControlItemDescription = qualityControlItem.Description,

                  QualityControlSampleId = qcSample.Id,
                  DateTime = qcSample.DateTime,
                  EmployeeFullName = qcSample.User.Employee.FirstName + " " + qcSample.User.Employee.LastName,
                  DepartementName = qcSample.User.Employee.Department.Name,
                  StatusChangerEmployeeFullName = qcSample.StatusChangerUser.Employee.FirstName + " " + qcSample.StatusChangerUser.Employee.LastName,
                  QualityControlSampleCode = qcSample.Code,
                  QualityControlSampleQty = qcSample.Qty,
                  QualityControlSampleStatus = qcSample.Status,
                  TestQty = qcSample.TestQty,
                  ConsumeQty = qcSample.ConsumeQty,
                  QualityControlSampleRowVersion = qcSample.RowVersion,

                  StuffId = qualityControlItem.StuffId,
                  StuffCode = qualityControlItem.Stuff.Code,
                  StuffName = qualityControlItem.Stuff.Name,
                  StuffSerialCode = qualityControlItem.StuffSerialCode,
                  Serial = qualityControlItem.StuffSerial.Serial,
                  UnitId = qualityControlItem.UnitId,
                  UnitName = qualityControlItem.Unit.Name,

                  WarehouseId = qualityControl.WarehouseId,
                  WarehouseName = qualityControl.Warehouse.Name
                });

      if (qualityControlId != null)
        query = query.Where(i => i.QualityControlId == qualityControlId);
      if (qualityControlItemId != null)
        query = query.Where(i => i.QualityControlItemId == qualityControlItemId);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (warehouseId != null)
        query = query.Where(i => i.WarehouseId == warehouseId);
      if (serial != null)
        query = query.Where(i => i.Serial == serial);
      if (status != null)
        query = query.Where(i => i.QualityControlSampleStatus == status);
      if (statuses != null)
        query = query.Where(i => statuses.Value.Contains(i.QualityControlSampleStatus.Value));

      return query;
    }
    #endregion

    #region Add QualityControlSample
    public QualityControlSample AddQualityControlSampleProcess(
    double qty,
    double sumInputQty,
    int qualityControlId,
    QualityControlSampleStatus status,
    int qualityControlItemId)
    {


      #region Check Permission
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
          throw new NotHavePermissionToQualityControlSampleStuffException(
                    stuffCode: stuff.Code,
                    validUserGroupId: stuffPurchaseCategory.QualityControlUserGroupId,
                    validUserGroupName: qcUserGroup.Name);
        }
      }

      #endregion
      var random = new Random();
      var qualityControlSamples = GetQualityControlSamples(e => e, qualityControlItemId: qualityControlItemId);
      var sumQty = qualityControlSamples.Where(i => i.Status == QualityControlSampleStatus.InWarehouse ||
                                                     i.Status == QualityControlSampleStatus.InQualityControl)
                                              .Select(a => a.Qty).DefaultIfEmpty(0).Sum();

      var qualityControlItem = App.Internals.QualityControl.GetQualityControlItem(id: qualityControlItemId);
      if (qualityControlItem.Qty < (sumQty + sumInputQty))
        throw new QualityControlSampleNotFoundException(qualityControlItem.Id);

      var serial = qualityControlItem.StuffSerial.Serial;
      var code = "SAMPLE-" + (Math.Round(Math.Abs(random.NextDouble() * 10000))).ToString() + serial;

      var equivalentStuff = AddQualityControlSampleProcess(
                   qualityControlSample: null,
                   qty: qty,
                   code: code,
                   status: status,
                   qualityControlItemId: qualityControlItemId);
      return equivalentStuff;
    }

    public QualityControlSample AddQualityControlSampleProcess(
        QualityControlSample qualityControlSample,
        double qty,
        string code,
        int qualityControlItemId,
        QualityControlSampleStatus status)
    {

      #region Check QualityControlAmount To Take Sample
      var qualityControlItemResult = App.Internals.QualityControl.GetQualityControlItem(id: qualityControlItemId);

      var warehouseInventory = App.Internals.WarehouseManagement.GetStuffSerialInventories(
               stuffId: qualityControlItemResult.StuffId,
               stuffSerialCode: qualityControlItemResult.StuffSerialCode,
               serial: qualityControlItemResult.StuffSerial.Serial)


           .FirstOrDefault();

      var remainQualityControlAmount = warehouseInventory.QualityControlAmount - qty;
      if (remainQualityControlAmount < 0.0)
      {
        throw new SerialHasNotEnoughQualityControlAmountToTakeSampleException(qualityControlItemResult.StuffSerial.Serial);
      }
      #endregion

      #region Add QualityControlSample
      var qualityControlSampleResult = AddQualityControlSample(
          qualityControlSample: qualityControlSample,
               qty: qty,
               code: code,
               qualityControlItemId: qualityControlItemId,
               status: QualityControlSampleStatus.InWarehouse);
      #endregion

      #region Get Version and WarhouseId
      var version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
            stuffSerialCode: qualityControlItemResult.StuffSerialCode,
            stuffId: qualityControlItemResult.StuffId);
      var warehouseId = warehouseInventory.WarehouseId;
      #endregion

      #region AddTransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion

      #region Add TakeSample WarehouseTransaction
      var takeSampleOfQcTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
              warehouseId: warehouseId,
              transactionBatchId: transactionBatch.Id,
              effectDateTime: transactionBatch.DateTime,
              stuffId: qualityControlItemResult.StuffId,
              billOfMaterialVersion: version,
              stuffSerialCode: qualityControlItemResult.StuffSerialCode,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.TakeSample.Id,
              amount: qty,
              unitId: qualityControlItemResult.UnitId,
              description: "برداشتن نمونه از سطح کنترل کیفی",
              referenceTransaction: null);
      #endregion

      return qualityControlSampleResult;
    }

    #endregion

    #region Add
    public QualityControlSample AddQualityControlSample(
        QualityControlSample qualityControlSample,
        double qty,
        string code,
        int qualityControlItemId,
        QualityControlSampleStatus status)
    {

      qualityControlSample = qualityControlSample ?? repository.Create<QualityControlSample>();
      qualityControlSample.Qty = qty;
      qualityControlSample.Code = code;
      qualityControlSample.Status = status;
      qualityControlSample.UserId = App.Providers.Security.CurrentLoginData.UserId;
      qualityControlSample.DateTime = DateTime.UtcNow;
      qualityControlSample.QualityControlItemId = qualityControlItemId;
      repository.Add(qualityControlSample);
      return qualityControlSample;
    }
    #endregion

    #region Edit QualityControlSampleProcess
    public QualityControlSample EditQualityControlSampleProcess(
            int id,
            byte[] rowVersion,
            TValue<double> qty = null,
            TValue<double> testQty = null,
            TValue<double> consumeQty = null,
            TValue<int> qualityControlId = null,
            TValue<int> qualityControlItemId = null,
            TValue<bool> delivarySampleToWarehouse = null,
            TValue<QualityControlSampleStatus> status = null
            )
    {

      var qualityControlSample = GetQualityControlSample(id: id);

      //var qualityControlItem = App.Internals.QualityControl.GetQualityControlItem(id: qualityControlItemId)
      // 
      //;
      var statusChangerUserId = App.Providers.Security.CurrentLoginData.UserId;
      var editQualityControlSample = EditQualityControlSample(
                    qty: qty,
                    status: status,
                    testQty: testQty,
                    consumeQty: consumeQty,
                    statusChangerUserId: statusChangerUserId,
                    qualityControlSample: qualityControlSample,
                    qualityControlItemId: qualityControlItemId,
                    rowVersion: qualityControlSample.RowVersion);

      #region QCSampleDeliverWarehouse
      if (delivarySampleToWarehouse)
      {
        QCSampleDeliverWarehouse(
                  id: editQualityControlSample.Id,
                  qualityControlId: qualityControlId,
                  qualityControlItemId: qualityControlItemId);
      }
      #endregion

      return editQualityControlSample;
    }

    public QualityControlSample EditQualityControlSample(
        QualityControlSample qualityControlSample,
        byte[] rowVersion,
        TValue<double> qty = null,
        TValue<double> testQty = null,
        TValue<double> consumeQty = null,
        TValue<int> statusChangerUserId = null,
        TValue<int> qualityControlItemId = null,
        TValue<QualityControlSampleStatus> status = null
        )
    {

      if (qty != null)
        qualityControlSample.Qty = qty;
      if (testQty != null)
        qualityControlSample.TestQty = testQty;
      if (consumeQty != null)
        qualityControlSample.ConsumeQty = consumeQty;
      if (status != null)
        qualityControlSample.Status = status;
      if (qualityControlItemId != null)
        qualityControlSample.QualityControlItemId = qualityControlItemId;
      if (statusChangerUserId != null)
        qualityControlSample.StatusChangerUserId = statusChangerUserId;

      repository.Update(rowVersion: rowVersion, entity: qualityControlSample);
      return qualityControlSample;
    }
    #endregion

    #region ToResult QualityControlSample
    public Expression<Func<QualityControlSample, QualityControlSampleResult>> ToQualityControlSampleResult =
        qualityControlSample => new QualityControlSampleResult
        {
          Id = qualityControlSample.Id,
          Code = qualityControlSample.Code,
          UserId = qualityControlSample.UserId,
          EmployeeFullName = qualityControlSample.User.Employee.FirstName + " " + qualityControlSample.User.Employee.LastName,
          StatusChangerUserId = qualityControlSample.StatusChangerUserId,
          StatusChangerEmployeeFullName = qualityControlSample.User.Employee.FirstName + " " + qualityControlSample.User.Employee.LastName,
          DateTime = qualityControlSample.DateTime,
          Qty = qualityControlSample.Qty,
          TestQty = qualityControlSample.TestQty ?? 0,
          ConsumeQty = qualityControlSample.ConsumeQty ?? 0,
          Status = qualityControlSample.Status,
          QualityControlItemId = qualityControlSample.QualityControlItemId,
          RowVersion = qualityControlSample.RowVersion
        };
    #endregion

    #region Sort QualityControlSample
    public IOrderedQueryable<QualityControlSampleFullResult> SortQualityControlSampleResult(
        IQueryable<QualityControlSampleFullResult> query,
        SortInput<QaulityControlSampleSorttype> sort)
    {
      switch (sort.SortType)
      {
        case QaulityControlSampleSorttype.StuffId:
          return query.OrderBy(m => m.StuffId, sort.SortOrder);
        case QaulityControlSampleSorttype.StuffCode:
          return query.OrderBy(m => m.StuffCode, sort.SortOrder);
        case QaulityControlSampleSorttype.StuffName:
          return query.OrderBy(m => m.StuffName, sort.SortOrder);
        case QaulityControlSampleSorttype.StuffSerialCode:
          return query.OrderBy(m => m.StuffSerialCode, sort.SortOrder);
        case QaulityControlSampleSorttype.Serial:
          return query.OrderBy(m => m.Serial, sort.SortOrder);
        case QaulityControlSampleSorttype.UnitId:
          return query.OrderBy(m => m.UnitId, sort.SortOrder);
        case QaulityControlSampleSorttype.UnitName:
          return query.OrderBy(m => m.UnitName, sort.SortOrder);
        case QaulityControlSampleSorttype.WarehouseId:
          return query.OrderBy(m => m.WarehouseId, sort.SortOrder);
        case QaulityControlSampleSorttype.WarehouseName:
          return query.OrderBy(m => m.WarehouseName, sort.SortOrder);
        case QaulityControlSampleSorttype.QualityControlSampleId:
          return query.OrderBy(m => m.QualityControlSampleId, sort.SortOrder);
        case QaulityControlSampleSorttype.QualityControlSampleCode:
          return query.OrderBy(m => m.QualityControlSampleCode, sort.SortOrder);
        case QaulityControlSampleSorttype.QualityControlSampleQty:
          return query.OrderBy(m => m.QualityControlSampleQty, sort.SortOrder);
        case QaulityControlSampleSorttype.QualityControlSampleStatus:
          return query.OrderBy(m => m.QualityControlSampleStatus, sort.SortOrder);
        case QaulityControlSampleSorttype.QualityControlItemId:
          return query.OrderBy(m => m.QualityControlItemId, sort.SortOrder);
        case QaulityControlSampleSorttype.QualityControlItemQty:
          return query.OrderBy(m => m.QualityControlItemQty, sort.SortOrder);
        case QaulityControlSampleSorttype.QualityControlItemStatus:
          return query.OrderBy(m => m.QualityControlItemStatus, sort.SortOrder);
        case QaulityControlSampleSorttype.QualityControlId:
          return query.OrderBy(m => m.QualityControlId, sort.SortOrder);
        case QaulityControlSampleSorttype.QualityControlQty:
          return query.OrderBy(m => m.QualityControlQty, sort.SortOrder);
        case QaulityControlSampleSorttype.QualityControlCode:
          return query.OrderBy(m => m.QualityControlCode, sort.SortOrder);
        case QaulityControlSampleSorttype.QualityControlStatus:
          return query.OrderBy(m => m.QualityControlStatus, sort.SortOrder);
        case QaulityControlSampleSorttype.DateTime:
          return query.OrderBy(m => m.DateTime, sort.SortOrder);
        case QaulityControlSampleSorttype.EmployeeFullName:
          return query.OrderBy(m => m.EmployeeFullName, sort.SortOrder);
        case QaulityControlSampleSorttype.StatusChangerEmployeeFullName:
          return query.OrderBy(m => m.StatusChangerEmployeeFullName, sort.SortOrder);
        case QaulityControlSampleSorttype.DepartementName:
          return query.OrderBy(m => m.DepartementName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search QaulityControlSample
    public IQueryable<QualityControlSampleFullResult> SearchQualityControlSampleResult(
    IQueryable<QualityControlSampleFullResult> query, string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where item.StuffName.Contains(search) ||
                      item.Serial.Contains(search) ||
                      item.StuffCode.Contains(search) ||
                      item.WarehouseName.Contains(search)
                select item;
      return query;
    }
    #endregion

    #region QCSampleDeliverQualityControl

    public QualityControlSample QCSampleDeliverQualityControl(
      int id,
      byte[] rowVersion)
    {

      var qualityControlSample = GetQualityControlSample(id: id);

      qualityControlSample = EditQualityControlSample(
            qualityControlSample: qualityControlSample,
                status: QualityControlSampleStatus.InQualityControl,
                rowVersion: rowVersion);
      return qualityControlSample;
    }
    #endregion

    #region QCSampleDeliverWarehouse

    public QualityControlSample QCSampleDeliverWarehouse(
      int id,
      int qualityControlItemId,
      int qualityControlId,
      TValue<double> testQty = null,
      TValue<double> consumeQty = null)
    {


      #region Check Permission
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
          throw new NotHavePermissionToQualityControlSampleStuffException(
                    stuffCode: stuff.Code,
                    validUserGroupId: stuffPurchaseCategory.QualityControlUserGroupId,
                    validUserGroupName: qcUserGroup.Name);
        }
      }

      #endregion
      var qualityControlSample = GetQualityControlSample(id: id);

      #region ReturnSample
      if (qualityControlSample.Status == QualityControlSampleStatus.InQualityControl)
      {
        var transactionTypeId = Models.StaticData.StaticTransactionTypes.ReturnSample.Id;

        #region Get Version and WarehouseId
        var qualityControlItemResult = App.Internals.QualityControl.GetQualityControlItem(
            id: qualityControlItemId);

        var version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
                    stuffSerialCode: qualityControlItemResult.StuffSerialCode,
                    stuffId: qualityControlItemResult.StuffId);

        var warehouseId = App.Internals.WarehouseManagement.GetStuffSerialInventories(
                  stuffId: qualityControlItemResult.StuffId,
                  stuffSerialCode: qualityControlItemResult.StuffSerialCode)

              .Select(m => m.WarehouseId).FirstOrDefault();
        #endregion

        #region AddTransactionBatch
        var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
        #endregion

        #region Add ReturnSample WarehouseTransaction
        var returnSampleTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
                warehouseId: warehouseId,
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: qualityControlItemResult.StuffId,
                billOfMaterialVersion: version,
                stuffSerialCode: qualityControlItemResult.StuffSerialCode,
                transactionTypeId: transactionTypeId,
                amount: qualityControlSample.Qty,
                unitId: qualityControlItemResult.UnitId,
                description: "برگرداندن نمونه به سطح کنترل کنترل کیفی(تحویل به انبار)",
                referenceTransaction: null);
        #endregion
      }
      #endregion

      qualityControlSample = EditQualityControlSample(
          qualityControlSample: qualityControlSample,
          testQty: testQty,
          consumeQty: consumeQty,
          status: QualityControlSampleStatus.Returned,
           rowVersion: qualityControlSample.RowVersion);
      return qualityControlSample;
    }
    #endregion

  }

}

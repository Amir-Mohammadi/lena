using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.ResponseStuffRequest;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add

    public ResponseStuffRequestItem AddResponseStuffRequestItem(
        ResponseStuffRequestItem responseStuffRequestItem,
        TransactionBatch transactionBatch,
        int stuffRequestItemId,
        int? stuffId,
        short? billOfMaterialVersion,
        double qty,
        StuffRequestItemStatusType status,
        string description)
    {

      responseStuffRequestItem = responseStuffRequestItem ?? repository.Create<ResponseStuffRequestItem>();
      responseStuffRequestItem.Qty = qty;
      responseStuffRequestItem.Status = status;
      responseStuffRequestItem.StuffRequestItemId = stuffRequestItemId;
      responseStuffRequestItem.StuffId = stuffId;
      responseStuffRequestItem.BillOfMaterialVersion = billOfMaterialVersion;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: responseStuffRequestItem,
                    transactionBatch: transactionBatch,
                    description: description);
      return responseStuffRequestItem;
    }

    #endregion

    #region AddProcess

    public ResponseStuffRequestItem AddResponseStuffRequestItemProcess(
        ResponseStuffRequestItem responseStuffRequestItem,
        TransactionBatch transactionBatch,
        int stuffRequestItemId,
        int? stuffId,
        short? billOfMaterialVersion,
        double qty,
        StuffRequestItemStatusType status,
        string description)
    {

      #region AddTransactionBatch
      transactionBatch = transactionBatch ?? AddTransactionBatch();
      #endregion
      #region AddResponseStuffRequestItem
      responseStuffRequestItem = AddResponseStuffRequestItem(
              responseStuffRequestItem: responseStuffRequestItem,
              transactionBatch: transactionBatch,
              stuffRequestItemId: stuffRequestItemId,
              stuffId: stuffId,
              billOfMaterialVersion: billOfMaterialVersion,
              qty: qty,
              status: status,
              description: description);
      #endregion
      #region Add WarehouseTransactions
      if (status == StuffRequestItemStatusType.Blocked && qty > 0)
      {
        if (responseStuffRequestItem.StuffId == null)
          throw new ResponseStuffRequestStuffIsNullException(stuffId: responseStuffRequestItem.StuffId);
        #region Add ExportAvailableTransaction

        var exportAvailableTransaction = AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                stuffId: responseStuffRequestItem.StuffId.Value,
                effectDateTime: transactionBatch.DateTime,
                billOfMaterialVersion: billOfMaterialVersion,
                stuffSerialCode: null,
                warehouseId: responseStuffRequestItem.StuffRequestItem.StuffRequest.FromWarehouseId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportAvailable.Id,
                amount: qty,
                unitId: responseStuffRequestItem.StuffRequestItem.UnitId,
                description: description,
                referenceTransaction: null);

        #endregion
        #region Add ImportBlockTransaction

        AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: responseStuffRequestItem.StuffId.Value,
                billOfMaterialVersion: billOfMaterialVersion,
                stuffSerialCode: null,
                warehouseId: responseStuffRequestItem.StuffRequestItem.StuffRequest.FromWarehouseId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportBlock.Id,
                amount: qty,
                unitId: responseStuffRequestItem.StuffRequestItem.UnitId,
                description: description,
                referenceTransaction: exportAvailableTransaction);

        #endregion
      }
      #endregion
      return responseStuffRequestItem;
    }

    #endregion

    #region Edit

    public ResponseStuffRequestItem EditResponseStuffRequestItem(
        int id,
        byte[] rowVersion,
        TValue<int> stuffRequestItemId = null,
        TValue<double> qty = null,
        TValue<StuffRequestItemStatusType> status = null,
        TValue<string> description = null)
    {

      var responseStuffRequestItem = GetResponseStuffRequestItem(id: id);
      return EditResponseStuffRequestItem(
                    responseStuffRequestItem: responseStuffRequestItem,
                    rowVersion: rowVersion,
                    stuffRequestItemId: stuffRequestItemId,
                    qty: qty,
                    status: status,
                    description: description);
    }

    public ResponseStuffRequestItem EditResponseStuffRequestItem(
        ResponseStuffRequestItem responseStuffRequestItem,
        byte[] rowVersion,
        TValue<int> stuffRequestItemId = null,
        TValue<double> qty = null,
        TValue<StuffRequestItemStatusType> status = null,
        TValue<string> description = null)
    {

      if (stuffRequestItemId != null)
        responseStuffRequestItem.StuffRequestItemId = stuffRequestItemId;
      if (qty != null)
        responseStuffRequestItem.Qty = qty;
      if (status != null)
        responseStuffRequestItem.Status = status;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: responseStuffRequestItem,
                    rowVersion: rowVersion,
                    description: description);
      return retValue as ResponseStuffRequestItem;
    }

    #endregion

    #region RemoveProcess

    public ResponseStuffRequestItem RemoveResponseStuffRequestItemProcess(
        int? transactionBatchId,
        int id,
        byte[] rowVersion)
    {

      var responseStuffRequestItem = GetResponseStuffRequestItem(id: id);
      var baseEntity = App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: transactionBatchId,
                    baseEntity: responseStuffRequestItem,
                    rowVersion: rowVersion);
      return baseEntity as ResponseStuffRequestItem;
    }

    #endregion

    #region Get

    public ResponseStuffRequestItem GetResponseStuffRequestItem(int id) => GetResponseStuffRequestItem(selector: e => e, id: id);

    public TResult GetResponseStuffRequestItem<TResult>(
        Expression<Func<ResponseStuffRequestItem, TResult>> selector,
        int id)
    {

      var result = GetResponseStuffRequestItems(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (result == null)
        throw new ResponseStuffRequestItemNotFoundException(id);
      return result;
    }

    public ResponseStuffRequestItem GetResponseStuffRequestItem(string code) => GetResponseStuffRequestItem(selector: e => e, code: code);

    public TResult GetResponseStuffRequestItem<TResult>(
        Expression<Func<ResponseStuffRequestItem, TResult>> selector,
        string code)
    {

      var result = GetResponseStuffRequestItems(
                    selector: selector,
                    code: code)


                .FirstOrDefault();
      if (result == null)
        throw new ResponseStuffRequestItemNotFoundException(code);
      return result;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetResponseStuffRequestItems<TResult>(
        Expression<Func<ResponseStuffRequestItem, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> stuffRequestItemId = null,
        TValue<double> qty = null,
        TValue<StuffRequestItemStatusType> status = null)
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
      var responseStuffRequestItem = baseQuery.OfType<ResponseStuffRequestItem>();
      if (stuffRequestItemId != null)
        responseStuffRequestItem = responseStuffRequestItem.Where(r => r.StuffRequestItemId == stuffRequestItemId);
      if (qty != null)
        responseStuffRequestItem = responseStuffRequestItem.Where(r => r.Qty == qty);
      if (status != null)
        responseStuffRequestItem = responseStuffRequestItem.Where(r => r.Status == status);
      return responseStuffRequestItem.Select(selector);
    }

    #endregion

    #region Search

    public IQueryable<ResponseStuffRequestItemResult> SearchResponseStuffRequestItemResult(
        IQueryable<ResponseStuffRequestItemResult> query,
        int[] ids,
        int? fromWarehouseId,
        int? toWarehouseId,
        int? stuffId,
        StuffRequestType? stuffRequestType,
        int? scrumEntityId,
        string scrumEntityCode,
        DateTime? fromDateTime,
        DateTime? toDateTime,
        int? stuffRequestId,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = query.Where(item =>
            item.Code.Contains(search) ||
            item.StuffCode.Contains(search) ||
            item.StuffName.Contains(search) ||
            item.StuffRequestCode.Contains(search) ||
            item.StuffRequestItemCode.Contains(search));

      if (ids != null)
        query = query.Where(i => ids.Contains(i.Id));
      if (fromWarehouseId != null)
        query = query.Where(i => i.FromWarehouseId == fromWarehouseId);
      if (toWarehouseId != null)
        query = query.Where(i => i.ToWarehouseId == toWarehouseId);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (stuffRequestType != null)
        query = query.Where(i => i.StuffRequestType == stuffRequestType);
      if (scrumEntityId != null)
        query = query.Where(i => i.ScrumEntityId == scrumEntityId);
      if (scrumEntityCode != null)
        query = query.Where(i => i.ScrumEntityCode == scrumEntityCode);
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);
      if (stuffRequestId != null)
        query = query.Where(i => i.StuffRequestId == stuffRequestId);
      return query;
    }

    #endregion

    #region Sort

    public IOrderedQueryable<ResponseStuffRequestItemResult> SortResponseStuffRequestItemResult(
        IQueryable<ResponseStuffRequestItemResult> query,
        SortInput<ResponseStuffRequestItemSortType> sort)
    {
      switch (sort.SortType)
      {
        case ResponseStuffRequestItemSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ResponseStuffRequestItemSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case ResponseStuffRequestItemSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case ResponseStuffRequestItemSortType.FromWarehouseName:
          return query.OrderBy(a => a.FromWarehouseName, sort.SortOrder);
        case ResponseStuffRequestItemSortType.ToWarehouseName:
          return query.OrderBy(a => a.ToWarehouseName, sort.SortOrder);
        case ResponseStuffRequestItemSortType.DepartmentName:
          return query.OrderBy(a => a.ToDepartmentName, sort.SortOrder);
        case ResponseStuffRequestItemSortType.ApplicantName:
          return query.OrderBy(a => a.ToEmployeeFullName, sort.SortOrder);
        case ResponseStuffRequestItemSortType.CreatorName:
          return query.OrderBy(a => a.RequestUserEmployeeName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    #endregion

    #region ToResponseStuffRequestItemResult

    public Expression<Func<ResponseStuffRequestItem, ResponseStuffRequestItemResult>>
        ToResponseStuffRequestItemResult =
            responseStuffRequestItem => new ResponseStuffRequestItemResult
            {
              Id = responseStuffRequestItem.Id,
              Code = responseStuffRequestItem.Code,
              RequestStuffCode = responseStuffRequestItem.StuffRequestItem.Stuff.Code,
              RequestStuffId = responseStuffRequestItem.StuffRequestItem.StuffId,
              RequestStuffName = responseStuffRequestItem.StuffRequestItem.Stuff.Name,
              RequestBillOfMaterialVersion = responseStuffRequestItem.StuffRequestItem.BillOfMaterialVersion,

              StuffCode = responseStuffRequestItem.Stuff.Code,
              StuffId = responseStuffRequestItem.StuffId,
              StuffName = responseStuffRequestItem.Stuff.Name,
              BillOfMaterialVersion = responseStuffRequestItem.BillOfMaterialVersion,
              RequestQty = responseStuffRequestItem.StuffRequestItem.Qty,
              UnitId = responseStuffRequestItem.StuffRequestItem.UnitId,
              UnitName = responseStuffRequestItem.StuffRequestItem.Unit.Name,
              UnitConversionRatio = responseStuffRequestItem.StuffRequestItem.Unit.ConversionRatio,
              RequestDescription = responseStuffRequestItem.StuffRequestItem.Description,
              Qty = responseStuffRequestItem.Qty,
              Status = responseStuffRequestItem.Status,
              RequestStatus = responseStuffRequestItem.StuffRequestItem.Status,
              Description = responseStuffRequestItem.Description,
              StuffRequestId = responseStuffRequestItem.StuffRequestItem.StuffRequestId,
              StuffRequestCode = responseStuffRequestItem.StuffRequestItem.StuffRequest.Code,
              StuffRequestItemId = responseStuffRequestItem.StuffRequestItemId,
              StuffRequestItemCode = responseStuffRequestItem.StuffRequestItem.Code,
              DateTime = responseStuffRequestItem.DateTime,
              StuffRequestType = responseStuffRequestItem.StuffRequestItem.StuffRequest.StuffRequestType,
              ScrumEntityId = responseStuffRequestItem.StuffRequestItem.StuffRequest.ScrumEntityId,
              ScrumEntityCode = responseStuffRequestItem.StuffRequestItem.StuffRequest.ScrumEntity.Code,
              ScrumEntityName = responseStuffRequestItem.StuffRequestItem.StuffRequest.ScrumEntity.Name,
              FromWarehouseId = responseStuffRequestItem.StuffRequestItem.StuffRequest.FromWarehouseId,
              FromWarehouseName = responseStuffRequestItem.StuffRequestItem.StuffRequest.FromWarehouse.Name,
              ToWarehouseId = responseStuffRequestItem.StuffRequestItem.StuffRequest.ToWarehouseId,
              ToWarehouseName = responseStuffRequestItem.StuffRequestItem.StuffRequest.ToWarehouse.Name,
              ToDepartmentId = responseStuffRequestItem.StuffRequestItem.StuffRequest.ToDepartmentId,
              ToDepartmentName = responseStuffRequestItem.StuffRequestItem.StuffRequest.ToDepartment.Name,
              ToEmployeeId = responseStuffRequestItem.StuffRequestItem.StuffRequest.ToEmployeeId,
              ToEmployeeFullName = responseStuffRequestItem.StuffRequestItem.StuffRequest.ToEmployee.FirstName + " " +
                responseStuffRequestItem.StuffRequestItem.StuffRequest.ToEmployee.LastName,
              RequestUserId = responseStuffRequestItem.StuffRequestItem.StuffRequest.UserId,
              RequestUserEmployeeName = responseStuffRequestItem.StuffRequestItem.StuffRequest.User.Employee.FirstName + " "
                    + responseStuffRequestItem.StuffRequestItem.StuffRequest.User.Employee.LastName,
              RowVersion = responseStuffRequestItem.RowVersion
            };

    #endregion

    #region ComplateResponseStuffRequestItemStatus
    public ResponseStuffRequestItem ComplateResponseStuffRequestItemStatus(
        ResponseStuffRequestItem responseStuffRequestItem,
        StuffRequestItemStatusType status)
    {

      if (status != StuffRequestItemStatusType.Issue &&
                status != StuffRequestItemStatusType.RejectedIssue &&
                status != StuffRequestItemStatusType.Complated)
        throw new ComplatedStuffRequestItemStatusTypeException();
      #region Set ComplateStatus
      responseStuffRequestItem = EditResponseStuffRequestItem(
              responseStuffRequestItem: responseStuffRequestItem,
              rowVersion: responseStuffRequestItem.RowVersion,
              status: status);
      #endregion
      #region ComplateStuffRequestItem
      ComplateStuffRequestItem(
              stuffRequestItem: responseStuffRequestItem.StuffRequestItem,
              status: status);
      #endregion
      return responseStuffRequestItem;
    }

    #endregion

  }
}
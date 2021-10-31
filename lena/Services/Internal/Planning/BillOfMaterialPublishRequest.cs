using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Services.Core;
using lena.Models.Planning.BillOfMaterialPublishRequest;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Production.ProductionOrder;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Add
    internal BillOfMaterialPublishRequest AddBillOfMaterialPublishRequestProcess(
        int billOfMaterialStuffId,
        short billOfMaterialVersion,
        string description)
    {

      #region PreviousRequest
      var previousRequests = this.GetBillOfMaterialPublishRequests(
              selector: e => e,
              billOfMaterialStuffId: billOfMaterialStuffId,
              status: BillOfMaterialPublishRequestStatus.NotAction);

      if (previousRequests.Any())
      {
        throw new BillOfMaterialPublishRequestExistsForMaterialException(
                  stuffId: billOfMaterialStuffId);
      }
      #endregion

      #region AddTransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion

      var bom = GetBillOfMaterial(stuffId: billOfMaterialStuffId, version: billOfMaterialVersion);

      if (bom.IsPublished)
        throw new PublishedBillOfMaterialException(billOfMaterialStuffId, billOfMaterialVersion);

      var bomVersions = GetBillOfMaterials(stuffId: billOfMaterialStuffId);

      var isFirstBomVersion = bomVersions.Count() == 1;


      var billOfMaterialPublishRequest = AddBillOfMaterialPublishRequest(
                    billOfMaterialPublishRequest: null,
                    transactionBatch: transactionBatch,
                    billOfMaterialStuffId: billOfMaterialStuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    type: BillOfMaterialPublishRequestType.Publish,
                    description: description,
                    status: isFirstBomVersion ? BillOfMaterialPublishRequestStatus.Accepted : BillOfMaterialPublishRequestStatus.NotAction);

      if (isFirstBomVersion)
        PublishBillOfMaterial(bom.RowVersion, billOfMaterialStuffId, billOfMaterialVersion);


      return billOfMaterialPublishRequest;
    }

    internal BillOfMaterialPublishRequest AddBillOfMaterialUnpublishRequestProcess(
        int billOfMaterialStuffId,
        short billOfMaterialVersion,
        string description)
    {

      #region PreviousRequest
      var previousRequests = this.GetBillOfMaterialPublishRequests(
              selector: e => e,
              billOfMaterialStuffId: billOfMaterialStuffId,
              status: BillOfMaterialPublishRequestStatus.NotAction);

      if (previousRequests.Any())
      {
        throw new BillOfMaterialPublishRequestExistsForMaterialException(
                  stuffId: billOfMaterialStuffId);
      }
      #endregion

      #region AddTransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion

      var billOfMaterialPublishRequest = AddBillOfMaterialPublishRequest(
              billOfMaterialPublishRequest: null,
              transactionBatch: transactionBatch,
              billOfMaterialStuffId: billOfMaterialStuffId,
              billOfMaterialVersion: billOfMaterialVersion,
              type: BillOfMaterialPublishRequestType.Unpublish,
              description: description);
      ;


      return billOfMaterialPublishRequest;
    }

    public BillOfMaterialPublishRequest AddBillOfMaterialPublishRequest(
        BillOfMaterialPublishRequest billOfMaterialPublishRequest,
        TransactionBatch transactionBatch,
        int billOfMaterialStuffId,
        short billOfMaterialVersion,
        BillOfMaterialPublishRequestType type,
        string description,
        BillOfMaterialPublishRequestStatus? status = null
        )
    {

      billOfMaterialPublishRequest = billOfMaterialPublishRequest ?? repository.Create<BillOfMaterialPublishRequest>();
      billOfMaterialPublishRequest.BillOfMaterialStuffId = billOfMaterialStuffId;
      billOfMaterialPublishRequest.BillOfMaterialVersion = billOfMaterialVersion;
      billOfMaterialPublishRequest.Status = status.HasValue ? status.Value : BillOfMaterialPublishRequestStatus.NotAction;
      billOfMaterialPublishRequest.Type = type;

      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: billOfMaterialPublishRequest,
                    transactionBatch: transactionBatch,
                    description: description);

      var billOfMaterial = App.Internals.Planning.GetBillOfMaterial(
                stuffId: billOfMaterialStuffId,
                version: billOfMaterialVersion);

      App.Internals.Planning.EditBillOfMaterial(
                rowVersion: billOfMaterial.RowVersion,
                stuffId: billOfMaterialStuffId,
                version: billOfMaterialVersion,
                latestBillOfMaterialPublishRequestId: billOfMaterialPublishRequest.Id);

      return billOfMaterialPublishRequest;
    }
    #endregion
    #region Get
    public lena.Domains.BillOfMaterialPublishRequest GetBillOfMaterialPublishRequest(int id) => GetBillOfMaterialPublishRequest(selector: e => e, id: id);
    public TResult GetBillOfMaterialPublishRequest<TResult>(
        Expression<Func<lena.Domains.BillOfMaterialPublishRequest, TResult>> selector,
        int id)
    {

      var billOfMaterialPublishRequest = GetBillOfMaterialPublishRequests(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (billOfMaterialPublishRequest == null)
        throw new BillOfMaterialPublishRequestNotFoundException(id);
      return billOfMaterialPublishRequest;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetBillOfMaterialPublishRequests<TResult>(
        Expression<Func<BillOfMaterialPublishRequest, TResult>> selector,
        TValue<int> id = null,
        TValue<int> userId = null,
        TValue<string> code = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<bool> isDelete = null,
        TValue<int> billOfMaterialVersion = null,
        TValue<string> description = null,
        TValue<int> transactionBatchId = null,
        TValue<BillOfMaterialPublishRequestStatus> status = null,
        TValue<BillOfMaterialPublishRequestStatus[]> statuses = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                     selector: e => e,
                     id: id,
                     code: code,
                     isDelete: isDelete,
                     userId: userId,
                     transactionBatchId: transactionBatchId,
                     description: description);
      var query = baseQuery.OfType<BillOfMaterialPublishRequest>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (code != null)
        query = query.Where(i => i.Code == code);
      if (billOfMaterialStuffId != null)
        query = query.Where(i => i.BillOfMaterialStuffId == billOfMaterialStuffId);
      if (isDelete != null)
        query = query.Where(i => i.IsDelete == isDelete);
      if (billOfMaterialVersion != null)
        query = query.Where(i => i.BillOfMaterialVersion == billOfMaterialVersion);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (statuses != null)
        query = query.Where(i => statuses.Value.Contains(i.Status));
      if (status != null)
        query = query.Where(i => i.Status == status);
      if (description != null)
        query = query.Where(i => i.Description == description);
      return query.Select(selector);
    }
    #endregion
    #region Delete
    public void RemoveBillOfMaterialPublishRequestProcess(
        int id,
        byte[] rowVersion)
    {

      var billOfMaterialPublishRequest = GetBillOfMaterialPublishRequest(id: id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                transactionBatchId: null,
                baseEntity: billOfMaterialPublishRequest,
                rowVersion: rowVersion);
    }
    #endregion
    #region Sort
    internal IOrderedQueryable<BillOfMaterialPublishRequestResult> SortBillOfMaterialPublishRequestResult(
        IQueryable<BillOfMaterialPublishRequestResult> query,
        SortInput<BillOfMaterialPublishRequestSortType> options)
    {
      switch (options.SortType)
      {
        case BillOfMaterialPublishRequestSortType.StuffName:
          return query.OrderBy(a => a.StuffName, options.SortOrder);
        case BillOfMaterialPublishRequestSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, options.SortOrder);
        case BillOfMaterialPublishRequestSortType.Status:
          return query.OrderBy(a => a.Status, options.SortOrder);
        case BillOfMaterialPublishRequestSortType.StuffId:
          return query.OrderBy(a => a.StuffId, options.SortOrder);
        case BillOfMaterialPublishRequestSortType.BillOfMaterialVersion:
          return query.OrderBy(a => a.BillOfMaterialVersion, options.SortOrder);
        case BillOfMaterialPublishRequestSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, options.SortOrder);
        case BillOfMaterialPublishRequestSortType.DateTime:
          return query.OrderBy(a => a.DateTime, options.SortOrder);
        case BillOfMaterialPublishRequestSortType.Type:
          return query.OrderBy(a => a.Type, options.SortOrder);
        case BillOfMaterialPublishRequestSortType.Description:
          return query.OrderBy(a => a.Description, options.SortOrder);
        case BillOfMaterialPublishRequestSortType.Code:
          return query.OrderBy(a => a.Code, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    internal Expression<Func<BillOfMaterialPublishRequest, BillOfMaterialPublishRequestResult>> ToBillOfMaterialPublishRequestResult =
        billOfMaterialPublishRequest => new BillOfMaterialPublishRequestResult()
        {
          BillOfMaterialTitle = billOfMaterialPublishRequest.BillOfMaterial.Title,
          BillOfMaterialVersion = billOfMaterialPublishRequest.BillOfMaterialVersion,
          BillOfMaterialVersionType = billOfMaterialPublishRequest.BillOfMaterial.BillOfMaterialVersionType,
          EmployeeFullName = billOfMaterialPublishRequest.User.Employee.FirstName + " " + billOfMaterialPublishRequest.User.Employee.LastName,
          Id = billOfMaterialPublishRequest.Id,
          DateTime = billOfMaterialPublishRequest.DateTime,
          RowVersion = billOfMaterialPublishRequest.RowVersion,
          Status = billOfMaterialPublishRequest.Status,
          StuffCode = billOfMaterialPublishRequest.BillOfMaterial.Stuff.Code,
          StuffId = billOfMaterialPublishRequest.BillOfMaterialStuffId,
          StuffName = billOfMaterialPublishRequest.BillOfMaterial.Stuff.Name,
          StuffType = billOfMaterialPublishRequest.BillOfMaterial.Stuff.StuffType,
          Type = billOfMaterialPublishRequest.Type,
          Description = billOfMaterialPublishRequest.Description,
          Code = billOfMaterialPublishRequest.Code
        };
    #endregion
    #region Search
    internal IQueryable<BillOfMaterialPublishRequestResult> SearchBillOfMaterialPublishRequestResultQuery(
        IQueryable<BillOfMaterialPublishRequestResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        int? stuffId = null,
        string stuffCode = null)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where
                    item.StuffCode.Contains(searchText) ||
                    item.StuffName.Contains(searchText) ||
                    item.BillOfMaterialTitle.Contains(searchText) ||
                    item.EmployeeFullName.Contains(searchText)
                select item;
      var isStuffIdNull = stuffId == null;
      var isStuffCodeNull = stuffCode == null;
      query = from item in query
              where (isStuffIdNull || item.StuffId == stuffId)
              where (isStuffCodeNull || item.StuffCode == stuffCode)
              select item;

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region AcceptBillOfMaterialPublishRequest
    public BillOfMaterialPublishRequest AcceptBillOfMaterialPublishRequestProcess(
        int id,
        EditProductionOrderWorkPlanStepInput[] modifiedProductionOrders,
        byte[] rowVersion)
    {

      var billOfMaterialPublishRequest = GetBillOfMaterialPublishRequest(id: id);
      return AcceptBillOfMaterialPublishRequestProcess(
                    billOfMaterialPublishRequest: billOfMaterialPublishRequest,
                    modifiedProductionOrders: modifiedProductionOrders,
                    rowVersion: rowVersion);
    }
    public BillOfMaterialPublishRequest AcceptBillOfMaterialPublishRequestProcess(
        BillOfMaterialPublishRequest billOfMaterialPublishRequest,
        EditProductionOrderWorkPlanStepInput[] modifiedProductionOrders,
        byte[] rowVersion)
    {

      if (billOfMaterialPublishRequest.Type != BillOfMaterialPublishRequestType.Publish)
      {
        throw new BillOfMaterialPublishRequestNotFoundException(id: billOfMaterialPublishRequest.Id);
      }

      #region Unpublish Previous BillOfMaterials

      var toUnpublishBillOfMaterials = App.Internals.Planning.GetBillOfMaterials(
              stuffId: billOfMaterialPublishRequest.BillOfMaterialStuffId,
              isPublished: true);

      foreach (var toPublishBillOfMaterial in toUnpublishBillOfMaterials)
      {
        App.Internals.Planning.UnPublishBillOfMaterial(
                 stuffId: toPublishBillOfMaterial.StuffId,
                 version: toPublishBillOfMaterial.Version,
                 rowVersion: toPublishBillOfMaterial.RowVersion);
      }

      #endregion

      #region Publish Accepted BillOfMaterials

      var billOfMaterial = App.Internals.Planning.GetBillOfMaterial(
              stuffId: billOfMaterialPublishRequest.BillOfMaterialStuffId,
              version: billOfMaterialPublishRequest.BillOfMaterialVersion);

      App.Internals.Planning.PublishBillOfMaterial(
                    stuffId: billOfMaterial.StuffId,
                    version: billOfMaterial.Version,
                    rowVersion: billOfMaterial.RowVersion
                );

      #endregion

      #region Update ProductionOrders

      foreach (var productionOrder in modifiedProductionOrders)
      {
        App.Internals.Production.EditProductionOrder(
                  id: productionOrder.Id,
                  rowVersion: productionOrder.RowVersion,
                  workPlanStepId: productionOrder.WorkPlanStepId,
                  billOfMaterialVersion: billOfMaterialPublishRequest.BillOfMaterialVersion
              );
      }
      #endregion

      #region Update BillOfMaterialPublishRequest Status
      billOfMaterialPublishRequest = EditBillOfMaterialPublishRequest(
              billOfMaterialPublishRequest: billOfMaterialPublishRequest,
              rowVersion: rowVersion,
              status: BillOfMaterialPublishRequestStatus.Accepted);
      return billOfMaterialPublishRequest;
      #endregion
    }

    public BillOfMaterialPublishRequest AcceptBillOfMaterialUnpublishRequestProcess(
        int id,
        byte[] rowVersion)
    {

      var billOfMaterialPublishRequest = GetBillOfMaterialPublishRequest(id: id);
      return AcceptBillOfMaterialUnpublishRequestProcess(
                    billOfMaterialPublishRequest: billOfMaterialPublishRequest,
                    rowVersion: rowVersion);
    }
    public BillOfMaterialPublishRequest AcceptBillOfMaterialUnpublishRequestProcess(
        BillOfMaterialPublishRequest billOfMaterialPublishRequest,
        byte[] rowVersion)
    {

      if (billOfMaterialPublishRequest.Type != BillOfMaterialPublishRequestType.Unpublish)
      {
        throw new BillOfMaterialPublishRequestNotFoundException(id: billOfMaterialPublishRequest.Id);
      }

      #region Unpublish BillOfMaterial

      var billOfMaterial = App.Internals.Planning.GetBillOfMaterial(
              stuffId: billOfMaterialPublishRequest.BillOfMaterialStuffId,
              version: billOfMaterialPublishRequest.BillOfMaterialVersion);

      App.Internals.Planning.UnPublishBillOfMaterial(
                    stuffId: billOfMaterial.StuffId,
                    version: billOfMaterial.Version,
                    rowVersion: billOfMaterial.RowVersion);
      #endregion

      #region Update BillOfMaterialPublishRequest Status
      billOfMaterialPublishRequest = EditBillOfMaterialPublishRequest(
              billOfMaterialPublishRequest: billOfMaterialPublishRequest,
              rowVersion: rowVersion,
              status: BillOfMaterialPublishRequestStatus.Accepted);
      return billOfMaterialPublishRequest;
      #endregion
    }
    #endregion
    #region RejectBillOfMaterialPublishRequest
    public BillOfMaterialPublishRequest RejectBillOfMaterialPublishRequestProcess(
        int id,
        byte[] rowVersion)
    {

      var billOfMaterialPublishRequest = GetBillOfMaterialPublishRequest(id: id);

      return RejectBillOfMaterialPublishRequestProcess(
                    billOfMaterialPublishRequest: billOfMaterialPublishRequest,
                    rowVersion: rowVersion);
    }
    public BillOfMaterialPublishRequest RejectBillOfMaterialPublishRequestProcess(
        BillOfMaterialPublishRequest billOfMaterialPublishRequest,
        byte[] rowVersion)
    {

      if (billOfMaterialPublishRequest.Type != BillOfMaterialPublishRequestType.Publish)
      {
        throw new BillOfMaterialPublishRequestNotFoundException(id: billOfMaterialPublishRequest.Id);
      }

      billOfMaterialPublishRequest = EditBillOfMaterialPublishRequest(
                    billOfMaterialPublishRequest: billOfMaterialPublishRequest,
                    rowVersion: rowVersion,
                    status: BillOfMaterialPublishRequestStatus.Rejected);

      return billOfMaterialPublishRequest;
    }


    public BillOfMaterialPublishRequest RejectBillOfMaterialUnpublishRequestProcess(
        int id,
        byte[] rowVersion)
    {

      var billOfMaterialPublishRequest = GetBillOfMaterialPublishRequest(id: id);



      return RejectBillOfMaterialUnpublishRequestProcess(
                    billOfMaterialPublishRequest: billOfMaterialPublishRequest,
                    rowVersion: rowVersion);
    }
    public BillOfMaterialPublishRequest RejectBillOfMaterialUnpublishRequestProcess(
        BillOfMaterialPublishRequest billOfMaterialPublishRequest,
        byte[] rowVersion)
    {

      if (billOfMaterialPublishRequest.Type != BillOfMaterialPublishRequestType.Unpublish)
      {
        throw new BillOfMaterialPublishRequestNotFoundException(id: billOfMaterialPublishRequest.Id);
      }

      billOfMaterialPublishRequest = EditBillOfMaterialPublishRequest(
                    billOfMaterialPublishRequest: billOfMaterialPublishRequest,
                    rowVersion: rowVersion,
                    status: BillOfMaterialPublishRequestStatus.Rejected);
      return billOfMaterialPublishRequest;
    }
    #endregion

    #region EditBillOfMaterialPublishRequest
    public BillOfMaterialPublishRequest EditBillOfMaterialPublishRequest(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<bool> isDelete = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<short> billOfMaterialVersion = null,
        TValue<BillOfMaterialPublishRequestStatus> status = null)
    {

      var billOfMaterialPublishRequest = GetBillOfMaterialPublishRequest(id: id);
      return EditBillOfMaterialPublishRequest(
                    billOfMaterialPublishRequest: billOfMaterialPublishRequest,
                    rowVersion: rowVersion,
                    description: description,
                    isDelete: isDelete,
                    billOfMaterialStuffId: billOfMaterialStuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    status: status);
    }
    public BillOfMaterialPublishRequest EditBillOfMaterialPublishRequest(
        BillOfMaterialPublishRequest billOfMaterialPublishRequest,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<bool> isDelete = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<short> billOfMaterialVersion = null,
        TValue<BillOfMaterialPublishRequestStatus> status = null,
        TValue<BillOfMaterialPublishRequestType> type = null)
    {

      if (billOfMaterialStuffId != null)
        billOfMaterialPublishRequest.BillOfMaterialStuffId = billOfMaterialStuffId;
      if (billOfMaterialVersion != null)
        billOfMaterialPublishRequest.BillOfMaterialVersion = billOfMaterialVersion;
      if (status != null)
        billOfMaterialPublishRequest.Status = status;
      if (type != null)
        billOfMaterialPublishRequest.Type = type;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: billOfMaterialPublishRequest,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description);

      var billOfMaterial = App.Internals.Planning.GetBillOfMaterial(
            stuffId: billOfMaterialPublishRequest.BillOfMaterialStuffId,
            version: billOfMaterialPublishRequest.BillOfMaterialVersion);

      App.Internals.Planning.EditBillOfMaterial(
                rowVersion: billOfMaterial.RowVersion,
                stuffId: billOfMaterialPublishRequest.BillOfMaterialStuffId,
                version: billOfMaterialPublishRequest.BillOfMaterialVersion,
                latestBillOfMaterialPublishRequestId: billOfMaterialPublishRequest.Id);

      return retValue as BillOfMaterialPublishRequest;
    }
    #endregion

  }
}

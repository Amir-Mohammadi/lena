using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.StuffRequest;
using lena.Models.WarehouseManagement.StuffRequestItem;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Get
    public ProductionMaterialRequest GetProductionMaterialRequest(int id) => GetProductionMaterialRequest(selector: e => e, id: id);
    public TResult GetProductionMaterialRequest<TResult>(
        Expression<Func<ProductionMaterialRequest, TResult>> selector,
        int id)
    {

      var result = GetProductionMaterialRequests(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new ProductionMaterialRequestNotFoundException(id);
      return result;
    }
    public ProductionMaterialRequest GetProductionMaterialRequest(string code) => GetProductionMaterialRequest(selector: e => e, code: code);
    public TResult GetProductionMaterialRequest<TResult>(
        Expression<Func<ProductionMaterialRequest, TResult>> selector,
        string code)
    {

      var result = GetProductionMaterialRequests(
                selector: selector,
                code: code).FirstOrDefault();
      if (result == null)
        throw new ProductionMaterialRequestNotFoundException(code);
      return result;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionMaterialRequests<TResult>(
        Expression<Func<ProductionMaterialRequest, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> productionOrderId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var productionMaterialRequest = baseQuery.OfType<ProductionMaterialRequest>();
      if (productionOrderId != null)
        productionMaterialRequest = productionMaterialRequest.Where(r => r.ProductionOrderId == productionOrderId);
      return productionMaterialRequest.Select(selector);


    }
    #endregion
    #region Add
    public ProductionMaterialRequest AddProductionMaterialRequest(
        ProductionMaterialRequest productionMaterialRequest,
        TransactionBatch transactionBatch,
        string description)
    {

      productionMaterialRequest = productionMaterialRequest ?? repository.Create<ProductionMaterialRequest>();
      //productionMaterialRequest.ProductionOrderId = productionOrderId;
      App.Internals.ApplicationBase.AddBaseEntity(
              baseEntity: productionMaterialRequest,
              transactionBatch: transactionBatch,
              description: description);
      return productionMaterialRequest;
    }
    #endregion
    #region Edit
    public ProductionMaterialRequest EditProductionMaterialRequest(
        int id,
        byte[] rowVersion,
        TValue<int> productionOrderId = null,
        TValue<string> description = null)
    {

      var productionMaterialRequest = GetProductionMaterialRequest(id: id);
      return EditProductionMaterialRequest(
                    productionMaterialRequest: productionMaterialRequest,
                    rowVersion: rowVersion,
                    productionOrderId: productionOrderId,
                    description: description);
    }
    public ProductionMaterialRequest EditProductionMaterialRequest(
        ProductionMaterialRequest productionMaterialRequest,
        byte[] rowVersion,
        TValue<int> productionOrderId = null,
        TValue<string> description = null)
    {

      if (productionOrderId != null)
        productionMaterialRequest.ProductionOrderId = productionOrderId;
      if (description != null)
        productionMaterialRequest.Description = description;
      App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: productionMaterialRequest,
                    description: description,
                    rowVersion: rowVersion);
      return productionMaterialRequest;
    }
    #endregion
    #region AddProcess
    public ProductionMaterialRequest AddProductionMaterialRequestProcess(
        ProductionMaterialRequest productionMaterialRequest,
        TransactionBatch transactionBatch,
        int[] productionOrderIds,
        string description,
        AddStuffRequestInput[] addStuffRequests)
    {

      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      transactionBatch = transactionBatch ?? warehouseManagement.AddTransactionBatch();
      #endregion
      #region AddProductionMaterialRequest
      productionMaterialRequest = AddProductionMaterialRequest(
              transactionBatch: transactionBatch,
              productionMaterialRequest: productionMaterialRequest,
              description: description);
      #endregion
      foreach (var productionOrderId in productionOrderIds)
        AddProductionMaterialRequestDetail(
                  detail: null,
                  productionOrderId: productionOrderId,
                  productionMaterialRequestId: productionMaterialRequest.Id);

      #region AddStuffRequests
      var productionMaterialRequestDetail = productionMaterialRequest.ProductionMaterialRequestDetails.FirstOrDefault();
      if (productionMaterialRequestDetail == null)
        throw new NotFoundAnyDetailsForProductionMaterialRequestException(productionMaterialRequest.Id);

      var toWarehouseId = productionMaterialRequestDetail.ProductionOrder.WorkPlanStep.ProductionLine.ConsumeWarehouseId;

      foreach (var addStuffRequest in addStuffRequests)
      {
        var items = addStuffRequest.StuffRequestItems.Where(r => r.Qty > 0).ToArray();
        if (items.Length > 0)
        {
          AddStuffRequestProcess(
                        transactionBatch: transactionBatch,
                        stuffRequest: null,
                        fromWarehouseId: addStuffRequest.FromWarehouseId,
                        toWarehouseId: toWarehouseId,
                        employeeId: null,
                        departmentId: null,
                        scrumEntityId: null,
                        productionMaterialRequestId: productionMaterialRequest.Id,
                        stuffRequestType: StuffRequestType.Production,
                        stuffRequestItems: items,
                        description: addStuffRequest.Description);
        }
      }
      #endregion
      #region ResetProductionOrderStatus
      foreach (var productionOrderId in productionOrderIds)
      {

        App.Internals.Production.ResetProductionOrderStatus(id: productionOrderId);
        #endregion

        #region GetBaseEntityScrumTask

        var scrumTask = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
          baseEntityId: productionOrderId,
          scrumTaskType: ScrumTaskTypes.ProductionMaterialRequest);
        #endregion
        #region DoneScrumTask
        if (scrumTask != null)
          App.Internals.ScrumManagement.DoneScrumTask(
                        scrumTask: scrumTask,
                        rowVersion: scrumTask.RowVersion);
        #endregion
      }
      return productionMaterialRequest;
    }
    #endregion

    #region EditProcess
    //public ProductionMaterialRequest EditProductionMaterialRequestProcess(
    //    int id,
    //    byte[] rowVersion,
    //    TValue<int> fromWarehouseId,
    //    TValue<int?> toWarehouseId,
    //    TValue<int?> scrumEntityId,
    //    TValue<StuffRequestType> stuffRequestType,
    //    TValue<AddStuffRequestItemInput[]> addStuffRequestItems,
    //    TValue<EditStuffRequestItemInput[]> editStuffRequestItems,
    //    TValue<DeleteStuffRequestItemInput[]> deleteStuffRequestItems,
    //    TValue<int> productionOrderId,
    //    TValue<string> description)
    //{
    //    
    //        var productionMaterialRequest = GetProductionMaterialRequest(id: id)
    //            
    //;
    //        EditProductionMaterialRequestProcess(
    //                productionMaterialRequest: productionMaterialRequest,
    //                rowVersion: rowVersion,
    //                fromWarehouseId: fromWarehouseId,
    //                toWarehouseId: toWarehouseId,
    //                scrumEntityId: scrumEntityId,
    //                stuffRequestType: stuffRequestType,
    //                addStuffRequestItems: addStuffRequestItems,
    //                editStuffRequestItems: editStuffRequestItems,
    //                deleteStuffRequestItems: deleteStuffRequestItems,
    //                productionOrderId: productionOrderId,
    //                description: description
    //            )
    //;
    //        return productionMaterialRequest;
    //    });
    //}
    //public ProductionMaterialRequest EditProductionMaterialRequestProcess(
    //    ProductionMaterialRequest productionMaterialRequest,
    //    byte[] rowVersion,
    //    TValue<int> fromWarehouseId,
    //    TValue<int?> toWarehouseId,
    //    TValue<int?> scrumEntityId,
    //    TValue<StuffRequestType> stuffRequestType,
    //    TValue<AddStuffRequestItemInput[]> addStuffRequestItems,
    //    TValue<EditStuffRequestItemInput[]> editStuffRequestItems,
    //    TValue<DeleteStuffRequestItemInput[]> deleteStuffRequestItems,
    //    TValue<int> productionOrderId,
    //    TValue<string> description)
    //{
    //    
    //        if (productionOrderId != null)
    //            productionMaterialRequest.ProductionOrderId = productionOrderId;
    //        EditStuffRequestProcess(
    //                stuffRequest: null,
    //                rowVersion: rowVersion,
    //                fromWarehouseId: fromWarehouseId,
    //                toWarehouseId: toWarehouseId,
    //                scrumEntityId: scrumEntityId,
    //                stuffRequestType: stuffRequestType,
    //                addStuffRequestItems: addStuffRequestItems,
    //                editStuffRequestItems: editStuffRequestItems,
    //                deleteStuffRequestItems: deleteStuffRequestItems,
    //                description: description)
    //            
    //;
    //        return productionMaterialRequest;
    //    });
    //}
    #endregion
    //#region AddProcess
    //public ProductionMaterialRequest AddProductionMaterialRequestProcess(
    //    int fromWarehouseId,
    //    int? toWarehouseId,
    //    int? scrumEntityId,
    //    StuffRequestType stuffRequestType,
    //    AddStuffRequestItemInput[] stuffRequestItems,
    //    int productionOrderId,
    //    string description)
    //{
    //    
    //        var productionMaterialRequest = repository.Create<ProductionMaterialRequest>();
    //        productionMaterialRequest.ProductionOrderId = productionOrderId;
    //        AddStuffRequestProcess(
    //            stuffRequest: productionMaterialRequest,
    //            fromWarehouseId: fromWarehouseId,
    //            toWarehouseId: toWarehouseId,
    //            scrumEntityId: scrumEntityId,
    //            stuffRequestType: stuffRequestType,
    //            stuffRequestItems: stuffRequestItems,
    //            description: description)
    //            
    //;
    //        return productionMaterialRequest;
    //    });
    //}
    //#endregion
    //#region EditProcess
    //public ProductionMaterialRequest EditProductionMaterialRequestProcess(
    //    int id,
    //    byte[] rowVersion,
    //    TValue<int> fromWarehouseId,
    //    TValue<int?> toWarehouseId,
    //    TValue<int?> scrumEntityId,
    //    TValue<StuffRequestType> stuffRequestType,
    //    TValue<AddStuffRequestItemInput[]> addStuffRequestItems,
    //    TValue<EditStuffRequestItemInput[]> editStuffRequestItems,
    //    TValue<DeleteStuffRequestItemInput[]> deleteStuffRequestItems,
    //    TValue<int> productionOrderId,
    //    TValue<string> description)
    //{
    //    
    //        var productionMaterialRequest = GetProductionMaterialRequest(id: id)
    //            
    //;
    //        EditProductionMaterialRequestProcess(
    //                productionMaterialRequest: productionMaterialRequest,
    //                rowVersion: rowVersion,
    //                fromWarehouseId: fromWarehouseId,
    //                toWarehouseId: toWarehouseId,
    //                scrumEntityId: scrumEntityId,
    //                stuffRequestType: stuffRequestType,
    //                addStuffRequestItems: addStuffRequestItems,
    //                editStuffRequestItems: editStuffRequestItems,
    //                deleteStuffRequestItems: deleteStuffRequestItems,
    //                productionOrderId: productionOrderId,
    //                description: description
    //            )
    //;
    //        return productionMaterialRequest;
    //    });
    //}
    //public ProductionMaterialRequest EditProductionMaterialRequestProcess(
    //    ProductionMaterialRequest productionMaterialRequest,
    //    byte[] rowVersion,
    //    TValue<int> fromWarehouseId,
    //    TValue<int?> toWarehouseId,
    //    TValue<int?> scrumEntityId,
    //    TValue<StuffRequestType> stuffRequestType,
    //    TValue<AddStuffRequestItemInput[]> addStuffRequestItems,
    //    TValue<EditStuffRequestItemInput[]> editStuffRequestItems,
    //    TValue<DeleteStuffRequestItemInput[]> deleteStuffRequestItems,
    //    TValue<int> productionOrderId,
    //    TValue<string> description)
    //{
    //    
    //        if (productionOrderId != null)
    //            productionMaterialRequest.ProductionOrderId = productionOrderId;
    //        EditStuffRequestProcess(
    //                stuffRequest: productionMaterialRequest,
    //                rowVersion: rowVersion,
    //                fromWarehouseId: fromWarehouseId,
    //                toWarehouseId: toWarehouseId,
    //                scrumEntityId: scrumEntityId,
    //                stuffRequestType: stuffRequestType,
    //                addStuffRequestItems: addStuffRequestItems,
    //                editStuffRequestItems: editStuffRequestItems,
    //                deleteStuffRequestItems: deleteStuffRequestItems,
    //                description: description)
    //            
    //;
    //        return productionMaterialRequest;
    //    });
    //}
    //#endregion
    //#region Search
    //public IQueryable<ProductionMaterialRequestFullResult> SearchProductionMaterialRequestResult(IQueryable<ProductionMaterialRequestFullResult> query,
    //    string search,
    //    int? fromWarehouseId,
    //    int? toWarehouseId,
    //    int? stuffRequestTypeId,
    //    int? productionMaterialRequestStatusTypeId,
    //    string scrumEntityCode,
    //    DateTime? fromDateTime,
    //    DateTime? toDateTime,
    //    int? stuffId
    //    )
    //{
    //    if (!string.IsNullOrEmpty(search))
    //        query = query.Where(item =>
    //            item.StuffCode.Contains(search) ||
    //            item.StuffName.Contains(search));
    //    if (fromWarehouseId != null)
    //        query = query.Where(i => i.FromWarehouseId == fromWarehouseId);
    //    if (toWarehouseId != null)
    //        query = query.Where(i => i.ToWarehouseId == toWarehouseId);
    //    if (stuffRequestTypeId != null)
    //        query = query.Where(i => (int)i.StuffRequestType == stuffRequestTypeId);
    //    if (productionMaterialRequestStatusTypeId != null)
    //        query = query.Where(i => i.StuffRequestItems.Any(s => (int)s.Status == productionMaterialRequestStatusTypeId));
    //    if (scrumEntityCode != null)
    //        query = query.Where(i => i.ScrumEntityCode == scrumEntityCode);
    //    if (fromDateTime != null && toDateTime != null)
    //        query = query.Where(i => fromDateTime <= i.DateTime && i.DateTime <= toDateTime);
    //    if (stuffId != null)
    //        query = query.Where(i => i.StuffId == stuffId);
    //    return query;
    //}
    //#endregion
    //#region Sort
    //public IOrderedQueryable<ProductionMaterialRequestFullResult> SortProductionMaterialRequestFullResult(IQueryable<ProductionMaterialRequestFullResult> query,
    //    SortInput<ProductionMaterialRequestSortType> sort)
    //{
    //    switch (sort.SortType)
    //    {
    //        case ProductionMaterialRequestSortType.Code:
    //            return query.OrderBy(a => a.Code, sort.SortOrder);
    //        case ProductionMaterialRequestSortType.DateTime:
    //            return query.OrderBy(a => a.DateTime, sort.SortOrder);
    //        case ProductionMaterialRequestSortType.FromWarehouse:
    //            return query.OrderBy(a => a.FromWarehouseName, sort.SortOrder);
    //        case ProductionMaterialRequestSortType.ProjectCode:
    //            return query.OrderBy(a => a.ScrumEntityCode, sort.SortOrder);
    //        case ProductionMaterialRequestSortType.ProjectName:
    //            return query.OrderBy(a => a.ScrumEntityName, sort.SortOrder);
    //        case ProductionMaterialRequestSortType.StuffRequestType:
    //            return query.OrderBy(a => a.StuffRequestType, sort.SortOrder);
    //        case ProductionMaterialRequestSortType.ToWarehouse:
    //            return query.OrderBy(a => a.ToWarehouseName, sort.SortOrder);
    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }
    //}
    //#endregion
    //#region ToProductionMaterialRequestResult
    //public Expression<Func<ProductionMaterialRequest, ProductionMaterialRequestResult>> ToProductionMaterialRequestResult =
    //    productionMaterialRequest => new ProductionMaterialRequestResult
    //    {
    //        Id = productionMaterialRequest.Id,
    //        Code = productionMaterialRequest.Code,
    //        //SendPermissionId = productionMaterialRequest.SendPermissionId,
    //        //SendPermissionCode = productionMaterialRequest.SendPermission.Code,
    //        //CustomerId = productionMaterialRequest.SendPermission.OrderItemBlock.OrderItem.Order.CustomerId,
    //        //OrderItemId = productionMaterialRequest.SendPermission.OrderItemBlock.OrderItemId,
    //        //OrderItemCode = productionMaterialRequest.SendPermission.OrderItemBlock.OrderItem.Code,
    //        //OrderItemUnitId = productionMaterialRequest.SendPermission.OrderItemBlock.OrderItem.UnitId,
    //        //OrderItemUnitName = productionMaterialRequest.SendPermission.OrderItemBlock.OrderItem.Unit.Name,
    //        //OrderItemQty = productionMaterialRequest.SendPermission.OrderItemBlock.Qty,
    //        //OrderItemBlockId = productionMaterialRequest.SendPermission.OrderItemBlockId,
    //        //OrderItemBlockQty = productionMaterialRequest.SendPermission.OrderItemBlock.Qty,
    //        //OrderItemBlockUnitId = productionMaterialRequest.SendPermission.OrderItemBlock.UnitId,
    //        //OrderItemBlockUnitName = productionMaterialRequest.SendPermission.OrderItemBlock.Unit.Name,
    //        //StuffId = productionMaterialRequest.SendPermission.OrderItemBlock.OrderItem.StuffId,
    //        //StuffCode = productionMaterialRequest.SendPermission.OrderItemBlock.OrderItem.Stuff.Code,
    //        //StuffName = productionMaterialRequest.SendPermission.OrderItemBlock.OrderItem.Stuff.Name,
    //        //Qty = productionMaterialRequest.SendPermission.Qty,
    //        //UnitId = productionMaterialRequest.SendPermission.UnitId,
    //        //UnitName = productionMaterialRequest.SendPermission.Unit.Name,
    //        //DateTime = productionMaterialRequest.DateTime,
    //        //SendProductId = productionMaterialRequest.SendProduct.Id,
    //        //ExitReceiptId = productionMaterialRequest.SendProduct.ExitReceiptId,
    //        //ExitReceiptDateTime = productionMaterialRequest.SendProduct.ExitReceipt.DateTime,
    //        //ExitReceiptConfirm = productionMaterialRequest.SendProduct.ExitReceipt.Confirmed,
    //        RowVersion = productionMaterialRequest.RowVersion
    //    };
    //#endregion
    //#region ToProductionMaterialRequestFullResult
    //public Expression<Func<ProductionMaterialRequest, ProductionMaterialRequestFullResult>> ToProductionMaterialRequestFullResult =
    //    productionMaterialRequest => new ProductionMaterialRequestFullResult
    //    {
    //        Id = productionMaterialRequest.Id,
    //        Code = productionMaterialRequest.Code,
    //        FromWarehouseId = productionMaterialRequest.FromWarehouseId,
    //        FromWarehouseName = productionMaterialRequest.FromWarehouse.Name,
    //        ToWarehouseId = productionMaterialRequest.ToWarehouseId,
    //        ToWarehouseName = productionMaterialRequest.ToWarehouse.Name,
    //        StuffRequestType = productionMaterialRequest.StuffRequestType,
    //        DateTime = productionMaterialRequest.DateTime,
    //        IsDelete = productionMaterialRequest.IsDelete,
    //        Description = productionMaterialRequest.Description,
    //        UserId = productionMaterialRequest.UserId,
    //        EmployeeFullName = productionMaterialRequest.User.Employee.FirstName + " " + productionMaterialRequest.User.Employee.LastName,
    //        ScrumEntityId = productionMaterialRequest.ScrumEntityId,
    //        ScrumEntityCode = productionMaterialRequest.ScrumEntity.Code,
    //        ScrumEntityName = productionMaterialRequest.ScrumEntity.Name,
    //        StuffRequestItems = productionMaterialRequest.StuffRequestItems.AsQueryable()
    //            .Select(App.Internals.WarehouseManagement.ToStuffRequestItemResult),
    //        RowVersion = productionMaterialRequest.RowVersion
    //    };
    //#endregion
  }
}

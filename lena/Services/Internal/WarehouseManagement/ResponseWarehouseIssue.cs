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
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public ResponseWarehouseIssue AddResponseWarehouseIssue(
        ResponseWarehouseIssue responseWarehouseIssue,
        TransactionBatch transactionBatch,
        int warehouseIssueId,
        string description)
    {

      responseWarehouseIssue = responseWarehouseIssue ?? repository.Create<ResponseWarehouseIssue>();
      responseWarehouseIssue.WarehouseIssue = GetWarehouseIssue(id: warehouseIssueId);
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: responseWarehouseIssue,
                    transactionBatch: transactionBatch,
                    description: description);
      return responseWarehouseIssue;
    }
    #endregion
    #region Edit
    public ResponseWarehouseIssue EditResponseWarehouseIssue(
        int id,
        byte[] rowVersion,
        TValue<int> warehouseIssueId = null,
        TValue<string> description = null)
    {

      var responseWarehouseIssue = GetResponseWarehouseIssue(id: id);

      return EditResponseWarehouseIssue(
                    responseWarehouseIssue: responseWarehouseIssue,
                    rowVersion: rowVersion,
                    warehouseIssueId: warehouseIssueId,
                    description: description);
    }
    public ResponseWarehouseIssue EditResponseWarehouseIssue(
        ResponseWarehouseIssue responseWarehouseIssue,
        byte[] rowVersion,
        TValue<int> warehouseIssueId = null,
        TValue<string> description = null)
    {

      if (warehouseIssueId != null)
        responseWarehouseIssue.WarehouseIssue = GetWarehouseIssue(id: warehouseIssueId);
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: responseWarehouseIssue,
                    rowVersion: rowVersion,
                    description: description);
      return retValue as ResponseWarehouseIssue;
    }
    #endregion
    #region Get
    public ResponseWarehouseIssue GetResponseWarehouseIssue(int id) => GetResponseWarehouseIssue(selector: e => e, id: id);
    public TResult GetResponseWarehouseIssue<TResult>(
        Expression<Func<ResponseWarehouseIssue, TResult>> selector,
        int id)
    {

      var result = GetResponseWarehouseIssues(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new ResponseWarehouseIssueNotFoundException(id);
      return result;
    }
    public ResponseWarehouseIssue GetResponseWarehouseIssue(string code) => GetResponseWarehouseIssue(selector: e => e, code: code);
    public TResult GetResponseWarehouseIssue<TResult>(
        Expression<Func<ResponseWarehouseIssue, TResult>> selector,
        string code)
    {

      var result = GetResponseWarehouseIssues(
                selector: selector,
                code: code).FirstOrDefault();
      if (result == null)
        throw new ResponseWarehouseIssueNotFoundException(code);
      return result;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetResponseWarehouseIssues<TResult>(
        Expression<Func<ResponseWarehouseIssue, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> warehouseIssueId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var responseWarehouseIssue = baseQuery.OfType<ResponseWarehouseIssue>();
      if (warehouseIssueId != null)
        responseWarehouseIssue = responseWarehouseIssue.Where(r => r.WarehouseIssue.Id == warehouseIssueId);

      return responseWarehouseIssue.Select(selector);
    }
    #endregion
    //#region AddProcess
    //public ResponseWarehouseIssue AddResponseWarehouseIssueProcess(
    //    TransactionBatch transactionBatch,
    //    int fromWarehouseId,
    //    int? toWarehouseId,
    //    AddTransactionInput[] transactions,
    //    string description)
    //{
    //    
    //        #region AddTransactionBatch
    //        var warehouseManagement = App.Internals.WarehouseManagement;
    //        transactionBatch = transactionBatch ?? warehouseManagement.AddTransactionBatch()
    //            
    //;
    //        #endregion
    //        #region Check IsDirectResponseWarehouseIssue

    //        var parameters = new Dictionary<string, string>();
    //        parameters.Add(key: "FromWarehouseId", value: fromWarehouseId.ToString());
    //        parameters.Add(key: "ToWarehouseId", value: toWarehouseId?.ToString() ?? fromWarehouseId.ToString());
    //        var directResponseWarehouseIssuePermission =
    //            App.Internals.UserManagement.CheckPermission(
    //                    actionName: Models.StaticData.StaticActionName.AcceptResponseWarehouseIssue,
    //                    parameters: parameters)
    //                
    //;
    //        #endregion
    //        #region AddProcess

    //        ResponseWarehouseIssue addFunc = null;
    //        if (directResponseWarehouseIssuePermission == AccessType.Allowed)
    //            addFunc = AddDirectResponseWarehouseIssueProcess(
    //                transactionBatch: transactionBatch,
    //                fromWarehouseId: fromWarehouseId,
    //                toWarehouseId: toWarehouseId,
    //                transactions: transactions,
    //                description: description);
    //        else
    //            addFunc = AddIndirectResponseWarehouseIssueProcess(
    //                transactionBatch: transactionBatch,
    //                fromWarehouseId: fromWarehouseId,
    //                toWarehouseId: toWarehouseId,
    //                transactions: transactions,
    //                description: description);
    //        var responseWarehouseIssue = addFunc;
    //        #endregion
    //        return responseWarehouseIssue;
    //    });
    //}
    //#endregion
    //#region AddIndirectProcess
    //public ResponseWarehouseIssue AddIndirectResponseWarehouseIssueProcess(
    //    TransactionBatch transactionBatch,
    //    int fromWarehouseId,
    //    int? toWarehouseId,
    //    AddTransactionInput[] transactions,
    //    string description)
    //{
    //    

    //        #region AddResponseWarehouseIssue
    //        var responseWarehouseIssue = AddResponseWarehouseIssue(
    //                responseWarehouseIssue: null,
    //                transactionBatch: transactionBatch,
    //                fromWarehouseId: fromWarehouseId,
    //                toWarehouseId: toWarehouseId,
    //                status: ResponseWarehouseIssueStatusType.Waiting,
    //                description: description)
    //            
    //;
    //        #endregion
    //        #region Add ResponseWarehouseIssueItems
    //        foreach (var transaction in transactions)
    //        {
    //            var exportAccessTransaction = AddWarehouseTransaction(
    //                    transactionBatchId: transactionBatch.Id,
    //                    effectDateTime: transactionBatch.DateTime,
    //                    stuffId: transaction.StuffId,
    //                    billOfMaterialVersion: transaction.Version,
    //                    stuffSerialCode: transaction.StuffSerialCode,
    //                    warehouseId: fromWarehouseId,
    //                    transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportAsIndirectStoreIssue.Id,
    //                    amount: transaction.Amount,
    //                    unitId: transaction.UnitId,
    //                    description: description,
    //                    referenceTransaction: null)
    //                
    //;
    //            var importBlockTransaction = AddWarehouseTransaction(
    //                    transactionBatchId: transactionBatch.Id,
    //                    effectDateTime: transactionBatch.DateTime,
    //                    stuffId: transaction.StuffId,
    //                    billOfMaterialVersion: transaction.Version,
    //                    stuffSerialCode: transaction.StuffSerialCode,
    //                    warehouseId: fromWarehouseId,
    //                    transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAsBlockedStoreIssue.Id,
    //                    amount: transaction.Amount,
    //                    unitId: transaction.UnitId,
    //                    description: description,
    //                    referenceTransaction: exportAccessTransaction)
    //                
    //;
    //        }
    //        #endregion
    //        return responseWarehouseIssue;
    //    });
    //}
    //#endregion  
    //#region AddDirectProcess
    //public ResponseWarehouseIssue AddDirectResponseWarehouseIssueProcess(
    //    TransactionBatch transactionBatch,
    //    int fromWarehouseId,
    //    int? toWarehouseId,
    //    AddTransactionInput[] transactions,
    //    string description)
    //{
    //    
    //        #region AddResponseWarehouseIssue
    //        var responseWarehouseIssue = AddResponseWarehouseIssue(
    //                responseWarehouseIssue: null,
    //                transactionBatch: transactionBatch,
    //                fromWarehouseId: fromWarehouseId,
    //                toWarehouseId: toWarehouseId,
    //                status: ResponseWarehouseIssueStatusType.Waiting,
    //                description: description)
    //            
    //;
    //        #endregion
    //        #region Add ResponseWarehouseIssueItems
    //        foreach (var transaction in transactions)
    //        {
    //            var exportAccessTransaction = AddWarehouseTransaction(
    //                    transactionBatchId: transactionBatch.Id,
    //                    effectDateTime: transactionBatch.DateTime,
    //                    stuffId: transaction.StuffId,
    //                    billOfMaterialVersion: transaction.Version,
    //                    stuffSerialCode: transaction.StuffSerialCode,
    //                    warehouseId: fromWarehouseId,
    //                    transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportAsDirectStoreIssue.Id,
    //                    amount: transaction.Amount,
    //                    unitId: transaction.UnitId,
    //                    description: description,
    //                    referenceTransaction: null)
    //                
    //;
    //            var importAccessTransaction = AddWarehouseTransaction(
    //                    transactionBatchId: transactionBatch.Id,
    //                    effectDateTime: transactionBatch.DateTime,
    //                    stuffId: transaction.StuffId,
    //                    billOfMaterialVersion: transaction.Version,
    //                    stuffSerialCode: transaction.StuffSerialCode,
    //                    warehouseId: fromWarehouseId,
    //                    transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAsDirectStoreIssue.Id,
    //                    amount: transaction.Amount,
    //                    unitId: transaction.UnitId,
    //                    description: description,
    //                    referenceTransaction: exportAccessTransaction)
    //                
    //;
    //        }
    //        #endregion
    //        return responseWarehouseIssue;
    //    });
    //}
    //#endregion
    //#region Search
    //public IQueryable<ResponseWarehouseIssueFullResult> SearchResponseWarehouseIssueResult(IQueryable<ResponseWarehouseIssueFullResult> query,
    //    string search,
    //    int? fromWarehouseId,
    //    int? toWarehouseId,
    //    int? responseWarehouseIssueTypeId,
    //    int? responseWarehouseIssueStatusTypeId,
    //    DateTime? fromDateTime,
    //    DateTime? toDateTime,
    //    int? stuffId
    //    )
    //{
    //    //if (!string.IsNullOrEmpty(search))
    //    //    query = query.Where(item =>
    //    //        item.StuffCode.Contains(search) ||
    //    //        item.StuffName.Contains(search));
    //    //if (fromWarehouseId != null)
    //    //    query = query.Where(i => i.FromWarehouseId == fromWarehouseId);
    //    //if (toWarehouseId != null)
    //    //    query = query.Where(i => i.ToWarehouseId == toWarehouseId);
    //    //if (responseWarehouseIssueTypeId != null)
    //    //    query = query.Where(i => (int)i.ResponseWarehouseIssueType == responseWarehouseIssueTypeId);
    //    //if (responseWarehouseIssueStatusTypeId != null)
    //    //    query = query.Where(i => i.ResponseWarehouseIssueItems.Any(s => (int)s.Status == responseWarehouseIssueStatusTypeId));
    //    //if (scrumEntityCode != null)
    //    //    query = query.Where(i => i.ScrumEntityCode == scrumEntityCode);
    //    //if (fromDateTime != null && toDateTime != null)
    //    //    query = query.Where(i => fromDateTime <= i.DateTime && i.DateTime <= toDateTime);
    //    //if (stuffId != null)
    //    //    query = query.Where(i => i.StuffId == stuffId);
    //    return query;
    //}
    //#endregion
    //#region Sort
    //public IOrderedQueryable<ResponseWarehouseIssueFullResult> SortResponseWarehouseIssueFullResult(IQueryable<ResponseWarehouseIssueFullResult> query,
    //    SortInput<ResponseWarehouseIssueSortType> sort)
    //{
    //    switch (sort.SortType)
    //    {
    //        //case ResponseWarehouseIssueSortType.Code:
    //        //    return query.OrderBy(a => a.Code, sort.SortOrder);
    //        //case ResponseWarehouseIssueSortType.DateTime:
    //        //    return query.OrderBy(a => a.DateTime, sort.SortOrder);
    //        //case ResponseWarehouseIssueSortType.FromWarehouse:
    //        //    return query.OrderBy(a => a.FromWarehouseName, sort.SortOrder);
    //        //case ResponseWarehouseIssueSortType.ProjectCode:
    //        //    return query.OrderBy(a => a.ScrumEntityCode, sort.SortOrder);
    //        //case ResponseWarehouseIssueSortType.ProjectName:
    //        //    return query.OrderBy(a => a.ScrumEntityName, sort.SortOrder);
    //        //case ResponseWarehouseIssueSortType.ResponseWarehouseIssueType:
    //        //    return query.OrderBy(a => a.ResponseWarehouseIssueType, sort.SortOrder);
    //        //case ResponseWarehouseIssueSortType.ToWarehouse:
    //        //    return query.OrderBy(a => a.ToWarehouseName, sort.SortOrder);
    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }
    //}
    //#endregion
    //#region ToResponseWarehouseIssueResult
    //public Expression<Func<ResponseWarehouseIssue, ResponseWarehouseIssueResult>> ToResponseWarehouseIssueResult =
    //    responseWarehouseIssue => new ResponseWarehouseIssueResult
    //    {
    //        Id = responseWarehouseIssue.Id,
    //        Code = responseWarehouseIssue.Code,
    //        //SendPermissionId = responseWarehouseIssue.SendPermissionId,
    //        //SendPermissionCode = responseWarehouseIssue.SendPermission.Code,
    //        //CustomerId = responseWarehouseIssue.SendPermission.OrderItemBlock.OrderItem.Order.CustomerId,
    //        //OrderItemId = responseWarehouseIssue.SendPermission.OrderItemBlock.OrderItemId,
    //        //OrderItemCode = responseWarehouseIssue.SendPermission.OrderItemBlock.OrderItem.Code,
    //        //OrderItemUnitId = responseWarehouseIssue.SendPermission.OrderItemBlock.OrderItem.UnitId,
    //        //OrderItemUnitName = responseWarehouseIssue.SendPermission.OrderItemBlock.OrderItem.Unit.Name,
    //        //OrderItemQty = responseWarehouseIssue.SendPermission.OrderItemBlock.Qty,
    //        //OrderItemBlockId = responseWarehouseIssue.SendPermission.OrderItemBlockId,
    //        //OrderItemBlockQty = responseWarehouseIssue.SendPermission.OrderItemBlock.Qty,
    //        //OrderItemBlockUnitId = responseWarehouseIssue.SendPermission.OrderItemBlock.UnitId,
    //        //OrderItemBlockUnitName = responseWarehouseIssue.SendPermission.OrderItemBlock.Unit.Name,
    //        //StuffId = responseWarehouseIssue.SendPermission.OrderItemBlock.OrderItem.StuffId,
    //        //StuffCode = responseWarehouseIssue.SendPermission.OrderItemBlock.OrderItem.Stuff.Code,
    //        //StuffName = responseWarehouseIssue.SendPermission.OrderItemBlock.OrderItem.Stuff.Name,
    //        //Qty = responseWarehouseIssue.SendPermission.Qty,
    //        //UnitId = responseWarehouseIssue.SendPermission.UnitId,
    //        //UnitName = responseWarehouseIssue.SendPermission.Unit.Name,
    //        //DateTime = responseWarehouseIssue.DateTime,
    //        //SendProductId = responseWarehouseIssue.SendProduct.Id,
    //        //ExitReceiptId = responseWarehouseIssue.SendProduct.ExitReceiptId,
    //        //ExitReceiptDateTime = responseWarehouseIssue.SendProduct.ExitReceipt.DateTime,
    //        //ExitReceiptConfirm = responseWarehouseIssue.SendProduct.ExitReceipt.Confirmed,
    //        RowVersion = responseWarehouseIssue.RowVersion
    //    };
    //#endregion
    //#region ToResponseWarehouseIssueFullResult
    //public Expression<Func<ResponseWarehouseIssue, ResponseWarehouseIssueFullResult>> ToResponseWarehouseIssueFullResult =
    //    responseWarehouseIssue => new ResponseWarehouseIssueFullResult
    //    {
    //        //Id = responseWarehouseIssue.Id,
    //        //Code = responseWarehouseIssue.Code,
    //        //FromWarehouseId = responseWarehouseIssue.FromWarehouseId,
    //        //FromWarehouseName = responseWarehouseIssue.FromWarehouse.Name,
    //        //ToWarehouseId = responseWarehouseIssue.ToWarehouseId,
    //        //ToWarehouseName = responseWarehouseIssue.ToWarehouse.Name,
    //        //ResponseWarehouseIssueType = responseWarehouseIssue.ResponseWarehouseIssueType,
    //        //DateTime = responseWarehouseIssue.DateTime,
    //        //IsDelete = responseWarehouseIssue.IsDelete,
    //        //Description = responseWarehouseIssue.Description,
    //        //UserId = responseWarehouseIssue.UserId,
    //        //EmployeeFullName = responseWarehouseIssue.User.Employee.FirstName + " " + responseWarehouseIssue.User.Employee.LastName,
    //        //ScrumEntityId = responseWarehouseIssue.ScrumEntityId,
    //        //ScrumEntityCode = responseWarehouseIssue.ScrumEntity.Code,
    //        //ScrumEntityName = responseWarehouseIssue.ScrumEntity.Name,
    //        //ResponseWarehouseIssueItems = responseWarehouseIssue.ResponseWarehouseIssueItems.AsQueryable()
    //        //    .Select(App.Internals.WarehouseManagement.ToResponseWarehouseIssueItemResult),
    //        //RowVersion = responseWarehouseIssue.RowVersion
    //    };
    //#endregion
  }
}

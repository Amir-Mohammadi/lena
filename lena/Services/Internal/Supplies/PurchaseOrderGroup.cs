using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.PurchaseOrderGroup;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Models.Supplies.PurchaseOrder;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region AddProcess
    public AddPurchaseOrderGroupResult AddPurchaseOrderGroupProcess(
       TransactionBatch transactionBatch,
       FinancialTransactionBatch financialTransactionBatch,
       string description,
       PurchaseOrderGroupItemInput[] purchaseOrderGroupItems)
    {

      var purchaseOrderGroup = AddPurchaseOrderGroup(
                    purchaseOrderGroup: null,
                    transactionBatch: transactionBatch,
                    financialTransactionBatch: financialTransactionBatch,
                    description: description);

      if (purchaseOrderGroupItems != null)
      {
        foreach (var item in purchaseOrderGroupItems)
        {
          AddToPurchaseOrderGroup(
                    id: item.Id,
                    rowVersion: item.RowVersion,
                    purchaseOrderGroupId: purchaseOrderGroup.Id);
        }
      }

      return new AddPurchaseOrderGroupResult { Id = purchaseOrderGroup.Id };
    }
    #endregion

    #region Add
    public PurchaseOrderGroup AddPurchaseOrderGroup(
       PurchaseOrderGroup purchaseOrderGroup,
       TransactionBatch transactionBatch,
       FinancialTransactionBatch financialTransactionBatch,
       string description)
    {

      purchaseOrderGroup = purchaseOrderGroup ?? repository.Create<PurchaseOrderGroup>();

      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: purchaseOrderGroup,
                transactionBatch: transactionBatch,
                financialTransactionBatch: financialTransactionBatch,
                description: description);

      var code = "POA-" + String.Format("{0:X}", purchaseOrderGroup.GetHashCode());
      App.Internals.ApplicationBase.EditBaseEntity(baseEntity: purchaseOrderGroup, rowVersion: purchaseOrderGroup.RowVersion, code: code);
      return purchaseOrderGroup;
    }
    #endregion

    #region Edit
    public PurchaseOrderGroup EditPurchaseOrderGroupProcess(
        int id,
        byte[] rowVersion,
        TValue<string> description,
        PurchaseOrderGroupItemInput[] addedPurchaseOrderGroupItems = null,
        PurchaseOrderGroupItemInput[] removedPurchaseOrderGroupItems = null
        )
    {


      var group = EditPurchaseOrderGroup(
                    id: id,
                    description: description,
                    rowVersion: rowVersion);

      if (addedPurchaseOrderGroupItems != null)
      {
        foreach (var item in addedPurchaseOrderGroupItems)
        {
          AddToPurchaseOrderGroup(item.Id, item.RowVersion, group.Id);
        }
      }

      if (removedPurchaseOrderGroupItems != null)
      {
        foreach (var item in removedPurchaseOrderGroupItems)
        {
          RemoveFromPurchaseOrderGroup(item.Id, item.RowVersion);

        }
      }
      return group;
    }
    #endregion
    #region Edit
    public PurchaseOrderGroup EditPurchaseOrderGroup(
        int id,
        byte[] rowVersion,
        TValue<string> description
        )
    {

      var purchaseOrderGroup = GetPurchaseOrderGroup(id);

      return EditPurchaseOrderGroup(
                    purchaseOrderGroup: purchaseOrderGroup,
                    rowVersion: rowVersion,
                    description: description);
    }
    #endregion
    #region Edit
    public PurchaseOrderGroup EditPurchaseOrderGroup(
        PurchaseOrderGroup purchaseOrderGroup,
        byte[] rowVersion,
        TValue<string> description
        )
    {

      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: purchaseOrderGroup,
                    description: description,
                    rowVersion: rowVersion);

      return retValue as PurchaseOrderGroup;
    }
    #endregion

    #region Get
    public PurchaseOrderGroup GetPurchaseOrderGroup(int id) => GetPurchaseOrderGroup(e => e, id: id);
    public TResult GetPurchaseOrderGroup<TResult>(
        Expression<Func<PurchaseOrderGroup, TResult>> selector,
        int id)
    {

      var purchaseOrderGroup = GetPurchaseOrderGroups(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (purchaseOrderGroup == null)
        throw new PurchaseOrderGroupNotFoundException(id);
      return purchaseOrderGroup;
    }
    public PurchaseOrderGroup GetPurchaseOrderGroup(string code) => GetPurchaseOrderGroup(e => e, code: code);
    public TResult GetPurchaseOrderGroup<TResult>(
        Expression<Func<PurchaseOrderGroup, TResult>> selector,
        string code)
    {

      var purchaseOrderGroup = GetPurchaseOrderGroups(
                    selector: selector,
                    code: code)


                .FirstOrDefault();
      if (purchaseOrderGroup == null)
        throw new PurchaseOrderGroupNotFoundException(code);
      return purchaseOrderGroup;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetPurchaseOrderGroups<TResult>(
        Expression<Func<PurchaseOrderGroup, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<PurchaseOrderGroup>();

      if (description != null)
        query = query.Where(i => i.Description == description);

      return query.Select(selector);
    }
    #endregion
    #region ToPurchaseOrderGroupResultQuery
    public IQueryable<PurchaseOrderGroupResult> ToPurchaseOrderGroupResultQuery(IQueryable<PurchaseOrderGroup> query)
    {

      var resultQuery = from purchaseOrderGroup in query

                        select new PurchaseOrderGroupResult
                        {
                          Id = purchaseOrderGroup.Id,
                          Code = purchaseOrderGroup.Code,
                          DateTime = purchaseOrderGroup.DateTime,
                          Description = purchaseOrderGroup.Description,
                          FinancialTransacionBatchId = purchaseOrderGroup.FinancialTransactionBatch.Id,
                          EmployeeFullName = purchaseOrderGroup.User.Employee.FirstName + " " + purchaseOrderGroup.User.Employee.LastName,
                          RowVersion = purchaseOrderGroup.RowVersion,
                        };

      return resultQuery;
    }
    public Expression<Func<PurchaseOrderGroup, PurchaseOrderGroupResult>> ToPurchaseOrderGroupResult =
        purchaseOrderGroup => new PurchaseOrderGroupResult()
        {
          Id = purchaseOrderGroup.Id,
          Code = purchaseOrderGroup.Code,
          DateTime = purchaseOrderGroup.DateTime,
          Description = purchaseOrderGroup.Description,
          FinancialTransacionBatchId = purchaseOrderGroup.FinancialTransactionBatch.Id,
          EmployeeFullName = purchaseOrderGroup.User.Employee.FirstName + " " + purchaseOrderGroup.User.Employee.LastName,
          RowVersion = purchaseOrderGroup.RowVersion,
        };
    #endregion
    #region Search
    public IQueryable<PurchaseOrderGroupResult> SearchPurchaseOrderGroup(
        IQueryable<PurchaseOrderGroupResult> query,
         AdvanceSearchItem[] advanceSearchItems,
        string searchText,
        string code,
        DateTime? fromDateTime,
        DateTime? toDateTime
       )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where
                      item.Code.Contains(searchText) ||
                      item.Description.Contains(searchText)
                select item;
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);
      if (code != null)
        query = query.Where(i => i.Code == code);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<PurchaseOrderGroupResult> SortPurchaseOrderGroupResult(IQueryable<PurchaseOrderGroupResult> query, SearchInput<PurchaseOrderGroupSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case PurchaseOrderGroupSortType.Code:
          return query.OrderBy(i => i.Code, sortInput.SortOrder);
        case PurchaseOrderGroupSortType.DateTime:
          return query.OrderBy(i => i.DateTime, sortInput.SortOrder);
        case PurchaseOrderGroupSortType.EmployeeFullName:
          return query.OrderBy(i => i.EmployeeFullName, sortInput.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Remove
    public void RemovePurchaseOrderGroup(int id, byte[] rowVersion)
    {

      var purchaseOrderGroup = GetPurchaseOrderGroup(id: id);

      if (purchaseOrderGroup == null)
        throw new PurchaseOrderGroupNotFoundException(id);

      if (purchaseOrderGroup.PurchaseOrders.Any())
      {
        foreach (var purchaseOrder in purchaseOrderGroup.PurchaseOrders)
        {
          RemoveFromPurchaseOrderGroup(
                    id: purchaseOrder.Id,
                    rowVersion: purchaseOrder.RowVersion
                 );
        }
      }


      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: null,
                    baseEntity: purchaseOrderGroup,
                    rowVersion: rowVersion);
    }
    #endregion

  }
}

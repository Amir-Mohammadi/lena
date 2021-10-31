//using LinqLib.Sort;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.SaleManagement.OrderDocument;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    public void AddOrderDocument(int orderId,
        Guid documentId,
        string description)
    {

      var user = App.Providers.Security.CurrentLoginData;
      var orderDocument = repository.Create<OrderDocument>();
      orderDocument.OrderId = orderId;
      orderDocument.DocumentId = documentId;
      orderDocument.ModifiedDate = DateTime.Now;
      orderDocument.UserId = user.UserId;
      orderDocument.Description = description;
      repository.Add(orderDocument);
    }

    #region Gets
    public IQueryable<TResult> GetOrderDocuments<TResult>(
           Expression<Func<OrderDocument, TResult>> selector,
           TValue<int> orderCode = null,
           TValue<int> orderId = null,
           TValue<DateTime> modifiedDate = null,
           TValue<DateTime> fromCreateDateTime = null,
           TValue<DateTime?> toCreateDateTime = null,
           TValue<int> userId = null,
           TValue<string> employeeFullName = null,
           TValue<string> documentFullName = null,
           TValue<Guid> documentId = null,
           TValue<string> documentName = null,
           TValue<string> description = null,
           TValue<bool> isDelete = null,
           TValue<byte[]> rowVersion = null
           )
    {


      var OrderDocument = repository.GetQuery<OrderDocument>();

      if (orderId != null)
        OrderDocument = OrderDocument.Where(r => r.OrderId == orderId);

      if (documentId != null)
        OrderDocument = OrderDocument.Where(r => r.DocumentId == documentId);

      if (fromCreateDateTime != null)
        OrderDocument = OrderDocument.Where(r => r.ModifiedDate >= fromCreateDateTime);

      if (toCreateDateTime != null)
        OrderDocument = OrderDocument.Where(r => r.ModifiedDate <= toCreateDateTime);

      if (userId != null)
        OrderDocument = OrderDocument.Where(r => r.UserId == userId);

      if (description != null)
        OrderDocument = OrderDocument.Where(i => i.Description == description);

      if (isDelete != null)
        OrderDocument = OrderDocument.Where(i => i.IsDelete == isDelete);

      return OrderDocument.Select(selector);
    }
    #endregion

    #region GetOrderDocument
    public OrderDocument GetOrderDocument(int orderId) => GetOrderDocument(selector: e => e, id: orderId);
    public TResult GetOrderDocument<TResult>(
        Expression<Func<OrderDocument, TResult>> selector,
        int id)
    {

      var orderDocument = GetOrderDocuments(selector: selector,
                orderId: id).FirstOrDefault();
      return orderDocument;
    }
    #endregion


    public void EditOrderDocument(
        int orderId,
        Guid documentId,
        TValue<string> description = null,
        TValue<bool> isDelete = null,
        TValue<byte[]> rowversion = null)
    {

      var orderDocument = GetOrderDocument(orderId: orderId);
      var user = App.Providers.Security.CurrentLoginData;
      if (description != null)
        orderDocument.Description = description;
      if (isDelete != null)
        orderDocument.IsDelete = isDelete;
      orderDocument.ModifiedDate = DateTime.Now.ToUniversalTime();
      orderDocument.UserId = user.UserId;
      repository.Update(orderDocument, rowversion);
    }

    public void DeleteOrderDocument(int orderId,
        Guid documentId,
        byte[] rowVersion)
    {

      EditOrderDocument(orderId: orderId, documentId: documentId, isDelete: true, rowversion: rowVersion);
    }

    #region ToFullResult
    public Expression<Func<OrderDocument, OrderDocumentResult>> ToOrderDocumentResult =
        OrderDocument => new OrderDocumentResult
        {
          OrderCode = OrderDocument.OrderId,
          ModifiedDate = OrderDocument.ModifiedDate,
          UserId = OrderDocument.UserId,
          EmployeeFullName = OrderDocument.User.Employee.FirstName + " " + OrderDocument.User.Employee.LastName,
          DocumentFullName = OrderDocument.Document.Name,
          DocumentId = OrderDocument.DocumentId,
          Description = OrderDocument.Description,
          RowVersion = OrderDocument.RowVersion,
        };
    #endregion

    #region Search
    public IQueryable<OrderDocumentResult> SearchOrderDocumentResult(
        IQueryable<OrderDocumentResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText = null)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = query.Where(item =>
            item.Description.ToString().Contains(searchText) ||
            item.OrderCode.ToString().Contains(searchText) ||
            item.ModifiedDate.ToString().Contains(searchText) ||
            item.DocumentName.Contains(searchText));

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion


    #region Sort
    public IQueryable<OrderDocumentResult> SortOrderDocumentResult(
           IQueryable<OrderDocumentResult> query,
        SortInput<OrderDocumentSortType> sort)
    {
      switch (sort.SortType)
      {
        case OrderDocumentSortType.OrderCode:
          return query.OrderBy(a => a.OrderCode, sort.SortOrder);
        case OrderDocumentSortType.ModifiedDate:
          return query.OrderBy(a => a.ModifiedDate, sort.SortOrder);
        case OrderDocumentSortType.UserId:
          return query.OrderBy(a => a.UserId, sort.SortOrder);
        case OrderDocumentSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case OrderDocumentSortType.DocumentFullName:
          return query.OrderBy(a => a.DocumentFullName, sort.SortOrder);
        case OrderDocumentSortType.DocumentId:
          return query.OrderBy(a => a.DocumentId, sort.SortOrder);
        case OrderDocumentSortType.DocumentName:
          return query.OrderBy(a => a.DocumentName, sort.SortOrder);
        case OrderDocumentSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case OrderDocumentSortType.RowVersion:
          return query.OrderBy(a => a.RowVersion, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}

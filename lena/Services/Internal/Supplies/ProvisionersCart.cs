using System;
using System.Linq;
using System.Linq.Expressions;
//using System.Runtime.Remoting.Channels;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Services.Core;
using lena.Services.Core.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.Common;
using lena.Models.Supplies.ProvisionersCart;
using lena.Models.Supplies.ProvisionersCartItem;
using lena.Models.Supplies.CargoItem;
using lena.Models.ApplicationBase.BaseEntityDocument;
using lena.Models.ApplicationBase.Unit;
using lena.Services.Internals.Accounting.Exception;
using System.Collections.Generic;
using lena.Models.StaticData;
using lena.Models.Supplies.ProvisionersCartItemDetail;
using lena.Models.Supplies.PurchaseRequest;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add

    public ProvisionersCart AddProvisionersCart(
        string description,
        int supplierId,
        DateTime reportDate
    )
    {

      var provisionersCart = repository.Create<ProvisionersCart>();
      provisionersCart.Description = description;
      provisionersCart.SupplierId = supplierId;
      provisionersCart.ResponsibleEmployeeId = App.Providers.Security.CurrentLoginData.UserId;
      provisionersCart.DateTime = DateTime.UtcNow;
      provisionersCart.ReportDate = reportDate;
      provisionersCart.Status = ProvisionersCartStatus.ToDo;
      repository.Add(provisionersCart);
      return provisionersCart;
    }
    #endregion
    #region AddPProcess
    public ProvisionersCart AddProvisionersCartProcess(
        string description,
        int supplierId,
        DateTime reportDate,
        AddProvisionersCartItemInput[] addProvisionersCartItemInputs
        )
    {

      #region  AddProvisionersCart         
      var provisionersCart = AddProvisionersCart(

              description: description,
              supplierId: supplierId,
              reportDate: reportDate
             );
      #endregion
      foreach (var item in addProvisionersCartItemInputs)
      {
        var purchaseRequest = GetPurchaseRequest(
                      id: item.PurchaseRequestId);
        #region CheckExistProvisionersPurchaseDetail
        var existProvisionersCartItems = GetProvisionersCartItems(
            selector: e => e,
            purchaseRequestId: item.PurchaseRequestId
            );
        if (existProvisionersCartItems.Any())
          throw new ProvisionersCartItemPurchaseRequestExistException(purchaseRequest.Id);
        #endregion
        AddProvisionersCartItem(
            providerId: item.ProviderId,
            provisionersCartId: provisionersCart.Id,
            purchaseRequest: purchaseRequest,
            requestQty: item.RequestQty,
            suppledQty: item.SuppliedQty);
      }

      return provisionersCart;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProvisionersCarts<TResult>(
        Expression<Func<ProvisionersCart, TResult>> selector,
        TValue<int> id = null,
        TValue<string> description = null,
        TValue<int> supplierId = null,
        TValue<int> responsibleEmployeeId = null,
        TValue<int> employeeId = null,
        TValue<DateTime> fromRegisterDate = null,
        TValue<DateTime> toRegisterDate = null,
        TValue<DateTime> reportDate = null
        )
    {

      var baseQuery = repository.GetQuery<ProvisionersCart>();
      if (id != null)
        baseQuery = baseQuery.Where(i => i.Id == id);
      if (description != null)
        baseQuery = baseQuery.Where(i => i.Description == description);
      if (supplierId != null)
        baseQuery = baseQuery.Where(i => i.SupplierId == supplierId);
      if (responsibleEmployeeId != null)
        baseQuery = baseQuery.Where(i => i.ResponsibleEmployeeId == responsibleEmployeeId);
      if (employeeId != null)
        baseQuery = baseQuery.Where(i => i.ResponsibleEmployee.Employee.Id == employeeId);
      if (fromRegisterDate != null)
        baseQuery = baseQuery.Where(i => i.ReportDate >= fromRegisterDate);
      if (toRegisterDate != null)
        baseQuery = baseQuery.Where(i => i.ReportDate <= toRegisterDate);
      return baseQuery.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<ProvisionersCart, ProvisionersCartResult>> ToProvisionersCartResult =
        provisionersCart => new ProvisionersCartResult
        {
          Id = provisionersCart.Id,
          Description = provisionersCart.Description,
          SupplierId = provisionersCart.SupplierId,
          ResponsibleEmployeeId = provisionersCart.ResponsibleEmployeeId,
          RegisterDateTime = provisionersCart.DateTime,
          ReportDate = provisionersCart.ReportDate,
          Status = provisionersCart.Status,
          RegisterEmployeeFullName = provisionersCart.ResponsibleEmployee.Employee.FirstName + " " + provisionersCart.ResponsibleEmployee.Employee.LastName,
          SupplierName = provisionersCart.Supplier.Employee.FirstName + " " + provisionersCart.Supplier.Employee.LastName
        };

    #endregion
    #region Search
    public IQueryable<ProvisionersCartResult> SearchProvisionersCart(IQueryable<ProvisionersCartResult>
    query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems
    )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.SupplierName.Contains(searchText) ||
            item.RegisterEmployeeFullName.Contains(searchText) ||
            item.Description.Contains(searchText)
            );
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ProvisionersCartResult> SortProvisionersCartResult(IQueryable<ProvisionersCartResult> query,
        SortInput<ProvisionersCartSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProvisionersCartSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ProvisionersCartSortType.DateTime:
          return query.OrderBy(a => a.RegisterDateTime, sort.SortOrder);
        case ProvisionersCartSortType.ReportDate:
          return query.OrderBy(a => a.ReportDate, sort.SortOrder);
        case ProvisionersCartSortType.EmployeeFullName:
          return query.OrderBy(a => a.RegisterEmployeeFullName, sort.SortOrder);
        case ProvisionersCartSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case ProvisionersCartSortType.SupplierName:
          return query.OrderBy(a => a.SupplierName, sort.SortOrder);
        case ProvisionersCartSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region GetFullResult
    public ProvisionersCart GetFullProvisionersCart(
        TValue<int> id = null)
    {

      var provisionersCart = App.Internals.Supplies.GetProvisionersCarts(
             selector: e => e,
             id: id)


             .FirstOrDefault();
      return provisionersCart;
    }
    #endregion

    #region ToFullResult
    public IQueryable<FullProvisionersCartResult> ToFullProvisionersCartResult(
       IQueryable<ProvisionersCart> provisionersCarts)
    {
      var resultQuery = from provisionersCart in provisionersCarts
                        select new FullProvisionersCartResult
                        {
                          Id = provisionersCart.Id,
                          RegisterEmployeeFullName = provisionersCart.ResponsibleEmployee.Employee.FirstName + " " + provisionersCart.ResponsibleEmployee.Employee.LastName,
                          SupplierName = provisionersCart.Supplier.Employee.FirstName + " " + provisionersCart.Supplier.Employee.LastName,
                          Description = provisionersCart.Description,
                          Status = provisionersCart.Status,
                          RowVersion = provisionersCart.RowVersion,
                          SupplierId = provisionersCart.SupplierId,
                          RegisterDateTime = provisionersCart.DateTime,
                          ReportDate = provisionersCart.ReportDate,
                          ResponsibleEmployeeId = provisionersCart.ResponsibleEmployeeId,
                          ProvisionersCartItems = provisionersCart.ProvisionersCartItems.AsQueryable().Select(App.Internals.Supplies.ToProvisionersCartItemResult),
                        };
      return resultQuery;
    }
    #endregion

    #region ToFullReportResult
    public IQueryable<FullProvisionersCartReportResult> ToFullProvisionersCartReportResult(
       IQueryable<ProvisionersCart> provisionersCarts)
    {

      var supplies = App.Internals.Supplies;
      var provisionersCartItems = supplies.GetProvisionersCartItems(selector: e => e); ; var provisionersCartItemDetails = supplies.GetProvisionersCartItemDetails(selector: e => e);
      var resultQuery = from provisionersCart in provisionersCarts
                        join provisionersCartItem in provisionersCartItems on provisionersCart.Id
                              equals provisionersCartItem.ProvisionersCartId into allProvisionersCartItem
                        from provisionersCartItemLeftJoin in allProvisionersCartItem.DefaultIfEmpty()
                        join provisionersCartItemDetail in provisionersCartItemDetails on provisionersCartItemLeftJoin.Id
                              equals provisionersCartItemDetail.ProvisionersCartItemId into allProvisionersCartItemDetail
                        from provisionersCartItemDetailLeftJoin in allProvisionersCartItemDetail.DefaultIfEmpty()
                        select new FullProvisionersCartReportResult
                        {
                          Id = provisionersCart.Id,
                          DateTime = provisionersCartItemDetailLeftJoin.DateTime,
                          DetailDescription = provisionersCartItemDetailLeftJoin.Description,
                          UnitPrice = provisionersCartItemDetailLeftJoin.UnitPrice,
                          DetailProviderName = provisionersCartItemDetailLeftJoin.Provider.Name,
                          SupplyQty = provisionersCartItemDetailLeftJoin.SupplyQty,
                          ProvisionersCartItemId = provisionersCartItemLeftJoin.Id,
                          RequestQty = provisionersCartItemLeftJoin.RequestQty,
                          SuppliedQty = provisionersCartItemLeftJoin.SuppliedQty,
                          ProviderName = provisionersCartItemLeftJoin.Provider.Name,
                          ItemStatus = provisionersCartItemLeftJoin.Status,
                          PurchaseRequestStuffCode = provisionersCartItemLeftJoin.PurchaseRequest.Stuff.Code,
                          PurchaseRequestStuffName = provisionersCartItemLeftJoin.PurchaseRequest.Stuff.Name,
                          PurchaseRequestUnitName = provisionersCartItemLeftJoin.PurchaseRequest.Unit.Name,
                          Status = provisionersCart.Status,
                          SupplierName = provisionersCart.Supplier.Employee.FirstName + " " + provisionersCart.Supplier.Employee.LastName,
                          RegisterEmployeeFullName = provisionersCart.ResponsibleEmployee.Employee.FirstName + " " + provisionersCart.ResponsibleEmployee.Employee.LastName,
                          RegisterDateTime = provisionersCart.DateTime,
                          ReportDate = provisionersCart.ReportDate,
                          Description = provisionersCart.Description,
                        };
      return resultQuery;
    }
    #endregion

    #region Get 
    public ProvisionersCart GetProvisionersCart(int id) => GetProvisionersCart(selector: e => e, id: id);
    public TResult GetProvisionersCart<TResult>(
        Expression<Func<ProvisionersCart, TResult>> selector,
        int id)
    {

      var provisionersCart = GetProvisionersCarts(selector: selector, id: id)


            .FirstOrDefault();
      if (provisionersCart == null)
        throw new ProvisionersCartItemNotFoundException(id);
      return provisionersCart;
    }
    #endregion

    #region ResetStatus
    public ProvisionersCart ResetProvisionersCartStatus(int provisionersCartId)
    {

      var provisionersCart = GetFullProvisionersCart(id: provisionersCartId);
      return ResetProvisionersCartStatus(provisionersCart: provisionersCart);

    }
    public ProvisionersCart ResetProvisionersCartStatus(ProvisionersCart provisionersCart)
    {

      var provisionersCartItems = GetProvisionersCartItems(
                    selector: e => e,
                    provisionersCartId: provisionersCart.Id);

      var cartItemStatus = ProvisionersCartItemStatus.Completed;


      foreach (var provisionersCartItem in provisionersCartItems)
      {
        cartItemStatus &= provisionersCartItem.Status;
      }
      if (cartItemStatus == ProvisionersCartItemStatus.Completed)
      {
        provisionersCart.Status = ProvisionersCartStatus.Done;
      }
      else
      {
        provisionersCart.Status = ProvisionersCartStatus.ToDo;
      }
      var editProvisionersCart = EditProvisionersCart(id: provisionersCart.Id, rowVersion: provisionersCart.RowVersion, status: provisionersCart.Status);
      return provisionersCart;
    }
    #endregion

    #region Edit
    public ProvisionersCart EditProvisionersCart(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<int> supplierId = null,
        TValue<ProvisionersCartStatus> status = null
        )
    {

      var provisionersCart = GetProvisionersCart(id: id);
      if (supplierId != null)
        provisionersCart.SupplierId = supplierId;
      if (description != null)
        provisionersCart.Description = description;
      if (status != null)
        provisionersCart.Status = status;

      repository.Update(rowVersion: rowVersion, entity: provisionersCart);
      return provisionersCart;
    }
    #endregion
  }
}

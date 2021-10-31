using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using lena.Models.Common;
using lena.Services.Internals.Accounting.Exception;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Accounting.DetailedCodeConfirmationRequest;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Add
    internal DetailedCodeConfirmationRequest AddDetailedCodeConfirmationRequestProcess(
        int? cooperatorId,
        int? productionLineId,
        DetailedCodeRequestType detailedCodeRequestType,
        DetailedCodeEntityType detailedCodeEntityType,
        string detailedCode,
        string description)
    {
      #region PreviousRequest
      var detailedCodeConfirmationRequests = GetDetailedCodeConfirmationRequests(
              selector: e => e,
              cooperatorId: cooperatorId,
              productionLineId: productionLineId,
              status: DetailCodeConfirmationStatus.NotAction);
      if (detailedCodeConfirmationRequests.Any())
      {
        throw new DetailedCodeConfirmationRequestExistsException(
                  id: detailedCodeConfirmationRequests.FirstOrDefault().Id);
      }
      #endregion
      var detailedCodeConfirmationRequest = AddDetailedCodeConfirmationRequest(
              cooperatorId: cooperatorId,
              productionLineId: productionLineId,
              detailedCodeRequestType: detailedCodeRequestType,
              detailedCodeEntityType: detailedCodeEntityType,
              detailedCode: detailedCode,
              description: description);
      return detailedCodeConfirmationRequest;
    }
    public DetailedCodeConfirmationRequest AddDetailedCodeConfirmationRequest(
        int? cooperatorId,
        int? productionLineId,
        DetailedCodeEntityType detailedCodeEntityType,
        DetailedCodeRequestType detailedCodeRequestType,
        string detailedCode,
        string description)
    {
      var detailedCodeConfirmationRequest = repository.Create<DetailedCodeConfirmationRequest>();
      detailedCodeConfirmationRequest.CooperatorId = cooperatorId;
      detailedCodeConfirmationRequest.ProductionLineId = productionLineId;
      detailedCodeConfirmationRequest.DateTime = DateTime.Now.ToUniversalTime();
      detailedCodeConfirmationRequest.UserId = App.Providers.Security.CurrentLoginData.UserId;
      detailedCodeConfirmationRequest.Description = description;
      detailedCodeConfirmationRequest.DetailedCode = detailedCode;
      detailedCodeConfirmationRequest.Status = DetailCodeConfirmationStatus.NotAction;
      detailedCodeConfirmationRequest.DetailedCodeRequestType = detailedCodeRequestType;
      detailedCodeConfirmationRequest.DetailedCodeEntityType = detailedCodeEntityType;
      repository.Add(detailedCodeConfirmationRequest);
      return detailedCodeConfirmationRequest;
    }
    #endregion
    #region Get
    public DetailedCodeConfirmationRequest GetDetailedCodeConfirmationRequest(int id) => GetDetailedCodeConfirmationRequest(selector: e => e, id: id);
    public TResult GetDetailedCodeConfirmationRequest<TResult>(
        Expression<Func<DetailedCodeConfirmationRequest, TResult>> selector,
        int id)
    {
      var detailedCodeConfirmationRequest = GetDetailedCodeConfirmationRequests(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (detailedCodeConfirmationRequest == null)
        throw new DetailedCodeConfirmationRequestNotFoundException(id);
      return detailedCodeConfirmationRequest;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetDetailedCodeConfirmationRequests<TResult>(
        Expression<Func<DetailedCodeConfirmationRequest, TResult>> selector,
        TValue<int> id = null,
        TValue<int> userId = null,
        TValue<int> cooperatorId = null,
        TValue<int> productionLineId = null,
        TValue<string> description = null,
        TValue<DetailCodeConfirmationStatus> status = null,
        TValue<DetailCodeConfirmationStatus[]> statuses = null)
    {
      var query = repository.GetQuery<DetailedCodeConfirmationRequest>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (statuses != null)
        query = query.Where(i => statuses.Value.Contains(i.Status));
      if (status != null)
        query = query.Where(i => i.Status == status);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (cooperatorId != null)
        query = query.Where(i => i.CooperatorId == cooperatorId);
      if (productionLineId != null)
        query = query.Where(i => i.ProductionLineId == productionLineId);
      return query.Select(selector);
    }
    #endregion
    #region Sort
    internal IOrderedQueryable<DetailedCodeConfirmationRequestResult> SortDetailedCodeConfirmationRequestResult(
        IQueryable<DetailedCodeConfirmationRequestResult> query,
        SortInput<DetailedCodeConfirmationRequestSortType> options)
    {
      switch (options.SortType)
      {
        case DetailedCodeConfirmationRequestSortType.DateTime:
          return query.OrderBy(a => a.DateTime, options.SortOrder);
        case DetailedCodeConfirmationRequestSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, options.SortOrder);
        case DetailedCodeConfirmationRequestSortType.ConfirmationDateTime:
          return query.OrderBy(a => a.ConfirmationDateTime, options.SortOrder);
        case DetailedCodeConfirmationRequestSortType.ConfirmationEmployeeFullName:
          return query.OrderBy(a => a.ConfirmationEmployeeFullName, options.SortOrder);
        case DetailedCodeConfirmationRequestSortType.Status:
          return query.OrderBy(a => a.Status, options.SortOrder);
        case DetailedCodeConfirmationRequestSortType.DetailedCodeRequestType:
          return query.OrderBy(a => a.DetailedCodeRequestType, options.SortOrder);
        case DetailedCodeConfirmationRequestSortType.Description:
          return query.OrderBy(a => a.Description, options.SortOrder);
        case DetailedCodeConfirmationRequestSortType.DetailedCode:
          return query.OrderBy(a => a.DetailedCode, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    internal Expression<Func<DetailedCodeConfirmationRequest, DetailedCodeConfirmationRequestResult>> ToDetailedCodeConfirmationRequestResult =
        detailedCodeConfirmationRequest => new DetailedCodeConfirmationRequestResult()
        {
          Id = detailedCodeConfirmationRequest.Id,
          CooperatorId = detailedCodeConfirmationRequest.CooperatorId,
          CooperatorCode = detailedCodeConfirmationRequest.Cooperator.Code,
          CooperatorName = detailedCodeConfirmationRequest.Cooperator.Name,
          CooperatorDetailedCode = detailedCodeConfirmationRequest.Cooperator.DetailedCode,
          CooperatorType = detailedCodeConfirmationRequest.Cooperator.CooperatorType,
          ProductionLineId = detailedCodeConfirmationRequest.ProductionLineId,
          ProductionLineName = detailedCodeConfirmationRequest.ProductionLine.Name,
          ProductionLineDetailedCode = detailedCodeConfirmationRequest.ProductionLine.DetailedCode,
          DateTime = detailedCodeConfirmationRequest.DateTime,
          UserId = detailedCodeConfirmationRequest.UserId,
          EmployeeFullName = detailedCodeConfirmationRequest.User.Employee.FirstName + " " + detailedCodeConfirmationRequest.User.Employee.LastName,
          ConfirmationUserId = detailedCodeConfirmationRequest.ConfirmationUserId,
          ConfirmationEmployeeFullName = detailedCodeConfirmationRequest.DetailedCodeConfirmerUser.Employee.FirstName + " " + detailedCodeConfirmationRequest.DetailedCodeConfirmerUser.Employee.LastName,
          ConfirmationDateTime = detailedCodeConfirmationRequest.ConfirmationDateTime,
          Status = detailedCodeConfirmationRequest.Status,
          DetailedCodeRequestType = detailedCodeConfirmationRequest.DetailedCodeRequestType,
          DetailedCodeEntityType = detailedCodeConfirmationRequest.DetailedCodeEntityType,
          Description = detailedCodeConfirmationRequest.Description,
          DetailedCode = detailedCodeConfirmationRequest.DetailedCode,
          RowVersion = detailedCodeConfirmationRequest.RowVersion,
        };
    #endregion
    #region Search
    internal IQueryable<DetailedCodeConfirmationRequestResult> SearchDetailedCodeConfirmationRequestResultQuery(
        IQueryable<DetailedCodeConfirmationRequestResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where
                    item.EmployeeFullName.Contains(searchText) ||
                    item.ConfirmationEmployeeFullName.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region AcceptDetailedCodeConfirmationRequest
    public DetailedCodeConfirmationRequest AcceptDetailedCodeConfirmationRequestProcess(
        int id,
        DetailedCodeRequestType detailedCodeRequestType,
        string detailedCode,
        byte[] rowVersion)
    {
      var detailedCodeConfirmationRequest = GetDetailedCodeConfirmationRequest(id: id);
      return AcceptDetailedCodeConfirmationRequestProcess(
                    detailedCodeConfirmationRequest: detailedCodeConfirmationRequest,
                    detailedCodeRequestType: detailedCodeRequestType,
                    detailedCode: detailedCode,
                    rowVersion: rowVersion);
    }
    public DetailedCodeConfirmationRequest AcceptDetailedCodeConfirmationRequestProcess(
        DetailedCodeConfirmationRequest detailedCodeConfirmationRequest,
        DetailedCodeRequestType detailedCodeRequestType,
        string detailedCode,
        byte[] rowVersion)
    {
      if (detailedCodeConfirmationRequest.Status != DetailCodeConfirmationStatus.NotAction)
      {
        throw new DetailedCodeConfirmationRequestNotFoundException(id: detailedCodeConfirmationRequest.Id);
      }
      Cooperator customer = null;
      Cooperator provider = null;
      ProductionLine producionLine = null;
      #region ConfirmationCustomerDetailedCode
      if (detailedCodeConfirmationRequest.DetailedCodeEntityType == DetailedCodeEntityType.Customer)
      {
        #region GetCustomer
        customer = App.Internals.SaleManagement.GetCustomer(
        id: (int)detailedCodeConfirmationRequest.CooperatorId);
        #endregion
        #region ConfirmationCustomerDetailedCode
        App.Internals.SaleManagement.ConfirmationCustomerDetailedCode(
        id: customer.Id,
        detailedCode: detailedCode,
        rowVersion: customer.RowVersion);
        #endregion
      }
      #endregion
      #region ConfirmationProviderDetailedCode
      if (detailedCodeConfirmationRequest.DetailedCodeEntityType == DetailedCodeEntityType.Provider)
      {
        #region GetProvider
        provider = App.Internals.SaleManagement.GetProvider(
        id: (int)detailedCodeConfirmationRequest.CooperatorId);
        #endregion
        #region ConfirmationProviderDetailedCode
        App.Internals.SaleManagement.ConfirmationProviderDetailedCode(
        id: provider.Id,
        detailedCode: detailedCode,
        rowVersion: provider.RowVersion);
        #endregion
      }
      #endregion
      #region ConfirmationOfProductionLineDetailedCode
      if (detailedCodeConfirmationRequest.DetailedCodeEntityType == DetailedCodeEntityType.ProductionLine)
      {
        #region GetProductionLine
        producionLine = App.Internals.Planning.GetProductionLine(
        id: (int)detailedCodeConfirmationRequest.ProductionLineId);
        #endregion
        #region ConfirmationOfProductionLineDetailedCode
        App.Internals.Planning.ConfirmationOfProductionLineDetailedCode(
        id: producionLine.Id,
        detailedCode: detailedCode,
        rowVersion: producionLine.RowVersion);
        #endregion
      }
      #endregion
      #region Update DetailedCodeConfirmationRequest Status
      detailedCodeConfirmationRequest = EditDetailedCodeConfirmationRequest(
              detailedCodeConfirmationRequest: detailedCodeConfirmationRequest,
              rowVersion: rowVersion,
              detailedCode: detailedCode,
              status: DetailCodeConfirmationStatus.Accepted);
      #endregion
      return detailedCodeConfirmationRequest;
    }
    #endregion
    #region RejectDetailedCodeConfirmationRequest
    public DetailedCodeConfirmationRequest RejectDetailedCodeConfirmationRequestProcess(
        int id,
        string detailedCode,
        DetailedCodeEntityType detailedCodeEntityType,
        byte[] rowVersion)
    {
      var detailedCodeConfirmationRequest = GetDetailedCodeConfirmationRequest(id: id);
      return RejectDetailedCodeConfirmationRequestProcess(
                    detailedCodeConfirmationRequest: detailedCodeConfirmationRequest,
                    detailedCodeEntityType: detailedCodeEntityType,
                    detailedCode: detailedCode,
                    rowVersion: rowVersion);
    }
    public DetailedCodeConfirmationRequest RejectDetailedCodeConfirmationRequestProcess(
        DetailedCodeConfirmationRequest detailedCodeConfirmationRequest,
        DetailedCodeEntityType detailedCodeEntityType,
        string detailedCode,
        byte[] rowVersion)
    {
      if (detailedCodeConfirmationRequest.Status != DetailCodeConfirmationStatus.NotAction)
      {
        throw new DetailedCodeConfirmationRequestNotFoundException(id: detailedCodeConfirmationRequest.Id);
      }
      Cooperator customer = null;
      Cooperator provider = null;
      ProductionLine producionLine = null;
      #region DisapprovalCustomerDetailedCode
      if (detailedCodeConfirmationRequest.DetailedCodeEntityType == DetailedCodeEntityType.Customer)
      {
        #region GetCustomer
        customer = App.Internals.SaleManagement.GetCustomer(
        id: (int)detailedCodeConfirmationRequest.CooperatorId);
        #endregion
        #region DisapprovalCustomerDetailedCode
        App.Internals.SaleManagement.DisapprovalCustomerDetailedCode(
        id: customer.Id,
        detailedCode: detailedCode,
        rowVersion: customer.RowVersion);
        #endregion
      }
      #endregion
      #region DisapprovalProviderDetailedCode
      if (detailedCodeConfirmationRequest.DetailedCodeEntityType == DetailedCodeEntityType.Provider)
      {
        #region GetProvider
        provider = App.Internals.SaleManagement.GetProvider(
        id: (int)detailedCodeConfirmationRequest.CooperatorId);
        #endregion
        #region DisapprovalProviderDetailedCode
        App.Internals.SaleManagement.DisapprovalProviderDetailedCode(
        id: provider.Id,
        detailedCode: detailedCode,
        rowVersion: provider.RowVersion);
        #endregion
      }
      #endregion
      #region DisapprovalOfProductionLineDetailedCode
      if (detailedCodeConfirmationRequest.DetailedCodeEntityType == DetailedCodeEntityType.ProductionLine)
      {
        #region GetProductionLine
        producionLine = App.Internals.Planning.GetProductionLine(
        id: (int)detailedCodeConfirmationRequest.ProductionLineId);
        #endregion
        #region DisapprovalOfProductionLineDetailedCode
        App.Internals.Planning.DisapprovalOfProductionLineDetailedCode(
        id: producionLine.Id,
        detailedCode: detailedCode,
        rowVersion: producionLine.RowVersion);
        #endregion
      }
      #endregion
      #region Update DetailedCodeConfirmationRequest Status
      detailedCodeConfirmationRequest = EditDetailedCodeConfirmationRequest(
              detailedCodeConfirmationRequest: detailedCodeConfirmationRequest,
              rowVersion: rowVersion,
              detailedCode: detailedCode,
              status: DetailCodeConfirmationStatus.Rejected);
      #endregion
      return detailedCodeConfirmationRequest;
    }
    #endregion
    #region EditDetailedCodeConfirmationRequest
    public DetailedCodeConfirmationRequest EditDetailedCodeConfirmationRequest(
        int id,
        byte[] rowVersion,
        string detailedCode,
        TValue<string> description = null,
        TValue<DetailCodeConfirmationStatus> status = null)
    {
      var detailedCodeConfirmationRequest = GetDetailedCodeConfirmationRequest(id: id);
      return EditDetailedCodeConfirmationRequest(
                    detailedCodeConfirmationRequest: detailedCodeConfirmationRequest,
                    rowVersion: rowVersion,
                    detailedCode: detailedCode,
                    description: description,
                    status: status);
    }
    public DetailedCodeConfirmationRequest EditDetailedCodeConfirmationRequest(
        DetailedCodeConfirmationRequest detailedCodeConfirmationRequest,
        byte[] rowVersion,
        TValue<string> detailedCode = null,
        TValue<string> description = null,
        TValue<DetailCodeConfirmationStatus> status = null)
    {
      var currentUser = App.Providers.Security.CurrentLoginData;
      detailedCodeConfirmationRequest.ConfirmationUserId = currentUser.UserId;
      detailedCodeConfirmationRequest.ConfirmationDateTime = DateTime.Now.ToUniversalTime();
      if (status != null)
        detailedCodeConfirmationRequest.Status = status;
      if (detailedCode != null)
        detailedCodeConfirmationRequest.DetailedCode = detailedCode;
      if (description != null)
        detailedCodeConfirmationRequest.Description = description;
      repository.Update(rowVersion: detailedCodeConfirmationRequest.RowVersion, entity: detailedCodeConfirmationRequest);
      return detailedCodeConfirmationRequest;
    }
    #endregion
  }
}
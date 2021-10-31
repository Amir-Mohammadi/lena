using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.UserManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.StaticData;
using lena.Models;
using lena.Models.Common;
using lena.Models.UserManagement.PermissionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    #region Get
    public PermissionRequest GetPermissionRequest(int id) => GetPermissionRequest(selector: e => e, id: id);
    public TResult GetPermissionRequest<TResult>(
        Expression<Func<PermissionRequest, TResult>> selector,
        int id)
    {

      var permissionRequest = GetPermissionRequests(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      return permissionRequest;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetPermissionRequests<TResult>(
        Expression<Func<PermissionRequest, TResult>> selector,
        TValue<int> id = null,
        TValue<DateTime> fromRegisterDateTime = null,
        TValue<DateTime> toRegisterDateTime = null,
        TValue<int> registrarUserId = null,
        TValue<int> intendedUserId = null,
        TValue<int> confirmationUserId = null
        )
    {

      var userId = GetPermissionRequestAllowedUserId();
      registrarUserId = userId;
      var baseQuery = repository.GetQuery<PermissionRequest>();
      if (id != null)
        baseQuery = baseQuery.Where(i => i.Id == id);
      if (fromRegisterDateTime != null)
        baseQuery = baseQuery.Where(i => i.RegisterDateTime >= fromRegisterDateTime);
      if (toRegisterDateTime != null)
        baseQuery = baseQuery.Where(i => i.RegisterDateTime <= toRegisterDateTime);
      if (registrarUserId != null)
        baseQuery = baseQuery.Where(i => i.RegistrarUserId == registrarUserId);
      if (intendedUserId != null)
        baseQuery = baseQuery.Where(i => i.IntendedUserId == intendedUserId);
      return baseQuery.Select(selector);
    }
    #endregion

    #region ToResult
    public IQueryable<PermissionRequestResult> ToPermissionRequestResult(
       IQueryable<PermissionRequest> permissionRequests
       )
    {


      var supplies = App.Internals.Supplies;
      var saleManagements = App.Internals.SaleManagement;

      var resultQuery = from permissionRequest in permissionRequests
                        select new PermissionRequestResult
                        {
                          Id = permissionRequest.Id,
                          RegistrarUserFullName = permissionRequest.RegistrarUser.Employee.FirstName + " " + permissionRequest.RegistrarUser.Employee.LastName,
                          IntendedUserFullName = permissionRequest.IntendedUser.Employee.FirstName + " " + permissionRequest.IntendedUser.Employee.LastName,
                          RegisterDateTime = permissionRequest.RegisterDateTime,
                          PermissionRequestActions = permissionRequest.PermissionRequestActions.AsQueryable().Select(App.Internals.UserManagement.ToPermissionRequestActionResult)
                        };
      return resultQuery;
    }
    #endregion
    #region Search
    public IQueryable<PermissionRequestResult> SearchPermissionRequest(IQueryable<PermissionRequestResult>
    query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems
    )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.IntendedUserFullName.Contains(searchText) ||
            item.RegistrarUserFullName.Contains(searchText)
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
    public IOrderedQueryable<PermissionRequestResult> SortPermissionRequestResult(IQueryable<PermissionRequestResult> query,
        SortInput<PermissionRequestSortType> sort)
    {
      switch (sort.SortType)
      {
        case PermissionRequestSortType.RegisterDateTime:
          return query.OrderBy(a => a.RegisterDateTime, sort.SortOrder);
        case PermissionRequestSortType.RegistrarUserFullName:
          return query.OrderBy(a => a.RegistrarUserFullName, sort.SortOrder);
        case PermissionRequestSortType.IntendedUserFullName:
          return query.OrderBy(a => a.IntendedUserFullName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Delete
    public void DeletePermissionRequest(int id)
    {

      var permissionRequest = GetPermissionRequest(id: id);
      repository.Delete(permissionRequest);
    }
    #endregion

    #region DeleteProcess
    public void DeletePermissionRequestProcess(int id)
    {

      var userId = App.Providers.Security.CurrentLoginData.UserId;
      var permissionRequest = GetPermissionRequest(id: id);
      if (userId != permissionRequest.RegistrarUserId)
      {
        throw new UserCantDeletePermissionRequestException();
      }
      int[] permissionRequestActionIds = permissionRequest.PermissionRequestActions.Select(s => s.Id).ToArray();
      foreach (var item in permissionRequestActionIds)
        DeletePermissionRequestAction(item);
      DeletePermissionRequest(id: id);
    }
    #endregion


    #region Add
    public PermissionRequest AddPermissionRequest(
        int? intendedUserId
        )
    {

      var permissionRequest = repository.Create<PermissionRequest>();
      permissionRequest.IntendedUserId = intendedUserId;
      permissionRequest.RegistrarUserId = App.Providers.Security.CurrentLoginData.UserId;
      permissionRequest.RegisterDateTime = DateTime.UtcNow;
      repository.Add(permissionRequest);
      return permissionRequest;
    }
    #endregion

    #region SaveUserPermissions
    public void AddPermissionRequests(
        int userId,
        IEnumerable<PermissionInput> permissionInputs)
    {

      var permissionRequest = AddPermissionRequest(
                intendedUserId: userId);
      foreach (var permissionInput in permissionInputs)
      {
        AddPermissionRequestAction(
                  permissionRequestId: permissionRequest.Id,
                  accessType: permissionInput.AccessType,
                  securityActionId: permissionInput.SecurityActionId
                  );
      }
    }
    #endregion

    #region GetPermissionRequestAllowedUserId
    public int? GetPermissionRequestAllowedUserId()
    {


      #region Check Confirm Permission And GetEmployeeId
      var checkPermissionResult = App.Internals.UserManagement.CheckPermission(
              actionName: StaticActionName.GetAllPermissionRequestList,
              actionParameters: null);

      if (checkPermissionResult.AccessType == AccessType.Denied)
      {
        int? userId = App.Providers.Security.CurrentLoginData.UserId;
        return userId;
      }
      else
      {
        return null;
      }

      #endregion

    }
    #endregion
  }
}
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.UserManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
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
    public PermissionRequestAction GetPermissionRequestAction(int id) => GetPermissionRequestAction(selector: e => e, id: id);
    public TResult GetPermissionRequestAction<TResult>(
        Expression<Func<PermissionRequestAction, TResult>> selector,
        int id)
    {

      var permissionRequestActions = GetPermissionRequestActions(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      return permissionRequestActions;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetPermissionRequestActions<TResult>(
        Expression<Func<PermissionRequestAction, TResult>> selector,
        TValue<int> id = null,
        TValue<int> permissionRequestId = null,
        TValue<AccessType> accessType = null
        )
    {

      var baseQuery = repository.GetQuery<PermissionRequestAction>();
      if (id != null)
        baseQuery = baseQuery.Where(i => i.Id == id);
      if (permissionRequestId != null)
        baseQuery = baseQuery.Where(i => i.PermissionRequestId == permissionRequestId);
      if (accessType != null)
        baseQuery = baseQuery.Where(i => i.AccessType == accessType);
      return baseQuery.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<PermissionRequestAction, PermissionRequestActionResult>> ToPermissionRequestActionResult =
        (permissionRequestAction) => new PermissionRequestActionResult
        {
          Id = permissionRequestAction.Id,
          PermissionRequestId = permissionRequestAction.PermissionRequestId,
          AccessType = permissionRequestAction.AccessType.Value,
          SecurityActionId = permissionRequestAction.SecurityAction.Id,
          SecurityActionName = permissionRequestAction.SecurityAction.Name,
          Status = permissionRequestAction.Status,
          Description = permissionRequestAction.Description,
          ConfirmationUserFullName = permissionRequestAction.ConfirmationUser.Employee.FirstName + " " + permissionRequestAction.ConfirmationUser.Employee.LastName,
        };
    #endregion

    #region Delete
    public void DeletePermissionRequestAction(int id)
    {

      var permissionRequestAction = GetPermissionRequestAction(id: id);
      if (permissionRequestAction.Status != PermissionRequestActionStatus.NotAction)
      {
        throw new PermissionRequestActionIsConfirmedException(id);
      }
      repository.Delete(permissionRequestAction);
    }
    #endregion

    #region RejectPermissionRequestAction
    public PermissionRequestAction RejectPermissionRequestAction(
        int id,
        TValue<string> description = null
        )
    {

      var permissionRequestAction = GetPermissionRequestAction(id);

      if (description != null)
        permissionRequestAction.Description = description;
      permissionRequestAction.Status = PermissionRequestActionStatus.Rejected;
      permissionRequestAction.ConfirmationUserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Update(permissionRequestAction, rowVersion: permissionRequestAction.RowVersion);
      return permissionRequestAction;
    }
    #endregion


    #region AcceptPermissionRequestActionProcess
    public PermissionRequestAction AcceptPermissionRequestActionProcess(
        int id,
        TValue<string> description = null
        )
    {

      var permissionRequestAction = GetPermissionRequestAction(id);
      var intendedUserId = permissionRequestAction.PermissionRequest.IntendedUserId;

      var permissionInputs = new List<PermissionInput>();
      PermissionInput permissionInput = new PermissionInput();
      permissionInput.AccessType = permissionRequestAction.AccessType;
      permissionInput.SecurityActionId = permissionRequestAction.SecurityAction.Id;
      permissionInputs.Add(permissionInput);
      SaveUserPermissions(
                userId: intendedUserId.Value,
                permissionInputs: permissionInputs
                );

      AcceptPermissionRequestAction(
                id: id,
                description: description
                );


      return permissionRequestAction;
    }
    #endregion

    #region AcceptPermissionRequestAction
    public PermissionRequestAction AcceptPermissionRequestAction(
        int id,
        TValue<string> description = null
        )
    {

      var permissionRequestAction = GetPermissionRequestAction(id);

      if (description != null)
        permissionRequestAction.Description = description;
      permissionRequestAction.Status = PermissionRequestActionStatus.Accepted;
      permissionRequestAction.ConfirmationUserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Update(permissionRequestAction, rowVersion: permissionRequestAction.RowVersion);
      return permissionRequestAction;
    }
    #endregion
    #region Add
    public PermissionRequestAction AddPermissionRequestAction(
        int permissionRequestId,
        AccessType? accessType,
        int securityActionId
        )
    {

      var permissionRequestAction = repository.Create<PermissionRequestAction>();
      permissionRequestAction.PermissionRequestId = permissionRequestId;
      if (accessType != null)
        permissionRequestAction.AccessType = accessType.Value;
      permissionRequestAction.SecurityActionId = securityActionId;
      repository.Add(permissionRequestAction);
      return permissionRequestAction;
    }
    #endregion
  }
}
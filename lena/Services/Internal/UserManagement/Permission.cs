using System;
using System.Collections.Generic;
using System.Linq;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.UserManagement.Permission;
using lena.Models.UserManagement.SecurityAction;
using lena.Models.UserManagement.User;
using lena.Models.Common;
using lena.Services.Common;
using lena.Services.Internals.UserManagement.Exception;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    #region Gets
    public IQueryable<Permission> GetPermissions(
        TValue<int> id = null,
        TValue<int> securityActionId = null,
        TValue<int> securityActionGroupId = null,
        TValue<int?> userGroupId = null,
        TValue<int?> userId = null,
        TValue<int?> employeeId = null,
        TValue<AccessType> accessType = null,
        TValue<int[]> userGroupIds = null,
        TValue<int[]> userIds = null,
        TValue<int[]> securityActionIds = null)
    {

      var query = repository.GetQuery<Permission>();

      if (id != null)
        query = query.Where(i => i.Id == id);
      if (securityActionId != null)
        query = query.Where(i => i.SecurityActionId == securityActionId);
      if (securityActionGroupId != null)
        query = query.Where(i => i.SecurityAction.SecurityActionGroupId == securityActionGroupId);
      if (userGroupId != null)
        query = query.Where(i => i.UserGroupId == userGroupId);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (employeeId != null)
        query = query.Where(i => i.User.Employee.Id == employeeId);
      if (accessType != null)
        query = query.Where(i => i.AccessType == accessType);
      if (userGroupIds != null)
        query = query.Where(i => userGroupIds.Value.Contains(i.UserGroupId.Value));
      if (userIds != null)
        query = query.Where(i => userIds.Value.Contains(i.UserId.Value));
      if (securityActionIds != null)
        query = query.Where(i => securityActionIds.Value.Contains(i.SecurityActionId));

      return query;
    }
    #endregion
    #region Get
    public Permission GetPermission(int id)
    {

      var permission = GetPermissions(id: id).SingleOrDefault();
      if (permission == null)
        throw new PermissoinNotFoundException(id);
      return permission;
    }
    #endregion
    #region Add
    public Permission AddPermission(int securityActionId,
        int? userGroupId,
        int? userId,
        AccessType accessType)
    {

      var permission = repository.Create<Permission>();
      permission.SecurityActionId = securityActionId;
      permission.UserId = userId;
      permission.UserGroupId = userGroupId;
      permission.AccessType = accessType;
      repository.Add(permission);
      return permission;
    }
    #endregion
    #region Edit
    public Permission EditPermission(
        int id,
        byte[] rowVersion,
        TValue<int> securityActionId = null,
        TValue<int?> userGroupId = null,
        TValue<int?> userId = null,
        TValue<AccessType> accessType = null)
    {

      var permission = GetPermission(id: id);
      EditPermission(
                    permission: permission,
                    rowVersion: rowVersion,
                    securityActionId: securityActionId,
                    userGroupId: userGroupId,
                    userId: userId,
                    accessType: accessType);
      return permission;
    }
    public Permission EditPermission(
        Permission permission,
        byte[] rowVersion,
        TValue<int> securityActionId = null,
        TValue<int?> userGroupId = null,
        TValue<int?> userId = null,
        TValue<AccessType> accessType = null)
    {

      if (securityActionId != null)
        permission.SecurityActionId = securityActionId;
      if (userGroupId != null)
        permission.UserGroupId = userGroupId;
      if (userId != null)
        permission.UserId = userId;
      if (accessType != null)
        permission.AccessType = accessType;
      repository.Update(permission, rowVersion: rowVersion);
      return permission;
    }
    #endregion
    #region Delete
    public void DeletePermission(int id)
    {

      var permission = GetPermission(id: id);
      DeletePermission(permission);
    }
    public void DeletePermission(Permission permission)
    {

      repository.Delete(permission);
    }
    #endregion
    #region AllowPermission
    public Permission AllowPermission(byte[] rowVersion, int id)
    {
      return EditPermission(rowVersion: rowVersion, id: id, accessType: AccessType.Allowed);
    }
    #endregion
    #region DenyPermission
    public Permission DenyPermission(byte[] rowVersion, int id)
    {
      return EditPermission(rowVersion: rowVersion, id: id, accessType: AccessType.Denied);
    }
    #endregion
    #region SaveUserPermissions
    public void SaveUserPermissions(
        int userId,
        IEnumerable<PermissionInput> permissionInputs)
    {

      var permissions = GetPermissions(userId: userId)


                .ToList();
      foreach (var permissionInput in permissionInputs)
      {
        var currentPermissions = permissions.Where(i => i.SecurityActionId == permissionInput.SecurityActionId);
        foreach (var permission in currentPermissions)
          repository.Delete(permission);
        if (permissionInput.AccessType != null)
        {
          AddPermission(
                        securityActionId: permissionInput.SecurityActionId,
                        userId: userId,
                        userGroupId: null,
                        accessType: permissionInput.AccessType.Value);
        }
      }
    }
    #endregion
    #region SaveUserGroupPermissions
    public void SaveUserGroupPermissions(
        int userGroupId,
        IEnumerable<PermissionInput> permissionInputs)
    {

      var permissions = GetPermissions(userGroupId: userGroupId)


                .ToList();

      foreach (var permissionInput in permissionInputs)
      {
        var currentPermissions = permissions.Where(i => i.SecurityActionId == permissionInput.SecurityActionId);
        foreach (var permission in currentPermissions)
          repository.Delete(permission);
        if (permissionInput.AccessType != null)
        {
          AddPermission(
                           securityActionId: permissionInput.SecurityActionId,
                           userId: null,
                           userGroupId: userGroupId,
                           accessType: permissionInput.AccessType.Value);

        }

      }
    }
    #endregion
    #region ToSecurityActionWithPermissionResults
    public IQueryable<SecurityActionWithPermissionResult> ToSecurityActionWithPermissionResults(
        IQueryable<Permission> permissions,
        IQueryable<SecurityAction> securityActions,
        IQueryable<int> defaultPermissionSecurityActionIds)
    {
      var data = from securityAction in securityActions
                 join tp in permissions on securityAction.Id equals tp.SecurityActionId into tempPermissions
                 from permission in tempPermissions.DefaultIfEmpty()
                 join ta in defaultPermissionSecurityActionIds on securityAction.Id equals ta into tempDefaultPermissionSecurityActionIds
                 from defaultPermissionSecurityActionId in tempDefaultPermissionSecurityActionIds.DefaultIfEmpty()
                 select new SecurityActionWithPermissionResult()
                 {
                   SecurityActionId = securityAction.Id,
                   SecurityActionName = securityAction.Name,
                   SecurityActionGroupId = securityAction.SecurityActionGroupId,
                   DefaultAccessType = defaultPermissionSecurityActionId > 0 ? AccessType.Allowed : AccessType.Denied,
                   //DefaultAccessType = AccessType.Allowed ,
                   AccessType = permission.AccessType
                 };
      return data;
    }
    #endregion
    #region ToSecurityActionPermissionResults
    public IQueryable<SecurityActionPermissionResult> ToSecurityActionPermissionResults(
        IQueryable<Permission> permissions)
    {
      var data = from permission in permissions
                 select new SecurityActionPermissionResult()
                 {
                   Id = permission.Id,
                   SecurityActionId = permission.SecurityActionId,
                   SecurityActionName = permission.SecurityAction.Name,
                   SecurityActionGroupId = permission.SecurityAction.SecurityActionGroupId,
                   SecurityActionGroupName = permission.SecurityAction.SecurityActionGroup.Name,
                   UserId = permission.UserId,
                   UserName = permission.User.UserName,
                   UserGroupId = permission.UserGroupId,
                   UserGroupName = permission.UserGroup.Name,
                   EmployeeId = permission.User.Employee.Id,
                   EmployeeCode = permission.User.Employee.Code,
                   EmployeeFullName = permission.User.Employee.FirstName + " " + permission.User.Employee.LastName,
                   AccessType = permission.AccessType,
                   RowVersion = permission.RowVersion
                 };
      return data;
    }
    #endregion
    #region Sort
    public IQueryable<SecurityActionPermissionResult> SortSecurityActionPermissionResult(
        IQueryable<SecurityActionPermissionResult> query,
        SortInput<SecurityActionPermissionSortType> sort)
    {
      switch (sort.SortType)
      {
        case SecurityActionPermissionSortType.AccessType:
          query = query.OrderBy(x => x.AccessType, sort.SortOrder);
          break;
        case SecurityActionPermissionSortType.UserName:
          query = query.OrderBy(x => x.UserName, sort.SortOrder);
          break;
        case SecurityActionPermissionSortType.UserGroupName:
          query = query.OrderBy(x => x.UserGroupName, sort.SortOrder);
          break;
        case SecurityActionPermissionSortType.SecurityActionGroupName:
          query = query.OrderBy(x => x.SecurityActionGroupName, sort.SortOrder);
          break;
        case SecurityActionPermissionSortType.SecurityActionName:
          query = query.OrderBy(x => x.SecurityActionName, sort.SortOrder);
          break;
        case SecurityActionPermissionSortType.SecurityActionId:
          query = query.OrderBy(x => x.SecurityActionId, sort.SortOrder);
          break;
        case SecurityActionPermissionSortType.EmployeeCode:
          query = query.OrderBy(x => x.EmployeeCode, sort.SortOrder);
          break;
        case SecurityActionPermissionSortType.EmployeeFullName:
          query = query.OrderBy(x => x.EmployeeFullName, sort.SortOrder);
          break;
        default:
          query = query.OrderBy(x => x.SecurityActionId, sort.SortOrder);
          break;
      }
      return query;
    }
    #endregion
    #region CheckPermission
    public CheckPermissionResult CheckPermission(
        string actionName,
        ActionParameterInput[] actionParameters,
        bool isExternalRequest = false)
    {

      var resultAccessType = AccessType.Denied;
      var session = App.Providers.Session;
      var userId = session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString().KeyPrefix(session.StateKey))?.UserId;
      if (userId == null)
        throw new UserNotLoginException();
      var secutityAction = FindMathSecurityAction(
                    actionName: actionName,
                    actionParameters: actionParameters);

      if (secutityAction != null)
      {
        if (isExternalRequest)
        {
          var user = App.Api.UserManagement.GetUser.Run(App.Providers.Security.CurrentLoginData.UserId);
          if (!user.Data.CanAccessFromInternet)
            throw new UserNotHasAccessToActionFromExternalException(userId: userId.Value, actionName = secutityAction.ActionName, actionDisplayName: secutityAction.Name);

          if (!secutityAction.IsPublicAction)
          {
            return new CheckPermissionResult
            {
              SecurityActionId = secutityAction?.Id,
              SecurityActionName = secutityAction?.Name,
              SecurityActionAddress = secutityAction?.ActionName,
              AccessType = AccessType.Denied
            };
          }
        }

        if (!userId.HasValue)
        {
          return new CheckPermissionResult
          {
            SecurityActionId = secutityAction?.Id,
            SecurityActionName = secutityAction?.Name,
            SecurityActionAddress = secutityAction?.ActionName,
            AccessType = resultAccessType
          };
        }

        var userPermission =
                  GetPermissions(userId: userId, securityActionId: secutityAction.Id)

                      .FirstOrDefault();
        if (userPermission != null)
          resultAccessType = userPermission.AccessType;
        else
        {
          var groupIds = GetMemberships(e => e, userId: userId)


                    .Select(i => i.UserGroupId)
                    .ToArray();
          var groupPermissions = GetPermissions(
                        userGroupIds: groupIds,
                        securityActionId: secutityAction.Id,
                        accessType: AccessType.Allowed);

          if (groupPermissions.Any())
            resultAccessType = AccessType.Allowed;
        }
      }
      else
      {
        resultAccessType = AccessType.Allowed;
      }

      return new CheckPermissionResult
      {
        SecurityActionId = secutityAction?.Id,
        SecurityActionName = secutityAction?.Name,
        SecurityActionAddress = secutityAction?.ActionName,
        AccessType = resultAccessType
      };
    }
    #endregion
    #region GetUserMenuPermissions
    public IQueryable<UserMenuPermissionResult> GetUserMenuPermissions(
        GetUserMenuPermissionInput[] getUserMenuPermissions)
    {


      #region Get Current UserId
      var userId = App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString())?.UserId;
      if (userId == null)
        throw new UserNotLoginException();
      #endregion
      #region GetSecurityActionIds
      var urls = getUserMenuPermissions.Select(i => i.Url).ToArray();
      var secutityActionIds = GetSecurityActions(actionNames: urls)


                .Select(i => i.Id)
                .ToArray();
      #endregion
      #region GetUserPermissions
      var userPermissions = GetPermissions(
          userId: userId,
          securityActionIds: secutityActionIds)


      .Select(i => new { i.SecurityAction.ActionName, i.AccessType })
      .ToList();
      #endregion
      #region Get Users UserGroupIds

      var groupIds = GetMemberships(e => e, userId: userId)


      .Select(i => i.UserGroupId)
      .ToArray();

      #endregion
      #region Get GroupPermissions
      var groupPermissions = GetPermissions(
          userGroupIds: groupIds,
          securityActionIds: secutityActionIds,
          accessType: AccessType.Allowed)


      .Select(i => new { i.SecurityAction.ActionName, i.AccessType })
      .ToList();
      #endregion
      #region CreateResult
      var result = from item in getUserMenuPermissions
                   let userPermission = userPermissions.FirstOrDefault(i => i.ActionName == item.Url)
                   let userGroupPermission = groupPermissions.Any(i => i.ActionName == item.Url)
                   let accessType = userPermission?.AccessType ??
                                          (userGroupPermission == true ? AccessType.Allowed : AccessType.Denied)
                   select new UserMenuPermissionResult()

                   {
                     Path = item.Path,
                     Url = item.Url,
                     AccessType = accessType

                   };
      #endregion
      return result.AsQueryable();
    }
    #endregion
    #region FindMathSecurityAction
    public SecurityAction FindMathSecurityAction(
        string actionName,
        ActionParameterInput[] actionParameters)
    {



      var existSecurityActions = GetSecurityActions(actionName: actionName)


                .ToList();



      if (!existSecurityActions.Any()) return null;
      var securityActions = from item in existSecurityActions
                            group item by item.Id into gItems
                            select new
                            {
                              SecurityActionId = gItems.Key,
                              ActionParamaters = gItems.SelectMany(i => i.ActionParamaters),
                              ActionParamatersCount = gItems.Count()
                            };
      Func<ActionParameter, bool> func = actionParameter =>
                actionParameters.Any(i => (i.Key == actionParameter.ParameterKey) &&
                                          (
                                              !actionParameter.CheckParameterValue ||
                                              i.Value == actionParameter.ParameterValue
                                          ));
      securityActions = securityActions.Where(i => i.ActionParamaters.All(func)).OrderByDescending(i => i.ActionParamaters.Count())
                    .ToList();

      var securityAction = securityActions.FirstOrDefault();
      if (securityAction == null)
        return null;
      else
        return GetSecurityAction(id: securityAction.SecurityActionId);
    }
    #endregion
    #region Search
    public IQueryable<SecurityActionPermissionResult> SearchSecurityActionPermission(
        IQueryable<SecurityActionPermissionResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where
                item.SecurityActionGroupId.ToString().Contains(searchText) ||
                item.SecurityActionName.Contains(searchText) ||
                item.SecurityActionGroupName.Contains(searchText) ||
                item.UserName.Contains(searchText) ||
                item.EmployeeFullName.Contains(searchText) ||
                item.EmployeeCode.Contains(searchText) ||
                item.UserGroupName.Contains(searchText)
                select item;

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }

    #endregion
  }
}

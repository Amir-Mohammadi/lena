using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.UserManagement.SecurityAction;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    public IQueryable<SecurityAction> GetSecurityActions(
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> actionName = null,
        TValue<string> searchText = null,
        TValue<string[]> actionNames = null,
        TValue<int> securityActionGroupId = null,
        SecurityActionSortType securityActionSortType = SecurityActionSortType.Id,
        SortOrder sortOrder = SortOrder.Unspecified)
    {
      var securityActions = repository.GetQuery<SecurityAction>();
      if (id != null)
        securityActions = securityActions.Where(i => i.Id == id);
      if (name != null)
        securityActions = securityActions.Where(i => i.Name == name);
      if (actionName != null)
        securityActions = securityActions.Where(i => i.ActionName == actionName);
      if (actionNames != null)
        securityActions = securityActions.Where(i => actionNames.Value.Contains(i.ActionName));
      if (searchText != null)
        securityActions = from securityAction in securityActions
                          where securityAction.ActionName.Contains(searchText) ||
                                      securityAction.Name.Contains(searchText)
                          select securityAction;
      if (securityActionGroupId != null)
        securityActions = securityActions.Where(i => i.SecurityActionGroupId == securityActionGroupId);
      Expression<Func<SecurityAction, string>> sortFunction = null;
      switch (securityActionSortType)
      {
        case SecurityActionSortType.Id:
          sortFunction = action => action.Id.ToString();
          break;
        case SecurityActionSortType.Name:
          sortFunction = action => action.Name;
          break;
        case SecurityActionSortType.ActionName:
          sortFunction = action => action.ActionName;
          break;
        case SecurityActionSortType.SecurityActionGroupName:
          sortFunction = action => action.SecurityActionGroup.Name;
          break;
      }
      switch (sortOrder)
      {
        case SortOrder.Unspecified:
        case SortOrder.Ascending:
          securityActions = securityActions.OrderBy(sortFunction);
          break;
        case SortOrder.Descending:
          securityActions = securityActions.OrderByDescending(sortFunction);
          break;
      }
      return securityActions;
    }
    public SecurityAction GetSecurityAction(int id)
    {
      var data = GetSecurityActions(id: id).SingleOrDefault();
      if (data == null)
        throw new SecurityActionNotFoundException();
      return data;
    }
    public SecurityAction AddSecurityAction(string name, string actionName, int? securityActionGroupId = null)
    {
      var data = repository.Create<SecurityAction>();
      data.Name = name;
      data.ActionName = actionName;
      if (securityActionGroupId.HasValue)
        data.SecurityActionGroupId = securityActionGroupId.Value;
      repository.Add(data);
      return data;
    }
    /// <summary>
    /// Check and insert security actions if not exsit in db
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public void AddBulkSecurityActions(AddBulkSecurityActionInput[] input)
    {
      var actions = GetSecurityActions();
      var actionResult = (from item in actions
                          select new
                          {
                            Id = item.Id,
                            GroupId = item.SecurityActionGroupId,
                            GroupName = item.SecurityActionGroup.Name,
                            Name = item.Name,
                            ActionName = item.ActionName
                          }).ToList();
      var groupedInputs = input.Where(i => actionResult.All(a => a.ActionName != i.Name))
                .ToList()
                .GroupBy(x => x.GroupName)
                .ToList();
      foreach (var item in groupedInputs)
      {
        var actionGroup = GetSecurityActionGroups(selector: e => e, name: item.Key)
                  .FirstOrDefault();
        if (actionGroup == null)
        {
          actionGroup = AddSecurityActionGroup(item.Key, displayName: item.Key);
        }
        foreach (var i in item.ToList())
        {
          var action = GetSecurityActions(actionName: i.ActionName)
                    .FirstOrDefault();
          if (action == null)
            AddSecurityAction(
                         name: i.Name,
                         actionName: i.ActionName,
                         securityActionGroupId: actionGroup.Id);
        }
      }
    }
    public SecurityAction EditSecurityAction(byte[] rowVersion, int id, TValue<string> name = null, TValue<string> actionName = null, TValue<int> securityActionGroupId = null)
    {
      var data = GetSecurityAction(id: id);
      if (name != null)
        data.Name = name;
      if (actionName != null)
        data.ActionName = actionName;
      if (securityActionGroupId != null)
        data.SecurityActionGroupId = securityActionGroupId;
      repository.Update(data, rowVersion: rowVersion);
      return data;
    }
    public SecurityAction EditSecurityActionProcess(
      byte[] rowVersion,
      int id,
      AddActionParameterInput[] addActionParameterInputs = null,
      EditActionParameterInput[] editActionParameterInputs = null,
      int[] deleteActionParameters = null,
      TValue<string> name = null,
      TValue<string> actionName = null,
      TValue<int> securityActionGroupId = null)
    {
      var securityAction = EditSecurityAction(
                    rowVersion: rowVersion,
                    id: id,
                    name: name,
                    actionName: actionName,
                    securityActionGroupId: securityActionGroupId);
      #region AddActionParameterInput
      if (addActionParameterInputs != null)
      {
        foreach (var actionParameterInput in addActionParameterInputs)
        {
          var newActionParameter = AddActionParameter(
                    parameterKey: actionParameterInput.Key,
                    parameterValue: actionParameterInput.Value,
                    checkParameterValue: actionParameterInput.CheckParameterValue,
                    securityActionId: securityAction.Id);
        }
      }
      #endregion
      #region EditActionParameterInput
      if (editActionParameterInputs != null)
        foreach (var editActionParameterInput in editActionParameterInputs)
        {
          var editaActionParameter = EditActionParameter(
                        rowVersion: editActionParameterInput.RowVersion,
                        id: editActionParameterInput.Id,
                        parameterValue: editActionParameterInput.Value,
                        parameterKey: editActionParameterInput.Key,
                        checkParameterValue: editActionParameterInput.CheckParameterValue);
        }
      #endregion
      #region DeleteActionParameters
      if (deleteActionParameters != null)
      {
        foreach (var actionParameterId in deleteActionParameters)
        {
          DeleteActionParameter(id: actionParameterId);
        }
      }
      #endregion
      return securityAction;
    }
    public SecurityAction AddSecurityActionProcess(
        AddActionParameterInput[] addActionParameterInputs,
        string name,
        string actionName,
        int? securityActionGroupId = null)
    {
      var securityAction = AddSecurityAction(name, actionName, securityActionGroupId);
      #region AddActionParameterInput
      foreach (var actionParameterInput in addActionParameterInputs)
      {
        var newActionParameter = AddActionParameter(parameterKey: actionParameterInput.Key,
                      parameterValue: actionParameterInput.Value,
                      checkParameterValue: actionParameterInput.CheckParameterValue,
                      securityActionId: securityAction.Id);
      }
      #endregion
      return securityAction;
    }
    public void DeleteSecurityAction(int id)
    {
      var data = GetSecurityAction(id: id);
      #region Delete Permissions
      var permissions = GetPermissions(securityActionId: id);
      foreach (var permission in permissions)
      {
        DeletePermission(id: permission.Id);
      }
      #endregion
      #region Delete Action Parameters
      var actionParameters = GetActionParameters(securityActionId: id);
      foreach (var actionParameter in actionParameters)
      {
        DeleteActionParameter(actionParameter: actionParameter);
      }
      #endregion
      repository.Delete(data);
    }
    public IQueryable<SecurityActionResult> ToSecurityActionResults(IQueryable<SecurityAction> query)
    {
      var data = from securityAction in query
                 select new SecurityActionResult()
                 {
                   Id = securityAction.Id,
                   Name = securityAction.Name,
                   ActionName = securityAction.ActionName,
                   SecurityActionGroupId = securityAction.SecurityActionGroupId,
                   SecurityActionGroupName = securityAction.SecurityActionGroup.Name,
                   SecurityActionGroupDisplayName = securityAction.SecurityActionGroup.DisplayName
                 };
      return data;
    }
    public SecurityActionResult ToSecurityActionResult(SecurityAction securityAction)
    {
      var data = new SecurityActionResult()
      {
        Id = securityAction.Id,
        Name = securityAction.Name,
        ActionName = securityAction.ActionName,
        SecurityActionGroupId = securityAction.SecurityActionGroupId,
        SecurityActionGroupName = securityAction.SecurityActionGroup.Name,
        SecurityActionGroupDisplayName = securityAction.SecurityActionGroup.DisplayName,
        ActionParameters = securityAction.ActionParamaters.AsQueryable().Select(ToActionParameterResult).ToArray(),
        RowVersion = securityAction.RowVersion,
      };
      return data;
    }
    public IQueryable<SecurityActionResult> SearchSecurityAction(
        IQueryable<SecurityActionResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.Name.Contains(searchText) ||
                item.ActionName.Contains(searchText) ||
                item.SecurityActionGroupName.Contains(searchText) ||
                item.SecurityActionGroupDisplayName.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
  }
}
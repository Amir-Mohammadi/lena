using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Internals.Exceptions;
//using Parlar.DAL;
//using Parlar.DAL.UnitOfWorks;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.UserManagement.SecurityAction;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    public IQueryable<ActionParameter> GetActionParameters(TValue<int> id = null,
        TValue<string> parameterKey = null,
        TValue<bool> checkParameterValue = null,
        TValue<string> parameterValue = null,
        TValue<string> actionName = null,
        TValue<string> searchText = null,
        TValue<int> securityActionId = null,
        ActionParameterSortType actionParameterSortType = ActionParameterSortType.Key,
        SortOrder sortOrder = SortOrder.Unspecified)
    {
      var isIdNull = id == null;
      var isParameterKeyNull = parameterKey == null;
      var isCheckParameterValueNull = checkParameterValue == null;
      var isParameterValueNull = parameterValue == null;
      var isSecurityActionIdNull = securityActionId == null;
      var isActionNameNull = actionName == null;
      var iSearchTextNull = searchText == null;
      var actionParameters = from actionParameter in repository.GetQuery<ActionParameter>()
                             where
                                       (isIdNull || actionParameter.Id == id) &&
                                       (isParameterKeyNull || actionParameter.ParameterKey == parameterKey) &&
                                       (isCheckParameterValueNull || actionParameter.CheckParameterValue == checkParameterValue) &&
                                       (isParameterValueNull || actionParameter.ParameterValue == parameterValue) &&
                                       (isSecurityActionIdNull || actionParameter.SecurityActionId == securityActionId) &&
                                       (isActionNameNull || actionParameter.SecurityAction.ActionName == actionName) &&
                                       (iSearchTextNull || actionParameter.ParameterKey.Contains(searchText) ||
                                        actionParameter.ParameterValue.Contains(searchText))
                             select actionParameter;
      Expression<Func<ActionParameter, object>> sortFunction = null;
      switch (actionParameterSortType)
      {
        case ActionParameterSortType.Key:
          sortFunction = action => action.ParameterKey;
          break;
        case ActionParameterSortType.Value:
          sortFunction = action => action.ParameterValue;
          break;
        case ActionParameterSortType.CheckParameterValue:
          sortFunction = action => action.CheckParameterValue;
          break;
      }
      switch (sortOrder)
      {
        case SortOrder.Unspecified:
        case SortOrder.Ascending:
          actionParameters = actionParameters.OrderBy(sortFunction);
          break;
        case SortOrder.Descending:
          actionParameters = actionParameters.OrderByDescending(sortFunction);
          break;
      }
      return actionParameters;
    }
    public ActionParameter GetActionParameter(int id)
    {
      var data = GetActionParameters(id: id).SingleOrDefault();
      if (data == null)
        throw new ActionParameterNotFoundException();
      return data;
    }
    public ActionParameter EditActionParameter(
      byte[] rowVersion,
      int id,
      TValue<int> securityActionId = null,
      TValue<string> parameterKey = null,
      TValue<string> parameterValue = null,
      TValue<bool> checkParameterValue = null)
    {
      var actionParameters = GetActionParameter(id: id);
      if (actionParameters == null)
        throw new ActionParameterNotFoundException();
      if (securityActionId != null)
        actionParameters.SecurityActionId = securityActionId;
      if (parameterKey != null)
        actionParameters.ParameterKey = parameterKey;
      if (parameterValue != null)
        actionParameters.ParameterValue = parameterValue;
      if (checkParameterValue != null)
        actionParameters.CheckParameterValue = checkParameterValue;
      repository.Update(actionParameters, rowVersion: rowVersion);
      return actionParameters;
    }
    public ActionParameter AddActionParameter(int securityActionId, string parameterKey, string parameterValue, bool? checkParameterValue = null)
    {
      var data = repository.Create<ActionParameter>();
      data.SecurityActionId = securityActionId;
      data.ParameterKey = parameterKey;
      data.ParameterValue = parameterValue;
      if (checkParameterValue.HasValue)
        data.CheckParameterValue = checkParameterValue.Value;
      repository.Add(data);
      return data;
    }
    public void DeleteActionParameter(int id)
    {
      var data = GetActionParameter(id: id);
      repository.Delete(data);
    }
    public void DeleteActionParameter(ActionParameter actionParameter)
    {
      repository.Delete(actionParameter);
    }
    public Expression<Func<ActionParameter, ActionParameterResult>> ToActionParameterResult =>
        actionParameter => new ActionParameterResult()
        {
          Id = actionParameter.Id,
          Key = actionParameter.ParameterKey,
          Value = actionParameter.ParameterValue,
          CheckParameterValue = actionParameter.CheckParameterValue,
          SecurityActionId = actionParameter.SecurityActionId,
          SecurityActionName = actionParameter.SecurityAction.Name,
          RowVersion = actionParameter.RowVersion
        };
  }
}
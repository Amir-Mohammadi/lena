using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.UserManagement.SecurityActionGroup;
using lena.Services.Common;
using lena.Services.Core;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    public IQueryable<TResult> GetSecurityActionGroups<TResult>(Expression<Func<SecurityActionGroup, TResult>> selector, TValue<int> id = null,
        TValue<string> name = null,
        SecurityActionGroupSortType securityActionGroupSortType = SecurityActionGroupSortType.Id,
        SortOrder sortOrder = SortOrder.Unspecified, TValue<string> searchText = null)
    {
      var isIdNull = (id == null);
      var isNameNull = (name == null);
      var securityActionGroups = from securityActionGroup in repository.GetQuery<SecurityActionGroup>()
                                 where (isIdNull || securityActionGroup.Id == id) &&
                                             (isNameNull || securityActionGroup.Name == name)
                                 select securityActionGroup;
      if (searchText != null)
        securityActionGroups = from securityActionGroup in securityActionGroups
                               where securityActionGroup.Name.Contains(searchText) ||
                                           securityActionGroup.Name.Contains(searchText)
                               select securityActionGroup;
      Expression<Func<SecurityActionGroup, string>> sortFunction = null;
      switch (securityActionGroupSortType)
      {
        case SecurityActionGroupSortType.Name:
          sortFunction = group => group.Name;
          break;
        case SecurityActionGroupSortType.Id:
          sortFunction = group => group.Id.ToString();
          break;
      }
      switch (sortOrder)
      {
        case SortOrder.Unspecified:
        case SortOrder.Ascending:
          securityActionGroups = securityActionGroups.OrderBy(sortFunction);
          break;
        case SortOrder.Descending:
          securityActionGroups = securityActionGroups.OrderByDescending(sortFunction);
          break;
      }
      //return securityActionGroups;
      return securityActionGroups.Select(selector);
    }
    public IQueryable<TResult> GetUserSecurityActionGroups<TResult>(
        Expression<Func<UserSecurityActionGroupResult, TResult>> selector,
        TValue<int> securityActionId = null,
        TValue<int> userId = null
      )
    {
      var userMemberships = App.Internals.UserManagement.GetMemberships(e => e, userId: userId);
      var userGroupPermissionSecurityActions = (from item in userMemberships
                                                from permission in item.UserGroup.Permissions
                                                where permission.SecurityActionId == securityActionId
                                                select new UserSecurityActionGroupResult
                                                {
                                                  GroupName = permission.UserGroup.Name,
                                                  GroupId = permission.UserGroupId,
                                                  AccessType = permission.AccessType
                                                });
      return userGroupPermissionSecurityActions.Select(selector);
    }
    public SecurityActionGroup GetSecurityActionGroup(int id)
    {
      var data = GetSecurityActionGroups(selector: e => e, id: id).SingleOrDefault();
      if (data == null)
        throw new SecurityActionNotFoundException();
      return data;
    }
    public SecurityActionGroup AddSecurityActionGroup(string name, string displayName)
    {
      var data = repository.Create<SecurityActionGroup>();
      data.Name = name;
      data.DisplayName = displayName;
      repository.Add(data);
      return data;
    }
    public SecurityActionGroup EditSecurityActionGroup(byte[] rowVersion, int id, TValue<string> name = null, TValue<string> displayName = null)
    {
      var data = GetSecurityActionGroup(id: id);
      if (name != null)
        data.Name = name;
      if (displayName != null)
        data.DisplayName = displayName;
      repository.Update(data, rowVersion: rowVersion);
      return data;
    }
    public void DeleteSecurityActionGroup(int id)
    {
      var data = GetSecurityActionGroup(id: id);
      repository.Delete(data);
    }
    public IQueryable<SecurityActionGroupResult> ToSecurityActionGroupResults(IQueryable<SecurityActionGroup> query)
    {
      var data = from securityActionGroup in query
                 select new SecurityActionGroupResult()
                 {
                   Id = securityActionGroup.Id,
                   Name = securityActionGroup.Name,
                   DisplayName = securityActionGroup.DisplayName,
                   RowVersion = securityActionGroup.RowVersion
                 };
      return data;
    }
    public SecurityActionGroupResult ToSecurityActionGroupResult(SecurityActionGroup securityAction)
    {
      var data = new SecurityActionGroupResult()
      {
        Id = securityAction.Id,
        Name = securityAction.Name,
        DisplayName = securityAction.DisplayName,
        RowVersion = securityAction.RowVersion
      };
      return data;
    }
    public Expression<Func<SecurityActionGroup, SecurityActionGroupComboResult>> ToSecurityActionGroupComboResult =
        securityActionGroup => new SecurityActionGroupComboResult()
        {
          Id = securityActionGroup.Id,
          Name = securityActionGroup.Name,
          DisplayName = securityActionGroup.DisplayName
        };
    public IOrderedQueryable<SecurityActionGroupComboResult> SortSecurityActionGroupComboResult(
        IQueryable<SecurityActionGroupComboResult> query, SortInput<SecurityActionGroupSortType> type)
    {
      switch (type.SortType)
      {
        case SecurityActionGroupSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case SecurityActionGroupSortType.Name:
          return query.OrderBy(a => a.Name, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<SecurityActionGroupResult> SearchSecurityActionGroup(
       IQueryable<SecurityActionGroupResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.Name.Contains(searchText) ||
                item.DisplayName.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
  }
}
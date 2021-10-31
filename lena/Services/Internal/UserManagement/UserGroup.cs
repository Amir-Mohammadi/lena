using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.UserManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.UserManagement.UserGroup;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    public IQueryable<UserGroup> GetUserGroups(
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> name = null,
        TValue<string> description = null,
        UserGroupSortType userGroupSortType = UserGroupSortType.Name,
        SortOrder sortOrder = SortOrder.Unspecified,
        string searchText = null)
    {

      var query = repository.GetQuery<UserGroup>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (ids != null)
        query = query.Where(i => ids.Value.Contains(i.Id));
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (description != null)
        query = query.Where(i => i.Description == description);


      if (searchText != null)
        query = query.Where(user => user.Name.Contains(searchText));
      Expression<Func<UserGroup, object>> sortFunction = null;
      switch (userGroupSortType)
      {
        case UserGroupSortType.Name:
          sortFunction = group => group.Name;
          break;
      }
      switch (sortOrder)
      {
        case SortOrder.Unspecified:
        case SortOrder.Ascending:
          query = query.OrderBy(sortFunction);
          break;
        case SortOrder.Descending:
          query = query.OrderByDescending(sortFunction);
          break;
      }
      return query;
    }
    public UserGroup GetUserGroup(int id)
    {

      var userGroup = GetUserGroups(id: id).SingleOrDefault();
      if (userGroup == null)
        throw new UserGroupNotFoundException(id);
      return userGroup;
    }
    public UserGroup AddUserGroup(
        string name,
        string description)
    {

      var userGroup = repository.Create<UserGroup>();
      userGroup.Name = name;
      userGroup.Description = description;
      repository.Add(userGroup);
      return userGroup;
    }
    public UserGroup EditUserGroup(
        byte[] rowVersion,
        int id,
        TValue<string> name = null,
        TValue<string> description = null)
    {

      var userGroup = GetUserGroup(id);

      if (name != null)
        userGroup.Name = name;
      if (description != null)
        userGroup.Description = description;

      repository.Update(userGroup, rowVersion: rowVersion);
      return userGroup;
    }

    public UserGroup EditUserGroup(
       byte[] rowVersion,
       UserGroup userGroup,
       TValue<string> name = null,
       TValue<OrganizationPost> organizationPost = null,
       TValue<string> description = null)
    {

      if (name != null)
        userGroup.Name = name;
      if (organizationPost != null)
        userGroup.OrganizationPost = organizationPost;
      if (description != null)
        userGroup.Description = description;
      repository.Update(userGroup, rowVersion: rowVersion);
      return userGroup;
    }

    public void DeleteUserGroup(int id)
    {

      var group = repository.GetQuery<UserGroup>().SingleOrDefault(i => i.Id == id);
      if (group == null)
        throw new UserGroupDeleteNotFoundException(id);
      if (group.Memberships.Any())
        throw new UserGroupDeleteFailedRemoveMembershipException(id);
      if (group.Permissions.Any())
        throw new UserGroupDeleteFailedRemovePermissionException(id);
      repository.Delete(group);
    }
    public IQueryable<UserGroupResult> ToUserGroupResultQuery(IQueryable<UserGroup> query)
    {
      var resultQuery = from i in query
                        select new UserGroupResult()
                        {
                          Id = i.Id,
                          Name = i.Name,
                          Description = i.Description
                        };
      return resultQuery;
    }
    public UserGroupResult ToUserGroupResult(UserGroup userGroup)
    {
      var result = new UserGroupResult()
      {
        Id = userGroup.Id,
        Name = userGroup.Name,
        Description = userGroup.Description,
        RowVersion = userGroup.RowVersion
      };
      return result;
    }

    public IQueryable<UserGroupResult> SearchUserGroup(
        IQueryable<UserGroupResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.Name.Contains(searchText) ||
                item.Description.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
  }
}

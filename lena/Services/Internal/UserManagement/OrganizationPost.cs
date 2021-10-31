using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.UserManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.UserManagement.OrganizationPosts;
using lena.Models.UserManagement.UserGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    #region Gets
    public IQueryable<TResult> GetOrganizationPosts<TResult>(
        Expression<Func<OrganizationPost, TResult>> selector,
        TValue<int> id = null,
        TValue<int> parentId = null,
          TValue<string> title = null,
          TValue<string> description = null,
          TValue<bool> isActive = null,
          TValue<bool> isAdmin = null,
          TValue<int> creatorId = null
    )
    {
      var query = repository.GetQuery<OrganizationPost>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (parentId != null)
        query = query.Where(i => i.ParentId == parentId);
      if (title != null)
        query = query.Where(i => i.Title == title);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);
      if (isAdmin != null)
        query = query.Where(i => i.IsAdmin == isAdmin);
      if (creatorId != null)
        query = query.Where(i => i.CreatorId == creatorId);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public OrganizationPost GetOrganizationPost(int id) => GetOrganizationPost(selector: e => e, id: id);
    public TResult GetOrganizationPost<TResult>(Expression<Func<OrganizationPost, TResult>> selector, int id)
    {
      var post = GetOrganizationPosts(
                selector: selector,
                id: id).SingleOrDefault();
      if (post == null)
        throw new OrganizationPostNotFoundException(id);
      return post;
    }
    #endregion
    #region AddProcess
    public OrganizationPost AddOrganizationPostProcess(
    int? parentId,
    string title,
    string description,
    bool isActive,
    bool isNewUserGroup,
    int? userGroupId,
    bool isAdmin)
    {
      if (isNewUserGroup)
      {
        var userGroup = AddUserGroup(
                  name: title,
                  description: description
              );
        userGroupId = userGroup.Id;
      }
      var post = AddOrganizationPost(
                parentId: parentId,
                title: title,
                description: description,
                isActive: isActive,
                isAdmin: isAdmin,
                userGroupId: userGroupId.Value
            );
      return post;
    }
    #endregion
    #region Add
    public OrganizationPost AddOrganizationPost(
    int? parentId,
    string title,
    string description,
    bool isActive,
    int userGroupId,
    bool isAdmin)
    {
      var post = repository.Create<OrganizationPost>();
      post.ParentId = parentId;
      post.Title = title;
      post.Description = description;
      post.IsActive = isActive;
      post.CreationTime = DateTime.UtcNow;
      post.CreatorId = App.Providers.Security.CurrentLoginData.UserId;
      post.IsAdmin = isAdmin;
      post.UserGroup = GetUserGroup(id: userGroupId);
      repository.Add(post);
      return post;
    }
    #endregion
    #region Delete
    public void DeleteOrganizationPost(int id)
    {
      var post = GetOrganizationPost(id);
      repository.Delete(post);
    }
    #endregion
    #region ToComboResult
    public Expression<Func<OrganizationPost, OrganizationPostComboResult>> ToOrganizationPostComboResult =
        post => new OrganizationPostComboResult()
        {
          Id = post.Id,
          Title = post.Title,
          RowVersion = post.RowVersion,
        };
    #endregion
    #region ToResult
    public Expression<Func<OrganizationPost, OrganizationPostResult>> ToOrganizationPostResult =
       result => new OrganizationPostResult()
       {
         Id = result.Id,
         ParentId = result.ParentId,
         ParentName = result.Parent.Title,
         Title = result.Title,
         Description = result.Description,
         IsActive = result.IsActive,
         RowVersion = result.RowVersion,
         CreationTime = result.CreationTime,
         CreatorId = result.CreatorId,
         IsAdmin = result.IsAdmin,
         UserGroupId = result.UserGroup.Id,
         UserGroupName = result.UserGroup.Name,
         CreatorFullName = result.Creator.Employee.FirstName + " " + result.Creator.Employee.LastName,
       };
    #endregion
    #region Search
    public IQueryable<OrganizationPostResult> SearchOrganizationPost(
        IQueryable<OrganizationPostResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                item.ParentName.Contains(searchText) ||
                item.Title.Contains(searchText) ||
                item.Description.Contains(searchText) ||
                item.CreatorFullName.Contains(searchText)
                select item;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region SortResult
    public IOrderedQueryable<OrganizationPostResult> SortOrganizationPostResult(
        IQueryable<OrganizationPostResult> query,
        SortInput<OrganizationPostSortType> sort)
    {
      switch (sort.SortType)
      {
        case OrganizationPostSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case OrganizationPostSortType.ParentName:
          return query.OrderBy(a => a.ParentName, sort.SortOrder);
        case OrganizationPostSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case OrganizationPostSortType.IsActive:
          return query.OrderBy(a => a.IsActive, sort.SortOrder);
        case OrganizationPostSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case OrganizationPostSortType.CreatorFullName:
          return query.OrderBy(a => a.CreatorFullName, sort.SortOrder);
        case OrganizationPostSortType.CreationTime:
          return query.OrderBy(a => a.CreationTime, sort.SortOrder);
        case OrganizationPostSortType.IsAdmin:
          return query.OrderBy(a => a.IsAdmin, sort.SortOrder);
        case OrganizationPostSortType.UserGroupName:
          return query.OrderBy(a => a.UserGroupName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region EditProcess
    public OrganizationPost EditOrganizationPostProcess(
        int id,
        byte[] rowVersion,
        TValue<int> parentId = null,
        TValue<int> userGroupId = null,
        TValue<string> description = null,
        TValue<string> title = null,
        TValue<bool> isAdmin = null,
        TValue<bool> isNewUserGroup = null,
        TValue<bool> isActive = null)
    {
      var organizationPost = GetOrganizationPost(id: id);
      if (isNewUserGroup)
      {
        var userGroup = AddUserGroup(
                  name: title,
                  description: description);
        userGroupId = userGroup.Id;
      }
      return EditOrganizationPost(
                organizationPost: organizationPost,
                rowVersion: rowVersion,
                userGroupId: userGroupId,
                parentId: parentId,
                description: description,
                title: title,
                isAdmin: isAdmin,
                isActive: isActive);
    }
    #endregion
    #region Edit
    public OrganizationPost EditOrganizationPost(
        int id,
        byte[] rowVersion,
        TValue<int> parentId = null,
        TValue<int> userGroupId = null,
        TValue<string> description = null,
        TValue<string> title = null,
        TValue<bool> isAdmin = null,
        TValue<bool> isActive = null)
    {
      var post = GetOrganizationPost(id: id);
      if (parentId != null)
        post.ParentId = parentId;
      if (title != null)
        post.Title = title;
      if (description != null)
        post.Description = description;
      if (isActive != null)
        post.IsActive = isActive;
      if (isAdmin != null)
        post.IsAdmin = isAdmin;
      if (userGroupId != null)
        post.UserGroup = GetUserGroup(id: userGroupId);
      repository.Update(post, rowVersion);
      return post;
    }
    public OrganizationPost EditOrganizationPost(
        OrganizationPost organizationPost,
        byte[] rowVersion,
        TValue<int> parentId = null,
        TValue<int> userGroupId = null,
        TValue<string> description = null,
        TValue<string> title = null,
        TValue<bool> isAdmin = null,
        TValue<bool> isActive = null)
    {
      if (parentId != null)
        organizationPost.ParentId = parentId;
      if (title != null)
        organizationPost.Title = title;
      if (description != null)
        organizationPost.Description = description;
      if (isActive != null)
        organizationPost.IsActive = isActive;
      if (isAdmin != null)
        organizationPost.IsAdmin = isAdmin;
      if (userGroupId != null)
        organizationPost.UserGroup = GetUserGroup(id: userGroupId);
      repository.Update(organizationPost, rowVersion);
      return organizationPost;
    }
    #endregion
  }
}
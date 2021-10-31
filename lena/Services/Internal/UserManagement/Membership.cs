using System.Linq;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Models.UserManagement.Membership;
using System.Linq.Expressions;
using System;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    public IQueryable<TResult> GetMemberships<TResult>(
        Expression<Func<Membership, TResult>> selector,
        TValue<int> id = null,
        TValue<int> userGroupId = null,
        TValue<int> userId = null,
        TValue<int[]> userGroupIds = null)
    {

      var query = repository.GetQuery<Membership>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (userGroupId != null)
        query = query.Where(i => i.UserGroupId == userGroupId);
      if (userGroupIds != null)
        query = query.Where(i => userGroupIds.Value.Contains(i.UserGroupId));

      return query.Select(selector: selector);
    }
    public Membership GetMembership(int id) => GetMembership(selector: e => e, id: id);
    public TResult GetMembership<TResult>(
        Expression<Func<Membership, TResult>> selector,
        int id)
    {

      var membership = GetMemberships(selector: selector, id: id).SingleOrDefault();
      if (membership == null)
        throw new MembershipNotFoundException(id);
      return membership;
    }
    public Membership AddMembership(int userGroupId, int userId)
    {

      GetUser(userId);
      GetUserGroup(userGroupId);
      var membership = repository.Create<Membership>();
      membership.UserId = userId;
      membership.UserGroupId = userGroupId;
      repository.Add(membership);
      return membership;
    }
    public Membership EditMembership(byte[] rowVersion, int id, TValue<int> userGroupId = null, TValue<int> userId = null)
    {

      var membership = GetMembership(id);
      if (userGroupId != null)
        membership.UserGroupId = userGroupId;
      if (userId != null)
        membership.UserId = userId;
      repository.Update(membership, rowVersion: rowVersion);
      return membership;
    }
    public void DeleteMembership(int id)
    {

      var membership = GetMembership(id);
      //if (membership.User != null)
      //    throw new MemebershipHasRelationshipWithUsers(id);
      //if (membership.UserGroup != null)
      //    throw new MemebershipHasRelationshipWithUsers(id);
      repository.Delete(membership);
    }
    public void SaveMemberships(SaveMembershipInput[] membershipInputs)
    {

      foreach (var membershipItem in membershipInputs)
      {
        var membership = GetMemberships(e => e, userId: membershipItem.UserId, userGroupId: membershipItem.UserGroupId)

                      .FirstOrDefault();
        if (!membershipItem.IsMember && membership != null)
          DeleteMembership(id: membership.Id);
        else if (membershipItem.IsMember && membership == null)
          AddMembership(membershipItem.UserGroupId, membershipItem.UserId);
      }
    }
    public IQueryable<UserMembershipResult> ToUserMembershipResult(IQueryable<Membership> memberships, IQueryable<UserGroup> userGroups)
    {
      var query = from userGroup in userGroups
                  from membership in memberships.Where(u => u.UserGroupId == userGroup.Id).DefaultIfEmpty()
                  select new UserMembershipResult()
                  {
                    MembershipId = membership.Id,
                    UserGroupId = userGroup.Id,
                    UserGroupName = userGroup.Name,
                    IsMember = membership != null
                  };
      return query;
    }
    public IQueryable<UserGroupMembershipResult> ToUserGroupMembershipResult(IQueryable<Membership> memberships, IQueryable<User> users)
    {
      var query =
                  from user in users
                  from membership in memberships.Where(u => u.UserId == user.Id).DefaultIfEmpty()
                  select new UserGroupMembershipResult()
                  {
                    MembershipId = membership.Id,
                    UserName = user.UserName,
                    UserId = user.Id,
                    IsMember = membership != null,
                    EmployeeCode = user.Employee.Code,
                    FirstName = user.Employee.FirstName,
                    LastName = user.Employee.LastName
                  };
      return query;
    }
  }
}
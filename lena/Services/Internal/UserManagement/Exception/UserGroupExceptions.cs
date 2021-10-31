using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement.Exception
{
  #region User Group Exceptions

  public class UserGroupNotFoundException : InternalServiceException
  {

    public int UserGroupId { get; }

    public UserGroupNotFoundException(int userGroupId)
    {
      UserGroupId = userGroupId;
    }

  }

  public class UserGroupDeleteNotFoundException : InternalServiceException
  {

    public int UserGroupId { get; }

    public UserGroupDeleteNotFoundException(int userGroupId)
    {
      UserGroupId = userGroupId;
    }

  }

  public class UserGroupDeleteFailedRemoveMembershipException : InternalServiceException
  {

    public int UserGroupId { get; }

    public UserGroupDeleteFailedRemoveMembershipException(int userGroupId)
    {
      UserGroupId = userGroupId;
    }

  }

  public class UserGroupDeleteFailedRemovePermissionException : InternalServiceException
  {

    public int UserGroupId { get; }

    public UserGroupDeleteFailedRemovePermissionException(int userGroupId)
    {
      UserGroupId = userGroupId;
    }

  }

  #endregion
}

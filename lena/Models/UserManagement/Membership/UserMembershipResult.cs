using lena.Domains.Enums;
namespace lena.Models.UserManagement.Membership
{
  public class UserMembershipResult
  {
    public int? MembershipId { get; set; }
    public int UserGroupId { get; set; }
    public string UserGroupName { get; set; }
    public bool IsMember { get; set; }
  }
}

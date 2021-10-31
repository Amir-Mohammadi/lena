using lena.Domains.Enums;
namespace lena.Models.UserManagement.Membership
{
  public class MembershipResult
  {
    public int UserId { get; set; }
    public int UserGroupId { get; set; }
    public bool IsMember { get; set; }
  }
}

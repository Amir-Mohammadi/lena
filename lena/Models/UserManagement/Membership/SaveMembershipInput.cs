using lena.Domains.Enums;
namespace lena.Models.UserManagement.Membership
{
  public class SaveMembershipInput
  {
    public int UserGroupId { get; set; }
    public int UserId { get; set; }
    public bool IsMember { get; set; }
  }
}

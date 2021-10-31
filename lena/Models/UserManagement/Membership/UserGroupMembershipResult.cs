using lena.Domains.Enums;
namespace lena.Models.UserManagement.Membership
{
  public class UserGroupMembershipResult
  {
    public int? MembershipId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public bool IsMember { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmployeeCode { get; set; }

  }
}

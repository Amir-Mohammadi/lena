using lena.Domains.Enums;
namespace lena.Models.UserManagement.OrganizationPosts
{
  public class AddOrganizationPostInput
  {
    public int? ParentId { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsAdmin { get; set; }
    public int? UserGroupId { get; set; }
    public bool IsNewUserGroup { get; set; }
  }
}

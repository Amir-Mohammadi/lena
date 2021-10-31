using lena.Domains.Enums;
namespace lena.Models.UserManagement.OrganizationPosts
{
  public class EditOrganizationPostInput : AddOrganizationPostInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

using lena.Domains.Enums;
namespace lena.Models.UserManagement.UserGroup
{
  public class EditUserGroupInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

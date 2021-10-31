using lena.Domains.Enums;
namespace lena.Models
{
  public class UserGroupPermissionInputs
  {
    public PermissionInput[] PermissionInputs { get; set; }
    public int UserGroupId { get; set; }
  }
}

using lena.Domains.Enums;
namespace lena.Models
{
  public class UserPermissionInputs
  {
    public PermissionInput[] PermissionInputs { get; set; }
    public int UserId { get; set; }
  }
}

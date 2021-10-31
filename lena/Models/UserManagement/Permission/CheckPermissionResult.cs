using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.Permission
{
  public class CheckPermissionResult
  {
    public int? SecurityActionId { get; set; }
    public string SecurityActionName { get; set; }
    public string SecurityActionAddress { get; set; }
    public AccessType AccessType { get; set; }
  }
}

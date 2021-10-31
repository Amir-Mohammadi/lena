using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.SecurityAction
{
  public class SecurityActionWithPermissionResult
  {
    public int SecurityActionId { get; set; }
    public string SecurityActionName { get; set; }
    public AccessType? DefaultAccessType { get; set; }
    public AccessType? AccessType { get; set; }
    public int SecurityActionGroupId { get; set; }
    public byte[] RowVersion { get; set; }

  }
}

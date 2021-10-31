using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.PermissionRequest
{
  public class PermissionRequestActionResult
  {
    public int Id { get; set; }
    public int PermissionRequestId { get; set; }
    public AccessType? AccessType { get; set; }
    public int SecurityActionId { get; set; }
    public string Description { get; set; }
    public string ConfirmationUserFullName { get; set; }
    public PermissionRequestActionStatus Status { get; set; }
    public string SecurityActionName { get; set; }
  }
}
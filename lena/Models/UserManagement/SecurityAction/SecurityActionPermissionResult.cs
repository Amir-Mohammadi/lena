using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Models.UserManagement.SecurityAction
{
  public class SecurityActionPermissionResult
  {
    public int Id { get; set; }
    public int SecurityActionId { get; set; }
    public string SecurityActionName { get; set; }
    public int SecurityActionGroupId { get; set; }
    public string SecurityActionGroupName { get; set; }
    public int? UserId { get; set; }
    public string UserName { get; set; }
    public int? UserGroupId { get; set; }
    public string UserGroupName { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public string EmployeeCode { get; set; }
    public AccessType AccessType { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

using lena.Models.UserManagement.SecurityAction;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.Permission
{
  public class CheckPermissionInput
  {
    public string ActionName { get; set; }
    public ActionParameterInput[] ActionParameters { get; set; }
    /// <summary>
    /// When requests come from internet, it must check action is public action otherwise prevent access to it.
    /// </summary>
    public bool IsExternalRequest { get; set; }
  }
}

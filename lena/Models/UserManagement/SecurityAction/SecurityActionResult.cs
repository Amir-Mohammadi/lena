using lena.Domains.Enums;
namespace lena.Models.UserManagement.SecurityAction
{
  public class SecurityActionResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string ActionName { get; set; }
    public int? SecurityActionGroupId { get; set; }
    public string SecurityActionGroupName { get; set; }
    public string SecurityActionGroupDisplayName { get; set; }
    public ActionParameterResult[] ActionParameters { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

using lena.Domains.Enums;
namespace lena.Models.UserManagement.SecurityActionGroup
{
  public class SecurityActionGroupResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{
  public class PermissionInput
  {
    public int SecurityActionId { get; set; }
    public AccessType? AccessType { get; set; }
  }
}

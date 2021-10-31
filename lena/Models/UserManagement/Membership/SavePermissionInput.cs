using System.Collections.Generic;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{
  public class SavePermissionInput
  {
    public Dictionary<int, AccessType?> SecurityActionAccesses { get; set; }
    public int? UserId { get; set; } = null;
    public int? UserGroupId { get; set; } = null;

  }
}

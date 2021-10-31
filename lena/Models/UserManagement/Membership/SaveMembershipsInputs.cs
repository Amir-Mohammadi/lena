using System.Collections.Generic;
using lena.Models.UserManagement.Membership;

using lena.Domains.Enums;
namespace lena.Models
{
  public class SaveMembershipsInputs
  {
    public List<MembershipResult> Memberships { get; set; }

  }
}

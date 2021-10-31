using System;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.OganizationPostHistory
{
  public class AddOrganizationPostHistoryInput
  {
    public int EmployeeId { get; set; }
    public int OrganizationPostId { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
  }
}

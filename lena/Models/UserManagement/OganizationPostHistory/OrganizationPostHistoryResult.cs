using System;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.OganizationPostHistory
{
  public class OrganizationPostHistoryResult
  {
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public string EmployeeFullName { get; set; }
    public int OrganizationPostId { get; set; }
    public string OrganizationPostName { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime CreationTime { get; set; }
    public int CreatorId { get; set; }
    public string CreatorFullName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

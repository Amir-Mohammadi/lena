using System;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.DepartmentManager
{
  public class DepartmentManagerResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int? DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int? OrganizationPostId { get; set; }
    public string OrganizationPostName { get; set; }
    public DateTime? DateTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}


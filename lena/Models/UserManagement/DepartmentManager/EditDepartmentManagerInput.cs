using System;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.DepartmentManager
{
  public class EditDepartmentManagerInput
  {
    public int Id { get; set; }
    public short DepartmentId { get; set; }
    public int OrganizationPostId { get; set; }
    public int UserId { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

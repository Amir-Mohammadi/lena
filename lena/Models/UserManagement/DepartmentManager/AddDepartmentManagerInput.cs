using System;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.DepartmentManager

{
  public class AddDepartmentManagerInput
  {
    public int Id { get; set; }

    public short DepartmentId { get; set; }
    public int OrganizationPostId { get; set; }

    public DateTime DateTime { get; set; }

  }


}

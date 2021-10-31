using lena.Domains.Enums;
namespace lena.Models.UserManagement.EmployeeComplainDepartment
{
  public class AddEmployeeComplainDepartmentInput
  {
    public int? EmployeeComplainItemId { get; set; }
    public short[] DepartmentIds { get; set; }

  }
}

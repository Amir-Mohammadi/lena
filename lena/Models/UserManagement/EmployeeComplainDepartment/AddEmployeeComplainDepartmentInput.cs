using lena.Domains.Enums;
namespace lena.Models.UserManagement.EmployeeComplainDepartment
{
  public class DeleteEmployeeComplainDepartmentInput
  {
    public int? EmployeeComplainItemId { get; set; }
    public int[] Ids { get; set; }

  }
}

using lena.Models.UserManagement.EmployeeComplainDepartment;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.EmployeeComplainItem
{
  public class DeleteEmployeeComplainItemInput
  {
    public int Id { get; set; }
    public DeleteEmployeeComplainDepartmentInput[] DeleteEmployeeComplainDepartmentInputs { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
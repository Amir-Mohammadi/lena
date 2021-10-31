using lena.Domains.Enums;
namespace lena.Models.UserManagement.EmployeeComplainDepartment
{
  public class EditEmployeeComplainDepartmentInput
  {
    public int Id { get; set; }
    public int EmployeeComplainItemId { get; set; }
    public string DepartmentName { get; set; }
    public int DepartmentId { get; set; }
    public byte[] RowVersion { get; set; }

  }
}

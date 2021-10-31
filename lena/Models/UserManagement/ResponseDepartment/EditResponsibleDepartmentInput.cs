using lena.Domains.Enums;
namespace lena.Models.UserManagement.ResponseDepartment
{
  public class EditResponsibleDepartmentInput
  {
    public int Id { get; set; }
    public int EmployeeComplainDepartmentId { get; set; }
    public string Opinion { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

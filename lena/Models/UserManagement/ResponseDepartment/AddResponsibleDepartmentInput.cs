using lena.Domains.Enums;
namespace lena.Models.UserManagement.ResponseDepartment
{
  public class AddResponsibleDepartmentInput
  {
    public int ResponseDepartmentId { get; set; }
    public int EmployeeComplainDepartmentId { get; set; }
    public string Opinion { get; set; }

  }
}

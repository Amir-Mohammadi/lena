using lena.Domains.Enums;
namespace lena.Models.UserManagement.ResponseDepartment
{
  public class ResponsibleDepartmentResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EmployeeComplainDepartmentId { get; set; }
    public string Opinion { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public string UserFullName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
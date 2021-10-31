using lena.Models.UserManagement.ResponseDepartment;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.EmployeeComplainDepartment
{
  public class EmployeeComplainDepartmentResult
  {
    public int Id { get; set; }
    public int EmployeeComplainItemId { get; set; }
    public string DepartmentName { get; set; }
    public int DepartmentId { get; set; }
    public IQueryable<ResponsibleDepartmentResult> ResponsibleDepartment { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
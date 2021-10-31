using lena.Domains.Enums;
using lena.Models.UserManagement.EmployeeComplainDepartment;
using lena.Models.UserManagement.QAReviewEmployeeComplain;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.EmployeeComplainItem
{
  public class EmployeeComplainItemResult
  {
    public int Id { get; set; }
    public int EmployeeComplainId { get; set; }
    public string EmployeeComplainFullName { get; set; }
    public EmployeeComplainType Type { get; set; }
    public string Description { get; set; }
    public IQueryable<EmployeeComplainDepartmentResult> EmployeeComplainDepartments { get; set; }
    public IQueryable<QAReviewEmployeeComplainResult> QAReviewEmployeeComplains { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
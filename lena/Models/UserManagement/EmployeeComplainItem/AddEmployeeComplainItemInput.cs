using lena.Domains.Enums;
using lena.Models.UserManagement.EmployeeComplainDepartment;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.EmployeeComplainItem
{
  public class AddEmployeeComplainItemInput
  {
    public int EmployeeComplainId { get; set; }
    public EmployeeComplainType Type { get; set; }
    public string Description { get; set; }
    public AddEmployeeComplainDepartmentInput[] AddEmployeeComplainDepartmentInput { get; set; }


  }
}

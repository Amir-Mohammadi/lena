using lena.Models.UserManagement.EmployeeComplainDepartment;
using lena.Models.UserManagement.EmployeeComplainItem;
using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AddEmployeeComplainInput
  {

    public int Id { get; set; }
    public int UserId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime DateTime { get; set; }
    public AddEmployeeComplainItemInput[] AddEmployeeComplainItemInput { get; set; }
    public DeleteEmployeeComplainItemInput[] DeleteEmployeeComplainItemInputs { get; set; }
    public DeleteEmployeeComplainDepartmentInput[] DeleteEmployeeComplainDepartmentInputs { get; set; }

  }
}


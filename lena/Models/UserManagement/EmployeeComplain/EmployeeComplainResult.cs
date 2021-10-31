using lena.Domains.Enums;
using lena.Models.UserManagement.EmployeeComplainItem;
using System;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.EmployeeComplain
{
  public class EmployeeComplainResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public string UserFullName { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public EmployeeComplainType Type { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int DepartmentCount { get; set; }
    public IQueryable<EmployeeComplainItemResult> EmployeeComplainItems { get; set; }
    public byte[] RowVersion { get; set; }
  }
}


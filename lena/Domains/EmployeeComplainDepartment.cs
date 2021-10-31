using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EmployeeComplainDepartment : IEntity
  {
    protected internal EmployeeComplainDepartment()
    {
      this.ResponsibleDepartments = new HashSet<ResponsibleDepartment>();
    }
    public int Id { get; set; }
    public short DepartmentId { get; set; }
    public int EmployeeComplainItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual EmployeeComplainItem EmployeeComplainItem { get; set; }
    public virtual Department Department { get; set; }
    public virtual ICollection<ResponsibleDepartment> ResponsibleDepartments { get; set; }
  }
}

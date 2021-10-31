using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ResponsibleDepartment : IEntity
  {
    protected internal ResponsibleDepartment()
    {
    }
    public int Id { get; set; }
    public int EmployeeComplainDepartmentId { get; set; }
    public int UserId { get; set; }
    public string Opinion { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual EmployeeComplainDepartment EmployeeComplainDepartment { get; set; }
  }
}

using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EmployeeComplainItem : IEntity
  {
    protected internal EmployeeComplainItem()
    {
      this.QAReviewEmployeeComplains = new HashSet<QAReviewEmployeeComplain>();
      this.EmployeeComplainDepartments = new HashSet<EmployeeComplainDepartment>();
    }
    public int Id { get; set; }
    public int EmployeeComplainId { get; set; }
    public EmployeeComplainType Type { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual EmployeeComplain EmployeeComplain { get; set; }
    public virtual ICollection<EmployeeComplainDepartment> EmployeeComplainDepartments { get; set; }
    public virtual ICollection<QAReviewEmployeeComplain> QAReviewEmployeeComplains { get; set; }
  }
}

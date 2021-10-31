using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EvaluationCategory : IEntity
  {
    protected internal EvaluationCategory()
    {
      this.EvaluationCategoryItems = new HashSet<EvaluationCategoryItem>();
      this.EmployeeEvaluationItems = new HashSet<EmployeeEvaluationItem>();
      this.EmployeeEvaluationPeriodItems = new HashSet<EmployeeEvaluationPeriodItem>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public Nullable<short> DepartmentId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Department Department { get; set; }
    public virtual ICollection<EmployeeEvaluationItem> EmployeeEvaluationItems { get; set; }
    public virtual ICollection<EvaluationCategoryItem> EvaluationCategoryItems { get; set; }
    public virtual ICollection<EmployeeEvaluationPeriodItem> EmployeeEvaluationPeriodItems { get; set; }
  }
}

using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EmployeeEvaluationPeriod : IEntity
  {
    protected internal EmployeeEvaluationPeriod()
    {
      this.EmployeeEvaluations = new HashSet<EmployeeEvaluation>();
      this.EmployeeEvaluationPeriodItems = new HashSet<EmployeeEvaluationPeriodItem>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
    public int UserId { get; set; }
    public bool? IsActive { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public EmployeeEvaluationPeriodStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<EmployeeEvaluation> EmployeeEvaluations { get; set; }
    public virtual ICollection<EmployeeEvaluationPeriodItem> EmployeeEvaluationPeriodItems { get; set; }
  }
}

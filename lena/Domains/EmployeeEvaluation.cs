using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EmployeeEvaluation : IEntity
  {
    protected EmployeeEvaluation()
    {
      this.EmployeeEvaluationItems = new HashSet<EmployeeEvaluationItem>();
    }
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int EmployeeEvaluationPeriodId { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public int CreatedUserId { get; set; }
    public byte[] RowVersion { get; set; }
    public Employee Employee { get; set; }
    public User User { get; set; }
    public virtual EmployeeEvaluationPeriod EmployeeEvaluationPeriod { get; set; }
    public virtual ICollection<EmployeeEvaluationItem> EmployeeEvaluationItems { get; set; }
  }
}

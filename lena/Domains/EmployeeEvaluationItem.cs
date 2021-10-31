using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EmployeeEvaluationItem : IEntity
  {
    protected EmployeeEvaluationItem()
    {
      this.EmployeeEvaluationItemDetails = new HashSet<EmployeeEvaluationItemDetail>();
    }
    public int EmployeeEvaluationId { get; set; }
    public int EvaluationCategoryId { get; set; }
    public EmployeeEvaluationStatus Status { get; set; }
    public Nullable<DateTime> PermanentDateTime { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public virtual EmployeeEvaluation EmployeeEvaluation { get; set; }
    public virtual EvaluationCategory EvaluationCategory { get; set; }
    public virtual ICollection<EmployeeEvaluationItemDetail> EmployeeEvaluationItemDetails { get; set; }
    public virtual User User { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

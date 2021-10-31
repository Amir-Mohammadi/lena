using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EmployeeEvaluationPeriodItem : IEntity
  {
    protected EmployeeEvaluationPeriodItem()
    {
    }
    public int EmployeeEvaluationPeriodId { get; set; }
    public int EvaluationCategoryId { get; set; }
    public int Coefficient { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual EmployeeEvaluationPeriod EmployeeEvaluationPeriod { get; set; }
    public virtual EvaluationCategory EvaluationCategory { get; set; }
  }
}

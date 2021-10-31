using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EmployeeEvaluationItemDetail : IEntity
  {
    protected EmployeeEvaluationItemDetail()
    { }
    public int EmployeeEvaluationId { get; set; }
    public int EvaluationCategoryId { get; set; }
    public int EvaluationCategoryItemId { get; set; }
    public Score Score { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual EmployeeEvaluationItem EmployeeEvaluationItem { get; set; }
    public virtual EvaluationCategoryItem EvaluationCategoryItem { get; set; }
  }
}

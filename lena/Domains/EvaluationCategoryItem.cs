using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EvaluationCategoryItem : IEntity
  {
    protected EvaluationCategoryItem()
    {
      this.IsArchive = false;
      this.EmployeeEvaluationItemDetails = new HashSet<EmployeeEvaluationItemDetail>();
    }
    public int Id { get; set; }
    public int EvaluationCategoryId { get; set; }
    public string Question { get; set; }
    public bool IsArchive { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual EvaluationCategory EvaluationCategory { get; set; }
    public virtual ICollection<EmployeeEvaluationItemDetail> EmployeeEvaluationItemDetails { get; set; }
  }
}

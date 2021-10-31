using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance.Exception
{
  public class EmployeeEvaluationItemDetailNotFoundException : InternalServiceException
  {
    public int EmployeeEvaluationId { get; set; }
    public int EvaluationCategoryId { get; set; }
    public int EvaluationCategoryItemId { get; set; }
    public EmployeeEvaluationItemDetailNotFoundException(int employeeEvaluationId, int evaluationCategoryId, int evaluationCategoryItemId)
    {
      this.EmployeeEvaluationId = employeeEvaluationId;
      this.EvaluationCategoryId = evaluationCategoryId;
      this.EvaluationCategoryItemId = EvaluationCategoryItemId;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationItem
{
  public class GetEmployeeEvaluationItemsInput
  {
    public int? EmployeeEvaluationId { get; set; }
    public int? EvaluationCategoryId { get; set; }
  }
}

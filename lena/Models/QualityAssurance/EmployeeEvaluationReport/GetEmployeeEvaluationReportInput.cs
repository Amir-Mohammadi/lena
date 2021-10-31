using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationReport
{
  public class GetEmployeeEvaluationReportInput
  {
    public int? EmployeeId { get; set; }
    public int? DepartmentId { get; set; }
    public int[] EvaluationPeriodIds { get; set; }
  }
}

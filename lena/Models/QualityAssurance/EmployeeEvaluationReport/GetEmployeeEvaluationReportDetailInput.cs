using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationReport
{
  public class GetEmployeeEvaluationReportDetailInput
  {
    public int EmployeeId { get; set; }
    public string EmployeeEvaluationPeriodTitle { get; set; }
  }
}

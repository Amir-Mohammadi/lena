using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationReport
{
  public class EmployeeEvaluationReportResult
  {
    public ExpandoObject[] Data { get; set; }
    public string[] DynamicColumnNames { get; set; }
  }
}

using lena.Models.QualityAssurance.EmployeeEvaluationPeriodItem;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationPeriod
{
  public class AddEmployeeEvaluationPeriodInput
  {
    public string Title { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }

    public AddEmployeeEvaluationPeriodItemInput[] AddEmployeeEvaluationPeriodItemInputs { get; set; }
  }
}

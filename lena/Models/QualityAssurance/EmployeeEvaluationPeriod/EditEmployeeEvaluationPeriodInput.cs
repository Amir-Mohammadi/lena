using lena.Models.QualityAssurance.EmployeeEvaluationPeriodItem;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationPeriod
{
  public class EditEmployeeEvaluationPeriodInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
    public bool IsActive { get; set; }
    public AddEmployeeEvaluationPeriodItemInput[] AddEmployeeEvaluationPeriodItemInputs { get; set; }
    public EditEmployeeEvaluationPeriodItemInput[] EditEmployeeEvaluationPeriodItemInputs { get; set; }
    public DeleteEmployeeEvaluationPeriodItemInput[] DeleteEmployeeEvaluationPeriodItemInputs { get; set; }
    public byte[] RowVersion { get; set; }


  }
}

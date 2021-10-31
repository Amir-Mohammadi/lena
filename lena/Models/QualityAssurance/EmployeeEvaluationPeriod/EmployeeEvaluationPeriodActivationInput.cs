using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationPeriod
{
  public class EmployeeEvaluationPeriodActivationInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public bool? IsActive { get; set; }
  }
}

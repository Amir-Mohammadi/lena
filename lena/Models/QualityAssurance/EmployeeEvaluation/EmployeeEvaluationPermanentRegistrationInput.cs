using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluation
{
  public class EmployeeEvaluationPermanentRegistrationInput
  {
    public int EmployeeEvaluationId { get; set; }
    public int EvaluationCategoryId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

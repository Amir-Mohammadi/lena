using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class AddFinancialDocumentCorrectionInput : AddFinancialDocumentBeginningInput
  {
    public bool IsActive { get; set; }
  }
}

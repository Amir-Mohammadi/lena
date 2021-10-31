using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class SetFinancialDocumentCorrectionStatusInput
  {
    public int FinancialDocumentCorrectionId { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
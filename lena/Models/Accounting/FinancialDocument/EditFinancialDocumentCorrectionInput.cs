using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class EditFinancialDocumentCorrectionInput : AddFinancialDocumentBeginningInput
  {
    public int Id { get; set; }
    public bool? IsActive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

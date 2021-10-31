using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class DeleteFinancialDocumentCorrectionInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
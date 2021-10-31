using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class EditFinancialDocumentBeginningInput
  {
    public int Id { get; set; }
    public FinancialTransactionLevel FinancialTransactionLevel { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

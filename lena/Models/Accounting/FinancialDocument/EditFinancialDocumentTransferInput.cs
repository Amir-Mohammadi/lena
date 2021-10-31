using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class EditFinancialDocumentTransferInput
  {
    public int Id { get; set; }
    public int ToFinancialAccountId { get; set; }
    public double ToAmount { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

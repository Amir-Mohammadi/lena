using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class AddFinancialDocumentTransferInput
  {

    public int ToFinancialAccountId { get; set; }
    public double ToAmount { get; set; }
  }
}

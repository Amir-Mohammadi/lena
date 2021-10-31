using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class EditFinancialDocumentDiscountInput : AddFinancialDocumentDiscountInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

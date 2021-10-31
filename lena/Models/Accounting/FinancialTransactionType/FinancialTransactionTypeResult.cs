using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialTransactionType
{
  public class FinancialTransactionTypeResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public TransactionTypeFactor Factor { get; set; }
    public FinancialTransactionLevel FinancialTransactionType { get; set; }
    public int? RollbackFinancialTransactionTypeId { get; set; }
    public string RollbackFinancialTransactionTypeTitle { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
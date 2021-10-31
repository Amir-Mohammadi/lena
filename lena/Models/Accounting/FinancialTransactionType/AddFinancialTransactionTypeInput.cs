using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialTransactionType
{
  public class AddFinancialTransactionTypeInput
  {
    public string Name { get; set; }
    public FinancialTransactionLevel FinancialTransactionLevel { get; set; }
    public int? RollbackFinancialTransactionTypeId { get; set; }
    public TransactionTypeFactor Factor { get; set; }
  }
}

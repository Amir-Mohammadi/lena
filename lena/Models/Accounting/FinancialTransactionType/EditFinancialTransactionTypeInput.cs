using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialTransactionType
{
  public class EditFinancialTransactionTypeInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public TransactionLevel transactionLevel { get; set; }
    public int? rollbackTransactionTypeId { get; set; }
    public TransactionTypeFactor Factor { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
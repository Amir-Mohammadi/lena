using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.TransactionType
{
  public class AddTransactionTypeInput
  {
    public string Name { get; set; }
    public TransactionLevel transactionLevel { get; set; }
    public short? rollbackTransactionTypeId { get; set; }
    public TransactionTypeFactor Factor { get; set; }
  }
}

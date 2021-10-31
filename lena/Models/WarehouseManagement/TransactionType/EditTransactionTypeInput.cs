using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.TransactionType
{
  public class EditTransactionTypeInput
  {
    public short Id { get; set; }
    public string Name { get; set; }
    public TransactionLevel transactionLevel { get; set; }
    public short? rollbackTransactionTypeId { get; set; }
    public TransactionTypeFactor Factor { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
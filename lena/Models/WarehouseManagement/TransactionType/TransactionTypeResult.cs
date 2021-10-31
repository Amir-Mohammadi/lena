using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.TransactionType
{
  public class TransactionTypeResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public TransactionTypeFactor Factor { get; set; }
    public TransactionLevel TransactionType { get; set; }
    public int? RollbackTransactionTypeId { get; set; }
    public string RollbackTransactionTypeName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
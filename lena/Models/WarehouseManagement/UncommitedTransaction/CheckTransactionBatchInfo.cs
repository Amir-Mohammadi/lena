using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.UncommitedTransaction
{
  public class CheckTransactionBatchInfo
  {
    public double Total;

    public TransactionLevel? TransactionLevel;

    public int StuffId { get; set; }

    public long? StuffSerialCode { get; set; }
    public int? WarehouseId { get; set; }
  }
}

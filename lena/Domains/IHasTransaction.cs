using lena.Domains.Enums;
namespace lena.Domains
{
  public interface IHasTransaction
  {
    int? TransactionBatchId { get; set; }
    TransactionBatch TransactionBatch { get; set; }
  }
}
using lena.Domains.Enums;
namespace lena.Domains
{
  public interface IHasFinancialTransaction
  {
    int? FinancialTransactionBatchId { get; set; }
    FinancialTransactionBatch FinancialTransactionBatch { get; set; }
  }
}
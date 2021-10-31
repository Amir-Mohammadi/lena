using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialTransactionBatch
{
  public class FinancialTransactionBatchResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BaseEntityId { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }

  }
}

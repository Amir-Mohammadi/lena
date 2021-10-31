using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialTransaction
{
  public class HasRialRateInput
  {
    public int FinancialAccountId { get; set; }
    public DateTime NewDocumentDateTime { get; set; }
    public FinancialTransactionLevel? FinancialTransactionLevel { get; set; }
    public TransactionTypeFactor? TransactionTypeFactor { get; set; }
  }
}

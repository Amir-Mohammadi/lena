using System.Collections.Generic;
using lena.Domains.Enums;

namespace lena.Models.Accounting.RialInvoice
{
  public class SetRialRateForTransferDepositOfTransferExpenseResult
  {
    public Domains.FinancialTransaction TransferDeposit { get; set; }
    public List<InMemoryRialRate> InMemoryRialRates { get; set; }
  }
}

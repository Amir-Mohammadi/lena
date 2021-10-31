using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  public class GetTransferFinancialTransactionsFromParlarResult
  {
    public IQueryable<int> FinancialTransactionIds { get; set; }
  }
}

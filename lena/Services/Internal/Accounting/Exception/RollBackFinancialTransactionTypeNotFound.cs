using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class RollBackFinancialTransactionTypeNotFound : InternalServiceException
  {
    public int FinancialTransactionTypeId { get; }

    public RollBackFinancialTransactionTypeNotFound(int financialTransactionTypeId)
    {
      this.FinancialTransactionTypeId = financialTransactionTypeId;
    }
  }
}

using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialTransferToTheSameAccountException : InternalServiceException
  {
    public int FinancialAccountId { get; }

    public FinancialTransferToTheSameAccountException(int financialAccountId)
    {
      FinancialAccountId = financialAccountId;
    }
  }
}

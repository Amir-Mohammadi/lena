using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class LadingCostHasNoFinancialTransactionException : InternalServiceException
  {
    public int LadingCostId { get; }

    public LadingCostHasNoFinancialTransactionException(int ladingCostId)
    {
      LadingCostId = ladingCostId;
    }
  }
}

using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class CargoCostHasNoFinancialTransactionException : InternalServiceException
  {
    public int CargoCostId { get; }

    public CargoCostHasNoFinancialTransactionException(int cargoCostId)
    {
      CargoCostId = cargoCostId;
    }
  }
}

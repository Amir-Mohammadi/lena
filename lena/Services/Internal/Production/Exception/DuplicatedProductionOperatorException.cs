using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class DuplicatedProductionOperatorException : InternalServiceException
  {
    public DuplicatedProductionOperatorException(int operationId, int productionOrderId)
    {

    }
  }
}

using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  internal class OperationConsumingMaterialNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public OperationConsumingMaterialNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

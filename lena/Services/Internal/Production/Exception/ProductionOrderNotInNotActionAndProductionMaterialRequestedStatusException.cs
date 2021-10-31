using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class ProductionOrderNotInNotActionAndProductionMaterialRequestedStatusException : InternalServiceException
  {
    public int Id { get; }

    public ProductionOrderNotInNotActionAndProductionMaterialRequestedStatusException(int id)
    {
      this.Id = id;
    }
  }
}

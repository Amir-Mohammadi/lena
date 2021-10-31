using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionPlanNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductionPlanNotFoundException(int id)
    {
      Id = id;
    }
  }
}

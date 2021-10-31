using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionPlanDetailNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductionPlanDetailNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

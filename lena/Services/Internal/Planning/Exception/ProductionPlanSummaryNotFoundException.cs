using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionPlanSummaryNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductionPlanSummaryNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

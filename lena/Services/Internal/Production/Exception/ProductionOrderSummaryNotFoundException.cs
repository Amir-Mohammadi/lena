using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class ProductionOrderSummaryNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductionOrderSummaryNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

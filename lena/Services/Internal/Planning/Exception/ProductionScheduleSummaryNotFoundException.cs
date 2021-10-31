using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionScheduleSummaryNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductionScheduleSummaryNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

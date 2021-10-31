using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class StuffPriceDiscrepancyNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public StuffPriceDiscrepancyNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

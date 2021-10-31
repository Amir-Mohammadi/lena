using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class StuffPriceNotFoundException : InternalServiceException
  {
    public long Id { get; }

    public StuffPriceNotFoundException(long id)
    {
      this.Id = id;
    }
  }
}

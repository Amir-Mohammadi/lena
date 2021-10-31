using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class StuffBasePriceNotFoundException : InternalServiceException
  {
    public long Id { get; }

    public StuffBasePriceNotFoundException(long id)
    {
      this.Id = id;
    }
  }
}

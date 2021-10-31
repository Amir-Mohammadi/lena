using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchasePriceNotFoundException : InternalServiceException
  {
    public long Id { get; }

    public PurchasePriceNotFoundException(long id)
    {
      this.Id = id;
    }
  }
}

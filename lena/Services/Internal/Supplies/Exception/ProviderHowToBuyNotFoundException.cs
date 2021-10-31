using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class ProviderHowToBuyNotFoundException : InternalServiceException
  {
    public int HowToBuyId { get; }
    public int ProviderId { get; }

    public ProviderHowToBuyNotFoundException(int howToBuyId, int providerId)
    {
      this.HowToBuyId = howToBuyId;
      this.ProviderId = providerId;
    }
  }
}

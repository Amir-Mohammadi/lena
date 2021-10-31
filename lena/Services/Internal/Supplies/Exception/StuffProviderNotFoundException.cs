using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class StuffProviderNotFoundException : InternalServiceException
  {
    public int StuffId { get; }
    public int ProviderId { get; }

    public StuffProviderNotFoundException(int stuffId, int providerId)
    {
      this.StuffId = stuffId;
      this.ProviderId = providerId;
    }
  }
}

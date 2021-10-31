using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Guard.Exception
{
  public class TransportNotFoundException : InternalServiceException
  {
    public int TransportId { get; }

    public TransportNotFoundException(int transportId)
    {
      this.TransportId = transportId;
    }
  }
}

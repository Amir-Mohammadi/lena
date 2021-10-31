using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Guard.Exception
{
  public class OutboundCargoNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public OutboundCargoNotFoundException(int id)
    {
      Id = id;
    }

  }
}

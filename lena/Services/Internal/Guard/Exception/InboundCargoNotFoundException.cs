using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Guard.Exception
{
  public class InboundCargoNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public InboundCargoNotFoundException(int id)
    {
      Id = id;
    }

  }
}

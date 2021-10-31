using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class StuffBasePriceTransportNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public StuffBasePriceTransportNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

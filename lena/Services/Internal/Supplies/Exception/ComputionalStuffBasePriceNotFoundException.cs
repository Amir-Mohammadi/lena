using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class ComputionalStuffBasePriceNotFoundException : InternalServiceException
  {
    public long Id { get; }

    public ComputionalStuffBasePriceNotFoundException(long id)
    {
      this.Id = id;
    }
  }
}

using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class StuffBasePriceCustomsNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public StuffBasePriceCustomsNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

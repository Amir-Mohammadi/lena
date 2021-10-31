using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseRequestNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public PurchaseRequestNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

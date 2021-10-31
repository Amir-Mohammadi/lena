using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseRequestNotConfirmedStatusException : InternalServiceException
  {
    public int Id { get; }

    public PurchaseRequestNotConfirmedStatusException(int id)
    {
      this.Id = id;
    }
  }
}

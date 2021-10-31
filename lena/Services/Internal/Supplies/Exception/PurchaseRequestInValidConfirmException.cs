using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseRequestInValidConfirmException : InternalServiceException
  {
    public int Id { get; }

    public PurchaseRequestInValidConfirmException(int id)
    {
      this.Id = id;
    }
  }
}

using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class ProvisionersCartItemPurchaseRequestExistException : InternalServiceException
  {

    public int PurchaseRequestId { get; }

    public ProvisionersCartItemPurchaseRequestExistException(int purchaserequestid)
    {
      this.PurchaseRequestId = purchaserequestid;
    }
  }
}
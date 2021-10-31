using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseRequestSummaryForPurchaseRequestNotFoundException : InternalServiceException
  {
    public int PurchaseRequestId { get; }

    public PurchaseRequestSummaryForPurchaseRequestNotFoundException(int purchaseRequestId)
    {
      this.PurchaseRequestId = purchaseRequestId;
    }
  }
}

using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseOrderDetailSummaryForPurchaseOrderDetailNotFoundException : InternalServiceException
  {
    public int PurchaseOrderDetailId { get; }

    public PurchaseOrderDetailSummaryForPurchaseOrderDetailNotFoundException(int purchaseOrderDetailId)
    {
      this.PurchaseOrderDetailId = purchaseOrderDetailId;
    }
  }
}

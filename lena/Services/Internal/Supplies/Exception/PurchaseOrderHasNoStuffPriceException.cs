using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseOrderHasNoStuffPriceException : InternalServiceException
  {
    public int PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }

    public PurchaseOrderHasNoStuffPriceException(int purchaseOrderId, string purchaseOrderCode)
    {
      PurchaseOrderId = purchaseOrderId;
      PurchaseOrderCode = purchaseOrderCode;
    }
  }
}

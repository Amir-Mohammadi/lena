using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseOrderHasNoProviderException : InternalServiceException
  {
    public int PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }

    public PurchaseOrderHasNoProviderException(
        int purchaseOrderId,
        string purchaseOrderCode)
    {
      PurchaseOrderId = purchaseOrderId;
      PurchaseOrderCode = purchaseOrderCode;
    }
  }
}

using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class PurchaseOrderCostHasNoFinancialTransactionException : InternalServiceException
  {
    public int PurchaseOrderCostId { get; }

    public PurchaseOrderCostHasNoFinancialTransactionException(int purchaseOrderCostId)
    {
      PurchaseOrderCostId = purchaseOrderCostId;
    }
  }
}

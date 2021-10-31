using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class PurchaseOrderDiscountHasNoFinancialTransactionException : InternalServiceException
  {
    public int PurchaseOrderDiscountId { get; }

    public PurchaseOrderDiscountHasNoFinancialTransactionException(int purchaseOrderDiscountId)
    {
      PurchaseOrderDiscountId = purchaseOrderDiscountId;
    }
  }
}

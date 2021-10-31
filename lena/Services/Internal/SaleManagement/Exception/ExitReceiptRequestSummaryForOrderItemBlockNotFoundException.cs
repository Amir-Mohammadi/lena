using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class ExitReceiptRequestSummaryForOrderItemBlockNotFoundException : InternalServiceException
  {
    public int OrderItemBlockId { get; }

    public ExitReceiptRequestSummaryForOrderItemBlockNotFoundException(int orderItemBlockId)
    {
      this.OrderItemBlockId = orderItemBlockId;
    }
  }
}

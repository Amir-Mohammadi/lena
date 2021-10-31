using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class OrderItemHasChangeException : InternalServiceException
  {
    public int OrderItemId { get; }

    public OrderItemHasChangeException(int orderItemId)
    {
      this.OrderItemId = orderItemId;
    }
  }
}

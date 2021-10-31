using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class OrderItemSummaryNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public OrderItemSummaryNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

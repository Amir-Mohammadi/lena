using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class OrderItemSaleBlockNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public OrderItemSaleBlockNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

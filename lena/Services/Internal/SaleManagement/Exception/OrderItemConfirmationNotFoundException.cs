using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class OrderItemConfirmationNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public OrderItemConfirmationNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

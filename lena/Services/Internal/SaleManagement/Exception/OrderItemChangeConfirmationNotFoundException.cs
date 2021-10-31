using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class OrderItemChangeConfirmationNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public OrderItemChangeConfirmationNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

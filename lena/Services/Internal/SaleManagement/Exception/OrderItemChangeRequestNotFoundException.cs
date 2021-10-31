using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class OrderItemChangeRequestNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public OrderItemChangeRequestNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

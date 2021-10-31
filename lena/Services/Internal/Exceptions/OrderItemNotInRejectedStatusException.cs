using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class OrderItemNotInRejectedStatusException : InternalServiceException
  {
    public int Id { get; }

    public OrderItemNotInRejectedStatusException(int id)
    {
      this.Id = id;
    }
  }
}

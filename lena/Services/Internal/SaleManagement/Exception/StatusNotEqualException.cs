using lena.Services.Core.Foundation;
using lena.Domains.Enums;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region OrderItemChangeStatus
  public class StatusNotEqualException : InternalServiceException
  {
    public OrderItemChangeStatus OrderItemChangeStatus { get; }
    public StatusNotEqualException(OrderItemChangeStatus currentChangeStatus)
    {
      this.OrderItemChangeStatus = currentChangeStatus;
    }

  }
  #endregion
}

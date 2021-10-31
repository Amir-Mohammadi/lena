using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class OrderItemNotInDeletableStatusException : InternalServiceException
  {
    public int OrderItemId { get; }
    public OrderItemNotInDeletableStatusException(int orderItemId)
    {
      OrderItemId = orderItemId;
    }
  }
}

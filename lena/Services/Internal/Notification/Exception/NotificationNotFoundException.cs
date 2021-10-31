using System;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Notification.Exception
{
  public class NotificationNotFoundException : System.Exception
  {
    public long Id { get; }

    public NotificationNotFoundException(long id)
    {
      Id = id;
    }
  }
}
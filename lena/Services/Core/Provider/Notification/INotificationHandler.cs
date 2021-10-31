using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Provider.Notification
{
  public interface INotificationHandler
  {
    bool Send(string address, object payload);

    bool Emit(string address, string eventName, object payload);

    bool Emit(string eventName, object payload);
  }
}

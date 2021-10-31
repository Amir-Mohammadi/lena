using System;
using lena.Models.Notification;

namespace lena.Services.Core.Provider.Notification
{
  public class NotificationProvider : Provider
  {
    private INotificationHandler Handler { get; }

    public NotificationProvider(INotificationHandler handler)
    {
      Handler = handler;
    }



    public bool Push(string channel, NotificationResult notification)
    {
      if (Handler == null) return false;
      try
      {
        Handler.Send(channel, notification);
      }
      catch (Exception)
      {
        return false;
      }
      return true;
    }


    public bool Emit(string channel, string eventName, object eventData)
    {
      if (Handler == null) return false;
      try
      {
        Handler.Emit(channel, eventName, eventData);
      }
      catch (Exception)
      {
        return false;
      }
      return true;
    }


    public bool Emit(string eventName, object eventData)
    {
      if (Handler == null) return false;
      try
      {
        Handler.Emit(eventName, eventData);
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }





  }
}

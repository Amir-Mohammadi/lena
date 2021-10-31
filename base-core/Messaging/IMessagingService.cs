using System.Threading;
using System.Threading.Tasks;
using core.Autofac;
namespace core.Messaging
{
  public interface IMessagingService : IScopedDependency
  {
    Task SendSMS(string phone, string message, CancellationToken cancellationToken);
  }
}
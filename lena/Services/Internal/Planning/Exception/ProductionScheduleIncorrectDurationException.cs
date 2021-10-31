using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionScheduleIncorrectDurationException : InternalServiceException
  {
    public long Duration { get; }
    public long SwitchTime { get; }
    public ProductionScheduleIncorrectDurationException(long duration, long switchTime)
    {
      this.Duration = duration;
      this.SwitchTime = switchTime;
    }
  }
}

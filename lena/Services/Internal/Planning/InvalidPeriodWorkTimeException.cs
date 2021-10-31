using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public class InvalidPeriodWorkTimeException : InternalServiceException
  {
    public int MaxDay { get; }
    public int Period { get; }

    public InvalidPeriodWorkTimeException(int maxDay, int period)
    {
      this.MaxDay = maxDay;
      this.Period = period;
    }
  }
}

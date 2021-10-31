using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance.Exception
{
  public class HasTheSameDayInWeightDayException : InternalServiceException
  {
    public int Day { get; }

    public HasTheSameDayInWeightDayException(int day)
    {
      this.Day = day;
    }
  }
}

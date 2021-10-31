using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class StuffHasNoGrossWeightException : InternalServiceException
  {
    public string StuffCode { get; set; }

    public StuffHasNoGrossWeightException(string stuffCode)
    {
      StuffCode = stuffCode;
    }
  }
}

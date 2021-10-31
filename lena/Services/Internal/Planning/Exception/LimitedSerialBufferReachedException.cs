using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class LimitedSerialBufferReachedException : InternalServiceException
  {
    public string StuffCode { get; set; }

    public LimitedSerialBufferReachedException(string stuffCode)
    {
      StuffCode = stuffCode;
    }
  }
}

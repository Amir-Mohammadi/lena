using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class SerialHasNoTransactionException : InternalServiceException
  {
    public string Serial { get; set; }

    public SerialHasNoTransactionException(string serial)
    {
      Serial = serial;
    }
  }
}
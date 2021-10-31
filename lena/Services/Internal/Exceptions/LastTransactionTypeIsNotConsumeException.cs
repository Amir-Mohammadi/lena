using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class LastTransactionTypeIsNotConsumeException : InternalServiceException
  {
    public string Serial { get; set; }

    public LastTransactionTypeIsNotConsumeException(string serial)
    {
      Serial = serial;
    }
  }
}
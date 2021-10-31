using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class MatchBlockedTransactionNotFoundException : InternalServiceException
  {
    public string Serial { get; }

    public MatchBlockedTransactionNotFoundException(string serial)
    {
      this.Serial = serial;
    }
  }
}

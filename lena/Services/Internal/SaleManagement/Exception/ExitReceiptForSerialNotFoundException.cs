using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class ExitReceiptForSerialNotFoundException : InternalServiceException
  {
    public string Serial { get; }

    public ExitReceiptForSerialNotFoundException(string serial)
    {
      this.Serial = serial;
    }
  }
}

using lena.Services.Core.Foundation;
using lena.Models.Common;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class WasteExitReceiptRequestHasExistException : InternalServiceException
  {
    public int ExitReceiptRequestId { get; }

    public WasteExitReceiptRequestHasExistException(int exitReceiptRequestId)
    {
      this.ExitReceiptRequestId = exitReceiptRequestId;
    }
  }
}

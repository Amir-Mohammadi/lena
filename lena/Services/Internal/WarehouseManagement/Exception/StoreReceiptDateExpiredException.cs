using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class StoreReceiptDateExpiredException : InternalServiceException
  {
    public int Days { get; set; }

    public StoreReceiptDateExpiredException(int days)
    {
      Days = days;
    }
  }
}

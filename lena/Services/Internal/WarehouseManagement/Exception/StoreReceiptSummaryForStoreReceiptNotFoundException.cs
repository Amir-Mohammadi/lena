using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class StoreReceiptSummaryForStoreReceiptNotFoundException : InternalServiceException
  {
    public int StoreReceiptId { get; }

    public StoreReceiptSummaryForStoreReceiptNotFoundException(int storeReceiptId)
    {
      this.StoreReceiptId = storeReceiptId; ;
    }
  }
}

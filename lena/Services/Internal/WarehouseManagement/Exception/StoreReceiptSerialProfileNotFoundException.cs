using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class StoreReceiptSerialProfileNotFoundException : InternalServiceException
  {
    public int Code { get; }
    public int StoreReceiptId { get; }
    public int StuffId { get; }

    public StoreReceiptSerialProfileNotFoundException(int storeReceiptId)
    {
      this.StoreReceiptId = storeReceiptId;
    }

    public StoreReceiptSerialProfileNotFoundException(int code, int stuffId)
    {
      this.Code = code;
      this.StuffId = stuffId;
    }
  }
}

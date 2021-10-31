using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class DuplicateStuffStockPlaceException : InternalServiceException
  {
    public int StockPlaceId { get; }
    public int StuffId { get; }

    public DuplicateStuffStockPlaceException(int stuffId, int stockPlaceId)
    {
      this.StuffId = stuffId;
      this.StockPlaceId = stockPlaceId;
    }
  }
}

using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class NotEnoughMaterialInWarehouseException : InternalServiceException
  {
    public short WarehouseId { get; }

    public double AvaliableAmount { get; }

    public double RequestedAmount { get; }

    public NotEnoughMaterialInWarehouseException(short warehouseId, double avaliableAmount, double requestedAmount)
    {
      WarehouseId = warehouseId;
      AvaliableAmount = avaliableAmount;
      RequestedAmount = requestedAmount;
    }
  }
}

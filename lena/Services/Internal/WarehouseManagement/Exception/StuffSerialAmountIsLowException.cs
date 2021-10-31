using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class StuffSerialAmountIsLowException : InternalServiceException
  {
    public string Serial { get; }
    public double? AvailableQty { get; }
    public double? BlockedQty { get; }
    public double? QualityControlQty { get; }
    public double? WasteQty { get; }

    public StuffSerialAmountIsLowException(string serial, double? availableQty, double? blockedQty, double? qualityControlQty, double? wasteQty)
    {
      this.Serial = serial;
      this.AvailableQty = availableQty;
      this.BlockedQty = blockedQty;
      this.QualityControlQty = qualityControlQty;
      this.WasteQty = wasteQty;
    }
  }
}

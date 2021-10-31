using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class StockCheckingTagNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public string Serial { get; }
    public int StockCheckingId { get; }
    public string TagSerial { get; }
    public short WarehouseId { get; }
    public int TagTypeId { get; }
    public int StuffId { get; }

    public StockCheckingTagNotFoundException(string tagSerial)
    {
      this.TagSerial = tagSerial;
    }

    public StockCheckingTagNotFoundException(int id)
    {
      this.Id = id;
    }

    public StockCheckingTagNotFoundException(
        int stockCheckingId,
        short warehouseId,
        int tagTypeId,
        int stuffId,
        string serial)
    {
      this.StockCheckingId = stockCheckingId;
      this.WarehouseId = warehouseId;
      this.TagTypeId = tagTypeId;
      this.StuffId = stuffId;
      this.Serial = serial;
    }
  }
}

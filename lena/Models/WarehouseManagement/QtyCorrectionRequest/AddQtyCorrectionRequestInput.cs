using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.QtyCorrectionRequest
{
  public class AddQtyCorrectionRequestInput
  {
    public int? StockCheckingTagId;
    public short WarehouseId { get; set; }
    public int StuffId { get; set; }
    public string Serial { get; set; }
    public string Description { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public QtyCorrectionRequestType Type { get; set; }
  }
}

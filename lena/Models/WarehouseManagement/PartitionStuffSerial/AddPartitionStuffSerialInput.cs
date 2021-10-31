using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.PartitionStuffSerial
{
  public class AddPartitionStuffSerialInput
  {
    public short warehouseId;
    public string Serial { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public int BoxCount { get; set; }
    public double QtyPerBox { get; set; }
    public string Description { get; set; }
    public SerialPrintType? PrintType { get; set; }
    public int? PrinterId { get; set; }
    public bool? PrintBarcodeFooterText { get; set; }
    public string ReportTemplateName { get; set; }
  }
}

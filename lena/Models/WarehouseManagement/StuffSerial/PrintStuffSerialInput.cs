using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSerial
{
  public class PrintStuffSerialInput
  {
    public int StuffId { get; set; }
    public string Serial { get; set; }
    public SerialPrintType SerialPrintType { get; set; }
    public int PrinterId { get; set; }
    public string ReportName { get; set; }
    public bool PrintBarcodeFooterText { get; set; }
  }
}

using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSerial
{
  public class PrintStuffSerialsInput
  {
    public SerialPrintType SerialPrintType { get; set; }
    public string ReportTemplateName { get; set; }
    public int PrinterId { get; set; }
    public string BillOfMaterialVersion { get; set; }
    public bool PrintBarcodeFooterText { get; set; }
    public bool PrintVersion { get; set; }
    public bool IsReprint { get; set; }
    public StuffSerialResult[] stuffSerialsList { get; set; }
  }
}

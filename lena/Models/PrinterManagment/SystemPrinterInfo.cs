using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Printers
{
  public class SystemPrinterInfo
  {
    public string Name { get; set; }
    public string SharedName { get; set; }
    public PrinterStatus Status { get; set; }
    public bool IsDefault { get; set; }
    public bool IsNetworkPrinter { get; set; }
  }
}

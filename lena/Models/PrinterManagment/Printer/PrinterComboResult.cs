using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Printers
{
  public class PrinterComboResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Manufacture { get; set; }
    public string Model { get; set; }
    public string Location { get; set; }
    public bool IsColored { get; set; }
    public bool IsSupportTemplatePrint { get; set; }
    public PrinterType PrinterType { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

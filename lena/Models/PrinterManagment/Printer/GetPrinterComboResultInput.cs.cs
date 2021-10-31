using lena.Domains.Enums;
namespace lena.Models.PrinterManagment.Printer
{
  public partial class GetPrinterComboResultInput
  {
    public int[] PrinterTypes { get; set; }
    public bool? HasLanPort { get; set; }
    public bool? IsColored { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsSupportTemplatePrint { get; set; }
  }
}

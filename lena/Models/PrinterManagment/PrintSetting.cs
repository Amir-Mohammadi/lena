using lena.Domains.Enums;
namespace lena.Models.PrinterManagment
{
  public enum PrintOrientation
  {
    Portrait,
    Landscape,
  }
  public class PrintSetting
  {
    public PrintOrientation Orientation { get; set; }
    public int NumberOfCopies { get; set; }
    public string PaperSize { get; set; }
  }
}

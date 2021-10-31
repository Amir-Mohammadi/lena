using lena.Domains.Enums;
namespace lena.Models.Reports.ReportPrintSettings
{
  public class SaveReportPrintSettingInput
  {
    public string ReportName { get; set; }
    public int PrinterId { get; set; }
    public int NumberOfCopies { get; set; }
    public bool ShowPreview { get; set; }
    public bool ShowPrintDialog { get; set; }
  }
}

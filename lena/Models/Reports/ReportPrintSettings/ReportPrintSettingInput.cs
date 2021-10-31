using lena.Domains.Enums;
namespace lena.Models.Reports.ReportPrintSettings
{
  public class ReportPrintSettingInput
  {
    public int ReportId { get; set; }
    public string ReportName { get; set; }
    public string ReportApiUrl { get; set; }
    public object ApiParams { get; set; }
    public int PrinterId { get; set; }
    public int NumberOfCopies { get; set; } = 1;
    public int PageRangeStart { get; set; } = 1;
    public int PageRangeEnd { get; set; } = 1;
    public bool ShowPreview { get; set; }
    public bool ShowPrintSetting { get; set; }
    public KeyValueInput[] ReportParams { get; set; }
  }
}

using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Reports
{
  public class GetReportExportInput
  {
    public string ReportName { get; set; }
    public object ApiParams { get; set; }
    public int ReportVersion { get; set; }
    public string ApiUrl { get; set; }
    public int UserId { get; set; }
    public StimulExportFormat ExportFormat { get; set; }
    public KeyValueInput[] ReportParams { get; set; }
  }

  public class ExportFileTypeInput
  {
    public int ExportFormat { get; set; }
    public string ReportFileName { get; set; }
  }
}

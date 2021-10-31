using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Reports
{
  public class AddReportInput
  {
    public string ReportName { get; set; }
    public string ReportContent { get; set; }
    public string ApiUrl { get; set; }

    public bool IsPublished { get; set; }
    public string Description { get; set; }
    public string CultureName { get; set; }
    public bool IsForExport { get; set; }
    public bool IsBarcodeTemplate { get; set; }
    public StimulExportFormat? ExportFormat { get; set; }
  }
}

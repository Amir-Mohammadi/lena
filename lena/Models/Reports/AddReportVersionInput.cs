using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Reports
{
  public class AddReportVersionInput
  {
    public int ReportId { get; set; }
    public string ApiUrl { get; set; }
    public string ReportContent { get; set; }
    public bool IsPublished { get; set; }
    public string CultureName { get; set; }
    public bool IsForExport { get; set; }
    public StimulExportFormat? ExportFoamt { get; set; }
    public bool IsBarcodeTemplate { get; set; }

  }


}

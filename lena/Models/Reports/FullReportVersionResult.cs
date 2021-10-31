using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Reports
{
  public class FullReportVersionResult
  {
    public int Id { get; set; }
    public string ApiUrl { get; set; }
    public string ReportContent { get; set; }
    public bool IsPublished { get; set; }
    public int CreatorUserId { get; set; }
    public string CreatorUserName { get; set; }
    public int ReportId { get; set; }
    public string ReportName { get; set; }
    public string CultureName { get; set; }
    public System.DateTime CreationTime { get; set; }
    public string CreatorEmployeeFullName { get; set; }
    public bool IsForExport { get; set; }
    public StimulExportFormat? ExportFormat { get; set; }
    public bool IsBarcodeTemplate { get; set; }
    public byte[] RowVersion { get; set; }
    public byte[] ReportRowVersion { get; set; }
    public string Description { get; set; }
  }
}

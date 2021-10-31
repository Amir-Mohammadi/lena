using lena.Domains.Enums;
namespace lena.Models.Reports
{
  public class EditReportInput : AddReportInput
  {
    public int ReportId { get; set; }
    public int ReportVersionId { get; set; }
    public bool IsNewVersion { get; set; }
    public byte[] ReportRowVersion { get; set; }
    public byte[] ReportVersionRowVersion { get; set; }
  }
}

using lena.Domains.Enums;
namespace lena.Models.Reports.ReportPrintSettings
{
  public class ReportPrintSettingResult
  {
    public int? Id { get; set; }
    public int? UserId { get; set; }
    public string UserName { get; set; }
    public string EmployeeFullName { get; set; }
    public int? PrinterId { get; set; }
    public int? ReportId { get; set; }
    public int? ReportVersion { get; set; }
    public int? ReportVersionId { get; set; }
    public string ApiUrl { get; set; }
    public string PrinterName { get; set; }
    public string ReportName { get; set; }
    public bool? ShowPreview { get; set; }
    public bool? ShowPrintDialog { get; set; }
    public int? NumberOfCopies { get; set; }
    public System.DateTime? CreationTime { get; set; }
    public int? CreatorId { get; set; }
    public string CreatorUserName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

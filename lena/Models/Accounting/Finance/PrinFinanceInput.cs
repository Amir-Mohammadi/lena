using lena.Domains.Enums;
namespace lena.Models.Accounting.Finance
{
  public class PrintFinanceInput
  {
    public int FinanceId { get; set; }
    public string ReportName { get; set; }
    public string AttachmentReportName { get; set; }
    public bool HasAttachmentReport { get; set; }
    public int PrinterId { get; set; }
  }
}

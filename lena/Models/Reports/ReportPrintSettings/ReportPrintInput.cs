using lena.Domains.Enums;
namespace lena.Models.Reports.ReportPrintSettings
{
  public class ReportPrintInput
  {
    public string ReportName { get; set; }
    public string ApiUrl { get; set; }
    public object ApiParams { get; set; }
    public int PrinterId { get; set; }
    public int NumberOfCopies { get; set; }
    public KeyValueInput[] ReportParams { get; set; }
  }
}

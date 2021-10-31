using lena.Domains.Enums;
namespace lena.Models.Reports
{
  public class GetReportRenderInput
  {
    public string ReportName { get; set; }
    public object ApiParams { get; set; }
    public string ApiUrl { get; set; }
    public KeyValueInput[] ReportParams { get; set; }
  }
}

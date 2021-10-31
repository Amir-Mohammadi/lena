using lena.Domains.Enums;
namespace lena.Models.Reports
{
  public class GenerateReportContentInput
  {
    public string ReportName { get; set; }
    public string ApiUrl { get; set; }
    public string ApiParams { get; set; }
    public KeyValueInput[] ReportParams { get; set; }
  }


}

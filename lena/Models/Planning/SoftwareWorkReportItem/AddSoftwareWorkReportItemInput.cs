using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.SoftwareWorkReportItem
{
  public class AddSoftwareWorkReportItemInput
  {
    public int Spent { get; set; }
    public int Estimated { get; set; }
    public string Issue { get; set; }
    public RestTimeStatus? RestTimeIssue { get; set; }
  }
}

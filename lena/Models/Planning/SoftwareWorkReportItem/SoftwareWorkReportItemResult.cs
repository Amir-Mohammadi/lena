using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.SoftwareWorkReportItem
{
  public class SoftwareWorkReportItemResult
  {
    public int Id { get; set; }
    public int Spent { get; set; }
    public int Estimated { get; set; }
    public string Issue { get; set; }
    public byte[] RowVersion { get; set; }
    public RestTimeStatus? RestTimeIssue { get; set; }
  }
}

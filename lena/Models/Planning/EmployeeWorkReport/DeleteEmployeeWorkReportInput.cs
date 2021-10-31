using lena.Domains.Enums;
namespace lena.Models.Planning.UserWorkReport
{
  public class DeleteEmployeeWorkReportInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

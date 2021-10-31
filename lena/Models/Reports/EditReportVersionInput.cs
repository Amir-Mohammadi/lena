using lena.Domains.Enums;
namespace lena.Models.Reports
{
  public class EditReportVersionInput : AddReportVersionInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }

  }
}

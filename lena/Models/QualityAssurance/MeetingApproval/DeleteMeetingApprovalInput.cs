using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.MeetingApproval
{
  public class DeleteMeetingApprovalInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

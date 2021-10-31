using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.MeetingParticipant
{
  public class DeleteMeetingParticipantInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

using lena.Domains.Enums;
namespace lena.Models
{
  public class EditMeetingParticipantInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int MinutesMeetingId { get; set; }
    public int? ParticipantEmployeeId { get; set; }
    public string GuestParticipantName { get; set; }
  }
}
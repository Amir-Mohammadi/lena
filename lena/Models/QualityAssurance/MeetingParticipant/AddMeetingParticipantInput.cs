using lena.Domains.Enums;
namespace lena.Models
{
  public class AddMeetingParticipantInput
  {
    public int MinutesMeetingId { get; set; }
    public int? ParticipantEmployeeId { get; set; }
    public string GuestParticipantName { get; set; }
  }
}

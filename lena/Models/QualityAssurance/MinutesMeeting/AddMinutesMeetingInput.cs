using System;
using lena.Domains.Enums;
namespace lena.Models
{
  public class AddMinutesMeetingInput
  {
    public AddMeetingApprovalInput[] MeetingApprovalsInput { get; set; }
    public AddMeetingParticipantInput[] MeetingParticipantsInput { get; set; }
    public DateTime? MeetingDateTime { get; set; }
    public string Place { get; set; }
    public string Agenda { get; set; }
    public int SecretaryUserId { get; set; }
    public int? BossUserId { get; set; }
    public bool IsConfidential { get; set; }
  }
}

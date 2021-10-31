using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class EditMinutesMeetingInput
  {
    public AddMeetingApprovalInput[] AddMeetingApprovalsInput { get; set; }
    public EditMeetingApprovalInput[] EditMeetingApprovalsInput { get; set; }
    public int[] MeetingApprovalsDeleteIds { get; set; }
    public AddMeetingParticipantInput[] AddMeetingParticipantsInput { get; set; }
    public EditMeetingParticipantInput[] EditMeetingParticipantsInput { get; set; }
    public int[] MeetingParticipantsDeleteIds { get; set; }
    public int Id { get; set; }
    public DateTime? MeetingDateTime { get; set; }
    public string Place { get; set; }
    public string Agenda { get; set; }
    public int? SecretaryUserId { get; set; }
    public int? BossUserId { get; set; }
    public bool? IsConfidential { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
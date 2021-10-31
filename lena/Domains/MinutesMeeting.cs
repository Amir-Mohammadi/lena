using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class MinutesMeeting : IEntity
  {
    protected internal MinutesMeeting()
    {
      this.MeetingParticipants = new HashSet<MeetingParticipant>();
      this.MeetingAprovals = new HashSet<MeetingApproval>();
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int RegistrantUserId { get; set; }
    public DateTime? RegistrationDateTime { get; set; }
    public DateTime? MeetingDateTime { get; set; }
    public string Place { get; set; }
    public string Agenda { get; set; }
    public int SecretaryUserId { get; set; }
    public int? BossUserId { get; set; }
    public bool IsConfidential { get; set; }
    public virtual ICollection<MeetingParticipant> MeetingParticipants { get; set; }
    public virtual ICollection<MeetingApproval> MeetingAprovals { get; set; }
    public virtual User RegistrantUser { get; set; }
    public virtual User SecretaryUser { get; set; }
    public virtual User BossUser { get; set; }
  }
}

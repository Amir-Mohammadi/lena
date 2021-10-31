using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class MeetingParticipant : IEntity
  {
    protected internal MeetingParticipant()
    {
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int MinutesMeetingId { get; set; }
    public int? ParticipantEmployeeId { get; set; }
    public string GuestParticipantName { get; set; }
    public virtual MinutesMeeting MinutesMeeting { get; set; }
    public virtual Employee ParticipantEmployee { get; set; }
  }
}

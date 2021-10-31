using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class MeetingApproval : IEntity
  {
    protected internal MeetingApproval()
    {
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int MinutesMeetingId { get; set; }
    public string Title { get; set; }
    public int? OperatorUserId { get; set; }
    public string GuestOperatorName { get; set; }
    public DateTime? ActionDateTime { get; set; }
    public DateTime? ActualDateTime { get; set; }
    public MeetingApprovalStatus? Status { get; set; }
    public string Description { get; set; }
    public Guid? DocumentId { get; set; }
    public Nullable<short> OperatorDepartmentId { get; set; }
    public virtual MinutesMeeting MinutesMeeting { get; set; }
    public virtual User OperatorUser { get; set; }
    public virtual Department Department { get; set; }
  }
}

using lena.Domains.Enums;
using System;
using lena.Domains.Enums;
namespace lena.Models
{
  public class AddMeetingApprovalInput
  {
    public int MinutesMeetingId { get; set; }
    public string Title { get; set; }
    public int? OperatorUserId { get; set; }
    public string GuestOperatorName { get; set; }
    public DateTime? ActionDateTime { get; set; }
    public DateTime? ActualDateTime { get; set; }
    public string Description { get; set; }
    public short? OperatorDepartmentId { get; set; }
    public MeetingApprovalStatus? Status { get; set; }
  }
}

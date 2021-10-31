using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class EditMeetingApprovalInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public int? OperatorUserId { get; set; }
    public string GuestOperatorName { get; set; }
    public DateTime? ActionDateTime { get; set; }
    public DateTime? ActualDateTime { get; set; }
    public string Description { get; set; }
    public string FileKey { get; set; }
    public MeetingApprovalStatus? Status { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
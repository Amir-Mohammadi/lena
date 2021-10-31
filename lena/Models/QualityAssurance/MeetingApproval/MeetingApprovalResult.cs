using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class MeetingApprovalResult
  {
    public int Id { get; set; }
    public int MinutesMeetingId { get; set; }
    public string Title { get; set; }
    public int? OperatorUserId { get; set; }
    public string OperatorUserFullName { get; set; }
    public string DepartmentName { get; set; }
    public string GuestOperatorName { get; set; }
    public DateTime? ActionDateTime { get; set; }
    public DateTime? ActualDateTime { get; set; }
    public MeetingApprovalStatus? Status { get; set; }
    public string Description { get; set; }
    public Guid? DocumentId { get; set; }
    public bool HasUploadedDocument { get; set; }
    public short? OperatorDepartmentId { get; set; }
    public string OperatorDepartmentFullName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

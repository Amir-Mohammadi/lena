using System;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models
{
  public class MinutesMeetingResult
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int RegistrantUserId { get; set; }
    public string RegistrantUserFullName { get; set; }
    public DateTime? RegistrationDateTime { get; set; }
    public DateTime? MeetingDateTime { get; set; }
    public string Place { get; set; }
    public string Agenda { get; set; }
    public int SecretaryUserId { get; set; }
    public string SecretaryUserFullName { get; set; }
    public int? BossUserId { get; set; }
    public string BossUserFullName { get; set; }
    public bool IsConfidential { get; set; }
    public short? DepartmentId { get; set; }
    public string DepartmentFullName { get; set; }
    public short? operatorDepartmentId { get; set; }
    public string operatorDepartmentFullName { get; set; }
    public IQueryable<MeetingParticipantResult> MeetingParticipants { get; set; }
    public IQueryable<MeetingApprovalResult> MeetingApprovals { get; set; }
  }
}
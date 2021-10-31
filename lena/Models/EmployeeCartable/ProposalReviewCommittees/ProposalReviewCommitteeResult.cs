using System;

using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.ProposalReviewCommittees
{
  public class ProposalReviewCommitteeResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; }
    public bool IsConfirmed { get; set; }
    public string ReviewResult { get; set; }
    public int ResponsibleUserId { get; set; }
    public string ResponsibleUserFullName { get; set; }
    public int ProposalId { get; set; }
    public DateTime ReviewDateTime { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

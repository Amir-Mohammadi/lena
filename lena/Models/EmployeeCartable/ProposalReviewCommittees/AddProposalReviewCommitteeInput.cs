using System;

using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.ProposalReviewCommittees
{
  public class AddProposalReviewCommitteeInput
  {
    public bool IsConfirmed { get; set; }
    public string ReviewResult { get; set; }
    public DateTime ReviewDateTime { get; set; }
    public int ResponsibleUserId { get; set; }
    public int ProposalId { get; set; }
  }
}

using System;

using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.ProposalQaReviews
{
  public class AddProposalQaReviewInput
  {
    public string ReviewResult { get; set; }
    public int ResponsibleUserId { get; set; }
    public int ProposalId { get; set; }
    public DateTime ReviewDateTime { get; set; }
  }
}

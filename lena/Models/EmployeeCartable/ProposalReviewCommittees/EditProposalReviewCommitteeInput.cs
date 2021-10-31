using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.ProposalReviewCommittees
{
  public class EditProposalReviewCommitteeInput : AddProposalReviewCommitteeInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

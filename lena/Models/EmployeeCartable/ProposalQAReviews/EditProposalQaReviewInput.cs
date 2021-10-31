using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.ProposalQaReviews
{
  public class EditProposalQaReviewInput : AddProposalQaReviewInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

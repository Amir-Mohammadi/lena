using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.Proposals
{
  public class AddProposalInput
  {
    public string CurrentSituationDescription { get; set; }
    public string ProposalDescription { get; set; }
    public string ProposalEffect { get; set; }
    public int ProposalTypeId { get; set; }
    public ProposalStatus Status { get; set; }
    public bool IsIncognitoUser { get; set; }
  }
}

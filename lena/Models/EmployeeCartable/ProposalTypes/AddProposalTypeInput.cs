using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.ProposalTypes
{
  public class AddProposalTypeInput
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
  }
}

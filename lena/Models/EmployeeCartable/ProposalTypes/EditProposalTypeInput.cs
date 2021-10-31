using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.ProposalTypes
{
  public class EditProposalTypeInput : AddProposalTypeInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

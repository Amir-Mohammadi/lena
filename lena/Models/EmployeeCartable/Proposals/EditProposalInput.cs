using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.Proposals
{
  public class EditProposalInput : AddProposalInput
  {
    public int Id { get; set; }
    public bool IsOpen { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

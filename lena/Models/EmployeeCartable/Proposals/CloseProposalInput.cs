using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.Proposals
{
  public class CloseProposalInput
  {
    public int Id { get; set; }
    public bool IsEffective { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

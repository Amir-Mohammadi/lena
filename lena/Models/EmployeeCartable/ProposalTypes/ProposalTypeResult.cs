using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.ProposalTypes
{
  public class ProposalTypeResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
